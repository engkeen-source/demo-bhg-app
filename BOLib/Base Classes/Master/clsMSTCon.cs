
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTCon : Csla.BusinessBase<MSTCon>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _conKey = 0;
        internal string _conID = string.Empty;
        internal string _conNm = string.Empty;
        internal int? _conType = 0;
        internal int? _cCBType = 0;
        internal bool? _noFinCharge = false;
        internal int? _accessLevel = 0;
        internal int? _accessGroup = 0;
        internal string _conUEN = string.Empty;
        internal bool? _inactive = false;
        //added by nnt on April 2019 check approved
        internal bool? _approval = false;
        internal bool? _rejected = false;
        //
        internal bool? _activewithproblem = false;
        internal bool? _cooapprovalrequired = false;
        internal int? _cBranchKey = 0;
        internal string _cBranchID = string.Empty;
        internal int? _cDeptKey = 0;
        internal string _cDeptID = string.Empty;
        internal int? _cGrpKey = 0;
        internal string _cGrpID = string.Empty;
        internal int? _cTerritoryKey = null;
        internal string _cTerritoryID = string.Empty;
        internal int? _cIndustryKey = null;
        internal string _cIndustryID = string.Empty;
        internal string _cClass = string.Empty;
        internal int? _cPriceType = null;
        internal decimal? _cOverallDefaultDis = 0;
        internal int? _cTermKey = null;
        internal decimal? _cCreditLimit = 0;
        internal int? _cTaxGrpKey = null;
        internal int? _cEMKey = null;
        internal string _cemid = string.Empty;
        internal int? _cCurrkey = 1;
        internal string _cCurrID = string.Empty;
        internal int? _cAccKey = null;
        internal string _cDefaultBillAddr = string.Empty;
        internal string _cDefaultShipAddr = string.Empty;
        internal string _cDefaultStateAddr = string.Empty;
        internal int? _cDefaultStateType = 10;
        internal string _cDefaultContact = string.Empty;
        internal string _cDefaultContactState = string.Empty;
        internal string _cRemDelivery = string.Empty;
        internal string _cRemPrice = string.Empty;
        internal string _cRemValidity = string.Empty;
        internal string _cRemPayment = string.Empty;
        internal string _cRem = string.Empty;
        internal string _formerknownas = string.Empty;
        internal bool? _cAttachment = false;
        internal DateTime? _customerSinceDate = null;
        internal decimal? _cCreditBal = 0;
        internal decimal? _cCashBal = 0;
        internal decimal? _cNTBal = 0;
        internal int? _vBranchKey = 0;
        internal string _vBranchID = string.Empty;
        internal int? _vDeptKey = 0;
        internal string _vDeptID = string.Empty;
        internal int? _vGrpKey = 0;
        internal string _vGrpID = string.Empty;
        internal int? _vTerritoryKey = null;
        internal string _vTerritoryID = string.Empty;
        internal int? _vIndustryKey = null;
        internal string _vIndustryID = string.Empty;
        internal string _vClass = string.Empty;
        internal int? _vPriceType = null;
        internal decimal? _vOverallDefaultDis = 0;
        internal int? _vTermKey = null;
        internal decimal? _vCreditLimit = 0;
        internal int? _vTaxGrpKey = null;
        internal int? _vEMKey = null;
        internal string _vemid = string.Empty;
        internal int? _vCurrkey = 1;
        internal string _vCurrID = string.Empty;
        internal int? _vAccKey = null;
        internal string _vDefaultBillAddr = string.Empty;
        internal string _vDefaultShipAddr = string.Empty;
        internal string _vDefaultContact = string.Empty;
        internal string _vDefaultAPPYDocType = string.Empty;
        internal string _vRemDelivery = string.Empty;
        internal string _vRemPrice = string.Empty;
        internal string _vRemValidity = string.Empty;
        internal string _vRemPayment = string.Empty;
        internal string _vRem = string.Empty;
        internal bool? _vAttachment = false;
        internal DateTime? _vendorSinceDate = null;
        internal decimal? _vBal = 0;
        internal string _conNamFirst = string.Empty;
        internal string _conNamLast = string.Empty;
        internal string _conNamMiddle = string.Empty;
        internal string _conNamInitials = string.Empty;
        internal DateTime? _conBirthday = null;
        internal int? _conGender = null;
        internal int? _conMarital = null;
        internal short? _conChildren = 0;
        internal string _conSocSecNo = string.Empty;
        internal string _conNationality = string.Empty;
        internal string _occuTitle = string.Empty;
        internal string _occuIndustry = string.Empty;
        internal string _occuSalary = string.Empty;
        internal string _occuGroup = string.Empty;

        internal bool _emailStatement = false;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = 0;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = 0;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _custom4 = string.Empty;
        internal string _custom5 = string.Empty;
        //added by thettm on 02 April 2018(start)
        internal string _accountName = string.Empty;
        internal string _bankName = string.Empty;
        internal string _branchName = string.Empty;
        internal string _country = string.Empty;
        internal string _bankCode = string.Empty;
        internal string _branchCode = string.Empty;
        internal string _sWIFTCode = string.Empty;
        internal int? _bankCurrKey = 1;
        internal string _bankCurrID = string.Empty;
        internal string _bankAccountNo = string.Empty;
        internal string _iBAN_NO = string.Empty;
        internal string _iNTBankName = string.Empty;
        internal string _iNTBankCountry = string.Empty;
        internal string _iNTBankCode = string.Empty;
        internal string _iNTBranchCode = string.Empty;
        internal string _iNTSWIFTCode = string.Empty;
        internal int? _iNTCurrKey = 1;
        internal string _iNTCurrID = string.Empty;
        internal string _iNTBankAccountNo = string.Empty;
        internal string _bankAddress = string.Empty;
        internal string _deliModeCode = string.Empty;
        internal string _deliModeCodeValue = string.Empty;
        internal bool _salesRepIsHeadSales = false;
        //added by nnt on 4th Sept 2019

        //added by nnt on 4th Sept 2019

        //added by thettm on 02 April 2018(end)
        internal string _error = string.Empty;
        private SYSAttachments attachments = new SYSAttachments();

        //added by thettm on 02 April 2018(start)
        public string DeliModeCodeValue
        {
            get
            {
                return _deliModeCodeValue;
            }
            set
            {
                _deliModeCodeValue = value;
            }
        }
        public string DeliModeCode
        {
            get
            {
                return _deliModeCode;
            }
            set
            {
                _deliModeCode = value;
            }
        }
        public string BankAddress
        {
            get
            {
                return _bankAddress;
            }
            set
            {
                _bankAddress = value;
            }
        }
        public string INTBankAccountNo
        {
            get
            {
                return _iNTBankAccountNo;
            }
            set
            {
                _iNTBankAccountNo = value;
            }
        }
        public string INTCurrID
        {
            get
            {
                return _iNTCurrID;
            }
            set
            {
                _iNTCurrID = value;
            }
        }
        public int? INTCurrKey
        {
            get
            {
                return _iNTCurrKey;
            }
            set
            {
                _iNTCurrKey = value;
            }
        }
        public string INTSWIFTCode
        {
            get
            {
                return _iNTSWIFTCode;
            }
            set
            {
                _iNTSWIFTCode = value;
            }
        }
        public string INTBranchCode
        {
            get
            {
                return _iNTBranchCode;
            }
            set
            {
                _iNTBranchCode = value;
            }
        }
        public string INTBankCode
        {
            get
            {
                return _iNTBankCode;
            }
            set
            {
                _iNTBankCode = value;
            }
        }
        public string INTBankCountry
        {
            get
            {
                return _iNTBankCountry;
            }
            set
            {
                _iNTBankCountry = value;
            }
        }
        public string INTBankName
        {
            get
            {
                return _iNTBankName;
            }
            set
            {
                _iNTBankName = value;
            }
        }
        public string IBAN_NO
        {
            get
            {
                return _iBAN_NO;
            }
            set
            {
                _iBAN_NO = value;
            }
        }
        public string BankAccountNo
        {
            get
            {
                return _bankAccountNo;
            }
            set
            {
                _bankAccountNo = value;
            }
        }
        public string BankCurrID
        {
            get
            {
                return _bankCurrID;
            }
            set
            {
                _bankCurrID = value;
            }
        }
        public int? BankCurrKey
        {
            get
            {
                return _bankCurrKey;
            }
            set
            {
                _bankCurrKey = value;
            }
        }
        public string SWIFTCode
        {
            get
            {
                return _sWIFTCode;
            }
            set
            {
                _sWIFTCode = value;
            }
        }
        public string BranchCode
        {
            get
            {
                return _branchCode;
            }
            set
            {
                _branchCode = value;
            }
        }
        public string BankCode
        {
            get
            {
                return _bankCode;
            }
            set
            {
                _bankCode = value;
            }
        }
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
            }
        }
        public string BranchName
        {
            get
            {
                return _branchName;
            }
            set
            {
                _branchName = value;
            }
        }
        public string BankName
        {
            get
            {
                return _bankName;
            }
            set
            {
                _bankName = value;
            }
        }
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
            }
        }
        //added by thettm on 02 April 2018(end)
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
            }
        }

        public int? ConKey
        {
            get
            {
                return _conKey;
            }
            set
            {
                _conKey = value;
                PropertyHasChanged("ConKey");
            }
        }

        public string ConID
        {
            get
            {
                return _conID;
            }
            set
            {
                _conID = value;
                PropertyHasChanged("ConID");
            }
        }

        public string ConNm
        {
            get
            {
                return _conNm;
            }
            set
            {
                _conNm = value;
                PropertyHasChanged("ConNm");
            }
        }

        public int? ConType
        {
            get
            {
                return _conType;
            }
            set
            {
                if (_conType != value)
                {
                    _conType = value;
                    PropertyHasChanged("ConType");
                }
            }
        }

        public int? CCBType
        {
            get
            {
                return _cCBType;
            }
            set
            {
                if (_cCBType != value)
                {
                    _cCBType = value;
                    PropertyHasChanged("CCBType");
                }
            }
        }

        public bool? NoFinCharge
        {
            get
            {
                return _noFinCharge;
            }
            set
            {
                _noFinCharge = value;
                PropertyHasChanged("NoFinCharge");
            }
        }

        public int? AccessLevel
        {
            get
            {
                return _accessLevel;
            }
            set
            {
                _accessLevel = value;
                PropertyHasChanged("AccessLevel");
            }
        }

        public int? AccessGroup
        {
            get
            {
                return _accessGroup;
            }
            set
            {
                if (_accessGroup != value)
                {
                    _accessGroup = value;
                    PropertyHasChanged("AccessGroup");
                }
            }
        }

        public string ConUEN
        {
            get
            {
                return _conUEN;
            }
            set
            {
                _conUEN = value;
                PropertyHasChanged("ConUEN");
            }
        }
        public bool? Inactive
        {
            get
            {
                return _inactive;
            }
            set
            {
                _inactive = value;
                PropertyHasChanged("Inactive");
            }
        }
        //added by nnt on April 2019
        public bool? Approval
        {
            get
            {
                return _approval;
            }
            set
            {
                _approval = value;
                PropertyHasChanged("Approval");
            }
        }

        public bool? Rejected
        {
            get
            {
                return _rejected;
            }
            set
            {
                _rejected = value;
                PropertyHasChanged("Rejected");
            }
        }
        //end

        public bool? ActiveWithProblem
        {
            get
            {
                return _activewithproblem;
            }
            set
            {
                _activewithproblem = value;
                PropertyHasChanged("ActiveWithProblem");
            }
        }

        public bool? COOApprovalRequired
        {
            get
            {
                return _cooapprovalrequired;
            }
            set
            {
                _cooapprovalrequired = value;
                PropertyHasChanged("COOApprovalRequired");
            }
        }

        public int? CBranchKey
        {
            get
            {
                return _cBranchKey;
            }
            set
            {
                if (_cBranchKey != value)
                {
                    _cBranchKey = value;
                    PropertyHasChanged("CBranchKey");
                }
            }
        }

        public string CBranchID
        {
            get
            {
                return _cBranchID;
            }
            set
            {
                _cBranchID = value;
                PropertyHasChanged("CBranchID");
            }
        }

        public int? CDeptKey
        {
            get
            {
                return _cDeptKey;
            }
            set
            {
                if (_cDeptKey != value)
                {
                    _cDeptKey = value;
                    PropertyHasChanged("CDeptKey");
                }
            }
        }

        public string CDeptID
        {
            get
            {
                return _cDeptID;
            }
            set
            {
                _cDeptID = value;
                PropertyHasChanged("CDeptID");
            }
        }

        public int? CGrpKey
        {
            get
            {
                return _cGrpKey;
            }
            set
            {
                if (_cGrpKey != value)
                {
                    _cGrpKey = value;
                    PropertyHasChanged("CGrpKey");
                }
            }
        }

        public string CGrpID
        {
            get
            {
                return _cGrpID;
            }
            set
            {
                _cGrpID = value;
                PropertyHasChanged("CGrpID");
            }
        }

        public int? CTerritoryKey
        {
            get
            {
                return _cTerritoryKey;
            }
            set
            {
                if (_cTerritoryKey != value)
                {
                    _cTerritoryKey = value;
                    PropertyHasChanged("CTerritoryKey");
                }
            }
        }

        public string CTerritoryID
        {
            get
            {
                return _cTerritoryID;
            }
            set
            {
                _cTerritoryID = value;
                PropertyHasChanged("CTerritoryID");
            }
        }

        public int? CIndustryKey
        {
            get
            {
                return _cIndustryKey;
            }
            set
            {
                if (_cIndustryKey != value)
                {
                    _cIndustryKey = value;
                    PropertyHasChanged("CIndustryKey");
                }
            }
        }

        public string CIndustryID
        {
            get
            {
                return _cIndustryID;
            }
            set
            {
                _cIndustryID = value;
                PropertyHasChanged("CIndustryID");
            }
        }

        public string CClass
        {
            get
            {
                return _cClass;
            }
            set
            {
                _cClass = value;
                PropertyHasChanged("CClass");
            }
        }

        public int? CPriceType
        {
            get
            {
                return _cPriceType;
            }
            set
            {
                if (_cPriceType != value)
                {
                    _cPriceType = value;
                    PropertyHasChanged("CPriceType");
                }
            }
        }

        public decimal? COverallDefaultDis
        {
            get
            {
                return _cOverallDefaultDis;
            }
            set
            {
                _cOverallDefaultDis = value;
                PropertyHasChanged("VOverallDefaultDis");
            }
        }

        public int? CTermKey
        {
            get
            {
                return _cTermKey;
            }
            set
            {
                if (_cTermKey != value)
                {
                    _cTermKey = value;
                    PropertyHasChanged("CTermKey");
                }
            }
        }

        public decimal? CCreditLimit
        {
            get
            {
                return _cCreditLimit;
            }
            set
            {
                _cCreditLimit = value;
                PropertyHasChanged("CCreditLimit");
            }
        }

        public int? CTaxGrpKey
        {
            get
            {
                return _cTaxGrpKey;
            }
            set
            {
                if (_cTaxGrpKey != value)
                {
                    _cTaxGrpKey = value;
                    PropertyHasChanged("CTaxGrpKey");
                }
            }
        }

        public int? CEMKey
        {
            get
            {
                return _cEMKey;
            }
            set
            {
                if (_cEMKey != value)
                {
                    _cEMKey = value;
                    PropertyHasChanged("CEMKey");
                }
            }
        }

        public string CEMID
        {
            get
            {
                return _cemid;
            }
            set
            {
                _cemid = value;
                PropertyHasChanged("CEMID");
            }
        }

        public int? CCurrkey
        {
            get
            {
                return _cCurrkey;
            }
            set
            {
                if (_cCurrkey != value)
                {
                    _cCurrkey = value;
                    PropertyHasChanged("CCurrkey");
                }
            }
        }

        public string CCurrID
        {
            get
            {
                return _cCurrID;
            }
            set
            {
                _cCurrID = value;
                PropertyHasChanged("CCurrID");
            }
        }

        public int? CAccKey
        {
            get
            {
                return _cAccKey;
            }
            set
            {
                if (_cAccKey != value)
                {
                    _cAccKey = value;
                    PropertyHasChanged("CAccKey");
                }
            }
        }

        public string CDefaultBillAddr
        {
            get
            {
                return _cDefaultBillAddr;
            }
            set
            {
                if (_cDefaultBillAddr != value)
                {
                    _cDefaultBillAddr = value;
                    PropertyHasChanged("CDefaultBillAddr");
                }
            }
        }

        public string CDefaultShipAddr
        {
            get
            {
                return _cDefaultShipAddr;
            }
            set
            {
                if (_cDefaultShipAddr != value)
                {
                    _cDefaultShipAddr = value;
                    PropertyHasChanged("CDefaultShipAddr");
                }
            }
        }

        public string CDefaultStateAddr
        {
            get
            {
                return _cDefaultStateAddr;
            }
            set
            {
                if (_cDefaultStateAddr != value)
                {
                    _cDefaultStateAddr = value;
                    PropertyHasChanged("CDefaultStateAddr");
                }
            }
        }

        public int? CDefaultStateType
        {
            get
            {
                return _cDefaultStateType;
            }
            set
            {
                if (_cDefaultStateType != value)
                {
                    _cDefaultStateType = value;
                    PropertyHasChanged("CDefaultStateType");
                }
            }
        }

        public string CDefaultContact
        {
            get
            {
                return _cDefaultContact;
            }
            set
            {
                _cDefaultContact = value;
                PropertyHasChanged("CDefaultContact");
            }
        }

        public string CDefaultContactState
        {
            get
            {
                return _cDefaultContactState;
            }
            set
            {
                _cDefaultContactState = value;
                PropertyHasChanged("CDefaultContactState");
            }
        }

        public string CRemDelivery
        {
            get
            {
                return _cRemDelivery;
            }
            set
            {
                _cRemDelivery = value;
                PropertyHasChanged("CRemDelivery");
            }
        }

        public string CRemPrice
        {
            get
            {
                return _cRemPrice;
            }
            set
            {
                _cRemPrice = value;
                PropertyHasChanged("CRemPrice");
            }
        }

        public string CRemValidity
        {
            get
            {
                return _cRemValidity;
            }
            set
            {
                _cRemValidity = value;
                PropertyHasChanged("CRemValidity");
            }
        }

        public string CRemPayment
        {
            get
            {
                return _cRemPayment;
            }
            set
            {
                _cRemPayment = value;
                PropertyHasChanged("CRemPayment");
            }
        }

        public string CRem
        {
            get
            {
                return _cRem;
            }
            set
            {
                _cRem = value;
                PropertyHasChanged("CRem");
            }
        }

        public string FormerKnownAs
        {
            get
            {
                return _formerknownas;
            }
            set
            {
                _formerknownas = value;
                PropertyHasChanged("FormerKnownAs");
            }
        }

        public bool? CAttachment
        {
            get
            {
                return _cAttachment;
            }
            set
            {
                _cAttachment = value;
                PropertyHasChanged("CAttachment");
            }
        }

        public DateTime? CustomerSinceDate
        {
            get
            {
                return _customerSinceDate;
            }
            set
            {
                _customerSinceDate = value;
                PropertyHasChanged("CustomerSinceDate");
            }
        }

        public decimal? CCreditBal
        {
            get
            {
                return _cCreditBal;
            }
            set
            {
                _cCreditBal = value;
                PropertyHasChanged("CCreditBal");
            }
        }

        public decimal? CCashBal
        {
            get
            {
                return _cCashBal;
            }
            set
            {
                _cCashBal = value;
                PropertyHasChanged("CCashBal");
            }
        }

        public decimal? CNTBal
        {
            get
            {
                return _cNTBal;
            }
            set
            {
                _cNTBal = value;
                PropertyHasChanged("CNTBal");
            }
        }
        public int? VBranchKey
        {
            get
            {
                return _vBranchKey;
            }
            set
            {
                if (_vBranchKey != value)
                {
                    _vBranchKey = value;
                    PropertyHasChanged("VBranchKey");
                }
            }
        }

        public string VBranchID
        {
            get
            {
                return _vBranchID;
            }
            set
            {
                _vBranchID = value;
                PropertyHasChanged("VBranchID");
            }
        }

        public int? VDeptKey
        {
            get
            {
                return _vDeptKey;
            }
            set
            {
                if (_vDeptKey != value)
                {
                    _vDeptKey = value;
                    PropertyHasChanged("VDeptKey");
                }
            }
        }

        public string VDeptID
        {
            get
            {
                return _vDeptID;
            }
            set
            {
                _vDeptID = value;
                PropertyHasChanged("VDeptID");
            }
        }

        public int? VGrpKey
        {
            get
            {
                return _vGrpKey;
            }
            set
            {
                if (_vGrpKey != value)
                {
                    _vGrpKey = value;
                    PropertyHasChanged("VGrpKey");
                }
            }
        }

        public string VGrpID
        {
            get
            {
                return _vGrpID;
            }
            set
            {
                _vGrpID = value;
                PropertyHasChanged("VGrpID");
            }
        }

        public int? VTerritoryKey
        {
            get
            {
                return _vTerritoryKey;
            }
            set
            {
                if (_vTerritoryKey != value)
                {
                    _vTerritoryKey = value;
                    PropertyHasChanged("VTerritoryKey");
                }
            }
        }

        public string VTerritoryID
        {
            get
            {
                return _vTerritoryID;
            }
            set
            {
                _vTerritoryID = value;
                PropertyHasChanged("VTerritoryID");
            }
        }

        public int? VIndustryKey
        {
            get
            {
                return _vIndustryKey;
            }
            set
            {
                if (_vIndustryKey != value)
                {
                    _vIndustryKey = value;
                    PropertyHasChanged("VIndustryKey");
                }
            }
        }

        public string VIndustryID
        {
            get
            {
                return _vIndustryID;
            }
            set
            {
                _vIndustryID = value;
                PropertyHasChanged("VIndustryID");
            }
        }

        public string VClass
        {
            get
            {
                return _vClass;
            }
            set
            {
                _vClass = value;
                PropertyHasChanged("VClass");
            }
        }

        public int? VPriceType
        {
            get
            {
                return _vPriceType;
            }
            set
            {
                if (_vPriceType != value)
                {
                    _vPriceType = value;
                    PropertyHasChanged("VPriceType");
                }
            }
        }

        public decimal? VOverallDefaultDis
        {
            get
            {
                return _vOverallDefaultDis;
            }
            set
            {
                _vOverallDefaultDis = value;
                PropertyHasChanged("VOverallDefaultDis");
            }
        }

        public int? VTermKey
        {
            get
            {
                return _vTermKey;
            }
            set
            {
                if (_vTermKey != value)
                {
                    _vTermKey = value;
                    PropertyHasChanged("VTermKey");
                }
            }
        }

        public decimal? VCreditLimit
        {
            get
            {
                return _vCreditLimit;
            }
            set
            {
                _vCreditLimit = value;
                PropertyHasChanged("VCreditLimit");
            }
        }

        public int? VTaxGrpKey
        {
            get
            {
                return _vTaxGrpKey;
            }
            set
            {
                if (_vTaxGrpKey != value)
                {
                    _vTaxGrpKey = value;
                    PropertyHasChanged("VTaxGrpKey");
                }
            }
        }

        public int? VEMKey
        {
            get
            {
                return _vEMKey;
            }
            set
            {
                if (_vEMKey != value)
                {
                    _vEMKey = value;
                    PropertyHasChanged("VEMKey");
                }
            }
        }

        public string VEMID
        {
            get
            {
                return _vemid;
            }
            set
            {
                _vemid = value;
                PropertyHasChanged("VEMID");
            }
        }

        public int? VCurrkey
        {
            get
            {
                return _vCurrkey;
            }
            set
            {
                if (_vCurrkey != value)
                {
                    _vCurrkey = value;
                    PropertyHasChanged("VCurrkey");
                }
            }
        }

        public string VCurrID
        {
            get
            {
                return _vCurrID;
            }
            set
            {
                _vCurrID = value;
                PropertyHasChanged("VCurrID");
            }
        }

        public int? VAccKey
        {
            get
            {
                return _vAccKey;
            }
            set
            {
                if (_vAccKey != value)
                {
                    _vAccKey = value;
                    PropertyHasChanged("VAccKey");
                }
            }
        }

        public string VDefaultBillAddr
        {
            get
            {
                return _vDefaultBillAddr;
            }
            set
            {
                if (_vDefaultBillAddr != value)
                {
                    _vDefaultBillAddr = value;
                    PropertyHasChanged("VDefaultBillAddr");
                }
            }
        }

        public string VDefaultShipAddr
        {
            get
            {
                return _vDefaultShipAddr;
            }
            set
            {
                if (_vDefaultShipAddr != value)
                {
                    _vDefaultShipAddr = value;
                    PropertyHasChanged("VDefaultShipAddr");
                }
            }
        }

        public string VDefaultContact
        {
            get
            {
                return _vDefaultContact;
            }
            set
            {
                _vDefaultContact = value;
                PropertyHasChanged("VDefaultContact");
            }
        }

        public string VDefaultAPPYDocType
        {
            get
            {
                return _vDefaultAPPYDocType;
            }
            set
            {
                _vDefaultAPPYDocType = value;
                PropertyHasChanged("VDefaultAPPYDocType");
            }
        }

        public string VRemDelivery
        {
            get
            {
                return _vRemDelivery;
            }
            set
            {
                _vRemDelivery = value;
                PropertyHasChanged("VRemDelivery");
            }
        }

        public string VRemPrice
        {
            get
            {
                return _vRemPrice;
            }
            set
            {
                _vRemPrice = value;
                PropertyHasChanged("VRemPrice");
            }
        }

        public string VRemValidity
        {
            get
            {
                return _vRemValidity;
            }
            set
            {
                _vRemValidity = value;
                PropertyHasChanged("VRemValidity");
            }
        }

        public string VRemPayment
        {
            get
            {
                return _vRemPayment;
            }
            set
            {
                _vRemPayment = value;
                PropertyHasChanged("VRemPayment");
            }
        }

        public string VRem
        {
            get
            {
                return _vRem;
            }
            set
            {
                _vRem = value;
                PropertyHasChanged("VRem");
            }
        }

        public bool? VAttachment
        {
            get
            {
                return _vAttachment;
            }
            set
            {
                _vAttachment = value;
                PropertyHasChanged("VAttachment");
            }
        }

        public DateTime? VendorSinceDate
        {
            get
            {
                return _vendorSinceDate;
            }
            set
            {
                _vendorSinceDate = value;
                PropertyHasChanged("VendorSinceDate");
            }
        }

        public decimal? VBal
        {
            get
            {
                return _vBal;
            }
            set
            {
                _vBal = value;
                PropertyHasChanged("VBal");
            }
        }

        public string ConNamFirst
        {
            get
            {
                return _conNamFirst;
            }
            set
            {
                _conNamFirst = value;
                PropertyHasChanged("ConNamFirst");
            }
        }

        public string ConNamLast
        {
            get
            {
                return _conNamLast;
            }
            set
            {
                _conNamLast = value;
                PropertyHasChanged("ConNamLast");
            }
        }

        public string ConNamMiddle
        {
            get
            {
                return _conNamMiddle;
            }
            set
            {
                _conNamMiddle = value;
                PropertyHasChanged("ConNamMiddle");
            }
        }

        public string ConNamInitials
        {
            get
            {
                return _conNamInitials;
            }
            set
            {
                _conNamInitials = value;
                PropertyHasChanged("ConNamInitials");
            }
        }

        public DateTime? ConBirthday
        {
            get
            {
                return _conBirthday;
            }
            set
            {
                _conBirthday = value;
                PropertyHasChanged("ConBirthday");
            }
        }

        public int? ConGender
        {
            get
            {
                return _conGender;
            }
            set
            {
                _conGender = value;
                PropertyHasChanged("ConGender");
            }
        }

        public int? ConMarital
        {
            get
            {
                return _conMarital;
            }
            set
            {
                _conMarital = value;
                PropertyHasChanged("ConMarital");
            }
        }

        public short? ConChildren
        {
            get
            {
                return _conChildren;
            }
            set
            {
                _conChildren = value;
                PropertyHasChanged("ConChildren");
            }
        }

        public string ConSocSecNo
        {
            get
            {
                return _conSocSecNo;
            }
            set
            {
                _conSocSecNo = value;
                PropertyHasChanged("ConSocSecNo");
            }
        }

        public string ConNationality
        {
            get
            {
                return _conNationality;
            }
            set
            {
                if (_conNationality != value)
                {
                    _conNationality = value;
                    PropertyHasChanged("ConNationality");
                }
            }
        }

        public string OccuTitle
        {
            get
            {
                return _occuTitle;
            }
            set
            {
                _occuTitle = value;
                PropertyHasChanged("OccuTitle");
            }
        }

        public string OccuIndustry
        {
            get
            {
                return _occuIndustry;
            }
            set
            {
                _occuIndustry = value;
                PropertyHasChanged("OccuIndustry");
            }
        }

        public string OccuSalary
        {
            get
            {
                return _occuSalary;
            }
            set
            {
                _occuSalary = value;
                PropertyHasChanged("OccuSalary");
            }
        }

        public string OccuGroup
        {
            get
            {
                return _occuGroup;
            }
            set
            {
                _occuGroup = value;
                PropertyHasChanged("OccuGroup");
            }
        }

        public bool EmailStatement
        {
            get
            {
                return _emailStatement;
            }
            set
            {
                _emailStatement = value;
                PropertyHasChanged("EmailStatement");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;
                PropertyHasChanged("CreateDate");
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return _createUserKey;
            }
            set
            {
                _createUserKey = value;
                PropertyHasChanged("CreateUserKey");
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }
            set
            {
                _lastModifiedDate = value;
                PropertyHasChanged("LastModifiedDate");
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return _lastModifiedUserKey;
            }
            set
            {
                _lastModifiedUserKey = value;
                PropertyHasChanged("LastModifiedUserKey");
            }
        }

        public string Custom1
        {
            get
            {
                return _custom1;
            }
            set
            {
                _custom1 = value;
                PropertyHasChanged("Custom1");
            }
        }

        public string Custom2
        {
            get
            {
                return _custom2;
            }
            set
            {
                _custom2 = value;
                PropertyHasChanged("Custom2");
            }
        }

        public string Custom3
        {
            get
            {
                return _custom3;
            }
            set
            {
                _custom3 = value;
                PropertyHasChanged("Custom3");
            }
        }

        public string Custom4
        {
            get
            {
                return _custom4;
            }
            set
            {
                _custom4 = value;
                PropertyHasChanged("Custom4");
            }
        }

        public string Custom5
        {
            get
            {
                return _custom5;
            }
            set
            {
                _custom5 = value;
                PropertyHasChanged("Custom5");
            }
        }

        public SYSAttachments Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        protected override object GetIdValue()
        {
            return _conKey.ToString();
        }

        public bool SalesRepIsHeadSales
        {
            get { return _salesRepIsHeadSales; }
            set { _salesRepIsHeadSales=value; }
        }
       #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            /*
           //
           // ConID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ConID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConID", 50));
           //
           // ConNm
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ConNm");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConNm", 255));
           //
           // CBranchID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CBranchID", 50));
           //
           // CDeptID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CDeptID", 50));
           //
           // CGrpID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CGrpID", 50));
           //
           // CTerritoryID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CTerritoryID", 50));
           //
           // CIndustryID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CIndustryID", 50));
           //
           // CClass
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CClass", 50));
           //
           // Cemid
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Cemid", 50));
           //
           // CCurrID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CCurrID", 50));
           //
           // CDefaultBillAddr
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CDefaultBillAddr", 50));
           //
           // CDefaultShipAddr
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CDefaultShipAddr", 50));
           //
           // CDefaultStateAddr
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CDefaultStateAddr", 50));
           //
           // CDefaultContact
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CDefaultContact", 255));
           //
           // CDefaultContactState
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CDefaultContactState", 255));
           //
           // CRemDelivery
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CRemDelivery", 255));
           //
           // CRemPrice
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CRemPrice", 255));
           //
           // CRemValidity
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CRemValidity", 255));
           //
           // CRemPayment
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CRemPayment", 255));
           //
           // VBranchID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VBranchID", 50));
           //
           // VDeptID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VDeptID", 50));
           //
           // VGrpID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VGrpID", 50));
           //
           // VTerritoryID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VTerritoryID", 50));
           //
           // VIndustryID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VIndustryID", 50));
           //
           // VClass
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VClass", 50));
           //
           // Vemid
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Vemid", 50));
           //
           // VCurrID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VCurrID", 50));
           //
           // VDefaultBillAddr
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VDefaultBillAddr", 50));
           //
           // VDefaultShipAddr
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VDefaultShipAddr", 50));
           //
           // VDefaultContact
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VDefaultContact", 255));
           //
           // VRemDelivery
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VRemDelivery", 255));
           //
           // VRemPrice
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VRemPrice", 255));
           //
           // VRemValidity
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VRemValidity", 255));
           //
           // VRemPayment
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VRemPayment", 255));
           //
           // VRem
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VRem", 255));
           //
           // ConNamFirst
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConNamFirst", 50));
           //
           // ConNamLast
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConNamLast", 50));
           //
           // ConNamMiddle
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConNamMiddle", 50));
           //
           // ConNamInitials
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConNamInitials", 50));
           //
           // ConSocSecNo
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConSocSecNo", 50));
           //
           // ConNationality
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConNationality", 50));
           //
           // OccuTitle
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OccuTitle", 50));
           //
           // OccuIndustry
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OccuIndustry", 50));
           //
           // OccuSalary
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OccuSalary", 50));
           //
           // OccuGroup
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OccuGroup", 50));
           //
           // Custom1
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
           //
           // Custom2
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
           //
           // Custom3
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
           //
           // Custom4
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
           //
           // Custom5
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
            */
        }

        protected override void AddBusinessRules()
        {
            /*
           AddCommonRules();
           AddCustomRules();
            */
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTCon()
        { /* require use of factory method */ }

        internal static MSTCon New()
        {
            MSTCon child = new MSTCon();   
            return child;
        }

        internal static MSTCon NewChild()
        {
            
            MSTCon child = new MSTCon();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static MSTCon Get(SafeDataReader dr)
        {   
            MSTCon child = new MSTCon();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTCon Get(int? conKey)
        {
            MSTCon child = new MSTCon();
            child.Fetch(new Criteria(conKey, 1));
            return child;
        }

        public static MSTCon Get(string conID)
        {
            MSTCon child = new MSTCon();
            child.Fetch(new Criteria(conID,2));
            return child;
        }

        public static MSTCon Get(SqlConnection cn, int? conKey)
        {
            MSTCon child = new MSTCon();
            child.Fetch(cn, new Criteria(conKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _conKey = null;
            public int? _option = null;
            public string _conID = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? ConKey)
            {
                _conKey = ConKey;
            }
            internal Criteria(string ConID)
            {
                _conID = ConID;
            }
            internal Criteria(string ConID,int? Option)
            {
                _conID = ConID;
                _option = Option;
            }
            internal Criteria(int? ConKey, int? Option)
            {
                _conKey = ConKey;
                _option = Option;
            }
            internal Criteria(int? ConKey, string ConID)
            {
                _conKey = ConKey;
                _conID = ConID;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
          
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria);
            }
         
            
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                if (!GFunc.IsNEZ(criteria._conKey))
                    cm.Parameters.AddWithValue("@ConKey", criteria._conKey);
                else
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);

                if (criteria._conID != string.Empty)
                    cm.Parameters.AddWithValue("@ConID", criteria._conID);
                else
                    cm.Parameters.AddWithValue("@ConID", DBNull.Value);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    else
                        this.Clear();

                }	// Already close and dispose data reader.              
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                    retValue = false;
               

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _conKey = dr.GetInt32("ConKey");
            _conID = dr.GetString("ConID");
            _conNm = dr.GetString("ConNm");
            _conType = dr.GetInt32("ConType");
            _cCBType = dr.GetInt32("CCBType");
            _noFinCharge = dr.GetBoolean("NoFinCharge");
            _accessLevel = dr.GetInt32("AccessLevel");
            _accessGroup = dr.GetInt32("AccessGroup");
            _conUEN = dr.GetString("ConUEN");
            _inactive = dr.GetBoolean("Inactive");
            _approval = dr.GetBoolean("Approval");
            _rejected = dr.GetBoolean("Rejected");
            _activewithproblem = dr.GetBoolean("ActiveWithProblem");
            _cooapprovalrequired = dr.GetBoolean("COOApprovalRequired");
            _cBranchKey = dr.GetInt32("CBranchKey");
            _cBranchID = dr.GetString("CBranchID");
            _cDeptKey = dr.GetInt32("CDeptKey");
            _cDeptID = dr.GetString("CDeptID");
            _cGrpKey = dr.GetInt32("CGrpKey");
            _cGrpID = dr.GetString("CGrpID");
            _cTerritoryKey = GFunc.NEInt(dr.GetValue("CTerritoryKey"),0);
            _cTerritoryID = dr.GetString("CTerritoryID");
            _cIndustryKey = GFunc.NEInt(dr.GetValue("CIndustryKey"),0);
            _cIndustryID = dr.GetString("CIndustryID");
            _cClass = dr.GetString("CClass");
            _cPriceType = GFunc.NEInt(dr.GetValue("CPriceType"),0);
            _cOverallDefaultDis = dr.GetDecimal("COverallDefaultDis");
            _cTermKey = GFunc.NEInt(dr.GetValue("CTermKey"),0);
            _cCreditLimit = dr.GetDecimal("CCreditLimit");
            _cTaxGrpKey = GFunc.NEInt(dr.GetValue("CTaxGrpKey"),0);
            _cEMKey = GFunc.NEInt(dr.GetValue("CEMKey"),0);
            _cemid = dr.GetString("CEMID");
            _cCurrkey = dr.GetInt32("CCurrkey");
            _cCurrID = dr.GetString("CCurrID");
            _cAccKey = GFunc.NEInt(dr.GetValue("CAccKey"),0);
            _cDefaultBillAddr = dr.GetString("CDefaultBillAddr");
            _cDefaultShipAddr = dr.GetString("CDefaultShipAddr");
            _cDefaultStateAddr = dr.GetString("CDefaultStateAddr");
            _cDefaultStateType = dr.GetInt32("CDefaultStateType");
            _cDefaultContact = dr.GetString("CDefaultContact");
            _cDefaultContactState = dr.GetString("CDefaultContactState");
            _cRemDelivery = dr.GetString("CRemDelivery");
            _cRemPrice = dr.GetString("CRemPrice");
            _cRemValidity = dr.GetString("CRemValidity");
            _cRemPayment = dr.GetString("CRemPayment");
            _cRem = dr.GetString("CRem");
            _formerknownas = dr.GetString("FormerKnownAs");
            _cAttachment = dr.GetBoolean("CAttachment");          
            _customerSinceDate = dr["CustomerSinceDate"] == DBNull.Value ? null : (DateTime?)dr["CustomerSinceDate"];
            _cCreditBal = dr.GetDecimal("CCreditBal");
            _cCashBal = dr.GetDecimal("CCashBal");
            if ((dr.GetSchemaTable().Select("ColumnName = 'CNTBal'")).Length == 1)
                _cNTBal = dr.GetDecimal("CNTBal");
            else
                _cNTBal = 0;
            _vBranchKey = dr.GetInt32("VBranchKey");
            _vBranchID = dr.GetString("VBranchID");
            _vDeptKey = dr.GetInt32("VDeptKey");
            _vDeptID = dr.GetString("VDeptID");
            _vGrpKey = dr.GetInt32("VGrpKey");
            _vGrpID = dr.GetString("VGrpID");
            _vTerritoryKey = GFunc.NEInt(dr.GetValue("VTerritoryKey"),0);
            _vTerritoryID = dr.GetString("VTerritoryID");
            _vIndustryKey = GFunc.NEInt(dr.GetValue("VIndustryKey"),0);
            _vIndustryID = dr.GetString("VIndustryID");
            _vClass = dr.GetString("VClass");
            _vPriceType = GFunc.NEInt(dr.GetValue("VPriceType"),0);
            _vOverallDefaultDis = dr.GetDecimal("VOverallDefaultDis");
            _vTermKey = GFunc.NEInt(dr.GetValue("VTermKey"),0);
            _vCreditLimit = dr.GetDecimal("VCreditLimit");
            _vTaxGrpKey = GFunc.NEInt(dr.GetValue("VTaxGrpKey"),0);
            _vEMKey = GFunc.NEInt(dr.GetValue("VEMKey"),0);
            _vemid = dr.GetString("VEMID");
            _vCurrkey = dr.GetInt32("VCurrkey");
            _vCurrID = dr.GetString("VCurrID");
            _vAccKey = GFunc.NEInt(dr.GetValue("VAccKey"), 0);
            _vDefaultBillAddr = dr.GetString("VDefaultBillAddr");
            _vDefaultShipAddr = dr.GetString("VDefaultShipAddr");
            _vDefaultContact = dr.GetString("VDefaultContact");
            _vDefaultAPPYDocType = dr.GetString("VDefaultAPPYDocType");
            _vRemDelivery = dr.GetString("VRemDelivery");
            _vRemPrice = dr.GetString("VRemPrice");
            _vRemValidity = dr.GetString("VRemValidity");
            _vRemPayment = dr.GetString("VRemPayment");
            _vRem = dr.GetString("VRem");
            _vAttachment = dr.GetBoolean("VAttachment");          
            _vendorSinceDate = dr["VendorSinceDate"] == DBNull.Value ? null : (DateTime?)dr["VendorSinceDate"];
            _vBal = dr.GetDecimal("VBal");
            _conNamFirst = dr.GetString("ConNamFirst");
            _conNamLast = dr.GetString("ConNamLast");
            _conNamMiddle = dr.GetString("ConNamMiddle");
            _conNamInitials = dr.GetString("ConNamInitials");
            _conBirthday = dr["ConBirthday"] == DBNull.Value ? null : (DateTime?)dr["ConBirthday"];
            _conGender = GFunc.NEInt(dr.GetValue("ConGender"),0);
            _conMarital = GFunc.NEInt(dr.GetValue("ConMarital"),0);
            _conChildren = dr.GetInt16("ConChildren");
            _conSocSecNo = dr.GetString("ConSocSecNo");
            _conNationality = dr.GetString("ConNationality");
            _occuTitle = dr.GetString("OccuTitle");
            _occuIndustry = dr.GetString("OccuIndustry");
            _occuSalary = dr.GetString("OccuSalary");
            _occuGroup = dr.GetString("OccuGroup");
            _emailStatement = dr.GetBoolean("EmailStatement");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _custom4 = dr.GetString("Custom4");
            _custom5 = dr.GetString("Custom5");
            //Added by nnt on 27 March 2019           


            //added by thettm on 02 April 2018(start)
            _accountName = dr.GetString("AccountName");
            _bankName = dr.GetString("BankName");
            _branchName = dr.GetString("BranchName");
            _country = dr.GetString("Country");
            _bankCode = dr.GetString("BankCode");
            _branchCode = dr.GetString("BranchCode");
            _sWIFTCode = dr.GetString("SWIFTCode");
            _bankCurrKey = dr.GetInt32("BankCurrKey");
            _bankCurrID = dr.GetString("BankCurrID");
            _bankAccountNo = dr.GetString("BankAccountNo");
            _iBAN_NO = dr.GetString("IBAN_NO");
            _iNTBankName = dr.GetString("INTBankName");
            _iNTBankCountry = dr.GetString("INTBankCountry");
            _iNTBankCode = dr.GetString("INTBankCode");
            _iNTBranchCode = dr.GetString("INTBranchCode");
            _iNTSWIFTCode = dr.GetString("INTSWIFTCode");
            _iNTCurrKey = dr.GetInt32("INTCurrKey");
            _iNTCurrID = dr.GetString("INTCurrID");
            _iNTBankAccountNo = dr.GetString("INTBankAccountNo");
            _bankAddress = dr.GetString("BankAddress");
            _deliModeCode = dr.GetString("DeliModeCode");
            _deliModeCodeValue = dr.GetString("DeliModeCodeValue");
            _salesRepIsHeadSales = dr.GetBoolean("SalesRepIsHeadSales");
            //added by thettm on 02 April 2018(end)

            // this.MarkOld();   disable not to reset the value        

            return true;
            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? conKey)
        {
            bool retValue = false;
            conKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out conKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? conKey)
        {
            conKey = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);                   

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewConKey", conKey);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_conID == null)
                    cm.Parameters.AddWithValue("@ConID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConID", _conID);

                if (_conNm == null)
                    cm.Parameters.AddWithValue("@ConNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNm", _conNm);

                if (_conType == null)
                    cm.Parameters.AddWithValue("@ConType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConType", _conType);

                if (_cCBType == null)
                    cm.Parameters.AddWithValue("@CCBType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCBType", _cCBType);

                if (_noFinCharge == null)
                    cm.Parameters.AddWithValue("@NoFinCharge", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@NoFinCharge", _noFinCharge);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                if (_conUEN == null)
                    cm.Parameters.AddWithValue("@ConUEN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConUEN", _conUEN);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);
                //added by nnt on April 2019
                if (_approval == null)
                    cm.Parameters.AddWithValue("@Approval", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Approval", _approval);

                if (_rejected == null)
                    cm.Parameters.AddWithValue("@Rejected", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Rejected", _rejected);

                if (_activewithproblem == null)
                    cm.Parameters.AddWithValue("@ActiveWithProblem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ActiveWithProblem", _activewithproblem);

                if (_cooapprovalrequired == null)
                    cm.Parameters.AddWithValue("@COOApprovalRequired", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@COOApprovalRequired", _cooapprovalrequired);

                if (_cBranchKey == null)
                    cm.Parameters.AddWithValue("@CBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CBranchKey", _cBranchKey);

                if (_cBranchID == null)
                    cm.Parameters.AddWithValue("@CBranchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CBranchID", _cBranchID);

                if (_cDeptKey == null)
                    cm.Parameters.AddWithValue("@CDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDeptKey", _cDeptKey);

                if (_cDeptID == null)
                    cm.Parameters.AddWithValue("@CDeptID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDeptID", _cDeptID);

                if (_cGrpKey == null)
                    cm.Parameters.AddWithValue("@CGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CGrpKey", _cGrpKey);

                if (_cGrpID == null)
                    cm.Parameters.AddWithValue("@CGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CGrpID", _cGrpID);

                if (_cTerritoryKey == null)
                    cm.Parameters.AddWithValue("@CTerritoryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTerritoryKey", _cTerritoryKey);

                if (_cTerritoryID == null)
                    cm.Parameters.AddWithValue("@CTerritoryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTerritoryID", _cTerritoryID);

                if (_cIndustryKey == null)
                    cm.Parameters.AddWithValue("@CIndustryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CIndustryKey", _cIndustryKey);

                if (_cIndustryID == null)
                    cm.Parameters.AddWithValue("@CIndustryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CIndustryID", _cIndustryID);

                if (_cClass == null)
                    cm.Parameters.AddWithValue("@CClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CClass", _cClass);

                if (_cPriceType == null)
                    cm.Parameters.AddWithValue("@CPriceType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CPriceType", _cPriceType);

                if (_cOverallDefaultDis == null)
                    cm.Parameters.AddWithValue("@COverallDefaultDis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@COverallDefaultDis", _cOverallDefaultDis);

                if (_cTermKey == null)
                    cm.Parameters.AddWithValue("@CTermKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTermKey", _cTermKey);

                if (_cCreditLimit == null)
                    cm.Parameters.AddWithValue("@CCreditLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCreditLimit", _cCreditLimit);

                if (_cTaxGrpKey == null)
                    cm.Parameters.AddWithValue("@CTaxGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTaxGrpKey", _cTaxGrpKey);

                if (_cEMKey == null)
                    cm.Parameters.AddWithValue("@CEMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CEMKey", _cEMKey);

                if (_cemid == null)
                    cm.Parameters.AddWithValue("@Cemid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cemid", _cemid);

                if (_cCurrkey == null)
                    cm.Parameters.AddWithValue("@CCurrkey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCurrkey", _cCurrkey);

                if (_cCurrID == null)
                    cm.Parameters.AddWithValue("@CCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCurrID", _cCurrID);

                if (_cAccKey == null)
                    cm.Parameters.AddWithValue("@CAccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CAccKey", _cAccKey);

                if (_cDefaultBillAddr == null)
                    cm.Parameters.AddWithValue("@CDefaultBillAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultBillAddr", _cDefaultBillAddr);

                if (_cDefaultShipAddr == null)
                    cm.Parameters.AddWithValue("@CDefaultShipAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultShipAddr", _cDefaultShipAddr);

                if (_cDefaultStateAddr == null)
                    cm.Parameters.AddWithValue("@CDefaultStateAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultStateAddr", _cDefaultStateAddr);

                if (_cDefaultStateType == null)
                    cm.Parameters.AddWithValue("@CDefaultStateType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultStateType", _cDefaultStateType);

                if (_cDefaultContact == null)
                    cm.Parameters.AddWithValue("@CDefaultContact", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultContact", _cDefaultContact);

                if (_cDefaultContactState == null)
                    cm.Parameters.AddWithValue("@CDefaultContactState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultContactState", _cDefaultContactState);

                if (_cRemDelivery == null)
                    cm.Parameters.AddWithValue("@CRemDelivery", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemDelivery", _cRemDelivery);

                if (_cRemPrice == null)
                    cm.Parameters.AddWithValue("@CRemPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemPrice", _cRemPrice);

                if (_cRemValidity == null)
                    cm.Parameters.AddWithValue("@CRemValidity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemValidity", _cRemValidity);

                if (_cRemPayment == null)
                    cm.Parameters.AddWithValue("@CRemPayment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemPayment", _cRemPayment);

                if (_cRem == null)
                    cm.Parameters.AddWithValue("@CRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRem", _cRem);

                if (_formerknownas == null)
                    cm.Parameters.AddWithValue("@FormerKnownAs", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FormerKnownAs", _formerknownas);

                if (_cAttachment == null)
                    cm.Parameters.AddWithValue("@CAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CAttachment", _cAttachment);

                if (_customerSinceDate == null)
                    cm.Parameters.AddWithValue("@CustomerSinceDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustomerSinceDate", _customerSinceDate.Value);

                if (_cCreditBal == null)
                    cm.Parameters.AddWithValue("@CCreditBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCreditBal", _cCreditBal);

                if (_cCashBal == null)
                    cm.Parameters.AddWithValue("@CCashBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCashBal", _cCashBal);

                if (_vBranchKey == null)
                    cm.Parameters.AddWithValue("@VBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VBranchKey", _vBranchKey);

                if (_vBranchID == null)
                    cm.Parameters.AddWithValue("@VBranchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VBranchID", _vBranchID);

                if (_vDeptKey == null)
                    cm.Parameters.AddWithValue("@VDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDeptKey", _vDeptKey);

                if (_vDeptID == null)
                    cm.Parameters.AddWithValue("@VDeptID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDeptID", _vDeptID);

                if (_vGrpKey == null)
                    cm.Parameters.AddWithValue("@VGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VGrpKey", _vGrpKey);

                if (_vGrpID == null)
                    cm.Parameters.AddWithValue("@VGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VGrpID", _vGrpID);

                if (_vTerritoryKey == null)
                    cm.Parameters.AddWithValue("@VTerritoryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTerritoryKey", _vTerritoryKey);

                if (_vTerritoryID == null)
                    cm.Parameters.AddWithValue("@VTerritoryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTerritoryID", _vTerritoryID);

                if (_vIndustryKey == null)
                    cm.Parameters.AddWithValue("@VIndustryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VIndustryKey", _vIndustryKey);

                if (_vIndustryID == null)
                    cm.Parameters.AddWithValue("@VIndustryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VIndustryID", _vIndustryID);

                if (_vClass == null)
                    cm.Parameters.AddWithValue("@VClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VClass", _vClass);

                if (_vPriceType == null)
                    cm.Parameters.AddWithValue("@VPriceType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VPriceType", _vPriceType);

                if (_vOverallDefaultDis == null)
                    cm.Parameters.AddWithValue("@VOverallDefaultDis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VOverallDefaultDis", _vOverallDefaultDis);

                if (_vTermKey == null)
                    cm.Parameters.AddWithValue("@VTermKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTermKey", _vTermKey);

                if (_vCreditLimit == null)
                    cm.Parameters.AddWithValue("@VCreditLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VCreditLimit", _vCreditLimit);

                if (_vTaxGrpKey == null)
                    cm.Parameters.AddWithValue("@VTaxGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTaxGrpKey", _vTaxGrpKey);

                if (_vEMKey == null)
                    cm.Parameters.AddWithValue("@VEMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VEMKey", _vEMKey);

                if (_vemid == null)
                    cm.Parameters.AddWithValue("@Vemid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Vemid", _vemid);

                if (_vCurrkey == null)
                    cm.Parameters.AddWithValue("@VCurrkey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VCurrkey", _vCurrkey);

                if (_vCurrID == null)
                    cm.Parameters.AddWithValue("@VCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VCurrID", _vCurrID);

                if (_vAccKey == null)
                    cm.Parameters.AddWithValue("@VAccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VAccKey", _vAccKey);

                if (_vDefaultBillAddr == null)
                    cm.Parameters.AddWithValue("@VDefaultBillAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultBillAddr", _vDefaultBillAddr);

                if (_vDefaultShipAddr == null)
                    cm.Parameters.AddWithValue("@VDefaultShipAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultShipAddr", _vDefaultShipAddr);

                if (_vDefaultContact == null)
                    cm.Parameters.AddWithValue("@VDefaultContact", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultContact", _vDefaultContact);

                if (_vDefaultAPPYDocType == null)
                    cm.Parameters.AddWithValue("@VDefaultAPPYDocType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultAPPYDocType", _vDefaultAPPYDocType);

                if (_vRemDelivery == null)
                    cm.Parameters.AddWithValue("@VRemDelivery", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemDelivery", _vRemDelivery);

                if (_vRemPrice == null)
                    cm.Parameters.AddWithValue("@VRemPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemPrice", _vRemPrice);

                if (_vRemValidity == null)
                    cm.Parameters.AddWithValue("@VRemValidity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemValidity", _vRemValidity);

                if (_vRemPayment == null)
                    cm.Parameters.AddWithValue("@VRemPayment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemPayment", _vRemPayment);

                if (_vRem == null)
                    cm.Parameters.AddWithValue("@VRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRem", _vRem);

                if (_vAttachment == null)
                    cm.Parameters.AddWithValue("@VAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VAttachment", _vAttachment);

                if (_vendorSinceDate == null)
                    cm.Parameters.AddWithValue("@VendorSinceDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VendorSinceDate", _vendorSinceDate.Value);

                if (_vBal == null)
                    cm.Parameters.AddWithValue("@VBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VBal", _vBal);

                if (_conNamFirst == null)
                    cm.Parameters.AddWithValue("@ConNamFirst", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamFirst", _conNamFirst);

                if (_conNamLast == null)
                    cm.Parameters.AddWithValue("@ConNamLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamLast", _conNamLast);

                if (_conNamMiddle == null)
                    cm.Parameters.AddWithValue("@ConNamMiddle", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamMiddle", _conNamMiddle);

                if (_conNamInitials == null)
                    cm.Parameters.AddWithValue("@ConNamInitials", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamInitials", _conNamInitials);

                if (_conBirthday == null)
                    cm.Parameters.AddWithValue("@ConBirthday", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConBirthday", _conBirthday.Value);

                if (_conGender == null)
                    cm.Parameters.AddWithValue("@ConGender", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConGender", _conGender);

                if (_conMarital == null)
                    cm.Parameters.AddWithValue("@ConMarital", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConMarital", _conMarital);

                if (_conChildren == null)
                    cm.Parameters.AddWithValue("@ConChildren", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConChildren", _conChildren);

                if (_conSocSecNo == null)
                    cm.Parameters.AddWithValue("@ConSocSecNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConSocSecNo", _conSocSecNo);

                if (_conNationality == null)
                    cm.Parameters.AddWithValue("@ConNationality", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNationality", _conNationality);

                if (_occuTitle == null)
                    cm.Parameters.AddWithValue("@OccuTitle", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuTitle", _occuTitle);

                if (_occuIndustry == null)
                    cm.Parameters.AddWithValue("@OccuIndustry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuIndustry", _occuIndustry);

                if (_occuSalary == null)
                    cm.Parameters.AddWithValue("@OccuSalary", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuSalary", _occuSalary);

                if (_occuGroup == null)
                    cm.Parameters.AddWithValue("@OccuGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuGroup", _occuGroup);
              
                if (_emailStatement == null)
                    cm.Parameters.AddWithValue("@EmailStatement", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmailStatement", _emailStatement);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                     cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (_lastModifiedUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

                if (_custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _custom1);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _custom2);

                if (_custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _custom3);

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                //added by thettm on 02 April 2018(start)
                if (_accountName == null)
                    cm.Parameters.AddWithValue("@AccountName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccountName", _accountName);

                if (_bankName == null)
                    cm.Parameters.AddWithValue("@BankName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankName", _bankName);

                if (_branchName == null)
                    cm.Parameters.AddWithValue("@BranchName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchName", _branchName);

                if (_country == null)
                    cm.Parameters.AddWithValue("@Country", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Country", _country);

                if (_bankCode == null)
                    cm.Parameters.AddWithValue("@BankCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankCode", _bankCode);

                if (_branchCode == null)
                    cm.Parameters.AddWithValue("@BranchCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchCode", _branchCode);

                if (_sWIFTCode == null)
                    cm.Parameters.AddWithValue("@SWIFTCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SWIFTCode", _sWIFTCode);

                if (_bankCurrKey == null)
                    cm.Parameters.AddWithValue("@BankCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankCurrKey", _bankCurrKey);

                if (_bankCurrID == null)
                    cm.Parameters.AddWithValue("@BankCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankCurrID", _bankCurrID);

                if (_bankAccountNo == null)
                    cm.Parameters.AddWithValue("@BankAccountNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankAccountNo", _bankAccountNo);

                if (_iBAN_NO == null)
                    cm.Parameters.AddWithValue("@IBAN_NO", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IBAN_NO", _iBAN_NO);

                if (_iNTBankName == null)
                    cm.Parameters.AddWithValue("@INTBankName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankName", _iNTBankName);

                if (_iNTBankCountry == null)
                    cm.Parameters.AddWithValue("@INTBankCountry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankCountry", _iNTBankCountry);

                if (_iNTBankCode == null)
                    cm.Parameters.AddWithValue("@INTBankCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankCode", _iNTBankCode);

                if (_iNTBranchCode == null)
                    cm.Parameters.AddWithValue("@INTBranchCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBranchCode", _iNTBranchCode);

                if (_iNTSWIFTCode == null)
                    cm.Parameters.AddWithValue("@INTSWIFTCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTSWIFTCode", _iNTSWIFTCode);

                if (_iNTCurrKey == null)
                    cm.Parameters.AddWithValue("@INTCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTCurrKey", _iNTCurrKey);

                if (_iNTCurrID == null)
                    cm.Parameters.AddWithValue("@INTCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTCurrID", _iNTCurrID);

                if (_iNTBankAccountNo == null)
                    cm.Parameters.AddWithValue("@INTBankAccountNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankAccountNo", _iNTBankAccountNo);

                if (_bankAddress == null)
                    cm.Parameters.AddWithValue("@BankAddress", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankAddress", _bankAddress);

                if (_deliModeCode == null)
                    cm.Parameters.AddWithValue("@DeliModeCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeliModeCode", _deliModeCode);

                if (_deliModeCodeValue == null)
                    cm.Parameters.AddWithValue("@DeliModeCodeValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeliModeCodeValue", _deliModeCodeValue);
                
                cm.Parameters.AddWithValue("@SalesRepIsHeadSales", _salesRepIsHeadSales);

                //added by thettm on 02 April 2018(end)

                cm.Parameters["@NewConKey"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();            

                conKey = (int)cm.Parameters["@NewConKey"].Value;
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
               

            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue = this.Update(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
             

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewConKey", 0);

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_conID == null)
                    cm.Parameters.AddWithValue("@ConID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConID", _conID);

                if (_conNm == null)
                    cm.Parameters.AddWithValue("@ConNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNm", _conNm);

                if (_conType == null)
                    cm.Parameters.AddWithValue("@ConType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConType", _conType);

                if (_cCBType == null)
                    cm.Parameters.AddWithValue("@CCBType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCBType", _cCBType);

                if (_noFinCharge == null)
                    cm.Parameters.AddWithValue("@NoFinCharge", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@NoFinCharge", _noFinCharge);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);
                if (_conUEN == null)
                    cm.Parameters.AddWithValue("@ConUEN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConUEN", _conUEN);
                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

                //added by nnt on April 2019
                if (_approval == null)
                    cm.Parameters.AddWithValue("@Approval", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Approval", _approval);
                if (_rejected == null)
                    cm.Parameters.AddWithValue("@Rejected", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Rejected", _rejected);
                //end

                if (_activewithproblem == null)
                    cm.Parameters.AddWithValue("@ActiveWithProblem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ActiveWithProblem", _activewithproblem);

                if (_cooapprovalrequired == null)
                    cm.Parameters.AddWithValue("@COOApprovalRequired", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@COOApprovalRequired", _cooapprovalrequired);

                if (_cBranchKey == null)
                    cm.Parameters.AddWithValue("@CBranchKey", 0);
                else
                    cm.Parameters.AddWithValue("@CBranchKey", _cBranchKey);

                if (_cBranchID == null)
                    cm.Parameters.AddWithValue("@CBranchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CBranchID", _cBranchID);

                if (_cDeptKey == null)
                    cm.Parameters.AddWithValue("@CDeptKey", 0);
                else
                    cm.Parameters.AddWithValue("@CDeptKey", _cDeptKey);

                if (_cDeptID == null)
                    cm.Parameters.AddWithValue("@CDeptID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDeptID", _cDeptID);

                if (_cGrpKey == null)
                    cm.Parameters.AddWithValue("@CGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CGrpKey", _cGrpKey);

                if (_cGrpID == null)
                    cm.Parameters.AddWithValue("@CGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CGrpID", _cGrpID);

                if (_cTerritoryKey == null)
                    cm.Parameters.AddWithValue("@CTerritoryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTerritoryKey", _cTerritoryKey);

                if (_cTerritoryID == null)
                    cm.Parameters.AddWithValue("@CTerritoryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTerritoryID", _cTerritoryID);

                if (_cIndustryKey == null)
                    cm.Parameters.AddWithValue("@CIndustryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CIndustryKey", _cIndustryKey);

                if (_cIndustryID == null)
                    cm.Parameters.AddWithValue("@CIndustryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CIndustryID", _cIndustryID);

                if (_cClass == null)
                    cm.Parameters.AddWithValue("@CClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CClass", _cClass);

                if (_cPriceType == null)
                    cm.Parameters.AddWithValue("@CPriceType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CPriceType", _cPriceType);

                if (_cOverallDefaultDis == null)
                    cm.Parameters.AddWithValue("@COverallDefaultDis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@COverallDefaultDis", _cOverallDefaultDis);

                if (_cTermKey == null)
                    cm.Parameters.AddWithValue("@CTermKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTermKey", _cTermKey);

                if (_cCreditLimit == null)
                    cm.Parameters.AddWithValue("@CCreditLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCreditLimit", _cCreditLimit);

                if (_cTaxGrpKey == null)
                    cm.Parameters.AddWithValue("@CTaxGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CTaxGrpKey", _cTaxGrpKey);

                if (_cEMKey == null)
                    cm.Parameters.AddWithValue("@CEMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CEMKey", _cEMKey);

                if (_cemid == null)
                    cm.Parameters.AddWithValue("@Cemid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Cemid", _cemid);

                if (_cCurrkey == null)
                    cm.Parameters.AddWithValue("@CCurrkey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCurrkey", _cCurrkey);

                if (_cCurrID == null)
                    cm.Parameters.AddWithValue("@CCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCurrID", _cCurrID);

                if (_cAccKey == null)
                    cm.Parameters.AddWithValue("@CAccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CAccKey", _cAccKey);

                if (_cDefaultBillAddr == null)
                    cm.Parameters.AddWithValue("@CDefaultBillAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultBillAddr", _cDefaultBillAddr);

                if (_cDefaultShipAddr == null)
                    cm.Parameters.AddWithValue("@CDefaultShipAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultShipAddr", _cDefaultShipAddr);

                if (_cDefaultStateAddr == null)
                    cm.Parameters.AddWithValue("@CDefaultStateAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultStateAddr", _cDefaultStateAddr);

                if (_cDefaultStateType == null)
                    cm.Parameters.AddWithValue("@CDefaultStateType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultStateType", _cDefaultStateType);

                if (_cDefaultContact == null)
                    cm.Parameters.AddWithValue("@CDefaultContact", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultContact", _cDefaultContact);

                if (_cDefaultContactState == null)
                    cm.Parameters.AddWithValue("@CDefaultContactState", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CDefaultContactState", _cDefaultContactState);

                if (_cRemDelivery == null)
                    cm.Parameters.AddWithValue("@CRemDelivery", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemDelivery", _cRemDelivery);

                if (_cRemPrice == null)
                    cm.Parameters.AddWithValue("@CRemPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemPrice", _cRemPrice);

                if (_cRemValidity == null)
                    cm.Parameters.AddWithValue("@CRemValidity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemValidity", _cRemValidity);

                if (_cRemPayment == null)
                    cm.Parameters.AddWithValue("@CRemPayment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRemPayment", _cRemPayment);

                if (_cRem == null)
                    cm.Parameters.AddWithValue("@CRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CRem", _cRem);

                if (_formerknownas == null)
                    cm.Parameters.AddWithValue("@FormerKnownAs", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FormerKnownAs", _formerknownas);

                if (_cAttachment == null)
                    cm.Parameters.AddWithValue("@CAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CAttachment", _cAttachment);

                if (_customerSinceDate == null || ((DateTime)_customerSinceDate).Year == 1)
                    cm.Parameters.AddWithValue("@CustomerSinceDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustomerSinceDate", _customerSinceDate.Value);

                if (_cCreditBal == null)
                    cm.Parameters.AddWithValue("@CCreditBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCreditBal", _cCreditBal);

                if (_cCashBal == null)
                    cm.Parameters.AddWithValue("@CCashBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CCashBal", _cCashBal);

                if (_vBranchKey == null)
                    cm.Parameters.AddWithValue("@VBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VBranchKey", _vBranchKey);

                if (_vBranchID == null)
                    cm.Parameters.AddWithValue("@VBranchID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VBranchID", _vBranchID);

                if (_vDeptKey == null)
                    cm.Parameters.AddWithValue("@VDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDeptKey", _vDeptKey);

                if (_vDeptID == null)
                    cm.Parameters.AddWithValue("@VDeptID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDeptID", _vDeptID);

                if (_vGrpKey == null)
                    cm.Parameters.AddWithValue("@VGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VGrpKey", _vGrpKey);

                if (_vGrpID == null)
                    cm.Parameters.AddWithValue("@VGrpID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VGrpID", _vGrpID);

                if (_vTerritoryKey == null)
                    cm.Parameters.AddWithValue("@VTerritoryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTerritoryKey", _vTerritoryKey);

                if (_vTerritoryID == null)
                    cm.Parameters.AddWithValue("@VTerritoryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTerritoryID", _vTerritoryID);

                if (_vIndustryKey == null)
                    cm.Parameters.AddWithValue("@VIndustryKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VIndustryKey", _vIndustryKey);

                if (_vIndustryID == null)
                    cm.Parameters.AddWithValue("@VIndustryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VIndustryID", _vIndustryID);

                if (_vClass == null)
                    cm.Parameters.AddWithValue("@VClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VClass", _vClass);

                if (_vPriceType == null)
                    cm.Parameters.AddWithValue("@VPriceType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VPriceType", _vPriceType);

                if (_vOverallDefaultDis == null)
                    cm.Parameters.AddWithValue("@VOverallDefaultDis", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VOverallDefaultDis", _vOverallDefaultDis);

                if (_vTermKey == null)
                    cm.Parameters.AddWithValue("@VTermKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTermKey", _vTermKey);

                if (_vCreditLimit == null)
                    cm.Parameters.AddWithValue("@VCreditLimit", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VCreditLimit", _vCreditLimit);

                if (_vTaxGrpKey == null)
                    cm.Parameters.AddWithValue("@VTaxGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VTaxGrpKey", _vTaxGrpKey);

                if (_vEMKey == null)
                    cm.Parameters.AddWithValue("@VEMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VEMKey", _vEMKey);

                if (_vemid == null)
                    cm.Parameters.AddWithValue("@Vemid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Vemid", _vemid);

                if (_vCurrkey == null)
                    cm.Parameters.AddWithValue("@VCurrkey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VCurrkey", _vCurrkey);

                if (_vCurrID == null)
                    cm.Parameters.AddWithValue("@VCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VCurrID", _vCurrID);

                if (_vAccKey == null)
                    cm.Parameters.AddWithValue("@VAccKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VAccKey", _vAccKey);

                if (_vDefaultBillAddr == null)
                    cm.Parameters.AddWithValue("@VDefaultBillAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultBillAddr", _vDefaultBillAddr);

                if (_vDefaultShipAddr == null)
                    cm.Parameters.AddWithValue("@VDefaultShipAddr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultShipAddr", _vDefaultShipAddr);

                if (_vDefaultContact == null)
                    cm.Parameters.AddWithValue("@VDefaultContact", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultContact", _vDefaultContact);

                if (_vDefaultAPPYDocType == null)
                    cm.Parameters.AddWithValue("@VDefaultAPPYDocType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VDefaultAPPYDocType", _vDefaultAPPYDocType);

                if (_vRemDelivery == null)
                    cm.Parameters.AddWithValue("@VRemDelivery", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemDelivery", _vRemDelivery);

                if (_vRemPrice == null)
                    cm.Parameters.AddWithValue("@VRemPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemPrice", _vRemPrice);

                if (_vRemValidity == null)
                    cm.Parameters.AddWithValue("@VRemValidity", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemValidity", _vRemValidity);

                if (_vRemPayment == null)
                    cm.Parameters.AddWithValue("@VRemPayment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRemPayment", _vRemPayment);

                if (_vRem == null)
                    cm.Parameters.AddWithValue("@VRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VRem", _vRem);

                if (_vAttachment == null)
                    cm.Parameters.AddWithValue("@VAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VAttachment", _vAttachment);

                if (_vendorSinceDate == null || ((DateTime)_vendorSinceDate).Year == 1)
                    cm.Parameters.AddWithValue("@VendorSinceDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VendorSinceDate", _vendorSinceDate.Value);

                if (_vBal == null)
                    cm.Parameters.AddWithValue("@VBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@VBal", _vBal);

                if (_conNamFirst == null)
                    cm.Parameters.AddWithValue("@ConNamFirst", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamFirst", _conNamFirst);

                if (_conNamLast == null)
                    cm.Parameters.AddWithValue("@ConNamLast", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamLast", _conNamLast);

                if (_conNamMiddle == null)
                    cm.Parameters.AddWithValue("@ConNamMiddle", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamMiddle", _conNamMiddle);

                if (_conNamInitials == null)
                    cm.Parameters.AddWithValue("@ConNamInitials", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNamInitials", _conNamInitials);

                if (_conBirthday == null || ((DateTime)_conBirthday).Year == 1)
                    cm.Parameters.AddWithValue("@ConBirthday", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConBirthday", _conBirthday.Value);

                if (_conGender == null)
                    cm.Parameters.AddWithValue("@ConGender", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConGender", _conGender);

                if (_conMarital == null)
                    cm.Parameters.AddWithValue("@ConMarital", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConMarital", _conMarital);

                if (_conChildren == null)
                    cm.Parameters.AddWithValue("@ConChildren", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConChildren", _conChildren);

                if (_conSocSecNo == null)
                    cm.Parameters.AddWithValue("@ConSocSecNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConSocSecNo", _conSocSecNo);

                if (_conNationality == null)
                    cm.Parameters.AddWithValue("@ConNationality", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConNationality", _conNationality);

                if (_occuTitle == null)
                    cm.Parameters.AddWithValue("@OccuTitle", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuTitle", _occuTitle);

                if (_occuIndustry == null)
                    cm.Parameters.AddWithValue("@OccuIndustry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuIndustry", _occuIndustry);

                if (_occuSalary == null)
                    cm.Parameters.AddWithValue("@OccuSalary", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuSalary", _occuSalary);

                if (_occuGroup == null)
                    cm.Parameters.AddWithValue("@OccuGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OccuGroup", _occuGroup);

                if (_emailStatement == null)
                    cm.Parameters.AddWithValue("@EmailStatement", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EmailStatement", _emailStatement);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                //if (_lastModifiedDate == null)
                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                //else
                //    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                if (_custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _custom1);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _custom2);

                if (_custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _custom3);

                if (_custom4 == null)
                    cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom4", _custom4);

                if (_custom5 == null)
                    cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom5", _custom5);

                

                //added by thettm on 02 April 2018(start)
                if (_accountName == null)
                    cm.Parameters.AddWithValue("@AccountName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccountName", _accountName);

                if (_bankName == null)
                    cm.Parameters.AddWithValue("@BankName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankName", _bankName);

                if (_branchName == null)
                    cm.Parameters.AddWithValue("@BranchName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchName", _branchName);

                if (_country == null)
                    cm.Parameters.AddWithValue("@Country", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Country", _country);

                if (_bankCode == null)
                    cm.Parameters.AddWithValue("@BankCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankCode", _bankCode);

                if (_branchCode == null)
                    cm.Parameters.AddWithValue("@BranchCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchCode", _branchCode);

                if (_sWIFTCode == null)
                    cm.Parameters.AddWithValue("@SWIFTCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SWIFTCode", _sWIFTCode);

                if (_bankCurrKey == null)
                    cm.Parameters.AddWithValue("@BankCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankCurrKey", _bankCurrKey);

                if (_bankCurrID == null)
                    cm.Parameters.AddWithValue("@BankCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankCurrID", _bankCurrID);

                if (_bankAccountNo == null)
                    cm.Parameters.AddWithValue("@BankAccountNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankAccountNo", _bankAccountNo);

                if (_iBAN_NO == null)
                    cm.Parameters.AddWithValue("@IBAN_NO", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IBAN_NO", _iBAN_NO);

                if (_iNTBankName == null)
                    cm.Parameters.AddWithValue("@INTBankName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankName", _iNTBankName);

                if (_iNTBankCountry == null)
                    cm.Parameters.AddWithValue("@INTBankCountry", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankCountry", _iNTBankCountry);

                if (_iNTBankCode == null)
                    cm.Parameters.AddWithValue("@INTBankCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankCode", _iNTBankCode);

                if (_iNTBranchCode == null)
                    cm.Parameters.AddWithValue("@INTBranchCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBranchCode", _iNTBranchCode);

                if (_iNTSWIFTCode == null)
                    cm.Parameters.AddWithValue("@INTSWIFTCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTSWIFTCode", _iNTSWIFTCode);

                if (_iNTCurrKey == null)
                    cm.Parameters.AddWithValue("@INTCurrKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTCurrKey", _iNTCurrKey);

                if (_iNTCurrID == null)
                    cm.Parameters.AddWithValue("@INTCurrID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTCurrID", _iNTCurrID);

                if (_iNTBankAccountNo == null)
                    cm.Parameters.AddWithValue("@INTBankAccountNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTBankAccountNo", _iNTBankAccountNo);

                if (_bankAddress == null)
                    cm.Parameters.AddWithValue("@BankAddress", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BankAddress", _bankAddress);

                if (_deliModeCode == null)
                    cm.Parameters.AddWithValue("@DeliModeCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeliModeCode", _deliModeCode);

                if (_deliModeCodeValue == null)
                    cm.Parameters.AddWithValue("@DeliModeCodeValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeliModeCodeValue", _deliModeCodeValue);

                cm.Parameters.AddWithValue("@SalesRepIsHeadSales", _salesRepIsHeadSales);
                //added by thettm on 02 April 2018(end)

                cm.Parameters["@NewConKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
               

            }// Already close and dispose sql connection.
            
        }
        #endregion //Data Access - Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call delete method.
                    retValue = this.Delete(cn, criteria);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_Delete";
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool? isNew)
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call validation method.
                    retValue = this.Validation(cn, criteria, isNew);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria,bool? isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);
               

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);

                cm.Parameters.AddWithValue("@ConID", criteria._conID);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
               
            }            
        }
        #endregion //Data Access - Validation

        #region Record Access Level

        internal bool CanAccessRecord(int? conKey)
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call validation method.
                    retValue = this.CanAccessRecord(cn, conKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
                
            return retValue;
        }
        internal bool CanAccessRecord(SqlConnection cn, int? conKey)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECRecAccess_Check";
               

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@Key", conKey);

                cm.Parameters.AddWithValue("@UserAccessLevel", AppInfor.conAccessLevel);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.CurrentUserKey);

                cm.ExecuteNonQuery();

               if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                   return false;
            }
        }
        internal bool AccessLevelUpdate(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_AccessLevelUpdate";

                cm.Parameters.AddWithValue("@Option", 1);


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_conKey == null)
                    cm.Parameters.AddWithValue("@ConKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey", _conKey);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                if (_conUEN == null)
                    cm.Parameters.AddWithValue("@ConUEN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConUEN", _conUEN);

                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;


            }// Already close and dispose sql connection.            
        }

        #endregion

        private void Clear()
        {
            _conKey = 0;
            _conID = string.Empty;
            _conNm = string.Empty;
            _conType = 10;
            _cCBType = 10;
            _noFinCharge = false;
            _accessLevel = 0;
            _accessGroup = 0;
            _conUEN = string.Empty;
            _inactive = false;
            _approval = false;
            _rejected = false;
            _activewithproblem = false;
            _cooapprovalrequired = false;
            _cBranchKey = 0;
            _cBranchID = string.Empty;
            _cDeptKey = 0;
            _cDeptID = string.Empty;
            _cGrpKey = 0;
            _cGrpID = string.Empty;
            _cTerritoryKey = null;
            _cTerritoryID = string.Empty;
            _cIndustryKey = null;
            _cIndustryID = string.Empty;
            _cClass = string.Empty;
            _cPriceType = null;
            _cTermKey = null;
            _cCreditLimit = 0;
            _cTaxGrpKey = null;
            _cEMKey = null;
            _cemid = string.Empty;
            _cCurrkey = 1;
            _cCurrID = string.Empty;
            _cAccKey = null;
            _cDefaultBillAddr = string.Empty;
            _cDefaultShipAddr = string.Empty;
            _cDefaultStateAddr = string.Empty;
            _cDefaultStateType = 10;
            _cDefaultContact = string.Empty;
            _cDefaultContactState = string.Empty;
            _cRemDelivery = string.Empty;
            _cRemPrice = string.Empty;
            _cRemValidity = string.Empty;
            _cRemPayment = string.Empty;
            _cRem = string.Empty;
            _formerknownas = string.Empty;
            _cAttachment = false;
            _customerSinceDate = null;
            _cCreditBal = 0;
            _cCashBal = 0;
            _cNTBal = 0;
            _vBranchKey = 0;
            _vBranchID = string.Empty;
            _vDeptKey = 0;
            _vDeptID = string.Empty;
            _vGrpKey = 0;
            _vGrpID = string.Empty;
            _vTerritoryKey = null;
            _vTerritoryID = string.Empty;
            _vIndustryKey = null;
            _vIndustryID = string.Empty;
            _vClass = string.Empty;
            _vPriceType = null;
            _vTermKey = null;
            _vCreditLimit = 0;
            _vTaxGrpKey = null;
            _vEMKey = null;
            _vemid = string.Empty;
            _vCurrkey = 1;
            _vCurrID = string.Empty;
            _vAccKey = null;
            _vDefaultBillAddr = string.Empty;
            _vDefaultShipAddr = string.Empty;
            _vDefaultContact = string.Empty;
            _vRemDelivery = string.Empty;
            _vRemPrice = string.Empty;
            _vRemValidity = string.Empty;
            _vRemPayment = string.Empty;
            _vRem = string.Empty;
            _vAttachment = false;
            _vendorSinceDate = null;
            _vBal = 0;
            _conNamFirst = string.Empty;
            _conNamLast = string.Empty;
            _conNamMiddle = string.Empty;
            _conNamInitials = string.Empty;
            _conBirthday = null;
            _conGender = null;
            _conMarital = null;
            _conChildren = 0;
            _conSocSecNo = string.Empty;
            _conNationality = string.Empty;
            _occuTitle = string.Empty;
            _occuIndustry = string.Empty;
            _occuSalary = string.Empty;
            _occuGroup = string.Empty;
            _emailStatement = false;
            _createDate = null;
            _createUserKey = 0;
            _lastModifiedDate = null;
            _lastModifiedUserKey = 0;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            _custom4 = string.Empty;
            _custom5 = string.Empty;

            //added by thettm on 02 april 2018 (start)
            _accountName = string.Empty;
            _bankName = string.Empty;
            _branchName = string.Empty;
            _country = string.Empty;
            _bankCode = string.Empty;
            _branchCode = string.Empty;
            _sWIFTCode = string.Empty;
            _bankCurrKey = 1;
            _bankCurrID = string.Empty;
            _bankAccountNo = string.Empty;
            _iBAN_NO = string.Empty;
            _iNTBankName = string.Empty;
            _iNTBankCountry = string.Empty;
            _iNTBankCode = string.Empty;
            _iNTBranchCode = string.Empty;
            _iNTSWIFTCode = string.Empty;
            _iNTCurrKey = 1;
            _iNTCurrID = string.Empty;
            _iNTBankAccountNo = string.Empty;
            _bankAddress = string.Empty;
            _deliModeCode = string.Empty;
            _deliModeCodeValue = string.Empty;
            //added by thettm on 02 april 2018 (end)

        }

    }
}


