using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataDynamics.ActiveReports;
using System.Transactions;
using System.Data.SqlClient;
using BOLib;
using System.Windows.Forms;
using System.IO;
using Infragistics.Win.UltraWinGrid;
using TAUtil;

namespace WinUI
{
    class ReportFactory
    {
        //Variables
        // private BOLib.SYSRepListFactory _RepListFactory;

        private ReportLoader _ReportLoader;

        private SYSRepParas _sysRepParas;

        private SYSRepCriterias _sysRepCriterias;

        //To store the single Report File Information
        private SYSRep _sysRep;

        //To store all the ReportFileList
        private SYSRepRpts _sysRepRpts;

        //To store all the ReportList based on Report Group
        private DataTable _ROSYSRep = null;

        private int _RepKey;
        private int _DocKey;
        private string _reportOrderBy = string.Empty;

        public enum PeriverSource { NoPreive = 0, Source1Only, Source2Only, Both };
        private PeriverSource _RepPreviewBy = PeriverSource.Source1Only;

        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private int _guID = 0;

        // System Code Key for this Factory.
        public const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Report_Set_Rpt_Files;

        // Permission ID for this Factory.
        public const string constPermID = GVar.PermissionID.Report_Rpt_File_Setting;

        private string pRepRange;
        private DateTime pDateF;
        private DateTime pDateT;
        private DateTime pYTD;
        private DateTime pAgeDate;
        private string pRepTitle;
        private string pDocTypeNm;
        private string pDocCodeDes;
        private int PeriodFrom;
        private int PeriodTo;
        int Period = 0;
        //internal string RptFilePath;
        private string _xmlAccSelected = string.Empty;
        private DateTime AgeDate;//for Aging Mth Rpt
        private string pMth1, pMth2, pMth3, pMth4, pMth5, pMth6, pMth7, pMth8, pMth9, pMth10, pMth11, pMth12;

        //Properties
        public int DocKey
        {
            get { return _DocKey; }
            set { _DocKey = value; }
        }

        public int RepKey
        {
            get { return _RepKey; }
        }

        //To Store the Report List Based on Report Group
        public DataTable ROSYSRep
        {
            get
            {
                return this._ROSYSRep;
            }
        }

        //To Store the single report file information
        public SYSRep ObjSYSRep
        {
            get { return _sysRep; }
            set { _sysRep = value; }
        }

        public SYSRepCriterias SYSRepCriterias
        {
            get { return _sysRepCriterias; }
            set { _sysRepCriterias = value; }
        }

        public SYSRepParas ReportParameters
        {
            get { return _sysRepParas; }
            set { _sysRepParas = value; }
        }

        public SYSRepRpts ReportFileList
        {
            get { return _sysRepRpts; }
            set { _sysRepRpts = value; }
        }

        public string XMLAccSelected
        {
            get { return _xmlAccSelected; }
            set { _xmlAccSelected = value; }
        }

        public ReportFactory(int RepKey)//Not ready-- to remove RepKey
        {
            try
            {
                //_RepListFactory = new SYSRepListFactory(GEnum.InstanceMode.Normal);
                _ReportLoader = new ReportLoader();

                //Not ready ---- To remove-----------------------------------
                if (RepKey > 0)
                {
                    _RepKey = RepKey;

                    //GetReport();                   
                    _sysRep = SYSRep.Get(_RepKey);
                    _sysRepParas = SYSRepParas.Get(_RepKey);
                    _sysRepRpts = SYSRepRpts.Get(_RepKey);
                    GetReportCriterias();
                    //DOCreateReportDOC();
                }
                //Not ready ---- To remove-----------------------------------
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable GetRepsByGroup(int Group)
        {
            try
            {
                if (Group == 0)
                {
                    _ROSYSRep = SYSList.GetReports(4, 0);
                }
                else
                {
                    _ROSYSRep = SYSList.GetReports(3, Group);
                }

                return _ROSYSRep;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SYSRepCriterias GetReportCriterias()
        {
            try
            {
                SYSRepCriterias = SYSRepCriterias.Get(_RepKey);
                SYSRepCriterias.Columns.Add("CriteriaLabel1", typeof(string));
                SYSRepCriterias.Columns.Add("CriteriaSource1", typeof(string));
                SYSRepCriterias.Columns.Add("CriteriaLabel2", typeof(string));
                SYSRepCriterias.Columns.Add("CriteriaSource2", typeof(string));
                SYSRepCriterias.Columns.Add("CriteriaLabel3", typeof(string));
                SYSRepCriterias.Columns.Add("CriteriaSource3", typeof(string));
                return SYSRepCriterias;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Methods
        public void RefreshDataByRepKey(int RepKey)
        {
            try
            {
                if (RepKey > 0)
                {
                    _RepKey = RepKey;

                    _sysRep = SYSRep.Get(_RepKey);
                    _sysRepParas = SYSRepParas.Get(_RepKey);
                    _sysRepRpts = SYSRepRpts.Get(_RepKey);
                    GetReportCriterias();
                }
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //called from Report Directory
        public bool PreviewReport(Infragistics.Win.UltraWinListView.UltraListView lvwRptFileList, TAUtil.TAGridEditor tagrdCriteriaList, bool view)
        {
            try
            {                
                string fName = "";
                //RptFilePath = fName;
                string recordSource = string.Empty;
                string AltRecordSource = string.Empty;

                if (tagrdCriteriaList.ActiveRow != null)
                    tagrdCriteriaList.ActiveRow.Update();

                if (lvwRptFileList.SelectedItems.Count == 0)
                    if (lvwRptFileList.Items.Count > 0)
                        lvwRptFileList.SelectedItems.Add(lvwRptFileList.Items[0]);
                    else
                        return false;

                if (!GFunc.IsNE(lvwRptFileList.SelectedItems[0].SubItems["RptPermission"].Value))
                {
                    if (!SECPermUtility.Read(lvwRptFileList.SelectedItems[0].SubItems["RptPermission"].Value.ToString(), true))
                    {
                        return false;
                    }
                }

                if (!GFunc.IsNE(lvwRptFileList.SelectedItems[0].SubItems["RptNm"].Value))
                {
                    fName += lvwRptFileList.SelectedItems[0].SubItems["RptNm"].Value.ToString();
                    pRepTitle = lvwRptFileList.SelectedItems[0].SubItems["RptDes"].Value.ToString();
                }
                else
                {
                    fName += ObjSYSRep.RPTname1;
                }

                //AltRecordSource is used as one of the parameter inside the storedprocedure(recordSource)
                AltRecordSource = GFunc.NEStr(lvwRptFileList.SelectedItems[0].SubItems["RptAltRecordSource"].Value, "");

                if (GFunc.IsNE(AltRecordSource))
                    recordSource = _sysRep.RPTRecordSource1;
                else
                    recordSource = AltRecordSource;


                if (!GFunc.IsNE(lvwRptFileList.SelectedItems[0].SubItems["RptOrderBy"].Value))
                {
                    _reportOrderBy = lvwRptFileList.SelectedItems[0].SubItems["RptOrderBy"].Value.ToString();
                }
                else
                    _reportOrderBy = "";

                if (!File.Exists(Application.StartupPath + @"\Reports\" + fName))
                {
                    MsgBox.Show("ReportFileNotExist%" + fName);
                    return false;
                }
                else
                {
                    List<SqlParameter> parmList = GetSqlParameters(tagrdCriteriaList);
                    if (!GFunc.IsNE(lvwRptFileList.SelectedItems[0].SubItems["Custom1"].Value))
                    {
                        SqlParameter param1 = new SqlParameter("@RptParamCustom1", lvwRptFileList.SelectedItems[0].SubItems["Custom1"].Value);
                        parmList.Add(param1);
                    }

                    if (parmList == null)
                        return false;

                    switch (RepKey)
                    {
                        //for CustAge & VendorAge
                        case 1280:
                        case 1305:
                        case 1550:
                        case 1875:
                        case 1880:
                        case 1890:
                        case 1510:
                        case 1511:
                        case 1525:
                        case 1282:
                        case 1527:
                        case 1512:
                            if (!MtnPara(AgeDate))
                                return false;
                            break;
                    }


                    if (view)
                    {
                        if (_RepPreviewBy == PeriverSource.Source1Only || _RepPreviewBy == PeriverSource.Both)
                        {
                            //GFunc.XMLOutput(parmList, _sysRep.RPTRecordSource1);
                            Application.DoEvents();
                            //string ss = GFunc.ExecuteProcQueryStringGet(recordSource, parmList);
                            DataSet ds = GFunc.ExecuteProcDataSet(recordSource, parmList);
                            DataTable dt = new DataTable();

                            //DataSet ds = new DataSet();
                            //DataTable dt=GFunc.ExecuteProcReader(recordSource,parmList);
                           
                            if (ds.Tables.Count > 0)
                                dt = ds.Tables[0];
                            else
                            {
                                MsgBox.Show("Report Data Get Fail"); //
                                return false;
                            }


                            if (GFunc.IsNE(_reportOrderBy) == false)
                            {
                                dt.DefaultView.Sort = _reportOrderBy;
                                dt = dt.DefaultView.ToTable();
                            }

                            if (fName.ToUpper().EndsWith(".RPX"))
                            {
                                _ReportLoader.ReportParameter = GetReportParameters();

                                //Additional User Input Parameters
                                tagrdCriteriaList.DisplayLayout.Bands[0].ColumnFilters["CriteriaSpecialTag"].FilterConditions.Add(FilterComparisionOperator.Equals, "reportparameterinput");
                                UltraGridRow[] paraRows = tagrdCriteriaList.Rows.GetFilteredInNonGroupByRows();
                                foreach (UltraGridRow row in paraRows)
                                {
                                    _ReportLoader.ReportParameter.Add(new ReportParameter(row.Cells["CriteriaNm"].Value.ToString(), row.Cells["CriteriaSource1"].Value));
                                }
                                tagrdCriteriaList.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();

                                _ReportLoader.LoadReport(dt, fName);

                                _ReportLoader.RepKey = GFunc.NEInt(_sysRep.RepKey, 0);
                                if (_ReportLoader.PrintPreview() == false)
                                    return false;
                            }
                            else if (fName.ToUpper().EndsWith(".RPT"))
                            {
                                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                                rptDoc.Load(Application.StartupPath + @"\Reports\" + fName);
                                rptDoc.SetDataSource(dt);

                                if (RepKey == 2250 && GFunc.CompareString(fName, "Sys_AlertLog_List.rpt"))
                                {
                                    if (rptDoc.Subreports.Count > 0 && ds.Tables.Count > 1)
                                    {
                                        rptDoc.Subreports[0].SetDataSource(ds.Tables[1]);
                                    }
                                }
                                //added by May on 27-Nov-2020. This RptName is only used in Report Rpt Setting for OMS
                                else if (RepKey == 1145 && GFunc.CompareString(fName, "zAcc_DocJrnl_DocChg.rpt"))
                                {
                                    if (rptDoc.Subreports.Count > 0 && ds.Tables.Count > 1)
                                    {
                                        rptDoc.Subreports[0].SetDataSource(ds.Tables[1]);
                                    }
                                }

                                List<ReportParameter> rptParams = GetReportParameters();
                                foreach (ReportParameter p in rptParams)
                                {
                                    rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                                }

                                frmReportViewer fRptViewer = new frmReportViewer();
                                fRptViewer.RepKey = RepKey;
                                fRptViewer.RptName = fName;
                                ObjSYSRep.RPTname1 = fName;//For To do Form
                                fRptViewer.RptDocument = rptDoc;
                                fRptViewer.MdiParent = frmMain.gfrmMain;
                                fRptViewer.Show();

                                GC.Collect();
                            }
                        }
                    }
                }

                return true;                
            }
            catch (TAException ex)
            {
                throw ex;
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
                GC.Collect();
            }
        }

        public static void AddAdditionalParameters(List<SqlParameter> parmList, int RepKey, string XMLAccSelected)
        {
            SqlParameter paramName = null;
            try
            {
                switch (RepKey)
                {
                    case 1140:
                        if (GFunc.IsNE(XMLAccSelected) == false)
                            parmList.Add(new SqlParameter("@xmlSelAccount", XMLAccSelected));
                        break;

                    case 1500:
                        //paramName = new SqlParameter("@ConType", 20);
                        //parmList.Add(new SqlParameter("@RepNm", _sysRep.RPTRecordSource1));
                        //parmList.Add(paramName);
                        break;
                    case 1495:
                        paramName = new SqlParameter("@RepKey", RepKey);
                        parmList.Add(paramName);
                        paramName = new SqlParameter("@ConType", 20);
                        parmList.Add(paramName);
                        break;
                    case 1695:
                    case 1700:
                        //paramName = new SqlParameter("@RepNm", _sysRep.RPTRecordSource1);
                        //parmList.Add(paramName);
                        break;
                    //for Cust List
                    case 1250:
                    //for Vend List
                    case 1485:
                    //for Job
                    case 2045:
                    case 2040:
                    case 2035:
                    case 2030:
                    //case 2025:
                    //case 2095:
                    case 2090:
                    case 2085:
                    case 2080:
                    case 2075:
                    case 2070:
                    case 2065:
                    case 2060:
                    case 2055:
                    case 2050:
                    //for Budget                        
                    case 1230:
                    case 1225:
                    case 1220:
                    case 1215:
                    case 1210:
                    case 1205:
                    case 1200:
                    case 1195:
                    case 1190:
                    case 1185:
                    case 1180:
                    case 1175:
                    case 1170:
                    //for CustAge & VendorAge
                    case 1275:
                    case 1300:
                    case 1295:
                    case 1290:
                    case 1285:
                    case 1280:
                    case 1505:
                    case 1510:
                    case 1511:
                    case 1515:
                    case 1520:
                    case 1525:
                    case 1530:
                    case 1535:
                    case 1540:
                    case 1545:

                    //for Customer Transaction
                    case 1305:
                    case 1315:
                        paramName = new SqlParameter("@RepKey", RepKey);
                        parmList.Add(paramName);
                        break;
                    //ItmTransaction                  
                    case 1895:
                    case 1890:
                    case 1892:
                    case 1885:
                    case 1880:
                    case 1875:
                    case 1870:
                    case 1865:
                    case 1860:
                    case 1855:
                    case 1850:
                    case 1845:
                    //Itm Price List
                    case 1720:
                    case 1255:
                    case 1490:
                    case 1896:
                        paramName = new SqlParameter("@RepKey", RepKey);
                        parmList.Add(paramName);
                        break;
                    //for purchase document
                    //case 1575://PURCHASE REQUEST LIST
                    //    paramName = new SqlParameter("@DocCodeKey",GEnum.SystemCode.Purchase_Request);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1580://PURCHASE ORDER LIST
                    //    paramName = new SqlParameter("@DocCodeKey",(int)GEnum.SystemCode.Purchase_Order);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1585://PURCHASE DELIVERY LIST
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Delivery);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1590://PURCHASE INVOICE LIST
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Invoice);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1595://PURCHASE INVOICE LIST (ORIGINAL DATE)
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Invoice);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1600://PURCHASE INVOICE  LIST LINK TO PAYMENT
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Invoice);
                    //    parmList.Add(paramName); 
                    //    break;
                    //case 1605://PURCHASE CREDIT NOTE LIST
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Credit_Note);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName); 
                    //    break;
                    //case 1610://PURCHASE CREDIT NOTE LIST (ORIGINAL DATE)
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Credit_Note);
                    //    parmList.Add(paramName); 
                    //    break;
                    //case 1615://PURCHASE CREDIT NOTE LIST LINK TO PAYMENT
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Credit_Note);
                    //    parmList.Add(paramName); 
                    //    break;
                    //case 1620://PURCHASE DEBIT NOTE LIST
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Debit_Note);
                    //    parmList.Add(paramName); 
                    //    break;
                    //case 1625://PURCHASE DEBIT NOTE LIST (ORIGINAL DATE)
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Debit_Note);
                    //    parmList.Add(paramName); 
                    //    break;
                    //case 1630://PURCHASE DEBIT NOTE LIST LINK TO PAYMENT
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Debit_Note);
                    //    parmList.Add(paramName); 
                    //    break;
                    //case 1635://PAYMENT ISSUE LIST
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Payment_Issue);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1640://PURCHASE ADJUSTMENT LIST
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Adjustment);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1645://PURCHASE ADJUSTMENT LIST LINK TO PAYMENT
                    //    paramName = new SqlParameter("@DocCodeKey", GEnum.SystemCode.Purchase_Adjustment);
                    //    parmList.Add(paramName); 
                    //    break;              
                    //end purchase document
                    //Sale Document
                    //case 1340://Quotation List
                    //    paramName = new SqlParameter("@DocCodeKey",(int)GEnum.SystemCode.Quotation);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;
                    //case 1355://Invlice List
                    //    paramName = new SqlParameter("@DocCodeKey", (int)GEnum.SystemCode.Sales_Invoice);
                    //    parmList.Add(paramName);
                    //    paramName = new SqlParameter("@RepKey", RepKey);
                    //    parmList.Add(paramName);
                    //    break;

                    //Sales Order List
                    case 1410:
                        paramName = new SqlParameter("@RepKey", RepKey);
                        parmList.Add(paramName);
                        break;
                    //Consignment 
                    case 1960:
                    case 1965:
                    case 1970:
                        paramName = new SqlParameter("@RepKey", RepKey);
                        parmList.Add(paramName);
                        break;

                    default:
                        //paramName = new SqlParameter("@RepKey", RepKey);
                        //parmList.Add(paramName);  
                        break;
                }
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SqlParameter> GetSqlParameters(TAUtil.TAGridEditor tagrdCriteriaList)
        {
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                pRepRange = string.Empty;
                SqlParameter param1 = null;
                SqlParameter param2 = null;

                GEnum.ReportSearchOption searchType = GEnum.ReportSearchOption.Range;
                for (int i = 0; i < SYSRepCriterias.Rows.Count; i++)
                {
                    int criteriaIntVal = 0;


                    UltraGridRow obj = tagrdCriteriaList.Rows[i];

                    #region Set row values
                    int CriteriaSeq = 0;
                    string CriteriaType = "";
                    string CriteriaDataType = "";
                    string CriteriaNm = "";
                    string CriteriaLabel = "";

                    string CriteriaListID = "";
                    bool CriteriaSetDateButton = false;
                    bool CriteriaHaveFormatSearch = false;

                    int CriteriaRangeColValue = 0;
                    string CriteriaSpecialTag = "";
                    bool CriteriaHidden = false;
                    bool CriteriaLimitToList = false;
                    string CriteriaDefaultValue = "";
                    bool CriteriaRequired = false;

                    string CriteriaSource1 = null;
                    string CriteriaSource2 = null;
                    string CriteriaSource3 = null;

                    int.TryParse(obj.Cells["CriteriaSeq"].Value.ToString(), out CriteriaSeq);
                    CriteriaType = obj.Cells["CriteriaType"].Value.ToString();
                    CriteriaDataType = obj.Cells["CriteriaDataType"].Value.ToString();
                    CriteriaNm = obj.Cells["CriteriaNm"].Value.ToString();
                    CriteriaLabel = obj.Cells["CriteriaLabel"].Value.ToString();
                    if (CriteriaNm == "DateV")
                    {
                    }
                    CriteriaListID = obj.Cells["CriteriaListID"].Value.ToString();
                    bool.TryParse(obj.Cells["CriteriaSetDateButton"].Value.ToString(), out CriteriaSetDateButton);
                    bool.TryParse(obj.Cells["CriteriaHaveFormatSearch"].Value.ToString(), out CriteriaHaveFormatSearch);

                    int.TryParse(obj.Cells["CriteriaRangeColValue"].Value.ToString(), out CriteriaRangeColValue);
                    CriteriaSpecialTag = obj.Cells["CriteriaSpecialTag"].Value.ToString();
                    bool.TryParse(obj.Cells["CriteriaHidden"].Value.ToString(), out CriteriaHidden);
                    bool.TryParse(obj.Cells["CriteriaLimitToList"].Value.ToString(), out CriteriaLimitToList);
                    CriteriaDefaultValue = obj.Cells["CriteriaDefaultValue"].Value.ToString();
                    bool.TryParse(obj.Cells["CriteriaRequired"].Value.ToString(), out CriteriaRequired);

                    CriteriaSource1 = obj.Cells["CriteriaSource1"].Value.ToString();
                    CriteriaSource2 = obj.Cells["CriteriaSource2"].Value.ToString();
                    CriteriaSource3 = obj.Cells["CriteriaSource3"].Value.ToString();
                    #endregion

                    if (CriteriaNm == "P")
                    {
                    }
                    //skip, intended for Report Parameters
                    if (GFunc.CompareString(CriteriaSpecialTag, "reportparameterinput"))
                        continue;
                    switch (RepKey)
                    {
                        //for CustAge & VendorAge
                        case 1280:
                        case 1305:
                        case 1550:
                        case 1875:
                        case 1880:
                        case 1890:
                        case 1510:
                        case 1511:
                        case 1525:
                        case 1282:
                        case 1527:
                        case 1512:
                            if (GFunc.CompareString(CriteriaNm, "DateV") && obj.Cells["CriteriaSource1"].Value != DBNull.Value)
                            {
                                AgeDate = Convert.ToDateTime(obj.Cells["CriteriaSource1"].Value.ToString());
                            }
                            break;
                    }
                   

                    if (CriteriaListID.ToString() != string.Empty)
                    {
                        #region Combo

                        if (GFunc.CompareString("msglist", CriteriaType))
                        {
                            if (GFunc.IsNE(CriteriaSource1))
                            {
                                if (CriteriaRequired)
                                {
                                    MsgBox.Show(CriteriaNm + "IsRequire");
                                    return null;
                                }
                            }

                            if (CriteriaLimitToList)
                            {
                                if (GFunc.IsNE(tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent) == false)
                                {
                                    TAUtil.TAComboBox cbo = (TAUtil.TAComboBox)tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent;
                                    IEnumerable<DataRow> dtFilter = (cbo.DataSource as DataTable).AsEnumerable().Where(r => r.Field<string>(cbo.DisplayMember).Equals(
                                        tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value.ToString()));
                                    //dt.DefaultView.RowFilter = cbo.DisplayMember + "='" + tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].ToString() + "'";
                                    //if (dt.DefaultView.Count > 0)

                                    if (dtFilter.Count() > 0)
                                    {
                                        Int32.TryParse(dtFilter.ElementAt(0)[cbo.ValueMember].ToString(), out criteriaIntVal);
                                    }
                                    // dt.DefaultView.RowFilter = "";
                                    Int32.TryParse(cbo.Value == null ? "" : cbo.Value.ToString(), out criteriaIntVal);

                                }
                                param1 = new SqlParameter("@" + CriteriaNm, criteriaIntVal);
                            }
                            else
                            {
                                //Added By Dimple because The Value of MsgList must be Integer Value
                                CriteriaSource1 = obj.Cells["CriteriaSource1"].Value.ToString();
                                param1 = new SqlParameter("@" + CriteriaNm, CriteriaSource1);
                            }
                            if (GFunc.IsNE(CriteriaSource1) == false)
                            {
                                parmList.Add(param1);
                            }
                            if (CriteriaNm != "RepKey")
                            {
                                if (CriteriaSource1 != string.Empty)
                                    pRepRange += CriteriaLabel.ToString() + " = \"" + CriteriaSource1 + "\"" + ",";
                                else
                                    pRepRange += "FOR ALL " + CriteriaLabel + ",";
                            }
                        }
                        else if (GFunc.CompareString(GVar.ReportCriteriaType.Range, CriteriaType) || GFunc.CompareString(GVar.ReportCriteriaType.Range10, CriteriaType))
                        {
                            if (GFunc.IsNE(CriteriaSource1) || GFunc.IsNE(CriteriaSource2))
                            {
                                if (CriteriaRequired.ToString() != "")
                                {
                                    if (Convert.ToBoolean(CriteriaRequired))
                                    {
                                        MsgBox.Show(CriteriaNm + "IsRequire");
                                        return null;
                                    }
                                }
                            }

                            if (CriteriaNm != "RepKey")
                            {
                                if (CriteriaSource1 != string.Empty && CriteriaSource2 != string.Empty)
                                    pRepRange += CriteriaLabel + " Between \"" + CriteriaSource1 + "\" And \"" + CriteriaSource2 + "\"" + ",";
                                else
                                    pRepRange += "FOR ALL " + CriteriaLabel + ",";
                            }

                            if (CriteriaDataType != null && CriteriaDataType != DBNull.Value.ToString())
                            {
                                if (CriteriaDataType.Contains("varchar"))
                                {
                                    if (CriteriaSource1 == "")
                                    {
                                        CriteriaSource1 = "a";
                                    }
                                    if (CriteriaSource2 == "")
                                    {
                                        CriteriaSource2 = "z";
                                    }
                                }
                            } 
                            string rangeSeq = "";
                            if (CriteriaType == GVar.ReportCriteriaType.Range10)
                            {
                                int seq = 0;
                                int.TryParse(CriteriaLabel.Substring(CriteriaLabel.Length - 1), out seq);
                                if (seq > 0)
                                {
                                    rangeSeq = seq.ToString();
                                }
                            }

                            //param1 = new SqlParameter("@" + CriteriaNm + rangeSeq + "From", CriteriaSource1 + CriteriaSource3);
                            //param2 = new SqlParameter("@" + CriteriaNm + rangeSeq + "To", CriteriaSource2);

                            if (CriteriaHaveFormatSearch && CriteriaSource3 != "")
                            {
                                param1 = new SqlParameter("@" + CriteriaNm + rangeSeq + "Wild", CriteriaSource1 + CriteriaSource3);
                                parmList.Add(param1);
                                CriteriaSource1 = "";
                                CriteriaSource2 = "";
                            }
                            else
                            {
                                param1 = new SqlParameter("@" + CriteriaNm + rangeSeq + "From", CriteriaSource1);
                                param2 = new SqlParameter("@" + CriteriaNm + rangeSeq + "To", CriteriaSource2);

                                //for BH vendor statement customized report
                                if (GFunc.CompareString(CriteriaDataType, GVar.ReportCriteriaDataType.IntegerPeriod))
                                {
                                    PeriodFrom = GFunc.NEInt(CriteriaSource1, 100000);
                                    PeriodTo = GFunc.NEInt(CriteriaSource2, 100000);
                                }
                             
                            }

                            if (GFunc.IsNE(CriteriaSource1) == false)
                            {
                                parmList.Add(param1);
                            }
                            if (GFunc.IsNE(CriteriaSource2) == false)
                            {
                                parmList.Add(param2);
                            }
                            if (param1 != null & param2 == null && CriteriaHaveFormatSearch == true)
                            {
                                param1 = new SqlParameter("@" + CriteriaNm + rangeSeq + "Wild", CriteriaSource1 + "%");
                                parmList.Add(param1);
                            }
                        }
                        else if (GFunc.CompareString(GVar.ReportCriteriaType.Single, CriteriaType))
                        {
                            if (GFunc.IsNE(CriteriaSource1))
                            {
                                if (CriteriaRequired)
                                {
                                    MsgBox.Show(CriteriaLabel + " Is Require");
                                    return null;
                                }
                            }
                            if (CriteriaLimitToList)
                            {
                                if (GFunc.IsNE(obj.Cells["CriteriaSource1"].EditorComponent) == false)
                                {

                                    TAUtil.TAComboBox cbo = (TAUtil.TAComboBox)obj.Cells["CriteriaSource1"].EditorComponent;
                                    //DataTable dt = cbo.DataSource as DataTable;
                                    //dt.DefaultView.RowFilter = cbo.DisplayMember + "='" + (CriteriaSource1 == null ? "" : CriteriaSource1 + "'");
                                    //if (dt.DefaultView.Count > 0)
                                    IEnumerable<DataRow> dtFilter;
                                    if (GFunc.CompareString(cbo.DisplayMember, "Period"))

                                        dtFilter = (cbo.DataSource as DataTable).AsEnumerable().Where(r => GFunc.NEInt(r.Field<int?>(cbo.DisplayMember), 0) ==
                                        GFunc.NEInt(tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value, 0));
                                    else
                                        dtFilter = (cbo.DataSource as DataTable).AsEnumerable().Where(r => GFunc.NEStr(r.Field<string>(cbo.DisplayMember), "").Equals(
                                      GFunc.NEStr(tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].Value, "")));



                                    if (GFunc.NEInt(dtFilter.Count(), 0) > 0)
                                    {
                                        Int32.TryParse(dtFilter.ElementAt(0)[cbo.ValueMember].ToString(), out criteriaIntVal);
                                    }
                                    // dt.DefaultView.RowFilter = "";

                                }
                                if (CriteriaLimitToList && !GFunc.CompareString(CriteriaDataType, GVar.ReportCriteriaDataType.String))
                                {
                                    Int32.TryParse(GFunc.NEStr(CriteriaSource1, 0)
                                        , out criteriaIntVal);
                                    param1 = new SqlParameter("@" + CriteriaNm, criteriaIntVal);
                                    if (GFunc.CompareString(CriteriaDataType, GVar.ReportCriteriaDataType.IntegerPeriod))
                                    {
                                        Period = criteriaIntVal;
                                        string pp = GFunc.NEStr(Period, "");
                                        int Year = GFunc.NEInt(pp.Substring(0, 4), 0);
                                        int Month = GFunc.NEInt(pp.Substring(4, 2), 0);
                                        int TotalDaysInMonth = DateTime.DaysInMonth(Year, Month);
                                        AgeDate = GFunc.NEDateTime(Year + "/" + Month + "/" + TotalDaysInMonth, DateTime.Today.Date);


                                        pDateT = AgeDate;
                                    }


                                }
                                else
                                {

                                    param1 = new SqlParameter("@" + CriteriaNm, CriteriaSource1);
                                }

                            }
                            else
                            {
                                /////////assign doctypenm
                                if (GFunc.CompareString(CriteriaNm, "DocTypeNm"))
                                    pDocTypeNm = CriteriaSource1;
                                param1 = new SqlParameter("@" + CriteriaNm, CriteriaSource1);
                            }
                            if (GFunc.IsNE(CriteriaSource1) == false)
                            {
                                parmList.Add(param1);
                            }

                            TAUtil.TAComboBox cboSingle = (TAUtil.TAComboBox)tagrdCriteriaList.Rows[i].Cells["CriteriaSource1"].EditorComponent;
                            DataTable dtSingle = cboSingle.DataSource as DataTable;
                            if (cboSingle.DisplayLayout.Bands[0].Columns.Count > 0)
                            {
                                bool IDCol = false;
                                string ValueMember = GFunc.NEStr(tagrdCriteriaList.Rows[i].Cells["CriteriaValueMember"].Value, "");
                                if (cboSingle.DisplayLayout.Bands[0].Columns[ValueMember].DataType != typeof(int))
                                    IDCol = true;

                                IEnumerable<DataRow> dtFilter = null;
                                if (IDCol)
                                {
                                    if (!GFunc.IsNE(CriteriaSource1))
                                        //dtSingle.DefaultView.RowFilter = cboSingle.ValueMember + "='" + CriteriaSource1 + "'";
                                        dtFilter = dtSingle.AsEnumerable().Where(r => GFunc.NEStr(r.Field<string>(cboSingle.ValueMember), "").Equals(CriteriaSource1));
                                    else
                                        //dtSingle.DefaultView.RowFilter = cboSingle.ValueMember + "='0'";
                                        dtFilter = dtSingle.AsEnumerable().Where(r => GFunc.NEStr(r.Field<string>(cboSingle.ValueMember), "").Equals('0'));
                                }
                                else
                                {
                                    if (!GFunc.IsNE(CriteriaSource1))
                                        //dtSingle.DefaultView.RowFilter = cboSingle.ValueMember + "=" + CriteriaSource1;
                                        dtFilter = dtSingle.AsEnumerable().Where(r => r.Field<int?>(cboSingle.ValueMember) == GFunc.NEInt(CriteriaSource1, 0));
                                    else
                                        // dtSingle.DefaultView.RowFilter = cboSingle.ValueMember + "=0";
                                        dtFilter = dtSingle.AsEnumerable().Where(r => r.Field<int?>(cboSingle.ValueMember) == 0);
                                }
                                string paraValue = "";
                                if (dtFilter.Count() > 0)
                                {
                                    paraValue = dtFilter.ElementAt(0)[cboSingle.DisplayMember].ToString();
                                    if (CriteriaNm == "DocDC")
                                        pDocCodeDes = paraValue;
                                }
                                // dtSingle.DefaultView.RowFilter = "";
                                if (CriteriaNm != "RepKey")
                                {
                                    if (CriteriaSource1 != string.Empty )
                                        pRepRange += CriteriaLabel + " = \"" + paraValue + "\"" + ",";
                                    else
                                        pRepRange += "FOR ALL " + CriteriaLabel + ",";
                                }
                            }

                        }

                        #endregion
                    }
                    else
                    {
                        if (GFunc.IsNE(CriteriaSource1) && CriteriaSource1 != null)
                            if (CriteriaRequired)
                            {
                                MsgBox.Show(CriteriaLabel + " Is Require.");
                                return null;
                            }

                        if (CriteriaType.Equals(GVar.ReportCriteriaType.Range))
                        {

                            if (CriteriaSetDateButton)
                            {
                                DateTime dt;
                                if (GFunc.IsNE(CriteriaSource1) == false)
                                {
                                    DateTime.TryParse(CriteriaSource1, out dt);
                                    param1 = new SqlParameter("@" + CriteriaNm + "From", String.Format("{0:dd/MMM/yyyy}", dt));
                                    pDateF = dt;
                                }
                                else
                                    param1 = new SqlParameter("@" + CriteriaNm + "From", DBNull.Value);

                                if (GFunc.IsNE(CriteriaSource2) == false)
                                {
                                    DateTime.TryParse(CriteriaSource2, out dt);
                                    param2 = new SqlParameter("@" + CriteriaNm + "To", String.Format("{0:dd/MMM/yyyy}", dt));
                                    pDateT = dt;
                                }
                                else
                                {
                                    param2 = new SqlParameter("@" + CriteriaNm + "To", DBNull.Value);
                                }

                            }
                            else
                            {
                                if (CriteriaHaveFormatSearch)
                                {
                                    param1 = new SqlParameter("@" + CriteriaNm + "From", "");
                                    param2 = new SqlParameter("@" + CriteriaNm + "To", "");
                                    CriteriaSource3 = CriteriaSource1;
                                    CriteriaSource1 = "";
                                    CriteriaSource2 = "";
                                }
                                else
                                {
                                    param1 = new SqlParameter("@" + CriteriaNm + "From", CriteriaSource1);
                                    param2 = new SqlParameter("@" + CriteriaNm + "To", CriteriaSource2);
                                }
                            }

                            if (GFunc.IsNE(CriteriaSource1) == false)
                            {
                                parmList.Add(param1);
                            }
                            if (GFunc.IsNE(CriteriaSource2) == false)
                            {
                                parmList.Add(param2);
                            }
                            if (CriteriaHaveFormatSearch)
                            {
                                parmList.Add(new SqlParameter("@" + CriteriaNm + "Wild", CriteriaSource3));
                            }
                            if (CriteriaNm != "RepKey")
                            {
                                if (CriteriaSource1 != string.Empty && CriteriaSource2 != string.Empty)
                                    pRepRange += CriteriaLabel + " Between \"" + CriteriaSource1 + "\" And \"" + CriteriaSource2 + "\"" + ",";
                                else
                                    pRepRange += "FOR ALL " + CriteriaLabel + ",";
                            }
                        }
                        else
                        {
                            if (CriteriaSetDateButton)
                            {
                                DateTime dt = new DateTime();
                                if (GFunc.IsNE(CriteriaSource1) == false)
                                    DateTime.TryParse(CriteriaSource1, out dt);
                                param1 = new SqlParameter("@" + CriteriaNm, String.Format("{0:dd/MMM/yyyy}", dt));
                                //if (GFunc.CompareString(CriteriaNm, "YTD"))
                                //{
                                pYTD = dt;
                                

                                //}
                            }
                            else if (GFunc.IsNE(CriteriaDefaultValue) == false && CriteriaHidden == true)
                            {
                                param1 = new SqlParameter("@" + CriteriaNm, CriteriaDefaultValue);

                            }
                            else
                            {
                                param1 = new SqlParameter("@" + CriteriaNm, CriteriaSource1);

                            }

                            //if (GFunc.IsNE(CriteriaSource1) == false)
                            //{
                            //    parmList.Add(param1);
                            //}

                            if (GFunc.IsNE(param1.Value) == false)
                            {
                                parmList.Add(param1);
                            }
                            
                            if (CriteriaNm != "RepKey")
                            {
                                if (CriteriaSource1 != string.Empty)
                                    pRepRange += CriteriaLabel.ToString() + " = \"" + CriteriaSource1 + "\"" + ",";
                                else
                                    pRepRange += "FOR ALL " + CriteriaLabel + ",";
                            }                            
                        }
                    }
                }
                if (pRepRange.LastIndexOf(",") > 0)
                {
                    pRepRange = pRepRange.Substring(0, pRepRange.Length - 1);
                }


                AddAdditionalParameters(parmList, RepKey, XMLAccSelected);
                return parmList;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ReportParameter> GetReportParameters()
        {
            try
            {


                REFCurr objCurr = null;
                List<ReportParameter> repportParmList = new List<ReportParameter>();
                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                string opCountryCurrSym = string.Empty;
                string opHomeCurrSym = string.Empty;
                DateTime PeriodEndDate;
                int Year;
                int Month;
                int TotalDaysInMonth;
                string sPeriod;



                objCurr = REFCurr.Get(SysOptionUtility.GetInt("CountryCurrency"));
                opCountryCurrSym = objCurr.SymHdom;

                objCurr = REFCurr.Get(1);
                opHomeCurrSym = objCurr.SymHdom;

                sPeriod = GFunc.NEStr(Period, "");

                foreach (DataRow obj in ReportParameters.Rows)
                {
                    ReportParameter l_Paramp = new ReportParameter(obj["ParName"].ToString(), false);
                    if (_RepPreviewBy == PeriverSource.Both || _RepPreviewBy == PeriverSource.Source1Only)
                    {
                        switch (obj["ParName"].ToString().ToLower())
                        {
                            case "pcmpname":
                                l_Paramp.ParameterValue = opCmpValue;
                                break;
                            case "preprange":
                                l_Paramp.ParameterValue = pRepRange;
                                break;
                            case "preptitle":
                                if (RepKey == 1145)//Document Transaction  Journal
                                {
                                    l_Paramp.ParameterValue = pDocCodeDes.Split('-')[0].ToString().TrimEnd() + " Transaction Journal";
                                }
                                else
                                    l_Paramp.ParameterValue = pRepTitle;
                                break;
                            case "pm1":
                                PeriodEndDate = GFunc.NEDateTime(pMth1, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm2":
                                PeriodEndDate = GFunc.NEDateTime(pMth2, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm3":
                                PeriodEndDate = GFunc.NEDateTime(pMth3, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm4":
                                PeriodEndDate = GFunc.NEDateTime(pMth4, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm5":
                                PeriodEndDate = GFunc.NEDateTime(pMth5, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm6":
                                PeriodEndDate = GFunc.NEDateTime(pMth6, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm7":
                                PeriodEndDate = GFunc.NEDateTime(pMth7, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm8":
                                PeriodEndDate = GFunc.NEDateTime(pMth8, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm9":
                                PeriodEndDate = GFunc.NEDateTime(pMth9, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm10":
                                PeriodEndDate = GFunc.NEDateTime(pMth10, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm11":
                                PeriodEndDate = GFunc.NEDateTime(pMth11, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pm12":
                                PeriodEndDate = GFunc.NEDateTime(pMth12, DateTime.Today.Date);
                                TotalDaysInMonth = DateTime.DaysInMonth(PeriodEndDate.Year, PeriodEndDate.Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + PeriodEndDate.Month.ToString() + "/" + PeriodEndDate.Year);//PeriodEndDate;
                                break;
                            case "pcountrycurrsym":
                                l_Paramp.ParameterValue = opCountryCurrSym;
                                break;
                            case "pdatef":
                                l_Paramp.ParameterValue = pDateF;
                                break;
                            case "pdatet":
                                l_Paramp.ParameterValue = pDateT;
                                break;
                            case "phomecurrsym":
                                l_Paramp.ParameterValue = opHomeCurrSym;
                                break;
                            case "prepperiodenddate":
                                if (obj["ParDataType"].ToString().ToLower() == "date")
                                {
                                    l_Paramp.ParameterValue = pDateT;  
                                }
                                else 
                                {
                                    Year = GFunc.NEInt(sPeriod.Substring(0, 4), 0);
                                    Month = GFunc.NEInt(sPeriod.Substring(4, 2), 0);
                                    TotalDaysInMonth = DateTime.DaysInMonth(Year, Month);
                                    l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + Month + "/" + Year);
                                }
                                break;
                            case "pdateytd":
                                l_Paramp.ParameterValue = pYTD;
                                break;
                            case "pagedate":
                                Year = GFunc.NEInt(sPeriod.Substring(0, 4), 0);
                                Month = GFunc.NEInt(sPeriod.Substring(4, 2), 0);
                                TotalDaysInMonth = DateTime.DaysInMonth(Year, Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + Month + "/" + Year);
                                break;
                            case "preportdate":
                                Year = GFunc.NEInt(sPeriod.Substring(0, 4), 0);
                                Month = GFunc.NEInt(sPeriod.Substring(4, 2), 0);
                                TotalDaysInMonth = DateTime.DaysInMonth(Year, Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + Month + "/" + Year); ;
                                break;
                            case "pcurrid1":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr1"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid2":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr2"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid3":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr3"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid4":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr4"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid5":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr5"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid6":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr6"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid7":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr7"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid8":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr8"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid9":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr9"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid10":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr10"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid11":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr11"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid12":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr12"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid13":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr13"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid14":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr14"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pcurrid15":
                                objCurr = REFCurr.Get(SysOptionUtility.GetInt("ItemPriceCurr15"));
                                l_Paramp.ParameterValue = objCurr.CurrID;
                                break;
                            case "pdoctypeuser":
                                l_Paramp.ParameterValue = GFunc.NEStr(pDocTypeNm,"");
                                break;
                            case "pexpirydate":
                                l_Paramp.ParameterValue = pYTD;
                                break;
                            case "pperiodfromdate":                            
                                Year = GFunc.NEInt(PeriodFrom.ToString().Substring(0, 4), 0);
                                Month = GFunc.NEInt(PeriodFrom.ToString().Substring(4, 2), 0);
                                TotalDaysInMonth = DateTime.DaysInMonth(Year, Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + Month + "/" + Year);
                                break;
                            case "pperiodtodate":
                                Year = GFunc.NEInt(PeriodTo.ToString().Substring(0, 4), 0);
                                Month = GFunc.NEInt(PeriodTo.ToString().Substring(4, 2), 0);
                                TotalDaysInMonth = DateTime.DaysInMonth(Year, Month);
                                l_Paramp.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + Month + "/" + Year);
                                break;
                            //
                            default:
                                //fViewr.rptDoc.Parameters[obj.ParName].PromptUser = true;
                                l_Paramp.ParameterValue = obj["ParDefaultValue"].ToString();
                                break;
                        }
                    }

                    repportParmList.Add(l_Paramp);
                }

                return repportParmList;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public bool MtnPara(DateTime AgeDate)
        {
            if (AgeDate.Year == 1)//AgeDate is hasn't set
            {
                AgeDate = DateTime.Today;
            }
            int days = 0;
            DateTime firstDate;


            days = (AgeDate.Day) - 1;
            firstDate = AgeDate.AddDays(-days);
            if (RepKey == 1280 || RepKey == 1282)
            {
                pMth1 = (firstDate.AddMonths(0).ToString("dd MMM yyyy"));
                pMth2 = (firstDate.AddMonths(-1).ToString("dd MMM yyyy"));
                pMth3 = (firstDate.AddMonths(-2).ToString("dd MMM yyyy"));
                pMth4 = (firstDate.AddMonths(-3).ToString("dd MMM yyyy"));
                pMth5 = (firstDate.AddMonths(-4).ToString("dd MMM yyyy"));
                pMth6 = (firstDate.AddMonths(-5).ToString("dd MMM yyyy"));
                pMth7 = (firstDate.AddMonths(-6).ToString("dd MMM yyyy"));
                pMth8 = (firstDate.AddMonths(-7).ToString("dd MMM yyyy"));
                pMth9 = (firstDate.AddMonths(-8).ToString("dd MMM yyyy"));
                pMth10 = (firstDate.AddMonths(-9).ToString("dd MMM yyyy"));
                pMth11 = (firstDate.AddMonths(-10).ToString("dd MMM yyyy"));
                pMth12 = (firstDate.AddMonths(-11).ToString("dd MMM yyyy"));
            }
            if (RepKey == 1875 || RepKey == 1880 || RepKey == 1890 || RepKey == 1510 || RepKey == 1511 || RepKey == 1525 || RepKey == 1527 || RepKey == 1512)
            {
                pMth1 = (firstDate.AddMonths(0).ToString("dd MMM yyyy"));
                pMth2 = (firstDate.AddMonths(-1).ToString("dd MMM yyyy"));
                pMth3 = (firstDate.AddMonths(-2).ToString("dd MMM yyyy"));
                pMth4 = (firstDate.AddMonths(-3).ToString("dd MMM yyyy"));
                pMth5 = (firstDate.AddMonths(-4).ToString("dd MMM yyyy"));
                pMth6 = (firstDate.AddMonths(-5).ToString("dd MMM yyyy"));
                pMth7 = (firstDate.AddMonths(-6).ToString("dd MMM yyyy"));
                pMth8 = (firstDate.AddMonths(-7).ToString("dd MMM yyyy"));
                pMth9 = (firstDate.AddMonths(-8).ToString("dd MMM yyyy"));
                pMth10 = (firstDate.AddMonths(-9).ToString("dd MMM yyyy"));
                pMth11 = (firstDate.AddMonths(-10).ToString("dd MMM yyyy"));
                pMth12 = (firstDate.AddMonths(-11).ToString("dd MMM yyyy"));
            }
            if (RepKey == 1305 || RepKey == 1550)
            {
                if (GFunc.CompareString(_sysRep.RPTname1, "Cust_Bal3YearF.rpt") || GFunc.CompareString(_sysRep.RPTname1, "Cust_Bal3YearH.rpt"))
                {
                    pMth1 = (firstDate.AddYears(0).ToString("yyyy"));
                    pMth2 = (firstDate.AddYears(-1).ToString("yyyy"));
                    pMth3 = (firstDate.AddYears(-2).ToString("yyyy"));
                    pMth4 = (firstDate.AddYears(-3).ToString("yyyy"));
                    pMth5 = (firstDate.AddYears(-4).ToString("yyyy"));
                    pMth6 = (firstDate.AddYears(-5).ToString("yyyy"));
                    pMth7 = (firstDate.AddYears(-6).ToString("yyyy"));
                    pMth8 = (firstDate.AddYears(-7).ToString("yyyy"));
                    pMth9 = (firstDate.AddYears(-8).ToString("yyyy"));
                    pMth10 = (firstDate.AddYears(-9).ToString("yyyy"));
                    pMth11 = (firstDate.AddYears(-10).ToString("yyyy"));
                    pMth12 = (firstDate.AddYears(-11).ToString("yyyy"));
                }
                else
                {
                    pMth1 = (firstDate.AddMonths(0).ToString("dd MMM yyyy"));
                    pMth2 = (firstDate.AddMonths(-1).ToString("dd MMM yyyy"));
                    pMth3 = (firstDate.AddMonths(-2).ToString("dd MMM yyyy"));
                    pMth4 = (firstDate.AddMonths(-3).ToString("dd MMM yyyy"));
                    pMth5 = (firstDate.AddMonths(-4).ToString("dd MMM yyyy"));
                    pMth6 = (firstDate.AddMonths(-5).ToString("dd MMM yyyy"));
                    pMth7 = (firstDate.AddMonths(-6).ToString("dd MMM yyyy"));
                    pMth8 = (firstDate.AddMonths(-7).ToString("dd MMM yyyy"));
                    pMth9 = (firstDate.AddMonths(-8).ToString("dd MMM yyyy"));
                    pMth10 = (firstDate.AddMonths(-9).ToString("dd MMM yyyy"));
                    pMth11 = (firstDate.AddMonths(-10).ToString("dd MMM yyyy"));
                    pMth12 = (firstDate.AddMonths(-11).ToString("dd MMM yyyy"));
                }
            }
            return true;
        }

        internal bool Dispose()
        {
            _ReportLoader = null;
            _sysRep = null;
            _sysRepRpts = null;
            _sysRepCriterias = null;
            _sysRepParas = null;
            return true;
        }

    }
}
