using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace BOLib
{
    public class GVar
    {
        [Serializable()]
        public delegate void DirtyEvent(object sender, System.ComponentModel.PropertyChangedEventArgs e);
        public delegate void ReadOnlyEvent(object sender, EventArgs e);
        public delegate void ErrorEvent(string errMsg, System.ComponentModel.PropertyChangedEventArgs e);
        public delegate void ListErrorEvent(string errMsg, System.ComponentModel.ListChangedEventArgs e);
        public delegate void UINotifierEvent(object sender, UINotifierEventArgs e);
        //Use In APPN Generate PO Process.
        public delegate void UINotifierEventWithCN(object sender, UINotifierEventArgs e, SqlConnection cn);

        public delegate void PopupSelectedEvent(int key, string ID);
        public delegate void PopupCommonSelectedEvent(int key, string ID, TAUtil.TAGridEditor grd);
        public delegate void JobPopupSelectedEvent(int key, int jobPhaseKey, int jobCostKey, int jobTaskKey, string ID);
        public delegate void PopupRichTextSelectedEvent(string plainText, string rtfText);
        public delegate void RecordSelectedEvent(int key);
        public delegate void DeleteRecordSelectedEvent(int key, bool toDelete);
        public delegate void ListUpdateEvent();
        public delegate void ItemBatchUpdateEvent(DataTable dtDetail);
        public delegate void ItemAttachmentUpdateEvent(bool attached);
        public delegate void InsertSOUpdateEvent(DataTable dtDetail);
        public delegate string TransferDataToSvrTempsEvent(SqlConnection cn);
        public delegate void PayMentByMonthEvent(DataTable dtDetail);
        public delegate void ApplyGainPopUpEvent(decimal ItmApplyRate, decimal ItmApplyDocAmtF, decimal ItmApplyDocAmtH, decimal ItmApplyPayAmtF, decimal ItmApplyPayAmtH, decimal GainAmt, int ItmApplyGainAccKey);

        public delegate void ListEvent_OpenRecord(int key);
        public delegate void ListEvent_DeleteRecord(int key);
        public delegate void ListEvent_CopyRecord(GEnum.CopyOption copyOption, int CopyDocCode, int CopyDocKey, DataTable CopyTable, bool NSLink);

        public delegate void ListEvent_RefreshRecord();
        public delegate void UpdateEvent_RefreshReport(SYSFinRepFactory objFactory,DataTable dtActiveReportData);
        public delegate void ListEvent_CloseFORM();
        
        
        public delegate void DocPrintUpdateEvent();

        public const string CancelMainFormClosing = "CancelMainFormClosing";

        public static bool ToDoCreateDocInProgress = false;
        public const string gcPass = "Process Completed OK";
        public const string gcCancel = "Process Cancelled non critical";
        public const string gcCritical = "Critical Process failure";
        public const string gcYes = "Yes";
        public const string gcNo = "No";

        //will maintain a boolean value with key name=>DeptUpdateOption. may add other key and values in future.
        public static Hashtable DocUpdateOption = new Hashtable();
        public const string DeptUpdateOption = "DeptUpdateOption";

        public const string EmailCheck = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[[0-9]{1,3}\\.[[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
        //Popup FORM key, id, des for Cust/Vend/Acc/Job/Itm
        public class PopupResult
        {
            public int key = 0;                 //ConKey,AccKey,JobKey,ItmKey
            public string id = string.Empty;    //ConID,AccID,JobID,ItmID
            public string des = string.Empty;   //ConNm,AccDes,JobDes,ItmDes
            public int ckey = 0;                //JobCostTypeKey
            public int pkey = 0;                //JobPhaseKey
            public int tkey = 0;                //JobTaskKey
            public int skey = 0;                //ItmSelectKey (Substitute ItmKey)
        }

        //Rounding ant decimal Place
        public static class RndDecs
        {
            public const int Qtypt = 4; //Stock/Assembly/Discount%/Charges% dec place
            public const int Conpt = 6; //UOM Conversion dec place
            public const int Prcpt = 6; //Price dec place
            public const int Amtpt = 2; //Amount dec place
            public const int Curpt = 8; //Currency Rate dec place
            public const int COSpt = 6; //For Added Stock Item Latest Cost % of Home Amount
        }
        public static class AddressType
        {
            public const string Mailing = "Mailing";
            public const string SalesOffice = "Sales Office";
            public const string ServiceCentre = "Service Centre";
            public const string Warehouse = "Warehouse";
            public const string WorkSite = "Work Site";

        }

        public static class ControlState
        {
            public const string BLANK = "";
            public const string A = "A";
            public const string N = "N";
            public const string R = "R";
            public const string O = "0";
            public const string D = "D";
        }
        public static class StockCountOption
        {
            public const string StockCountStartDate = "StockCountStartDate";
            public const string StockCountLastDate = "StockCountLastDate";
            public const string StockCountCompletedDate = "StockCountCompletedDate";
            public const string StockCountStatus = "StockCountStatus";
            public const string StockCountItemTotal = "StockCountItemTotal";
            public const string StockCountItemCounted = "StockCountItemCounted";
            public const string StockCountItemRemaining = "StockCountItemRemaining";

        }
        public static class ControlType
        {
            public const string TATextBoxEditor = "TATextBoxEditor";
            public const string TANumericEditor = "TANumericEditor";
            public const string TAGridEditor = "TAGridEditor";
            public const string TAComboBox = "TAComboBox";
            public const string TAComboBoxEditor = "TAComboBoxEditor";            
            public const string TADateEditor = "TADateEditor";
            public const string EmbeddableTextBoxWithUIPermissions = "EmbeddableTextBoxWithUIPermissions";
            
           
        }
        public static class RptPrintCondition
        {
            public const string BlockBelowMinMarkup = "BlockBelowMinMarkup";
        }

        public static class ReportCriteriaDataType
        {
            public const string BigInteger = "BIGINTEGER";
            public const string Boolean = "BOOLEAN";
            public const string Date = "DATE";
            public const string Decimal = "DECIMAL";
            public const string Integer = "INTEGER";
            public const string IntegerPeriod = "INTEGERPERIOD";
            public const string Money = "MONEY";
            public const string String = "STRING";

        }
        public static class ReportCriteriaType
        {
            public const string Range = "Range";
            public const string Range10 = "Range10";
            public const string Single = "Single";
            public const string SubFormSelection = "SubFormSelection";

        }

        public static class ReportOption
        {
            public const string FinancialStatementMeasurement = "Financial Statement Measurement";
        }
        public class FormName
        {
            public const string frmDocList = "frmDocList";
            public const string frmDocSearch = "frmDocSearch";
            public const string frmSYSMsgListText = "frmSYSMsgListText";
            public const string frmSYSCodeSYS_IDCounter = "frmSYSCodeSYS_IDCounter";
            public const string frmDocCopy = "frmDocCopy";
            public const string frmDocSelection = "frmDocSelection";
            public const string frmInsertPackingList = "frmInsertPackingList";
            public const string frmAssemblyEntry = "frmAssemblyEntry";
            public const string frmPrintSelectionList = "frmPrintSelectionList";
            public const string frmWOSearch = "frmWOSearch";
            public const string frmList = "frmList"; //ttm on 01/dec/2016
        }

        public class ListSettingID
        {
            public const string frmDocCopyGridAPRQ = "frmDocCopyGridAPRQ";
            public const string SYSRepRptDefaultRepKey = "SYSRepRptDefaultRepKey";


            public const string frmListGridConCust = "frmListGridConCust";
            public const string frmListGridConVend = "frmListGridConVend";
            public const string frmListGridItm = "frmListGridItm";
            public const string frmListGridJob = "frmListGridJob";
            public const string frmListGridAcc = "frmListGridAcc";
            public const string frmListGridAlert = "frmListGridAlert";
            public const string frmListGridPrice = "frmListGridPrice";
            public const string frmListGridShipName = "frmListGridShipName";
            public const string frmListGridToDo = "frmListGridToDo";
            public const string frmGLFinChargeGrid = "frmGLFinChargeGrid";
            public const string frmInsertDataMatrixMasterGrid = "frmInsertDataMatrixMasterGrid";
            public const string frmMSTBudgetCopyGrid = "frmMSTBudgetCopyGrid";
            public const string frmAttachmentGrid = "frmAttachmentGrid";

            public const string SYSMsgItmHisSearchType = "SYSMsgItmHisSearchType";
            public const string SYSMsgListINTypeReport = "SYSMsgINTypeReport";

            public const string SYSMsgDocApplyIVSalesCredit = "SYSMsgDocApplyIVSalesCredit";
            public const string SYSMsgDocApplyIVSalesCash = "SYSMsgDocApplyIVSalesCash";
            public const string SYSMsgDocApplyIVPurchase = "SYSMsgDocApplyIVPurchase";
            public const string frmInsertPODocID = "frmInsertPODocID";
            public const string frmInsertCODocID = "frmInsertCODocID";
            public const string frmInsertPDDocID = "frmInsertPDDocID";
            public const string SECGrpExcludeParameterGroup = "SECGrpExcludeParameterGroup";
            public const string frmAccSelectionGrid = "frmAccSelectionGrid";

            public const string MSTAccAll_id = "MSTAccAll_id";
            public const string MSTAccByCurr_id = "MSTAccByCurr_id";
            public const string MSTAccLiability_id = "MSTAccLiability_id";
            public const string MSTAccLiability_des = "MSTAccLiability_des";

            public const string MSTAccForeign_id = "MSTAccForeign_id";
            public const string MSTAccHome_id = "MSTAccHome_id";
            public const string MSTAccPayment_id = "MSTAccPayment_id";
            public const string MSTAccIncome_des = "MSTAccIncome_des"; /*added by YST to account as sales category especially for non-stock items , requested by Jia Ying from ADPL finance department */


            public const string MSTConAll_des = "MSTConAll_des";
            public const string MSTConAll_id = "MSTConAll_id";
            public const string MSTConBothCash_des = "MSTConBothCash_des";
            public const string MSTConBothCash_id = "MSTConBothCash_id";
            public const string MSTConBothCredit_des = "MSTConBothCredit_des";
            public const string MSTConBothCredit_id = "MSTConBothCredit_id";
            public const string MSTConPurchase_des = "MSTConPurchase_des";
            public const string MSTConPurchase_id = "MSTConPurchase_id";

            //ttm
            public const string MSTConALLPurchase_id = "MSTConALLPurchase_id";
            //ttm

            public const string MSTConPurchasePY_des = "MSTConPurchasePY_des";
            public const string MSTConPurchasePY_id = "MSTConPurchasePY_id";
            public const string MSTConSales_des = "MSTConSales_des";
            public const string MSTConSales_id = "MSTConSales_id";
            public const string MSTConSalesAll_des = "MSTConSalesAll_des";
            public const string MSTConSalesAll_id = "MSTConSalesAll_id";
            public const string MSTConJobsAll_id = "MSTConJobsAll_id";
            public const string MSTConJobsAll_des = "MSTConJobsAll_des";
            public const string MSTConSalesCash_des = "MSTConSalesCash_des";
            public const string MSTConSalesCash_id = "MSTConSalesCash_id";
            public const string MSTConSalesCredit_des = "MSTConSalesCredit_des";
            public const string MSTConSalesCredit_id = "MSTConSalesCredit_id";
            public const string MSTConSalesPYCash_des = "MSTConSalesPYCash_des";
            public const string MSTConSalesPYCash_id = "MSTConSalesPYCash_id";
            public const string MSTConSalesPYCredit_des = "MSTConSalesPYCredit_des";
            public const string MSTConSalesPYCredit_id = "MSTConSalesPYCredit_id";
            public const string MSTVehicle_des = "MSTVehicle_des";

            public const string MSTItmAll_id = "MSTItmAll_id";
            public const string MSTItmAll_des = "MSTItmAll_des";
            public const string MSTItmC_des = "MSTItmC_des";
            public const string MSTItmC_id = "MSTItmC_id";
            public const string MSTItmF_des = "MSTItmF_des";
            public const string MSTItmF_id = "MSTItmF_id";
            public const string MSTItmFS_des = "MSTItmFS_des";
            public const string MSTItmFS_id = "MSTItmFS_id";
            public const string MSTItmFSC_des = "MSTItmFSC_des";
            public const string MSTItmFSC_id = "MSTItmFSC_id";
            public const string MSTItmFSCAN_des = "MSTItmFSCAN_des";
            public const string MSTItmFSCAN_id = "MSTItmFSCAN_id";
            public const string MSTItmFSCANVG_des = "MSTItmFSCANVG_des";
            public const string MSTItmFSCANVG_id = "MSTItmFSCANVG_id";
            public const string MSTItmFSCN_des = "MSTItmFSCN_des";
            public const string MSTItmFSCN_id = "MSTItmFSCN_id";
            public const string MSTItmFSN_des = "MSTItmFSN_des";
            public const string MSTItmFSN_id = "MSTItmFSN_id";
            public const string MSTItmFSNVG_des = "MSTItmFSNVG_des";
            public const string MSTItmFSNVG_id = "MSTItmFSNVG_id";
            public const string MSTItmG_des = "MSTItmG_des";
            public const string MSTItmG_id = "MSTItmG_id";
            public const string MSTItmJob_des = "MSTItmJob_des";
            public const string MSTItmJob_id = "MSTItmJob_id";
            public const string MSTItmM_des = "MSTItmM_des";
            public const string MSTItmM_id = "MSTItmM_id";
            public const string MSTItmMCSG_id = "MSTItmMCSG_id";//consignment MasterItmtype
            public const string MSTItmH_id = "MSTItmH_id";
            public const string MSTItmR_id = "MSTItmR_id";
            public const string MSTItmN_des = "MSTItmN_des";
            public const string MSTItmN_id = "MSTItmN_id";
            public const string MSTItmPack_des = "MSTItmPack_des";
            public const string MSTItmPack_id = "MSTItmPack_id";
            public const string MSTItmPurchase_des = "MSTItmPurchase_des";
            public const string MSTItmPurchase_id = "MSTItmPurchase_id";
            public const string MSTItmSales_des = "MSTItmSales_des";
            public const string MSTItmSales_id = "MSTItmSales_id";
            public const string MSTItmType_des = "MSTItmType_des";
            public const string MSTItmType_id = "MSTItmType_id";
            public const string MSTItmV_des = "MSTItmV_des";
            public const string MSTItmV_id = "MSTItmV_id";
            public const string MSTJobAll_id = "MSTJobAll_id";
            public const string MSTJobSales_id = "MSTJobSales_id";
            public const string MSTJobAll_des = "MSTJobAll_des";
            public const string MSTJobSales_des = "MSTJobSales_des";


            public const string AssemblyItmList = "AssemblyItmList";

            public const string MSTShipMarkByShipNameKey = "MSTShipMarkByShipNameKey";
            public const string MSTShipMarkByShipName = "MSTShipMarkByShipName";
            public const string MSTAccAll_des = "MSTAccAll_des";
            public const string MSTAccByCurr_des = "MSTAccByCurr_des";

            public const string REFAddrByCon = "REFAddrByCon";
            public const string REFDocGrp = "REFDocGrp";
            public const string REFIndustry = "REFIndustry";
            public const string REFTerritory = "REFTerritory";
            public const string REFPayMode = "REFPayMode";
            public const string REFBank = "REFBank";

            public const string MSTJobSalesByConKey = "MSTJobSalesByConKey";

            public const string MSTItmAssemblyList = "AssemblyItmList";
            public const string MSTTransGrp = "MSTTransGrp";

            public const string MSTShipNameByConKey = "MSTShipNameByConKey";

            public const string SYSRepRpt = "SYSRepRpt";
            public const string REFContactInforConEmail = "REFContactInforConEmail";
            public const string DocApplyIVSalesCredit = "DocApplyIVSalesCredit";
            public const string DocApplyIVSalesCash = "DocApplyIVSalesCash";
            public const string DocApplyIVPurchase = "DocApplyIVPurchase";

            public const string MSTPriceListByCurrKey = "MSTPriceListByCurrKey";
            public const string REFAddrEmail_Con = "REFAddrEmail_Con";
            public const string SYSDocTypeNmByDC = "SYSDocTypeNmByDC";
            public const string SYSCounterGrpByDC = "SYSCounterGrpByDC";
            public const string SYSMsgSystemPeriodStatus = "SYSMsgSystemPeriodStatus";
            public const string VendorStatus = "RFQVendorStatus";
            public const string ReportSearchFormatCombo = "REFIDFormatValue";
            public const string SECGroupsByUserBrowseCombo = "SECGrpByUser";
            public const string SECPerm = "SECPerm";
            public const string REFScale = "REFScale";
            public const string REFUOM = "REFUOM";

            //for report criteria
            public const string MSTAccReport_id = "MSTAccReport_id";
            public const string MSTConPurchaseReport_id = "MSTConPurchaseReport_id";
            public const string MSTConReport_id = "MSTConReport_id";
            public const string MSTConSalesReport_id = "MSTConSalesReport_id";
            public const string MSTItmReport_id = "MSTItmReport_id";
            public const string MSTJobReport_id = "MSTJobReport_id";


        }

        public static class SystemOption
        {
            public class OptionType
            {
                public const string Date = "Date";
                public const string Bit = "bit";
                public const string Integer = "Integer";
            }

            public class System_Configuration
            {
                public const string DatabaseRegCode = "DatabaseRegCode";
                public const string BaseCurrency = "BaseCurrency";
                public const string CompanyName = "CompanyName"; //Added by Jane on 10-Jun-2014
            }

            public class Posting_Option
            {
                public const string DocumentMinMarkup = "DocumentMinMarkup";
            }

            public class Item_Defaults
            {
                public const string ItemSKULastCounter = "ItemSKULastCounter";
            }
            public class Document_Defaults
            {
                public const string DefaultItmHeader = "DefaultItmHeader";
                public const string DefaultItmRemark = "DefaultItmRemark";
            }
            public class ItemPriceList_CurrencySetup
            {
                //if changed, need to change in GetItmPrice function of UIDocRecCMUtility class, 
                //it is hard coded for looping purpose
                public const string ItemPriceCurr1 = "ItemPriceCurr1";
                public const string ItemPriceCurr10 = "ItemPriceCurr10";
                public const string ItemPriceCurr11 = "ItemPriceCurr11";
                public const string ItemPriceCurr12 = "ItemPriceCurr12";
                public const string ItemPriceCurr13 = "ItemPriceCurr13";
                public const string ItemPriceCurr14 = "ItemPriceCurr14";
                public const string ItemPriceCurr15 = "ItemPriceCurr15";
                public const string ItemPriceCurr2 = "ItemPriceCurr2";
                public const string ItemPriceCurr3 = "ItemPriceCurr3";
                public const string ItemPriceCurr4 = "ItemPriceCurr4";
                public const string ItemPriceCurr5 = "ItemPriceCurr5";
                public const string ItemPriceCurr6 = "ItemPriceCurr6";
                public const string ItemPriceCurr7 = "ItemPriceCurr7";
                public const string ItemPriceCurr8 = "ItemPriceCurr8";
                public const string ItemPriceCurr9 = "ItemPriceCurr9";

            }

            public class AccountDefaults
            {
                public const string DefaultAccountType = "DefaultAccountType";
                public const string AccUnrealisedLoss = "AccUnrealisedLoss";
                public const string AccUnrealisedGain = "AccUnrealisedGain";
                public const string AccRoundingAdj = "AccRoundingAdj";
                public const string AccRetainEarning = "AccRetainEarning";
                public const string AccLandedCost = "AccLandedCost";
                public const string AccGSTCustom = "AccGSTCustom";
                public const string AccExchangeLoss = "AccExchangeLoss";
                public const string AccExchangeGain = "AccExchangeGain";
                public const string AccDiscountReceived = "AccDiscountReceived";
                public const string AccDiscountAllowed = "AccDiscountAllowed";
                public const string AccCostAdjustment = "AccCostAdjustment";
            }

            public class SystemWarning
            {
                public const string WarnDeleteRecord = "WarnDeleteRecord";
                public const string WarnDeleteRecordDetail = "WarnDeleteRecordDetail";
                public const string WarnOpenRecordAsReadOnly = "WarnOpenRecordAsReadOnly";
                public const string WarnDeleteDocument = "WarnDeleteDocument";
                public const string WarnDeleteDocumentDetail = "WarnDeleteDocumentDetail";
                public const string WarnResaveDocument = "WarnResaveDocument";
                public const string WarnOpenDocumentAsReadOnly = "WarnOpenDocumentAsReadOnly";
                public const string WarnCurrencyRateNotValid = "WarnCurrencyRateNotValid";
                public const string WarnCountryRateNotValid = "WarnCountryRateNotValid";
                public const string WarnNonConvertableUOM = "WarnNonConvertableUOM";
                public const string WarnRestoreRecord = "WarnRestoreRecord";
                public const string WarnPriceByTranInValid = "WarnPriceByTranInValid";
                public const string WarnCurrencyPositionNotValid = "WarnCurrencyPositionNotValid";
                public const string WarnClearRecord = "WarnClearRecord";
                public const string QuestionOpenExportFile = "QuestionOpenExportFile";
            }

            public class OpID
            {
                public const string UseDept = "UseDept";
                public const string UseBranch = "UseBranch";
                public const string UseMultiDO = "UseMultiDO";
                public const string UseProject = "UseProject";
                public const string UseDefaultEntry = "UseDefaultEntry";
                public const string UseQtyDiscountPricing = "UseQtyDiscountPricing";
                
                public const string CashFlowStartDate = "CashFlowStartDate";
                public const string CashFlowIncludeOverdraft = "CashFlowIncludeOverdraft";

                public const string DefaultDateValid = "DefaultDateValid";
                public const string DefaultDocMarkupType = "DefaultDocMarkupType";
                public const string DefaultInventoryLocation = "DefaultInventoryLocation";
                public const string DefaultARIVPaymentAcc = "DefaultARIVPaymentAcc";
                public const string DefaultARIVPaymentMode = "DefaultARIVPaymentMode";
                public const string DefaultPaymentMode = "DefaultPaymentMode";
                public const string DefaultPaymentTaxGrp = "DefaultPaymentTaxGrp";
                public const string DefaultAPPDAcc = "DefaultAPPDAcc";
                public const string DefaultINAdjAcc = "DefaultINAdjAcc";
                public const string DefaultOverallDiscountAccountAR = "DefaultOverallDiscountAccountAR";
                public const string DefaultOverallDiscountAccountAP = "DefaultOverallDiscountAccountAP";

                public const string ARSOAutoUnTrack = "ARSOAutoUnTrack";
                public const string APPOAutoUnTrack = "APPOAutoUnTrack";
                public const string INCSOAutoUnTrack = "INCSOAutoUnTrack";

                public const string InsertARSOUseMaxStock = "InsertARSOUseMaxStock";
                public const string InsertARSOUseSysPrice = "InsertARSOUseSysPrice";
                public const string InsertAPPOUseSysPrice = "InsertAPPOUseSysPrice";
                public const string InsertDataUseSysPrice = "InsertDataUseSysPrice";
                public const string InsertDataWithNSLink = "InsertDataWithNSLink";

                public const string AutoApply = "AutoApply";
                public const string ARDocPrintIncludeSODetail = "ARDocPrintIncludeSODetail";
                public const string DefaultDataMatrixSelection = "DefaultDataMatrixSelection";
                public const string DocListDefaultDateRange = "DocListDefaultDateRange";
                public const string DocMaximumRow = "DocMaximumRow";
                public const string DocumentDetailAutoMarkingNumbering = "DocumentDetailAutoMarkingNumbering";
                public const string DocumentDetailChangeVendorChangePrice = "DocumentDetailChangeVendorChangePrice";
                public const string ShowEnquiryScreen = "ShowEnquiryScreen";
                public const string ShipMarkInitialNumber = "ShipMarkInitialNumber";
                public const string UOMDesOption = "UOMDesOption";

                public const string MFNOverHeadAcc = "MFNOverHeadAcc";
                public const string MFNRoundingAcc = "MFNRoundingAcc";
                public const string MFNCostMode = "MFNCostMode";

                public const string PriceDecPlace = "PriceDecPlace";
                public const string PriceRoundMode = "PriceRoundMode";

                public const string DepartmentOptionForAR = "DepartmentOptionForAR";
                public const string DepartmentOptionForAP = "DepartmentOptionForAP";
                public const string DepartmentOptionForCSG = "DepartmentOptionForCSG";
                public const string DepartmentOptionForIN = "DepartmentOptionForIN";
                public const string DepartmentOptionForGL = "DepartmentOptionForGL";

                public const string SystemMessageListTable = "SYS_MsgList";
                public const string FoundDialog = "FoundDialog";
                public const string FieldBehavior = "FieldBehavior";
                public const string ArrowBehavior = "ArrowBehavior";
                public const string PasswordMinLength = "PasswordMinLength";
                public const string PasswordMaxLength = "PasswordMaxLength";

                public const string ARPLPackingType = "ARPLPackingType";
                public const string ARPLWeightUOM = "ARPLWeightUOM";
                public const string ARPLMeasurementUOM = "ARPLMeasurementUOM";
                public const string ARPLTLMeasurementUOM = "ARPLTLMeasurementUOM";

                public const string ARPLHeaderCaption1 = "ARPLHeaderCaption1";
                public const string ARPLHeaderCaption2 = "ARPLHeaderCaption2";
                public const string ARPLHeaderCaption3 = "ARPLHeaderCaption3";
                public const string ARPLHeaderCaption4 = "ARPLHeaderCaption4";
                public const string ARPLHeaderCaption5 = "ARPLHeaderCaption5";
                public const string ARPLTotalItemText = "ARPLTotalItemText";

                public const string EmailPassword = "EmailPassword";
            }
        }

        public static class PermissionID
        {
            //public const string Address = "Addr";

            // added by ttm on 26-Jan-2017 (start)
            public const string KeyCustomer = "KeyCustomer";
            public const string KeyCustomerImport = "KeyCustomerImport";
            // added by ttm on 26-Jan-2017 (end)

            // added by ttm on 11-Jan-2017 (start)
            public const string Sales_Approver = "SalesApproval";
            // added by ttm on 11-Jan-2017 (end)
            public const string PurchaseReturnRequest = "PRRequest";
            public const string POAmendRequest = "POAmendRequest";
            public const string SaleReturnRequest = "SRRequest";
            public const string InventoryReturnRequest = "InvRequest";

            public const string Purchase_Adjustment = "APAJ";
            public const string Save_Printed_Purchase_Adjustment = "APAJSavePrinted";
            public const string Purchase_Invoice = "APBL";
            public const string Save_Printed_Purchase_Invoice = "APBLSavePrinted";
            public const string Purchase_Credit_Note = "APCN";
            public const string Save_Printed_Purchase_Credit_Note = "APCNSavePrinted";
            public const string Purchase_Debit_Note = "APDN";
            public const string Save_Printed_Purchase_Debit_Note = "APDNSavePrinted";
            public const string Purchase_Shipment = "APPS";
            public const string Purchase_Delivery = "APPD";
            public const string Save_Printed_Purchase_Delivery = "APPDSavePrinted";
            public const string PO_Adjustment = "APPJ";
            public const string APPJApprove = "APPJApprove";
            public const string Save_Printed_PO_Adjustment = "APPJSavePrinted";
            public const string Purchase_Planning = "APPN";
            public const string APPNApprove = "APPNApprove";
            public const string Save_Printed_Purchase_Plan = "APPNSavePrinted";
            public const string Purchase_Order = "APPO";
            public const string APPOApproveLimit1 = "APPOApproveLimit1";
            public const string APPOApproveLimit2 = "APPOApproveLimit2";
            public const string APPOApproveLimit3 = "APPOApproveLimit3";
            public const string APPOApproveLimit4 = "APPOApproveLimit4";
            public const string APPOApproveLimit5 = "APPOApproveLimit5";
            public const string APPOApproveNoLimit = "APPOApproveNoLimit";            
           
            public const string Able_to_edit_PO_that_has_been_link_to_Document = "APPOEditLinked";
            public const string Save_Printed_Purchase_Order = "APPOSavePrinted";
            public const string Untrack_PO_Document = "APPOUntrack";
         
            public const string Payment_Issue = "APPY";
            public const string Save_Printed_Payment_Issue = "APPYSavePrinted";
            public const string Purchase_Requisition = "APRequisition";
            public const string Purchase_Request = "APRQ";
            public const string APRQApprove = "APRQApprove";
            public const string Save_Printed_Purchase_Request = "APRQSavePrinted";
            public const string Sales_Adjustment = "ARAJ";
            public const string Cash_Adjustment = "ARAJC";
            public const string Save_Printed_Cash_Adjustment = "ARAJCSavePrinted";
            public const string Save_Printed_Sales_Adjustment = "ARAJSavePrinted";
     
            public const string Sales_Credit_Note = "ARCN";
            public const string Cash_Credit_Note = "ARCNC";
            public const string Save_Printed_Cash_Credit_Note = "ARCNCSavePrinted";
            public const string Save_Printed_Sales_Credit_Note = "ARCNSavePrinted";
            public const string Sales_Debit_Note = "ARDN";
            public const string Cash_Debit_Note = "ARDNC";
            public const string Save_Printed_Cash_Debit_Note = "ARDNCSavePrinted";
            public const string Save_Printed_Sales_Debit_Note = "ARDNSavePrinted";
            public const string Delivery_Order = "ARDO";
            public const string Save_Printed_Delivery_Order = "ARDOSavePrinted";
            public const string DO_to_IV_Transfer = "ARDOTransfer";
            public const string Sales_Invoice = "ARIV";
            public const string Cash_Sale = "ARIVC";
            public const string Save_Printed_Cash_Sale = "ARIVCSavePrinted";
            public const string Save_Printed_Sales_Invoice = "ARIVSavePrinted";
            public const string Packing_List = "ARPL";
            public const string Save_Printed_Packing_List = "ARPLSavePrinted";           
            public const string Payment_Received = "ARPY";
            public const string Cash_Payment_Received = "ARPYC";
            public const string Save_Printed_Cash_Payment_Received = "ARPYCSavePrinted";
            public const string Save_Printed_Payment_Received = "ARPYSavePrinted";
            public const string Quotation = "ARQO";
            public const string ARQOApproveLimit1 = "ARQOApproveLimit1";
            public const string ARQOApproveLimit2 = "ARQOApproveLimit2";
            public const string ARQOApproveLimit3 = "ARQOApproveLimit3";
            public const string ARQOApproveLimit4 = "ARQOApproveLimit4";
            public const string ARQOApproveLimit5 = "ARQOApproveLimit5";
            public const string ARQOApproveNoLimit = "ARQOApproveNoLimit";
            public const string Save_Printed_Quotation = "ARQOSavePrinted";
            public const string Sales_Order_Adjustment = "ARSJ";
            public const string ARSJApprove = "ARSJApprove";
            public const string Save_Printed_SO_Adjustment = "ARSJSavePrinted";
            public const string Sales_Order = "ARSO";
            public const string ARSOApproveLimit1 = "ARSOApproveLimit1";
            public const string ARSOApproveLimit2 = "ARSOApproveLimit2";
            public const string ARSOApproveLimit3 = "ARSOApproveLimit3";
            public const string ARSOApproveLimit4 = "ARSOApproveLimit4";
            public const string ARSOApproveLimit5 = "ARSOApproveLimit5";
            public const string ARSOApproveNoLimit = "ARSOApproveNoLimit";
            public const string Able_to_edit_SO_that_has_been_link_to_Document = "ARSOEditLinked";
            public const string Save_Printed_Sales_Order = "ARSOSavePrinted";
            public const string Untrack_SO_Document = "ARSOUntrack";
            public const string Contra = "CMCT";
            public const string Cash_Contra = "CMCTC";
            public const string Save_Printed_Cash_Contra = "CMCTCSavePrinted";
            public const string Save_Printed_Contra = "CMCTSavePrinted";

            public const string Customer_Record = "CVCustomer";
            public const string Customer_Opening_Balance = "CVCustOpenBal";
            public const string Edit_Credit_Limit = "CVEditLimit";
            public const string Industry = "CVIndustry";
            public const string Add_Customer_Remark = "AddCusRemark";
            public const string Packing_Type = "CVPackingType";
            public const string Payment_Term = "CVPaymentTerm";
            public const string Price_List_Batch_Update = "CVPriceUpdate";
            public const string Ship_Name = "CVShipName";
            public const string Shipping_Mode = "CVShipVia";
            public const string Territory = "CVTerritory";
            public const string Vendor_Opening_Balance = "CVVendOpenBal";
            public const string Vendor_Record = "CVVendor";
            public const string Machine_Configuration = "EqptConfiguration";
            public const string Machine_Type = "EqptType";
            public const string Account = "GLAcc";
            public const string Account_Group = "GLAccGrp";
            public const string Account_Opening_Balance = "GLAccOpenBal";
            public const string Account_Opening_Balance_Unreconciled_transactions = "GLAccUnReconciledTrans";
            public const string Bank = "GLBank";
            public const string Bank_Reconciliation = "GLBankRecon";
            public const string Branch = "GLBranch";
            public const string Account_Budget = "GLBudget";
            public const string Cash_Flow = "GLCashFlow";
            public const string Cost_of_Sales_batch_posting = "GLCOSBatchPost";
            public const string Currency = "GLCurrency";
            public const string Department = "GLDept";
            public const string Department_Distribution = "GLDeptDistribution";
            public const string Bank_Deposit_Slip = "GLDP";
            public const string Save_Printed_Deposit_Slip = "GLDPSavePrinted";
            public const string Sales_Representative = "GLEmployee";
            public const string Sales_Representative_Payroll = "GLEmployeePayroll";
            public const string Financial_Charge = "GLFinCharge";
            public const string Finance_Charge_Interest_Rates = "GLInterestRate";
            public const string Journal = "GLJN";
            public const string Save_Printed_Journal = "GLJNSavePrinted";
            public const string Overhead_Cost = "GLOverhead";
            public const string Payment_Mode = "GLPaymentMode";
            public const string Accounting_Period = "GLPeriod";          
            public const string ARAP_Revaluation = "GLRevalueARAP";
            public const string Bank_Revaluation = "GLRV";
            public const string Save_Printed_Bank_Revaluation = "GLRVSavePrinted";
            public const string Tax_Authority = "GLTaxAuthority";
            public const string Tax_Group = "GLTaxGroup";
            public const string Transaction_Group = "GLTranGrp";           
            public const string Inventory_Adjustment = "INADJ";
            public const string Save_Printed_Inventory_Adjustment = "INADJSavePrinted";
            public const string Received_Consignment = "INCPD";
           
            public const string Save_Printed_Received_Consignment = "INCPDSavePrinted";
            public const string Order_Consignment_Adjustment = "INCPJ";
            public const string CSCPJApprove = "INCPJApprove";
            public const string Save_Printed_CO_Adjustment = "INCPJSavePrinted";
            public const string Order_Consignment = "INCPO";
            public const string CSCPOApproveLimit1 = "INCPOApproveLimit1";
            public const string CSCPOApproveLimit2 = "INCPOApproveLimit2";
            public const string CSCPOApproveLimit3 = "INCPOApproveLimit3";
            public const string CSCPOApproveLimit4 = "INCPOApproveLimit4";
            public const string CSCPOApproveLimit5 = "INCPOApproveLimit5";
            public const string Approve_Order_Consignment_with_no_limit = "INCPOApproveNoLimit";
            public const string Able_to_edit_Order_Consignment_that_has_been_linked_to_Document = "INCPOEditLinked";
            public const string Save_Printed_Order_Consignment = "INCPOSavePrinted";
            public const string UnTrack_Order_Consignment = "INCPOUnTrack";
            public const string Consignment_Settlement = "INCPS";
            public const string Save_Printed_Consignment_Settlement = "INCPSSavePrinted";
            public const string Issue_Consignment = "INCSI";
            public const string Able_to_edit_Issue_Consignment_that_has_been_linked_to_Document = "INCSIEditLinked";
            public const string Save_Printed_Issue_Consignment = "INCSISavePrinted";
            public const string UnTrack_Issue_Consignment = "INCSIUnTrack";
            public const string Return_Consignment = "INCSR";
            public const string Save_Printed_Return_Consignment = "INCSRSavePrinted";
            public const string Inventory_Production = "INPDT";
            public const string INPDTApprove = "INPDTApprove";
            public const string Save_Print_Inventory_Production = "INPDTSavePrinted";
            public const string Inventory_Transfer = "INTRN";
            public const string Save_Printed_Inventory_Transfer = "INTRNSavePrinted";
            public const string Inventory = "Item";
            public const string Brand = "ItemBrand";
            public const string Category = "ItemCategory";
            public const string Color = "ItemColor";
            public const string Edit_Item_Control_Price = "ItemControlPrice";
            public const string Edit_Item_BOM = "ItemEditBOM";          
            public const string Item_Location = "ItemLocation";
            public const string Inventory_Opening_Balance = "ItemOpenBal";
            public const string Scale_and_Size = "ItemScale";
           
            public const string Stock_Take = "ItemStocktake";
            public const string Unit_of_Measure = "ItemUOM";
            public const string View_Item_Cost = "ItemViewCost";
            public const string Job = "Job";
            public const string Job_Cost_Type = "JobCostType";
            public const string Job_Group = "JobGroup";
            public const string Job_Detail = "JobDetail";
            public const string Job_Phase = "JobPhase";
            public const string Job_Task = "JobTask";
            public const string Job_TimeSheet = "JobTimeSheet";
            public const string Price_List = "PriceList";
            public const string Accounts_Reports = "RepAccount";
            public const string Customer_Reports = "RepCustomer";
            public const string AP_Documents_Reports = "RepDocAP";
            public const string AR_Documents_Reports = "RepDocAR";
            public const string Inventory_Documents_Reports = "RepDocIN";
            public const string Consignment_Documents_Reports = "RepDocCS";
            public const string Financial_Reports = "RepFinancial";
            public const string Report_Wildcard_ID_Format = "RepIDFormat";
            public const string Inventory_Reports = "RepItem";
            public const string Job_Report = "RepJob"; 
            public const string Others_Report_Setting = "RepOthersSet";
           
            public const string Report_Rpt_File_Setting = "RepRptFileSet";
            public const string Security_Reports = "RepSecurity";
            public const string Administration_Reports = "RepSystem";
            public const string Customer_Manage = "CustomerManage";
            public const string Vendor_Reports = "RepVendor";
            public const string Security_Groups = "SecGroup";
            public const string Security_Password = "SecPassWord";

            public const string Assignment_of_Restricted_Records = "SecRecAccessAssign";          
            public const string Security_Users = "SecUser";
            public const string Alerts = "SysAlert";         
            public const string Audit_Log = "SysAuditLog";
            public const string Company_Setup_Check_List = "SysCmpSetup";
            public const string Document_Type_and_Numbering = "SysCode";
            public const string Able_to_Import_Data_from_external_source = "SysDataImport";
            public const string Document_Group = "SysDocGroup";
            public const string Ability_to_modify_Document_ID = "SysDocID";
            public const string Able_to_print_document_with_details_below_minimum_markup = "SecDocPrintMinMarkup";
            public const string General_List = "SysGeneralList";     
            public const string System_Option = "SysOption";
            public const string User_Option = "SysOptionUser";
            public const string Year_End_Data_Purging = "SysPurgeData";
            public const string Ability_to_modify_Record_ID = "SysRecID";         
            public const string To_Do = "SysToDo";           
            public const string Year_End_Process = "SysYearEnd";

            public const string Work_Order_Type = "WorkOrderType";
            public const string WO_ReqType = "WorkOrderReqType";
            public const string MST_Vehicle = "Vehicle";
            public const string Work_Order = "WorkOrder";

            //
            public const string ARDOTrack = "DOTrack";
           
        }

        public static class UpdateType
        {
            public const string Svr = "Svr";
            public const string Obj = "Obj";
            public const string All = "All";

        }  
    }

    public class MsgID
    {

        public const string ItemDeleteFailOpeningBalNotZero = "ItemDeleteFailOpeningBalNotZero";

        public static class BatchEntry
        {
            public static string InvalidBatchQuantity = "InvalidBatchQuantity";
            public static string NotEnoughQuantity = "NotEnoughQuantity";
            public static string BatchIDAlreadyExists = "BatchIDAlreadyExists";
        }

        public static class Common
        {
            public const string SysErr = "SysErr";
            public const string NewFail = "RecordNewFail";
            public const string GetFail = "RecordGetFail";
            public const string GetFailWithID = "RecordGetFailWithID";
            public const string AddFail = "RecordAddFail";
            public const string SaveFail = "RecordSaveFail";
            public const string UpdateFail = "RecordUpdateFail";
            public const string DeleteFail = "RecordDeleteFail";
            public const string CopyFail = "RecordCopyFail";
            public const string InitialisationFail = "InitialisationFail";
            public const string ValidationFail = "RecordValidationFail";
            public const string RecordIsReadOnly = "RecordIsReadOnly";
            public const string WrongInstanceMode = "MethodNotForThisInstanceMode";
            public const string AddNewDetailFail = "AddNewDetailFail";
            public const string UnableToValidate = "UnableToValidate";
            public const string SaveChanges = "SaveChanges";
            public const string ConfirmDelete = "ConfirmDelete";
            public const string ConfirmClear = "ConfirmClear";
            public const string ConfirmSave = "ConfirmSave";
            public const string ConfirmAdd = "ConfirmAdd";
            public const string ConfirmReSave = "ConfirmReSave";
            public const string ResponseOpenAsReadOnly = "ResponseOpenAsReadOnly";
            public const string ResponseDeleteRecord = "ResponseDeleteRecord";
            public const string NoMultiInstanceAllowed = "NoMultiInstanceAllowed";
            public const string DisposeFail = "DisposeFail";
            public const string RecordDetailValidationFail = "RecordDetailValidationFail";
            public const string GetInforFail = "GetInforFail";
            public const string InvalidCellDataTypeDate = "InvalidCellDataTypeDate";
            public const string InvalidCellDataTypeNumeric = "InvalidCellDataTypeNumeric";
            public const string SearchFail = "SearchFail";
            public const string ItemNotInList = "ItemNotInList";
            public const string AddNewRecord = "AddNewRecord";
            public const string ConfirmCopy = "ConfirmCopy";
            public const string PropertyNotFound = "PropertyNotFound";
            public const string CannotBeEmpty = "CannotBeEmpty";
            public const string MustBeSame = "MustBeSame";
            public const string InvalidParameters = "InvalidParameters";
            public const string InvalidParametersWithFields = "InvalidParametersWithFields";
            public const string RetriveInforFail = "RetriveInforFail";
            public const string UnableToGetPriceRatio = "UnableToGetPriceRatio";
            public const string IncorrectEmailAddress = "IncorrectEmailAddress";
            public const string GetPaymentListFail = "GetPaymentListFail";
            public const string ConfirmToSubstitute = "ConfirmToSubstitute";
            public const string ConfirmtoResetStockCount = "ConfirmtoResetStockCount";
            public const string ConfirmtoAlreadyStockCount = "ConfirmtoAlreadyStockCount";
            public const string ConfirmtoAlreadyStockGenearte = "ConfirmtoAlreadyStockGenearte";
        }

        public static class ItemPrepare
        {
            public const string InsufficientStock = "InsufficientStock";
        }
        public static class SalesRepresentive
        {
            public const string PayrollProcessPermissionDenied = "PayrollProcessPermissionDenied";
            public const string PayrollValidationFail = "PayrollDetailValidationFail";
        }
        public static class Document
        {

            public const string FGCannotSelect = "FGCannotSelect";//Invalid: Can not select this Finished Goods because this is not in your finished goods list.
            public const string DocumentCodeNotMatch = "DocumentCodeNotMatch";//Error: cannot match Document Code
            public const string OneRowmustbeSelected = "OneRowmustbeSelected";
            public const string CustomerIDcannotbeEmpty = "CustomerIDcannotbeEmpty";
            public const string ShipNamecannotbeEmpty = "ShipNamecannotbeEmpty";
            public const string GeneratenewShipmark = "GeneratenewShipmark";
            public const string CurrencyRateNotCurrent = "CurrencyRateNotCurrent";//"Currency rate is not current."
            public const string CountryCurrencyRateNotCurrent = "CountryCurrencyRateNotCurrent";//"Country Currency rate is not current."
            public const string ConfirmRegenerateItemMarkingSequence = "ConfirmRegenerateItemMarkingSequence";//"Confirm Re generate Item marking sequence?"
            public const string ConfirmZeroOffBalance = "ConfirmZeroOffBalance";//"Confirm Zero Off Balance?"
            public const string ConfirmGenerateItemVendor = "ConfirmGenerateItemVendor";//"Confirm generate Item vendors?"
            public const string ConfirmResetAllItmLoc = "ConfirmResetAllItmLoc";//"Confirm reset all item locations with this?"
            public const string ConKeyIsRequired = "ConKeyIsRequired";//"ConKeyIsRequired"
            public const string AccKeyIsRequired = "AccKeyIsRequired";//"AccKeyIsRequired"
            public const string CurrKeyNotMatchWithConPriceType = "CurrKeyNotMatchWithConPriceType";//Invoice input currency is different with Cust/Vend Currency.
            public const string CurrKeyNotFoundInSysOption = "CurrKeyNotFoundInSysOption";//Currency is not found among values of SysOption "ItemPrice1" to "ItemPrice15"
            public const string CurrCannotChangePaymentMade = "CurrConnotChangePaymentMade"; //Can't change currency when Payment has been made
            public const string CurrCannotChangePaymentApplied = "CurrCannotChangePaymentApplied"; //Can't change currency when Payment has been APPLIED            

            public const string InactiveSelection = "InactiveSelection";  //"The " & fieldname & " selected is INACTIVE, you can't select this " & fieldname
            public const string DefaultDiscAccNotSet = "DefaultDiscAccNotSet"; //Your Default Discount Account has not been setup, Please setup the default discount account in company System tab
            public const string NoUOMConversionRate = "NoUOMConversionRate";

            public const string SetDetDepartmentWhenHDRChange = "SetDetDepartmentWhenHDRChange";
            public const string SetDetTranGrpWhenHDRChange = "SetDetTranGrpWhenHDRChange";
            public const string PostedDocCannotBeSaved = "PostedDocCannotBeSaved";
            public const string PostedDocCannotBeSubmitted = "PostedDocCannotBeSubmitted";
            public const string PostedDocCannotBeApproved = "PostedDocCannotBeApproved";
            public const string PostedDocCannotBeRejected = "PostedDocCannotBeRejected";
            public const string DocCannotBeSavedAsDraft = "DocCannotBeSavedAsDraft";
            public const string DocCannotBeSubmitted = "DocCannotBeSubmitted";
            public const string NotApprovedDoc = "NotApprovedDoc";
            public const string NeedApprovalDoc = "NeedApprovalDoc";
            public const string NeedRejectDoc = "NeedRejectDoc";
            public const string NeedSubmittionDoc = "NeedSubmittionDoc";
            public const string DocCannotBeRejected = "DocCannotBeRejected";
            public const string NoAuthorityToRejectDoc = "NoAuthorityToRejectDoc";
            public const string NoAuthorityToApproveDoc = "NoAuthorityToApproveDoc";
            public const string DocDateChangeSystemDate = "DocDateChangeSystemDate";
            public const string DocCannotBeResaved = "DocCannotBeResaved";
            public const string InvoiceWarnDeleteDO = "InvoiceWarnDeleteDO";
            public const string UserCancelDeleteOperation = "UserCancelDeleteOperation";
            public const string AppliedDocCannotBeDeleted = "AppliedDocCannotBeDeleted";
            public const string InvoicedDocCannotBeDeleted = "InvoicedDocCannotBeDeleted";
            public const string ReconciledDocCannotBeDeleted = "ReconciledDocCannotBeDeleted";
            public const string WarnPostedDocDelete = "WarnPostedDocDelete";
            public const string DocUseInTemplate = "DocUseInTemplate";
            public const string WarnDeleteDocHasLinked = "WarnDeleteDocHasLinked";
            public const string PostPeriodFallInRevaluationPeriod = "PostPeriodFallInRevaluationPeriod";
            public const string PeriodIsClosed = "PeriodIsClosed";
            public const string ItemsBelowSaleLimit = "ItemsBelowSaleLimit";
            public const string ConfirmSaveAmountExceedCreditLimit = "ConfirmSaveAmountExceedCreditLimit";
            public const string UserCancelSaveOperation = "UserCancelSaveOperation";
            public const string SaveFailCreditLimitExceed = "SaveFailCreditLimitExceed";
            public const string WarnOutofStockSaving = "WarnOutofStockSaving";
            public const string SaveFailOutofStock = "SaveFailOutofStock";
            public const string ValidateFailOnReconcileOrDepositRec = "ValidateFailOnReconcileOrDepositRec";
            public const string UnBalancedPostingError = "UnBalancedPostingError";
            public const string BatchUsedValidateFail = "BatchUsedValidateFail";
            public const string CVCannotBeChangedDocLinked = "CVCannotBeChangedDocLinked";
            public const string SaveFailDetailCalculationWrong = "SaveFailDetailCalculationWrong";
            public const string SaveFailTotalCalculationWrong = "SaveFailTotalCalculationWrong";
            public const string SaveFailTotalCalculationWrongInPY = "SaveFailTotalCalculationWrongInPY";
            public const string DocDateValidIsLessThanDocDate = "DocDateValidIsLessThanDocDate";
            public const string CannotMatchCurrencyPosition = "CannotMatchCurrencyPosition";
            public const string CannotGetStandardPrice = "CannotGetStandardPrice";
            public const string UnableToGetPriceRatio = "UnableToGetPriceRatio";
            public const string InvalidItmTaxGrpWhenDocTaxGrpIsZero = "InvalidItmTaxGrpWhenDocTaxGrpIsZero";
            public static string MustEqualARnAP = "MustEqualARnAP";


            public const string NotAllowedInput = "NotAllowedInput";
            public const string NotAllowedAccForINType = "NotAllowedAccForINType";
            public const string NotAllowedUOMEmpty = "NotAllowedUOMEmpty";
            public const string NotAllowedDeptForINType = "NotAllowedDeptForINType";
            public const string NotAllowedTranGrpForINType = "NotAllowedTranGrpForINType";
            public const string NotAllowedLocForINType = "NotAllowedLocForINType";
            public const string NotAllowedItmAmtShwForINType = "NotAllowedItmAmtShwForINType";
            public const string DocumentHasBeenPosted = "DocumentHasBeenPosted";
            public const string AdjustmentItemEmpty = "AdjustmentItemEmpty";
            public const string CannotGetApplyListByMonth = "CannotGetApplyListByMonth";

            public static string DisAllowChangesOnDeliveredDoc = "DisAllowChangesOnDeliveredDoc";
            public static string DisAllowChangesOnDocLinkToSettlement = "DisAllowChangesOnDocLinkToSettlement";
            public static string DisAllowChangesOnDocLinkToReceiveConsignment = "DisAllowChangesOnDocLinkToReceiveConsignment";
            public static string DisAllowChangesOnDocLinkToInvoice = "DisAllowChangesOnDocLinkToInvoice";
            public static string WarnDocLinkToSO = "WarnDocLinkToSO";
            public static string WarnDocLinkToDO = "WarnDocLinkToDO";
            public static string WarnDocLinkToInvoice = "WarnDocLinkToInvoice";
            public static string WarnDocLinkToOrderConsignment = "WarnDocLinkToOrderConsignment";
            public static string WarnDocLinkToSettlement = "WarnDocLinkToSettlement";

            public static string CannotDeleteDocApplied = "CannotDeleteDocApplied";
            public static string CannotEditDocApplied = "CannotEditDocApplied";
            public static string InvoicedDocCannotBeSave = "InvoicedDocCannotBeSave";
            public static string CannotDeleteDocHasLinked = "CannotDeleteDocHasLinked";
            public static string CannotSaveDocHasLinked = "CannotSaveDocHasLinked";
            public static string WarnDocIsInvoiced = "WarnDocIsInvoiced";
            public static string WarnDocIsLink = "WarnDocIsLink";



            public static string PackingNoDuplicate = "PackingNoDuplicate";
        }

        public static class SystemOption
        {
            public struct Posting
            {
                public const string AllowOutOfStock = "AllowOutOfStock";
                public const string AllowOutOfStockLocation = "AllowOutOfStockLocation";
            }
        }

        

        public static class Report
        {
            public const string NoDataExistForThisReport = "NoDataExistForThisReport";
        }
        public static class MSTCon
        {
            public const string ConTypeIsEmpty = "ConTypeIsEmpty";
            public const string CCBTypeIsEmpty = "CCBTypeIsEmpty";
            public const string AddrIDInUse = "AddrIDInUse";
        }
        public static class MSTJob
        {
            public const string DateLessThanStartDate = "DateLessThanStartDate";

            public const string TaskEmptyWhilePhaseNot = "TaskEmptyWhilePhaseNot";
            public const string CostTypeEmpty = "CostTypeEmpty";
            public const string PhaseEmptyWhileTaskNot = "PhaseEmptyWhileTaskNot";
            public const string PhaseTaskCostTypeEmpty = "PhaseTaskCostTypeEmpty";

        }
        public static class CommonSuccess
        {
            public const string UpdateSuccess = "UpdateSuccess";
            public const string SaveSuccess = "SaveSuccess";
            public const string DeleteSuccess = "DeleteSuccess";
            public const string CopySuccess = "CopySuccess";
        }

        public static class Locking
        {
            public const string IsLockTrue = "IsLockTrue";
            public const string IsProcessLockCheckFail = "IsProcessLockCheckFail";
            public const string IsProcessLockFail = "IsProcessLockFail";
            public const string LockAddFail = "LockAddFail";
            public const string LockDeleteFail = "LockDeleteFail";
            public const string IsUserFormInUse = "IsUserFormInUse";
            public const string IsGroupFormInUse = "IsGroupFormInUse";
            public const string IsChangePasswordFormInUse = "IsChangePasswordFormInUse";
            //            public const string IsLockBySameGUID = "IsLockBySameGUID";
        }
        public static class MSTItmDetail
        {
            public const string DuplicateAssItmKey = "DuplicateAssItmKey";
            public const string DuplicateBomItmKey = "DuplicateBOMItmKey";
            public const string DuplicateLocItmKey = "DuplicateLocItmKey";
            public const string DuplicateAltItmKey = "DuplicateAltItmKey";
        }

        public static class MSTEqpt
        {
            public const string EqptSubNameDuplicateRecord = "EqptSubNameDuplicateRecord";
        }
        public static class Validation
        {
            public const string IsRequire = "IsRequire";
            public const string ExceedMaxChar = "ExceedMaxChar";
            public const string NotDate = "NotDate";
            public const string DateOverflow = "DateOverflow";
            public const string DateExceedLimit = "DateExceedLimit";
            public const string DateOutOfRange = "DateOutOfRange";
            public const string NotInteger = "NotInteger";
            public const string IntegerOverflow = "IntegerOverflow";
            public const string IntegerExceedLimit = "IntegerExceedLimit";
            public const string IntegerOutOfRange = "IntegerOutOfRange";
            public const string NotDecimal = "NotDecimal";
            public const string NotBoolean = "NotBoolean";
            public const string DecimalOverflow = "DecimalOverflow";
            public const string DecimalExceedLimit = "DecimalExceedLimit";
            public const string DecimalOutOfRange = "DecimalOutOfRange";
            public const string ProgramError = "ProgramError";
            public const string DuplicateRecord = "DuplicateRecord";
            public const string DuplicateRecordDetail = "DuplicateRecordDetail";
            public const string DataKeyInvalid = "DataKeyInvalid";
            public const string CommitTransFail = "CommitTransFail";
            public const string IncorrectRFQInformation = "IncorrectRFQInformation";
            public const string InvalidFormat = "InvalidFormat";

            public const string DuplicateDetailIDCurrDate = "DuplicateDetailIDCurrDate";
            public const string DuplicateDetailIDEffDate = "DuplicateDetailIDEffDate";
            public const string DuplicateDetailIDModel = "DuplicateDetailIDModel";
            public const string DuplicateDetailIDTaxKey = "DuplicateDetailIDTaxKey";
            public const string DuplicateDetailIDUOMConKey = "DuplicateDetailIDUOMConKey";
            public const string DuplicateRecordID = "DuplicateRecordID";
            public const string DuplicateRecordIDParams = "DuplicateRecordIDParams";
            public const string DuplicateRecordIDAcc = "DuplicateRecordIDAcc";
            public const string DuplicateRecordIDAccGrp = "DuplicateRecordIDAccGrp";
            public const string DuplicateRecordIDAddr = "DuplicateRecordIDAddr";
            public const string DuplicateRecordIDBank = "DuplicateRecordIDBank";
            public const string DuplicateRecordIDBatch = "DuplicateRecordIDBatch";
            public const string DuplicateRecordIDBranch = "DuplicateRecordIDBranch";
            public const string DuplicateRecordIDBrand = "DuplicateRecordIDBrand";
            public const string DuplicateRecordIDCat = "DuplicateRecordIDCat";
            public const string DuplicateRecordIDColor = "DuplicateRecordIDColor";
            public const string DuplicateRecordIDCon = "DuplicateRecordIDCon";
            public const string DuplicateRecordIDCurr = "DuplicateRecordIDCurr";
            public const string DuplicateRecordIDDept = "DuplicateRecordIDDept";
            public const string DuplicateRecordIDDocGrp = "DuplicateRecordIDDocGrp";
            public const string DuplicateRecordIDEqpt = "DuplicateRecordIDEqpt";
            public const string DuplicateRecordIDEqptType = "DuplicateRecordIDEqptType";
            public const string DuplicateRecordIDFinMain = "DuplicateRecordIDFinMain";
            public const string DuplicateRecordIDIDFormat = "DuplicateRecordIDIDFormat";
            public const string DuplicateRecordIDIndustry = "DuplicateRecordIDIndustry";
            public const string DuplicateRecordIDInterest = "DuplicateRecordIDInterest";
            public const string DuplicateRecordIDItm = "DuplicateRecordIDItm";
            public const string DuplicateRecordIDJob = "DuplicateRecordIDJob";
            public const string DuplicateRecordIDJobCostType = "DuplicateRecordIDJobCostType";
            public const string DuplicateRecordIDJobGrp = "DuplicateRecordIDJobGrp";
            public const string DuplicateRecordIDJobPhase = "DuplicateRecordIDJobPhase";
            public const string DuplicateRecordIDJobTask = "DuplicateRecordIDJobTask";
            public const string DuplicateRecordIDLoc = "DuplicateRecordIDLoc";
            public const string DuplicateRecordIDOverHead = "DuplicateRecordIDOverHead";
            public const string DuplicateRecordIDPackingType = "DuplicateRecordIDPackingType";
            public const string DuplicateRecordIDPayMode = "DuplicateRecordIDPayMode";
            public const string DuplicateRecordIDPrice = "DuplicateRecordIDPrice";
            public const string DuplicateRecordIDScale = "DuplicateRecordIDScale";
            public const string DuplicateRecordIDSecGrp = "DuplicateRecordIDSecGrp";
            public const string DuplicateRecordIDSecUser = "DuplicateRecordIDSecUser";
            public const string DuplicateRecordIDSerial = "DuplicateRecordIDSerial";
            public const string DuplicateRecordIDShipVia = "DuplicateRecordIDShipVia";
            public const string DuplicateRecordIDSysApp = "DuplicateRecordIDSysApp";
            public const string DuplicateRecordIDTASAlert = "DuplicateRecordIDTASAlert";
            public const string DuplicateRecordIDTASToDo = "DuplicateRecordIDTASToDo";
            public const string DuplicateRecordIDTaxA = "DuplicateRecordIDTaxA";
            public const string DuplicateRecordIDTaxGrp = "DuplicateRecordIDTaxGrp";
            public const string DuplicateRecordIDTerm = "DuplicateRecordIDTerm";
            public const string DuplicateRecordIDTerritory = "DuplicateRecordIDTerritory";
            public const string DuplicateRecordIDUOM = "DuplicateRecordIDUOM";
            public const string GSTCustomTrueButGSTFalse = "GSTCustomTrueButGSTFalse";
        }

        public static class ChangePassword
        {
            public const string PasswordIsNotEqual = "PasswordIsNotEqual";
            public const string PasswordAreNotMatch = "PasswordAreNotMatch";
            public const string PasswordMinMax = "PasswordMinMax";
            //added by KKAung on 8 Aug 2022 (start) 
            public const string PasswordIsEqualLast3 = "Password must not be one of the last three passwords, excluding OTP.";  
            public const string PasswordNotContainUpperLowerCharacters = "Password must contain both upper and lower case letters (a-zA-Z).";
            public const string PasswordNotContainDigit = "Password must include at least one digit(0-9).";
            public const string PasswordNotContainSpecialCharacters = "Password must include at least one special character (!@#$%^&*()_+|~-=\\`{}[]:\";'<>?,./).";
            // (end)
        }

        public static class Reference
        {
            public const string REFAccGrp = "DuplicateRecordIDAccGrp";
            public const string REFBank = "DuplicateRecordIDBank";
            public const string REFColor = "DuplicateRecordIDColor";
            public const string REFDocGrp = "DuplicateRecordIDDocGrp";
            public const string REFEqptType = "DuplicateRecordIDEqptType";
            public const string REFIndustry = "DuplicateRecordIDIndustry";
            public const string REFJobCostType = "DuplicateRecordIDJobCostType";
            public const string REFJobGrp = "DuplicateRecordIDJobGrp";
            public const string REFJobPhase = "DuplicateRecordIDJobPhase";
            public const string REFJobTask = "DuplicateRecordIDJobTask";
            public const string REFLoc = "DuplicateRecordIDLoc";
            public const string REFPackingType = "DuplicateRecordIDPackingType";
            public const string REFPayMode = "DuplicateRecordIDPayMode";
            public const string REFShipVia = "DuplicateRecordIDShipVia";
            public const string REFTerritory = "DuplicateRecordIDTerritory";
            public const string REFIDFormat = "DuplicateRecordIDIDFormat";
            public const string REFInterest = "DuplicateRecordIDInterest";
            public const string REFOverHead = "DuplicateRecordIDOverHead";
            public const string REFTerm = "DuplicateRecordIDTerm";
            public const string REFBrand = "DuplicateRecordIDBrand";
            public const string REFCurr = "DuplicateRecordIDCurr";
            public const string REFTaxA = "DuplicateRecordIDTaxA";
            public const string REFTaxGrp = "DuplicateRecordIDTaxGrp";
            public const string REFUOM = "DuplicateRecordIDUOM";
            public const string MSTAccBranch = "DuplicateRecordIDBranch";
            public const string MSTAccDept = "DuplicateRecordIDDept";
            public const string MSTItem = "DuplicateRecordIDItem";
            public const string MSTBudget = "BudgetPeriodInvalid";
            public const string REFBrandDetItm = "DuplicateDetailIDModel";
            public const string REFCurrDetItm = "DuplicateDetailIDCurrDate";
            public const string REFScaleDetItm = "DuplicateDetailIDScaleID";
            public const string REFTaxADetItm = "DuplicateDetailIDEffDate";
            public const string REFTaxGrpDetItm = "DuplicateDetailIDTaxKey";
            public const string REFUOMDetItm = "DuplicateDetailIDUOMConKey";
            public const string REFBankClear = "REFBankClear";
            public const string REFTaxGrpDetItmWarnChange = "REFTaxGrpDetItmWarnChange";
        }


        public static class SystemCode
        {
            public const string NoTemplate = "NoTemplate";

            public const string InvalidTextSegmentValue = "InvalidSegmentValue";
            public const string InvalidDateFormat = "InvalidDateFormat";
            public const string LockPeriodLessThanLastYearEnd = "LockPeriodLessThanLastYearEnd";
            public const string InvalidDocTypeName = "InvalidDocTypeName";
            public const string InvalidDocType = "InvalidDocType";
            public const string InvalidCounterGrp = "InvalidCounterGrp";
            public const string WordNumMustGreaterThan0 = "WordNumMustGreaterThan0";
            public const string CharNumMustGreaterThan0 = "CharNumMustGreaterThan0";
            public const string CounterGrpUsedInDocType = "CounterGrpUsedInDocType";
            public const string CounterGrpZeroValue = "CounterGrpZeroValue";
        }

        public static class System
        {
            public const string AuditLogAddFail = "AuditLogAddFail";
        }

        public static class Period
        {
            public const string GenerateNewYearFail = "GenerateNewYearFail";
            public const string GeneratePeriodLessThan12 = "GeneratePeriodLessThan12";
            public const string CloseStausCannotSelect = "CloseStausCannotSelect";
        }


        public static class Record
        {
            public const string ExceedMaxAutoIDTries = "ExceedMaxAutoIDTries";
            public const string NotEffected = "NotEffected";
            public const string AlreadyOpen = "AlreadyOpen";
            public const string ConfirmDeleteRecord = "ConfirmDeleteRecord";
        }

        public static class Process
        {
            public const string GetProcessStateFail = "GetProcessStateFail";
            public const string GeneratePeriodLessThan12 = "GeneratePeriodLessThan12";
        }
        public static class Option
        {
            public const string GetLockingGUIDFail = "GetLockingGUIDFail";
            public const string GetOptionFail = "GetOptionFail";
            public const string InvalidCallOption = "InvalidCallOption";
        }

        public static class SECUser
        {
            public const string CannotDeleteBuiltUpUser = "CannotDeleteBuiltUpUser";
            public const string CannotDeleteCurrentUser = "CannotDeleteCurrentUser";
            public const string CannotDeleteCurrentLogInUser = "CannotDeleteCurrentLogInUser";
            public const string ClearLocksNotClearLogin = "ClearLocksNotClearLogin";
        }

        public static class SECGrp
        {
            public const string CannotDeleteBuiltUpGrp = "CannotDeleteBuiltUpGrp";
        }

        public static class Permission
        {
            public const string LoadAllUserPermissionFail = "LoadAllUserPermissionFail";
            public const string PermAddIsFalse = "PermAddIsFalse";
            public const string PermAnyIsFalse = "PermAnyIsFalse";
            public const string PermDeletesFalse = "PermDeletesFalse";
            public const string PermEditIsFalse = "PermEditIsFalse";
            public const string PermListIsFalse = "PermListIsFalse";
            public const string PermPerformIsFalse = "PermPerformIsFalse";
            public const string PermReadIsFalse = "PermReadIsFalse";
            public const string PermOpenRecIsFalse = "PermOpenRecIsFalse";
        }

        public static class Login
        {
            public const string ConfigurationFileIsMissing = "ConfigurationFileIsMissing";
            public const string ConfigurationFileSaveFail = "ConfigurationFileSaveFail";
            public const string DatabaseConnectionInValid = "DatabaseConnectionInValid";
            public const string DatabaseConnectionStrInValid = "DatabaseConnectionStrInValid";
            public const string DataBaseIDIsEmpty = "DataBaseIDIsEmpty";
            public const string DatabaseIDNotInList = "DatabaseIDNotInList";
            public const string DatabaseRegCodeInvalid = "DatabaseRegCodeInvalid";
            public const string DatabaseVersionInValid = "DatabaseVersionInValid";
            public const string IncrementUserLoginRetryFail = "IncrementUserLoginRetryFail";
            public const string LoginFail = "LoginFail";
            public const string LogoffFail = "LogoffFail";
            public const string ResetUserLoginRetryFail = "ResetUserLoginRetryFail";
            public const string SaveUserLoginFail = "SaveUserLoginFail";
            public const string SetUserLockoutFail = "SetUserLockoutFail";
            public const string UserFirstLogin = "UserFirstLogin";
            public const string UserIDOrPasswordInValid = "UserIDOrPasswordInValid";
            public const string UserIsAlredayLogin = "UserIsAlredayLogin";
            public const string UserIsDisabled = "UserIsDisabled";
            public const string UserIsLockout = "UserIsLockout";
            public const string UserLastLoginInfo = "UserLastLoginInfo";
        }

        public static class MSTPriceList
        {
            public const string CustomPriceDecimalExceedLimit = "CustomPriceDecimalExceedLimit";
            public const string CustomPriceDecimalOutOfRange = "CustomPriceDecimalOutOfRange";
            public const string CustomPriceDecimalOverflow = "CustomPriceDecimalOverflow";
            public const string CustomPriceIsRequire = "CustomPriceIsRequire";
            public const string CustomPriceNotDecimal = "CustomPriceNotDecimal";
            public const string ItmDesExceedMaxChar = "ItmDesExceedMaxChar";
            public const string ItmDesIsRequire = "ItmDesIsRequire";
            public const string ItmKeyIntegerExceedLimit = "ItmKeyIntegerExceedLimit";
            public const string ItmKeyIntegerOutOfRange = "ItmKeyIntegerOutOfRange";
            public const string ItmKeyIntegerOverflow = "ItmKeyIntegerOverflow";
            public const string ItmKeyIsRequire = "ItmKeyIsRequire";
            public const string ItmKeyNotInteger = "ItmKeyNotInteger";
            public const string ItmPriceDecimalExceedLimit = "ItmPriceDecimalExceedLimit";
            public const string ItmPriceDecimalOutOfRange = "ItmPriceDecimalOutOfRange";
            public const string ItmPriceDecimalOverflow = "ItmPriceDecimalOverflow";
            public const string ItmPriceIsRequire = "ItmPriceIsRequire";
            public const string ItmPriceNotDecimal = "ItmPriceNotDecimal";
            public const string ItmQtyDecimalExceedLimit = "ItmQtyDecimalExceedLimit";
            public const string ItmQtyDecimalOutOfRange = "ItmQtyDecimalOutOfRange";
            public const string ItmQtyDecimalOverflow = "ItmQtyDecimalOverflow";
            public const string ItmQtyIsRequire = "ItmQtyIsRequire";
            public const string ItmQtyNotDecimal = "ItmQtyNotDecimal";
            public const string ItmTypeIntegerExceedLimit = "ItmTypeIntegerExceedLimit";
            public const string ItmTypeIntegerOutOfRange = "ItmTypeIntegerOutOfRange";
            public const string ItmTypeIntegerOverflow = "ItmTypeIntegerOverflow";
            public const string ItmTypeIsRequire = "ItmTypeIsRequire";
            public const string ItmTypeNotInteger = "ItmTypeNotInteger";
            public const string PriceDesExceedMaxChar = "PriceDesExceedMaxChar";
            public const string PriceDesIsRequire = "PriceDesIsRequire";
            public const string PriceIDDuplicateRecord = "PriceIDDuplicateRecord";
            public const string PriceIDExceedMaxChar = "PriceIDExceedMaxChar";
            public const string PriceIDIsRequire = "PriceIDIsRequire";
            public const string PriceKeyDataKeyInvalid = "PriceKeyDataKeyInvalid";
            public const string PriceKeyIntegerExceedLimit = "PriceKeyIntegerExceedLimit";
            public const string PriceKeyIntegerOutOfRange = "PriceKeyIntegerOutOfRange";
            public const string PriceKeyIntegerOverflow = "PriceKeyIntegerOverflow";
            public const string PriceKeyIsRequire = "PriceKeyIsRequire";
            public const string PriceKeyNotInteger = "PriceKeyNotInteger";
            public const string EffStartDateIsRequire = "EffStartDateIsRequire";
            public const string EffStartDateNotDate = "EffStartDateNotDate";
            public const string EffStartDateDateOverflow = "EffStartDateDateOverflow";
            public const string EffStartDateDateExceedLimit = "EffStartDateDateExceedLimit";
            public const string EffStartDateDateOutOfRange = "EffStartDateDateOutOfRange";
            public const string EffEndDateIsRequire = "EffEndDateIsRequire";
            public const string EffEndDateNotDate = "EffEndDateNotDate";
            public const string EffEndDateDateOverflow = "EffEndDateDateOverflow";
            public const string EffEndDateDateExceedLimit = "EffEndDateDateExceedLimit";
            public const string EffEndDateDateOutOfRange = "EffEndDateDateOutOfRange";
            public const string EffItmQtyIsRequire = "EffItmQtyIsRequire";
            public const string EffItmQtyNotDecimal = "EffItmQtyNotDecimal";
            public const string EffItmQtyDecimalOverflow = "EffItmQtyDecimalOverflow";
            public const string EffItmQtyDecimalExceedLimit = "EffItmQtyDecimalExceedLimit";
            public const string EffItmQtyDecimalOutOfRange = "EffItmQtyDecimalOutOfRange";
            public const string EffItmPriceIsRequire = "EffItmPriceIsRequire";
            public const string EffItmPriceNotDecimal = "EffItmPriceNotDecimal";
            public const string EffItmPriceDecimalOverflow = "EffItmPriceDecimalOverflow";
            public const string EffItmPriceDecimalExceedLimit = "EffItmPriceDecimalExceedLimit";
            public const string EffItmPriceDecimalOutOfRange = "EffItmPriceDecimalOutOfRange";

            // Price Percentage Messages
            public const string PercentageNotDecimal = "PercentageNotDecimal";
            public const string PercentageDecimalOverflow = "PercentageDecimalOverflow";
            public const string PercentageDecimalExceedLimit = "PercentageDecimalExceedLimit";
            public const string PercentageDecimalOutOfRange = "PercentageDecimalOutOfRange";
            public const string RatioNotDecimal = "RatioNotDecimal";
            public const string RatioDecimalOverflow = "RatioDecimalOverflow";
            public const string RatioDecimalExceedLimit = "RatioDecimalExceedLimit";
            public const string RatioDecimalOutOfRange = "RatioDecimalOutOfRange";
            public const string EffPercentagNotDecimal = "EffPercentagNotDecimal";
            public const string EffPercentageDecimalOverflow = "EffPercentageDecimalOverflow";
            public const string EffPercentageDecimalExceedLimit = "EffPercentageDecimalExceedLimit";
            public const string EffPercentageDecimalOutOfRange = "EffPercentageDecimalOutOfRange";
            public const string EffRatioNotDecimal = "EffRatioNotDecimal";
            public const string EffRatioDecimalOverflow = "EffRatioDecimalOverflow";
            public const string EffRatioDecimalExceedLimit = "EffRatioDecimalExceedLimit";
            public const string EffRatioDecimalOutOfRange = "EffRatioDecimalOutOfRange";
            public const string CategoryIsRequire = "CategoryIsRequire";

            public const string PriceValueDetailHasError = "PriceValueDetailHasError";
            public const string PriceRatioDetailHasError = "PriceRatioDetailHasError";

        }

        public static class ReportSetting
        {
            public const string AlreadyUsedAsDefaultReport = "AlreadyUsedAsDefaultReport";
            public const string AlreadyUsedAsSecondaryReport = "AlreadyUsedAsSecondaryReport";
        }


    }

    public static class PostType
    {
        public const string OPN = "OPN";            //Account Opening Balance
        public const string ITMREV = "ITMRev";      //Document Detail - reverse for APBL with link from APPD
        public const string ITM = "ITM";            //Document Detail
        public const string ItmAR = "ItmAR";        //Payment Detail Applied Amt (AR)
        public const string ItmAP = "ItmAP";        //Payment Detail Applied Amt (AP)
        public const string ItmDisB = "ItmDisB";    //Payment Detail Discount Balance Sheet type
        public const string ItmDisP = "ItmDisP";    //Payment Detail Discount ProfitLoss type
        public const string ItmGLP = "ItmGLP";      //Payment Detail Applied GainLoss ProfitLoss type
        public const string DetRev = "DetRev";      //AR Payment Detail Revenue
        public const string DetExp = "DetExp";      //AP Payment Detail Expense
        public const string DIS = "DIS";            //Overall Discount
        public const string TX = "TX";              //Tax
        public const string TXC = "TXC";            //Custom Import Tax Posting
        public const string LDC = "LDC";            //Landed Cost
        public const string BK = "BK";              //Bank
        public const string ADJ = "ADJ";            //Inventory Adjustment 
        public const string AR = "AR";              //Document Header AR
        public const string AP = "AP";              //Document Header AP
        public const string GLB = "GLB";            //Gain/Loss for AR/AP (Balance Sheet Item) Credit Note
        public const string GLP = "GLP";            //Gain/Loss for Expense/Income (Profit/Loss Item) Credit Note
        public const string RND = "RND";            //Rounding
    }

    public static class PostTypeCode
    {
        public const int OPN = 100;             //Account Opening Balance
        public const int ITMREV = 200;          //Document Detail - reverse for APBL with link from APPD
        public const int ITM = 210;             //Document Detail
        public const int ItmAR = 300;           //Payment Detail Applied Amt (AR)
        public const int ItmAP = 310;           //Payment Detail Applied Amt (AP)
        public const int ItmDisB = 320;         //Payment Detail Discount Balance Sheet type
        public const int ItmDisP = 330;         //Payment Detail Discount ProfitLoss type
        public const int ItmGLP = 340;          //Payment Detail Applied GainLoss ProfitLoss type
        public const int DetRev = 400;          //AR Payment Detail Revenue
        public const int DetExp = 410;          //AP Payment Detail Expense
        public const int DIS = 500;             //Overall Discount
        public const int TX = 510;              //Tax
        public const int TXC = 520;             //Custom Import Tax Posting
        public const int LDC = 600;             //Landed Cost
        public const int BK = 610;              //Bank
        public const int ADJ = 620;             //Inventory Adjustment 
        public const int AR = 700;              //Document Header AR
        public const int AP = 710;              //Document Header AP
        public const int GLB = 720;             //Gain/Loss for AR/AP (Balance Sheet Item) Credit Note
        public const int GLP = 730;             //Gain/Loss for Expense/Income (Profit/Loss Item) Credit Note
        public const int RND = 900;             //Rounding
    }
    

    //Report Export File Type
    public struct ReportFileType
    {
        public const string AcrobatPDFFile = "Acrobat Format (PDF)";
        public const string ExcelFile = "MS Excel 97-2000";
        public const string ExcelFileDataOnly = "MS Excel 97-2000 (Data only)";
        public const string RichTextFile = "Rich Text Format";
        public const string HTMLFile = "HTML 4.0";
        public const string CSVFile = "Separated Values (CSV)";

    }

    public struct ReportFileDestinationType
    {
        public static string Application = "Application";
        public static string DiskFile = "Disk file";
    }

    public class UINotifierEventArgs : EventArgs
    {
        private Hashtable _propertyMessage = null;

        public Hashtable PropertyMessage
        {
            get
            {
                return this._propertyMessage;
            }
            set
            {
                this._propertyMessage = value;
            }
        }

        public UINotifierEventArgs()
        {
            this._propertyMessage = new Hashtable();
        }

        public UINotifierEventArgs(Hashtable propertyMessage)
        {
            this._propertyMessage = propertyMessage;
        }
    }

    public static class SpecialRemark
    {
        /* added by YST */
        public const string Cancel = "QACancel";
        public const string Transfer = "QATransfer";
        public const string FOC = "QAFOC";
        public const string Sample = "QASample";
        public const string GoodsReplacement = "QAGoodsReplacement";
        public const string GoodsModification = "QAGoodsModification";
        public const string WarrantyClaim = "QAWarrantyClaim";
        public const string ShortageSupply = "QAShortageSupply";
        public const string WrongSupply = "QAWrongSupply";
        public const string GSTReverse = "GST Reverse DO";
        public const string GSTReverseRemark = "GST Reverse Remark";
        public const string CustomerPO = "Customer PO";

    }

    public static class ApprovalStatus
    {
        /* added by YST */
        public const string Requested = "Requested";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Recommended = "Recommended";
        public const string Notified = "Notified";
        public const string Pending = "Pending";
        public const string Sent = "Sent";
        public const string Draft = "Draft";

    }

    public static class DBCode
    {
        /* added by YST on 2023/05/08 */
        public const string ADL = "ADL"; //BossAthena
        public const string ARCO = "ARCO"; //BossGLINew
        public const string BHE = "BHE"; //BossBHE
        public const string BHG = "BHG"; //BossBHG
        public const string BHH = "BHH"; //BossBHH
        public const string BHM = "BHM"; //BossBHM
        //public const string BLH = "BLH"; //BossBLH
        public const string GLH = "GLH"; //BossGLH
        public const string BlueSky = "BlueSky"; //BossBlueSky
        public const string NGSS = "NGSS"; //BossNGSS
        public const string BOS = "BOS"; //BossYMO
        public const string GLI = "GLI"; //BossGLI
        public const string GSI = "GSI"; //BossGSI
        public const string LLH = "LLH"; //BossLLH
        public const string OMS = "OMS"; //BossOMS
        public const string OMSTW = "OMSTW"; //BossOMSTW
        public const string ONEBHG = "ONEBHG"; //BossONEBHG
        public const string SFE = "SFE"; //BossSFE
        public const string SFP = "SFP"; //BossPWS
        public const string SFT = "SFT"; //BossBOSI
        public const string SKY = "SKY"; //BossSKY
        public const string SOP = "SOP"; //BossSSM
        public const string SSA = "SSA"; //BossSaSa
        public const string TH = "TH"; //BossTongHong
        public const string ITS = "ITS"; //BossIVT

    }

}
