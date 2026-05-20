using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOLib
{
    [Serializable()]
    public static class GEnum
    {
        public enum RecordAccessType
        {
            CustomerAndVendor = 1,
            Item = 2,
            Job = 3
        }

        public enum PrintType
        {
            Print=10,
            Preview=20,
            Email=30,
        }
        public enum AlertStatus
        {
            Pending = 10,
            Alert = 20,
            Read = 30,
            Hidden = 40,
            Error = 99,
        } 
        public enum CashFlowParticularType
        {
            Cash=10,
            Cash_Blank_Line=15,
            Add_Debtor=20,
            Add_Others=30,
            Total_Add=40,
            Total_Add_Blank_Line=45,
            Total_Available=50,
            Total_Available_Blank_Line=55,
            Less_Creditor=60,
            Less_Others=65,
            Total_Less=70,
            Total_Less_Blank_Line=75,
            Cash_Forecast=80,
        }
        public enum TrialBalReportSelection
        {
            TrialBalance=10,
            OpeningBalance = 20,
        }
        public enum InstanceMode
        {
            Normal = 0,
            InternalCall = 1,
            UserCallOnly = 2
        }
        public enum ApprovalOpiton
        {
            None = 10,
            Approval_Only = 20,
            Approval_By_Limit_Set=30
        }
        public enum AuditLogType
        {
            AuditTrail = 10,
            SystemError = 20
        }

        public enum AlertType
        {
            Normal = 10,
            Popup_Messgae = 20,
            Email = 30,
            SMS = 40,
        }
        public enum AlertApplyTo
        {
            BalanceCurrentPeriod=10100,
            BalanceYearToDate=10110,
            BudgetYearToDate=10130,
            BudgetCurrentPeriod=10140,
            AccountRecordDeletion=10199,
            OverCreditLimit=20100,
            SalesVolumeCurrentPeriod=20110,
            SalesVolumeYearToDate=20120,
            TargetSalesVolumeCurrentPeriod=20130,
            TargetSalesVolumeYearToDate=20140,
            CustomerCurrentBalance=20150,
            OutstandingInvoiceBalance=20160,
            OutstandingInvoiceAgeCondition=20170,
            Quotationvaliditydeadline=20180,
            CustomerRecordDeletion=20199,
            PurchaseVolumeCurrentPeriod=30100,
            PurchaseVolumeYearToDate=30110,
            BudgetPurchaseVolumeCurrentPeriod=30120,
            BudgetPurchaseVolumeYearToDate=30130,
            VendorCurrentBalance=30140,
            VendorOutstandingInvoiceBalance=30150,
            VendorOutstandingInvoiceAgeCondition = 30160,
            VendorRecordDeletion=30199,
            Qtyonhandbelowminimumstock=40100,
            ItemSalesVolumeCurrentPeriod=40110,
            ItemSalesVolumeYearToDate=40120,
            TargetItemSalesVolumeCurrentPeriod=40130,
            TargetItemSalesVolumeYearToDate=40140,
            QtysoldCurrentPeriod=40150,
            QtysoldYearToDate=40160,
            TargetQtysoldCurrentPeriod=40170,
            TargetQtysoldYearToDate=40180,
            OutstandingSOPOCOETAdue=40190,
            InventoryRecordDeletion=40199,
            Qtybelowavailablestock=40200,
            Qtyoutofstock=40210,
            Jobestimateexceeded=50100,
            JobRecordDeletion=50199,
            Approvedocumentthatexceedcreditlimit=60110,
            Approvedocumentthatispricebelowcontrolprice=60120,
            SalesQORecordDeletion=60199,
            SalesSORecordDeletion=60299,
            SalesDORecordDeletion=60399,
            SalesPackingListRecordDeletion=60499,
            SalesInvoiceRecordDeletion=60599,
            SalesDebitCreditNoteRecordDeletion=60699,
            SalesAdjustmentRecordDeletion=60799,
            SalesPaymentRecordDeletion=60899,
            ContraRecordDeletion=60999,
            Whendocumentrequireapproval=70100,
            PurchasePlanRecordDeletion=70199,
            PurchaseRequestRecordDeletion=70299,
            PurchaseOrderRecordDeletion=70399,
            PurchaseDeliveryRecordDeletion=70499,
            PurchaseInvoiceRecordDeletion=70599,
            PurchaseCreditDebitNoteRecordDeletion=70699,
            PurchaseAdjustmentRecordDeletion=70799,
            PaymentIssueRecordDeletion=70899,
            Wrongpasswordlockouts=80120,
            HouroflogonOddhours=80130,
            Documentsawaitingapproval=90100,
            Periodsrunningout=90110,
            Systemmessages=90120,
            Scheduledprocessesupdatesstatus=90130,
            Fromevents=90140
    }
        public enum ContactType
        {
            Phone = 10,
            Fax = 20,
            Mobile = 30,
            Email = 40,
            Website = 50,
            Others = 60
        }
        public enum ConType
        {
            Customer = 10,
            Vendor = 20,
            Both = 30,
            Prospect = 40 
           
        }
        public enum TransmitMode
        {
            Email = 10,
            Fax = 20,
            Print = 30
        }
        public enum TransmitStatus
        {
            Sucessful = 10,
            Failed = 20,
            Pending = 30
        }
        public enum OrderStatus
        {
            NA = 0,
            Pending = 10,
            Delivered = 20,
            Cancelled = 30

        }
        public enum ReportGroup
        {
            System = 100,
            Account = 200,
            Customer = 300,
            Vendor = 400,
            Inventory = 500,
            Sales = 1000,
            Purchase = 1100,
            Consignment = 1200,
            Job = 1300,
            Financial = 1400,
            Security = 1500,
            POS = 7000,
            Others = 8000,
            Misc = 8100,
            NA = 8200,
            Document_PrintOut = 9000
        }
        public enum SpState
        {
            Fail = 0,
            Pass = 1
        }

        public enum BOMLineType
        {
            Raw_Material = 10,
            Packing_Material = 20,
            Labour = 30
        }

        public enum AllowOutOfStockType
        {
            No_Warning = 10,
            Warn_When_Not_Enough_Stock = 20,
            Can_not_Save_When_Not_Enough_Stock = 30

        }
        public enum AuditLogMode
        {
            Read = 10,
            Add = 20,
            Edit = 30,
            Delete = 40,
            Error = 50,
            

            Created_From_DO_Transfer = 100,
            Created_From_AP_Requisition = 110,
            Created_From_IN_Res = 120,
            Created_From_IN_Req = 130,
            Created_From_Financial_Charge = 140,
            Doc_Exceed_CreditLimit=150,
            Doc_Exceed_Term = 160,
            Doc_Price_BelowControlPrice = 170,
            EmailSent=200,
            NA = 999
        }

        public enum CosBatchLogMode
        {
            Add = 10,
            Edit = 20,
            Delete = 30
        }

        public enum BatchLogType
        {
            NormalTransaction = 10,
            AbNormalTransaction = 20

        }
        //Added By May on 9-Sep-2008
        public enum AddrLinkType
        {
            Company = 10,
            Branch = 20,
            Department = 30,
            CustomerOrVendor = 40,
            Employee = 50,
            Job = 60,
            ItemLocation = 70
        }

        //Added By May on 9-Sep-2008
        public enum ContactLinkType
        {
            Company = 10,
            Branch = 20,
            Department = 30,
            CustomerOrVendor = 40,
            Employee = 50,
            Job = 60
        }

        public enum SysLockOption
        {
            ByAll = 0,
            ByCodKey = 1,
            ByCodeKeyandDataKey = 2,
            ByCodeKeyDataKeyAndInprogressKey = 3,
            ByGUID = 4,
            ByCodeKeyAndGUID = 5,
            ByCodeKeyAndGUIDAndDataKey = 6,
            ByCodeKeyAndDataKeyAndInprogressKeyAndGUID = 7,
            ByUserKey = 8,
            ByCodeKeyAndUserKey = 9,
            ByCodeKeyAndDataKeyAndUserKey = 10,
            ByCodeKeyAndDataKeyAndInprogressKeyAndUserKey = 11,
            ByCodeKeyAndGUIDANDDataKeyNotEqual0 = 12,
            ByReadOnlyList = 13,
            //ByOptionKey=14
        }

        public enum SysOptFetchOption
        {
            ByAllSystemAndUsers = 0,
            ByOpID = 1,
            BySystem = 2,
            ByUser = 3
        }

        //Comment by Thida
        //public enum AccType
        //{
        //    ALL = 0,    //All records (default)
        //    ARAP = 1,   //AccTypeKey = AR or AP
        //    BC = 2,     //AccTypeKey = Bank or Cash
        //    BCF = 3,    //AccTypeKey = Bank or Cash in foreign currency
        //    BCT = 4,    //AccTypeKey = Bank or Cash or TempFund
        //    NotBC = 5,  //AccTypeKey <> Bank or Cash
        //    BL = 6,     //AccTypeKey = Balance Sheet
        //    PL = 7,     //AccTypeKey = Profit and Lost
        //}

        public enum SpecialCalculationType 
        { 
            Sale = 10,
            Purchase = 20
        }

        public enum SpecialCalculationProcessType
        {
            UOM = 10,
            PriceMarkup = 20
        }

        public enum AccTypeKey
        {
            All = 0,
            Equity = 100,
            Retain_Earning = 110,
            Fixed_Asset = 210,
            Accu_Depreciation = 220,
            Current_Asset = 230,
            Other_Asset = 240,
            Inventory = 250,
            Bank = 300,
            Petty_Cash = 310,
            Temp_Holding_Fund = 320,
            Account_Receivable = 400,
            Other_Receivable = 410,
            Account_Payable = 500,
            Other_Payable = 510,
            Current_Liability = 600,
            Long_Term_Liability = 610,
            Provision_for_Tax = 620,
            Income = 700,
            Other_Income = 710,
            Opening_Stock = 800,
            Purchases = 810,
            Cost_of_Sales = 820,
            Closing_Stock = 830,
            Expenses = 900,
            Other_Expenses = 910,
        }

        public enum ConTypeCust
        {
            ALL = 0,    //All records (default)
            C = 10,      //Customer
            B = 30,      //Both
            P = 40,      //Prospect
            CB = 4,     //Customer or Both
        }

        public enum ConTypeVend
        {
            ALL = 0,    //All records (default)
            V = 20,      //Vendor
            B = 30,      //Both
            //VB = 3,     //Vendor or Both
        }

        public enum CCBType
        {
            ALL = 0,    //All records (default)
            CR = 10,     //Credit
            CH = 20,     //Cash
            B = 30,      //Both Credit and Cash
        }

        //Commend By Thida
        //public enum ItmType
        //{
        //    ALL = 0,    //All records (default)
        //    Stk = 1,    //Stock, StockB, 
        //                //Finished GD, Finished GDB
        //                //Serial Stock, Serial StockB, Serial Finished GD
        //                //Substitute
        //                //Assembly
        //                //Consignment
        //    NS  = 2,    //Non Stock (Service)
        //    CSG = 3,    //Consignment
        //    FG  = 4,    //Finished GD, Finished GDB, Serial Finished GD
        //    MAS = 5,    //Master
        //    CHG = 6,    //Charges
        //    DIS = 7,    //Discount
        //}



        public enum INType
        {
            All = 0,
            STK = 1,//STK =   Stock = 100, StockB = 110 , Finished GD = 200, Finished GDB = 210, Assembly = 250, Serial Stock = 300, Serial StockB = 310, Serial Finished GD = 400, Substitute = 500, Consignment = 510
            NS = 2, //NS  =   Non Stock = 600, Service = 610
            CSG = 3,//CSG =   Consignment = 510
            FG = 4, //FG  =   Finished GD = 200, Finished GDB = 210, Serial Finished GD = 400
            MAS = 5,//MAS =   Master = 900
            CHG = 6,//CHG =   Charges = 700
            DIS = 7,//DIS =   Discount = 710
            AMT = 8,//AMT =   Stk,NS,CSG,FG,CHG  (100 ~ 700)
        }

        public enum INTypeGrp
        {
            //case (int)GEnum.INTypeGrp.Stock:
            //case (int)GEnum.INTypeGrp.Non_Stock:
            //case (int)GEnum.INTypeGrp.Charges:
            //case (int)GEnum.INTypeGrp.Discount:
            //case (int)GEnum.INTypeGrp.Remark:
            //case (int)GEnum.INTypeGrp.Total:
            //case (int)GEnum.INTypeGrp.Empty:

            Empty = 0,  //Null,Empty
            Stock = 1,
            Non_Stock = 2,
            Charges = 3,
            Remark = 4,
            Total = 5,
            Discount = 6,
        }
        public enum Require
        {
            Yes = 0,
            No = 1,
        }

        public enum DataType
        {
            String = 0,
            Boolean = 1,
            DateTime = 2,
            Integer = 3,
            Decimel = 4,
        }

        public enum DateType
        { 
            Use_Selected_Date = 10,
            Current_Day = 20,
            Current_Week = 30,
            Current_Month = 40,
            Current_Year = 50
        }

        public enum CostMethod
        {
            Average = 10,
            FIFO = 20,
            LIFO = 30
        }
        public enum ItemType
        {
            Stock = 100,
            StockB = 110,
            Finished_GD = 200,
            Finished_GDB = 210,
            Assembly = 250,
            Serial_StockB = 310,
            Serial_Finished_GDB = 410,
            //Substitute = 500, not use -- comment by pauk
            Consignment = 510,
            Non_Stock = 600,
            Service = 610,
            Charges = 700,
            Discount = 710,
            Header = 800,
            Remark = 810,
            Sub_Total = 820,
            Total = 825,
            BF_Total = 830,
            Master = 900
            //Serial_Stock = 300, --removed
            //Serial_Finished_GD = 400, --removed

            //-------------------------------------------------
            //case (int)GEnum.ItemType.Finished_GDB:
            //case (int)GEnum.ItemType.Serial_Finished_GDB:
            //case (int)GEnum.ItemType.Serial_StockB:
            //case (int)GEnum.ItemType.StockB:

            //case (int)GEnum.ItemType.Finished_GD:
            //case (int)GEnum.ItemType.Stock:
            //case (int)GEnum.ItemType.Consignment:

            //case (int)GEnum.ItemType.Assembly:
            //case (int)GEnum.ItemType.Non_Stock:
            //case (int)GEnum.ItemType.Service:

            //case (int)GEnum.ItemType.Charges:
            //case (int)GEnum.ItemType.Discount:

            //case (int)GEnum.ItemType.Header:
            //case (int)GEnum.ItemType.Remark:

            //case (int)GEnum.ItemType.BF_Total:
            //case (int)GEnum.ItemType.Sub_Total:
            //case (int)GEnum.ItemType.Total:
            //-------------------------------------------------


            //-------------------------------------------------
            //case GEnum.ItemType.Finished_GDB:
            //case GEnum.ItemType.Serial_Finished_GDB:
            //case GEnum.ItemType.Serial_StockB:
            //case GEnum.ItemType.StockB:
            
            //case GEnum.ItemType.Finished_GD:
            //case GEnum.ItemType.Stock:
            //case GEnum.ItemType.Consignment:

            //case GEnum.ItemType.Assembly:
            //case GEnum.ItemType.Non_Stock:
            //case GEnum.ItemType.Service:

            //case GEnum.ItemType.Charges:
            //case GEnum.ItemType.Discount:

            //case GEnum.ItemType.Header:
            //case GEnum.ItemType.Remark:

            //case GEnum.ItemType.BF_Total:
            //case GEnum.ItemType.Sub_Total:
            //case GEnum.ItemType.Total:

            //case GEnum.ItemType.Master:
            //-------------------------------------------------
        }
        public enum ItmHisSearchType
        {
            Invoice=10,
            Delivery_Order=20,
            Sales_Order=30,
            Quotation=40,
            Invoice_Pending_DO =50,
            All=90,
            Delivery=110,
            Purchase_Order=120,
            Consignment=130
        }
        public enum ItemFilterCondition
        {
            No_Condition = 999,
            Available_Qty_Less_than_Or_Equal_Zero = 130,
            Available_Qty_Less_than_Minimun_Qty = 120,
            Stock_Level_Less_than_Or_Equal_Zero = 110,
            Stock_Level_Less_than_Minimun_Qty = 100
        }
        public enum PriceListCode
        {
            Normal = 10,
            UseLastTransactedSOPrice = 100,
            UseLastTransactedSOAndDOPrice = 110,
            UseLastTransactedSOAndIVPrice = 120,
            UseLastTransactedDOPrice = 130,
            UseLastTransactedDOAndIVPrice = 140,
            UseLastTransactedIVPrice = 150,
            UseLastTransactedAllSalesPrice = 160,
            UseLastTransactedPOPrice = 200,
            UseLastTransactedPOAndPurchaseDeliveryPrice = 210,
            UseLastTransactedPOAndPurchaseIVPrice = 220,
            UseLastTransactedPurchaseDeliveryPrice = 230,
            UseLastTransactedPurchaseDeliveryAndPurchaseIVPrice = 240,
            UseLastTransactedPurchaseIVPrice = 250,
            UseLastTransactedAllPurchasePrice = 260,
            UseStandardPrice = 1000,
            UsePrice1 = 1100,
            UsePrice2 = 1200,
            UsePrice3 = 1300,
            UsePrice4 = 1400,
            UsePrice5 = 1500,
            UsePrice6 = 1600,
            UsePrice7 = 1700,
            UsePrice8 = 1800,
            UsePrice9 = 1900,
            UsePrice10 = 2000,
            UsePrice11 = 2100,
            UsePrice12 = 2200,
            UsePrice13 = 2300,
            UsePrice14 = 2400,
            UsePrice15 = 2500,
            UseStandardCost = 3000

        }
        public enum CompareOperator
        {
            Equal = 0,
            NotEqual = 1,
            GreatherThan = 2,
            GreatherThanEqual = 3,
            LessThan = 4,
            LessThanEqual = 5,
        }

        public enum Message
        {
            IsRequire = 0,
            IsEmpty = 1,
            IsMaxLength = 2,
            IsDate = 3,
            IsBoolean = 4,
            IsNumeric = 5,
            IsEqual = 6,
            OverFlow = 7, // >=0
            Between = 8,
        }

        public enum MsgBoxButton
        {
            OK = 10,
            Cancel = 20,
            I_Dont_Know = 30,
            Save_Changes = 40,
            Discard_Changes = 50,
            Update_Changes = 60,
            Delete = 70,
            Dont_Delete = 80,
            Yes = 90,
            No = 100,
            Clear = 110,
            Dont_Clear = 120,
            Read_Only = 130,
            GoTo_OpenRecord = 140,
            Add_New_Record = 150,
            Try_Again = 160,
            Open = 170,
            Dont_Open = 180,
            Continue = 190,
            Save_Send_Email=200,
            Save_Only=210,
            Append_Job=220,
            Replace_Job=230
        }

        public enum MsgBoxDefaultButton
        {
            DefaultButton1 = 1,
            DefaultButton2 = 2,
            DefaultButton3 = 3,
            DefaultButton4 = 4,
        }

        public enum MsgBoxIcon
        {
            Information,
            Warning,
            Error,
            ErrorInfo,
            Question,
            Alert,
            Serious,
            Help
        }

        public enum SystemCode
        {
            //----FOR PROGRAMMER USE
            //case (int)GEnum.SystemCode.Quotation:                       //Document Enquiry
            //case (int)GEnum.SystemCode.Purchase_Plan:                   //Document Request
            //case (int)GEnum.SystemCode.Purchase_Request:
            //case (int)GEnum.SystemCode.Sales_Order:                     //Document Order
            //case (int)GEnum.SystemCode.Purchase_Order:
            //case (int)GEnum.SystemCode.Order_Consignment:
            //case (int)GEnum.SystemCode.Sales_Order_Adjustment:          //Document Order Adjustment
            //case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
            //case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
            //case (int)GEnum.SystemCode.Works_Order:                     //Document Work
            //case (int)GEnum.SystemCode.Delivery_Order:                  //Document Delivery
            //case (int)GEnum.SystemCode.Purchase_Delivery:
            //case (int)GEnum.SystemCode.Received_Consignment:
            //case (int)GEnum.SystemCode.Issue_Consignment:
            //case (int)GEnum.SystemCode.Return_Consignment:
            //case (int)GEnum.SystemCode.Packing_List:                    //Document Packing List
            //case (int)GEnum.SystemCode.Consignment_Settlement:          //Document Settlement
            //case (int)GEnum.SystemCode.Sales_Invoice:                   //Document Invoice
            //case (int)GEnum.SystemCode.Sales_Debit_Note:
            //case (int)GEnum.SystemCode.Sales_Credit_Note:
            //case (int)GEnum.SystemCode.Cash_Sale:
            //case (int)GEnum.SystemCode.Cash_Debit_Note:
            //case (int)GEnum.SystemCode.Cash_Credit_Note:
            //case (int)GEnum.SystemCode.Purchase_Invoice:
            //case (int)GEnum.SystemCode.Purchase_Debit_Note:
            //case (int)GEnum.SystemCode.Purchase_Credit_Note:
            //case (int)GEnum.SystemCode.Sales_Adjustment:                //Document Adjustment
            //case (int)GEnum.SystemCode.Cash_Adjustment:
            //case (int)GEnum.SystemCode.Purchase_Adjustment:
            //case (int)GEnum.SystemCode.Payment_Received:                //Document Payment
            //case (int)GEnum.SystemCode.Cash_Payment_Received:
            //case (int)GEnum.SystemCode.Payment_Issue:
            //case (int)GEnum.SystemCode.Contra:
            //case (int)GEnum.SystemCode.Cash_Contra:
            //case (int)GEnum.SystemCode.AR_Opening_Balance:              //Code Opening Balance
            //case (int)GEnum.SystemCode.AR_Cash_Opening_Balance:
            //case (int)GEnum.SystemCode.AP_Opening_Balance:
            //case (int)GEnum.SystemCode.AR_Revaluation:                  //Code Revaluation
            //case (int)GEnum.SystemCode.AR_Cash_Revaluation:
            //case (int)GEnum.SystemCode.AP_Revaluation:
            //case (int)GEnum.SystemCode.Inventory_Adjustment:            //Document Inventory
            //case (int)GEnum.SystemCode.Inventory_Production:
            //case (int)GEnum.SystemCode.Inventory_Transfer:
            //case (int)GEnum.SystemCode.Journal:                         //Document Account
            //case (int)GEnum.SystemCode.Deposit:
            //case (int)GEnum.SystemCode.Bank_Revaluation:


            //----FOR PROGRAMMER USE

            // added by ttm on 26-Jan-2017 (start)
            KeyCustomer = 80450,
            KeyCustomerImport = 80500,
            // added by ttm on 26-Jan-2017 (end)

            Utility = -99,
            Quotation = 11100,
            Sales_Order = 11150,
            Reserve_Order = 11151,
            Sales_Order_Adjustment = 11200,
            Untrack_SO = 11250,
            Works_Order = 11300,
            Works_Order_Type = 11305,
            WO_Request_Type = 11310,
            Delivery_Order = 11350,
            DO_to_IV_Transfer = 11400,
            Packing_List = 11450,
            Defect_Report = 11451,
            Sales_Invoice = 11500,
            Sales_Debit_Note = 11550,
            Sales_Credit_Note = 11600,
            Sales_Adjustment = 11650,
            Payment_Received = 11700,
            AR_Opening_Balance = 11750,
            AR_Revaluation = 11800,
            Contra = 11850,
            Cash_Sale = 12100,
            Cash_Debit_Note = 12150,
            Cash_Credit_Note = 12200,
            Cash_Adjustment = 12250,
            Cash_Payment_Received = 12300,
            AR_Cash_Opening_Balance = 12350,
            AR_Cash_Revaluation = 12400,
            Cash_Contra = 12450,
            Purchase_Plan = 13100,
            Purchase_Requisition = 13150,
            Purchase_Request = 13200,
            Purchase_Order = 13250,
            Purchase_Order_Adjustment = 13300,
            Untrack_PO = 13350,
            AP_PO_Confirm_Number = 13400,
            Purchase_Shipment = 13430,
            Purchase_Delivery = 13450,
            Purchase_Invoice = 13500,
            Purchase_Debit_Note = 13550,
            Purchase_Credit_Note = 13600,
            Purchase_Adjustment = 13650,
            GL_Payment = 13701,
            Payment_Issue = 13700,
            AP_Opening_Balance = 13750,
            AP_Revaluation = 13800,
            Inventory_Adjustment = 14100,
            Inventory_Production = 14150,
            Inventory_Transfer = 14200,
            Issue_Consignment = 15100,
            Return_Consignment = 15150,
            Untrack_Issue_Consignment = 15200,
            Order_Consignment = 15250,
            Consignment_Order_Adjustment = 15300,
            Untrack_Consignment_Order = 15350,
            Received_Consignment = 15400,
            Consignment_Settlement = 15450,
            Journal = 16100,
            Deposit = 16150,
            Bank_Revaluation = 16200,
            Main_Screen = 20100,
            System_Code = 20150,
            CounterGrp = 20200,
            System_Option = 20250,
            Screen_Customisation = 20300,
            Company_Setup_Check_List = 20350,
            Document_Group = 20400,
            General_List = 20450,
            Audit_Log = 20500,
            Account = 21100,            
            Account_Opening_Balance = 21150,
            Account_Unreconciled_Trans = 21200,
            Period = 21250,
            Branch = 21300,
            Department = 21350,
            Bank_Reconcilation = 21400,
            COSBatchPost = 21450,
            Currency = 21500,
            Bank = 21550,
            Payment_Mode = 21600,
            Tax_Authority = 21650,
            Tax_Group = 21700,
            Overhead = 21750,
            Account_Group = 21800,
            Sales_Representative = 21850,
            Budget = 21900,
            Transaction_Group = 21950,
            ARAP_Revaluation = 21960,
            CustomerManage = 22220,
            Customer = 22100,
            Vendor = 22150,
            Price_List = 22300,
            Price_Update = 22350,
            Payment_Term = 22400,
            Territory = 22450,
            Industry = 22500,
            Shipping_Mode = 22550,
            Packing_Type = 22600,
            Ship_Name = 22650,
            Interest = 22700,
            Inventory = 23100,
            Inventory_Opening_Balance = 23150,
            Category = 23200,
            Brand = 23250,
            UOM = 23300,
            Color = 23350,
            Scale = 23400,
            Location = 23450,
            Stock_Count = 23500,
            Item_Copy = 23550,
            Job = 24100,
            Job_Opening_Balance = 24150,
            Job_Cost_Type = 24200,
            Job_Phase = 24250,
            Job_Task = 24300,
            Job_Group = 24350,
            Job_Timesheet = 24400,
            Machine_List = 30100,
            Machine_Type_List = 30150,
            Alerts = 70100,
            Alert_Log = 70150,
            To_Do = 70200,
            To_Do_Log = 70250,
            Other_Report_Setting = 80100,
            Report_ID_Format = 20600,//- remove Not Used
            Report_Set_Rpt_Files = 80200,
            Cash_Flow = 80250,
            Financial_Charge = 80300,
            Interest_Rate = 80350, // Need To Ask
            Security_User = 90100,
            Security_Group = 90150,
            Security_ChangePassword = 90151,
            Uploaded_Document = 95100,
            Import_Data = 95150,
            Message_List = 20550,
            RecordAccess = 90250,

            Financial_Statement = 80400,
            Vehicle = 95000,
            WorkOrder = 95200,
            EStorePriceUpload = 42100,
            DOTrack = 96000,
            Consolidate_Invoice = 11411,
            IV_to_CSDIV_Transfer = 11410,
            MRO_TopCust=30001,
            ItemHSCodeUpload = 42101
        }

        public enum SYSMsgList
        {
            AccessGrp = 10,
            AccOpenSelection = 11,
            AccTypeKey = 12,
            AddrLinkType = 13,
            AlertApplyTo = 14,
            AlertApplytoGrp = 15,
            AlertLogRecType = 16,
            AlertStatus = 17,
            AlertType = 18,
            AllowOutOfStock = 19,
            ArrowBehavior = 20,
            BatchLogType = 21,
            BOMLineType = 22,
            BOMType = 23,
            BudgetItmMode = 24,
            BudgetMode = 25,
            BudgetType = 26,
            CashFlowParticularType = 27,
            CashFlowRecType = 28,
            CCBType = 29,
            CDefaultStateType = 30,
            CheckCustomerLimit = 31,
            CodeGrp = 32,
            ContactLinkType = 33,
            ContactType = 34,
            ConType = 35,
            CostMethod = 36,
            DataType = 37,
            DateType = 38,
            DefaultDataMatrixSelection = 39,
            DeptDistributionPeriod = 40,
            DeptDistributionType = 41,
            DocApproveOption = 42,
            DocMaximumRow = 43,
            DocPriceAssign = 44,
            DocState = 45,
            DocType = 46,
            FieldBehavior = 47,
            Gender = 48,
            INSerialLogType = 49,
            INType = 50,
            INTypeMaster = 51,
            InventoryValuationMethod = 52,
            ItmStatus = 53,
            JobStatus = 54,
            LandedCostOption = 55,
            LatestCostOption = 56,
            LogBatchMode = 57,
            LogMode = 58,
            LogSeq = 59,
            Marital = 60,
            MFNCostMode = 61,
            MFNQtyRndDecPlace = 62,
            MsgButton = 63,
            MsgTextGrp = 65,
            MthWeek = 66,
            OperateAsType = 67,
            OpGrp = 68,
            OthLineType = 69,
            PeriodType = 70,
            PermGrpKey = 71,
            PermType = 72,
            PlanMth = 73,
            PostErr = 74,
            PriceDecPlace = 75,
            PriceListCode = 76,
            PriceType = 77,
            QtyMultiplier = 78,
            RatioType = 79,
            RecDetailType = 80,
            ReconPurgeType = 81,
            RecurType = 82,
            RemindType = 83,
            RepGrp = 84,
            ReportCode = 85,
            ReportStatus = 86,
            RFQVendorStatus = 87,
            RptLayOut = 88,
            SalesPriceControl = 89,
            SerialStatus = 90,
            StatementAgeCalculation = 91,
            StatementAgeMthorDays = 92,
            StatementAgeNoOfMth = 93,
            StockCountStatus = 94,
            SyncStatus = 95,
            SysLogType = 96,
            ToDoPriority = 97,
            ToDoStatus = 98,
            ToDoType = 99,
            TransmitMode = 100,
            TransmitStatus = 101,
            UOMDesOption = 102,
            UOMType = 103,
            WarnCurrencyRateNotValid = 104,
            WarnNonConvertableUOM = 105,
            WeekDay = 106,
            YearMth = 107,
            SalesRepTransactionType = 110,
            JobHeaderType = 111,
            SystemPeriodStatus = 112,
            QuoteStatus = 108,
            MsgListGrp = 999,
            SeachMatchOption = 113,
            ItemHistorySearchType = 121,
        }
        public enum SearchMatchOption
        {
            AnyPartofField = 10,
            StartofField = 20
        }
        public enum ReportSearchOption
        {
            Range = 10,
            WildCard = 20
        }
        public enum SystemPeriodStatus
        {
            Open = 0,
            Lock = 10,
            Closed = 99,
            All = -1,
        }
        public enum MsgListTextGrp
        {
            Action = 10,
            AddressType = 20,
            AlertCondition = 30,
            Country = 40,
            DataType = 50,
            DocRemarks = 60,
            LogTrans = 70,
            MsgListTextDataGrp = 80,
            Region = 90,
            RepType = 100,
            SegmentType = 110,
            EqpipmenttStatus = 120,

        }

        public enum PermType
        {
            PERFORM = 10,
            READ = 20,
            RE = 30,
            RA = 40,
            REA = 50,
            RD = 60,
            EAD=70,
        }

        public enum SECUserCustomUpdateOption
        {
            ResetUserLoginRetry = 0,
            IncrementUserLoginRetry = 1,
            SetUserLockout = 2,
            SaveUserLogin = 3,
            SaveUserLogoff = 4,
            ChangePassword = 5,
        }

        public enum BudgetMode
        {
            Equity = 100,
            Retain_Earning = 110,
            Fixed_Asset = 210,
            Accu_Depreciation = 220,
            Current_Asset = 230,
            Other_Asset = 240,
            Inventory = 250,
            Bank = 300,
            Petty_Cash = 310,
            Temp_Holding_Fund = 320,
            Account_Receivable = 400,
            Other_Receivable = 410,
            Account_Payable = 500,
            Other_Payable = 510,
            Current_Liability = 600,
            Long_Term_Liability = 610,
            Provision_for_Tax = 620,
            Income = 700,
            Other_Income = 710,
            Opening_Stock = 800,
            Purchases = 810,
            Cost_of_Sales = 820,
            Closing_Stock = 830,
            Expenses = 900,
            Other_Expenses = 910
        }

        public enum BudgetType
        {
            Account = 100,
            Item_Sales = 110,
            Item_Purchases = 120,
            Customer_Sales = 210,
            Industry_Sales = 220,
            Territory_Sales = 230,
            Vendor_Purchase = 310,
            Industry_Purchase = 320,
            Territory_Purchase = 330,
            Document_Group_Sales = 410,
            Document_Group_and_Item_Sales = 420,
            Document_Group_Purchase = 430,
            Document_Group_Item_Purchases = 440
        }

        public enum BudgetItemMode
        {
            Unit = 10,
            Weight = 20,
        }

        public enum BudgetModeValue
        {
            Budget = 10,
            Target = 20,
        }

        public enum FormOpenMode
        {
            Add = 10,
            Edit = 20,
        }


        public enum Details
        {
            Doc_Itm = 10,
            Doc_Exp = 20,
            Doc_Pack = 30,
            Doc_Vendor = 40,
            Doc_ItmVendor = 50,
            Doc_Attachment=60,
            WO_PartsUsed=70,
            WO_Req=80,
            Doc_CsgItm=90 //May 11 Feb 2020 

            #region Remove later
            //AP_BLItm=0,
            //AP_PDItm=1,
            //AP_PJItm=2,
            //AP_PNItm=3,
            //AP_POItm = 4,
            //AP_PYDetExp	=5,
            //AP_PYDetItm	=6,
            //AP_RQDetItm	=7,
            //AR_CTDetItm	=8,
            //AR_DODetItm	=9,
            //AR_IVDetItm	=10,

            //AR_PLDetPack_Itm=11,
            //AR_PYDetExp	=12,
            //AR_PYDetItm	=13,

            //AR_SODetItm	=14,

            //CS_CPDDetItm=15,
            //CS_CPODetItm=16,
            //CS_CPSDetItm=17,
            //CS_CSIDetExp=18,
            //CS_CSIDetItm=19,

            //AR_QOVendor = 20,
            //AR_QOItm =21,
            //AR_QOItmVendor = 22,

            //GL_GPDetItm=23,
            //GL_GRNDetItm=24,

            //IN_ADJItm = 25,
            //IN_MFNItm = 26,
            //IN_TRNItm = 27,
            //CS_CSIExp = 28,
            //CS_CPOItm = 29,
            //CS_CPDItm = 30,
            //CS_CPSItm = 31,
            //GL_JNLItm = 32,
            //GL_DPItm = 33,            
            //AR_CTItm = 34,
            //AR_PYExp = 35,
            //AP_PYExp = 36,          
            //AR_PLPack_Itm =37, 
            #endregion
        }
        public enum ItemAdjustmentStatus
        {
            No_Adjustment = 10,
            Advance = 20,
            Postpone = 30,
            Cancel = 40
        }

        public enum DocState
        {
            New = 10,
            Draft = 20,
            Pending = 30,
            Rejected = 40,
            Approved = 50,
            InProgress = 60,
            Posted = 100,
            Void = 200,
            Invoiced = 300,
            Transferred = 400
        }

        public enum DocAction
        {
            Save = 0,
            Submit = 1,
            Reject = 2,
            Approve = 3,
            Post = 4,
            Delete = 5,
            Print = 6,
            Undetermine = 99
        }

        public enum DocSearchType
        { 
            NA = 0,
            Cust_Vend_Name = 10,
            Ship_Name = 20,
            Ship_Mark = 30,
            Document_Ref = 40
        }

        public enum WOSearchType
        {
            NA = 0,
            Work_Order = 10,
            Work_Order_Type = 20,
            Status = 30,
            Vehicle = 40,
            Customer_Name = 50
        }

        public enum DocApproveType
        {
            None= 10,
            ApprovalOnly = 20,
            ApprovalByLimitSet = 30
        }

        public enum PopupType
        {
            CusID = 1,
            CusNm = 2,
            VendID = 3,
            VendNm = 4,
            VendItmID = 5,
            VehicleID = 6,
            Vehicle = 7,
            JobID = 10,
            JobDes = 20,
            ItmID = 100,
            ItmDes = 200,
            ItmFinishID = 300,
            ItmFinishDes = 400,
            ItmStkID = 500,
            ItmStkDes = 600,
            AccID = 1000,
            AccDes = 2000,
            AccDisID = 3000,
            AccLiabilityID = 4000,
            AccLiabilityDes = 5000,
            ItmSalesID = 800,
            ItmSalesDes = 802,
            ItmPurchaseID = 804,
            ItmPurchaseDes = 806,
            ItmJobID = 808,
            ItmJobDes = 810,
            ItmPackID = 812,
            ItmPackDes = 814,
            ItmFSNVGid = 816,
            ItmFSNVGdes = 818,
            ItmFSCid = 820,
            ItmFSCdes = 822,
            ItmCid = 824,
            ItmCdes = 826,
            ItmFSCANid = 828,
            ItmFSCANdes = 830,
            ItmFSCANVGid = 832,
            ItmFSCANVGdes = 834,
            ItmGid = 836,
            ItmGdes = 838,
            ItmVid = 840,
            ItmVdes = 842,
            ItmMid = 844,
            ItmMdes = 846,
            ItmFSid = 848,
            ItmFSdes = 850,
            ItmFid = 852,
            ItmFdes = 854,
            //---For OpenID
            ShipNmOpenID = 856,
            PriceInfoOpenID = 858,
            ItmOpenID = 860,
            CustOpenID = 862,
            VendOpenID = 864,
            AccOpenID = 866,
            JobOpenID = 868,
            VehicleOpenID = 870,
        }
        public enum RecDetailType
        {
            NoDetail = 10,
            DItems = 1000,
            DItmAssembly = 1100,
            DItmAssembly_Batch = 1110,
            DItmAssembly_Batch_Serial = 1120,
            DItmAssembly_Serial = 1130,
            DItmBatch = 1200,
            DItmBatch_Serial = 1210,
            DItmSerial = 1300,
            DItmExpenses = 2000,
            DItmFinishedGoods = 3000,
            DItmFinishedGoods_Batch = 3010,
            DItmFinishedGoods_Batch_Serial = 3020,
            DItmFinishedGoods_Serial = 3030,
            DItmRaw_Material = 3100,
            DItmRawMaterial_Batch = 3110,
            DItmRawMaterial_Batch_Serial = 3120,
            DItmRawMaterial_Serial = 3130,
            DItmPacking_Material = 3200,
            DItmPackingMaterial_Batch = 3210,
            DItmPackingMaterial_Batch_Serial = 3220,
            DItmPackingMaterial_Serial = 3230,
            DItmOtherManuafacturingCost = 3300,
            DItmCSGSettlement_ARIV = 4000,
            DItmCSGSettlement_INADJ = 4010,
            DItmCSGSettlement_CS_CPReturn = 4020,
            DItmCSGSettlement_Expenses = 4030
        }

        public enum GLLogSeq
        {
            DocDetItm = 20,
            DocDetItmChild = 25,
            DocDetExp = 30,
            DocHeader = 50,
            SYSClosing = 99
        }

        public enum InventoryValuationMethod
        {
            Continuous = 10,
            Periodic = 20,
            COSBatchPosting = 30

        }

        public enum DOHoldOption
        {
            Enable = 10,
            Disable = 20
        }
        public enum DOTransferReportType
        {
            None = 10,
            Summary = 20,
            Detail = 30
        }
        public enum DOTransferSelectionType
        {
            SelectedTransfer = 20,
            SelectedNotTransfer = 10
        }
        public enum ItmStatus
        {
            No_Adjustment = 10,
            Advance = 20,
            Postphone = 30,
            Cancel = 40
        }
        public enum FormCtrlLockMode
        {
            FormLoad = 10,
            DataLoad = 20,
            DocData_Change = 30,
            DocDetItmData_Change = 40,
            DocDetItmRow_OnCurrent = 50
        }

        public enum ValidateField
        {
            DocAccKey = 10,
            DocConKey = 20,
            DocJobKey = 30,
            DocCurrKey = 35,
            ItmKey = 40,
            ItmAccKey = 50,
            ItmConKey = 60,
            ItmJobKey = 70,
            ItmQty = 80,
            ItmQtyAdj = 90,
            ItmOrderStatus = 100,
            ItmUOMKey = 110,
            DelDocItmKey = 120
        }
        public enum DocumentLinkStatus
        {
            Pass = 10,
            Fail = 20,
            Warning = 30
        }

        public enum CopyOption
        {
            CopyMySelf = 0,
            CopyFrom = 1,
            Import = 2
        }

        public enum CtlPropertyUpdate
        {
            Readonly = 10,
            Enabled = 20,
            Visible = 30,
            Value = 100,
        }
        public enum PopupCSSortOrder
        {
            ItmID = 10,
            DocID = 20,
        }
        public enum RecAccessTypeDC
        {
            CustKey_DC = 10,
            CustID_DC = 20,
            CustNm_DC = 30,
            VendKey_DC = 40,
            VendID_DC = 50,
            VendNm_DC = 60,
            ItemKey_DC = 70,
            ItemID_DC = 80,
            ItemDes_DC = 90,
            JobKey_DC = 100,
            JobID_DC = 110,
            JobDes_DC = 120,
        }

        public enum RecType
        {
            Cust = 10,
            Vend = 20,
            Job = 30,
            Itm = 40
        }

        public enum RecAccessTypeCust
        {
            CustKey = 10,
            CustID = 20,
            CustNm = 30,
        }

        public enum RecAccessTypeVend
        {
            VendKey = 40,
            VendID = 50,
            VendNm = 60,

        }
        public enum RecAccessTypeItem
        {
            ItemKey = 70,
            ItemID = 80,
            ItemDes = 90,
            ItemKeySub = 100,
            ItemIDSub = 110,
            ItemDesSub = 120,

        }
        public enum RecAccessTypeJob
        {
            JobKey = 130,
            JobID = 140,
            JobDes = 150
        }
        public enum RecAccessTypeAcc
        {
            AccKey = 160,
            AccID = 170,
            AccDes = 180
        }

        public enum RecAccessType
        {
            CustKey = 10,
            CustID = 20,
            CustNm = 30,
            VendKey = 40,
            VendID = 50,
            VendNm = 60,
            ItemKey = 70,
            ItemID = 80,
            ItemDes = 90,
            ItemKeySub = 100,
            ItemIDSub = 110,
            ItemDesSub = 120,
            JobKey = 130,
            JobID = 140,
            JobDes = 150,
            AccKey = 160,
            AccID = 170,
            AccDes = 180,
            Vehicle = 200

        }
        public enum InventoryDetails
        {
            FinishedGoods = 10,
            RawMaterial = 20,
            PackingMaterial = 30
        }
        public enum AccountOpeningBalacne
        {
            ByCompany = 10,
            ByDepartment = 20
        }
        public enum PriceUpdateOption
        {
            UpdateCustomerPriceList =10,
            UpdateInventoryPriceList = 20,
             UpdateInventoryPriceRatioList = 30
            
        }
        public enum PriceUpdateMode
        {
            PreviewData =10,
            Update = 20             
        }
        public enum PriceType
        {
            ByValue = 10,
            ByPercentage = 20
        }
        public enum StockCountSelection
        {
            All = 10,
            Counted = 20,
            Remaining = 30,
            Discrepancies = 40
        }
        public enum StockCountStatus
        {
            NA = 10,
            NewCount = 20,
            StartCount = 30,
            StopCount = 40,
            SaveEntries = 50,
            Pending = 60,
            Completed = 70
        }
        public enum AdjDefault
        {
            ByDate = 1,
            Month1 = 10,
            Month2 = 20,
            Month3 = 30,
            Month4 = 40,
            Month5 = 50,
            Month6 = 60,
            Month7 = 70,
            Month8 = 80,
            Month9 = 90,
            Month10 = 100,
            Month11 = 110,
            Month12 = 120
        }
        public enum DocPriceAssign
        {
            FIFO = 10,
            LastConsignmentPrice = 20,
            LastVendorPrice = 30,
            ItemLatestCost = 40,
            ItemStdCost = 50,
        }

        //Financial Report Form Enum
        public enum FinDetType
        {
            Header = 5,
            Column = 10,
            Body_Text = 20,
            Line_Detail = 30,
            Total = 40,
            To_be_removed = 50,
            Footer = 90
        }

        public enum FinColType
        {
            Not_Used = 10,
            AccountID = 20,
            Balance = 40,
            AccountDes = 50,
            Formula = 60,
            BranchID = 70,
            BranchDes = 75,
            DeptID = 80,
            DeptDes = 85,
            Blank = 90

        }

        public enum FinQualifier
        {
            Balance = 10,
            Reverse_Balance = 20,
            Only_Debits = 30,
            Only_Credits = 40,
            Budget = 50
        }

        public enum FinActivity
        {
            Period = 10,
            Year_to_Date = 20,
            Balance = 30,
        }

        public enum FinStatementMeasurement
        {
            Centimeters = 10,
            Inches =20
        }

        public enum FinTimeFram
        { 
            Current	= 10,
            Current1	=	20,
            Current2	=	30,
            Current3	=	40,
            Current4	=	50,
            Current5	=	60,
            Current6	=	70,
            Current7	=	80,
            Current8	=	90,
            Current9	=	100,
            Current10	= 110,
            Current11	= 120,
            Current12	= 130,
            LastYear	=	140,
            LastYear1	=	150,
            LastYear2	=	160,
            LastYear3	=	170,
            LastYear4	=	180,
            LastYear5	=	190,
            LastYear6	=	200,
            LastYear7	=	210,
            LastYear8	=	220,
            LastYear9	=	230,
            LastYear10	=	240,
            LastYear11	=	250,	
            LastYear12	=	260,
            LastTwoYearAgo	=	270,
            LastTwoYearAgo1	=	280,
            LastTwoYearAgo2	=	290,
            LastTwoYearAgo3	=	300,
            LastTwoYearAgo4	=	310,
            LastTwoYearAgo5	=	320,
            LastTwoYearAgo6	=	330,
            LastTwoYearAgo7	=	340,
            LastTwoYearAgo8	=	350,
            LastTwoYearAgo9	=	360,
            LastTwoYearAgo10	= 370,
            LastTwoYearAgo11	=	380,
            LastTwoYearAgo12	=	390,
            TotalThisYear	=	400,
            TotalLastYear	=	410,
            TotalTwoYearAgo	=	420	

        }

        public enum FinRowDisplayType
        {
            Display = 10,
            Hide_when_zero = 20,
            Hide = 30
        }

        //This For the Feature Use
        public enum FinRepType
        {
            Active_Report = 10,
            Crystal_Report = 20
        }

        public enum PaperOrientation
        {
            Portrait = 10,
            Landscape = 20
        }

        public enum PeriodType
        { 
            Use_Selected_Period = 10,
            Current_Period = 20,
            Current_Year_Jan = 30,
            Current_Year_Dec = 40,
            Current_Fiscal_Year_Start = 50,
            Current_Fiscal_Year_End = 60
        }

        public enum FinRpxDetType
        {
            Detail_Show = 10,
            Detail_Hide = 20,
            Summary = 30,
            BodyText = 40,
            Total = 50,
            Formula = 60,
            Column = 70
        }

        public enum FormSettingGetType
        {
            PopupForm = 2,
            ListSettingID = 3,
            HeaderPopupForm = 4,
            HeaderListSettingID = 5,           
        }

        public enum DetType
        {
            Detail_Hide = 0,
            Detail_Show = 1,
            Summary,
            BodyText,
            Total
        }

        public enum formInitMode
        {
            Neither = 0,
            Add = 10,
            Edit = 20
        }
        public enum InsertAction 
        {           
            InsertSO = 10,
            InsertPO = 20,
            InsertCO = 30,
            InsertCS = 40,
            InsertPD = 50,
            InsertCSR = 60,
            InsertData = 70,
            DOIVTransfer = 100
            

        }

    }
}
