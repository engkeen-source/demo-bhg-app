using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BOLib;
using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using TAUtil;
using System.Drawing;

namespace WinUI
{
    public partial class frmAssemblyEntry : Form
    {
        #region Local Variables

        private Document objDoc = null;                 //Caller ObjDoc
        private UltraGrid DocDetgrd = null;             //Caller grid
        private DataTable DocDetdt = null;              //Caller grid datasource all rows
        private DataTable dtAssParent = null;           //datatable for Assembly parent row only

        string ContextMenuSetting = string.Empty;
        private bool DataEntryMode = true;
        private int priceDecPlace = GVar.RndDecs.Prcpt;

        private int CallerDocKey = 0;                   //Caller current DocKey
        private int CallerDocItmKey = 0;                //Caller current row DocItmKey
        private int ParentItmKey = 0;
        private decimal ParentItmQty = 0;
        private decimal ParentItmPrice = 0;
        private decimal ParentItmSN = 0;
        private int ParentItmDeptKey = 0;
        private int ParentItmTranGrpKey = 0;
        private DateTime ParentItmReqDate;
        private DateTime ParentItmPrmDate;
        private string ParentItmMark = string.Empty;
        private bool ParentItmHide = false;
        private int ParentJobKey = 0;

        private int defaultLocKey = 0;
        private bool FirstTimeEntry = false;

        private int docCurrKey = 1;
        private decimal docCurrRate = 1;
        private int docPriceType = 0;
        private int docConKey = 0;

        private bool formClose = false;
        private string ErrorMsg = "";

        private int QtySetPt = 18;
        private decimal ItmQtySetPtVal = 0.0M;
        private decimal ItmAmtSetPtVal = 0.0M;

        #endregion

        //Initialize
        public frmAssemblyEntry()
        {
            InitializeComponent();
        }
        public frmAssemblyEntry(Document vobjDoc, UltraGrid tagrdDetItms, int vCallerDocItmKey, bool vDataEntryMode)
        {
            //use for opening the assembly form in editing, this allows the user to 
            InitializeComponent();
            objDoc = vobjDoc;
            CallerDocItmKey = vCallerDocItmKey;
            DataEntryMode = vDataEntryMode;
            tagrdDetItms.ActiveRow.Update();
            DocDetgrd = tagrdDetItms;
            DocDetdt = tagrdDetItms.DataSource as DataTable;
        }//Completed

        //Form Events
        private void frmAssemblyEntry_Load(object sender, EventArgs e)
        {
            try
            {
                //Initialise Variables
                priceDecPlace = SysOptionUtility.GetInt(GVar.SystemOption.OpID.PriceDecPlace);
                defaultLocKey = SysOptionUtility.GetInt("DefaultInventoryLocation");

                //Prepare Form's Grid and Controls format
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.Combos_Fill(this, 0);

                //Prepare Form Header and Detail Data
                HeaderData_Get();
                DetailData_DocDetAssChildGet();

                if (DataEntryMode)
                {
                    //Format Form for DataEntry
                    DatailData_MSTItmDetAss_Get();
                    tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmLocKey"].DefaultCellValue = (int)LocKey.Value;
                    LockGridCol();
                }
                else
                {
                    //Format Form As readonly
                    ItmDes.Enabled = false;
                    LocKey.Enabled = false;
                    ItmQty.Enabled = false;
                    ItmPrice.Enabled = false;
                    btnOK.Visible = false;
                    tagrdItmGroupSelected.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                    tagrdItmGroupSelected.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                    tagrdItmGroupSelected.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

                    //To prevent button_Click from firing
                    foreach (UltraGridColumn col in tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns)
                    {
                        col.CellActivation = Activation.ActivateOnly;
                    }                   

                    CalculateTotal();
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
        private void frmAssemblyEntry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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
        private void frmAssemblyEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* Validate data when users click form-close button instead of OK-button or Closed-button - added by YST */
            formClose = true;
            btnOK_Click(sender, e);
            formClose = false;
            e.Cancel = ErrorMsg == "" ? false : true;
        }

        public void Reload(int? docItmKey)
        {
            try
            {
                CallerDocItmKey = (int)docItmKey;
                HeaderData_Get();
                DetailData_DocDetAssChildGet();
                CalculateTotal();
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

        public void LoadJobChildItems()
        {
            frmAssemblyEntry_Load(null, null);
            btnOK_Click(null, null);
        }
        //Control Events
        private void Value_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                ParentItmQty = GFunc.RndC(ItmQty.Value, GVar.RndDecs.Qtypt);
                ParentItmPrice = GFunc.RndC(ItmPrice.Value, GVar.RndDecs.Prcpt);
                decimal amt = GFunc.RndC(ParentItmQty * ParentItmPrice, GVar.RndDecs.Amtpt);

                this.ItmQty.SetValueTrigger(ParentItmQty, false);
                this.ItmPrice.SetValueTrigger(ParentItmPrice, false);
                this.Amount.SetValueTrigger(amt, false);

                DataTable dt = tagrdItmGroupSelected.DataSource as DataTable;

                foreach (DataRow dr in dt.Rows)
                {
                    CalculateAmount(dr);
                }

                CalculateTotal();
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

        //Grid Common Events  
        private void tagrdItmGroupSelected_BeforeRowActivate(object sender, RowEventArgs e)
        {

        }//Completed
        private void tagrdItmGroupSelected_AfterRowInsert(object sender, RowEventArgs e)
        {
            try
            {
                if ((tagrdItmGroupSelected.DataSource as DataTable).Rows.Count == 0)
                    e.Row.Cells["ItmDetSN"].Value = 1;
                else
                    e.Row.Cells["ItmDetSN"].Value = (tagrdItmGroupSelected.DataSource as DataTable).AsEnumerable().Max(k => k.Field<decimal>("ItmDetSN")) + 1;
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
        private void tagrdItmGroupSelected_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                GridLock_Set();
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
        private void tagrdItmGroupSelected_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                //Resequence SN
                DataTable dt = tagrdItmGroupSelected.DataSource as DataTable;
                dt.DefaultView.Sort = "ItmDetSN Asc";
                decimal SN = 1;
                foreach (DataRowView dr in dt.DefaultView)
                {
                    dr["ItmDetSN"] = SN;
                    SN++;
                }
                dt.AcceptChanges();
                CalculateTotal();
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
        private void tagrdItmGroupSelected_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
                CalculateTotal();
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
        private void tagrdItmGroupSelected_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            try
            {
                e.DisplayPromptMsg = false;
                if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.WarnDeleteDocumentDetail))
                {
                    // Ask Confirmation for Delete
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Record.ConfirmDeleteRecord + "%" + tagrdItmGroupSelected.ActiveRow.Cells["ItmDetSN"].Value,
                                          GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton2, GEnum.MsgBoxButton.Delete,
                                          GEnum.MsgBoxButton.Dont_Delete,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    // Check Delete
                    if (btnSelect != GEnum.MsgBoxButton.Delete)
                        e.Cancel = true;
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
        private void tagrdItmGroupSelected_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            UINotifierEventArgs ep = new UINotifierEventArgs(new Hashtable());
            UltraGridRow dr = tagrdItmGroupSelected.ActiveRow;
            bool isValidate = true;


            try
            {
                BOLib.BaseUtility.Validation(dr.Cells["DocKey"].Value, "DocKey", "DocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["DocItmKey"].Value, "DocItmKey", "DocItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["LineType"].Value, "LineType", "LineType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["LineLinkKey"].Value, "LineLinkKey", "LineLinkKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmSN"].Value, "ItmSN", "ItmSN", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmKey"].Value, "ItmKey", "ItmKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmKeySelect"].Value, "ItmKeySelect", "ItmKeySelect", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmType"].Value, "ItmType", "ItmType", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmID"].Value, "ItmID", "ItmID", GEnum.DataType.String, GEnum.Require.Yes, null, GEnum.CompareOperator.LessThanEqual, 255, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmDes"].Value, "ItmDes", "ItmDes", GEnum.DataType.String, GEnum.Require.Yes, 8000, null, null, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmDeptKey"].Value, "ItmDeptKey", "ItmDeptKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmTranGrpKey"].Value, "ItmTranGrpKey", "ItmTranGrpKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmVendorCurrKey"].Value, "ItmVendorCurrKey", "ItmVendorCurrKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmVendorCurrRate"].Value, "ItmVendorCurrRate", "ItmVendorCurrRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmVendorPrice"].Value, "ItmVendorPrice", "ItmVendorPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmVendorPriceRatio"].Value, "ItmVendorPriceRatio", "ItmVendorPriceRatio", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.Equal, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ItmVendorPriceLock"].Value, "ItmVendorPriceLock", "ItmVendorPriceLock", GEnum.DataType.Boolean, GEnum.Require.Yes, null, GEnum.CompareOperator.Equal, 1, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["Selected"].Value, "Selected", "Selected", GEnum.DataType.Boolean, GEnum.Require.Yes, null, null, null, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ARQODK"].Value, "ARQODK", "ARQODK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ARQODItm"].Value, "ARQODItm", "ARQODItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ARSODK"].Value, "ARSODK", "ARSODK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ARSODItm"].Value, "ARSODItm", "ARSODItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ARDODK"].Value, "ARDODK", "ARDODK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["ARDODItm"].Value, "ARDODItm", "ARDODItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["CSCPSDK"].Value, "CSCPSDK", "CSCPSDK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["CSCPSDItm"].Value, "CSCPSDItm", "CSCPSDItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["CSCSIDK"].Value, "CSCSIDK", "CSCSIDK", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                BOLib.BaseUtility.Validation(dr.Cells["CSCSIDItm"].Value, "CSCSIDItm", "CSCSIDItm", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);

                switch ((int)GFunc.GetINTypeGroup(dr.Cells["ItmType"].Value))
                {
                    case (int)GEnum.ItemType.Stock:
                        BOLib.BaseUtility.Validation(dr.Cells["ItmLocKey"].Value, "ItmLocKey", "ItmLocKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmUOMKey"].Value, "ItmUOMKey", "ItmUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmIGrpQtySet"].Value, "ItmIGrpQtySet", "ItmIGrpQtySet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmQty"].Value, "ItmQty", "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmConRate"].Value, "ItmConRate", "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmLatestCostF"].Value, "ItmLatestCostF", "ItmLatestCostF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmLatestCostH"].Value, "ItmLatestCostH", "ItmLatestCostH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmListPrice"].Value, "ItmListPrice", "ItmListPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmPriceAfter"].Value, "ItmPriceAfter", "ItmPriceAfter", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmIGrpAmtSet"].Value, "ItmIGrpAmtSet", "ItmIGrpAmtSet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtShw"].Value, "ItmAmtShw", "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtF"].Value, "ItmAmtF", "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtH"].Value, "ItmAmtH", "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        break;

                    case (int)GEnum.ItemType.Non_Stock:
                        BOLib.BaseUtility.Validation(dr.Cells["ItmUOMKey"].Value, "ItmUOMKey", "ItmUOMKey", GEnum.DataType.Integer, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmIGrpQtySet"].Value, "ItmIGrpQtySet", "ItmIGrpQtySet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmQty"].Value, "ItmQty", "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmConRate"].Value, "ItmConRate", "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmLatestCostF"].Value, "ItmLatestCostF", "ItmLatestCostF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmLatestCostH"].Value, "ItmLatestCostH", "ItmLatestCostH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmListPrice"].Value, "ItmListPrice", "ItmListPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmPriceAfter"].Value, "ItmPriceAfter", "ItmPriceAfter", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmIGrpAmtSet"].Value, "ItmIGrpAmtSet", "ItmIGrpAmtSet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtShw"].Value, "ItmAmtShw", "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtF"].Value, "ItmAmtF", "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtH"].Value, "ItmAmtH", "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        break;

                    case (int)GEnum.ItemType.Charges:
                        BOLib.BaseUtility.Validation(dr.Cells["ItmIGrpQtySet"].Value, "ItmIGrpQtySet", "ItmIGrpQtySet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmQty"].Value, "ItmQty", "ItmQty", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmConRate"].Value, "ItmConRate", "ItmConRate", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThan, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmLatestCostF"].Value, "ItmLatestCostF", "ItmLatestCostF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmLatestCostH"].Value, "ItmLatestCostH", "ItmLatestCostH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmListPrice"].Value, "ItmListPrice", "ItmListPrice", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmPriceAfter"].Value, "ItmPriceAfter", "ItmPriceAfter", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmIGrpAmtSet"].Value, "ItmIGrpAmtSet", "ItmIGrpAmtSet", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtShw"].Value, "ItmAmtShw", "ItmAmtShw", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtF"].Value, "ItmAmtF", "ItmAmtF", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        BOLib.BaseUtility.Validation(dr.Cells["ItmAmtH"].Value, "ItmAmtH", "ItmAmtH", GEnum.DataType.Decimel, GEnum.Require.Yes, null, GEnum.CompareOperator.GreatherThanEqual, 0, null, null, ref isValidate, false, ep);
                        break;
                }

                //Set RowError Text
                if (isValidate == false)
                {
                    ((DataRowView)(dr.ListObject)).Row.RowError = GFunc.PropertyMessage_Merge(ep);
                    e.Cancel = true;
                }
                else
                    ((DataRowView)(dr.ListObject)).Row.RowError = string.Empty;



            }
            catch (TAException tex)
            {
                e.Cancel = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void tagrdItmGroupSelected_ClickCellButton(object sender, CellEventArgs e)
        {

            try
            {
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                DataRow NewRow = null;

                if (tagrdItmGroupSelected.ActiveCell == null)
                    return;

                switch (e.Cell.Column.Key.ToLower())
                {
                    case "itmid":
                        DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, tagrdItmGroupSelected.ActiveCell.Text, "MSTItmAss_id", (int)GEnum.PopupType.ItmID, ref key, ref id, ref des);
                        if (key > 0)
                        {
                            SetItmInfor(key, id, des);
                            NewRow = ((DataRowView)(sender as UltraGrid).ActiveRow.ListObject).Row;
                            CalculateAmount(NewRow);
                        }
                        break;

                    case "itmdes":
                        DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, tagrdItmGroupSelected.ActiveCell.Text, "MSTItmAss_des", (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des);
                        if (key > 0)
                        {
                            SetItmInfor(key, id, des);
                            NewRow = ((DataRowView)(sender as UltraGrid).ActiveRow.ListObject).Row;
                            CalculateAmount(NewRow);
                        }
                        break;

                    case "itmvendorkey":
                        DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, tagrdItmGroupSelected.ActiveCell.Text, "MSTConPurchase_id", (int)GEnum.PopupType.VendID, ref key, ref id, ref des);
                        if (key > 0)
                            SetVendInfor(key, id, des);
                        break;

                    case "itmvendornm":
                        DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, tagrdItmGroupSelected.ActiveCell.Text, "MSTConPurchase_des", (int)GEnum.PopupType.VendNm, ref key, ref id, ref des);
                        if (key > 0)
                            SetVendInfor(key, id, des);
                        break;
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
        private void tagrdItmGroupSelected_CustomCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                UltraGridCell grdCell = tagrdItmGroupSelected.ActiveCell;
                int key = 0;
                string id = string.Empty;
                string des = string.Empty;
                string listSettingID = string.Empty;
                DataRow NewRow = null;
                bool runCalculation = false;

                if (grdCell != null)
                {
                    switch (grdCell.Column.Key.ToLower())
                    {
                        case "itmid":
                            runCalculation = true;
                            
                            listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                            key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemID, listSettingID, GFunc.NEStr(grdCell.Value, string.Empty), 0, ref id, ref des, true);
                            if (key == 0)
                            {
                                if (DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, GFunc.NEStr(grdCell.Value, string.Empty), listSettingID, (int)GEnum.PopupType.ItmID, ref key, ref id, ref des))
                                    e.Cancel = !SetItmInfor(key, id, des);
                                else
                                    e.Cancel = true;
                            }
                            else
                                e.Cancel = !SetItmInfor(key, id, des);

                            /* added by YST on 2023/04/24 */
                            if (e.Cancel == false && grdCell.Row.Cells.Exists("SlowMovingGoods") && GFunc.NEBool(grdCell.Row.Cells["SlowMovingGoods"].Value, false) == true)
                            {
                                tagrdItmGroupSelected.ActiveRow.Appearance.BackColor = Color.LightGreen;
                            }
                            break;
                        case "itmdes":
                            runCalculation = true;
                            if (GFunc.NEInt(grdCell.Row.Cells["ItmKey"].Value, 0) == 0)
                            {
                                listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                                key = GFunc.ItmRecord_GetKey(GEnum.RecAccessType.ItemDes, listSettingID, GFunc.NEStr(grdCell.Value, string.Empty), 0, ref id, ref des, true);
                                if (key == 0)
                                {
                                    if (DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, GFunc.NEStr(grdCell.Value, string.Empty), listSettingID, (int)GEnum.PopupType.ItmDes, ref key, ref id, ref des) == false)
                                        e.Cancel = !SetItmInfor(key, id, des);
                                    else
                                        e.Cancel = true;
                                }
                                else
                                    e.Cancel = !SetItmInfor(key, id, des);
                            }
                            break;

                        case "itmlatestcostf":
                            decimal v = GFunc.RndC(grdCell.Value, GVar.RndDecs.Prcpt);
                            grdCell.Value = v;
                            tagrdItmGroupSelected.ActiveRow.Cells["itmlatestcosth"].Value = GFunc.RndC(v * docCurrRate, GVar.RndDecs.Prcpt);
                            break;

                        case "itmlistprice":
                        case "itmvendorprice":
                            grdCell.Value = GFunc.RndC(grdCell.Value, GVar.RndDecs.Prcpt);
                            break;

                        case "itmvendorcurrrate":
                            decimal u = GFunc.RndC(grdCell.Value, GVar.RndDecs.Curpt);
                            grdCell.Value = u;
                            break;

                        case "itmuomkey":
                            int itmKey = GFunc.NEInt(grdCell.Row.Cells["ItmKey"].Value, 0);
                            int uomKey = GFunc.NEInt(grdCell.Row.Cells["ItmUOMKey"].Value, 0);
                            if (itmKey == 0 || uomKey == 0)
                                grdCell.Value = 1M;
                            else
                                grdCell.Value = DocComUtility.UOMConRate_Get(itmKey, uomKey);
                            break;

                        case "itmqty":
                        case "itmigrpqtyset":
                        case "itmpriceafter":
                        case "itmigrpqtylock":                        
                            runCalculation = true;
                            break;

                        case "itmvendorkey":
                            if (GFunc.IsNE(grdCell.Text))
                            {
                                SetVendInfor(0, "", "");
                            }
                            else
                            {
                                listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                                key = GFunc.ConRecord_GetKey(GEnum.RecAccessType.VendKey, listSettingID, GFunc.NEStr(grdCell.Text, string.Empty), ref id, ref des, true);
                                if (key == 0)
                                {
                                    if (DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, GFunc.NEStr(grdCell.Text, string.Empty), listSettingID, (int)GEnum.PopupType.VendID, ref key, ref id, ref des))
                                        e.Cancel = !SetVendInfor(key, id, des);
                                    else
                                        e.Cancel = true;
                                }
                                else
                                    e.Cancel = !SetVendInfor(key, id, des);
                            }
                            break;

                        case "itmvendornm":
                            if (GFunc.NEInt(grdCell.Row.Cells["ItmVendorKey"].Value, 0) == 0)
                            {
                                listSettingID = GlobalUI.ListSettingID_Get(ContextMenuSetting, e.Cell.Column.Key, ((Control)sender).Name);
                                key = GFunc.ConRecord_GetKey(GEnum.RecAccessType.VendNm, listSettingID, GFunc.NEStr(grdCell.Text, string.Empty), ref id, ref des, true);
                                if (key == 0)
                                {
                                    if (DocHDRUtil.EditorButton_Popup(objDoc, (int)objDoc.DocCodeKey, GFunc.NEStr(grdCell.Text, string.Empty), listSettingID, (int)GEnum.PopupType.VendNm, ref key, ref id, ref des))
                                        e.Cancel = !SetVendInfor(key, id, des);
                                    else
                                        e.Cancel = true;
                                }
                                else
                                    e.Cancel = !SetVendInfor(key, id, des);
                            }
                            break;
                    }

                    //Run Calculation
                    if (runCalculation && e.Cancel == false)
                    {
                        NewRow = ((DataRowView)(sender as UltraGrid).ActiveRow.ListObject).Row;
                        CalculateAmount(NewRow, grdCell);
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

        //Functions
        private void HeaderData_Get()
        {
            try
            {
                if (GFunc.IsNEZ(CallerDocItmKey))
                    return;

                IEnumerable<DataRow> dtAssParentFilter = (DocDetgrd.DataSource as DataTable).AsEnumerable().Where(r => r.Field<int>("DocItmKey") == CallerDocItmKey);

                if (dtAssParentFilter.Count() == 0)
                    return;

                CallerDocKey = GFunc.NEInt(dtAssParentFilter.ElementAt(0)["DocKey"], 0);
                ParentItmKey = GFunc.NEInt(dtAssParentFilter.ElementAt(0)["ItmKey"], 0);
                ParentItmQty = GFunc.NEDec(dtAssParentFilter.ElementAt(0)["ItmQty"], 0);
                ParentItmPrice = GFunc.NEDec(dtAssParentFilter.ElementAt(0)["ItmPriceAfter"], 0);
                ParentItmSN = GFunc.NEDec(dtAssParentFilter.ElementAt(0)["ItmSN"], 0);
                ParentItmDeptKey = GFunc.NEInt(dtAssParentFilter.ElementAt(0)["ItmDeptKey"], 0);
                ParentItmTranGrpKey = GFunc.NEInt(dtAssParentFilter.ElementAt(0)["ItmTranGrpKey"], 0);
                ParentItmMark = GFunc.NEStr(dtAssParentFilter.ElementAt(0)["ItmMark"], string.Empty);
                ParentItmHide = GFunc.NEBool(dtAssParentFilter.ElementAt(0)["ItmHide"], 0);

                switch ((int)objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                        ParentItmReqDate = GFunc.NEDateTime(dtAssParentFilter.ElementAt(0)["ItmReqDate"], DateTime.Now);
                        ParentItmPrmDate = GFunc.NEDateTime(dtAssParentFilter.ElementAt(0)["ItmPrmDate"], DateTime.Now);
                        ParentJobKey = GFunc.NEInt(dtAssParentFilter.ElementAt(0)["ItmJobKey"], 0);
                        break;
                }

                docCurrKey = GFunc.NEInt(GFunc.GetPropertyValue("DocCurrKey", objDoc), 1);
                docCurrRate = GFunc.NEDec(GFunc.GetPropertyValue("DocCurrRate", objDoc), 1);
                docPriceType = GFunc.NEInt(GFunc.GetPropertyValue("DocPriceType", objDoc), 0);
                docConKey = GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0);

                ItmID.Text = dtAssParentFilter.ElementAt(0)["ItmID"].ToString();
                ItmDes.SetValueTrigger(dtAssParentFilter.ElementAt(0)["ItmDes"].ToString(), false);
                ItmQty.SetValueTrigger(ParentItmQty, false);
                ItmPrice.SetValueTrigger(ParentItmPrice, false);
                Amount.SetValueTrigger(GFunc.RndC(ParentItmQty * ParentItmPrice, GVar.RndDecs.Amtpt), false);

                //Set default Location
                if (GFunc.IsNEZ(objDoc.DefLocKey) == false)
                    defaultLocKey = (int)objDoc.DefLocKey;

                dtAssParent = dtAssParentFilter.CopyToDataTable();
                this.LocKey.SetValueTrigger(defaultLocKey, false);
                this.tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmLocKey"].DefaultCellValue = LocKey.Value;
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
        private void DetailData_DocDetAssChildGet()
        {
            //Get Data from Caller Doc Detail Grid for Child
            try
            {
                if (GFunc.IsNEZ(CallerDocItmKey))
                    return;

                IEnumerable<DataRow> dtFilter = (DocDetgrd.DataSource as DataTable).Copy().AsEnumerable().Where(r => r.Field<int>("LineLinkKey") == CallerDocItmKey);
                if (dtFilter.Count() == 0)
                    FirstTimeEntry = true;

                DataTable dtGrid = tagrdItmGroupSelected.DataSource as DataTable;
                dtGrid.Rows.Clear();
                foreach (DataRow dr in dtFilter)
                {
                    if (CheckQtySetDecimalPt(GFunc.RndC(dr["ItmQty"], GVar.RndDecs.Qtypt), GFunc.RndC(dr["ItmPriceAfter"], GVar.RndDecs.Amtpt)))
                    {
                        dr["ItmIGrpQtySet"] = ItmQtySetPtVal;
                        dr["ItmIGrpAmtSet"] = ItmAmtSetPtVal;
                    }

                    DataRow drGrid = dtGrid.NewRow();
                    drGrid["DocKey"] = dr["DocKey"];
                    drGrid["DocItmKey"] = dr["DocItmKey"];
                    drGrid["LineType"] = dr["LineType"];
                    drGrid["LineLinkKey"] = dr["LineLinkKey"];
                    drGrid["ItmSN"] = dr["ItmSN"];
                    drGrid["ItmKey"] = dr["ItmKey"];
                    drGrid["ItmKeySelect"] = dr["ItmKeySelect"];
                    drGrid["ItmType"] = dr["ItmType"];
                    drGrid["ItmDes"] = dr["ItmDes"];
                    drGrid["ItmDeptKey"] = dr["ItmDeptKey"];
                    drGrid["ItmTranGrpKey"] = dr["ItmTranGrpKey"];
                    drGrid["ItmAccKey"] = dr["ItmAccKey"];
                    drGrid["ItmLocKey"] = dr["ItmLocKey"];
                    drGrid["ItmStock"] = dr["ItmStock"];
                    drGrid["ItmQty"] = dr["ItmQty"];
                    drGrid["ItmUOMKey"] = dr["ItmUOMKey"];
                    drGrid["ItmConRate"] = dr["ItmConRate"];
                    drGrid["ItmLatestCostF"] = dr["ItmLatestCostF"];
                    drGrid["ItmLatestCostH"] = dr["ItmLatestCostH"];
                    drGrid["ItmListPrice"] = dr["ItmListPrice"];
                    drGrid["ItmPriceBefore"] = dr["ItmPriceBefore"];
                    drGrid["ItmPriceAfter"] = dr["ItmPriceAfter"];
                    drGrid["ItmPrice"] = dr["ItmPrice"];
                    drGrid["ItmPriceUser"] = dr["ItmPriceUser"];
                    drGrid["ItmAmtShw"] = dr["ItmAmtShw"];
                    drGrid["ItmAmtF"] = dr["ItmAmtF"];
                    drGrid["ItmAmtH"] = dr["ItmAmtH"];
                    drGrid["ItmHide"] = dr["ItmHide"];
                    drGrid["ItmColorKey"] = dr["ItmColorKey"];
                    drGrid["ItmScaleSize"] = dr["ItmScaleSize"];
                    drGrid["ItmPacking"] = dr["ItmPacking"];
                    drGrid["ItmRem"] = dr["ItmRem"];
                    drGrid["ItmMark"] = dr["ItmMark"];
                    drGrid["ItmVendorKey"] = dr["ItmVendorKey"];
                    drGrid["ItmVendorNm"] = dr["ItmVendorNm"];
                    drGrid["ItmVendorCurrKey"] = dr["ItmVendorCurrKey"];
                    drGrid["ItmVendorCurrRate"] = dr["ItmVendorCurrRate"];
                    drGrid["ItmVendorPrice"] = dr["ItmVendorPrice"];
                    drGrid["ItmVendorPriceRatio"] = dr["ItmVendorPriceRatio"];
                    drGrid["ItmVendorPriceLock"] = dr["ItmVendorPriceLock"];
                    drGrid["ItmIGrpQtyLock"] = dr["ItmIGrpQtyLock"];
                    drGrid["ItmIGrpToPrint"] = dr["ItmIGrpToPrint"];
                    drGrid["ItmIGrpQtySet"] = dr["ItmIGrpQtySet"];
                    drGrid["ItmIGrpAmtSet"] = dr["ItmIGrpAmtSet"];                                  
                    drGrid["NSLink"] = dr["NSLink"];
                    drGrid["CreateDate"] = dr["CreateDate"];
                    drGrid["CreateUserKey"] = dr["CreateUserKey"];
                    drGrid["LastModifiedDate"] = dr["LastModifiedDate"];
                    drGrid["LastModifiedUserKey"] = dr["LastModifiedUserKey"];
                    drGrid["Custom1"] = dr["Custom1"];
                    drGrid["Custom2"] = dr["Custom2"];
                    drGrid["Custom3"] = dr["Custom3"];
                    drGrid["ItmDetSN"] = dr["ItmDetSN"];
                    drGrid["ItmID"] = dr["ItmID"];
                    drGrid["ItmAccID"] = dr["ItmAccID"];
                    drGrid["ItmAccDes"] = dr["ItmAccDes"];                    
                    drGrid["Selected"] = 1;
                    if (dtGrid.Columns.Contains("SlowMovingGoods")) drGrid["SlowMovingGoods"] = false; /* added by YST on 2023/04/24 */

                    switch ((int)objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                            drGrid["ItmReqDate"] = dr["ItmReqDate"];
                            drGrid["ItmPrmDate"] = dr["ItmPrmDate"];
                            drGrid["ARQOID"] = string.Empty;
                            drGrid["ARQODK"] = 0;
                            drGrid["ARQODItm"] = 0;
                            drGrid["ARSOID"] = string.Empty;
                            drGrid["ARSODK"] = 0;
                            drGrid["ARSODItm"] = 0;
                            drGrid["ARSOPOID"] = string.Empty;
                            drGrid["ARDOID"] = string.Empty;
                            drGrid["ARDODK"] = 0;
                            drGrid["ARDODItm"] = 0;
                            drGrid["CSCPSID"] = string.Empty;
                            drGrid["CSCPSDK"] = 0;
                            drGrid["CSCPSDItm"] = 0;
                            drGrid["CSCSIID"] = string.Empty;
                            drGrid["CSCSIDK"] = 0;
                            drGrid["CSCSIDItm"] = 0;
                            break;

                        case (int)GEnum.SystemCode.Sales_Order:
                            // added by KKAung on 22 Sep 2023 (start)
                            drGrid["ItmReqDate"] = dr["ItmReqDate"];
                            drGrid["ItmPrmDate"] = dr["ItmPrmDate"];
                            drGrid["ARQOID"] = dr["ARQOID"];
                            drGrid["ARQODK"] = dr["ARQODK"];
                            drGrid["ARQODItm"] = dr["ARQODItm"];
                            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                            {
                                drGrid["ARROID"] = dr["ARROID"];
                                drGrid["ARRODK"] = dr["ARRODK"];
                                drGrid["ARRODItm"] = dr["ARRODItm"];
                            }
                            drGrid["ARSOID"] = string.Empty;
                            drGrid["ARSODK"] = 0;
                            drGrid["ARSODItm"] = 0;
                            drGrid["ARSOPOID"] = string.Empty;
                            drGrid["ARDOID"] = string.Empty;
                            drGrid["ARDODK"] = 0;
                            drGrid["ARDODItm"] = 0;
                            drGrid["CSCPSID"] = string.Empty;
                            drGrid["CSCPSDK"] = 0;
                            drGrid["CSCPSDItm"] = 0;
                            drGrid["CSCSIID"] = string.Empty;
                            drGrid["CSCSIDK"] = 0;
                            drGrid["CSCSIDItm"] = 0;
                            break;
                            // (end)
                        case (int)GEnum.SystemCode.Reserve_Order:
                            drGrid["ItmReqDate"] = dr["ItmReqDate"];
                            drGrid["ItmPrmDate"] = dr["ItmPrmDate"];
                            drGrid["ARQOID"] = dr["ARQOID"];
                            drGrid["ARQODK"] = dr["ARQODK"];
                            drGrid["ARQODItm"] = dr["ARQODItm"];
                            drGrid["ARSOID"] = string.Empty;
                            drGrid["ARSODK"] = 0;
                            drGrid["ARSODItm"] = 0;
                            drGrid["ARSOPOID"] = string.Empty;
                            drGrid["ARDOID"] = string.Empty;
                            drGrid["ARDODK"] = 0;
                            drGrid["ARDODItm"] = 0;
                            drGrid["CSCPSID"] = string.Empty;
                            drGrid["CSCPSDK"] = 0;
                            drGrid["CSCPSDItm"] = 0;
                            drGrid["CSCSIID"] = string.Empty;
                            drGrid["CSCSIDK"] = 0;
                            drGrid["CSCSIDItm"] = 0;
                            break;

                        case (int)GEnum.SystemCode.Delivery_Order:
                            drGrid["ARQOID"] = dr["ARQOID"];
                            drGrid["ARQODK"] = dr["ARQODK"];
                            drGrid["ARQODItm"] = dr["ARQODItm"];
                            drGrid["ARSOID"] = dr["ARSOID"];
                            drGrid["ARSODK"] = dr["ARSODK"];
                            drGrid["ARSODItm"] = dr["ARSODItm"];
                            drGrid["ARSOPOID"] = dr["ARSOPOID"];
                            drGrid["ARDOID"] = string.Empty;
                            drGrid["ARDODK"] = 0;
                            drGrid["ARDODItm"] = 0;
                            drGrid["CSCPSID"] = string.Empty;
                            drGrid["CSCPSDK"] = 0;
                            drGrid["CSCPSDItm"] = 0;
                            drGrid["CSCSIID"] = string.Empty;
                            drGrid["CSCSIDK"] = 0;
                            drGrid["CSCSIDItm"] = 0;
                            //to be able to show serial nos by thettm (start)                    
                            drGrid["SerialNos"] = dr["SerialNos"];
                            //to be able to show serial nos by thettm(end)
                            break;

                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            drGrid["ARQOID"] = dr["ARQOID"];
                            drGrid["ARQODK"] = dr["ARQODK"];
                            drGrid["ARQODItm"] = dr["ARQODItm"];
                            drGrid["ARSOID"] = dr["ARSOID"];
                            drGrid["ARSODK"] = dr["ARSODK"];
                            drGrid["ARSODItm"] = dr["ARSODItm"];
                            drGrid["ARSOPOID"] = dr["ARSOPOID"];
                            drGrid["ARDOID"] = dr["ARDOID"];
                            drGrid["ARDODK"] = dr["ARDODK"];
                            drGrid["ARDODItm"] = dr["ARDODItm"];
                            drGrid["CSCPSID"] = dr["CSCPSID"];
                            drGrid["CSCPSDK"] = dr["CSCPSDK"];
                            drGrid["CSCPSDItm"] = dr["CSCPSDItm"];
                            drGrid["CSCSIID"] = dr["CSCSIID"];
                            drGrid["CSCSIDK"] = dr["CSCSIDK"];
                            drGrid["CSCSIDItm"] = dr["CSCSIDItm"];
                            //to be able to show serial nos by thettm (start)                    
                            drGrid["SerialNos"] = dr["SerialNos"];
                            //to be able to show serial nos by thettm(end)
                            break;
                    }
                    dtGrid.Rows.Add(drGrid);
                }
                dtGrid.AcceptChanges();
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
        private void DatailData_MSTItmDetAss_Get()
        {
            //Get Data from MSTItmDetAss
            try
            {
                //get Item detail
                int itmKey = 0;
                decimal qty = 0;
                decimal price = 0;
                decimal amtf = 0;

                //get assembly child data (current data in caller DocDetItm + MSTItmDetAss(which has not in DocDetItm))
                DataTable dt = tagrdItmGroupSelected.DataSource as DataTable;
                dt.TableName = "dtDocDetail";
                string xmlString = GFunc.ConvertDataTableToXML(dt);
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@ItmKey", ParentItmKey));
                paraList.Add(new SqlParameter("@DocKey", CallerDocKey));
                paraList.Add(new SqlParameter("@DocItmKey", CallerDocItmKey));
                paraList.Add(new SqlParameter("@xmlData", xmlString));
                DataTable dtresult = GFunc.ExecuteProc("Doc_AssemblyData_Merge", paraList);

                //get calculation variables required for assembly child price calculation
                decimal SN = 0M;

                //Update result
                dtresult.DefaultView.Sort = "ItmDetSN ASC";
                foreach (DataRowView dr in dtresult.DefaultView)
                {
                    if (CheckQtySetDecimalPt(GFunc.RndC(dr["ItmQty"], GVar.RndDecs.Qtypt), GFunc.RndC(dr["ItmPriceAfter"], GVar.RndDecs.Amtpt)))
                    {
                        dr["ItmIGrpQtySet"] = ItmQtySetPtVal;
                        dr["ItmIGrpAmtSet"] = ItmAmtSetPtVal;
                    }

                    //Update SN
                    SN = SN + 1;
                    dr["ItmDetSN"] = SN;                    

                    //Update assembly child price (only for new assembly child added)
                    if (GFunc.IsNEZ(dr["DocItmKey"]))
                    {

                        //Update Parent Infor
                        dr["ItmSN"] = ParentItmSN;
                        dr["ItmDeptKey"] = ParentItmDeptKey;
                        dr["ItmTranGrpKey"] = ParentItmTranGrpKey;
                        dr["ItmMark"] = ParentItmMark;
                        dr["ItmHide"] = ParentItmHide;
                        dr["ItmReqDate"] = ParentItmReqDate;
                        //dr["ItmJobKey"] = ParentJobKey;
                        dr["ItmPrmDate"] = ParentItmPrmDate;
                        dr["CreateUserKey"] = AppInfor.CurrentUserKey;
                        dr["LastModifiedUserKey"] = AppInfor.CurrentUserKey;

                        //When DataEntry is not for the 1st time we need to reset the default selection
                        if (FirstTimeEntry == false)
                            dr["Selected"] = 0;

                        //Set Location
                        if (GFunc.GetINTypeGroup(dr["ItmType"]) == (int)GEnum.INTypeGrp.Stock)
                            dr["ItmLocKey"] = LocKey.Value;

                        //Update price
                        itmKey = GFunc.NEInt(dr["ItmKey"], 0);
                        qty = GFunc.NEDec(dr["ItmIGrpQtySet"], 0);
                        price = DocComUtility.Price_Get(docPriceType, itmKey, docConKey, docCurrKey);
                        dr["ItmListPrice"] = DocComUtility.PriceByMSTItmDet_Get((int)GEnum.PriceListCode.UseStandardPrice, itmKey, docCurrKey);
                        dr["ItmPriceAfter"] = price;
                        dr["ItmPrice"] = price;
                        dr["ItmPriceUser"] = price;
                        dr["ItmLatestCostF"] = GFunc.RndDC(dr["ItmLatestCostH"], docCurrRate, GVar.RndDecs.Prcpt);

                        //Calculate 
                        if (GFunc.NEBool(dr["ItmIGrpQtyLock"], false) == false)
                        {
                            amtf = GFunc.RndC(qty * price * ParentItmQty, GVar.RndDecs.Amtpt);
                            dr["ItmQty"] = GFunc.RndC(qty * ParentItmQty, GVar.RndDecs.Qtypt);
                            dr["ItmIGrpAmtSet"] = GFunc.RndC(qty * price, GVar.RndDecs.Amtpt);
                            dr["ItmAmtShw"] = amtf;
                            dr["ItmAmtF"] = amtf;
                            dr["ItmAmtH"] = GFunc.RndC(amtf * docCurrRate, GVar.RndDecs.Amtpt);
                        }
                        else
                        {
                            amtf = GFunc.RndC(qty * price, GVar.RndDecs.Amtpt);
                            dr["ItmQty"] = GFunc.RndC(qty, GVar.RndDecs.Qtypt);
                            dr["ItmIGrpAmtSet"] = amtf;
                            dr["ItmAmtShw"] = amtf;
                            dr["ItmAmtF"] = amtf;
                            dr["ItmAmtH"] = GFunc.RndC(amtf * docCurrRate, GVar.RndDecs.Amtpt);
                        }
                    }
                }
              
                //rebind grid
                tagrdItmGroupSelected.DataSource = dtresult;               
                CalculateTotal();
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
        private void CalculateTotal()
        {
            DataTable dt = tagrdItmGroupSelected.DataSource as DataTable;
            dt.AcceptChanges();
            decimal total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (GFunc.NEBool(dr["Selected"], false))
                {
                    total = total + GFunc.NEDec(dr["ItmAmtShw"], 0);
                }
            }
            Total.SetValueTrigger(total, false);
        }//Completed
        private void CalculateAmount(DataRow dr, UltraGridCell currentCell = null)
        {
            try
            {
                ParentItmQty = GFunc.NEDec(ItmQty.Value, 0);//Must alway get the latest value from Header as the user could press ESC to undo the value
                decimal qtyset = 0;
                decimal qty = 0;
                decimal price = 0;
                decimal amtf = 0;               

                switch (GFunc.NEInt(dr["ItmType"], 0))
                {
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                    case (int)GEnum.ItemType.Charges:
                    case (int)GEnum.ItemType.Finished_GD:

                        if (currentCell != null && currentCell.Column.Key.ToLower() == "itmqty")
                        if (currentCell != null)
                        {
                            qty = GFunc.NEDec(currentCell.Value,0);
                            if (ParentItmQty > 0 && qty > 0)
                                dr["ItmIGrpQtySet"] = GFunc.RndC(qty / ParentItmQty, QtySetPt);
                        }

                        qtyset = GFunc.RndC(dr["ItmIGrpQtySet"], QtySetPt);
                        price = GFunc.RndC(dr["ItmPriceAfter"], priceDecPlace);

                        if (GFunc.NEBool(dr["ItmIGrpQtyLock"], false) == false)
                            qty = GFunc.RndC(qtyset * ParentItmQty, GVar.RndDecs.Qtypt);
                        else
                            qty = qtyset;

                        amtf = GFunc.RndC(qty * price, GVar.RndDecs.Amtpt);
                        dr["ItmIGrpQtySet"] = qtyset;
                        dr["ItmQty"] = qty;
                        dr["ItmPriceAfter"] = price;
                        dr["ItmPrice"] = price;
                        dr["ItmPriceUser"] = price;
                        dr["ItmIGrpAmtSet"] = GFunc.RndC(qtyset * price, QtySetPt);
                        dr["ItmAmtShw"] = amtf;
                        dr["ItmAmtF"] = amtf;
                        dr["ItmAmtH"] = GFunc.RndC(amtf * docCurrRate, GVar.RndDecs.Amtpt);
                        break;
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
        private bool SetItmInfor(int key, string id, string des)
        {
            try
            {
                UltraGridRow grdRow = tagrdItmGroupSelected.ActiveRow;
                if (grdRow == null)
                    return false;

                grdRow.Cells["ItmKeySelect"].Value = key;
                grdRow.Cells["ItmID"].Value = id;
                grdRow.Cells["ItmDes"].Value = des;
                MSTItm objItm = MSTItm.GetParent(key);

                grdRow.Cells["DocKey"].Value = CallerDocKey;
                grdRow.Cells["DocItmKey"].Value = GFunc.NEInt(grdRow.Cells["DocItmKey"].Value, 0);
                grdRow.Cells["LineType"].Value = 1100;//Detail Assembly
                grdRow.Cells["LineLinkKey"].Value = CallerDocItmKey;
                grdRow.Cells["ItmSN"].Value = ParentItmSN;
                grdRow.Cells["ItmKey"].Value = objItm.ItmKey;
                grdRow.Cells["ItmType"].Value = objItm.ItmType;
                grdRow.Cells["ItmDeptKey"].Value = ParentItmDeptKey;
                grdRow.Cells["ItmTranGrpKey"].Value = ParentItmTranGrpKey;
                grdRow.Cells["ItmAccKey"].Value = 0;
                grdRow.Cells["ItmReqDate"].Value = ParentItmReqDate;
                grdRow.Cells["ItmPrmDate"].Value = ParentItmPrmDate;
                grdRow.Cells["ItmHide"].Value = ParentItmHide;
                grdRow.Cells["ItmPacking"].Value = objItm.INPacking;
                grdRow.Cells["ItmMark"].Value = ParentItmMark;
                grdRow.Cells["ItmVendorKey"].Value = 0;
                grdRow.Cells["ItmVendorNm"].Value = string.Empty;
                grdRow.Cells["ItmVendorCurrKey"].Value = 1;
                grdRow.Cells["ItmVendorCurrRate"].Value = 1;
                grdRow.Cells["ItmVendorPrice"].Value = 0;
                grdRow.Cells["ItmVendorPriceRatio"].Value = 0;
                grdRow.Cells["ItmVendorPriceLock"].Value = 1;
                grdRow.Cells["ItmIGrpQtyLock"].Value = 0;
                grdRow.Cells["ItmIGrpToPrint"].Value = 1;
                grdRow.Cells["ItmIGrpAmtSet"].Value = 0;
                grdRow.Cells["ItmAmtShw"].Value = 0;
                grdRow.Cells["ItmAmtF"].Value = 0;
                grdRow.Cells["ItmAmtH"].Value = 0;
                grdRow.Cells["ARQODK"].Value = 0;
                grdRow.Cells["ARQODItm"].Value = 0;
                grdRow.Cells["ARSODK"].Value = 0;
                grdRow.Cells["ARSODItm"].Value = 0;
                grdRow.Cells["ARDODK"].Value = 0;
                grdRow.Cells["ARDODItm"].Value = 0;
                grdRow.Cells["CSCPSDK"].Value = 0;
                grdRow.Cells["CSCPSDItm"].Value = 0;
                grdRow.Cells["CSCSIDK"].Value = 0;
                grdRow.Cells["CSCSIDItm"].Value = 0;
                grdRow.Cells["CreateDate"].Value = DateTime.Today;
                grdRow.Cells["CreateUserKey"].Value = AppInfor.CurrentUserKey;
                grdRow.Cells["LastModifiedDate"].Value = DateTime.Today;
                grdRow.Cells["LastModifiedUserKey"].Value = AppInfor.CurrentUserKey;
                grdRow.Cells["Selected"].Value = 1;            
                if (grdRow.Cells.Exists("SlowMovingGoods")) grdRow.Cells["SlowMovingGoods"].Value = objItm.ObCost > 0; /* added by YST on 2023/04/24 */

                switch ((GEnum.INTypeGrp)GFunc.GetINTypeGroup(objItm.ItmType))
                {
                    case GEnum.INTypeGrp.Stock:
                        grdRow.Cells["ItmLocKey"].Value = LocKey.Value;
                        grdRow.Cells["ItmStock"].Value = objItm.QtyStock;
                        grdRow.Cells["ItmIGrpQtySet"].Value = 0;
                        grdRow.Cells["ItmQty"].Value = 0;
                        grdRow.Cells["ItmUOMKey"].Value = objItm.BUOMKey;
                        grdRow.Cells["ItmConRate"].Value = 1;
                        grdRow.Cells["ItmLatestCostF"].Value = GFunc.RndC(GFunc.NEDec(objItm.CostLatest, 0) / docCurrRate, GVar.RndDecs.COSpt);
                        grdRow.Cells["ItmLatestCostH"].Value = objItm.CostLatest;
                        grdRow.Cells["ItmListPrice"].Value = DocComUtility.PriceByMSTItmDet_Get((int)GEnum.PriceListCode.UseStandardPrice, key, docCurrKey);
                        grdRow.Cells["ItmPriceAfter"].Value = DocComUtility.Price_Get(docPriceType, key, docConKey, docCurrKey);
                        grdRow.Cells["ItmPriceBefore"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPrice"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPriceUser"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmColorKey"].Value = objItm.ColorKey;
                        grdRow.Cells["ItmScaleSize"].Value = objItm.ScaleSize;

                        break;
                    case GEnum.INTypeGrp.Non_Stock:
                        grdRow.Cells["ItmLocKey"].Value = null;
                        grdRow.Cells["ItmStock"].Value = 0;
                        grdRow.Cells["ItmIGrpQtySet"].Value = 0;
                        grdRow.Cells["ItmQty"].Value = 0;
                        grdRow.Cells["ItmUOMKey"].Value = objItm.BUOMKey;
                        grdRow.Cells["ItmConRate"].Value = 1;
                        grdRow.Cells["ItmLatestCostF"].Value = GFunc.RndC(GFunc.NEDec(objItm.CostLatest, 0) / docCurrRate, GVar.RndDecs.COSpt);
                        grdRow.Cells["ItmLatestCostH"].Value = objItm.CostLatest;
                        grdRow.Cells["ItmListPrice"].Value = DocComUtility.PriceByMSTItmDet_Get((int)GEnum.PriceListCode.UseStandardPrice, key, docCurrKey);
                        grdRow.Cells["ItmPriceAfter"].Value = DocComUtility.Price_Get(docPriceType, key, docConKey, docCurrKey);
                        grdRow.Cells["ItmPriceBefore"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPrice"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPriceUser"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmColorKey"].Value = objItm.ColorKey;
                        grdRow.Cells["ItmScaleSize"].Value = objItm.ScaleSize;
                        break;
                    case GEnum.INTypeGrp.Charges:
                        grdRow.Cells["ItmLocKey"].Value = null;
                        grdRow.Cells["ItmStock"].Value = 0;
                        grdRow.Cells["ItmIGrpQtySet"].Value = 0;
                        grdRow.Cells["ItmQty"].Value = 0;
                        grdRow.Cells["ItmUOMKey"].Value = null;
                        grdRow.Cells["ItmConRate"].Value = 1;
                        grdRow.Cells["ItmLatestCostF"].Value = GFunc.RndC(GFunc.NEDec(objItm.CostLatest, 0) / docCurrRate, GVar.RndDecs.COSpt);
                        grdRow.Cells["ItmLatestCostH"].Value = objItm.CostLatest;
                        grdRow.Cells["ItmListPrice"].Value = DocComUtility.PriceByMSTItmDet_Get((int)GEnum.PriceListCode.UseStandardPrice, key, docCurrKey);
                        grdRow.Cells["ItmPriceAfter"].Value = DocComUtility.Price_Get(docPriceType, key, docConKey, docCurrKey); ;
                        grdRow.Cells["ItmPriceBefore"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPrice"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPriceUser"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmColorKey"].Value = 0;
                        grdRow.Cells["ItmScaleSize"].Value = string.Empty;

                        break;
                    case GEnum.INTypeGrp.Remark:
                        grdRow.Cells["ItmLocKey"].Value = null;
                        grdRow.Cells["ItmStock"].Value = null;
                        grdRow.Cells["ItmIGrpQtySet"].Value = null;
                        grdRow.Cells["ItmQty"].Value = null;
                        grdRow.Cells["ItmUOMKey"].Value = null;
                        grdRow.Cells["ItmConRate"].Value = 1;
                        grdRow.Cells["ItmLatestCostF"].Value = 0;
                        grdRow.Cells["ItmLatestCostH"].Value = 0;
                        grdRow.Cells["ItmListPrice"].Value = 0;
                        grdRow.Cells["ItmPriceAfter"].Value = 0;
                        grdRow.Cells["ItmPriceBefore"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPrice"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmPriceUser"].Value = grdRow.Cells["ItmPriceAfter"].Value;
                        grdRow.Cells["ItmColorKey"].Value = 0;
                        grdRow.Cells["ItmScaleSize"].Value = string.Empty;
                        break;
                }

                GridLock_Set();
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
        private bool SetVendInfor(int key, string id, string des)
        {
            try
            {
                UltraGridRow grdRow = tagrdItmGroupSelected.ActiveRow;
                if (grdRow == null)
                    return false;

                MSTCon objCon = MSTCon.Get(key);
                if (objCon.ConKey == null)
                {
                    MsgBox.Show("Unable to locate selected vendor, default values will be used.");
                    grdRow.Cells["ItmVendorKey"].Value = null;
                    grdRow.Cells["ItmVendorNm"].Value = string.Empty;
                    grdRow.Cells["ItmVendorCurrKey"].Value = 1;
                    grdRow.Cells["ItmVendorCurrRate"].Value = 1M;
                }
                else
                {
                    grdRow.Cells["ItmVendorKey"].Value = key;
                    grdRow.Cells["ItmVendorNm"].Value = objCon.ConNm;
                    grdRow.Cells["ItmVendorCurrKey"].Value = objCon.VCurrkey;
                    grdRow.Cells["ItmVendorCurrRate"].Value = DocComUtility.CurrRate_Get((int)objCon.VCurrkey, objDoc.DocDate, true);
                }

                GridLock_Set();
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
        private void LockGridCol()
        {
            #region /* commented by YST on 2022/12/09 */
            /*
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["DocKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["DocItmKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LineType"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LineLinkKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmSN"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmKeySelect"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmType"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmDeptKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmTranGrpKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAccKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmStock"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmQty"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmReqDate"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPrmDate"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmConRate"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPriceBefore"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPrice"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPriceUser"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAmtShw"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAmtF"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAmtH"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmHide"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmColorKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmScaleSize"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmMark"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmVendorCurrKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmVendorPriceLock"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmIGrpAmtSet"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["NSLink"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARQOID"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARQODK"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARQODItm"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSOID"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSODK"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSODItm"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSOPOID"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARDOID"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARDODK"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARDODItm"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCPSID"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCPSDK"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCPSDItm"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCSIID"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCSIDK"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCSIDItm"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CreateDate"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CreateUserKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LastModifiedDate"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LastModifiedUserKey"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["Custom1"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["Custom2"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["Custom3"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmDetSN"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAccID"].CellActivation = Activation.ActivateOnly;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAccDes"].CellActivation = Activation.ActivateOnly;

            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["DocKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["DocItmKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LineType"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LineLinkKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmSN"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmKeySelect"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmType"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmDeptKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmTranGrpKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAccKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmStock"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmQty"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmReqDate"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPrmDate"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmConRate"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmLatestCostH"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPriceBefore"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPrice"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmPriceUser"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAmtShw"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAmtF"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAmtH"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmHide"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmColorKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmScaleSize"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmMark"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmVendorCurrKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmVendorPriceRatio"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmVendorPriceLock"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmIGrpAmtSet"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["NSLink"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARQOID"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARQODK"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARQODItm"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSOID"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSODK"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSODItm"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARSOPOID"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARDOID"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARDODK"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ARDODItm"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCPSID"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCPSDK"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCPSDItm"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCSIID"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCSIDK"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CSCSIDItm"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CreateDate"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["CreateUserKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LastModifiedDate"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["LastModifiedUserKey"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["Custom1"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["Custom2"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["Custom3"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmDetSN"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAccID"].TabStop = false;
            tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns["ItmAccDes"].TabStop = false;
            */
            #endregion

            /* added by YST on 2022/12/09 */
            foreach (UltraGridColumn col in tagrdItmGroupSelected.DisplayLayout.Bands[0].Columns)
            {                
                switch (col.Key)
                {
                    case "Selected":
                    case "ItmID":
                    case "ItmDes":
                    case "ItmQty":
                    case "ItmIGrpQtySet":                    
                    case "ItmUOMKey":
                    case "ItmPriceAfter":
                        col.CellActivation = Activation.AllowEdit;
                        col.TabStop = true;
                        break;
                    default:
                        col.CellActivation = Activation.ActivateOnly;
                        col.TabStop = false;
                        break;
                }
            }

        }//Completed
        private void GridLock_Set()
        {                     
            try
            {
                #region /*commented by YST on 2022/12/09 */
                /*
                UltraGridRow grdRow = null;
                Activation Stk = Activation.AllowEdit;
                Activation NS = Activation.AllowEdit;
                Activation Chg = Activation.AllowEdit;
                bool StkTS = true;
                bool NSTS = true;
                bool ChgTS = true;

                if (tagrdItmGroupSelected.ActiveRow == null)
                    return;

                grdRow = tagrdItmGroupSelected.ActiveRow;
                switch (GFunc.NEInt(grdRow.Cells["ItmType"].Value, 0))
                {
                    case (int)GEnum.ItemType.Stock:
                    case (int)GEnum.ItemType.Finished_GD:
                        break;

                    case (int)GEnum.ItemType.Non_Stock:
                    case (int)GEnum.ItemType.Service:
                        Stk = Activation.ActivateOnly;
                        StkTS = false;
                        break;

                    case (int)GEnum.ItemType.Charges:
                        Stk = Activation.ActivateOnly;
                        NS = Activation.ActivateOnly;
                        StkTS = false;
                        NSTS = false;
                        break;

                    default:
                        Stk = Activation.ActivateOnly;
                        NS = Activation.ActivateOnly;
                        Chg = Activation.ActivateOnly;
                        StkTS = false;
                        NSTS = false;
                        ChgTS = false;
                        break;
                }
                grdRow.Cells["ItmLocKey"].Column.CellActivation = Stk;
                grdRow.Cells["ItmUOMKey"].Column.CellActivation = NS;
                grdRow.Cells["ItmLatestCostF"].Column.CellActivation = Chg;
                grdRow.Cells["ItmListPrice"].Column.CellActivation = Chg;
                grdRow.Cells["ItmPriceAfter"].Column.CellActivation = Chg;
                grdRow.Cells["ItmVendorKey"].Column.CellActivation = Chg;
                grdRow.Cells["ItmVendorNm"].Column.CellActivation = Chg;
                grdRow.Cells["ItmVendorPrice"].Column.CellActivation = Chg;
                grdRow.Cells["ItmIGrpQtySet"].Column.CellActivation = Chg;
                grdRow.Cells["ItmIGrpQtyLock"].Column.CellActivation = Chg;

                grdRow.Cells["ItmLocKey"].Column.TabStop = StkTS;
                grdRow.Cells["ItmUOMKey"].Column.TabStop = NSTS;
                grdRow.Cells["ItmLatestCostF"].Column.TabStop = ChgTS;
                grdRow.Cells["ItmListPrice"].Column.TabStop = ChgTS;
                grdRow.Cells["ItmPriceAfter"].Column.TabStop = ChgTS;
                grdRow.Cells["ItmVendorKey"].Column.TabStop = ChgTS;
                grdRow.Cells["ItmVendorNm"].Column.TabStop = ChgTS;
                grdRow.Cells["ItmVendorPrice"].Column.TabStop = ChgTS;
                grdRow.Cells["ItmIGrpQtySet"].Column.TabStop = ChgTS;
                grdRow.Cells["ItmIGrpQtyLock"].Column.TabStop = ChgTS;

                //Special Condition for CurrRate
                if (GFunc.NEInt(grdRow.Cells["ItmVendorCurrKey"].Value, 1) == 1)
                {
                    grdRow.Cells["ItmVendorCurrRate"].Column.CellActivation = Activation.ActivateOnly;
                    grdRow.Cells["ItmVendorCurrRate"].Column.TabStop = false;
                }
                else
                {
                    grdRow.Cells["ItmVendorCurrRate"].Column.CellActivation = Chg;
                    grdRow.Cells["ItmVendorCurrRate"].Column.TabStop = ChgTS;
                }
                */
                #endregion               

            }
            catch (TAException tex)
            {
                Error(tex, false);
            }
            catch (Exception ex)
            {
                Error(ex, false);
            }                    
        }//Completed
        
        //Button Click Event
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validation
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.Validate();
                this.tagrdItmGroupSelected.PerformAction(UltraGridAction.ExitEditMode);
                this.tagrdItmGroupSelected.UpdateData();

                //we need to check if the active row data cannot be commited 
                //if it cannot be commited, the IsGridDirty would return a false
                //thus saving should not be perform and the user needs to be inform of the data error
                if (tagrdItmGroupSelected.ActiveRow != null)
                {
                    if (tagrdItmGroupSelected.ActiveRow.DataChanged && !tagrdItmGroupSelected.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        //Fail to Commit Changes in grid
                        MsgBox.Show("Validation Failed");
                        return;
                    }
                }

                if (TAUtil.ControlGVar.FormValidateFail)
                {
                    //Fail to Commit Changes in grid
                    MsgBox.Show("Validation Failed");
                    return;
                }               

                #endregion

                //Delete data in Document DocDetail Child for the CallerDocItmKey
                int rowCount = DocDetdt.Rows.Count;
                for (int i = rowCount - 1; i > -1; i--)
                {
                    if (DocDetdt.Rows[i]["LineLinkKey"].ToString() == CallerDocItmKey.ToString())
                    {
                        DocDetdt.Rows[i].Delete();
                    }
                }
                DocDetdt.AcceptChanges();

                //Syncronise Assembly Child data
                //DataTable dtAssChild = tagrdItmGroupSelected.DataSource as DataTable;
                // dtAssChild.DefaultView.RowFilter = "Selected =1";
                IEnumerable<DataRow> dtAssChildFilter = (tagrdItmGroupSelected.DataSource as DataTable).AsEnumerable().Where(r => r.Field<bool>("Selected") == true).OrderBy(o=>o.Field<decimal>("ItmDetSN"));

                decimal ItmDetSN = 0;
                foreach (DataRow dr in dtAssChildFilter)
                {
                    if (CheckQtySetDecimalPt(GFunc.RndC(dr["ItmQty"], GVar.RndDecs.Qtypt), GFunc.RndC(dr["ItmPriceAfter"], GVar.RndDecs.Amtpt)))
                    {
                        dr["ItmIGrpQtySet"] = ItmQtySetPtVal;
                        dr["ItmIGrpAmtSet"] = ItmAmtSetPtVal;
                    }

                    //if (dr.IsNew)
                    //    continue;
                    DataRow drNew = DocDetdt.NewRow();

                    #region assign Assembly Item values

                    drNew["DocKey"] = dr["DocKey"];
                    if (GFunc.IsNEZ(dr["DocItmKey"]))

                        drNew["DocItmKey"] = DocComUtility.GridAutoID_Get(DocDetgrd, "DocKey", "DocItmKey");
                    else
                        drNew["DocItmKey"] = dr["DocItmKey"];

                    drNew["LineType"] = dr["LineType"];
                    drNew["LineLinkKey"] = dtAssParent.Rows[0]["DocItmKey"];//Parent
                    drNew["ItmSN"] = dr["ItmSN"];
                    drNew["ItmKey"] = dr["ItmKey"];
                    drNew["ItmKeySelect"] = dr["ItmKeySelect"];
                    drNew["ItmType"] = dr["ItmType"];
                    drNew["ItmID"] = dr["ItmID"];
                    drNew["ItmDes"] = dr["ItmDes"];
                    drNew["ItmDeptKey"] = dr["ItmDeptKey"];
                    drNew["ItmTranGrpKey"] = dr["ItmTranGrpKey"];
                    drNew["ItmAccKey"] = dr["ItmAccKey"];
                    drNew["ItmLocKey"] = dr["ItmLocKey"];
                    drNew["ItmStock"] = dr["ItmStock"];
                    drNew["ItmQty"] = dr["ItmQty"];
                    if (DocDetdt.Columns.Contains("ItmReqDate"))
                    {
                        drNew["ItmReqDate"] = dr["ItmReqDate"];
                        drNew["ItmPrmDate"] = dr["ItmPrmDate"];
                    }
                    drNew["ItmUOMKey"] = dr["ItmUOMKey"];
                    drNew["ItmConRate"] = dr["ItmConRate"];
                    drNew["ItmLatestCostF"] = dr["ItmLatestCostF"];
                    drNew["ItmLatestCostH"] = dr["ItmLatestCostH"];
                    drNew["ItmListPrice"] = dr["ItmListPrice"];
                    drNew["ItmPriceBefore"] = dr["ItmPriceAfter"];//Use ItmPriceAfter
                    drNew["ItmPriceAfter"] = dr["ItmPriceAfter"];
                    drNew["ItmDisPercent"] = 0;
                    drNew["ItmDisValue"] = 0;
                    drNew["ItmPrice"] = dr["ItmPrice"];
                    drNew["ItmPriceUser"] = dr["ItmPriceUser"];
                    drNew["ItmControlPrice"] = 0;
                    drNew["ItmControlPriceBase"] = 0;
                    drNew["ItmAmtShw"] = dr["ItmAmtShw"];
                    drNew["ItmAmtF"] = dr["ItmAmtF"];
                    drNew["ItmAmtH"] = dr["ItmAmtH"];
                    drNew["ItmTaxable"] = 0;
                    drNew["ItmTaxGrpKey"] = 0;
                    drNew["ItmTaxGrpRate"] = 0;
                    drNew["ItmTaxGrpAmtF"] = 0;
                    drNew["ItmTaxGrpAmtL"] = 0;
                    drNew["ItmHide"] = 0;
                    drNew["ItmColorKey"] = dr["ItmColorKey"];
                    drNew["ItmScaleSize"] = dr["ItmScaleSize"];
                    drNew["ItmPacking"] = dr["ItmPacking"];
                    drNew["ItmRem"] = dr["ItmRem"];
                    drNew["ItmMark"] = dr["ItmMark"];
                    drNew["ItmVendorKey"] = dr["ItmVendorKey"];
                    drNew["ItmVendorNm"] = dr["ItmVendorNm"];
                    drNew["ItmVendorCurrKey"] = dr["ItmVendorCurrKey"];
                    drNew["ItmVendorCurrRate"] = dr["ItmVendorCurrRate"];
                    drNew["ItmVendorPrice"] = dr["ItmVendorPrice"];
                    drNew["ItmVendorPriceRatio"] = 0;
                    drNew["ItmVendorPriceLock"] = 1;
                    drNew["ItmJobKey"] = ParentJobKey;
                    drNew["ItmJobPhaseKey"] = 0;
                    drNew["ItmJobTaskKey"] = 0;
                    drNew["ItmJobCostTypeKey"] = 0;
                    drNew["ItmIGrpDItm"] = 0;
                    drNew["ItmIGrpQtyLock"] = dr["ItmIGrpQtyLock"];
                    drNew["ItmIGrpToPrint"] = dr["ItmIGrpToPrint"];
                    drNew["ItmIGrpQtySet"] = dr["ItmIGrpQtySet"];
                    drNew["ItmIGrpAmtSet"] = dr["ItmIGrpAmtSet"];
                    drNew["ItmAttachment"] = 0;
                    drNew["ItmBatchKey"] = ParentJobKey>0?999:0;
                    drNew["ItmBatchQty"] = 0;
                    drNew["NSLink"] = dr["NSLink"];
                    drNew["CreateDate"] = dr["CreateDate"];
                    drNew["CreateUserKey"] = dr["CreateUserKey"];
                    drNew["LastModifiedDate"] = dr["LastModifiedDate"];
                    drNew["LastModifiedUserKey"] = dr["LastModifiedUserKey"];
                    drNew["Custom1"] = dr["Custom1"];
                    drNew["Custom2"] = dr["Custom2"];
                    drNew["Custom3"] = dr["Custom3"];

                    switch ((int)objDoc.DocCodeKey)
                    {
                        case (int)GEnum.SystemCode.Quotation:
                            break;

                        case (int)GEnum.SystemCode.Sales_Order:
                          // added by KKAung on 22 Sep 2023 (start)                            
                            drNew["ARQOID"] = dr["ARQOID"];
                            drNew["ARQODK"] = dr["ARQODK"];
                            drNew["ARQODItm"] = dr["ARQODItm"];
                            if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                            {
                                drNew["ARROID"] = dr["ARROID"];
                                drNew["ARRODK"] = dr["ARRODK"];
                                drNew["ARRODItm"] = dr["ARRODItm"];
                            }
                            break;
                        // (end)
                        case (int)GEnum.SystemCode.Reserve_Order:
                            drNew["ARQOID"] = dr["ARQOID"];
                            drNew["ARQODK"] = dr["ARQODK"];
                            drNew["ARQODItm"] = dr["ARQODItm"];
                            break;

                        case (int)GEnum.SystemCode.Delivery_Order:
                            drNew["ARQOID"] = dr["ARQOID"];
                            drNew["ARQODK"] = dr["ARQODK"];
                            drNew["ARQODItm"] = dr["ARQODItm"];
                            drNew["ARSOID"] = dr["ARSOID"];
                            drNew["ARSODK"] = dr["ARSODK"];
                            drNew["ARSODItm"] = dr["ARSODItm"];
                            drNew["ARSOPOID"] = dr["ARSOPOID"];
                            drNew["SerialNos"] = dr["SerialNos"]; /* added by YST */
                            break;

                        case (int)GEnum.SystemCode.Sales_Invoice:
                        case (int)GEnum.SystemCode.Sales_Debit_Note:
                        case (int)GEnum.SystemCode.Sales_Credit_Note:
                        case (int)GEnum.SystemCode.Cash_Sale:
                        case (int)GEnum.SystemCode.Cash_Debit_Note:
                        case (int)GEnum.SystemCode.Cash_Credit_Note:
                            drNew["ARQOID"] = dr["ARQOID"];
                            drNew["ARQODK"] = dr["ARQODK"];
                            drNew["ARQODItm"] = dr["ARQODItm"];
                            drNew["ARSOID"] = dr["ARSOID"];
                            drNew["ARSODK"] = dr["ARSODK"];
                            drNew["ARSODItm"] = dr["ARSODItm"];
                            drNew["ARSOPOID"] = dr["ARSOPOID"];
                            drNew["ARDOID"] = dr["ARDOID"];
                            drNew["ARDODK"] = dr["ARDODK"];
                            drNew["ARDODItm"] = dr["ARDODItm"];
                            drNew["CSCPSID"] = dr["CSCPSID"];
                            drNew["CSCPSDK"] = dr["CSCPSDK"];
                            drNew["CSCPSDItm"] = dr["CSCPSDItm"];
                            drNew["CSCSIID"] = dr["CSCSIID"];
                            drNew["CSCSIDK"] = dr["CSCSIDK"];
                            drNew["CSCSIDItm"] = dr["CSCSIDItm"];
                            drNew["SerialNos"] = dr["SerialNos"]; /* added by YST */
                            break;
                    }

                    //increament ItmDetSN for child assembly items
                    ItmDetSN++;
                    drNew["ItmDetSN"] = ItmDetSN;

                    #endregion

                    DocDetdt.Rows.Add(drNew);
                }
                DocDetdt.AcceptChanges();

                if (DocDetgrd != null)
                    DocDetgrd.DataSource = DocDetdt;

                //assign infor to Assembly Parent and recalculate
                DocDetgrd.ActiveRow.Cells["ItmQty"].Value = ItmQty.Value;
                DocDetgrd.ActiveRow.Cells["ItmPrice"].Value = ItmPrice.Value;
                DocDetgrd.ActiveRow.Cells["ItmPriceAfter"].Value = ItmPrice.Value;
                DocDetgrd.ActiveRow.Cells["ItmDes"].Value = ItmDes.Value;
                DocDetUtil.ItmPriceAfter_CustomUpdate(objDoc, DocDetgrd);
                DocDetgrd.ActiveRow.Update();

                /* Checking qty of child items - added by YST on 2021/08/20 */
                ErrorMsg = "";
                if (ItmQty.Enabled == true && ParentItmQty > 0)
                {                    
                    DataRow[] dr = (tagrdItmGroupSelected.DataSource as DataTable).Select("ItmQty = 0 and Selected = 1");
                    if (dr != null && dr.Length > 0)
                    {
                        ErrorMsg = "<font color='red'>Qty of selected child items should not be 0.</font>";
                        MsgBox.Show(ErrorMsg, GEnum.MsgBoxIcon.Alert, GEnum.MsgBoxButton.OK);
                    }
                    else
                    {
                        if (formClose == false)
                        {
                            ErrorMsg = "";
                            this.Close();                          
                        }
                    }
                }
                else
                {
                    if (formClose == false)
                    {
                        this.Close();
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }//Completed

        //Set Error Methods
        private void OnDataError(object sender, TAUtil.TAErrorEventArgs e)
        {
            if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
            {
                if (sender.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    TAUtil.TAGridEditor grd = sender as TAUtil.TAGridEditor;
                    if (grd.ActiveCell.Column.EditorComponent != null)
                    {
                        grd.PerformAction(UltraGridAction.EnterEditMode);
                        GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);
                    }
                }
            }
            else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC)
            {

                MsgBox.Show(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
            }
            else if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
            {

                MsgBox.Show("FORMULA NOT RECOGNIZE");
            }
            else
            {
                MsgBox.Show(e.ErrorMessage);
            }

        }//Completed
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

        /* added by YST on 2023/04/24 */
        private void tagrdItmGroupSelected_InitializeRow(object sender, InitializeRowEventArgs e)
        {           
            if (e.Row.Cells.Exists("SlowMovingGoods") && GFunc.NEBool(e.Row.Cells["SlowMovingGoods"].Value, false) == true)
            {
                e.Row.Appearance.BackColor = Color.LightGreen;
            }
        }

        /* added by YST on 2024/11/15 => 
         * ParentQty = 15 , 
         * ChildQty =  5 for Item1 , QtySet of ChildItem1 = 0.333333333333333 
         * ChildQty = 10 for Item2 , QtySet of ChildItem2 = 0.666666666666667
         * ColDataFormat = #,##0.00################ 
         * select ColFldNm,ColDataFormat 
         * from SYS_FormSettingIDDet 
         * where ListID like 'frmAssemblyEntryGrid' and ColFldNm in ('ItmIGrpQtySet','ItmIGrpAmtSet') and userKey = 0 
        */
        private bool CheckQtySetDecimalPt(decimal ItmQty, decimal ItmPriceAfter)
        {
            try
            {
                if (ParentItmQty > 0)
                {
                    decimal RoundingPt = 0.0M;
                    ItmQtySetPtVal = GFunc.RndC(ItmQty / ParentItmQty, QtySetPt);
                    RoundingPt = Math.Round(ItmQtySetPtVal, GVar.RndDecs.Qtypt);
                    if (ItmQtySetPtVal != RoundingPt)
                    {
                        ItmAmtSetPtVal = GFunc.RndC(ItmQtySetPtVal * ItmPriceAfter, QtySetPt);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
                return false;
            }           
        }
    }
}
