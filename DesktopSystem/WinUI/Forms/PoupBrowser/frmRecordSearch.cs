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
using BOLib;
using System.Data.SqlClient;
using System.Reflection;
using TAUtil;


namespace WinUI
{
    public partial class frmRecordSearch : Form
    {
        #region Declaration
        //Local Variables
        private string ContextMenuSetting = string.Empty;
        private bool formClose = false;
        private string listSettingId = string.Empty;
        private int DocCodeKey;
        private int CallerDocCodeKey;
        private int DocConKey;
        string[] parmList;

        string SPSearchValue = string.Empty;
        int SearchNumChar = 1;
        Boolean SearchFromStart = false;
        Boolean PerformSPCall = false;

        //Event to be assigned by the caller detail form
        public GVar.PopupSelectedEvent RecordSelectedEvent = null;
        public GVar.PopupCommonSelectedEvent CommonRecordSelectedEvent = null;
        public GVar.JobPopupSelectedEvent JobRecordSelectedEvent = null;

        private int _key = 0;
        private string _id = string.Empty;
        private string _Des = string.Empty;
        private int _jobPhaseKey = 0;
        private int _jobCostTypeKey = 0;
        private int _jobTaskKey = 0;
        private GEnum.PopupType popupType = GEnum.PopupType.CusID;
        private string popupMSTFrmNm = string.Empty;

        //Properties
        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Des
        {
            get { return _Des; }
            set { _Des = value; }
        }
        public int JobPhaseKey
        {
            get { return _jobPhaseKey; }
            set { _jobPhaseKey = value; }
        }
        public int JobCostTypeKey
        {
            get { return _jobCostTypeKey; }
            set { _jobCostTypeKey = value; }
        }
        public int JobTaskKey
        {
            get { return _jobTaskKey; }
            set { _jobTaskKey = value; }
        }

        #endregion

        //Initialize
        public frmRecordSearch()
        {
            InitializeComponent();
        }
        public frmRecordSearch(string lstSettingID, string SearchText, GEnum.PopupType PopupType, int vDocCodeKey)
        {
            InitializeComponent();
            listSettingId = lstSettingID;
            DocCodeKey = vDocCodeKey;
            popupType = PopupType;
            parmList = new string[] { SearchText.Replace("\r\n", "") };
        }//Completed
        public frmRecordSearch(string lstSettingID, string SearchText, GEnum.PopupType PopupType, int vDocCodeKey, int vCallerDocCodeKey)
        {
            InitializeComponent();
            listSettingId = lstSettingID;
            DocCodeKey = vDocCodeKey;
            CallerDocCodeKey = vCallerDocCodeKey;
            popupType = PopupType;
            parmList = new string[] { SearchText.Replace("\r\n", "") };
        }//Completed
        public frmRecordSearch(string lstSettingID, string SearchText, int VendorKey, GEnum.PopupType PopupType, int vDocCodeKey)
        {
            InitializeComponent();
            listSettingId = lstSettingID;
            DocCodeKey = vDocCodeKey;
            popupType = PopupType;
            parmList = new string[] { SearchText.Replace("\r\n", ""), VendorKey.ToString() };

        }//Completed
        public frmRecordSearch(string lstSettingID, string SearchText, int VendorKey, GEnum.PopupType PopupType, int vDocCodeKey, int vCallerDocCodeKey)
        {
            InitializeComponent();
            listSettingId = lstSettingID;
            DocCodeKey = vDocCodeKey;
            CallerDocCodeKey = vCallerDocCodeKey;
            popupType = PopupType;
            parmList = new string[] { SearchText.Replace("\r\n", ""), VendorKey.ToString() };

        }//Completed

        //Form Events
        private void frmRecordSearch_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                GlobalUI.FormGrids_Set(this, this.DocCodeKey, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(this.DocCodeKey);
                GlobalUI.Combos_Fill(this, this.DocCodeKey);
                FormLayout();

                #region Set Group Option Check Value
                switch (popupType)
                {
                    case GEnum.PopupType.CusID:
                    case GEnum.PopupType.CustOpenID:
                    case GEnum.PopupType.VendID:
                    case GEnum.PopupType.VendOpenID:
                    case GEnum.PopupType.JobID:
                    case GEnum.PopupType.JobOpenID:
                    case GEnum.PopupType.AccID:
                    case GEnum.PopupType.AccDisID:
                    case GEnum.PopupType.AccLiabilityID:
                    case GEnum.PopupType.AccOpenID:
                    case GEnum.PopupType.ItmID:
                    case GEnum.PopupType.ItmFinishID:
                    case GEnum.PopupType.ItmStkID:
                    case GEnum.PopupType.ItmSalesID:
                    case GEnum.PopupType.ItmPurchaseID:
                    case GEnum.PopupType.ItmJobID:
                    case GEnum.PopupType.ItmPackID:
                    case GEnum.PopupType.ItmFSNVGid:
                    case GEnum.PopupType.ItmFSCid:
                    case GEnum.PopupType.ItmCid:
                    case GEnum.PopupType.ItmFSCANid:
                    case GEnum.PopupType.ItmFSCANVGid:
                    case GEnum.PopupType.ItmGid:
                    case GEnum.PopupType.ItmVid:
                    case GEnum.PopupType.ItmMid:
                    case GEnum.PopupType.ItmFSid:
                    case GEnum.PopupType.ItmFid:
                    case GEnum.PopupType.VendItmID:
                    case GEnum.PopupType.ItmOpenID:
                    case GEnum.PopupType.ShipNmOpenID:
                    case GEnum.PopupType.PriceInfoOpenID:
                    case GEnum.PopupType.VehicleID:
                        Identity.Checked = true;
                        break;

                    case GEnum.PopupType.CusNm:
                    case GEnum.PopupType.VendNm:
                    case GEnum.PopupType.JobDes:
                    case GEnum.PopupType.AccDes:
                    case GEnum.PopupType.AccLiabilityDes:
                    case GEnum.PopupType.ItmDes:
                    case GEnum.PopupType.ItmFinishDes:
                    case GEnum.PopupType.ItmStkDes:
                    case GEnum.PopupType.ItmSalesDes:
                    case GEnum.PopupType.ItmPurchaseDes:
                    case GEnum.PopupType.ItmJobDes:
                    case GEnum.PopupType.ItmPackDes:
                    case GEnum.PopupType.ItmFSNVGdes:
                    case GEnum.PopupType.ItmFSCdes:
                    case GEnum.PopupType.ItmCdes:
                    case GEnum.PopupType.ItmFSCANdes:
                    case GEnum.PopupType.ItmFSCANVGdes:
                    case GEnum.PopupType.ItmGdes:
                    case GEnum.PopupType.ItmVdes:
                    case GEnum.PopupType.ItmMdes:
                    case GEnum.PopupType.ItmFSdes:
                    case GEnum.PopupType.ItmFdes:
                    case GEnum.PopupType.Vehicle:
                        Description.Checked = true;
                        break;

                    default:
                        Identity.Checked = true;
                        break;

                }
                #endregion

                SearchText.SetValueTrigger(parmList[0], false);
                SearchText.Focus();
                //decimal? rowHeight = SysOptionUtility.GetDec("DatasheetHeightForRecordSearch");
                tagrdSearchResult.DisplayLayout.Bands[0].Override.RowSizing = RowSizing.Sychronized;
                //tagrdSearchResult.DisplayLayout.Bands[0].Override.DefaultRowHeight = GFunc.NEInt((rowHeight * 32),0);
                InitializeSearchParameter();
                Search(false);

                this.MaximizeBox = false;
                this.StartPosition = FormStartPosition.CenterScreen;
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
        private void frmPopup_Shown(object sender, EventArgs e)
        {
            try
            {
                if (formClose)
                {
                    this.Close();
                }
                else
                {
                    SearchText.Focus();
                    SearchText.Select(SearchText.Text.Length, SearchText.Text.Length);
                    SearchCombo1.TabStop = false;
                    SearchCombo2.TabStop = false;
                    SearchCombo3.TabStop = false;
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
        private void frmRecordSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                Control c = this.ActiveControl;
                if (e.KeyCode == Keys.Enter)
                {
                    if (c == tagrdSearchResult)
                    {
                        tsbSelect.PerformClick();
                        e.SuppressKeyPress = true;
                    }
                    else if (c.GetType() == typeof(TAUtil.TAComboBox) || c.GetType() == typeof(TAUtil.TATextBoxEditor)
                        || c.Parent.GetType() == typeof(TAUtil.TAComboBox) || c.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                    {
                        Search(true);
                        if (tagrdSearchResult.Focused && tagrdSearchResult.Rows.Count > 0)
                            tagrdSearchResult.Rows[0].Selected = true;

                        if (tagrdSearchResult.Rows.Count == 1)
                        {
                            tsbSelect_Click(null, null);
                        }
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                    this.Close();
                else
                    //to make other keys work
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed

        //Button Click Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.formClose = true;
            this.DialogResult = DialogResult.Cancel;
        }//Completed
        private void tsbSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                Search(true);
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
        private void tsbSelect_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                //Set Transfer Value to Properties such as Key, Id, Desc
                int key = 0, jobPhaseKey = 0, jobCostTypeKey = 0, jobTaskKey = 0;
                string id = string.Empty; string des = string.Empty;
                int checkCodeKey = 0;

                if (!GFunc.IsNE(tagrdSearchResult.ActiveRow))
                {
                    key = GFunc.NEInt(tagrdSearchResult.ActiveRow.Cells["Key"].Value, 0);
                    if (popupType != BOLib.GEnum.PopupType.Vehicle)
                    {
                        id = tagrdSearchResult.ActiveRow.Cells["ID"].Value.ToString();
                    }
                    des = tagrdSearchResult.ActiveRow.Cells["Des"].Value.ToString();

                    #region Check for record Access for Itm,Cust,Vend,Job
                    switch (popupType)
                    {
                        case GEnum.PopupType.CusID:
                        case GEnum.PopupType.CustOpenID:
                        case GEnum.PopupType.CusNm:
                            checkCodeKey = (int)GEnum.SystemCode.Customer;
                            break;

                        case GEnum.PopupType.VehicleID:
                        case GEnum.PopupType.VehicleOpenID:
                        case GEnum.PopupType.Vehicle:
                            checkCodeKey = (int)GEnum.SystemCode.Vehicle;
                            break;

                        case GEnum.PopupType.VendID:
                        case GEnum.PopupType.VendOpenID:
                        case GEnum.PopupType.VendNm:
                            checkCodeKey = (int)GEnum.SystemCode.Vendor;
                            break;

                        case GEnum.PopupType.JobID:
                        case GEnum.PopupType.JobOpenID:
                        case GEnum.PopupType.JobDes:
                            checkCodeKey = (int)GEnum.SystemCode.Job;
                            break;

                        case GEnum.PopupType.ItmID:
                        case GEnum.PopupType.ItmFinishID:
                        case GEnum.PopupType.ItmStkID:
                        case GEnum.PopupType.ItmSalesID:
                        case GEnum.PopupType.ItmPurchaseID:
                        case GEnum.PopupType.ItmJobID:
                        case GEnum.PopupType.ItmPackID:
                        case GEnum.PopupType.ItmFSNVGid:
                        case GEnum.PopupType.ItmFSCid:
                        case GEnum.PopupType.ItmCid:
                        case GEnum.PopupType.ItmFSCANid:
                        case GEnum.PopupType.ItmFSCANVGid:
                        case GEnum.PopupType.ItmGid:
                        case GEnum.PopupType.ItmVid:
                        case GEnum.PopupType.ItmMid:
                        case GEnum.PopupType.ItmFSid:
                        case GEnum.PopupType.ItmFid:
                        case GEnum.PopupType.VendItmID:
                        case GEnum.PopupType.ItmOpenID:
                        case GEnum.PopupType.ItmDes:
                        case GEnum.PopupType.ItmFinishDes:
                        case GEnum.PopupType.ItmStkDes:
                        case GEnum.PopupType.ItmSalesDes:
                        case GEnum.PopupType.ItmPurchaseDes:
                        case GEnum.PopupType.ItmJobDes:
                        case GEnum.PopupType.ItmPackDes:
                        case GEnum.PopupType.ItmFSNVGdes:
                        case GEnum.PopupType.ItmFSCdes:
                        case GEnum.PopupType.ItmCdes:
                        case GEnum.PopupType.ItmFSCANdes:
                        case GEnum.PopupType.ItmFSCANVGdes:
                        case GEnum.PopupType.ItmGdes:
                        case GEnum.PopupType.ItmVdes:
                        case GEnum.PopupType.ItmMdes:
                        case GEnum.PopupType.ItmFSdes:
                        case GEnum.PopupType.ItmFdes:
                            checkCodeKey = (int)GEnum.SystemCode.Inventory;
                            break;
                    }

                    //if (checkCodeKey == (int)GEnum.SystemCode.Customer && DocCodeKey != (int)GEnum.SystemCode.Quotation) /* commented by yst on 20190613 to check customer in QO like SO suggested by Jasmin */
                    if (checkCodeKey == (int)GEnum.SystemCode.Customer)
                    {
                        if (GFunc.NEBool(tagrdSearchResult.ActiveRow.Cells["ActiveWithProblem"].Value, false))
                        {
                            //added by nnt 27 Jun 2019 --allow active with problem customer to key in Payment Received - not show this msg in Payment Received.
                            if (CallerDocCodeKey != (int)GEnum.SystemCode.Payment_Received)
                            {
                                MsgBox.Show("Customer is not allowed to select. Please check with the account department.");
                                return;
                            }
                        }
                    }

                    if (checkCodeKey != 0)
                    {
                        if (!GFunc.CanAccessRecord(key, checkCodeKey))
                            return;
                    }
                    #endregion

                    if (this.RecordSelectedEvent != null)
                        this.RecordSelectedEvent.Invoke(key, id);

                    this.Key = key;
                    this.ID = id;
                    this.Des = des;
                    this.JobCostTypeKey = jobCostTypeKey;
                    this.JobPhaseKey = jobPhaseKey;
                    this.JobTaskKey = jobTaskKey;
                    this.DialogResult = DialogResult.OK;
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
            this.Close();
        }//Completed
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GFunc.IsNE(tagrdSearchResult.ActiveRow))
                {
                    int key = GFunc.NEInt(tagrdSearchResult.ActiveRow.Cells["Key"].Value, 0);
                    Assembly oAName = Assembly.LoadFile(Application.ExecutablePath.ToString());
                    Type typ = oAName.GetType("WinUI." + popupMSTFrmNm);
                    object formObj = null;
                    if (popupMSTFrmNm == "frmMSTItm")
                    {
                        formObj = Activator.CreateInstance(typ, new object[] { this.SearchText.Text, true });
                    }
                    else
                    {
                        formObj = Activator.CreateInstance(typ, new object[] { this.SearchText.Text });
                    }

                    formObj = Activator.CreateInstance(typ, new object[] { key });
                    Form f = (Form)formObj;
                    f.ShowDialog();

                    //refresh result grid 
                    Search(true);

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
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Assembly oAName = Assembly.LoadFile(Application.ExecutablePath.ToString());
                Type typ = oAName.GetType("WinUI." + popupMSTFrmNm);
                object formObj = null;
                if (popupMSTFrmNm == "frmMSTItm")
                {
                    formObj = Activator.CreateInstance(typ, new object[] { this.SearchText.Text, false });
                }
                else
                {
                    formObj = Activator.CreateInstance(typ, new object[] { this.SearchText.Text });
                }
                Form f = (Form)formObj;
                f.ShowDialog();

                //refresh result grid
                Search(true);

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
        private void grdPopups_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                tsbSelect.PerformClick();
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

        //Controls Events       
        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                Search(false);
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
        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ////alway perform search when press Enter Key
                //if (e.KeyCode == Keys.Enter)
                //{
                //    Search(true);

                //    if (tagrdSearchResult.Focused && tagrdSearchResult.Rows.Count > 0)
                //        tagrdSearchResult.Rows[0].Selected = true;

                //    if (tagrdSearchResult.Rows.Count == 1)
                //    {
                //        tsbSelect_Click(null, null);
                //    }
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
        }
        private void SearchCombo_CustomUpdate(object sender, CancelEventArgs e)
        {
            Search(false);
        }

        //Methods
        private void FormLayout()
        {
            try
            {
                #region Button Layout
                switch (DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Customer:
                    case (int)GEnum.SystemCode.Vehicle:
                    case (int)GEnum.SystemCode.Vendor:
                    case (int)GEnum.SystemCode.Job:
                    case (int)GEnum.SystemCode.Inventory:
                    case (int)GEnum.SystemCode.Account:
                        //tsbAdd.Enabled = false;
                        //tsbEdit.Enabled = false;
                        break;
                }
                #endregion

                #region Set Label Text and Group Option Check value
                switch (popupType)
                {
                    case GEnum.PopupType.CusID:
                    case GEnum.PopupType.CustOpenID:
                    case GEnum.PopupType.CusNm:
                        Identity.Text = "Customer ID  (&1)";
                        Description.Text = "Customer Name  (&2)";
                        popupMSTFrmNm = "frmMSTCon";
                        lblSearchCombo1.Text = "Class";
                        lblSearchCombo2.Text = "Industory ID";
                        lblSearchCombo3.Text = "Territory ID";
                        break;

                    case GEnum.PopupType.VendID:
                    case GEnum.PopupType.VendOpenID:
                    case GEnum.PopupType.VendNm:
                        Identity.Text = "Vendor ID  (&1)";
                        Description.Text = "Vendor Name  (&2)";
                        popupMSTFrmNm = "frmMSTCon";
                        lblSearchCombo1.Text = "Class";
                        lblSearchCombo2.Text = "Industory ID";
                        lblSearchCombo3.Text = "Territory ID";
                        break;

                    case GEnum.PopupType.JobID:
                    case GEnum.PopupType.JobOpenID:
                    case GEnum.PopupType.JobDes:
                        Identity.Text = "Job ID  (&1)";
                        Description.Text = "Job Desc  (&2)";
                        popupMSTFrmNm = "frmMSTJob";
                        lblSearchCombo1.Text = "Class";
                        lblSearchCombo2.Text = "Job Group ID";
                        lblSearchCombo3.Text = "Job Contact ID";
                        break;

                    case GEnum.PopupType.AccID:
                    case GEnum.PopupType.AccDisID:
                    case GEnum.PopupType.AccLiabilityID:
                    case GEnum.PopupType.AccOpenID:
                    case GEnum.PopupType.AccDes:
                    case GEnum.PopupType.AccLiabilityDes:
                        Identity.Text = "Account ID  (&1)";
                        Description.Text = "Account Name  (&2)";
                        popupMSTFrmNm = "frmMstAcc";
                        lblSearchCombo1.Text = "Acc Group ID";
                        lblSearchCombo2.Text = "Acc Type Des";
                        SearchCombo3.Visible = false;
                        lblSearchCombo3.Visible = false;
                        break;
                    case GEnum.PopupType.ItmID:
                    case GEnum.PopupType.ItmFinishID:
                    case GEnum.PopupType.ItmStkID:
                    case GEnum.PopupType.ItmSalesID:
                    case GEnum.PopupType.ItmPurchaseID:
                    case GEnum.PopupType.ItmJobID:
                    case GEnum.PopupType.ItmPackID:
                    case GEnum.PopupType.ItmFSNVGid:
                    case GEnum.PopupType.ItmFSCid:
                    case GEnum.PopupType.ItmCid:
                    case GEnum.PopupType.ItmFSCANid:
                    case GEnum.PopupType.ItmFSCANVGid:
                    case GEnum.PopupType.ItmGid:
                    case GEnum.PopupType.ItmVid:
                    case GEnum.PopupType.ItmMid:
                    case GEnum.PopupType.ItmFSid:
                    case GEnum.PopupType.ItmFid:
                    case GEnum.PopupType.VendItmID:
                    case GEnum.PopupType.ItmOpenID:
                    case GEnum.PopupType.ItmDes:
                    case GEnum.PopupType.ItmFinishDes:
                    case GEnum.PopupType.ItmStkDes:
                    case GEnum.PopupType.ItmSalesDes:
                    case GEnum.PopupType.ItmPurchaseDes:
                    case GEnum.PopupType.ItmJobDes:
                    case GEnum.PopupType.ItmPackDes:
                    case GEnum.PopupType.ItmFSNVGdes:
                    case GEnum.PopupType.ItmFSCdes:
                    case GEnum.PopupType.ItmCdes:
                    case GEnum.PopupType.ItmFSCANdes:
                    case GEnum.PopupType.ItmFSCANVGdes:
                    case GEnum.PopupType.ItmGdes:
                    case GEnum.PopupType.ItmVdes:
                    case GEnum.PopupType.ItmMdes:
                    case GEnum.PopupType.ItmFSdes:
                    case GEnum.PopupType.ItmFdes:
                        Identity.Text = "Item ID  (&1)";
                        Description.Text = "Item Name  (&2)";
                        popupMSTFrmNm = "frmMSTItm";
                        //Search Combo Labels //default
                        gbItemCates.Visible = true;
                        break;

                    case GEnum.PopupType.ShipNmOpenID:
                        Identity.Text = "Ship ID  (&1)";
                        Description.Text = "Ship Name  (&2)";
                        popupMSTFrmNm = "frmMSTShipName";
                        lblSearchCombo1.Text = "";
                        lblSearchCombo2.Text = "";
                        lblSearchCombo3.Text = "";
                        break;

                    case GEnum.PopupType.PriceInfoOpenID:
                        Identity.Text = "Price ID  (&1)";
                        Description.Text = "Price Name  (&2)";
                        popupMSTFrmNm = "frmMSTPriceInfo";
                        lblSearchCombo1.Text = "";
                        lblSearchCombo2.Text = "";
                        lblSearchCombo3.Text = "";
                        break;

                    case GEnum.PopupType.VehicleID:
                    case GEnum.PopupType.Vehicle:
                    case GEnum.PopupType.VehicleOpenID:
                        //Identity.Text = "Vehicle ID  (&1)";
                        Description.Text = "Vehicle";
                        popupMSTFrmNm = "frmMSTVehicle";
                        lblSearchCombo1.Text = "Customer Name";
                        lblSearchCombo2.Text = "Model";
                        lblSearchCombo3.Text = "Brand";
                        Identity.Visible = false;
                        tsbAdd.Visible = false;
                        tsbEdit.Visible = false;
                        break;

                    default:
                        break;

                }
                #endregion
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
        private void InitializeSearchParameter()
        {
            //Set SearchNumChar and SearchFromStart
            if (this.popupType.ToString().ToLower().StartsWith("itm") && this.popupType != GEnum.PopupType.ItmOpenID)
            {
                SearchNumChar = SysOptionUtility.SearchItemNumChar;
                if (this.popupType.ToString().ToLower().EndsWith("id"))
                {
                    if (SysOptionUtility.SearchItemMatchID == GEnum.SearchMatchOption.StartofField)
                        SearchFromStart = true;
                    else
                        SearchFromStart = false;
                }
                else if (this.popupType.ToString().ToLower().EndsWith("des"))
                {
                    if (SysOptionUtility.SearchItemMatch == GEnum.SearchMatchOption.StartofField)
                        SearchFromStart = true;
                    else
                        SearchFromStart = false;
                }
            }
            else if (this.popupType.ToString().ToLower().StartsWith("cus") && this.popupType != GEnum.PopupType.CustOpenID)
            {
                SearchNumChar = SysOptionUtility.SearchCustomerNumChar;
                if (SysOptionUtility.SearchCustomerMatch == GEnum.SearchMatchOption.StartofField)
                    SearchFromStart = true;
                else
                    SearchFromStart = false;
            }
            else if (this.popupType.ToString().ToLower().StartsWith("vend") && this.popupType != GEnum.PopupType.VendOpenID)
            {
                SearchNumChar = SysOptionUtility.SearchVendorNumChar;
                if (SysOptionUtility.SearchVendorMatch == GEnum.SearchMatchOption.StartofField)
                    SearchFromStart = true;
                else
                    SearchFromStart = false;
            }
            else if (this.popupType.ToString().ToLower().StartsWith("acc") && this.popupType != GEnum.PopupType.AccOpenID)
            {
                SearchNumChar = SysOptionUtility.SearchAccountNumChar;
                if (SysOptionUtility.SearchAccountMatch == GEnum.SearchMatchOption.StartofField)
                    SearchFromStart = true;
                else
                    SearchFromStart = false;
            }
            else if (this.popupType.ToString().ToLower().StartsWith("job") && this.popupType != GEnum.PopupType.JobOpenID)
            {
                SearchNumChar = SysOptionUtility.SearchJobNumChar;
                if (SysOptionUtility.SearchJobMatch == GEnum.SearchMatchOption.StartofField)
                    SearchFromStart = true;
                else
                    SearchFromStart = false;
            }
        }//Completed
        private void Search(Boolean ForceSearch)
        {
            //Perform Data retrival or Grid Filter
            DataTable dt = null;
            bool processOK = false;

            //Determine to retrive data from SP or just filter the current datasource
            if (SearchText.Text.Trim().Length >= SearchNumChar || ForceSearch)
            {
                if (ForceSearch && SPSearchValue == string.Empty)
                    PerformSPCall = true;
                else
                {
                    if (SearchFromStart)
                    {
                        if (SearchText.Text.Trim() == string.Empty || SearchText.Text.Trim().Length < SearchNumChar)
                            PerformSPCall = true;
                        else
                        {
                            if (SPSearchValue == SearchText.Text.Trim().Substring(0, SearchNumChar))
                                PerformSPCall = false;
                            else
                                PerformSPCall = true;
                        }
                    }
                    else
                    {
                        if (SearchText.Text.Contains(SPSearchValue) && SPSearchValue != "")
                            PerformSPCall = false;
                        else
                            PerformSPCall = true;
                    }
                }

                //tagrdSearchResult.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                ((DataTable)tagrdSearchResult.DataSource).DefaultView.RowFilter = "";

                if (PerformSPCall)
                {
                    SPSearchValue = SearchText.Text;

                    parmList[0] = SPSearchValue.Replace("*", "%");

                    //added by May on 16-Feb-2024, to only call SECRecAccess_GetData proc
                    if (this.listSettingId.ToLower().Contains("func"))
                        this.listSettingId = this.listSettingId.Replace("Func", "").Replace("func","");

                    if (Description.Checked) //Des Search 
                    {
                        if (this.listSettingId.Contains("_id"))
                            this.listSettingId = this.listSettingId.Replace("_id", "_des");

                        processOK = GFunc.GetPopupListDataWithParams(this.listSettingId, ref dt, parmList, true);
                    }
                    else if (Identity.Checked) //ID Search
                    {
                        if (this.listSettingId.Contains("_des"))
                            this.listSettingId = this.listSettingId.Replace("_des", "_id");

                        processOK = GFunc.GetPopupListDataWithParams(this.listSettingId, ref dt, parmList, false);
                    }

                    if (processOK)
                    {
                        if (popupType != GEnum.PopupType.JobID && popupType != GEnum.PopupType.JobDes && popupType != GEnum.PopupType.ShipNmOpenID && dt.Rows.Count > 0)
                        {
                            //check if current active form is master form?                           
                            switch (CallerDocCodeKey)
                            {
                                case (int)GEnum.SystemCode.Customer:
                                case (int)GEnum.SystemCode.Vendor:
                                case (int)GEnum.SystemCode.Vehicle:
                                case (int)GEnum.SystemCode.Inventory:
                                case (int)GEnum.SystemCode.Account:
                                    tagrdSearchResult.DataSource = dt;
                                    break;
                                default:
                                    dt.DefaultView.RowFilter = "InActive = false";
                                    if (dt.DefaultView.Count > 0)
                                    {
                                        //dt.DefaultView.RowFilter = "";
                                        //tagrdSearchResult.DataSource = dt.AsEnumerable().Where(r => r.Field<bool?>("InActive") == false).CopyToDataTable();
                                        tagrdSearchResult.DataSource = dt.DefaultView.ToTable();
                                    }
                                    break;
                            }
                        }
                        else
                            tagrdSearchResult.DataSource = dt;
                        tagrdSearchResult.DataBind();
                    }
                    else
                    {
                        MsgBox.Show("Error : Unable to retrive data from server, please try again later");
                    }
                }
                else
                {
                    string currSearchCol = "ID";
                    if (Description.Checked == true)
                        currSearchCol = "Des";
                    string searchstr = "";

                    if (SearchText.Text.Contains("*") || SearchText.Text.Contains("*"))
                    {
                        string[] searchstrs;

                        if (SearchText.Text.Contains("*"))
                            searchstrs = SearchText.Text.Replace('*', '%').Split('%');
                        else if (SearchText.Text.Contains("%"))
                            searchstrs = SearchText.Text.Split('%');
                        else
                            searchstrs = new string[] { SearchText.Text };

                        foreach (string s in searchstrs)
                        {
                            if (searchstr == "")
                                searchstr = currSearchCol + " like '%" + s + "%'";
                            else if (s != "")
                                searchstr += " and " + currSearchCol + " like '%" + s + "%'";
                        }
                    }
                    else
                    {
                        string wildcard = SearchFromStart ? "" : "%";
                        searchstr = currSearchCol + " like '" + wildcard + SearchText.Text.Trim() + "%'";
                    }

                    ((DataTable)tagrdSearchResult.DataSource).DefaultView.RowFilter = searchstr;

                    ////GridFilterToDefaultView   
                    //if (SearchFromStart)
                    //    //tagrdSearchResult.DisplayLayout.Bands[0].ColumnFilters[currSearchCol].FilterConditions.Add(FilterComparisionOperator.StartsWith, SearchText.Text.Trim());
                    //    ((DataTable)tagrdSearchResult.DataSource).DefaultView.RowFilter = currSearchCol + " like '" + SearchText.Text.Trim() + "%'";
                    //else
                    //    //tagrdSearchResult.DisplayLayout.Bands[0].ColumnFilters[currSearchCol].FilterConditions.Add(FilterComparisionOperator.Contains, SearchText.Text.Trim());
                    //    ((DataTable)tagrdSearchResult.DataSource).DefaultView.RowFilter = currSearchCol + " like '%" + SearchText.Text.Trim() + "%'";
                }
            }
            else if (tagrdSearchResult.DataSource != null)
                ((DataTable)tagrdSearchResult.DataSource).DefaultView.RowFilter = "";
            else
                return;

            //Additional filter 3 combo boxes added on 29-Aug-2012            
            //INClass, CatID1, BrandID for MST_Itm
            //CClass, CIndustryID, CTerritoryID for Customer
            //VClass, VIndustryID, VTerritoryID for Vendor
            //AccGrpID, AccTypeDes, AccCurrID for Account
            //JobClass, JobGrpID, JobConID for Job

            string filterStr = ((DataTable)tagrdSearchResult.DataSource).DefaultView.RowFilter;
            filterStr += GetComboFilter(SearchCombo1);
            filterStr += GetComboFilter(SearchCombo2);
            filterStr += GetComboFilter(SearchCombo3);

            if(popupType == GEnum.PopupType.ItmID || popupType == GEnum.PopupType.ItmDes)
            {
                if(cboCat2.Text!="")
                    filterStr += " AND CatID2='" + cboCat2.Text + "'";
                if (cboCat3.Text != "")
                    filterStr += " AND CatID3='" + cboCat3.Text + "'";
                if (cboCat4.Text != "")
                    filterStr += " AND CatID4='" + cboCat4.Text + "'";
                if (cboCat5.Text != "")
                    filterStr += " AND CatID5='" + cboCat5.Text + "'";
            }

            if (filterStr.StartsWith(" AND"))//If SP call, no filter before Additional filter
                filterStr = filterStr.Substring(5);
            ((DataTable)tagrdSearchResult.DataSource).DefaultView.RowFilter = filterStr;

            if (SysOptionUtility.DatabaseBranchCode == "BHM" && (popupType == GEnum.PopupType.ItmID || popupType == GEnum.PopupType.ItmDes)
                && tagrdSearchResult.DisplayLayout.Bands[0].Columns.Exists("Slow Moving Goods") )
            {
                tagrdSearchResult.DisplayLayout.Override.RowFilterAction = RowFilterAction.AppearancesOnly;
                tagrdSearchResult.DisplayLayout.Override.FilteredInCellAppearance.BackColor = Color.LightGreen;

                tagrdSearchResult.DisplayLayout.Bands[0].ColumnFilters["Slow Moving Goods"].FilterConditions.Add(FilterComparisionOperator.Equals, true);               
            }

        }//Completed 

        //private void SetColorSlowMovingGoods()
        //{
        //    if (tagrdSearchResult.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < tagrdSearchResult.Rows.Count; i++)
        //        {
        //            if (tagrdSearchResult.Rows[i].Cells["Slow Moving Goods"].Value.ToString().ToUpper() == "TRUE")
        //            {
        //                tagrdSearchResult.Rows[i].Appearance.BackColor = Color.Green;
        //                tagrdSearchResult.Rows[i].Cells["Slow Moving Goods"].Value = true;

        //            }
        //            else tagrdSearchResult.Rows[i].Cells["Slow Moving Goods"].Value = false;
        //        }
        //    }

        //}

        private string GetComboFilter(TAUtil.TAComboBox cbo)
        {
            string filterStr = "";
            string fieldNm = "";
            bool exactMatch = false;

            if (cbo.Text.Trim() != "")
            {
                if (cbo.IsItemInList(cbo.Text))
                    exactMatch = true;
                int comboNumber = Convert.ToInt32(cbo.Name.Substring(cbo.Name.Length - 1));

                switch (comboNumber)
                {
                    case 1:
                        if (popupType == GEnum.PopupType.ItmID || popupType == GEnum.PopupType.ItmDes)
                            fieldNm = "INClass";
                        else if (popupType == GEnum.PopupType.CusID || popupType == GEnum.PopupType.CusNm)
                            fieldNm = "CClass";
                        else if (popupType == GEnum.PopupType.VendID || popupType == GEnum.PopupType.VendNm)
                            fieldNm = "VClass";
                        else if (popupType == GEnum.PopupType.AccID || popupType == GEnum.PopupType.AccDes)
                            fieldNm = "AccGrpID";
                        else if (popupType == GEnum.PopupType.JobID || popupType == GEnum.PopupType.JobDes)
                            fieldNm = "JobClass";
                        else if (popupType == GEnum.PopupType.VehicleID || popupType == GEnum.PopupType.Vehicle)
                            fieldNm = "ConNm";
                        break;
                    case 2:
                        if (popupType == GEnum.PopupType.ItmID || popupType == GEnum.PopupType.ItmDes)
                            fieldNm = "CatID1";
                        else if (popupType == GEnum.PopupType.CusID || popupType == GEnum.PopupType.CusNm)
                            fieldNm = "CIndustryID";
                        else if (popupType == GEnum.PopupType.VendID || popupType == GEnum.PopupType.VendNm)
                            fieldNm = "VIndustryID";
                        else if (popupType == GEnum.PopupType.AccID || popupType == GEnum.PopupType.AccDes)
                            fieldNm = "AccTypeDes";
                        else if (popupType == GEnum.PopupType.JobID || popupType == GEnum.PopupType.JobDes)
                            fieldNm = "JobGrpID";
                        else if (popupType == GEnum.PopupType.VehicleID || popupType == GEnum.PopupType.Vehicle)
                            fieldNm = "Model";
                        break;
                    case 3:
                        if (popupType == GEnum.PopupType.ItmID || popupType == GEnum.PopupType.ItmDes)
                            fieldNm = "BrandID";
                        else if (popupType == GEnum.PopupType.CusID || popupType == GEnum.PopupType.CusNm)
                            fieldNm = "CTerritoryID";
                        else if (popupType == GEnum.PopupType.VendID || popupType == GEnum.PopupType.VendNm)
                            fieldNm = "VTerritoryID";
                        else if (popupType == GEnum.PopupType.AccID || popupType == GEnum.PopupType.AccDes)
                            fieldNm = "AccCurrID";
                        else if (popupType == GEnum.PopupType.JobID || popupType == GEnum.PopupType.JobDes)
                            fieldNm = "JobConID";
                        else if (popupType == GEnum.PopupType.VehicleID || popupType == GEnum.PopupType.Vehicle)
                            fieldNm = "Brand";
                        break;
                }

                if (exactMatch)
                    filterStr += " AND " + fieldNm + "='" + cbo.Text + "'";
                else
                    filterStr = " AND " + fieldNm + " Like'%" + cbo.Text + "%'";
            }

            return filterStr;
        }

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

        private void PaintBorderlessGroupBox(object sender, PaintEventArgs p)
        {
            GroupBox box = (GroupBox)sender;
            p.Graphics.Clear(SystemColors.Window);
            p.Graphics.DrawString(box.Text, box.Font, Brushes.Black, 0, 0);
        }

        //add nnt on 4th March 2020
        //private void tagrdSearchResult_InitializeRow(object sender, InitializeRowEventArgs e)
        //{
        //    try
        //    {
        //        if (SysOptionUtility.DatabaseBranchCode == "BHM")
        //        {


        //            foreach (UltraGridCell cell in e.Row.Cells)
        //            {

        //                if (e.Row.Cells.Exists("Slow Moving Goods"))
        //                {
        //                    if (e.Row.Cells["Slow Moving Goods"].Value.ToString().ToUpper() == "TRUE")
        //                    {
        //                        e.Row.Cells["Slow Moving Goods"].Value = true;
        //                        cell.Appearance.BackColor = Color.LightGreen;
        //                    }
        //                    else e.Row.Cells["Slow Moving Goods"].Value = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex, false);
        //    }
        //}
    }   
}

