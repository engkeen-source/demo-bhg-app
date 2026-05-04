using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTItm : Csla.BusinessBase<MSTItm>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _itmKey = 0;
        internal int? _itmType = 100;
        internal int? _masterItmKey = 0;
        internal int? _masterItmType = 0;
        internal int? _substituteItmKey = 0;
        internal string _itmID = string.Empty;

        //added by thettm on 06 jul 2017(start)
        internal string _mapitmID = string.Empty;
        //added by thettm on 06 jul 2017(end)

        internal string _itmDes = string.Empty;
        internal string _itmRem = string.Empty;
        internal int? _accessLevel = 0;
        internal int? _accessGroup = 0;
        internal int? _cSGVendorKey = null;
        internal string _cSGVendorID = string.Empty;
        internal string _industryPN = string.Empty;
        internal string _sku1 = string.Empty;
        internal string _sku2 = string.Empty;
        internal int? _catKey1 = 0;
        internal string _catID1 = string.Empty;
        internal int? _catKey2 = 0;
        internal string _catID2 = string.Empty;
        internal int? _catKey3 = 0;
        internal string _catID3 = string.Empty;
        internal int? _catKey4 = 0;
        internal string _catID4 = string.Empty;
        internal int? _catKey5 = 0;
        internal string _catID5 = string.Empty;
        internal int? _brandkey = null;
        internal string _brandID = string.Empty;
        internal string _model = string.Empty;
        internal string _iNClass = string.Empty;
        internal bool? _inactive = false;
        internal int? _costMethod = null;
        internal int? _branchKey = 0;
        internal int? _deptKey = 0;
        internal int? _accICKey = null;
        internal int? _accINKey = null;
        internal int? _accPHKey = null;
        internal int? _bUOMKey = null;
        internal string _buomid = string.Empty;
        internal string _masterItmID = string.Empty;
        internal string _substituteItmID = string.Empty;
        internal string _accICID = string.Empty;
        internal string _accINID = string.Empty;
        internal string _accPHID = string.Empty;
        internal int? _accDSICKey = null;
        internal int? _accDSPHKey = null;
        internal string _accDSICID = string.Empty;
        internal string _accDSPHID = string.Empty;

        internal decimal? _qtyStock = null;
        internal decimal? _qtyMin = null;
        internal decimal? _qtyMax = null;
        internal decimal? _qtyReOrder = null;
        internal decimal? _salesWrtyYr = null;
        internal decimal? _purchaseWrtyYr = null;
        internal int? _defLocSale = null;
        internal int? _defLocPurchase = null;
        internal decimal? _leadTimeInDays = null;
        internal decimal? _costLatest = null;
        internal DateTime? _costLatestDate = null;
        internal decimal? _costLanded = null;
        internal DateTime? _costLandedDate = null;
        internal decimal? _costAvg = null;
        internal decimal? _controlPriceH = null;
        internal decimal? _openBalCost = 0;
        internal decimal? _openBalQty = 0;
        internal decimal? _openBalAmtH = 0;
        internal bool? _taxable = true;
        internal int? _commisionType = 0;
        internal int? _bOMType = 10;
        internal int? _bOMMultiplier = 1;
        internal int? _bOMOverHeadKey = null;
        internal string _defaultExpDate = string.Empty;
        internal int? _colorKey = null;
        internal string _colorID = string.Empty;
        internal int? _scaleKey = null;
        internal string _scaleID = string.Empty;
        internal short? _scaleSizeNum = null;
        internal string _scaleSize = string.Empty;
        internal decimal? _weightNet = 0;
        internal decimal? _weightGross = 0;
        internal int? _weightUOMKey = null;
        internal string _weightUOMID = string.Empty;
        internal decimal? _iNLength = 0;
        internal decimal? _iNWidth = 0;
        internal decimal? _iNHeight = 0;
        internal decimal? _iNVolume = 0;
        internal string _iNPacking = string.Empty;
        internal bool? _iNAttachment = false;
        internal decimal? _stdPackSize = 0;
        internal decimal? _stdPackWeight = 0;
        internal decimal? _stdPackLength = 0;
        internal decimal? _stdPackWidth = 0;
        internal decimal? _stdPackHeight = 0;
        internal string _saleUOM = string.Empty;
        internal decimal? _saleUOMRate = null;
        internal string _purchaseUOM = string.Empty;
        internal decimal? _purchaseUOMRate = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;
        internal string _custom4 = string.Empty;
        internal string _custom5 = string.Empty;
        internal string _custom6 = string.Empty;
        internal string _custom7 = string.Empty;
        internal string _custom8 = string.Empty;
        internal string _custom9 = string.Empty;
        internal string _custom10 = string.Empty;
        internal decimal _eStorePrice = 0.0M;
        internal string _error = string.Empty;

        //added by thettm on 23-oct-2017 (start)
        internal bool? _lotTracking = false;
        internal bool? _serialTracking = false;
        internal string _certiLink = string.Empty;
        //added by thettm on 23-oct-2017 (end)

        //added by nnt 26 Feb 19 (start)

        internal bool? _scan = false;
        //added by nnt 26 Feb 19(end)
        internal bool _blockPurchase = false;
        internal decimal? _ObCost = 0;

        //added by jane 2024 Jan 17 -- country of origin for HSCode
        internal string _countryID = string.Empty;      

        private SYSAttachments attachments= new SYSAttachments();

        //added by thettm on 23-oct-2017 (start)
        public bool? LotTracking
        {
            get
            {
                return _lotTracking;
            }
            set
            {
                _lotTracking = value;
                PropertyHasChanged("LotTracking");
            }
        }
       
        public bool? SerialTracking
        {
            get
            {
                return _serialTracking;
            }
            set
            {
                _serialTracking = value;
                PropertyHasChanged("SerialTracking");
            }
        }
        
        public string CertiLink
        {
            get
            {
                return _certiLink;
            }
            set
            {
                _certiLink = value;
                PropertyHasChanged("CertiLink");
            }
        }
        //added by thettm on 23-oct-2017 (end)

        //added by nnt on 26-feb-2019(start)
        public bool? Scanable
        {
            get
            {
                return _scan;
            }
            set
            {
                _scan = value;
                PropertyHasChanged("Scan");
            }
        }
        //added by nnt on 26-feb-2019(
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

        public int? ItmKey
        {
            get
            {
                return _itmKey;
            }
            set
            {
                _itmKey = value;
                PropertyHasChanged("ItmKey");
            }
        }

        public int? ItmType
        {
            get
            {
                return _itmType;
            }
            set
            {
                _itmType = value;
                PropertyHasChanged("ItmType");
            }
        }

        public int? MasterItmKey
        {
            get
            {
                return _masterItmKey;
            }
            set
            {
                _masterItmKey = value;
                PropertyHasChanged("MasterItmKey");
            }
        }

        public string MasterItmID
        {
            get
            {
                return _masterItmID;
            }
            set
            {
                _masterItmID = value;
                PropertyHasChanged("MasterItmID");
            }
        }

        public int? MasterItmType
        {
            get
            {
                return _masterItmType;
            }
            set
            {
                _masterItmType = value;
                PropertyHasChanged("MasterItmType");
            }
        }

        public int? SubstituteItmKey
        {
            get
            {
                return _substituteItmKey;
            }
            set
            {
                _substituteItmKey = value;
                PropertyHasChanged("SubstituteItmKey");
            }
        }

        public string SubstituteItmID
        {
            get
            {
                return _substituteItmID;
            }
            set
            {
                _substituteItmID = value;
                PropertyHasChanged("SubstituteItmID");
            }
        }

        public string ItmID
        {
            get
            {
                return _itmID;
            }
            set
            {
                _itmID = value;
                PropertyHasChanged("ItmID");
            }
        }

        //added by thettm on 06 jul 2017(start)
        public string MapitmID
        {
            get
            {
                return _mapitmID;
            }
            set
            {
                _mapitmID = value;
                PropertyHasChanged("MapitmID");
            }
        }
        //added by thettm on 06 jul 2017(end)

        public string ItmDes
        {
            get
            {
                return _itmDes;
            }
            set
            {
                _itmDes = value;
                PropertyHasChanged("ItmDes");
            }
        }

        public string ItmRem
        {
            get
            {
                return _itmRem;
            }
            set
            {
                _itmRem = value;
                PropertyHasChanged("ItmRem");
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
                _accessGroup = value;
                PropertyHasChanged("AccessGroup");
            }
        }

        public int? CSGVendorKey
        {
            get
            {
                return _cSGVendorKey;
            }
            set
            {
                _cSGVendorKey = value;
                PropertyHasChanged("CSGVendorKey");
            }
        }

        public string CSGVendorID
        {
            get
            {
                return _cSGVendorID;
            }
            set
            {
                _cSGVendorID = value;
                PropertyHasChanged("CSGVendorID");
            }
        }

        public string IndustryPN
        {
            get
            {
                return _industryPN;
            }
            set
            {
                _industryPN = value;
                PropertyHasChanged("IndustryPN");
            }
        }

        public string SKU1
        {
            get
            {
                return _sku1;
            }
            set
            {
                _sku1 = value;
                PropertyHasChanged("SKU1");
            }
        }

        public string SKU2
        {
            get
            {
                return _sku2;
            }
            set
            {
                _sku2 = value;
                PropertyHasChanged("SKU2");
            }
        }

        public int? CatKey1
        {
            get
            {
                return _catKey1;
            }
            set
            {
                _catKey1 = value;
                PropertyHasChanged("CatKey1");
            }
        }

        public string CatID1
        {
            get
            {
                return _catID1;
            }
            set
            {
                _catID1 = value;
                PropertyHasChanged("CatID1");
            }
        }

        public int? CatKey2
        {
            get
            {
                return _catKey2;
            }
            set
            {
                _catKey2 = value;
                PropertyHasChanged("CatKey2");
            }
        }

        public string CatID2
        {
            get
            {
                return _catID2;
            }
            set
            {
                _catID2 = value;
                PropertyHasChanged("CatID2");
            }
        }

        public int? CatKey3
        {
            get
            {
                return _catKey3;
            }
            set
            {
                _catKey3 = value;
                PropertyHasChanged("CatKey3");
            }
        }

        public string CatID3
        {
            get
            {
                return _catID3;
            }
            set
            {
                _catID3 = value;
                PropertyHasChanged("CatID3");
            }
        }

        public int? CatKey4
        {
            get
            {
                return _catKey4;
            }
            set
            {
                _catKey4 = value;
                PropertyHasChanged("CatKey4");
            }
        }

        public string CatID4
        {
            get
            {
                return _catID4;
            }
            set
            {
                _catID4 = value;
                PropertyHasChanged("CatID4");
            }
        }

        public int? CatKey5
        {
            get
            {
                return _catKey5;
            }
            set
            {
                _catKey5 = value;
                PropertyHasChanged("CatKey5");
            }
        }

        public string CatID5
        {
            get
            {
                return _catID5;
            }
            set
            {
                _catID5 = value;
                PropertyHasChanged("CatID5");
            }
        }

        public int? Brandkey
        {
            get
            {
                return _brandkey;
            }
            set
            {
                _brandkey = value;
                PropertyHasChanged("Brandkey");
            }
        }

        public string BrandID
        {
            get
            {
                return _brandID;
            }
            set
            {
                _brandID = value;
                PropertyHasChanged("BrandID");
            }
        }

        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                PropertyHasChanged("Model");
            }
        }

        public string INClass
        {
            get
            {
                return _iNClass;
            }
            set
            {
                _iNClass = value;
                PropertyHasChanged("INClass");
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

        public int? CostMethod
        {
            get
            {
                return _costMethod;
            }
            set
            {
                _costMethod = value;
                PropertyHasChanged("CostMethod");
            }
        }

        public int? BranchKey
        {
            get
            {
                return _branchKey;
            }
            set
            {
                _branchKey = value;
                PropertyHasChanged("BranchKey");
            }
        }

        public int? DeptKey
        {
            get
            {
                return _deptKey;
            }
            set
            {
                _deptKey = value;
                PropertyHasChanged("DeptKey");
            }
        }

        public int? AccICKey
        {
            get
            {
                return _accICKey;
            }
            set
            {
                _accICKey = value;
                PropertyHasChanged("AccICKey");
            }
        }

        public string AccICID
        {
            get
            {
                return _accICID;
            }
            set
            {
                _accICID = value;
                PropertyHasChanged("AccICID");
            }
        }

        public int? AccINKey
        {
            get
            {
                return _accINKey;
            }
            set
            {
                _accINKey = value;
                PropertyHasChanged("AccINKey");
            }
        }

        public string AccINID
        {
            get
            {
                return _accINID;
            }
            set
            {
                _accINID = value;
                PropertyHasChanged("AccINID");
            }
        }

        public int? AccPHKey
        {
            get
            {
                return _accPHKey;
            }
            set
            {
                _accPHKey = value;
                PropertyHasChanged("AccPHKey");
            }
        }


        public string AccPHID
        {
            get
            {
                return _accPHID;
            }
            set
            {
                _accPHID = value;
                PropertyHasChanged("AccPHID");
            }
        }
        public int? AccDSICKey
        {
            get
            {
                return _accDSICKey;
            }
            set
            {
                _accDSICKey = value;
                PropertyHasChanged("AccDSICKey");
            }
        }

        public string AccDSICID
        {
            get
            {
                return _accDSICID;
            }
            set
            {
                _accDSICID = value;
                PropertyHasChanged("AccDSICID");
            }
        }

        public int? AccDSPHKey
        {
            get
            {
                return _accDSPHKey;
            }
            set
            {
                _accDSPHKey = value;
                PropertyHasChanged("_accDSPHKey");
            }
        }
        public string AccDSPHID
        {
            get
            {
                return _accDSPHID;
            }
            set
            {
                _accDSPHID = value;
                PropertyHasChanged("AccDSPHID");
            }
        }

        public int? BUOMKey
        {
            get
            {
                return _bUOMKey;
            }
            set
            {
                _bUOMKey = value;
                PropertyHasChanged("BUOMKey");
            }
        }

        public string BUOMID
        {
            get
            {
                return _buomid;
            }
            set
            {
                _buomid = value;
                PropertyHasChanged("BUOMID");
            }
        }

        public decimal? QtyStock
        {
            get
            {
                return _qtyStock;
            }
            set
            {
                _qtyStock = value;
                PropertyHasChanged("QtyStock");
            }
        }

        public decimal? QtyMin
        {
            get
            {
                return _qtyMin;
            }
            set
            {
                _qtyMin = value;
                PropertyHasChanged("QtyMin");
            }
        }

        public decimal? QtyMax
        {
            get
            {
                return _qtyMax;
            }
            set
            {
                _qtyMax = value;
                PropertyHasChanged("QtyMax");
            }
        }

        public decimal? QtyReOrder
        {
            get
            {
                return _qtyReOrder;
            }
            set
            {
                _qtyReOrder = value;
                PropertyHasChanged("QtyReOrder");
            }
        }
        public decimal? SalesWrtyYr
        {
            get
            {
                return _salesWrtyYr;
            }
            set
            {
                _salesWrtyYr = value;
                PropertyHasChanged("SalesWrtyYr");
            }
        }
        public decimal? PurchaseWrtyYr
        {
            get
            {
                return _purchaseWrtyYr;
            }
            set
            {
                _purchaseWrtyYr = value;
                PropertyHasChanged("PurchaseWrtyYr");
            }
        }

        public int? DefLocSale
        {
            get
            {
                return _defLocSale;
            }
            set
            {
                _defLocSale = value;
                PropertyHasChanged("DefLocSale");
            }
        }
        public int? DefLocPurchase
        {
            get
            {
                return _defLocPurchase;
            }
            set
            {
                _defLocPurchase = value;
                PropertyHasChanged("DefLocPurchase");
            }
        }

        public decimal? LeadTimeInDays
        {
            get
            {
                return _leadTimeInDays;
            }
            set
            {
                _leadTimeInDays = value;
                PropertyHasChanged("LeadTimeInDays");
            }
        }

        public decimal? CostLatest
        {
            get
            {
                return _costLatest;
            }
            set
            {
                _costLatest = value;
                PropertyHasChanged("CostLatest");
            }
        }

        public DateTime? CostLatestDate
        {
            get
            {
                return _costLatestDate;
            }
            set
            {
                _costLatestDate = value;
                PropertyHasChanged("CostLatestDate");
            }
        }

        public decimal? CostLanded
        {
            get
            {
                return _costLanded;
            }
            set
            {
                _costLanded = value;
                PropertyHasChanged("CostLanded");
            }
        }

        public DateTime? CostLandedDate
        {
            get
            {
                return _costLandedDate;
            }
            set
            {
                _costLandedDate = value;
                PropertyHasChanged("CostLandedDate");
            }
        }

        public decimal? CostAvg
        {
            get
            {
                return _costAvg;
            }
            set
            {
                _costAvg = value;
                PropertyHasChanged("CostAvg");
            }
        }

        public decimal? ControlPriceH
        {
            get
            {
                return _controlPriceH;
            }
            set
            {
                _controlPriceH = value;
                PropertyHasChanged("ControlPriceH");
            }
        }

        public decimal? OpenBalCost
        {
            get
            {
                return _openBalCost;
            }
            set
            {
                _openBalCost = value;
                PropertyHasChanged("OpenBalCost");
            }
        }

        public decimal? OpenBalQty
        {
            get
            {
                return _openBalQty;
            }
            set
            {
                _openBalQty = value;
                PropertyHasChanged("OpenBalQty");
            }
        }

        public decimal? OpenBalAmtH
        {
            get
            {
                return _openBalAmtH;
            }
            set
            {
                _openBalAmtH = value;
                PropertyHasChanged("OpenBalAmtH");
            }
        }

        public bool? Taxable
        {
            get
            {
                return _taxable;
            }
            set
            {
                _taxable = value;
                PropertyHasChanged("Taxable");
            }
        }

        public int? CommisionType
        {
            get
            {
                return _commisionType;
            }
            set
            {
                _commisionType = value;
                PropertyHasChanged("CommisionType");
            }
        }

        public int? BOMType
        {
            get
            {
                return _bOMType;
            }
            set
            {
                _bOMType = value;
                PropertyHasChanged("BOMType");
            }
        }

        public int? BOMMultiplier
        {
            get
            {
                return _bOMMultiplier;
            }
            set
            {
                _bOMMultiplier = value;
                PropertyHasChanged("BOMMultiplier");
            }
        }

        public int? BOMOverHeadKey
        {
            get
            {
                return _bOMOverHeadKey;
            }
            set
            {
                _bOMOverHeadKey = value;
                PropertyHasChanged("BOMOverHeadKey");
            }
        }

        public string DefaultExpDate
        {
            get
            {
                return _defaultExpDate;
            }
            set
            {
                _defaultExpDate = value;
                PropertyHasChanged("DefaultExpDate");
            }
        }

        public int? ColorKey
        {
            get
            {
                return _colorKey;
            }
            set
            {
                _colorKey = value;
                PropertyHasChanged("ColorKey");
            }
        }

        public string ColorID
        {
            get
            {
                return _colorID;
            }
            set
            {
                _colorID = value;
                PropertyHasChanged("ColorID");
            }
        }

        public int? ScaleKey
        {
            get
            {
                return _scaleKey;
            }
            set
            {
                _scaleKey = value;
                PropertyHasChanged("ScaleKey");
            }
        }

        public string ScaleID
        {
            get
            {
                return _scaleID;
            }
            set
            {
                _scaleID = value;
                PropertyHasChanged("ScaleID");
            }
        }

        public short? ScaleSizeNum
        {
            get
            {
                return _scaleSizeNum;
            }
            set
            {
                _scaleSizeNum = value;
                PropertyHasChanged("ScaleSizeNum");
            }
        }

        public string ScaleSize
        {
            get
            {
                return _scaleSize;
            }
            set
            {
                _scaleSize = value;
                PropertyHasChanged("ScaleSize");
            }
        }

        public decimal? WeightNet
        {
            get
            {
                return _weightNet;
            }
            set
            {
                _weightNet = value;
                PropertyHasChanged("WeightNet");
            }
        }

        public decimal? WeightGross
        {
            get
            {
                return _weightGross;
            }
            set
            {
                _weightGross = value;
                PropertyHasChanged("WeightGross");
            }
        }

        public int? WeightUOMKey
        {
            get
            {
                return _weightUOMKey;
            }
            set
            {
                _weightUOMKey = value;
                PropertyHasChanged("WeightUOMKey");
            }
        }

        public string WeightUOMID
        {
            get
            {
                return _weightUOMID;
            }
            set
            {
                _weightUOMID = value;
                PropertyHasChanged("WeightUOMID");
            }
        }

        public decimal? INLength
        {
            get
            {
                return _iNLength;
            }
            set
            {
                _iNLength = value;
                PropertyHasChanged("INLength");
            }
        }

        public decimal? INWidth
        {
            get
            {
                return _iNWidth;
            }
            set
            {
                _iNWidth = value;
                PropertyHasChanged("INWidth");
            }
        }

        public decimal? INHeight
        {
            get
            {
                return _iNHeight;
            }
            set
            {
                _iNHeight = value;
                PropertyHasChanged("INHeight");
            }
        }

        public decimal? INVolume
        {
            get
            {
                return _iNVolume;
            }
            set
            {
                _iNVolume = value;
                PropertyHasChanged("INVolume");
            }
        }

        public string INPacking
        {
            get
            {
                return _iNPacking;
            }
            set
            {
                _iNPacking = value;
                PropertyHasChanged("INPacking");
            }
        }

        public bool? INAttachment
        {
            get
            {
                return _iNAttachment;
            }
            set
            {
                _iNAttachment = value;
                PropertyHasChanged("INAttachment");
            }
        }

        public decimal? StdPackSize
        {
            get
            {
                return _stdPackSize;
            }
            set
            {
                _stdPackSize = value;
                PropertyHasChanged("StdPackSize");
            }
        }

        public decimal? StdPackWeight
        {
            get
            {
                return _stdPackWeight;
            }
            set
            {
                _stdPackWeight = value;
                PropertyHasChanged("StdPackWeight");
            }
        }

        public decimal? StdPackLength
        {
            get
            {
                return _stdPackLength;
            }
            set
            {
                _stdPackLength = value;
                PropertyHasChanged("StdPackLength");
            }
        }

        public decimal? StdPackWidth
        {
            get
            {
                return _stdPackWidth;
            }
            set
            {
                _stdPackWidth = value;
                PropertyHasChanged("StdPackWidth");
            }
        }

        public decimal? StdPackHeight
        {
            get
            {
                return _stdPackHeight;
            }
            set
            {
                _stdPackHeight = value;
                PropertyHasChanged("StdPackHeight");
            }
        }

        public string SaleUOM
        {
            get
            {
                return _saleUOM;
            }
            set
            {
                _saleUOM = value;
                PropertyHasChanged("SaleUOM");
            }
        }

        public decimal? SaleUOMRate
        {
            get
            {
                return _saleUOMRate;
            }
            set
            {
                _saleUOMRate = value;
                PropertyHasChanged("SaleUOMRate");
            }
        }

        public string PurchaseUOM
        {
            get
            {
                return _purchaseUOM;
            }
            set
            {
                _purchaseUOM = value;
                PropertyHasChanged("PurchaseUOM");
            }
        }

        public decimal? PurchaseUOMRate
        {
            get
            {
                return _purchaseUOMRate;
            }
            set
            {
                _purchaseUOMRate = value;
                PropertyHasChanged("PurchaseUOMRate");
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

        public string Custom6
        {
            get
            {
                return _custom6;
            }
            set
            {
                _custom6 = value;
                PropertyHasChanged("Custom6");
            }
        }

        public string Custom7
        {
            get
            {
                return _custom7;
            }
            set
            {
                _custom7 = value;
                PropertyHasChanged("Custom7");
            }
        }

        public string Custom8
        {
            get
            {
                return _custom8;
            }
            set
            {
                _custom8 = value;
                PropertyHasChanged("Custom8");
            }
        }

        public string Custom9
        {
            get
            {
                return _custom9;
            }
            set
            {
                _custom9 = value;
                PropertyHasChanged("Custom9");
            }
        }

        public string Custom10
        {
            get
            {
                return _custom10;
            }
            set
            {
                _custom10 = value;
                PropertyHasChanged("Custom10");
            }
        }

        public decimal EStorePrice
        {
            get
            {
                return _eStorePrice;
            }
            set
            {
                _eStorePrice = value;
                PropertyHasChanged("EStorePrice");
            }
        }

        public bool BlockPurchase
        {
            get
            {
                return _blockPurchase;
            }
            set
            {
                _blockPurchase = value;
                PropertyHasChanged("BlockPurchase");
            }
        }
        public decimal? ObCost
        {
            get
            {
                return _ObCost;
            }
            
        }

        public string CountryID
        {
            get
            {
                return _countryID;
            }
            set
            {
                _countryID = value;
                PropertyHasChanged("CountryID");
            }
        }
        public SYSAttachments Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }
        protected override object GetIdValue()
        {
            return _itmKey.ToString();
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
           // ItmID
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ItmID");
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ItmID", 50));
           //
           // ItmDes
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "ItmDes");
           //
           // CSGVendorID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CSGVendorID", 50));
           //
           // IndustryPN
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IndustryPN", 50));
           //
           // Sku1
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Sku1", 50));
           //
           // Sku2
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Sku2", 50));
           //
           // CatID1
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID1", 50));
           //
           // CatID2
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID2", 50));
           //
           // CatID3
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID3", 50));
           //
           // CatID4
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID4", 50));
           //
           // CatID5
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CatID5", 50));
           //
           // BrandID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BrandID", 50));
           //
           // Model
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Model", 50));
           //
           // INClass
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("INClass", 50));
           //
           // Buomid
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Buomid", 50));
           //
           // DefaultExpDate
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DefaultExpDate", 50));
           //
           // ColorID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ColorID", 50));
           //
           // ScaleID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ScaleID", 50));
           //
           // ScaleSize
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ScaleSize", 50));
           //
           // WeightUOMID
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("WeightUOMID", 50));
           //
           // INPacking
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("INPacking", 255));
           //
           // SaleUOM
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SaleUOM", 50));
           //
           // PurchaseUOM
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PurchaseUOM", 50));
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
           //
           // Custom6
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom6", 255));
           //
           // Custom7
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom7", 255));
           //
           // Custom8
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom8", 255));
           //
           // Custom9
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom9", 255));
           //
           // Custom10
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom10", 255));
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

        public MSTItm()
        { /* require use of factory method */ }

        public static MSTItm New()
        {            
            MSTItm child = new MSTItm();           
            return child;
        }

        internal static MSTItm NewChild()
        {            
            MSTItm child = new MSTItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();          
            return child;
        }

        internal static MSTItm Get(SafeDataReader dr)
        {            
            MSTItm child = new MSTItm();
            child.MarkAsChild();
            child.Fetch(dr);            
            return child;
        }

        public static MSTItm GetFirstChild(int? masterItmeKey)
        {
            MSTItm child = new MSTItm();
            child.Fetch(new Criteria(masterItmeKey, 10));
            return child;
        }

        public static MSTItm Get(int? itmKey)
        {            
            MSTItm child = new MSTItm();
            child.Fetch(new Criteria(itmKey, 1));
            return child;
        }


        public static MSTItm Get(SqlConnection cn, int? itmKey)
        {
            MSTItm child = new MSTItm();
            child.Fetch(cn, new Criteria(itmKey, 1));
            return child;
        }

        public static MSTItm GetParent(SqlConnection cn, int? itmKey)
        {
            MSTItm child = new MSTItm();
            child.Fetch(cn, new Criteria(itmKey,11));//11 for Substitute
            return child;
        }

        public static MSTItm GetParent(int? itmKey)
        {
            MSTItm child = new MSTItm();
            child.Fetch( new Criteria(itmKey, 11));//11 for Substitute
            return child;
        }
        
        public static MSTItm Get(string itmID)
        {            
            MSTItm child = new MSTItm();
            child.Fetch(new Criteria(itmID, 2));
            return child;
        }

        internal static MSTItm Get(SqlConnection cn, string itmID)
        {
            MSTItm child = new MSTItm();
            child.Fetch(cn, new Criteria(itmID, 2));
            return child;
        }

        public static MSTItm Get(string skuTxt, int? skuType)
        {
            MSTItm child = new MSTItm();
            child.Fetch(new Criteria(8,skuTxt,skuType));
            return child;
        }

        public static MSTItm Get(SqlConnection cn, string skuTxt, int? skuType)
        {
            MSTItm child = new MSTItm();
            child.Fetch(cn, new Criteria(8, skuTxt, skuType));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        public class Criteria
        {
            public int? _itmKey = null;
            public int? _option = null;
            public string _itemID = string.Empty;
            public string _itemIDFrom = "%";
            public string _itemIDTo = "%";

            public string _skuTxt = string.Empty;
            public int? _skuType = 0;

            public Criteria()
            {
            }

            public Criteria(int? ItmKey)
            {
                _itmKey = ItmKey;
            }

            public Criteria(int? ItmKey, int? Option)
            {
                _itmKey = ItmKey;
                _option = Option;
            }
            public Criteria(int? ItmKey, string ItemID)
            {
                _itmKey = ItmKey;
                _itemID = ItemID;
            }
            public Criteria(string ItemID, int? Option)
            {
                _option = Option;
                _itemID = ItemID;
            }
            public Criteria(int? Option, int? ItmKey, string ItemID)
            {
                _option = Option;
                _itmKey = ItmKey;
                _itemID = ItemID;
            }
            public Criteria(int?Option,string SKUTxt, int? SKUType)
            {
                _option = Option;
                _skuTxt = SKUTxt;
                _skuType = SKUType;
            }
            
        }

        #endregion //Criteria

        #region Data Access - Fetch

        public bool Fetch(Criteria criteria)
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

        public bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
             
                if (!GFunc.IsNEZ(criteria._itmKey))
                    cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                else
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);

                cm.Parameters.AddWithValue("@ItmIDFrom", criteria._itemIDFrom);
                cm.Parameters.AddWithValue("@ItmIDTo", criteria._itemIDTo);
                cm.Parameters.AddWithValue("@ItmID", criteria._itemID);

                cm.Parameters.AddWithValue("@SkuTxt", criteria._skuTxt);
                cm.Parameters.AddWithValue("@SkuType", criteria._skuType);               
              

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

                

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    retValue = false;
                }            

            }// Already close and dispose sql connection.
            
            return retValue;
        }

        public bool Fetch(SafeDataReader dr)
        {
            _itmKey = dr.GetInt32("ItmKey");
            _itmType = dr.GetInt32("ItmType");
            _masterItmKey = dr.GetInt32("MasterItmKey");
            _masterItmID = dr.GetString("MasterItmID");
            _masterItmType = dr.GetInt32("MasterItmType");
            _substituteItmKey = dr.GetInt32("SubstituteItmKey");
            _substituteItmID = dr.GetString("SubstituteItmID");
            _itmID = dr.GetString("ItmID");
            //added by thettm on 06 jul 2017(start)
            _mapitmID = dr.GetString("MapitmID");
            //added by thettm on 06 jul 2017(end)
            _itmDes = dr.GetString("ItmDes");
            _itmRem = dr.GetString("ItmRem");
            _accessLevel = dr.GetInt32("AccessLevel");
            _accessGroup = dr.GetInt32("AccessGroup");
            _cSGVendorKey = GFunc.NEInt(dr.GetValue("CSGVendorKey"),0);
            _cSGVendorID = dr.GetString("CSGVendorID");
            _industryPN = dr.GetString("IndustryPN");
            _sku1 = dr.GetString("SKU1");
            _sku2 = dr.GetString("SKU2");
            _catKey1 = dr.GetInt32("CatKey1");
            _catID1 = dr.GetString("CatID1");
            _catKey2 = dr.GetInt32("CatKey2");
            _catID2 = dr.GetString("CatID2");
            _catKey3 = dr.GetInt32("CatKey3");
            _catID3 = dr.GetString("CatID3");
            _catKey4 = dr.GetInt32("CatKey4");
            _catID4 = dr.GetString("CatID4");
            _catKey5 = dr.GetInt32("CatKey5");
            _catID5 = dr.GetString("CatID5");
            _brandkey = GFunc.NEInt(dr.GetValue("Brandkey"),0);
            _brandID = dr.GetString("BrandID");
            _model = dr.GetString("Model");
            _iNClass = dr.GetString("INClass");
            _inactive = dr.GetBoolean("Inactive");
            //added by thettm on 23-oct-2017(start)
            _lotTracking = dr.GetBoolean("LotTracking");
            _serialTracking = dr.GetBoolean("SerialTracking");
            _certiLink = dr.GetString("CertiLink");
            //added by thettm on 23-oct-2017(end)
            //added by nnt on 26-feb-2019 (start)
            _scan = dr.GetBoolean("Scan");
            //added by nnt on 26-feb-2019 (end)
            _costMethod = dr.GetInt32("CostMethod");
            _branchKey = dr.GetInt32("BranchKey");
            _deptKey = dr.GetInt32("DeptKey");
            _accICKey = dr.GetInt32("AccICKey");
            _accICID = dr.GetString("AccICID");
            _accDSICKey = dr.GetInt32("AccDSICKey");
            _accDSICID = dr.GetString("AccDSICID");
            _accINKey = dr.GetInt32("AccINKey");
            _accINID = dr.GetString("AccINID");
            _accPHKey = dr.GetInt32("AccPHKey");
            _accPHID = dr.GetString("AccPHID");
            _accDSPHKey = dr.GetInt32("AccDSPHKey");
            _accDSPHID = dr.GetString("AccDSPHID");
            _bUOMKey = dr.GetInt32("BUOMKey");
            _buomid = dr.GetString("BUOMID");
            _qtyStock = dr.GetDecimal("QtyStock");
            _qtyMin = dr.GetDecimal("QtyMin");
            _qtyMax = dr.GetDecimal("QtyMax");
            _qtyReOrder = dr.GetDecimal("QtyReOrder");
            _salesWrtyYr = dr.GetDecimal("SalesWrtyYr");
            _purchaseWrtyYr = dr.GetDecimal("PurchaseWrtyYr");
            _defLocSale = dr.GetInt32("DefLocSale");
            _defLocPurchase = dr.GetInt32("defLocPurchase");
            _leadTimeInDays = dr.GetDecimal("LeadTimeInDays");
            _costLatest = dr.GetDecimal("CostLatest");
            _eStorePrice = dr.GetDecimal("EStorePrice");

            if (GFunc.IsNE(dr.GetValue("CostLatestDate")))
                _costLatestDate = null;
            else
                _costLatestDate = dr.GetDateTime("CostLatestDate");

            _costLanded = dr.GetDecimal("CostLanded");

            if (GFunc.IsNE(dr.GetValue("CostLandedDate")))
                _costLandedDate = null;
            else
                _costLandedDate = dr.GetDateTime("CostLandedDate");

            _costAvg = dr.GetDecimal("CostAvg");
            _controlPriceH = dr.GetDecimal("ControlPriceH");
            _openBalCost = dr.GetDecimal("OpenBalCost");
            _openBalQty = dr.GetDecimal("OpenBalQty");
            _openBalAmtH = dr.GetDecimal("OpenBalAmtH");
            _taxable = dr.GetBoolean("Taxable");
            _commisionType = dr.GetInt32("CommisionType");
            _bOMType = dr.GetInt32("BOMType");
            _bOMMultiplier = dr.GetInt32("BOMMultiplier");
            _bOMOverHeadKey = dr.GetInt32("BOMOverHeadKey");
            _defaultExpDate = dr.GetString("DefaultExpDate");
            _colorKey = GFunc.NEInt(dr.GetValue("ColorKey"), 0);
            _colorID = dr.GetString("ColorID");
            _scaleKey = GFunc.NEInt(dr.GetValue("ScaleKey"),0);
            _scaleID = dr.GetString("ScaleID");
            _scaleSizeNum = dr.GetInt16("ScaleSizeNum");
            _scaleSize = dr.GetString("ScaleSize");
            _weightNet = dr.GetDecimal("WeightNet");
            _weightGross = dr.GetDecimal("WeightGross");
            _weightUOMKey = dr.GetInt32("WeightUOMKey");
            _weightUOMID = dr.GetString("WeightUOMID");
            _iNLength = dr.GetDecimal("INLength");
            _iNWidth = dr.GetDecimal("INWidth");
            _iNHeight = dr.GetDecimal("INHeight");
            _iNVolume = dr.GetDecimal("INVolume");
            _iNPacking = dr.GetString("INPacking");
            _iNAttachment = dr.GetBoolean("INAttachment");
            _stdPackSize = dr.GetDecimal("StdPackSize");
            _stdPackWeight = dr.GetDecimal("StdPackWeight");
            _stdPackLength = dr.GetDecimal("StdPackLength");
            _stdPackWidth = dr.GetDecimal("StdPackWidth");
            _stdPackHeight = dr.GetDecimal("StdPackHeight");
            _saleUOM = dr.GetString("SaleUOM");
            _saleUOMRate = dr.GetDecimal("SaleUOMRate");
            _purchaseUOM = dr.GetString("PurchaseUOM");
            _purchaseUOMRate = dr.GetDecimal("PurchaseUOMRate");

            if (GFunc.IsNE(dr.GetValue("CreateDate")))
                _createDate = null;
            else
                _createDate = dr.GetDateTime("CreateDate");

            _createUserKey = dr.GetInt32("CreateUserKey");

            if (GFunc.IsNE(dr.GetValue("LastModifiedDate")))
                _lastModifiedDate = null;
            else
                _lastModifiedDate = dr.GetDateTime("LastModifiedDate");

            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            _custom4 = dr.GetString("Custom4");
            _custom5 = dr.GetString("Custom5");
            _custom6 = dr.GetString("Custom6");
            _custom7 = dr.GetString("Custom7");
            _custom8 = dr.GetString("Custom8");
            _custom9 = dr.GetString("Custom9");
            _custom10 = dr.GetString("Custom10");

            _blockPurchase = dr.GetBoolean("BlockPurchase");
            _ObCost = dr.GetDecimal("ObCost");
            _countryID = dr.GetString("CountryID");
            ValidationRules.CheckRules();
            return true;
            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? itmKey)
        {
            bool retValue = false;           
            itmKey = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out itmKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? itmKey,int Option)
        {
            
            itmKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", Option);                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", itmKey);

                //if (_itmKey == null)
                //    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                //else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_itmType == null)
                    cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmType", _itmType);

                if (_masterItmKey == null)
                    cm.Parameters.AddWithValue("@MasterItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterItmKey", _masterItmKey);

                if (_masterItmID == null)
                    cm.Parameters.AddWithValue("@MasterItmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterItmID", _masterItmID);

                if (_masterItmType == null)
                    cm.Parameters.AddWithValue("@MasterItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterItmType", _masterItmType);

                if (_substituteItmKey == null)
                    cm.Parameters.AddWithValue("@SubstituteItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SubstituteItmKey", _substituteItmKey);

                if (_substituteItmID == null)
                    cm.Parameters.AddWithValue("@SubstituteItmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SubstituteItmID", _substituteItmID);

                if (_itmID == null)
                    cm.Parameters.AddWithValue("@ItmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmID", _itmID);

                //added by thettm on 06 jul 2017(start)
                if (_mapitmID == null)
                    cm.Parameters.AddWithValue("@MapitmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MapitmID", _mapitmID);
                //added by thettm on 06 jul 2017(end)


                if (_itmDes == null)
                    cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDes", _itmDes);

                if (_itmRem == null)
                    cm.Parameters.AddWithValue("@ItmRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmRem", _itmRem);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                if (_cSGVendorKey == null)
                    cm.Parameters.AddWithValue("@CSGVendorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSGVendorKey", _cSGVendorKey);

                if (_cSGVendorID == null)
                    cm.Parameters.AddWithValue("@CSGVendorID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSGVendorID", _cSGVendorID);

                if (_industryPN == null)
                    cm.Parameters.AddWithValue("@IndustryPN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IndustryPN", _industryPN);

                if (_sku1 == null)
                    cm.Parameters.AddWithValue("@Sku1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku1", _sku1);

                if (_sku2 == null)
                    cm.Parameters.AddWithValue("@Sku2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku2", _sku2);

                if (_catKey1 == null)
                    cm.Parameters.AddWithValue("@CatKey1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey1", _catKey1);

                if (_catID1 == null)
                    cm.Parameters.AddWithValue("@CatID1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID1", _catID1);

                if (_catKey2 == null)
                    cm.Parameters.AddWithValue("@CatKey2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey2", _catKey2);

                if (_catID2 == null)
                    cm.Parameters.AddWithValue("@CatID2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID2", _catID2);

                if (_catKey3 == null)
                    cm.Parameters.AddWithValue("@CatKey3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey3", _catKey3);

                if (_catID3 == null)
                    cm.Parameters.AddWithValue("@CatID3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID3", _catID3);

                if (_catKey4 == null)
                    cm.Parameters.AddWithValue("@CatKey4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey4", _catKey4);

                if (_catID4 == null)
                    cm.Parameters.AddWithValue("@CatID4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID4", _catID4);

                if (_catKey5 == null)
                    cm.Parameters.AddWithValue("@CatKey5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatKey5", _catKey5);

                if (_catID5 == null)
                    cm.Parameters.AddWithValue("@CatID5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID5", _catID5);

                if (_brandkey == null)
                    cm.Parameters.AddWithValue("@Brandkey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Brandkey", _brandkey);

                if (_brandID == null)
                    cm.Parameters.AddWithValue("@BrandID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandID", _brandID);

                if (_model == null)
                    cm.Parameters.AddWithValue("@Model", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Model", _model);

                if (_iNClass == null)
                    cm.Parameters.AddWithValue("@INClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INClass", _iNClass);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

                if (_costMethod == null)
                    cm.Parameters.AddWithValue("@CostMethod", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostMethod", _costMethod);

                if (_branchKey == null)
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                if (_deptKey == null)
                    cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeptKey", _deptKey);

                if (_accICKey == null)
                    cm.Parameters.AddWithValue("@AccICKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccICKey", _accICKey);

                if (_accICID == null)
                    cm.Parameters.AddWithValue("@AccICID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccICID", _accICID);

                if (_accDSICKey == null)
                    cm.Parameters.AddWithValue("@AccDSICKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDSICKey", _accDSICKey);

                if (_accDSICID == null)
                    cm.Parameters.AddWithValue("@AccDSICID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDSICID", _accDSICID);


                if (_accINKey == null)
                    cm.Parameters.AddWithValue("@AccINKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccINKey", _accINKey);

                if (_accINID == null)
                    cm.Parameters.AddWithValue("@AccINID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccINID", _accINID);

                if (_accPHKey == null)
                    cm.Parameters.AddWithValue("@AccPHKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccPHKey", _accPHKey);

                if (_accPHID == null)
                    cm.Parameters.AddWithValue("@AccPHID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccPHID", _accPHID);

                if (_bUOMKey == null)
                    cm.Parameters.AddWithValue("@BUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUOMKey", _bUOMKey);

                if (_buomid == null)
                    cm.Parameters.AddWithValue("@Buomid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Buomid", _buomid);

                if (_qtyStock == null)
                    cm.Parameters.AddWithValue("@QtyStock", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyStock", _qtyStock);

                if (_qtyMin == null)
                    cm.Parameters.AddWithValue("@QtyMin", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyMin", _qtyMin);

                if (_qtyMax == null)
                    cm.Parameters.AddWithValue("@QtyMax", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyMax", _qtyMax);

                if (_qtyReOrder == null)
                    cm.Parameters.AddWithValue("@QtyReOrder", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyReOrder", _qtyReOrder);

                if (_salesWrtyYr == null)
                    cm.Parameters.AddWithValue("@SalesWrtyYr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SalesWrtyYrr", _salesWrtyYr);

                if (_purchaseWrtyYr == null)
                    cm.Parameters.AddWithValue("@PurchaseWrtyYr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseWrtyYr", _purchaseWrtyYr);

                if (_defLocSale == null)
                    cm.Parameters.AddWithValue("@DefLocSale", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefLocSale", _defLocSale);

                if (_defLocPurchase == null)
                    cm.Parameters.AddWithValue("@DefLocPurchase", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefLocPurchase", _defLocPurchase);

                if (_leadTimeInDays == null)
                    cm.Parameters.AddWithValue("@LeadTimeInDays", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LeadTimeInDays", _leadTimeInDays);

                if (_costLatest == null)
                    cm.Parameters.AddWithValue("@CostLatest", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLatest", _costLatest);

                if (_costLatestDate == null)
                    cm.Parameters.AddWithValue("@CostLatestDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLatestDate", _costLatestDate.Value);

                if (_costLanded == null)
                    cm.Parameters.AddWithValue("@CostLanded", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLanded", _costLanded);

                if (_costLandedDate == null)
                    cm.Parameters.AddWithValue("@CostLandedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLandedDate", _costLandedDate.Value);

                if (_costAvg == null)
                    cm.Parameters.AddWithValue("@CostAvg", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostAvg", _costAvg);

                if (_controlPriceH == null)
                    cm.Parameters.AddWithValue("@ControlPriceH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ControlPriceH", _controlPriceH);

                if (_openBalCost == null)
                    cm.Parameters.AddWithValue("@OpenBalCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBalCost", _openBalCost);

                if (_openBalQty == null)
                    cm.Parameters.AddWithValue("@OpenBalQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBalQty", _openBalQty);

                if (_openBalAmtH == null)
                    cm.Parameters.AddWithValue("@OpenBalAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBalAmtH", _openBalAmtH);

                if (_taxable == null)
                    cm.Parameters.AddWithValue("@Taxable", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Taxable", _taxable);

                //added by nnt on 26-feb-2019
                if (_taxable == null)
                    cm.Parameters.AddWithValue("@Scan", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Scan", _scan);

                //add by nnt on 26-feb-2019

                if (_commisionType == null)
                    cm.Parameters.AddWithValue("@CommisionType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CommisionType", _commisionType);

                if (_bOMType == null)
                    cm.Parameters.AddWithValue("@BOMType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMType", _bOMType);

                if (_bOMMultiplier == null)
                    cm.Parameters.AddWithValue("@BOMMultiplier", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMMultiplier", _bOMMultiplier);

                if (_bOMOverHeadKey == null)
                    cm.Parameters.AddWithValue("@BOMOverHeadKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMOverHeadKey", _bOMOverHeadKey);

                if (_defaultExpDate == null)
                    cm.Parameters.AddWithValue("@DefaultExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefaultExpDate", _defaultExpDate);

                if (_colorKey == null)
                    cm.Parameters.AddWithValue("@ColorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorKey", _colorKey);

                if (_colorID == null)
                    cm.Parameters.AddWithValue("@ColorID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorID", _colorID);

                if (_scaleKey == null)
                    cm.Parameters.AddWithValue("@ScaleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleKey", _scaleKey);

                if (_scaleID == null)
                    cm.Parameters.AddWithValue("@ScaleID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleID", _scaleID);

                if (_scaleSizeNum == null)
                    cm.Parameters.AddWithValue("@ScaleSizeNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleSizeNum", _scaleSizeNum);

                if (_scaleSize == null)
                    cm.Parameters.AddWithValue("@ScaleSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleSize", _scaleSize);

                if (_weightNet == null)
                    cm.Parameters.AddWithValue("@WeightNet", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightNet", _weightNet);

                if (_weightGross == null)
                    cm.Parameters.AddWithValue("@WeightGross", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightGross", _weightGross);

                if (_weightUOMKey == null)
                    cm.Parameters.AddWithValue("@WeightUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightUOMKey", _weightUOMKey);

                if (_weightUOMID == null)
                    cm.Parameters.AddWithValue("@WeightUOMID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightUOMID", _weightUOMID);

                if (_iNLength == null)
                    cm.Parameters.AddWithValue("@INLength", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INLength", _iNLength);

                if (_iNWidth == null)
                    cm.Parameters.AddWithValue("@INWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INWidth", _iNWidth);

                if (_iNHeight == null)
                    cm.Parameters.AddWithValue("@INHeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INHeight", _iNHeight);

                if (_iNVolume == null)
                    cm.Parameters.AddWithValue("@INVolume", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INVolume", _iNVolume);

                if (_iNPacking == null)
                    cm.Parameters.AddWithValue("@INPacking", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPacking", _iNPacking);

                if (_iNAttachment == null)
                    cm.Parameters.AddWithValue("@INAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INAttachment", _iNAttachment);

                if (_stdPackSize == null)
                    cm.Parameters.AddWithValue("@StdPackSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackSize", _stdPackSize);

                if (_stdPackWeight == null)
                    cm.Parameters.AddWithValue("@StdPackWeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackWeight", _stdPackWeight);

                if (_stdPackLength == null)
                    cm.Parameters.AddWithValue("@StdPackLength", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackLength", _stdPackLength);

                if (_stdPackWidth == null)
                    cm.Parameters.AddWithValue("@StdPackWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackWidth", _stdPackWidth);

                if (_stdPackHeight == null)
                    cm.Parameters.AddWithValue("@StdPackHeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackHeight", _stdPackHeight);

                if (_saleUOM == null)
                    cm.Parameters.AddWithValue("@SaleUOM", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleUOM", _saleUOM);

                if (_saleUOMRate == null)
                    cm.Parameters.AddWithValue("@SaleUOMRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleUOMRate", _saleUOMRate);

                if (_purchaseUOM == null)
                    cm.Parameters.AddWithValue("@PurchaseUOM", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseUOM", _purchaseUOM);

                if (_purchaseUOMRate == null)
                    cm.Parameters.AddWithValue("@PurchaseUOMRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseUOMRate", _purchaseUOMRate);

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

                if (_custom6 == null)
                    cm.Parameters.AddWithValue("@Custom6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom6", _custom6);

                if (_custom7 == null)
                    cm.Parameters.AddWithValue("@Custom7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom7", _custom7);

                if (_custom8 == null)
                    cm.Parameters.AddWithValue("@Custom8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom8", _custom8);

                if (_custom9 == null)
                    cm.Parameters.AddWithValue("@Custom9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom9", _custom9);

                if (_custom10 == null)
                    cm.Parameters.AddWithValue("@Custom10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom10", _custom10);
                cm.Parameters.AddWithValue("@BlockPurchase", _blockPurchase);

                if (_countryID == null)
                    cm.Parameters.AddWithValue("@CountryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CountryID", _countryID);

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                itmKey = (int)cm.Parameters["@NewItmKey"].Value;
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }            
               
            }// Already close and dispose sql connection.            
        }
        internal bool Insert(SqlConnection cn, out int? itmKey)
        {
            itmKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", itmKey);

                //if (_itmKey == null)
                //    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                //else
                cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_itmType == null)
                    cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmType", _itmType);

                if (_masterItmKey == null)
                    cm.Parameters.AddWithValue("@MasterItmKey", 0);
                else
                    cm.Parameters.AddWithValue("@MasterItmKey", _masterItmKey);

                if (_masterItmID == null)
                    cm.Parameters.AddWithValue("@MasterItmID", string.Empty);
                else
                    cm.Parameters.AddWithValue("@MasterItmID", _masterItmID);

                if (_masterItmType == null)
                    cm.Parameters.AddWithValue("@MasterItmType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MasterItmType", _masterItmType);

                if (_substituteItmKey == null)
                    cm.Parameters.AddWithValue("@SubstituteItmKey",0);
                else
                    cm.Parameters.AddWithValue("@SubstituteItmKey", _substituteItmKey);

                if (_substituteItmID == null)
                    cm.Parameters.AddWithValue("@SubstituteItmID", 0);
                else
                    cm.Parameters.AddWithValue("@SubstituteItmID", _substituteItmID);

                if (_itmID == null)
                    cm.Parameters.AddWithValue("@ItmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmID", _itmID);

                //added by thettm on 06 jul 2017(start)
                if (_mapitmID == null)
                    cm.Parameters.AddWithValue("@MapitmID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@MapitmID", _mapitmID);
                //added by thettm on 06 jul 2017(end)

                if (_itmDes == null)
                    cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmDes", _itmDes);

                if (_itmRem == null)
                    cm.Parameters.AddWithValue("@ItmRem", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmRem", _itmRem);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", 0);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", 01);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                if (_cSGVendorKey == null)
                    cm.Parameters.AddWithValue("@CSGVendorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSGVendorKey", _cSGVendorKey);

                if (_cSGVendorID == null)
                    cm.Parameters.AddWithValue("@CSGVendorID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSGVendorID", _cSGVendorID);

                if (_industryPN == null)
                    cm.Parameters.AddWithValue("@IndustryPN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@IndustryPN", _industryPN);

                if (_sku1 == null)
                    cm.Parameters.AddWithValue("@Sku1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku1", _sku1);

                if (_sku2 == null)
                    cm.Parameters.AddWithValue("@Sku2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Sku2", _sku2);

                if (_catKey1 == null)
                    cm.Parameters.AddWithValue("@CatKey1", 0);
                else
                    cm.Parameters.AddWithValue("@CatKey1", _catKey1);

                if (_catID1 == null)
                    cm.Parameters.AddWithValue("@CatID1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID1", _catID1);

                if (_catKey2 == null)
                    cm.Parameters.AddWithValue("@CatKey2", 0);
                else
                    cm.Parameters.AddWithValue("@CatKey2", _catKey2);

                if (_catID2 == null)
                    cm.Parameters.AddWithValue("@CatID2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID2", _catID2);

                if (_catKey3 == null)
                    cm.Parameters.AddWithValue("@CatKey3", 0);
                else
                    cm.Parameters.AddWithValue("@CatKey3", _catKey3);

                if (_catID3 == null)
                    cm.Parameters.AddWithValue("@CatID3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID3", _catID3);

                if (_catKey4 == null)
                    cm.Parameters.AddWithValue("@CatKey4", 0);
                else
                    cm.Parameters.AddWithValue("@CatKey4", _catKey4);

                if (_catID4 == null)
                    cm.Parameters.AddWithValue("@CatID4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID4", _catID4);

                if (_catKey5 == null)
                    cm.Parameters.AddWithValue("@CatKey5", 0);
                else
                    cm.Parameters.AddWithValue("@CatKey5", _catKey5);

                if (_catID5 == null)
                    cm.Parameters.AddWithValue("@CatID5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CatID5", _catID5);

                if (_brandkey == null)
                    cm.Parameters.AddWithValue("@Brandkey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Brandkey", _brandkey);

                if (_brandID == null)
                    cm.Parameters.AddWithValue("@BrandID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BrandID", _brandID);

                if (_model == null)
                    cm.Parameters.AddWithValue("@Model", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Model", _model);

                if (_iNClass == null)
                    cm.Parameters.AddWithValue("@INClass", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INClass", _iNClass);

                if (_inactive == null)
                    cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Inactive", _inactive);

                //added by thettm on 23-oct-2017(start)
                if (_lotTracking == null)
                    cm.Parameters.AddWithValue("@LotTracking", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LotTracking", _lotTracking);
                if (_serialTracking == null)
                    cm.Parameters.AddWithValue("@SerialTracking", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialTracking", _serialTracking);               
                //added by thettm on 23-oct-2017(end)

                //added by nnt on 26-feb-2019(start)
                if (_scan == null)
                    cm.Parameters.AddWithValue("@Scan", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Scan", _scan);                
                //added by nnt on 26-feb-2019(end)

                if (_costMethod == null)
                    cm.Parameters.AddWithValue("@CostMethod", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostMethod", _costMethod);

                if (_branchKey == null)
                    cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                if (_deptKey == null)
                    cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DeptKey", _deptKey);

                if (_accICKey == null)
                    cm.Parameters.AddWithValue("@AccICKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccICKey", _accICKey);
                
                if (_accICID == null)
                    cm.Parameters.AddWithValue("@AccICID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccICID", _accICID);

                if (_accINKey == null)
                    cm.Parameters.AddWithValue("@AccINKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccINKey", _accINKey);

                if (_accINID == null)
                    cm.Parameters.AddWithValue("@AccINID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccINID", _accINID);

                if (_accPHKey == null)
                    cm.Parameters.AddWithValue("@AccPHKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccPHKey", _accPHKey);

                if (_accPHID == null)
                    cm.Parameters.AddWithValue("@AccPHID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccPHID", _accPHID);

                if (_accDSICKey == null)
                    cm.Parameters.AddWithValue("@AccDSICKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDSICKey", _accDSICKey);

                if (_accDSICID == null)
                    cm.Parameters.AddWithValue("@AccDSICID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDSICID", _accDSICID);

                if (_accDSPHKey == null)
                    cm.Parameters.AddWithValue("@AccDSPHKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDSPHKey", _accDSPHKey);

                if (_accDSPHID == null)
                    cm.Parameters.AddWithValue("@AccDSPHID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccDSPHID", _accDSPHID);


                if (_bUOMKey == null)
                    cm.Parameters.AddWithValue("@BUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUOMKey", _bUOMKey);

                if (_buomid == null)
                    cm.Parameters.AddWithValue("@Buomid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Buomid", _buomid);

                if (_qtyStock == null)
                    cm.Parameters.AddWithValue("@QtyStock", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyStock", _qtyStock);

                if (_qtyMin == null)
                    cm.Parameters.AddWithValue("@QtyMin", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyMin", _qtyMin);

                if (_qtyMax == null)
                    cm.Parameters.AddWithValue("@QtyMax", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyMax", _qtyMax);

                if (_qtyReOrder == null)
                    cm.Parameters.AddWithValue("@QtyReOrder", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyReOrder", _qtyReOrder);

                if (_salesWrtyYr == null)
                    cm.Parameters.AddWithValue("@SalesWrtyYr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SalesWrtyYr", _salesWrtyYr);
                if (_purchaseWrtyYr == null)
                    cm.Parameters.AddWithValue("@PurchaseWrtyYr", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseWrtyYr", _purchaseWrtyYr);


                if (_defLocSale == null)
                    cm.Parameters.AddWithValue("@DefLocSale", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefLocSale", _defLocSale);

                if (_defLocPurchase == null)
                    cm.Parameters.AddWithValue("@DefLocPurchase", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefLocPurchase", _defLocPurchase);

                if (_leadTimeInDays == null)
                    cm.Parameters.AddWithValue("@LeadTimeInDays", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LeadTimeInDays", _leadTimeInDays);

                if (_costLatest == null)
                    cm.Parameters.AddWithValue("@CostLatest", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLatest", _costLatest);

                if (_costLatestDate == null)
                    cm.Parameters.AddWithValue("@CostLatestDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLatestDate", _costLatestDate.Value);

                if (_costLanded == null)
                    cm.Parameters.AddWithValue("@CostLanded", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLanded", _costLanded);

                if (_costLandedDate == null)
                    cm.Parameters.AddWithValue("@CostLandedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostLandedDate", _costLandedDate.Value);

                if (_costAvg == null)
                    cm.Parameters.AddWithValue("@CostAvg", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CostAvg", _costAvg);

                if (_controlPriceH == null)
                    cm.Parameters.AddWithValue("@ControlPriceH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ControlPriceH", _controlPriceH);

                if (_openBalCost == null)
                    cm.Parameters.AddWithValue("@OpenBalCost", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBalCost", _openBalCost);

                if (_openBalQty == null)
                    cm.Parameters.AddWithValue("@OpenBalQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBalQty", _openBalQty);

                if (_openBalAmtH == null)
                    cm.Parameters.AddWithValue("@OpenBalAmtH", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@OpenBalAmtH", _openBalAmtH);

                if (_taxable == null)
                    cm.Parameters.AddWithValue("@Taxable", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Taxable", _taxable);

                if (_commisionType == null)
                    cm.Parameters.AddWithValue("@CommisionType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CommisionType", _commisionType);

                if (_bOMType == null)
                    cm.Parameters.AddWithValue("@BOMType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMType", _bOMType);

                if (_bOMMultiplier == null)
                    cm.Parameters.AddWithValue("@BOMMultiplier", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMMultiplier", _bOMMultiplier);

                if (_bOMOverHeadKey == null)
                    cm.Parameters.AddWithValue("@BOMOverHeadKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BOMOverHeadKey", _bOMOverHeadKey);

                if (_defaultExpDate == null)
                    cm.Parameters.AddWithValue("@DefaultExpDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DefaultExpDate", _defaultExpDate);

                if (_colorKey == null)
                    cm.Parameters.AddWithValue("@ColorKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorKey", _colorKey);

                if (_colorID == null)
                    cm.Parameters.AddWithValue("@ColorID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColorID", _colorID);

                if (_scaleKey == null)
                    cm.Parameters.AddWithValue("@ScaleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleKey", _scaleKey);

                if (_scaleID == null)
                    cm.Parameters.AddWithValue("@ScaleID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleID", _scaleID);

                if (_scaleSizeNum == null)
                    cm.Parameters.AddWithValue("@ScaleSizeNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleSizeNum", _scaleSizeNum);

                if (_scaleSize == null)
                    cm.Parameters.AddWithValue("@ScaleSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleSize", _scaleSize);

                if (_weightNet == null)
                    cm.Parameters.AddWithValue("@WeightNet", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightNet", _weightNet);

                if (_weightGross == null)
                    cm.Parameters.AddWithValue("@WeightGross", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightGross", _weightGross);

                if (_weightUOMKey == null)
                    cm.Parameters.AddWithValue("@WeightUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightUOMKey", _weightUOMKey);

                if (_weightUOMID == null)
                    cm.Parameters.AddWithValue("@WeightUOMID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@WeightUOMID", _weightUOMID);

                if (_iNLength == null)
                    cm.Parameters.AddWithValue("@INLength", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INLength", _iNLength);

                if (_iNWidth == null)
                    cm.Parameters.AddWithValue("@INWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INWidth", _iNWidth);

                if (_iNHeight == null)
                    cm.Parameters.AddWithValue("@INHeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INHeight", _iNHeight);

                if (_iNVolume == null)
                    cm.Parameters.AddWithValue("@INVolume", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INVolume", _iNVolume);

                if (_iNPacking == null)
                    cm.Parameters.AddWithValue("@INPacking", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPacking", _iNPacking);

                if (_iNAttachment == null)
                    cm.Parameters.AddWithValue("@INAttachment", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INAttachment", _iNAttachment);

                if (_stdPackSize == null)
                    cm.Parameters.AddWithValue("@StdPackSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackSize", _stdPackSize);

                if (_stdPackWeight == null)
                    cm.Parameters.AddWithValue("@StdPackWeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackWeight", _stdPackWeight);

                if (_stdPackLength == null)
                    cm.Parameters.AddWithValue("@StdPackLength", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackLength", _stdPackLength);

                if (_stdPackWidth == null)
                    cm.Parameters.AddWithValue("@StdPackWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackWidth", _stdPackWidth);

                if (_stdPackHeight == null)
                    cm.Parameters.AddWithValue("@StdPackHeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StdPackHeight", _stdPackHeight);

                if (_saleUOM == null)
                    cm.Parameters.AddWithValue("@SaleUOM", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleUOM", _saleUOM);

                if (_saleUOMRate == null)
                    cm.Parameters.AddWithValue("@SaleUOMRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SaleUOMRate", _saleUOMRate);

                if (_purchaseUOM == null)
                    cm.Parameters.AddWithValue("@PurchaseUOM", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseUOM", _purchaseUOM);

                if (_purchaseUOMRate == null)
                    cm.Parameters.AddWithValue("@PurchaseUOMRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PurchaseUOMRate", _purchaseUOMRate);

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

                if (_custom6 == null)
                    cm.Parameters.AddWithValue("@Custom6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom6", _custom6);

                if (_custom7 == null)
                    cm.Parameters.AddWithValue("@Custom7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom7", _custom7);

                if (_custom8 == null)
                    cm.Parameters.AddWithValue("@Custom8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom8", _custom8);

                if (_custom9 == null)
                    cm.Parameters.AddWithValue("@Custom9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom9", _custom9);

                if (_custom10 == null)
                    cm.Parameters.AddWithValue("@Custom10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom10", _custom10);
                cm.Parameters.AddWithValue("@BlockPurchase", _blockPurchase);

                if (_countryID == null)
                    cm.Parameters.AddWithValue("@CountryID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CountryID", _countryID);

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();



                itmKey = (int)cm.Parameters["@NewItmKey"].Value;
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }
        internal bool InsertOpeningLedger(SqlConnection cn, int? itmKey)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandTimeout = 0;
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_AddUpdateOpeningLedger";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@ItmKey", itmKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                  
                
                cm.ExecuteNonQuery();
                
             
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql connection.
            
        }
        //internal bool InsertOpeningDetailLedger(SqlConnection cn, int? itmKey, int option, string XMlData,ref decimal AvgCost)
        //{
        //    // Using existing sql connection.
        //    using (SqlCommand cm = cn.CreateCommand())
        //    {
        //        cm.CommandType = CommandType.StoredProcedure;
        //        cm.CommandText = "IN_ConfirmOpeningLedger";

        //        cm.Parameters.AddWithValue("@Option", option);
        //        cm.Parameters.AddWithValue("@ItmKey", itmKey);
        //        cm.Parameters.AddWithValue("@XMLData", XMlData);
        //        cm.Parameters.AddWithValue("@RetValue", 0);
        //        SqlParameter pAvgCost = new SqlParameter("@AvgCost", SqlDbType.Decimal, sizeof(decimal), ParameterDirection.Output, true, (byte)2, (byte)4, "", DataRowVersion.Default, 0);
        //        cm.Parameters.Add(pAvgCost);  
              
        //        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
        //        cm.Parameters["@AvgCost"].Direction = ParameterDirection.Output;
        //        cm.ExecuteNonQuery();

        //        AvgCost = GFunc.NEDec(cm.Parameters["@AvgCost"].Value,0);
        //        // Check Return Value -- Changed By Richard
        //        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //            return true;
        //        else
        //            return false;

        //    }// Already close and dispose sql connection.            
        //}
        internal bool InsertCosBatchOpeningDetailLedger(SqlConnection cn, int? itmKey, int option, string XMlData)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "IN_ConfirmCOSBatchOpeningLedger";

                cm.Parameters.AddWithValue("@Option", option);
                cm.Parameters.AddWithValue("@ItmKey", itmKey);
                cm.Parameters.AddWithValue("@XMLData", XMlData);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
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
            try
            {
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
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;
            
            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                   
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.Parameters.AddWithValue("@NewItmKey", 0);

                    if (_itmKey == null)
                        cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                    if (_itmType == null)
                        cm.Parameters.AddWithValue("@ItmType", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ItmType", _itmType);

                    if (_masterItmKey == null)
                        cm.Parameters.AddWithValue("@MasterItmKey",0);
                    else
                        cm.Parameters.AddWithValue("@MasterItmKey", _masterItmKey);

                    if (_masterItmID == null)
                        cm.Parameters.AddWithValue("@MasterItmID", string.Empty);
                    else
                        cm.Parameters.AddWithValue("@MasterItmID", _masterItmID);

                    if (_masterItmType == null)
                        cm.Parameters.AddWithValue("@MasterItmType", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@MasterItmType", _masterItmType);

                    if (_substituteItmKey == null)
                        cm.Parameters.AddWithValue("@SubstituteItmKey", 0);
                    else
                        cm.Parameters.AddWithValue("@SubstituteItmKey", _substituteItmKey);

                    if (_substituteItmID == null)
                        cm.Parameters.AddWithValue("@SubstituteItmID", string.Empty);
                    else
                        cm.Parameters.AddWithValue("@SubstituteItmID", _substituteItmID);

                    if (_itmID == null)
                        cm.Parameters.AddWithValue("@ItmID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ItmID", _itmID);

                    //added by thettm on 06 july 2017(start)
                    if (_mapitmID == null)
                        cm.Parameters.AddWithValue("@MapitmID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@MapitmID", _mapitmID);
                    //added by thettm on 06 july 2017(end)

                    //added by thettm on 23-oct-2017(start)
                    if (_lotTracking == null)
                        cm.Parameters.AddWithValue("@LotTracking", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@LotTracking", _lotTracking);
                    if (_serialTracking == null)
                        cm.Parameters.AddWithValue("@SerialTracking", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@SerialTracking", _serialTracking);                   
                    //added by thettm on 23-oct-2017(end)

                    //added by nnt on 26-Feb-2019(start)
                    if (_scan == null)
                        cm.Parameters.AddWithValue("@Scan", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Scan", _scan);
                    //added by nnt on 26-Feb-2019(end)

                    if (_itmDes == null)
                        cm.Parameters.AddWithValue("@ItmDes", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ItmDes", _itmDes);

                    if (_itmRem == null)
                        cm.Parameters.AddWithValue("@ItmRem", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ItmRem", _itmRem);

                    if (_accessLevel == null)
                        cm.Parameters.AddWithValue("@AccessLevel", 0);
                    else
                        cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                    if (_accessGroup == null)
                        cm.Parameters.AddWithValue("@AccessGroup", 0);
                    else
                        cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                    if (_cSGVendorKey == null)
                        cm.Parameters.AddWithValue("@CSGVendorKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CSGVendorKey", _cSGVendorKey);

                    if (_cSGVendorID == null)
                        cm.Parameters.AddWithValue("@CSGVendorID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CSGVendorID", _cSGVendorID);

                    if (_industryPN == null)
                        cm.Parameters.AddWithValue("@IndustryPN", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@IndustryPN", _industryPN);

                    if (_sku1 == null)
                        cm.Parameters.AddWithValue("@Sku1", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Sku1", _sku1);

                    if (_sku2 == null)
                        cm.Parameters.AddWithValue("@Sku2", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Sku2", _sku2);

                    if (_catKey1 == null)
                        cm.Parameters.AddWithValue("@CatKey1", 0);
                    else
                        cm.Parameters.AddWithValue("@CatKey1", _catKey1);

                    if (_catID1 == null)
                        cm.Parameters.AddWithValue("@CatID1", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CatID1", _catID1);

                    if (_catKey2 == null)
                        cm.Parameters.AddWithValue("@CatKey2", 0);
                    else
                        cm.Parameters.AddWithValue("@CatKey2", _catKey2);

                    if (_catID2 == null)
                        cm.Parameters.AddWithValue("@CatID2", 0);
                    else
                        cm.Parameters.AddWithValue("@CatID2", _catID2);

                    if (_catKey3 == null)
                        cm.Parameters.AddWithValue("@CatKey3", 0);
                    else
                        cm.Parameters.AddWithValue("@CatKey3", _catKey3);

                    if (_catID3 == null)
                        cm.Parameters.AddWithValue("@CatID3", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CatID3", _catID3);

                    if (_catKey4 == null)
                        cm.Parameters.AddWithValue("@CatKey4",0);
                    else
                        cm.Parameters.AddWithValue("@CatKey4", _catKey4);

                    if (_catID4 == null)
                        cm.Parameters.AddWithValue("@CatID4", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CatID4", _catID4);

                    if (_catKey5 == null)
                        cm.Parameters.AddWithValue("@CatKey5", 0);
                    else
                        cm.Parameters.AddWithValue("@CatKey5", _catKey5);

                    if (_catID5 == null)
                        cm.Parameters.AddWithValue("@CatID5", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CatID5", _catID5);

                    if (_brandkey == null)
                        cm.Parameters.AddWithValue("@Brandkey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Brandkey", _brandkey);

                    if (_brandID == null)
                        cm.Parameters.AddWithValue("@BrandID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@BrandID", _brandID);

                    if (_model == null)
                        cm.Parameters.AddWithValue("@Model", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Model", _model);

                    if (_iNClass == null)
                        cm.Parameters.AddWithValue("@INClass", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@INClass", _iNClass);

                    if (_inactive == null)
                        cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Inactive", _inactive);

                    if (_costMethod == null)
                        cm.Parameters.AddWithValue("@CostMethod", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CostMethod", _costMethod);

                    if (_branchKey == null)
                        cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@BranchKey", _branchKey);

                    if (_deptKey == null)
                        cm.Parameters.AddWithValue("@DeptKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DeptKey", _deptKey);

                    if (_accICKey == null)
                        cm.Parameters.AddWithValue("@AccICKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccICKey", _accICKey);

                    if (_accICID == null)
                        cm.Parameters.AddWithValue("@AccICID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccICID", _accICID);

                    if (_accINKey == null)
                        cm.Parameters.AddWithValue("@AccINKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccINKey", _accINKey);

                    if (_accINID == null)
                        cm.Parameters.AddWithValue("@AccINID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccINID", _accINID);

                    if (_accPHKey == null)
                        cm.Parameters.AddWithValue("@AccPHKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccPHKey", _accPHKey);

                    if (_accPHID == null)
                        cm.Parameters.AddWithValue("@AccPHID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccPHID", _accPHID);
                 
                    if (_accDSICKey == null)
                        cm.Parameters.AddWithValue("@AccDSICKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccDSICKey", _accDSICKey);

                    if (_accDSICID == null)
                        cm.Parameters.AddWithValue("@AccDSICID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccDSICID", _accDSICID);

                    if (_accDSPHKey == null)
                        cm.Parameters.AddWithValue("@AccDSPHKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccDSPHKey", _accDSPHKey);

                    if (_accDSPHID == null)
                        cm.Parameters.AddWithValue("@AccDSPHID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@AccDSPHID", _accDSPHID);

                    if (_bUOMKey == null)
                        cm.Parameters.AddWithValue("@BUOMKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@BUOMKey", _bUOMKey);

                    if (_buomid == null)
                        cm.Parameters.AddWithValue("@Buomid", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Buomid", _buomid);

                    if (_qtyStock == null)
                        cm.Parameters.AddWithValue("@QtyStock", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@QtyStock", _qtyStock);

                    if (_qtyMin == null)
                        cm.Parameters.AddWithValue("@QtyMin", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@QtyMin", _qtyMin);

                    if (_qtyMax == null)
                        cm.Parameters.AddWithValue("@QtyMax", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@QtyMax", _qtyMax);

                    if (_qtyReOrder == null)
                        cm.Parameters.AddWithValue("@QtyReOrder", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@QtyReOrder", _qtyReOrder);

                    if (_salesWrtyYr == null)
                        cm.Parameters.AddWithValue("@SalesWrtyYr", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@SalesWrtyYr", _salesWrtyYr);

                    if (_purchaseWrtyYr == null)
                        cm.Parameters.AddWithValue("@PurchaseWrtyYr", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@PurchaseWrtyYr", _purchaseWrtyYr);

                    if (_defLocSale == null)
                        cm.Parameters.AddWithValue("@DefLocSale", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DefLocSale", _defLocSale);

                    if (_defLocPurchase == null)
                        cm.Parameters.AddWithValue("@DefLocPurchase", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DefLocPurchase", _defLocPurchase);

                    if (_leadTimeInDays == null)
                        cm.Parameters.AddWithValue("@LeadTimeInDays", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@LeadTimeInDays", _leadTimeInDays);

                    if (_costLatest == null)
                        cm.Parameters.AddWithValue("@CostLatest", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CostLatest", _costLatest);

                    if (_costLatestDate == null)
                        cm.Parameters.AddWithValue("@CostLatestDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CostLatestDate", _costLatestDate.Value);

                    if (_costLanded == null)
                        cm.Parameters.AddWithValue("@CostLanded", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CostLanded", _costLanded);

                    if (_costLandedDate == null)
                        cm.Parameters.AddWithValue("@CostLandedDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CostLandedDate", _costLandedDate.Value);

                    if (_costAvg == null)
                        cm.Parameters.AddWithValue("@CostAvg", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CostAvg", _costAvg);

                    if (_controlPriceH == null)
                        cm.Parameters.AddWithValue("@ControlPriceH", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ControlPriceH", _controlPriceH);

                    if (_openBalCost == null)
                        cm.Parameters.AddWithValue("@OpenBalCost", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@OpenBalCost", _openBalCost);

                    if (_openBalQty == null)
                        cm.Parameters.AddWithValue("@OpenBalQty", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@OpenBalQty", _openBalQty);

                    if (_openBalAmtH == null)
                        cm.Parameters.AddWithValue("@OpenBalAmtH", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@OpenBalAmtH", _openBalAmtH);

                    if (_taxable == null)
                        cm.Parameters.AddWithValue("@Taxable", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Taxable", _taxable);

                    if (_commisionType == null)
                        cm.Parameters.AddWithValue("@CommisionType", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CommisionType", _commisionType);

                    if (_bOMType == null)
                        cm.Parameters.AddWithValue("@BOMType", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@BOMType", _bOMType);

                    if (_bOMMultiplier == null)
                        cm.Parameters.AddWithValue("@BOMMultiplier", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@BOMMultiplier", _bOMMultiplier);

                    if (_bOMOverHeadKey == null)
                        cm.Parameters.AddWithValue("@BOMOverHeadKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@BOMOverHeadKey", _bOMOverHeadKey);

                    if (_defaultExpDate == null)
                        cm.Parameters.AddWithValue("@DefaultExpDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DefaultExpDate", _defaultExpDate);

                    if (_colorKey == null)
                        cm.Parameters.AddWithValue("@ColorKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ColorKey", _colorKey);

                    if (_colorID == null)
                        cm.Parameters.AddWithValue("@ColorID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ColorID", _colorID);

                    if (_scaleKey == null)
                        cm.Parameters.AddWithValue("@ScaleKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ScaleKey", _scaleKey);

                    if (_scaleID == null)
                        cm.Parameters.AddWithValue("@ScaleID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ScaleID", _scaleID);

                    if (_scaleSizeNum == null)
                        cm.Parameters.AddWithValue("@ScaleSizeNum", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ScaleSizeNum", _scaleSizeNum);

                    if (_scaleSize == null)
                        cm.Parameters.AddWithValue("@ScaleSize", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ScaleSize", _scaleSize);

                    if (_weightNet == null)
                        cm.Parameters.AddWithValue("@WeightNet", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WeightNet", _weightNet);

                    if (_weightGross == null)
                        cm.Parameters.AddWithValue("@WeightGross", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WeightGross", _weightGross);

                    if (_weightUOMKey == null)
                        cm.Parameters.AddWithValue("@WeightUOMKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WeightUOMKey", _weightUOMKey);

                    if (_weightUOMID == null)
                        cm.Parameters.AddWithValue("@WeightUOMID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@WeightUOMID", _weightUOMID);

                    if (_iNLength == null)
                        cm.Parameters.AddWithValue("@INLength", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@INLength", _iNLength);

                    if (_iNWidth == null)
                        cm.Parameters.AddWithValue("@INWidth", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@INWidth", _iNWidth);

                    if (_iNHeight == null)
                        cm.Parameters.AddWithValue("@INHeight", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@INHeight", _iNHeight);

                    if (_iNVolume == null)
                        cm.Parameters.AddWithValue("@INVolume", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@INVolume", _iNVolume);

                    if (_iNPacking == null)
                        cm.Parameters.AddWithValue("@INPacking", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@INPacking", _iNPacking);

                    if (_iNAttachment == null)
                        cm.Parameters.AddWithValue("@INAttachment", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@INAttachment", _iNAttachment);

                    if (_stdPackSize == null)
                        cm.Parameters.AddWithValue("@StdPackSize", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@StdPackSize", _stdPackSize);

                    if (_stdPackWeight == null)
                        cm.Parameters.AddWithValue("@StdPackWeight", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@StdPackWeight", _stdPackWeight);

                    if (_stdPackLength == null)
                        cm.Parameters.AddWithValue("@StdPackLength", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@StdPackLength", _stdPackLength);

                    if (_stdPackWidth == null)
                        cm.Parameters.AddWithValue("@StdPackWidth", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@StdPackWidth", _stdPackWidth);

                    if (_stdPackHeight == null)
                        cm.Parameters.AddWithValue("@StdPackHeight", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@StdPackHeight", _stdPackHeight);

                    if (_saleUOM == null)
                        cm.Parameters.AddWithValue("@SaleUOM", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@SaleUOM", _saleUOM);

                    if (_saleUOMRate == null)
                        cm.Parameters.AddWithValue("@SaleUOMRate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@SaleUOMRate", _saleUOMRate);

                    if (_purchaseUOM == null)
                        cm.Parameters.AddWithValue("@PurchaseUOM", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@PurchaseUOM", _purchaseUOM);

                    if (_purchaseUOMRate == null)
                        cm.Parameters.AddWithValue("@PurchaseUOMRate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@PurchaseUOMRate", _purchaseUOMRate);

                    if (_createDate == null)
                        cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                    if (_createUserKey == null)
                        cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                    if (_lastModifiedDate == null)
                        cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

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

                    if (_custom6 == null)
                        cm.Parameters.AddWithValue("@Custom6", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom6", _custom6);

                    if (_custom7 == null)
                        cm.Parameters.AddWithValue("@Custom7", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom7", _custom7);

                    if (_custom8 == null)
                        cm.Parameters.AddWithValue("@Custom8", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom8", _custom8);

                    if (_custom9 == null)
                        cm.Parameters.AddWithValue("@Custom9", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom9", _custom9);

                    if (_custom10 == null)
                        cm.Parameters.AddWithValue("@Custom10", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Custom10", _custom10);

                    cm.Parameters.AddWithValue("@BlockPurchase", _blockPurchase);

                    if (_countryID == null)
                        cm.Parameters.AddWithValue("@CountryID", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CountryID", _countryID);

                    cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                    {
                        throw new TAException(MsgID.Common.UpdateFail);
                    }            

                }// Already close and dispose sql connection.
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }
        internal bool UpdateOpeningLedger(SqlConnection cn)
        {
            bool retValue = false;
            
            try
            {
                // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTItm_AddUpdateOpeningLedger";

                    cm.Parameters.AddWithValue("@Option", 1);
                    if (_itmKey == null)
                        cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@ItmKey", _itmKey);   

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;                   

                                

                    cm.ExecuteNonQuery();

                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                    {
                        throw new TAException(MsgID.Common.UpdateFail);
                    }

                }// Already close and dispose sql connection.
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
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
                cm.CommandText = "MSTItm_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }
        internal bool DeleteOpeningLedger(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "IN_ConfirmOpeningDelete"; 
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@DelTran", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
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

        internal bool Validation(SqlConnection cn, Criteria criteria,  bool? isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_Validation";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@isNew", isNew);
              
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@ItmID", criteria._itemID);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;               
            }            
        }
        #endregion //Data Access - Validation

        #region Record Access Level

        internal bool AccessLevelUpdate(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_AccessLevelUpdate";

                cm.Parameters.AddWithValue("@Option", 1);


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_accessLevel == null)
                    cm.Parameters.AddWithValue("@AccessLevel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessLevel", _accessLevel);

                if (_accessGroup == null)
                    cm.Parameters.AddWithValue("@AccessGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AccessGroup", _accessGroup);

                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;


            }// Already close and dispose sql connection.            
        }
        internal bool CanAccessRecord(int? itmKey)
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
                    retValue = this.CanAccessRecord(cn, itmKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }
        internal bool CanAccessRecord(SqlConnection cn, int? itmKey)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECRecAccess_Check";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@Key", itmKey);                    

                cm.Parameters.AddWithValue("@UserAccessLevel", AppInfor.itemAccessLevel);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.CurrentUserKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }

        #endregion //Check Record Access Level

        private void Clear()
        {
            _itmKey = 0;
            _itmType = 100;
            _masterItmKey = 0;
            _masterItmType = 0;
            _substituteItmKey = 0;
            _itmID = string.Empty;
            //added by thettm on 06 jul 2017(start)
            _mapitmID = string.Empty;
            //added by thettm on 06 jul 2017(end)
            _itmDes = string.Empty;
            _itmRem = string.Empty;
            _accessLevel = 0;
            _accessGroup = 0;
            _cSGVendorKey = null;
            _cSGVendorID = string.Empty;
            _industryPN = string.Empty;
            _sku1 = string.Empty;
            _sku2 = string.Empty;
            _catKey1 = 0;
            _catID1 = string.Empty;
            _catKey2 = 0;
            _catID2 = string.Empty;
            _catKey3 = 0;
            _catID3 = string.Empty;
            _catKey4 = 0;
            _catID4 = string.Empty;
            _catKey5 = 0;
            _catID5 = string.Empty;
            _brandkey = null;
            _brandID = string.Empty;
            _model = string.Empty;
            _iNClass = string.Empty;
            _inactive = false;
            //added by thettm on 23-oct-2017(start)
            _lotTracking = false;
            _serialTracking = false;
            _certiLink = string.Empty;
            //added by thettm on 23-oct-2017(end)

            //added by nnt on 26-feb-2019(start)
            _scan = false;
            //added by thettm on 26-feb-2019(end)
            _costMethod = null;
            _branchKey = 0;
            _deptKey = 0;
            _accICKey = null;
            _accINKey = null;
            _accPHKey = null;
            _bUOMKey = null;
            _buomid = string.Empty;
            _masterItmID = string.Empty;
            _substituteItmID = string.Empty;
            _accICID = string.Empty;
            _accINID = string.Empty;
            _accPHID = string.Empty;
            _qtyStock = null;
            _qtyMin = null;
            _qtyMax = null;
            _qtyReOrder = null;
            _salesWrtyYr = null;
            _purchaseWrtyYr = null;
            _defLocSale = null;
            _defLocPurchase = null;
            _leadTimeInDays = null;
            _costLatest = null;
            _costLatestDate = null;
            _costLanded = null;
            _costLandedDate = null;
            _costAvg = null;
            _controlPriceH = null;
            _openBalCost = 0;
            _openBalQty = 0;
            _openBalAmtH = 0;
            _taxable = true;
            _commisionType = 0;
            _bOMType = 10;
            _bOMMultiplier = 1;
            _bOMOverHeadKey = null;
            _defaultExpDate = string.Empty;
            _colorKey = null;
            _colorID = string.Empty;
            _scaleKey = null;
            _scaleID = string.Empty;
            _scaleSizeNum = null;
            _scaleSize = string.Empty;
            _weightNet = 0;
            _weightGross = 0;
            _weightUOMKey = null;
            _weightUOMID = string.Empty;
            _iNLength = 0;
            _iNWidth = 0;
            _iNHeight = 0;
            _iNVolume = 0;
            _iNPacking = string.Empty;
            _iNAttachment = false;
            _stdPackSize = 0;
            _stdPackWeight = 0;
            _stdPackLength = 0;
            _stdPackWidth = 0;
            _stdPackHeight = 0;
            _saleUOM = string.Empty;
            _saleUOMRate = null;
            _purchaseUOM = string.Empty;
            _purchaseUOMRate = null;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;
            _custom4 = string.Empty;
            _custom5 = string.Empty;
            _custom6 = string.Empty;
            _custom7 = string.Empty;
            _custom8 = string.Empty;
            _custom9 = string.Empty;
            _custom10 = string.Empty;
            _countryID = string.Empty;

        }    
    
    }
}


