using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Infragistics.Win.UltraWinTabbedMdi;
using BOLib;
using System.Threading;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TAUtil;
using WinUI.Forms;
using Infragistics.Win;
using System.Data.SqlClient;
using System.IO;

namespace WinUI
{
    public partial class frmMain : Form
    {
        #region +++ Global variable declaration +++

        public static frmMain gfrmMain;
        private MdiTab tabFirst;

        //Added by May
        private Form formToActivate=null;
        public string QOID = "";

        #endregion

        public frmMain()
        {
            InitializeComponent();
            //Initialize Global COntext Menu for all forms
            GlobalUI.cmnuGlobal_Initialize();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {               

                MdiClient ctlMDI;
                foreach (Control ctl in this.Controls)
                {
                    try
                    {
                        // Attempt to cast the control to type MdiClient.
                        ctlMDI = (MdiClient)ctl;

                        // Set the BackColor of the MdiClient control.
                        ctlMDI.BackColor = this.BackColor;
                    }
                    catch (InvalidCastException exc)
                    {
                        // Catch and ignore the error if casting failed.
                    }
                }

                frmMain.gfrmMain = this;
                //utolmgrMain.Toolbars[0].Tools["ADL"].SharedProps.Visible = true;
                Login();               

                Version v = Assembly.GetExecutingAssembly().GetName().Version;

                //Check to see if we are ClickOnce Deployed.

                //i.e. the executing code was installed via ClickOnce

                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {

                    //Collect the ClickOnce Current Version

                    v = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;

                }

                //Show the version in a simple manner

                this.Text = this.Text; //+ string.Format(" Version: {0}", v);

                if (QOID != "")
                {
                    GlobalUI.OpenQOForm(QOID);
                }
               if(SysOptionUtility.GetBool("UseWorkOrder")==false)
                    utolmgrMain.Toolbars[0].Tools["WorkOrder"].SharedProps.Visible = false;                
               
            }
            catch (TAException tex)
            {
                Error(tex, true,false);
            }
            catch (Exception ex)
            {
                Error(ex, true,false);
            }            
        }

        public void ShowQOForm(string QOID)
        {
            GlobalUI.OpenQOForm(QOID);
        }

        //private static void ShowForm()
        //{
           
        //        frmUpdater fUpdateInfo = new frmUpdater();
        //        fUpdateInfo.ShowDialog();
            
        //}

        private void utolmgrMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {                
                switch (e.Tool.Key)
                {
                    #region FILE
                    case "Switch_User":
                        if (this.LogOff())
                            Login();
                        break;

                    case "Exit_":                        
                        this.Close();
                        break;

                    #endregion



                    #region REFERENCE
                    //Inventory
                    case "Unit of Measure":
                        frmREFUOM frmUOM = new frmREFUOM();
                        frmUOM.MdiParent = this;
                        frmUOM.Show();
                        break;
                    case "Category":
                        frmREFCat fREFCat = new frmREFCat();
                        fREFCat.MdiParent = this;
                        fREFCat.Show();
                        break;
                    case "Brand":
                        frmREFBrand frmBrand = new frmREFBrand();
                        frmBrand.MdiParent = this;
                        frmBrand.Show();
                        break;
                    case "Location":
                        frmREFLoc fREFLoc = new frmREFLoc();
                        fREFLoc.MdiParent = this;
                        fREFLoc.Show();
                        break;
                    case "Color":
                        frmREFColor fREFColor = new frmREFColor();
                        fREFColor.MdiParent = this;
                        fREFColor.Show();
                        break;
                    case "Scale":
                        frmREFScale frmScale = new frmREFScale();
                        frmScale.MdiParent = this;
                        frmScale.Show();
                        break;
                    //Customer, Vendor
                    case "Term":
                        frmREFTerm fREFTerm = new frmREFTerm();
                        fREFTerm.MdiParent = this;
                        fREFTerm.Show();
                        break;
                    case "Packing Type":
                        frmREFPackingType fREFPackingType = new frmREFPackingType();
                        fREFPackingType.MdiParent = this;
                        fREFPackingType.Show();
                        break;
                    case "Ship Via":
                        frmREFShipVia fREFShipVia = new frmREFShipVia();
                        fREFShipVia.MdiParent = this;
                        fREFShipVia.Show();
                        break;
                    case "Industry":
                        frmREFIndustry fREFIndustry = new frmREFIndustry();
                        fREFIndustry.MdiParent = this;
                        fREFIndustry.Show();
                        break;
                    case "Territory":
                        frmREFTerritory fREFTerritory = new frmREFTerritory();
                        fREFTerritory.MdiParent = this;
                        fREFTerritory.Show();
                        break;
                    case "In&terest":
                        frmREFInterest fREFInterest = new frmREFInterest();
                        fREFInterest.MdiParent = this;
                        fREFInterest.Show();
                        break;
                    //Job
                    case "Job Group":
                        frmREFJobGrp fREFJobGrp = new frmREFJobGrp();
                        fREFJobGrp.MdiParent = this;
                        fREFJobGrp.Show();
                        break;
                    case "Job Cost":
                        frmREFJobCostType fREFJobCostType = new frmREFJobCostType();
                        fREFJobCostType.MdiParent = this;
                        fREFJobCostType.Show();
                        break;
                    case "Job Phase":
                        frmREFJobPhase fREFJobPhase = new frmREFJobPhase();
                        fREFJobPhase.MdiParent = this;
                        fREFJobPhase.Show();
                        break;
                    case "Job Task":
                        frmREFJobTask fREFJobTask = new frmREFJobTask();
                        fREFJobTask.MdiParent = this;
                        fREFJobTask.Show();
                        break;
                    //Account
                    case "Account Group":
                        frmREFAccGrp frmAccGrp = new frmREFAccGrp();
                        frmAccGrp.MdiParent = this;
                        frmAccGrp.Show();
                        break;
                    case "Currency":
                        frmREFCurr frmCurr = new frmREFCurr();
                        frmCurr.MdiParent = this;
                        frmCurr.Show();
                        break;
                    case "Overhead":
                        frmREFOverHead fREFOverHead = new frmREFOverHead();
                        fREFOverHead.MdiParent = this;
                        fREFOverHead.Show();
                        break;
                    case "Bank":
                        frmREFBank fREFBank = new frmREFBank();
                        fREFBank.MdiParent = this;
                        fREFBank.Show();
                        break;
                    case "Pay Mode":
                        frmREFPayMode fREFPayMode = new frmREFPayMode();
                        fREFPayMode.MdiParent = this;
                        fREFPayMode.Show();
                        break;
                    case "Tax":
                        frmREFTaxA frmTaxA = new frmREFTaxA();
                        frmTaxA.MdiParent = this;
                        frmTaxA.Show();
                        break;
                    case "Tax Group":
                        frmREFTaxGrp frmTaxGrp = new frmREFTaxGrp();
                        frmTaxGrp.MdiParent = this;
                        frmTaxGrp.Show();
                        break;
                    #endregion

                    #region MASTER
                    case "CustVendRec":
                        frmMSTCon fMstCon = new frmMSTCon(GEnum.SystemCode.Customer);
                        fMstCon.MdiParent = this;
                        fMstCon.Show();
                        break;
                    case "Customer Opening Balance":
                        frmMSTConOpenBal openBalCustomer = new frmMSTConOpenBal(GEnum.SystemCode.AR_Opening_Balance);
                        openBalCustomer.MdiParent = this;
                        openBalCustomer.Show();
                        break;
                    case "Customer Opening Balance (Cash)":
                        frmMSTConOpenBal openBalCustomercash = new frmMSTConOpenBal(GEnum.SystemCode.AR_Cash_Opening_Balance);
                        openBalCustomercash.MdiParent = this;
                        openBalCustomercash.Show();
                        break;
                    case "Vendor":
                        frmMSTCon fMstVen = new frmMSTCon(GEnum.SystemCode.Vendor);
                        fMstVen.MdiParent = this;
                        fMstVen.Show();
                        break;
                    case "Vendor Opening Balance":
                        frmMSTConOpenBal openBalVendor = new frmMSTConOpenBal(GEnum.SystemCode.AP_Opening_Balance);
                        openBalVendor.MdiParent = this;
                        openBalVendor.Show();
                        break;
                    case "Sales Representative":
                        frmMstSalesRep fMSTSalesRep = new frmMstSalesRep();
                        fMSTSalesRep.MdiParent = this;
                        fMSTSalesRep.Show();
                        break;
                    case "Ship Name":
                        frmMSTShipName frm = new frmMSTShipName();
                        frm.MdiParent = this;
                        frm.Show();
                        break;
                    case "Job":
                        frmMSTJob fMSTJob = new frmMSTJob();
                        fMSTJob.MdiParent = this;
                        fMSTJob.Show();
                        break;
                    case "InventoryItem":
                        frmMSTItm fMSTItm = new frmMSTItm();
                        fMSTItm.MdiParent = this;
                        fMSTItm.Show();
                        break;
                    case "ItmOpeningBal":
                        frmMSTItmOpeningBal fItmOpeningBal = new frmMSTItmOpeningBal();
                        fItmOpeningBal.MdiParent = this;
                        fItmOpeningBal.Show();
                        break;
                    case "Item Stock Count":
                        frmItemStockTake itmStock = new frmItemStockTake();
                        itmStock.MdiParent = this;
                        itmStock.Show();
                        break;
                    case "InventoryBalance":
                        frmMSTItmInvBal fMSTItmBal = new frmMSTItmInvBal();
                        fMSTItmBal.MdiParent = this;
                        fMSTItmBal.Show();
                        break;
                    case "KittingAssembly":
                        frmMSTItmKittAssBal fMSTItmKittAssBal = new frmMSTItmKittAssBal();
                        fMSTItmKittAssBal.MdiParent = this;
                        fMSTItmKittAssBal.Show();
                        break;
                    case "Price Information":
                        frmMSTPriceInfo fMSTPriceInfo = new frmMSTPriceInfo();
                        fMSTPriceInfo.MdiParent = this;
                        fMSTPriceInfo.Show();
                        break;
                    case "Price Update":
                        frmMSTPriceInfoUpdate frmMSTPriceInfoUpdate = new frmMSTPriceInfoUpdate(GEnum.SystemCode.Price_Update);
                        frmMSTPriceInfoUpdate.MdiParent = this;
                        frmMSTPriceInfoUpdate.Show();
                        break;
                    case "Account":
                        frmMstAcc fMSTAcc = new frmMstAcc();
                        fMSTAcc.MdiParent = this;
                        fMSTAcc.Show();
                        break;
                    case "Chart Of Account Opening Balance":
                        frmMSTAccOpenBalSelect frmMSTAccOpenBalSelect = new frmMSTAccOpenBalSelect();
                        frmMSTAccOpenBalSelect.MdiParent = this;
                        frmMSTAccOpenBalSelect.Show();
                        break;
                    case "Branch":
                        frmMstAccBranch fMstAccBranch = new frmMstAccBranch();
                        fMstAccBranch.MdiParent = this;
                        fMstAccBranch.Show();
                        break;
                    case "Dept":
                        frmMstAccDept fMSTAccDept = new frmMstAccDept();
                        fMSTAccDept.MdiParent = this;
                        fMSTAccDept.Show();
                        break;
                    case "TranGrp":
                        frmMstAccTranGrp fMstAccTranGrp = new frmMstAccTranGrp();
                        fMstAccTranGrp.MdiParent = this;
                        fMstAccTranGrp.Show();
                        break;
                    case "Budget/Target":
                        frmMSTBudget fBudget = new frmMSTBudget();
                        fBudget.MdiParent = this;
                        fBudget.Show();
                        break;
                    case "Key Customer":
                        frmKeyCustomer fkeycustomer = new frmKeyCustomer();
                        fkeycustomer.MdiParent = this;
                        fkeycustomer.Show();
                        break;
                    case "Import Key Customer":
                        frmKeyCustomersImport fKeyCustomersImport = new frmKeyCustomersImport();
                        fKeyCustomersImport.MdiParent = this;
                        fKeyCustomersImport.Show();
                        break;


                    #endregion

                    #region SALES                   
                   
                    case "_Quotation":
                        frmARQO fARQO = new frmARQO(GEnum.SystemCode.Quotation);
                        fARQO.MdiParent = this;
                        fARQO.Show();
                        break;
                    case "_SalesOrder":
                        frmARSO fARSO = new frmARSO(GEnum.SystemCode.Sales_Order);
                        fARSO.MdiParent = this;
                        fARSO.Show();
                        break;
                    //added the following by thettm on 29 jan 2018 (start)
                    case "Cash Sales Order":
                        frmARSO fARCSO = new frmARSO(GEnum.SystemCode.Sales_Order,true);
                        fARCSO.MdiParent = this;
                        fARCSO.Show();
                        break;                    
                    #endregion

                    #region Help
                    case "About":
                        frmAboutBox about = new frmAboutBox();
                        about.ShowDialog(this);
                        break;
                    #endregion

                   

                    default:
                        break;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        
        public bool IsExistingForm(Form objectForm)
        {
            if (objectForm != null && utabmdiMain.ActiveTab != null)
                foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                {
                    if (tab.Form.GetType().Equals(objectForm.GetType()))
                    {
                        tab.Form.Focus();
                        objectForm = null;
                        return true;
                    }
                }
            return false;
        }

        public Form ExistingForm(string sFormName)
        {
            try
            {
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                Form objectForm = null;

                if (sFormName.Equals("frmFinRepDesigner"))
                    objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + sFormName, true, new BindingFlags(), null, new object[] { 0 }, null, null);
                else
                    objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + sFormName);

                if (objectForm != null && utabmdiMain.ActiveTab != null)
                    foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                    {
                        if (tab.Form.GetType().Equals(objectForm.GetType()))
                        {
                            tab.Form.Focus();
                            objectForm.Close();
                            objectForm.Dispose();
                            return tab.Form;
                        }
                    }
                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }            
        }

        public Form ExistingForm(string sFormName, int CodeKey, int DocKey)
        {
            try
            {
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                Form objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + sFormName);

                if (objectForm != null && utabmdiMain.ActiveTab != null)
                    foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                    {
                        if (tab.Form.GetType().Equals(objectForm.GetType()))
                        {

                            if ((int)GFunc.GetMemberValue("OpenCode", tab.Form) == CodeKey)
                            {
                                Hashtable details;
                                Document objDoc = null;
                                Form f = frmMain.gfrmMain.ActiveMdiChild;
                                try
                                {
                                    ((DocInterface)f).GetDocInfor(out objDoc, out details);

                                }
                                catch
                                {
                                    //Some forms will not be able to cast to DocInterface. That's ok
                                }
                                if ((int)objDoc.DocKey != DocKey)
                                    continue;

                                tab.Form.Focus();
                                objectForm.Close();
                                objectForm.Dispose();
                                return tab.Form;
                            }
                        }
                    }
                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }            
        }
        public Form ExistingPrintOutForm(int CodeKey, int DocKey)
        {
            try
            {
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                Form rpxVeiwerForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI.frmRpxViewer");
                Form rptVeiwerForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI.frmReportViewer");

                if (rpxVeiwerForm != null && utabmdiMain.ActiveTab != null && rptVeiwerForm!=null)
                    foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                    {
                        if (tab.Form.GetType().Equals(rpxVeiwerForm.GetType()) || tab.Form.GetType().Equals(rptVeiwerForm.GetType()))
                        {
                            if ((int)GFunc.GetMemberValue("DocCodeKey", tab.Form) == CodeKey && (int)GFunc.GetMemberValue("DocKey", tab.Form)==DocKey)
                            {                                
                                Hashtable details;
                                Document objDoc = null;
                                Form f = frmMain.gfrmMain.ActiveMdiChild;

                                if (f.Name != rpxVeiwerForm.Name && f.Name != rptVeiwerForm.Name && f.Name!="frmFinRepDesigner" && f.Name != "frmARIVConsolidate")
                                {
                                    try
                                    {
                                        ((DocInterface)f).GetDocInfor(out objDoc, out details);
                                    }
                                    catch
                                    {
                                        //Some forms will not be able to cast to DocInterface. That's ok
                                    }
                                    if ((int)objDoc.DocKey != DocKey)
                                        continue;
                                }
                             
                                tab.Form.Focus();
                                rpxVeiwerForm.Close();
                                rpxVeiwerForm.Dispose();
                                rptVeiwerForm.Close();
                                rptVeiwerForm.Dispose();
                                return tab.Form;
                            }
                        }
                    }
                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public Form ExistingFinStatementForm(int CodeKey, int DocKey,int ReportPeriod)
        {
            try
            {                
                if (utabmdiMain.ActiveTab != null)
                    foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                    {
                        if (tab.Form.Name.Equals("frmRpxViewer"))
                        {
                            if ((int)GFunc.GetMemberValue("DocCodeKey", tab.Form) == CodeKey && (int)GFunc.GetMemberValue("DocKey", tab.Form) == DocKey
                                && (int)GFunc.GetMemberValue("ReportPeriod", tab.Form) == ReportPeriod)
                            {  
                                tab.Form.Focus();                                                  
                                return tab.Form;
                            }
                        }
                    }
                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public Form ExistingContactForm(string sFormName, int CodeKey)
        {
            try
            {
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                Form objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + sFormName);

                if (objectForm != null && utabmdiMain.ActiveTab != null)
                    foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                    {
                        if (tab.Form.Name.Equals(sFormName))
                        {
                            if ((int)GFunc.GetMemberValue("CodeKey", tab.Form) == CodeKey)
                            {
                                tab.Form.Focus();
                                objectForm.Close();
                                objectForm.Dispose();
                                return tab.Form;
                            }
                        }
                    }
                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }            
        }
        public Form ExistingDocListForm(int DocCodeKey)
        {
            try
            {
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                Form objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + GlobalUI.Form_Name.FRM_DOCUMENT_LIST);

                if (objectForm != null && utabmdiMain.ActiveTab != null)
                    foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                    {
                        if (tab.Form.GetType().Equals(objectForm.GetType()))
                        {
                            if (((frmDocList)tab.Form).DocCodeKey != DocCodeKey)
                                continue;
                            tab.Form.Focus();
                            objectForm.Close();
                            objectForm.Dispose();
                            return tab.Form;
                        }
                    }
                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }            
        }
        public Form ExistingDocListForm(int DocCodeKey, Guid key)
        {
            try
            {
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                Form objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + GlobalUI.Form_Name.FRM_DOCUMENT_LIST);

                if (objectForm != null && utabmdiMain.ActiveTab != null)
                    foreach (MdiTab tab in utabmdiMain.ActiveTab.TabGroup.Tabs)
                    {
                        if (tab.Form.GetType().Equals(objectForm.GetType()))
                        {
                            if (((frmDocList)tab.Form).DocCodeKey != DocCodeKey)
                            {
                                continue;
                            }
                            else
                            {
                                tab.Form.Focus();
                                objectForm.Close();
                                objectForm.Dispose();
                                return tab.Form;
                            }
                        }
                    }
                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
            
        }
        public void ShowExistingPopupForm(string sFormName)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--) //Popup forms are always the last index
            {                
                if (Application.OpenForms[i].Name.ToLower() == sFormName.ToLower())
                    Application.OpenForms[i].Activate();                     
            }     
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
                this.Tag = null;
                if (GlobalUI.CloseOpenForms() == false)
                {
                    e.Cancel = true;
                    return;
                }
               
                e.Cancel = false;
            
        }

        public void SetNotifyStatus(string msg)
        {
            this.ustsbarMain.Panels["ChildStatus"].Text =msg;            
            this.ustsbarMain.Panels["ChildStatus"].Appearance.FontData.Bold = DefaultableBoolean.True;
            this.ustsbarMain.Panels["ChildStatus"].Appearance.FontData.SizeInPoints = 11;
            this.ustsbarMain.Panels["ChildStatus"].Appearance.ForeColor = System.Drawing.Color.DarkRed;//System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            frmMain.gfrmMain.Refresh();
        }

        public void SetNormalStaus(string msg)
        {
            this.ustsbarMain.Panels["ChildStatus"].Text = msg;
           // this.ustsbarMain.Panels["ChildStatus"].Appearance = this.ustsbarMain.Panels["Status"].Appearance;
            this.ustsbarMain.Panels["ChildStatus"].Appearance.FontData.Bold = DefaultableBoolean.False;
            this.ustsbarMain.Panels["ChildStatus"].Appearance.FontData.SizeInPoints = this.ustsbarMain.Panels["Status"].Appearance.FontData.SizeInPoints;
            this.ustsbarMain.Panels["ChildStatus"].Appearance.ForeColor = this.ustsbarMain.Panels["Status"].Appearance.ForeColor;
            frmMain.gfrmMain.Refresh();
        }

        private void utabmdiMain_TabMoved(object sender, MdiTabEventArgs e)
        {
            if (GFunc.CompareString(this.utabmdiMain.TabGroups[0].FirstDisplayedTab.TextResolved, "Navigator") == false &&
                GFunc.CompareString(this.utabmdiMain.TabGroups[0].Tabs[0].TextResolved, "Navigator") == false &&
                this.tabFirst != null &&
                this.tabFirst.TextResolved != string.Empty)
            {
                this.utabmdiMain.BeginUpdate();
                this.tabFirst.Reposition(this.utabmdiMain.TabGroups[0].FirstDisplayedTab, MdiTabPosition.First);
                this.utabmdiMain.EndUpdate();
            }
        }

        public void utabmdiMain_TabClosing(object sender, Infragistics.Win.UltraWinTabbedMdi.CancelableMdiTabEventArgs e)
        {
            if (e.Tab.Form.Name.Equals("frmRpxViewer"))
            {
                frmRpxViewer f = (frmRpxViewer)e.Tab.Form;
                if (f.DocCodeKey == GEnum.SystemCode.Financial_Statement)
                {
                    formToActivate = gfrmMain.ExistingForm("frmFinRepDesigner");
                    if(formToActivate==null)
                        formToActivate = gfrmMain.ExistingForm("FrmFinRepMain");
                }
            }
            else if (e.Tab.Form.Name.Equals("frmFinRepDesigner"))
            {
                formToActivate = gfrmMain.ExistingForm("FrmFinRepMain");
            }
        }
        void utabmdiMain_TabClosed(object sender, Infragistics.Win.UltraWinTabbedMdi.MdiTabEventArgs e)
        {
            if (formToActivate != null)
                formToActivate.Focus();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Log Off the user when exit from application
            if (AppInfor.SecurityKey != Guid.Empty)
            {
                this.LogOff();                
                Application.Exit();
                //Process.Start("Boss.exe");
                Process.GetCurrentProcess().Kill();
            }
        }

        private void Login()
        {
            try
            {
                DialogResult dlgResult = DialogResult.None;

                //Get new Login form instance
                frmLogin frm = new frmLogin();
                //this.Hide();

                //_
                while (dlgResult == DialogResult.None)
                    dlgResult = frm.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {

                    this.Show();
                    this.Text = "Boss Accounting Software (" + SysOptionUtility.GetStr("CompanyName") + ")";
                    List<SqlParameter> list = new List<SqlParameter>();
                    list.Add(new SqlParameter("@Option", 1));
                    list.Add(new SqlParameter("@EmID", AppInfor.CurrentUserID));
                    DataTable dt = GFunc.ExecuteProc("MSTUNIT_GET", list);

                    this.utolmgrMain.Tools["MRO"].SharedProps.Visible = false;                    
                    this.utolmgrMain.Tools["ReserveOrder"].SharedProps.Visible = SysOptionUtility.DatabaseBranchCode == "BHM"; /* added by YST on 2023/03/07 becuause RO was added becuse of WMS using for BHM only  */

                    if (dt.Rows.Count > 0)
                    {                        
                        if (GFunc.NEStr(dt.Rows[0]["Team"], "") == "MRO" || GFunc.NEStr(dt.Rows[0]["AccessTeam"], "") == "ALL")
                            this.utolmgrMain.Tools["MRO"].SharedProps.Visible = true;   
                    }
                    if (SysOptionUtility.DatabaseBranchCode == "ADL")
                        utolmgrMain.Toolbars[0].Tools["ADL"].SharedProps.Visible = true;
                    else
                        utolmgrMain.Toolbars[0].Tools["ADL"].SharedProps.Visible = false;

                    //((Infragistics.Win.UltraWinToolbars.PopupMenuTool)utolmgrMain.Toolbars[0].Tools["Purchase"].SharedProps.RootTool).Tools["PYListToRequest"].SharedProps.Visible = SECPermUtility.Perform("PaymentApprovalRequest", false);
                    //((Infragistics.Win.UltraWinToolbars.PopupMenuTool)utolmgrMain.Toolbars[0].Tools["Purchase"].SharedProps.RootTool).Tools["PYListToVerify"].SharedProps.Visible = SECPermUtility.Perform("PaymentVerify", false);
                    //((Infragistics.Win.UltraWinToolbars.PopupMenuTool)utolmgrMain.Toolbars[0].Tools["Purchase"].SharedProps.RootTool).Tools["PYListToApprove"].SharedProps.Visible = SECPermUtility.Perform("PaymentApproval", false);
                }
                else if (dlgResult == DialogResult.Cancel)
                    this.Close();
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            

            // frm.Close();

        }

        public bool LogOff()
        {
            try
            {
                string msgID = string.Empty;
                // Logout Process 
                foreach (Form frm in this.MdiChildren)
                {
                    if (frm != null)
                    {
                        frm.Activate();
                        frm.Close();
                        if (frmMain.gfrmMain.Tag != null &&
                            (string)frmMain.gfrmMain.Tag == GVar.CancelMainFormClosing)
                        {
                            frmMain.gfrmMain.Tag = null;
                            return false;
                        }
                    }
                }

                // When user logout, call database procedure to update Login Time Last with Login Time Current
                if (AppInfor.SecurityKey != Guid.Empty)
                {
                    if (!LoginFactory.Logoff())
                    {
                        MsgBox.Show(msgID, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);
                        return false;
                    }
                }
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
            
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void utolmgrMain_BeforeShortcutKeyProcessed(object sender, Infragistics.Win.UltraWinToolbars.BeforeShortcutKeyProcessedEventArgs e)
        {
            try
            {
                //Check Grid
                UltraGrid ugrd = null;
                if (frmMain.gfrmMain.ActiveControl != null)
                {
                    Control vCN = GlobalUI.GetActiveControlAtActiveForm();
                    if (vCN.GetType() == typeof(TAUtil.TAGridEditor))
                    {
                        ugrd = (UltraGrid)vCN;
                    }
                    else if (vCN.Parent.GetType() == typeof(TAUtil.TAGridEditor)) //If Cursour is in the Editor Control
                    {
                        ugrd = (UltraGrid)vCN.Parent;
                    }
                }

                switch (e.Tool.Key)
                {
                    case "Ctrl1Dummy":
                    case "Ctrl2Dummy":
                    case "Ctrl3Dummy":
                    case "Ctrl4Dummy":
                    case "Ctrl5Dummy":
                    case "Ctrl6Dummy":
                    case "Ctrl7Dummy":
                    case "Ctrl8Dummy":
                    case "Ctrl9Dummy":
                        int codeKey = SysOptionUtility.GetInt("ShortCut" + e.Tool.Key.Replace("Ctrl", "").Replace("Dummy", ""));
                        if (GFunc.NEInt(codeKey, 0) > 0)
                        {
                            DataTable dt = GFunc.ExecuteQuery("Select FormNm from SYS_Code Where CodeKey="+codeKey.ToString());
                            if (dt.Rows.Count > 0)
                            {                          

                                string fromName = GFunc.NEStr(dt.Rows[0]["FormNm"], string.Empty);
                                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                                Form objectForm =(Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + fromName);                           
                                    

                                System.Reflection.ConstructorInfo[] ctors = objectForm.GetType().GetConstructors();
                                foreach (ConstructorInfo ctor in ctors)
                                {
                                    ParameterInfo[] pi = ctor.GetParameters();

                                    if (pi.Count() == 1)
                                    {
                                        if (pi[0].ParameterType.Name == "SystemCode")
                                        {
                                            objectForm = null;
                                            objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + fromName, false, BindingFlags.CreateInstance, null, new object[] { (GEnum.SystemCode)codeKey }, System.Globalization.CultureInfo.CurrentCulture, null);
                                            break;
                                        }
                                    }
                                }                           

                                objectForm.MdiParent = this;
                                objectForm.Show();
                            }
                        }
                        break;
                }
                
                if (GFunc.CompareString(e.Tool.Key, "F10Dummy"))
                {
                    GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_ITMHISTORYSALE, ugrd, true);

                }
                else if (GFunc.CompareString(e.Tool.Key, "F11Dummy"))
                {
                    GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_ITMHISTORYPURCHASE, ugrd, true);

                }
                else if (GFunc.CompareString(e.Tool.Key, "F12Dummy"))
                {
                    GlobalUI.PopupDisplay(GlobalUI.Form_Name.FRM_ITMENQUIRY, ugrd, true);
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            

        }     
        
        //there's created public static void OpenDocument(int CodeKey, int DocKey) function in GlobalUI, may be also used for this method also --Jack      
        private GEnum.SystemCode DocKeyGet(string DataColumn)
        {
            try
            {
                GEnum.SystemCode docCode = GEnum.SystemCode.Customer;//Default
                switch (DataColumn)
                {
                    case "ConID":
                        docCode = GEnum.SystemCode.Customer;
                        break;
                    default:
                        break;
                }
                return docCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
        private Exception Error(Exception ex, bool ShowMessage, bool AddLog)
        {
            try
            {
                ex = SysAuditLogUtility.AppendException(ex, ShowMessage);
                if (AddLog)
                    SysAuditLogUtility.AddErrorLog_New(ex);

            }
            catch (Exception nex)
            {
                throw Error(nex, false, false);
            }
            return ex;
        }//CodeCompleted
        private TAException Error(TAException ex, bool ShowMessage, bool AddLog)
        {
            try
            {
                ex = SysAuditLogUtility.AppendTAException(ex, ShowMessage);
                if (AddLog)
                    SysAuditLogUtility.AddErrorLog_New(ex);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }//CodeCompleted    

        

        //protected override void WndProc(ref Message m)
        //{
        //    try
        //    {
        //        switch (m.Msg)
        //        {
        //            case Win32.WM_COPYDATA:
        //                Win32.CopyDataStruct st = (Win32.CopyDataStruct)Marshal.PtrToStructure(m.LParam, typeof(Win32.CopyDataStruct));
        //                string strData = Marshal.PtrToStringUni(st.lpData);
        //                DisplayMessageReceived(strData);
        //                break;
        //            default:
        //                //let the base class deal with it
        //                base.WndProc(ref m);
        //                break;
        //        }
        //    }
        //    catch (TAException tex)
        //    {
        //        Error(tex, true, false);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error(ex, true, false);
        //    } 
        //}
        //Set Error Methods
        //void DisplayMessageReceived(string message)
        //{
        //    DOProcessForHyperLinkEvent(message);

        //}

        //private void DOProcessForHyperLinkEvent(string HyperLinkText)
        //{
        //    try
        //    {
        //        GEnum.SystemCode DocCodeKey = 0;
        //        if (HyperLinkText != "" && HyperLinkText.Contains(":"))
        //        {
        //            string vHyperLinkFilter = "";
        //            string vHyperLinkValue = "";
        //            string[] vHyperLinkTmp = HyperLinkText.Split(new char[] { ',' });

        //            foreach (string item in vHyperLinkTmp)
        //            {
        //                string[] vFieldAndValue = item.Split(new char[] { ':' });
        //                if (GFunc.CompareString(vFieldAndValue[0], "DocCodeKey"))
        //                {
        //                    DocCodeKey = (GEnum.SystemCode)GFunc.NEInt(vFieldAndValue[1], 0);
        //                }
        //                else
        //                {
        //                    vHyperLinkFilter = vFieldAndValue[0].ToString();
        //                    vHyperLinkValue = vFieldAndValue[1].ToString();
        //                }
        //            }

        //            if (DocCodeKey == null || DocCodeKey == 0)
        //            {
        //                DocKeyGet(vHyperLinkFilter);
        //            }

        //            DOJobForHyperLinkEvent(DocCodeKey, vHyperLinkFilter, vHyperLinkValue);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void DOJobForHyperLinkEvent(GEnum.SystemCode docCodeKey, string HyperLink, string HyperLinkValue)
        //{
        //    try
        //    {
        //        switch (docCodeKey)
        //        {
        //            case GEnum.SystemCode.Utility:
        //                break;
        //            case GEnum.SystemCode.Quotation:
        //                frmARQO fARQO = new frmARQO(GEnum.SystemCode.Quotation);
        //                fARQO.MdiParent = frmMain.gfrmMain;
        //                fARQO.Show();
        //                fARQO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));//Open By DocKey.
        //                break;
        //            case GEnum.SystemCode.Sales_Order:
        //                frmARSO vfrmARSO = new frmARSO(GEnum.SystemCode.Sales_Order);
        //                vfrmARSO.MdiParent = frmMain.gfrmMain;
        //                vfrmARSO.Show();
        //                vfrmARSO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Sales_Adjustment:
        //                frmARADJ vfrmARAdj = new frmARADJ(GEnum.SystemCode.Sales_Adjustment);
        //                vfrmARAdj.MdiParent = frmMain.gfrmMain;
        //                vfrmARAdj.Show();
        //                vfrmARAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));//Open By DocKey.
        //                break;
        //            case GEnum.SystemCode.Untrack_SO:
        //                break;
        //            case GEnum.SystemCode.Works_Order:
        //                break;
        //            case GEnum.SystemCode.Delivery_Order:
        //                frmARDO vfrmARDO = new frmARDO(GEnum.SystemCode.Delivery_Order);
        //                vfrmARDO.MdiParent = frmMain.gfrmMain;
        //                vfrmARDO.Show();
        //                vfrmARDO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.DO_to_IV_Transfer:
        //                break;
        //            case GEnum.SystemCode.Packing_List:
        //                frmARPL vfrmARPL = new frmARPL(GEnum.SystemCode.Packing_List);
        //                vfrmARPL.MdiParent = frmMain.gfrmMain;
        //                vfrmARPL.Show();
        //                vfrmARPL.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Sales_Invoice:
        //                frmARIV vfrmARIV = new frmARIV(GEnum.SystemCode.Sales_Invoice);
        //                vfrmARIV.MdiParent = frmMain.gfrmMain;
        //                vfrmARIV.Show();
        //                vfrmARIV.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Sales_Debit_Note:
        //                frmARIV vfrmARDN = new frmARIV(GEnum.SystemCode.Sales_Debit_Note);
        //                vfrmARDN.MdiParent = frmMain.gfrmMain;
        //                vfrmARDN.Show();
        //                vfrmARDN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Sales_Credit_Note:
        //                frmARIV vfrmARCN = new frmARIV(GEnum.SystemCode.Sales_Credit_Note);
        //                vfrmARCN.MdiParent = frmMain.gfrmMain;
        //                vfrmARCN.Show();
        //                vfrmARCN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            //case GEnum.SystemCode.Sales_Adjustment:
        //            //    frmARADJ vfrmARAdj = new frmARADJ(GEnum.SystemCode.Sales_Order_Adjustment);
        //            //    vfrmARAdj.MdiParent =frmMain.gfrmMain;
        //            //    vfrmARAdj.Show();
        //            //    vfrmARAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));//Open By DocKey.
        //            //    break;
        //            case GEnum.SystemCode.Payment_Received:
        //                frmARPY vfrmARPY = new frmARPY(GEnum.SystemCode.Payment_Received);
        //                vfrmARPY.MdiParent = frmMain.gfrmMain;
        //                vfrmARPY.Show();
        //                vfrmARPY.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.AR_Opening_Balance:
        //                frmMSTConOpenBal vfrmARopen = new frmMSTConOpenBal(GEnum.SystemCode.AR_Opening_Balance);
        //                vfrmARopen.MdiParent = frmMain.gfrmMain;
        //                vfrmARopen.Show();
        //                break;
        //            case GEnum.SystemCode.AR_Revaluation:
        //                frmGLRV vfrmGLVR = new frmGLRV(GEnum.SystemCode.AR_Revaluation);
        //                vfrmGLVR.MdiParent = frmMain.gfrmMain;
        //                vfrmGLVR.Show();
        //                vfrmGLVR.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Contra:
        //                frmARCT vfrmARCT = new frmARCT(GEnum.SystemCode.Contra);
        //                vfrmARCT.MdiParent = frmMain.gfrmMain;
        //                vfrmARCT.Show();
        //                vfrmARCT.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Cash_Sale:
        //                frmARIV vfrmARIVC = new frmARIV(GEnum.SystemCode.Cash_Sale);
        //                vfrmARIVC.MdiParent = frmMain.gfrmMain;
        //                vfrmARIVC.Show();
        //                vfrmARIVC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Cash_Debit_Note:
        //                frmARIV vfrmARDNC = new frmARIV(GEnum.SystemCode.Cash_Debit_Note);
        //                vfrmARDNC.MdiParent = frmMain.gfrmMain;
        //                vfrmARDNC.Show();
        //                vfrmARDNC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Cash_Credit_Note:
        //                frmARIV vfrmARCNC = new frmARIV(GEnum.SystemCode.Cash_Credit_Note);
        //                vfrmARCNC.MdiParent = frmMain.gfrmMain;
        //                vfrmARCNC.Show();
        //                vfrmARCNC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Cash_Adjustment:
        //                frmARADJ vfrmARADJC = new frmARADJ(GEnum.SystemCode.Cash_Adjustment);
        //                vfrmARADJC.MdiParent = frmMain.gfrmMain;
        //                vfrmARADJC.Show();
        //                vfrmARADJC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Cash_Payment_Received:
        //                frmARPY vfrmARPYC = new frmARPY(GEnum.SystemCode.Cash_Payment_Received);
        //                vfrmARPYC.MdiParent = frmMain.gfrmMain;
        //                vfrmARPYC.Show();
        //                vfrmARPYC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.AR_Cash_Opening_Balance:
        //                break;
        //            case GEnum.SystemCode.AR_Cash_Revaluation:
        //                break;
        //            case GEnum.SystemCode.Cash_Contra:
        //                frmARCT vfrmARCTC = new frmARCT(GEnum.SystemCode.Cash_Contra);
        //                vfrmARCTC.MdiParent = frmMain.gfrmMain;
        //                vfrmARCTC.Show();
        //                vfrmARCTC.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Plan:
        //                frmAPPN vfrmAPPN = new frmAPPN(GEnum.SystemCode.Purchase_Plan);
        //                vfrmAPPN.MdiParent = frmMain.gfrmMain;
        //                vfrmAPPN.Show();
        //                vfrmAPPN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Requisition:
        //                break;
        //            case GEnum.SystemCode.Purchase_Request:
        //                frmAPRQ vfrmAPRQ = new frmAPRQ(GEnum.SystemCode.Purchase_Request);
        //                vfrmAPRQ.MdiParent = frmMain.gfrmMain;
        //                vfrmAPRQ.Show();
        //                vfrmAPRQ.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Order:
        //                frmAPPO vfrmAppo = new frmAPPO(GEnum.SystemCode.Purchase_Order);
        //                vfrmAppo.MdiParent = frmMain.gfrmMain;
        //                vfrmAppo.Show();
        //                vfrmAppo.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Order_Adjustment:
        //                frmAPPJ vfrmApPOAdj = new frmAPPJ(GEnum.SystemCode.Purchase_Order_Adjustment);
        //                vfrmApPOAdj.MdiParent = frmMain.gfrmMain;
        //                vfrmApPOAdj.Show();
        //                vfrmApPOAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Untrack_PO:
        //                break;
        //            case GEnum.SystemCode.AP_PO_Confirm_Number:
        //                break;
        //            case GEnum.SystemCode.Purchase_Delivery:
        //                frmAPPD vfrmAPPD = new frmAPPD(GEnum.SystemCode.Purchase_Delivery);
        //                vfrmAPPD.MdiParent = frmMain.gfrmMain;
        //                vfrmAPPD.Show();
        //                vfrmAPPD.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Invoice:
        //                frmAPBL vfrmAPBL = new frmAPBL(GEnum.SystemCode.Purchase_Invoice);
        //                vfrmAPBL.MdiParent = frmMain.gfrmMain;
        //                vfrmAPBL.Show();
        //                vfrmAPBL.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Debit_Note:
        //                frmAPBL vfrmAPDN = new frmAPBL(GEnum.SystemCode.Purchase_Debit_Note);
        //                vfrmAPDN.MdiParent = frmMain.gfrmMain;
        //                vfrmAPDN.Show();
        //                vfrmAPDN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Credit_Note:
        //                frmAPBL vfrmAPCN = new frmAPBL(GEnum.SystemCode.Purchase_Credit_Note);
        //                vfrmAPCN.MdiParent = frmMain.gfrmMain;
        //                vfrmAPCN.Show();
        //                vfrmAPCN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Purchase_Adjustment:
        //                if (GFunc.CompareString(HyperLink, "DocKey"))
        //                {
        //                    frmAPADJ frm_APAdj = new frmAPADJ(GEnum.SystemCode.Purchase_Adjustment);
        //                    frm_APAdj.MdiParent = frmMain.gfrmMain;
        //                    frm_APAdj.Show();
        //                    frm_APAdj.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                }

        //                break;
        //            case GEnum.SystemCode.Payment_Issue:
        //                if (GFunc.CompareString(HyperLink, "DocKey"))
        //                {
        //                    frmAPPY frm_APPY = new frmAPPY(GEnum.SystemCode.Payment_Issue);
        //                    frm_APPY.MdiParent = frmMain.gfrmMain;
        //                    frm_APPY.Show();
        //                    frm_APPY.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                }

        //                break;
        //            case GEnum.SystemCode.AP_Opening_Balance:
        //                break;
        //            case GEnum.SystemCode.AP_Revaluation:
        //                break;
        //            case GEnum.SystemCode.Inventory_Adjustment:
        //                frmINADJ vfrmINADJ = new frmINADJ(GEnum.SystemCode.Inventory_Adjustment);
        //                vfrmINADJ.MdiParent = frmMain.gfrmMain;
        //                vfrmINADJ.Show();
        //                vfrmINADJ.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Inventory_Production:
        //                frmINMFN vfrmINMFN = new frmINMFN(GEnum.SystemCode.Inventory_Production);
        //                vfrmINMFN.MdiParent = frmMain.gfrmMain;
        //                vfrmINMFN.Show();
        //                vfrmINMFN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Inventory_Transfer:
        //                frmINTRN vfrmINTRN = new frmINTRN(GEnum.SystemCode.Inventory_Transfer);
        //                vfrmINTRN.MdiParent = frmMain.gfrmMain;
        //                vfrmINTRN.Show();
        //                vfrmINTRN.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Issue_Consignment:
        //                frmCSCSI vfrmCSCSI = new frmCSCSI(GEnum.SystemCode.Issue_Consignment);
        //                vfrmCSCSI.MdiParent = frmMain.gfrmMain;
        //                vfrmCSCSI.Show();
        //                vfrmCSCSI.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Return_Consignment:
        //                frmCSCSI vfrmCSCSR = new frmCSCSI(GEnum.SystemCode.Return_Consignment);
        //                vfrmCSCSR.MdiParent = frmMain.gfrmMain;
        //                vfrmCSCSR.Show();
        //                vfrmCSCSR.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Untrack_Issue_Consignment:
        //                break;
        //            case GEnum.SystemCode.Order_Consignment:
        //                frmCSCPO vfrmCSCPO = new frmCSCPO(GEnum.SystemCode.Order_Consignment);
        //                vfrmCSCPO.MdiParent = frmMain.gfrmMain;
        //                vfrmCSCPO.Show();
        //                vfrmCSCPO.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Consignment_Order_Adjustment:
        //                break;
        //            case GEnum.SystemCode.Untrack_Consignment_Order:
        //                break;
        //            case GEnum.SystemCode.Received_Consignment:
        //                frmCSCPD vfrmCSCPD = new frmCSCPD(GEnum.SystemCode.Received_Consignment);
        //                vfrmCSCPD.MdiParent = frmMain.gfrmMain;
        //                vfrmCSCPD.Show();
        //                vfrmCSCPD.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Consignment_Settlement:
        //                frmCSCPS vfrmCSCPS = new frmCSCPS(GEnum.SystemCode.Consignment_Settlement);
        //                vfrmCSCPS.MdiParent = frmMain.gfrmMain;
        //                vfrmCSCPS.Show();
        //                vfrmCSCPS.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Journal:
        //                frmGLJNL vfrmGLJNL = new frmGLJNL(GEnum.SystemCode.Journal);
        //                vfrmGLJNL.MdiParent = frmMain.gfrmMain;
        //                vfrmGLJNL.Show();
        //                vfrmGLJNL.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Deposit:
        //                frmGLDP vfrmGLDP = new frmGLDP(GEnum.SystemCode.Deposit);
        //                vfrmGLDP.MdiParent = frmMain.gfrmMain;
        //                vfrmGLDP.Show();
        //                vfrmGLDP.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Bank_Revaluation:
        //                frmGLRV vfrmGLRV = new frmGLRV(GEnum.SystemCode.Bank_Revaluation);
        //                vfrmGLRV.MdiParent = frmMain.gfrmMain;
        //                vfrmGLRV.Show();
        //                vfrmGLRV.OnDocList_OpenRecord(GFunc.NEInt(HyperLinkValue, 0));
        //                break;
        //            case GEnum.SystemCode.Main_Screen:
        //                break;
        //            case GEnum.SystemCode.System_Code:
        //                break;
        //            case GEnum.SystemCode.CounterGrp:
        //                break;
        //            case GEnum.SystemCode.System_Option:
        //                break;
        //            case GEnum.SystemCode.Screen_Customisation:
        //                break;
        //            case GEnum.SystemCode.Company_Setup_Check_List:
        //                break;
        //            case GEnum.SystemCode.Document_Group:
        //                frmREFDocGrp frm_refDocGrp = new frmREFDocGrp(GFunc.NEInt(HyperLinkValue, 0));
        //                frm_refDocGrp.MdiParent = frmMain.gfrmMain;
        //                frm_refDocGrp.Show();

        //                break;
        //            case GEnum.SystemCode.General_List:
        //                break;
        //            case GEnum.SystemCode.Audit_Log:
        //                break;
        //            case GEnum.SystemCode.Account:
        //                if (GFunc.CompareString(HyperLink, "AccKey"))
        //                {
        //                    frmMstAcc frm_mstAcc = new frmMstAcc(GFunc.NEInt(HyperLinkValue, 0));
        //                    frm_mstAcc.MdiParent = frmMain.gfrmMain;
        //                    frm_mstAcc.Show();
        //                }

        //                break;
        //            case GEnum.SystemCode.Account_Opening_Balance:
        //                break;
        //            case GEnum.SystemCode.Account_Unreconciled_Trans:
        //                break;
        //            case GEnum.SystemCode.Period:
        //                break;
        //            case GEnum.SystemCode.Branch:
        //                frmMstAccBranch vfrmBranch = new frmMstAccBranch(GFunc.NEInt(HyperLinkValue, 0));
        //                vfrmBranch.MdiParent = frmMain.gfrmMain;
        //                vfrmBranch.Show();
        //                break;
        //            case GEnum.SystemCode.Department:
        //                frmMstAccDept vfrmDept = new frmMstAccDept(GFunc.NEInt(HyperLinkValue, 0));
        //                vfrmDept.MdiParent = frmMain.gfrmMain;
        //                vfrmDept.Show();
        //                break;
        //            case GEnum.SystemCode.Bank_Reconcilation:
        //                break;
        //            case GEnum.SystemCode.COSBatchPost:
        //                break;
        //            case GEnum.SystemCode.Currency:
        //                frmREFCurr vRefCurr = new frmREFCurr(GFunc.NEInt(HyperLinkValue, 0));   //CurrID
        //                vRefCurr.MdiParent = frmMain.gfrmMain;
        //                vRefCurr.Show();
        //                break;
        //            case GEnum.SystemCode.Bank:
        //                frmREFBank vRefBank = new frmREFBank(GFunc.NEInt(HyperLinkValue, 0));   //BankID
        //                vRefBank.MdiParent = frmMain.gfrmMain;
        //                vRefBank.Show();
        //                break;
        //            case GEnum.SystemCode.Payment_Mode:
        //                frmREFPayMode vRefPayMode = new frmREFPayMode(GFunc.NEInt(HyperLinkValue, 0));   //PayModeID
        //                vRefPayMode.MdiParent = frmMain.gfrmMain;
        //                vRefPayMode.Show();
        //                break;
        //            case GEnum.SystemCode.Tax_Authority:
        //                frmREFTaxA vRefTaxA = new frmREFTaxA(GFunc.NEInt(HyperLinkValue, 0));   //TaxAID
        //                vRefTaxA.MdiParent = frmMain.gfrmMain;
        //                vRefTaxA.Show();
        //                break;
        //            case GEnum.SystemCode.Tax_Group:
        //                frmREFTaxGrp vRefTaxGrp = new frmREFTaxGrp(GFunc.NEInt(HyperLinkValue, 0));   //TaxGrpID
        //                vRefTaxGrp.MdiParent = frmMain.gfrmMain;
        //                vRefTaxGrp.Show();
        //                break;
        //            case GEnum.SystemCode.Overhead:
        //                frmREFOverHead vRefOverHead = new frmREFOverHead(GFunc.NEInt(HyperLinkValue, 0));   //OverHeadID
        //                vRefOverHead.MdiParent = frmMain.gfrmMain;
        //                vRefOverHead.Show();
        //                break;
        //            case GEnum.SystemCode.Account_Group:
        //                frmREFAccGrp vRefAccGrp = new frmREFAccGrp(GFunc.NEInt(HyperLinkValue, 0));   //AccGrpID
        //                vRefAccGrp.MdiParent = frmMain.gfrmMain;
        //                vRefAccGrp.Show();
        //                break;
        //            case GEnum.SystemCode.Sales_Representative:
        //                frmMstSalesRep vfrmSalesRep = new frmMstSalesRep(GFunc.NEInt(HyperLinkValue, 0));
        //                vfrmSalesRep.MdiParent = frmMain.gfrmMain;
        //                vfrmSalesRep.Show();
        //                break;
        //            case GEnum.SystemCode.Budget:
        //                break;
        //            case GEnum.SystemCode.Transaction_Group:
        //                frmMstAccTranGrp vfrmTranGrp = new frmMstAccTranGrp(GFunc.NEInt(HyperLinkValue, 0));
        //                vfrmTranGrp.MdiParent = frmMain.gfrmMain;
        //                vfrmTranGrp.Show();
        //                break;
        //            case GEnum.SystemCode.ARAP_Revaluation:
        //                break;
        //            case GEnum.SystemCode.Customer:
        //                frmMSTCon vfrmMstCon = new frmMSTCon(GFunc.NEInt(HyperLinkValue, 0));   //By Use ConID
        //                vfrmMstCon.MdiParent = frmMain.gfrmMain;
        //                vfrmMstCon.Show();
        //                break;
        //            case GEnum.SystemCode.Vendor:
        //                frmMSTCon vfrmCon = new frmMSTCon(GFunc.NEInt(HyperLinkValue, 0));
        //                vfrmCon.MdiParent = frmMain.gfrmMain;
        //                vfrmCon.Show();
        //                break;
        //            case GEnum.SystemCode.Price_List:
        //                break;
        //            case GEnum.SystemCode.Payment_Term:
        //                frmREFTerm vRefTerm = new frmREFTerm(GFunc.NEInt(HyperLinkValue, 0));   //TermID
        //                vRefTerm.MdiParent = frmMain.gfrmMain;
        //                vRefTerm.Show();
        //                break;
        //            case GEnum.SystemCode.Territory:

        //                frmREFTerritory frm_refTerritory = new frmREFTerritory(GFunc.NEInt(HyperLinkValue, 0));
        //                frm_refTerritory.MdiParent = frmMain.gfrmMain;
        //                frm_refTerritory.Show();



        //                break;
        //            case GEnum.SystemCode.Industry:

        //                frmREFIndustry frm_refIndustry = new frmREFIndustry(GFunc.NEInt(HyperLinkValue, 0));
        //                frm_refIndustry.MdiParent = frmMain.gfrmMain;
        //                frm_refIndustry.Show();

        //                break;
        //            case GEnum.SystemCode.Shipping_Mode:
        //                frmREFShipVia vRefShipVia = new frmREFShipVia(GFunc.NEInt(HyperLinkValue, 0));   //ShipViaKey
        //                vRefShipVia.MdiParent = frmMain.gfrmMain;
        //                vRefShipVia.Show();
        //                break;
        //            case GEnum.SystemCode.Packing_Type:
        //                frmREFPackingType vRefPackingType = new frmREFPackingType(GFunc.NEInt(HyperLinkValue, 0));   //PackingTypeKey
        //                vRefPackingType.MdiParent = frmMain.gfrmMain;
        //                vRefPackingType.Show();
        //                break;
        //            case GEnum.SystemCode.Ship_Name:
        //                break;
        //            case GEnum.SystemCode.Inventory:
        //                frmMSTItm vfrmMstItm = new frmMSTItm(GFunc.NEInt(HyperLinkValue, 0));   //By Use ItemKey
        //                vfrmMstItm.MdiParent = frmMain.gfrmMain;
        //                vfrmMstItm.Show();

        //                break;
        //            case GEnum.SystemCode.Inventory_Opening_Balance:
        //                break;
        //            case GEnum.SystemCode.Category:
        //                frmREFCat vRefCat = new frmREFCat(GFunc.NEInt(HyperLinkValue, 0));   //CatKey
        //                vRefCat.MdiParent = frmMain.gfrmMain;
        //                vRefCat.Show();
        //                break;
        //            case GEnum.SystemCode.Brand:
        //                frmREFBrand vRefBrand = new frmREFBrand(GFunc.NEInt(HyperLinkValue, 0));   //BrandKey
        //                vRefBrand.MdiParent = frmMain.gfrmMain;
        //                vRefBrand.Show();
        //                break;
        //            case GEnum.SystemCode.UOM:
        //                frmREFUOM vRefUOM = new frmREFUOM(GFunc.NEInt(HyperLinkValue, 0));   //UOMKey
        //                vRefUOM.MdiParent = frmMain.gfrmMain;
        //                vRefUOM.Show();
        //                break;
        //            case GEnum.SystemCode.Color:
        //                frmREFColor vRefColor = new frmREFColor(GFunc.NEInt(HyperLinkValue, 0));   //ColorKey
        //                vRefColor.MdiParent = frmMain.gfrmMain;
        //                vRefColor.Show();
        //                break;
        //            case GEnum.SystemCode.Scale:
        //                frmREFScale vRefScale = new frmREFScale(GFunc.NEInt(HyperLinkValue, 0));   //ScaleKey
        //                vRefScale.MdiParent = frmMain.gfrmMain;
        //                vRefScale.Show();
        //                break;
        //            case GEnum.SystemCode.Location:
        //                frmREFLoc vRefLocation = new frmREFLoc(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
        //                vRefLocation.MdiParent = frmMain.gfrmMain;
        //                vRefLocation.Show();
        //                break;
        //            case GEnum.SystemCode.Stock_Count:
        //                break;
        //            case GEnum.SystemCode.Job:
        //                frmMSTJob vfrmJob = new frmMSTJob(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
        //                vfrmJob.MdiParent = frmMain.gfrmMain;
        //                vfrmJob.Show();
        //                break;
        //            case GEnum.SystemCode.Job_Opening_Balance:

        //                break;

        //            case GEnum.SystemCode.Job_Cost_Type:
        //                frmREFJobCostType vRefJobCostType = new frmREFJobCostType(HyperLinkValue);   //JobCostTypeID
        //                vRefJobCostType.MdiParent = frmMain.gfrmMain;
        //                vRefJobCostType.Show();
        //                break;
        //            case GEnum.SystemCode.Job_Phase:
        //                frmREFJobPhase vRefJobPhase = new frmREFJobPhase(GFunc.NEInt(HyperLinkValue, 0));   //JobPhaseID
        //                vRefJobPhase.MdiParent = frmMain.gfrmMain;
        //                vRefJobPhase.Show();
        //                break;
        //            case GEnum.SystemCode.Job_Task:
        //                frmREFJobTask vRefJobTask = new frmREFJobTask(GFunc.NEInt(HyperLinkValue, 0));   //JobTaskID
        //                vRefJobTask.MdiParent = frmMain.gfrmMain;
        //                vRefJobTask.Show();
        //                break;
        //            case GEnum.SystemCode.Job_Group:
        //                frmREFJobGrp vRefJobGrp = new frmREFJobGrp(GFunc.NEInt(HyperLinkValue, 0));   //JobGrpID
        //                vRefJobGrp.MdiParent = frmMain.gfrmMain;
        //                vRefJobGrp.Show();
        //                break;
        //            case GEnum.SystemCode.Job_Timesheet:
        //                break;
        //            case GEnum.SystemCode.Machine_List:
        //                break;
        //            case GEnum.SystemCode.Machine_Type_List:
        //                break;
        //            case GEnum.SystemCode.Alerts:
        //                break;
        //            case GEnum.SystemCode.Alert_Log:
        //                break;
        //            case GEnum.SystemCode.To_Do:
        //                break;
        //            case GEnum.SystemCode.To_Do_Log:
        //                break;
        //            case GEnum.SystemCode.Other_Report_Setting:
        //                break;
        //            case GEnum.SystemCode.Report_ID_Format:
        //                break;
        //            case GEnum.SystemCode.Report_Set_Rpt_Files:
        //                break;
        //            case GEnum.SystemCode.Cash_Flow:
        //                break;
        //            case GEnum.SystemCode.Financial_Charge:
        //                break;
        //            case GEnum.SystemCode.Interest_Rate:
        //                frmREFInterest vRefInterest = new frmREFInterest(HyperLinkValue);   //IntID
        //                vRefInterest.MdiParent = frmMain.gfrmMain;
        //                vRefInterest.Show();
        //                break;
        //            case GEnum.SystemCode.Security_User:
        //                frmSECUser vfrmUser = new frmSECUser(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
        //                vfrmUser.MdiParent = frmMain.gfrmMain;
        //                vfrmUser.Show();
        //                break;
        //            case GEnum.SystemCode.Security_Group:
        //                frmSECGroup vfrmUserGrp = new frmSECGroup(GFunc.NEInt(HyperLinkValue, 0));   //LocKey
        //                vfrmUserGrp.MdiParent = frmMain.gfrmMain;
        //                vfrmUserGrp.Show();
        //                break;
        //            case GEnum.SystemCode.Security_ChangePassword:
        //                break;
        //            case GEnum.SystemCode.Uploaded_Document:
        //                break;
        //            case GEnum.SystemCode.Import_Data:
        //                break;
        //            case GEnum.SystemCode.Message_List:
        //                break;
        //            case GEnum.SystemCode.RecordAccess:
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}