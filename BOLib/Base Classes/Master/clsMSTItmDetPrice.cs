

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
    public class MSTItmDetPrice : Csla.BusinessBase<MSTItmDetPrice>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _itmKey = null;
        internal string _error = string.Empty;
        internal decimal? _standardPrice1 =0;
        internal decimal? _standardPrice2 =0;
        internal decimal? _standardPrice3 =0;
        internal decimal? _standardPrice4 =0;
        internal decimal? _standardPrice5 =0;
        internal decimal? _standardPrice6 =0;
        internal decimal? _standardPrice7 =0;
        internal decimal? _standardPrice8 =0;
        internal decimal? _standardPrice9 =0;
        internal decimal? _standardPrice10 =0;
        internal decimal? _standardPrice11 =0;
        internal decimal? _standardPrice12 =0;
        internal decimal? _standardPrice13 =0;
        internal decimal? _standardPrice14 =0;
        internal decimal? _standardPrice15 =0;
        internal decimal? _price0101 =0;
        internal decimal? _price0102 =0;
        internal decimal? _price0103 =0;
        internal decimal? _price0104 =0;
        internal decimal? _price0105 =0;
        internal decimal? _price0106 =0;
        internal decimal? _price0107 =0;
        internal decimal? _price0108 =0;
        internal decimal? _price0109 =0;
        internal decimal? _price0110 =0;
        internal decimal? _price0111 =0;
        internal decimal? _price0112 =0;
        internal decimal? _price0113 =0;
        internal decimal? _price0114 =0;
        internal decimal? _price0115 =0;
        internal decimal? _price0201 =0;
        internal decimal? _price0202 =0;
        internal decimal? _price0203 =0;
        internal decimal? _price0204 =0;
        internal decimal? _price0205 =0;
        internal decimal? _price0206 =0;
        internal decimal? _price0207 =0;
        internal decimal? _price0208 =0;
        internal decimal? _price0209 =0;
        internal decimal? _price0210 =0;
        internal decimal? _price0211 =0;
        internal decimal? _price0212 =0;
        internal decimal? _price0213 =0;
        internal decimal? _price0214 =0;
        internal decimal? _price0215 =0;
        internal decimal? _price0301 =0;
        internal decimal? _price0302 =0;
        internal decimal? _price0303 =0;
        internal decimal? _price0304 =0;
        internal decimal? _price0305 =0;
        internal decimal? _price0306 =0;
        internal decimal? _price0307 =0;
        internal decimal? _price0308 =0;
        internal decimal? _price0309 =0;
        internal decimal? _price0310 =0;
        internal decimal? _price0311 =0;
        internal decimal? _price0312 =0;
        internal decimal? _price0313 =0;
        internal decimal? _price0314 =0;
        internal decimal? _price0315 =0;
        internal decimal? _price0401 =0;
        internal decimal? _price0402 =0;
        internal decimal? _price0403 =0;
        internal decimal? _price0404 =0;
        internal decimal? _price0405 =0;
        internal decimal? _price0406 =0;
        internal decimal? _price0407 =0;
        internal decimal? _price0408 =0;
        internal decimal? _price0409 =0;
        internal decimal? _price0410 =0;
        internal decimal? _price0411 =0;
        internal decimal? _price0412 =0;
        internal decimal? _price0413 =0;
        internal decimal? _price0414 =0;
        internal decimal? _price0415 =0;
        internal decimal? _price0501 =0;
        internal decimal? _price0502 =0;
        internal decimal? _price0503 =0;
        internal decimal? _price0504 =0;
        internal decimal? _price0505 =0;
        internal decimal? _price0506 =0;
        internal decimal? _price0507 =0;
        internal decimal? _price0508 =0;
        internal decimal? _price0509 =0;
        internal decimal? _price0510 =0;
        internal decimal? _price0511 =0;
        internal decimal? _price0512 =0;
        internal decimal? _price0513 =0;
        internal decimal? _price0514 =0;
        internal decimal? _price0515 =0;
        internal decimal? _price0601 =0;
        internal decimal? _price0602 =0;
        internal decimal? _price0603 =0;
        internal decimal? _price0604 =0;
        internal decimal? _price0605 =0;
        internal decimal? _price0606 =0;
        internal decimal? _price0607 =0;
        internal decimal? _price0608 =0;
        internal decimal? _price0609 =0;
        internal decimal? _price0610 =0;
        internal decimal? _price0611 =0;
        internal decimal? _price0612 =0;
        internal decimal? _price0613 =0;
        internal decimal? _price0614 =0;
        internal decimal? _price0615 =0;
        internal decimal? _price0701 =0;
        internal decimal? _price0702 =0;
        internal decimal? _price0703 =0;
        internal decimal? _price0704 =0;
        internal decimal? _price0705 =0;
        internal decimal? _price0706 =0;
        internal decimal? _price0707 =0;
        internal decimal? _price0708 =0;
        internal decimal? _price0709 =0;
        internal decimal? _price0710 =0;
        internal decimal? _price0711 =0;
        internal decimal? _price0712 =0;
        internal decimal? _price0713 =0;
        internal decimal? _price0714 =0;
        internal decimal? _price0715 =0;
        internal decimal? _price0801 =0;
        internal decimal? _price0802 =0;
        internal decimal? _price0803 =0;
        internal decimal? _price0804 =0;
        internal decimal? _price0805 =0;
        internal decimal? _price0806 =0;
        internal decimal? _price0807 =0;
        internal decimal? _price0808 =0;
        internal decimal? _price0809 =0;
        internal decimal? _price0810 =0;
        internal decimal? _price0811 =0;
        internal decimal? _price0812 =0;
        internal decimal? _price0813 =0;
        internal decimal? _price0814 =0;
        internal decimal? _price0815 =0;
        internal decimal? _price0901 =0;
        internal decimal? _price0902 =0;
        internal decimal? _price0903 =0;
        internal decimal? _price0904 =0;
        internal decimal? _price0905 =0;
        internal decimal? _price0906 =0;
        internal decimal? _price0907 =0;
        internal decimal? _price0908 =0;
        internal decimal? _price0909 =0;
        internal decimal? _price0910 =0;
        internal decimal? _price0911 =0;
        internal decimal? _price0912 =0;
        internal decimal? _price0913 =0;
        internal decimal? _price0914 =0;
        internal decimal? _price0915 =0;
        internal decimal? _price1001 =0;
        internal decimal? _price1002 =0;
        internal decimal? _price1003 =0;
        internal decimal? _price1004 =0;
        internal decimal? _price1005 =0;
        internal decimal? _price1006 =0;
        internal decimal? _price1007 =0;
        internal decimal? _price1008 =0;
        internal decimal? _price1009 =0;
        internal decimal? _price1010 =0;
        internal decimal? _price1011 =0;
        internal decimal? _price1012 =0;
        internal decimal? _price1013 =0;
        internal decimal? _price1014 =0;
        internal decimal? _price1015 =0;
        internal decimal? _price1101 =0;
        internal decimal? _price1102 =0;
        internal decimal? _price1103 =0;
        internal decimal? _price1104 =0;
        internal decimal? _price1105 =0;
        internal decimal? _price1106 =0;
        internal decimal? _price1107 =0;
        internal decimal? _price1108 =0;
        internal decimal? _price1109 =0;
        internal decimal? _price1110 =0;
        internal decimal? _price1111 =0;
        internal decimal? _price1112 =0;
        internal decimal? _price1113 =0;
        internal decimal? _price1114 =0;
        internal decimal? _price1115 =0;
        internal decimal? _price1201 =0;
        internal decimal? _price1202 =0;
        internal decimal? _price1203 =0;
        internal decimal? _price1204 =0;
        internal decimal? _price1205 =0;
        internal decimal? _price1206 =0;
        internal decimal? _price1207 =0;
        internal decimal? _price1208 =0;
        internal decimal? _price1209 =0;
        internal decimal? _price1210 =0;
        internal decimal? _price1211 =0;
        internal decimal? _price1212 =0;
        internal decimal? _price1213 =0;
        internal decimal? _price1214 =0;
        internal decimal? _price1215 =0;
        internal decimal? _price1301 =0;
        internal decimal? _price1302 =0;
        internal decimal? _price1303 =0;
        internal decimal? _price1304 =0;
        internal decimal? _price1305 =0;
        internal decimal? _price1306 =0;
        internal decimal? _price1307 =0;
        internal decimal? _price1308 =0;
        internal decimal? _price1309 =0;
        internal decimal? _price1310 =0;
        internal decimal? _price1311 =0;
        internal decimal? _price1312 =0;
        internal decimal? _price1313 =0;
        internal decimal? _price1314 =0;
        internal decimal? _price1315 =0;
        internal decimal? _price1401 =0;
        internal decimal? _price1402 =0;
        internal decimal? _price1403 =0;
        internal decimal? _price1404 =0;
        internal decimal? _price1405 =0;
        internal decimal? _price1406 =0;
        internal decimal? _price1407 =0;
        internal decimal? _price1408 =0;
        internal decimal? _price1409 =0;
        internal decimal? _price1410 =0;
        internal decimal? _price1411 =0;
        internal decimal? _price1412 =0;
        internal decimal? _price1413 =0;
        internal decimal? _price1414 =0;
        internal decimal? _price1415 =0;
        internal decimal? _price1501 =0;
        internal decimal? _price1502 =0;
        internal decimal? _price1503 =0;
        internal decimal? _price1504 =0;
        internal decimal? _price1505 =0;
        internal decimal? _price1506 =0;
        internal decimal? _price1507 =0;
        internal decimal? _price1508 =0;
        internal decimal? _price1509 =0;
        internal decimal? _price1510 =0;
        internal decimal? _price1511 =0;
        internal decimal? _price1512 =0;
        internal decimal? _price1513 =0;
        internal decimal? _price1514 =0;
        internal decimal? _price1515 =0;
        internal decimal? _ratio1 =0;
        internal decimal? _ratio2 =0;
        internal decimal? _ratio3 =0;
        internal decimal? _ratio4 =0;
        internal decimal? _ratio5 =0;
        internal decimal? _ratio6 =0;
        internal decimal? _ratio7 =0;
        internal decimal? _ratio8 =0;
        internal decimal? _ratio9 =0;
        internal decimal? _ratio10 =0;
        internal decimal? _ratio11 =0;
        internal decimal? _ratio12 =0;
        internal decimal? _ratio13 =0;
        internal decimal? _ratio14 =0;
        internal decimal? _ratio15 =0;
        internal decimal? _qtyDisQty1 =0;
        internal decimal? _qtyDisQty2 =0;
        internal decimal? _qtyDisQty3 =0;
        internal decimal? _qtyDisQty4 =0;
        internal decimal? _qtyDisQty5 =0;
        internal decimal? _qtyDisRatio1 =0;
        internal decimal? _qtyDisRatio2 =0;
        internal decimal? _qtyDisRatio3 =0;
        internal decimal? _qtyDisRatio4 =0;
        internal decimal? _qtyDisRatio5 =0;
        internal decimal? _standardCost1 =0;
        internal decimal? _standardCost2 =0;
        internal decimal? _standardCost3 =0;
        internal decimal? _standardCost4 =0;
        internal decimal? _standardCost5 =0;
        internal decimal? _standardCost6 =0;
        internal decimal? _standardCost7 =0;
        internal decimal? _standardCost8 =0;
        internal decimal? _standardCost9 =0;
        internal decimal? _standardCost10 =0;
        internal decimal? _standardCost11 =0;
        internal decimal? _standardCost12 =0;
        internal decimal? _standardCost13 =0;
        internal decimal? _standardCost14 =0;
        internal decimal? _standardCost15 =0;
        internal DateTime? _createDate =null;
        internal int? _createUserKey =null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

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
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                if (_error != value)
                    _error = value;
            }
        }

        public decimal? StandardPrice1
        {
            get
            {
                return _standardPrice1;
            }
            set
            {
                _standardPrice1 = value;
                PropertyHasChanged("StandardPrice1");
            }
        }

        public decimal? StandardPrice2
        {
            get
            {
                return _standardPrice2;
            }
            set
            {
                _standardPrice2 = value;
                PropertyHasChanged("StandardPrice2");
            }
        }

        public decimal? StandardPrice3
        {
            get
            {
                return _standardPrice3;
            }
            set
            {
                _standardPrice3 = value;
                PropertyHasChanged("StandardPrice3");
            }
        }

        public decimal? StandardPrice4
        {
            get
            {
                return _standardPrice4;
            }
            set
            {
                _standardPrice4 = value;
                PropertyHasChanged("StandardPrice4");
            }
        }

        public decimal? StandardPrice5
        {
            get
            {
                return _standardPrice5;
            }
            set
            {
                _standardPrice5 = value;
                PropertyHasChanged("StandardPrice5");
            }
        }

        public decimal? StandardPrice6
        {
            get
            {
                return _standardPrice6;
            }
            set
            {
                _standardPrice6 = value;
                PropertyHasChanged("StandardPrice6");
            }
        }

        public decimal? StandardPrice7
        {
            get
            {
                return _standardPrice7;
            }
            set
            {
                _standardPrice7 = value;
                PropertyHasChanged("StandardPrice7");
            }
        }


        public decimal? StandardPrice8
        {
            get
            {
                return _standardPrice8;
            }
            set
            {
                _standardPrice8 = value;
                PropertyHasChanged("StandardPrice8");
            }
        }

        public decimal? StandardPrice9
        {
            get
            {
                return _standardPrice9;
            }
            set
            {
                _standardPrice9 = value;
                PropertyHasChanged("StandardPrice9");
            }
        }

        public decimal? StandardPrice10
        {
            get
            {
                return _standardPrice10;
            }
            set
            {
                _standardPrice10 = value;
                PropertyHasChanged("StandardPrice10");
            }
        }

        public decimal? StandardPrice11
        {
            get
            {
                return _standardPrice11;
            }
            set
            {
                _standardPrice11 = value;
                PropertyHasChanged("StandardPrice11");
            }
        }

        public decimal? StandardPrice12
        {
            get
            {
                return _standardPrice12;
            }
            set
            {
                _standardPrice12 = value;
                PropertyHasChanged("StandardPrice12");
            }
        }

        public decimal? StandardPrice13
        {
            get
            {
                return _standardPrice13;
            }
            set
            {
                _standardPrice13 = value;
                PropertyHasChanged("StandardPrice13");
            }
        }

        public decimal? StandardPrice14
        {
            get
            {
                return _standardPrice14;
            }
            set
            {
                _standardPrice14 = value;
                PropertyHasChanged("StandardPrice14");
            }
        }

        public decimal? StandardPrice15
        {
            get
            {
                return _standardPrice15;
            }
            set
            {
                _standardPrice15 = value;
                PropertyHasChanged("StandardPrice15");
            }
        }

        public decimal? Price0101
        {
            get
            {
                return _price0101;
            }
            set
            {
                _price0101 = value;
                PropertyHasChanged("Price0101");
            }
        }

        public decimal? Price0102
        {
            get
            {
                return _price0102;
            }
            set
            {
                _price0102 = value;
                PropertyHasChanged("Price0102");
            }
        }

        public decimal? Price0103
        {
            get
            {
                return _price0103;
            }
            set
            {
                _price0103 = value;
                PropertyHasChanged("Price0103");
            }
        }

        public decimal? Price0104
        {
            get
            {
                return _price0104;
            }
            set
            {
                _price0104 = value;
                PropertyHasChanged("Price0104");
            }
        }

        public decimal? Price0105
        {
            get
            {
                return _price0105;
            }
            set
            {
                _price0105 = value;
                PropertyHasChanged("Price0105");
            }
        }

        public decimal? Price0106
        {
            get
            {
                return _price0106;
            }
            set
            {
                _price0106 = value;
                PropertyHasChanged("Price0106");
            }
        }

        public decimal? Price0107
        {
            get
            {
                return _price0107;
            }
            set
            {
                _price0107 = value;
                PropertyHasChanged("Price0107");
            }
        }

        public decimal? Price0108
        {
            get
            {
                return _price0108;
            }
            set
            {
                _price0108 = value;
                PropertyHasChanged("Price0108");
            }
        }

        public decimal? Price0109
        {
            get
            {
                return _price0109;
            }
            set
            {
                _price0109 = value;
                PropertyHasChanged("Price0109");
            }
        }

        public decimal? Price0110
        {
            get
            {
                return _price0110;
            }
            set
            {
                _price0110 = value;
                PropertyHasChanged("Price0110");
            }
        }

        public decimal? Price0111
        {
            get
            {
                return _price0111;
            }
            set
            {
                _price0111 = value;
                PropertyHasChanged("Price0111");
            }
        }

        public decimal? Price0112
        {
            get
            {
                return _price0112;
            }
            set
            {
                _price0112 = value;
                PropertyHasChanged("Price0112");
            }
        }

        public decimal? Price0113
        {
            get
            {
                return _price0113;
            }
            set
            {
                _price0113 = value;
                PropertyHasChanged("Price0113");
            }
        }

        public decimal? Price0114
        {
            get
            {
                return _price0114;
            }
            set
            {
                _price0114 = value;
                PropertyHasChanged("Price0114");
            }
        }

        public decimal? Price0115
        {
            get
            {
                return _price0115;
            }
            set
            {
                _price0115 = value;
                PropertyHasChanged("Price0115");
            }
        }

        public decimal? Price0201
        {
            get
            {
                return _price0201;
            }
            set
            {
                _price0201 = value;
                PropertyHasChanged("Price0201");
            }
        }

        public decimal? Price0202
        {
            get
            {
                return _price0202;
            }
            set
            {
                _price0202 = value;
                PropertyHasChanged("Price0202");
            }
        }

        public decimal? Price0203
        {
            get
            {
                return _price0203;
            }
            set
            {
                _price0203 = value;
                PropertyHasChanged("Price0203");
            }
        }

        public decimal? Price0204
        {
            get
            {
                return _price0204;
            }
            set
            {
                _price0204 = value;
                PropertyHasChanged("Price0204");
            }
        }

        public decimal? Price0205
        {
            get
            {
                return _price0205;
            }
            set
            {
                _price0205 = value;
                PropertyHasChanged("Price0205");
            }
        }

        public decimal? Price0206
        {
            get
            {
                return _price0206;
            }
            set
            {
                _price0206 = value;
                PropertyHasChanged("Price0206");
            }
        }

        public decimal? Price0207
        {
            get
            {
                return _price0207;
            }
            set
            {
                _price0207 = value;
                PropertyHasChanged("Price0207");
            }
        }

        public decimal? Price0208
        {
            get
            {
                return _price0208;
            }
            set
            {
                _price0208 = value;
                PropertyHasChanged("Price0208");
            }
        }

        public decimal? Price0209
        {
            get
            {
                return _price0209;
            }
            set
            {
                _price0209 = value;
                PropertyHasChanged("Price0209");
            }
        }

        public decimal? Price0210
        {
            get
            {
                return _price0210;
            }
            set
            {
                _price0210 = value;
                PropertyHasChanged("Price0210");
            }
        }

        public decimal? Price0211
        {
            get
            {
                return _price0211;
            }
            set
            {
                _price0211 = value;
                PropertyHasChanged("Price0211");
            }
        }

        public decimal? Price0212
        {
            get
            {
                return _price0212;
            }
            set
            {
                _price0212 = value;
                PropertyHasChanged("Price0212");
            }
        }

        public decimal? Price0213
        {
            get
            {
                return _price0213;
            }
            set
            {
                _price0213 = value;
                PropertyHasChanged("Price0213");
            }
        }

        public decimal? Price0214
        {
            get
            {
                return _price0214;
            }
            set
            {
                _price0214 = value;
                PropertyHasChanged("Price0214");
            }
        }

        public decimal? Price0215
        {
            get
            {
                return _price0215;
            }
            set
            {
                _price0215 = value;
                PropertyHasChanged("Price0215");
            }
        }

        public decimal? Price0301
        {
            get
            {
                return _price0301;
            }
            set
            {
                _price0301 = value;
                PropertyHasChanged("Price0301");
            }
        }

        public decimal? Price0302
        {
            get
            {
                return _price0302;
            }
            set
            {
                _price0302 = value;
                PropertyHasChanged("Price0302");
            }
        }

        public decimal? Price0303
        {
            get
            {
                return _price0303;
            }
            set
            {
                _price0303 = value;
                PropertyHasChanged("Price0303");
            }
        }

        public decimal? Price0304
        {
            get
            {
                return _price0304;
            }
            set
            {
                _price0304 = value;
                PropertyHasChanged("Price0304");
            }
        }

        public decimal? Price0305
        {
            get
            {
                return _price0305;
            }
            set
            {
                _price0305 = value;
                PropertyHasChanged("Price0305");
            }
        }

        public decimal? Price0306
        {
            get
            {
                return _price0306;
            }
            set
            {
                _price0306 = value;
                PropertyHasChanged("Price0306");
            }
        }

        public decimal? Price0307
        {
            get
            {
                return _price0307;
            }
            set
            {
                _price0307 = value;
                PropertyHasChanged("Price0307");
            }
        }

        public decimal? Price0308
        {
            get
            {
                return _price0308;
            }
            set
            {
                _price0308 = value;
                PropertyHasChanged("Price0308");
            }
        }

        public decimal? Price0309
        {
            get
            {
                return _price0309;
            }
            set
            {
                _price0309 = value;
                PropertyHasChanged("Price0309");
            }
        }

        public decimal? Price0310
        {
            get
            {
                return _price0310;
            }
            set
            {
                _price0310 = value;
                PropertyHasChanged("Price0310");
            }
        }

        public decimal? Price0311
        {
            get
            {
                return _price0311;
            }
            set
            {
                _price0311 = value;
                PropertyHasChanged("Price0311");
            }
        }

        public decimal? Price0312
        {
            get
            {
                return _price0312;
            }
            set
            {
                _price0312 = value;
                PropertyHasChanged("Price0312");
            }
        }

        public decimal? Price0313
        {
            get
            {
                return _price0313;
            }
            set
            {
                _price0313 = value;
                PropertyHasChanged("Price0313");
            }
        }

        public decimal? Price0314
        {
            get
            {
                return _price0314;
            }
            set
            {
                _price0314 = value;
                PropertyHasChanged("Price0314");
            }
        }

        public decimal? Price0315
        {
            get
            {
                return _price0315;
            }
            set
            {
                _price0315 = value;
                PropertyHasChanged("Price0315");
            }
        }

        public decimal? Price0401
        {
            get
            {
                return _price0401;
            }
            set
            {
                _price0401 = value;
                PropertyHasChanged("Price0401");
            }
        }

        public decimal? Price0402
        {
            get
            {
                return _price0402;
            }
            set
            {
                _price0402 = value;
                PropertyHasChanged("Price0402");
            }
        }

        public decimal? Price0403
        {
            get
            {
                return _price0403;
            }
            set
            {
                _price0403 = value;
                PropertyHasChanged("Price0403");
            }
        }

        public decimal? Price0404
        {
            get
            {
                return _price0404;
            }
            set
            {
                _price0404 = value;
                PropertyHasChanged("Price0404");
            }
        }

        public decimal? Price0405
        {
            get
            {
                return _price0405;
            }
            set
            {
                _price0405 = value;
                PropertyHasChanged("Price0405");
            }
        }

        public decimal? Price0406
        {
            get
            {
                return _price0406;
            }
            set
            {
                _price0406 = value;
                PropertyHasChanged("Price0406");
            }
        }

        public decimal? Price0407
        {
            get
            {
                return _price0407;
            }
            set
            {
                _price0407 = value;
                PropertyHasChanged("Price0407");
            }
        }

        public decimal? Price0408
        {
            get
            {
                return _price0408;
            }
            set
            {
                _price0408 = value;
                PropertyHasChanged("Price0408");
            }
        }

        public decimal? Price0409
        {
            get
            {
                return _price0409;
            }
            set
            {
                _price0409 = value;
                PropertyHasChanged("Price0409");
            }
        }

        public decimal? Price0410
        {
            get
            {
                return _price0410;
            }
            set
            {
                _price0410 = value;
                PropertyHasChanged("Price0410");
            }
        }

        public decimal? Price0411
        {
            get
            {
                return _price0411;
            }
            set
            {
                _price0411 = value;
                PropertyHasChanged("Price0411");
            }
        }

        public decimal? Price0412
        {
            get
            {
                return _price0412;
            }
            set
            {
                _price0412 = value;
                PropertyHasChanged("Price0412");
            }
        }

        public decimal? Price0413
        {
            get
            {
                return _price0413;
            }
            set
            {
                _price0413 = value;
                PropertyHasChanged("Price0413");
            }
        }

        public decimal? Price0414
        {
            get
            {
                return _price0414;
            }
            set
            {
                _price0414 = value;
                PropertyHasChanged("Price0414");
            }
        }

        public decimal? Price0415
        {
            get
            {
                return _price0415;
            }
            set
            {
                _price0415 = value;
                PropertyHasChanged("Price0415");
            }
        }

        public decimal? Price0501
        {
            get
            {
                return _price0501;
            }
            set
            {
                _price0501 = value;
                PropertyHasChanged("Price0501");
            }
        }

        public decimal? Price0502
        {
            get
            {
                return _price0502;
            }
            set
            {
                _price0502 = value;
                PropertyHasChanged("Price0502");
            }
        }

        public decimal? Price0503
        {
            get
            {
                return _price0503;
            }
            set
            {
                _price0503 = value;
                PropertyHasChanged("Price0503");
            }
        }

        public decimal? Price0504
        {
            get
            {
                return _price0504;
            }
            set
            {
                _price0504 = value;
                PropertyHasChanged("Price0504");
            }
        }

        public decimal? Price0505
        {
            get
            {
                return _price0505;
            }
            set
            {
                _price0505 = value;
                PropertyHasChanged("Price0505");
            }
        }

        public decimal? Price0506
        {
            get
            {
                return _price0506;
            }
            set
            {
                _price0506 = value;
                PropertyHasChanged("Price0506");
            }
        }

        public decimal? Price0507
        {
            get
            {
                return _price0507;
            }
            set
            {
                _price0507 = value;
                PropertyHasChanged("Price0507");
            }
        }

        public decimal? Price0508
        {
            get
            {
                return _price0508;
            }
            set
            {
                _price0508 = value;
                PropertyHasChanged("Price0508");
            }
        }

        public decimal? Price0509
        {
            get
            {
                return _price0509;
            }
            set
            {
                _price0509 = value;
                PropertyHasChanged("Price0509");
            }
        }

        public decimal? Price0510
        {
            get
            {
                return _price0510;
            }
            set
            {
                _price0510 = value;
                PropertyHasChanged("Price0510");
            }
        }

        public decimal? Price0511
        {
            get
            {
                return _price0511;
            }
            set
            {
                _price0511 = value;
                PropertyHasChanged("Price0511");
            }
        }

        public decimal? Price0512
        {
            get
            {
                return _price0512;
            }
            set
            {
                _price0512 = value;
                PropertyHasChanged("Price0512");
            }
        }

        public decimal? Price0513
        {
            get
            {
                return _price0513;
            }
            set
            {
                _price0513 = value;
                PropertyHasChanged("Price0513");
            }
        }

        public decimal? Price0514
        {
            get
            {
                return _price0514;
            }
            set
            {
                _price0514 = value;
                PropertyHasChanged("Price0514");
            }
        }

        public decimal? Price0515
        {
            get
            {
                return _price0515;
            }
            set
            {
                _price0515 = value;
                PropertyHasChanged("Price0515");
            }
        }

        public decimal? Price0601
        {
            get
            {
                return _price0601;
            }
            set
            {
                _price0601 = value;
                PropertyHasChanged("Price0601");
            }
        }

        public decimal? Price0602
        {
            get
            {
                return _price0602;
            }
            set
            {
                _price0602 = value;
                PropertyHasChanged("Price0602");
            }
        }

        public decimal? Price0603
        {
            get
            {
                return _price0603;
            }
            set
            {
                _price0603 = value;
                PropertyHasChanged("Price0603");
            }
        }

        public decimal? Price0604
        {
            get
            {
                return _price0604;
            }
            set
            {
                _price0604 = value;
                PropertyHasChanged("Price0604");
            }
        }

        public decimal? Price0605
        {
            get
            {
                return _price0605;
            }
            set
            {
                _price0605 = value;
                PropertyHasChanged("Price0605");
            }
        }

        public decimal? Price0606
        {
            get
            {
                return _price0606;
            }
            set
            {
                _price0606 = value;
                PropertyHasChanged("Price0606");
            }
        }

        public decimal? Price0607
        {
            get
            {
                return _price0607;
            }
            set
            {
                _price0607 = value;
                PropertyHasChanged("Price0607");
            }
        }

        public decimal? Price0608
        {
            get
            {
                return _price0608;
            }
            set
            {
                _price0608 = value;
                PropertyHasChanged("Price0608");
            }
        }

        public decimal? Price0609
        {
            get
            {
                return _price0609;
            }
            set
            {
                _price0609 = value;
                PropertyHasChanged("Price0609");
            }
        }

        public decimal? Price0610
        {
            get
            {
                return _price0610;
            }
            set
            {
                _price0610 = value;
                PropertyHasChanged("Price0610");
            }
        }

        public decimal? Price0611
        {
            get
            {
                return _price0611;
            }
            set
            {
                _price0611 = value;
                PropertyHasChanged("Price0611");
            }
        }

        public decimal? Price0612
        {
            get
            {
                return _price0612;
            }
            set
            {
                _price0612 = value;
                PropertyHasChanged("Price0612");
            }
        }

        public decimal? Price0613
        {
            get
            {
                return _price0613;
            }
            set
            {
                _price0613 = value;
                PropertyHasChanged("Price0613");
            }
        }

        public decimal? Price0614
        {
            get
            {
                return _price0614;
            }
            set
            {
                _price0614 = value;
                PropertyHasChanged("Price0614");
            }
        }

        public decimal? Price0615
        {
            get
            {
                return _price0615;
            }
            set
            {
                _price0615 = value;
                PropertyHasChanged("Price0615");
            }
        }

        public decimal? Price0701
        {
            get
            {
                return _price0701;
            }
            set
            {
                _price0701 = value;
                PropertyHasChanged("Price0701");
            }
        }

        public decimal? Price0702
        {
            get
            {
                return _price0702;
            }
            set
            {
                _price0702 = value;
                PropertyHasChanged("Price0702");
            }
        }

        public decimal? Price0703
        {
            get
            {
                return _price0703;
            }
            set
            {
                _price0703 = value;
                PropertyHasChanged("Price0703");
            }
        }

        public decimal? Price0704
        {
            get
            {
                return _price0704;
            }
            set
            {
                _price0704 = value;
                PropertyHasChanged("Price0704");
            }
        }

        public decimal? Price0705
        {
            get
            {
                return _price0705;
            }
            set
            {
                _price0705 = value;
                PropertyHasChanged("Price0705");
            }
        }

        public decimal? Price0706
        {
            get
            {
                return _price0706;
            }
            set
            {
                _price0706 = value;
                PropertyHasChanged("Price0706");
            }
        }

        public decimal? Price0707
        {
            get
            {
                return _price0707;
            }
            set
            {
                _price0707 = value;
                PropertyHasChanged("Price0707");
            }
        }

        public decimal? Price0708
        {
            get
            {
                return _price0708;
            }
            set
            {
                _price0708 = value;
                PropertyHasChanged("Price0708");
            }
        }

        public decimal? Price0709
        {
            get
            {
                return _price0709;
            }
            set
            {
                _price0709 = value;
                PropertyHasChanged("Price0709");
            }
        }

        public decimal? Price0710
        {
            get
            {
                return _price0710;
            }
            set
            {
                _price0710 = value;
                PropertyHasChanged("Price0710");
            }
        }

        public decimal? Price0711
        {
            get
            {
                return _price0711;
            }
            set
            {
                _price0711 = value;
                PropertyHasChanged("Price0711");
            }
        }

        public decimal? Price0712
        {
            get
            {
                return _price0712;
            }
            set
            {
                _price0712 = value;
                PropertyHasChanged("Price0712");
            }
        }

        public decimal? Price0713
        {
            get
            {
                return _price0713;
            }
            set
            {
                _price0713 = value;
                PropertyHasChanged("Price0713");
            }
        }

        public decimal? Price0714
        {
            get
            {
                return _price0714;
            }
            set
            {
                _price0714 = value;
                PropertyHasChanged("Price0714");
            }
        }

        public decimal? Price0715
        {
            get
            {
                return _price0715;
            }
            set
            {
                _price0715 = value;
                PropertyHasChanged("Price0715");
            }
        }

        public decimal? Price0801
        {
            get
            {
                return _price0801;
            }
            set
            {
                _price0801 = value;
                PropertyHasChanged("Price0801");
            }
        }

        public decimal? Price0802
        {
            get
            {
                return _price0802;
            }
            set
            {
                _price0802 = value;
                PropertyHasChanged("Price0802");
            }
        }

        public decimal? Price0803
        {
            get
            {
                return _price0803;
            }
            set
            {
                _price0803 = value;
                PropertyHasChanged("Price0803");
            }
        }

        public decimal? Price0804
        {
            get
            {
                return _price0804;
            }
            set
            {
                _price0804 = value;
                PropertyHasChanged("Price0804");
            }
        }

        public decimal? Price0805
        {
            get
            {
                return _price0805;
            }
            set
            {
                _price0805 = value;
                PropertyHasChanged("Price0805");
            }
        }

        public decimal? Price0806
        {
            get
            {
                return _price0806;
            }
            set
            {
                _price0806 = value;
                PropertyHasChanged("Price0806");
            }
        }

        public decimal? Price0807
        {
            get
            {
                return _price0807;
            }
            set
            {
                _price0807 = value;
                PropertyHasChanged("Price0807");
            }
        }

        public decimal? Price0808
        {
            get
            {
                return _price0808;
            }
            set
            {
                _price0808 = value;
                PropertyHasChanged("Price0808");
            }
        }

        public decimal? Price0809
        {
            get
            {
                return _price0809;
            }
            set
            {
                _price0809 = value;
                PropertyHasChanged("Price0809");
            }
        }

        public decimal? Price0810
        {
            get
            {
                return _price0810;
            }
            set
            {
                _price0810 = value;
                PropertyHasChanged("Price0810");
            }
        }

        public decimal? Price0811
        {
            get
            {
                return _price0811;
            }
            set
            {
                _price0811 = value;
                PropertyHasChanged("Price0811");
            }
        }

        public decimal? Price0812
        {
            get
            {
                return _price0812;
            }
            set
            {
                _price0812 = value;
                PropertyHasChanged("Price0812");
            }
        }

        public decimal? Price0813
        {
            get
            {
                return _price0813;
            }
            set
            {
                _price0813 = value;
                PropertyHasChanged("Price0813");
            }
        }

        public decimal? Price0814
        {
            get
            {
                return _price0814;
            }
            set
            {
                _price0814 = value;
                PropertyHasChanged("Price0814");
            }
        }

        public decimal? Price0815
        {
            get
            {
                return _price0815;
            }
            set
            {
                _price0815 = value;
                PropertyHasChanged("Price0815");
            }
        }

        public decimal? Price0901
        {
            get
            {
                return _price0901;
            }
            set
            {
                _price0901 = value;
                PropertyHasChanged("Price0901");
            }
        }

        public decimal? Price0902
        {
            get
            {
                return _price0902;
            }
            set
            {
                _price0902 = value;
                PropertyHasChanged("Price0902");
            }
        }

        public decimal? Price0903
        {
            get
            {
                return _price0903;
            }
            set
            {
                _price0903 = value;
                PropertyHasChanged("Price0903");
            }
        }

        public decimal? Price0904
        {
            get
            {
                return _price0904;
            }
            set
            {
                _price0904 = value;
                PropertyHasChanged("Price0904");
            }
        }

        public decimal? Price0905
        {
            get
            {
                return _price0905;
            }
            set
            {
                _price0905 = value;
                PropertyHasChanged("Price0905");
            }
        }

        public decimal? Price0906
        {
            get
            {
                return _price0906;
            }
            set
            {
                _price0906 = value;
                PropertyHasChanged("Price0906");
            }
        }

        public decimal? Price0907
        {
            get
            {
                return _price0907;
            }
            set
            {
                _price0907 = value;
                PropertyHasChanged("Price0907");
            }
        }

        public decimal? Price0908
        {
            get
            {
                return _price0908;
            }
            set
            {
                _price0908 = value;
                PropertyHasChanged("Price0908");
            }
        }

        public decimal? Price0909
        {
            get
            {
                return _price0909;
            }
            set
            {
                _price0909 = value;
                PropertyHasChanged("Price0909");
            }
        }

        public decimal? Price0910
        {
            get
            {
                return _price0910;
            }
            set
            {
                _price0910 = value;
                PropertyHasChanged("Price0910");
            }
        }

        public decimal? Price0911
        {
            get
            {
                return _price0911;
            }
            set
            {
                _price0911 = value;
                PropertyHasChanged("Price0911");
            }
        }

        public decimal? Price0912
        {
            get
            {
                return _price0912;
            }
            set
            {
                _price0912 = value;
                PropertyHasChanged("Price0912");
            }
        }

        public decimal? Price0913
        {
            get
            {
                return _price0913;
            }
            set
            {
                _price0913 = value;
                PropertyHasChanged("Price0913");
            }
        }

        public decimal? Price0914
        {
            get
            {
                return _price0914;
            }
            set
            {
                _price0914 = value;
                PropertyHasChanged("Price0914");
            }
        }

        public decimal? Price0915
        {
            get
            {
                return _price0915;
            }
            set
            {
                _price0915 = value;
                PropertyHasChanged("Price0915");
            }
        }

        public decimal? Price1001
        {
            get
            {
                return _price1001;
            }
            set
            {
                _price1001 = value;
                PropertyHasChanged("Price1001");
            }
        }

        public decimal? Price1002
        {
            get
            {
                return _price1002;
            }
            set
            {
                _price1002 = value;
                PropertyHasChanged("Price1002");
            }
        }

        public decimal? Price1003
        {
            get
            {
                return _price1003;
            }
            set
            {
                _price1003 = value;
                PropertyHasChanged("Price1003");
            }
        }

        public decimal? Price1004
        {
            get
            {
                return _price1004;
            }
            set
            {
                _price1004 = value;
                PropertyHasChanged("Price1004");
            }
        }

        public decimal? Price1005
        {
            get
            {
                return _price1005;
            }
            set
            {
                _price1005 = value;
                PropertyHasChanged("Price1005");
            }
        }

        public decimal? Price1006
        {
            get
            {
                return _price1006;
            }
            set
            {
                _price1006 = value;
                PropertyHasChanged("Price1006");
            }
        }

        public decimal? Price1007
        {
            get
            {
                return _price1007;
            }
            set
            {
                _price1007 = value;
                PropertyHasChanged("Price1007");
            }
        }

        public decimal? Price1008
        {
            get
            {
                return _price1008;
            }
            set
            {
                _price1008 = value;
                PropertyHasChanged("Price1008");
            }
        }

        public decimal? Price1009
        {
            get
            {
                return _price1009;
            }
            set
            {
                _price1009 = value;
                PropertyHasChanged("Price1009");
            }
        }

        public decimal? Price1010
        {
            get
            {
                return _price1010;
            }
            set
            {
                _price1010 = value;
                PropertyHasChanged("Price1010");
            }
        }

        public decimal? Price1011
        {
            get
            {
                return _price1011;
            }
            set
            {
                _price1011 = value;
                PropertyHasChanged("Price1011");
            }
        }

        public decimal? Price1012
        {
            get
            {
                return _price1012;
            }
            set
            {
                _price1012 = value;
                PropertyHasChanged("Price1012");
            }
        }

        public decimal? Price1013
        {
            get
            {
                return _price1013;
            }
            set
            {
                _price1013 = value;
                PropertyHasChanged("Price1013");
            }
        }

        public decimal? Price1014
        {
            get
            {
                return _price1014;
            }
            set
            {
                _price1014 = value;
                PropertyHasChanged("Price1014");
            }
        }

        public decimal? Price1015
        {
            get
            {
                return _price1015;
            }
            set
            {
                _price1015 = value;
                PropertyHasChanged("Price1015");
            }
        }

        public decimal? Price1101
        {
            get
            {
                return _price1101;
            }
            set
            {
                _price1101 = value;
                PropertyHasChanged("Price1101");
            }
        }

        public decimal? Price1102
        {
            get
            {
                return _price1102;
            }
            set
            {
                _price1102 = value;
                PropertyHasChanged("Price1102");
            }
        }

        public decimal? Price1103
        {
            get
            {
                return _price1103;
            }
            set
            {
                _price1103 = value;
                PropertyHasChanged("Price1103");
            }
        }

        public decimal? Price1104
        {
            get
            {
                return _price1104;
            }
            set
            {
                _price1104 = value;
                PropertyHasChanged("Price1104");
            }
        }

        public decimal? Price1105
        {
            get
            {
                return _price1105;
            }
            set
            {
                _price1105 = value;
                PropertyHasChanged("Price1105");
            }
        }

        public decimal? Price1106
        {
            get
            {
                return _price1106;
            }
            set
            {
                _price1106 = value;
                PropertyHasChanged("Price1106");
            }
        }

        public decimal? Price1107
        {
            get
            {
                return _price1107;
            }
            set
            {
                _price1107 = value;
                PropertyHasChanged("Price1107");
            }
        }

        public decimal? Price1108
        {
            get
            {
                return _price1108;
            }
            set
            {
                _price1108 = value;
                PropertyHasChanged("Price1108");
            }
        }

        public decimal? Price1109
        {
            get
            {
                return _price1109;
            }
            set
            {
                _price1109 = value;
                PropertyHasChanged("Price1109");
            }
        }

        public decimal? Price1110
        {
            get
            {
                return _price1110;
            }
            set
            {
                _price1110 = value;
                PropertyHasChanged("Price1110");
            }
        }

        public decimal? Price1111
        {
            get
            {
                return _price1111;
            }
            set
            {
                _price1111 = value;
                PropertyHasChanged("Price1111");
            }
        }

        public decimal? Price1112
        {
            get
            {
                return _price1112;
            }
            set
            {
                _price1112 = value;
                PropertyHasChanged("Price1112");
            }
        }

        public decimal? Price1113
        {
            get
            {
                return _price1113;
            }
            set
            {
                _price1113 = value;
                PropertyHasChanged("Price1113");
            }
        }

        public decimal? Price1114
        {
            get
            {
                return _price1114;
            }
            set
            {
                _price1114 = value;
                PropertyHasChanged("Price1114");
            }
        }

        public decimal? Price1115
        {
            get
            {
                return _price1115;
            }
            set
            {
                _price1115 = value;
                PropertyHasChanged("Price1115");
            }
        }

        public decimal? Price1201
        {
            get
            {
                return _price1201;
            }
            set
            {
                _price1201 = value;
                PropertyHasChanged("Price1201");
            }
        }

        public decimal? Price1202
        {
            get
            {
                return _price1202;
            }
            set
            {
                _price1202 = value;
                PropertyHasChanged("Price1202");
            }
        }

        public decimal? Price1203
        {
            get
            {
                return _price1203;
            }
            set
            {
                _price1203 = value;
                PropertyHasChanged("Price1203");
            }
        }

        public decimal? Price1204
        {
            get
            {
                return _price1204;
            }
            set
            {
                _price1204 = value;
                PropertyHasChanged("Price1204");
            }
        }

        public decimal? Price1205
        {
            get
            {
                return _price1205;
            }
            set
            {
                _price1205 = value;
                PropertyHasChanged("Price1205");
            }
        }

        public decimal? Price1206
        {
            get
            {
                return _price1206;
            }
            set
            {
                _price1206 = value;
                PropertyHasChanged("Price1206");
            }
        }

        public decimal? Price1207
        {
            get
            {
                return _price1207;
            }
            set
            {
                _price1207 = value;
                PropertyHasChanged("Price1207");
            }
        }

        public decimal? Price1208
        {
            get
            {
                return _price1208;
            }
            set
            {
                _price1208 = value;
                PropertyHasChanged("Price1208");
            }
        }

        public decimal? Price1209
        {
            get
            {
                return _price1209;
            }
            set
            {
                _price1209 = value;
                PropertyHasChanged("Price1209");
            }
        }

        public decimal? Price1210
        {
            get
            {
                return _price1210;
            }
            set
            {
                _price1210 = value;
                PropertyHasChanged("Price1210");
            }
        }

        public decimal? Price1211
        {
            get
            {
                return _price1211;
            }
            set
            {
                _price1211 = value;
                PropertyHasChanged("Price1211");
            }
        }

        public decimal? Price1212
        {
            get
            {
                return _price1212;
            }
            set
            {
                _price1212 = value;
                PropertyHasChanged("Price1212");
            }
        }

        public decimal? Price1213
        {
            get
            {
                return _price1213;
            }
            set
            {
                _price1213 = value;
                PropertyHasChanged("Price1213");
            }
        }

        public decimal? Price1214
        {
            get
            {
                return _price1214;
            }
            set
            {
                _price1214 = value;
                PropertyHasChanged("Price1214");
            }
        }

        public decimal? Price1215
        {
            get
            {
                return _price1215;
            }
            set
            {
                _price1215 = value;
                PropertyHasChanged("Price1215");
            }
        }

        public decimal? Price1301
        {
            get
            {
                return _price1301;
            }
            set
            {
                _price1301 = value;
                PropertyHasChanged("Price1301");
            }
        }

        public decimal? Price1302
        {
            get
            {
                return _price1302;
            }
            set
            {
                _price1302 = value;
                PropertyHasChanged("Price1302");
            }
        }

        public decimal? Price1303
        {
            get
            {
                return _price1303;
            }
            set
            {
                _price1303 = value;
                PropertyHasChanged("Price1303");
            }
        }

        public decimal? Price1304
        {
            get
            {
                return _price1304;
            }
            set
            {
                _price1304 = value;
                PropertyHasChanged("Price1304");
            }
        }

        public decimal? Price1305
        {
            get
            {
                return _price1305;
            }
            set
            {
                _price1305 = value;
                PropertyHasChanged("Price1305");
            }
        }

        public decimal? Price1306
        {
            get
            {
                return _price1306;
            }
            set
            {
                _price1306 = value;
                PropertyHasChanged("Price1306");
            }
        }

        public decimal? Price1307
        {
            get
            {
                return _price1307;
            }
            set
            {
                _price1307 = value;
                PropertyHasChanged("Price1307");
            }
        }

        public decimal? Price1308
        {
            get
            {
                return _price1308;
            }
            set
            {
                _price1308 = value;
                PropertyHasChanged("Price1308");
            }
        }

        public decimal? Price1309
        {
            get
            {
                return _price1309;
            }
            set
            {
                _price1309 = value;
                PropertyHasChanged("Price1309");
            }
        }

        public decimal? Price1310
        {
            get
            {
                return _price1310;
            }
            set
            {
                _price1310 = value;
                PropertyHasChanged("Price1310");
            }
        }

        public decimal? Price1311
        {
            get
            {
                return _price1311;
            }
            set
            {
                _price1311 = value;
                PropertyHasChanged("Price1311");
            }
        }

        public decimal? Price1312
        {
            get
            {
                return _price1312;
            }
            set
            {
                _price1312 = value;
                PropertyHasChanged("Price1312");
            }
        }

        public decimal? Price1313
        {
            get
            {
                return _price1313;
            }
            set
            {
                _price1313 = value;
                PropertyHasChanged("Price1313");
            }
        }

        public decimal? Price1314
        {
            get
            {
                return _price1314;
            }
            set
            {
                _price1314 = value;
                PropertyHasChanged("Price1314");
            }
        }

        public decimal? Price1315
        {
            get
            {
                return _price1315;
            }
            set
            {
                _price1315 = value;
                PropertyHasChanged("Price1315");
            }
        }

        public decimal? Price1401
        {
            get
            {
                return _price1401;
            }
            set
            {
                _price1401 = value;
                PropertyHasChanged("Price1401");
            }
        }

        public decimal? Price1402
        {
            get
            {
                return _price1402;
            }
            set
            {
                _price1402 = value;
                PropertyHasChanged("Price1402");
            }
        }

        public decimal? Price1403
        {
            get
            {
                return _price1403;
            }
            set
            {
                _price1403 = value;
                PropertyHasChanged("Price1403");
            }
        }

        public decimal? Price1404
        {
            get
            {
                return _price1404;
            }
            set
            {
                _price1404 = value;
                PropertyHasChanged("Price1404");
            }
        }

        public decimal? Price1405
        {
            get
            {
                return _price1405;
            }
            set
            {
                _price1405 = value;
                PropertyHasChanged("Price1405");
            }
        }

        public decimal? Price1406
        {
            get
            {
                return _price1406;
            }
            set
            {
                _price1406 = value;
                PropertyHasChanged("Price1406");
            }
        }

        public decimal? Price1407
        {
            get
            {
                return _price1407;
            }
            set
            {
                _price1407 = value;
                PropertyHasChanged("Price1407");
            }
        }

        public decimal? Price1408
        {
            get
            {
                return _price1408;
            }
            set
            {
                _price1408 = value;
                PropertyHasChanged("Price1408");
            }
        }

        public decimal? Price1409
        {
            get
            {
                return _price1409;
            }
            set
            {
                _price1409 = value;
                PropertyHasChanged("Price1409");
            }
        }

        public decimal? Price1410
        {
            get
            {
                return _price1410;
            }
            set
            {
                _price1410 = value;
                PropertyHasChanged("Price1410");
            }
        }

        public decimal? Price1411
        {
            get
            {
                return _price1411;
            }
            set
            {
                _price1411 = value;
                PropertyHasChanged("Price1411");
            }
        }

        public decimal? Price1412
        {
            get
            {
                return _price1412;
            }
            set
            {
                _price1412 = value;
                PropertyHasChanged("Price1412");
            }
        }

        public decimal? Price1413
        {
            get
            {
                return _price1413;
            }
            set
            {
                _price1413 = value;
                PropertyHasChanged("Price1413");
            }
        }

        public decimal? Price1414
        {
            get
            {
                return _price1414;
            }
            set
            {
                _price1414 = value;
                PropertyHasChanged("Price1414");
            }
        }

        public decimal? Price1415
        {
            get
            {
                return _price1415;
            }
            set
            {
                _price1415 = value;
                PropertyHasChanged("Price1415");
            }
        }

        public decimal? Price1501
        {
            get
            {
                return _price1501;
            }
            set
            {
                _price1501 = value;
                PropertyHasChanged("Price1501");
            }
        }

        public decimal? Price1502
        {
            get
            {
                return _price1502;
            }
            set
            {
                _price1502 = value;
                PropertyHasChanged("Price1502");
            }
        }

        public decimal? Price1503
        {
            get
            {
                return _price1503;
            }
            set
            {
                _price1503 = value;
                PropertyHasChanged("Price1503");
            }
        }

        public decimal? Price1504
        {
            get
            {
                return _price1504;
            }
            set
            {
                _price1504 = value;
                PropertyHasChanged("Price1504");
            }
        }

        public decimal? Price1505
        {
            get
            {
                return _price1505;
            }
            set
            {
                _price1505 = value;
                PropertyHasChanged("Price1505");
            }
        }

        public decimal? Price1506
        {
            get
            {
                return _price1506;
            }
            set
            {
                _price1506 = value;
                PropertyHasChanged("Price1506");
            }
        }

        public decimal? Price1507
        {
            get
            {
                return _price1507;
            }
            set
            {
                _price1507 = value;
                PropertyHasChanged("Price1507");
            }
        }

        public decimal? Price1508
        {
            get
            {
                return _price1508;
            }
            set
            {
                _price1508 = value;
                PropertyHasChanged("Price1508");
            }
        }

        public decimal? Price1509
        {
            get
            {
                return _price1509;
            }
            set
            {
                _price1509 = value;
                PropertyHasChanged("Price1509");
            }
        }

        public decimal? Price1510
        {
            get
            {
                return _price1510;
            }
            set
            {
                _price1510 = value;
                PropertyHasChanged("Price1510");
            }
        }

        public decimal? Price1511
        {
            get
            {
                return _price1511;
            }
            set
            {
                _price1511 = value;
                PropertyHasChanged("Price1511");
            }
        }

        public decimal? Price1512
        {
            get
            {
                return _price1512;
            }
            set
            {
                _price1512 = value;
                PropertyHasChanged("Price1512");
            }
        }

        public decimal? Price1513
        {
            get
            {
                return _price1513;
            }
            set
            {
                _price1513 = value;
                PropertyHasChanged("Price1513");
            }
        }

        public decimal? Price1514
        {
            get
            {
                return _price1514;
            }
            set
            {
                _price1514 = value;
                PropertyHasChanged("Price1514");
            }
        }

        public decimal? Price1515
        {
            get
            {
                return _price1515;
            }
            set
            {
                _price1515 = value;
                PropertyHasChanged("Price1515");
            }
        }

        public decimal? Ratio1
        {
            get
            {
                return _ratio1;
            }
            set
            {
                _ratio1 = value;
                PropertyHasChanged("Ratio1");
            }
        }

        public decimal? Ratio2
        {
            get
            {
                return _ratio2;
            }
            set
            {
                _ratio2 = value;
                PropertyHasChanged("Ratio2");
            }
        }

        public decimal? Ratio3
        {
            get
            {
                return _ratio3;
            }
            set
            {
                _ratio3 = value;
                PropertyHasChanged("Ratio3");
            }
        }

        public decimal? Ratio4
        {
            get
            {
                return _ratio4;
            }
            set
            {
                _ratio4 = value;
                PropertyHasChanged("Ratio4");
            }
        }

        public decimal? Ratio5
        {
            get
            {
                return _ratio5;
            }
            set
            {
                _ratio5 = value;
                PropertyHasChanged("Ratio5");
            }
        }

        public decimal? Ratio6
        {
            get
            {
                return _ratio6;
            }
            set
            {
                _ratio6 = value;
                PropertyHasChanged("Ratio6");
            }
        }

        public decimal? Ratio7
        {
            get
            {
                return _ratio7;
            }
            set
            {
                _ratio7 = value;
                PropertyHasChanged("Ratio7");
            }
        }

        public decimal? Ratio8
        {
            get
            {
                return _ratio8;
            }
            set
            {
                _ratio8 = value;
                PropertyHasChanged("Ratio8");
            }
        }

        public decimal? Ratio9
        {
            get
            {
                return _ratio9;
            }
            set
            {
                _ratio9 = value;
                PropertyHasChanged("Ratio9");
            }
        }

        public decimal? Ratio10
        {
            get
            {
                return _ratio10;
            }
            set
            {
                _ratio10 = value;
                PropertyHasChanged("Ratio10");
            }
        }

        public decimal? Ratio11
        {
            get
            {
                return _ratio11;
            }
            set
            {
                _ratio11 = value;
                PropertyHasChanged("Ratio11");
            }
        }

        public decimal? Ratio12
        {
            get
            {
                return _ratio12;
            }
            set
            {
                _ratio12 = value;
                PropertyHasChanged("Ratio12");
            }
        }

        public decimal? Ratio13
        {
            get
            {
                return _ratio13;
            }
            set
            {
                _ratio13 = value;
                PropertyHasChanged("Ratio13");
            }
        }

        public decimal? Ratio14
        {
            get
            {
                return _ratio14;
            }
            set
            {
                _ratio14 = value;
                PropertyHasChanged("Ratio14");
            }
        }

        public decimal? Ratio15
        {
            get
            {
                return _ratio15;
            }
            set
            {
                _ratio15 = value;
                PropertyHasChanged("Ratio15");
            }
        }

        public decimal? QtyDisQty1
        {
            get
            {
                return _qtyDisQty1;
            }
            set
            {
                _qtyDisQty1 = value;
                PropertyHasChanged("QtyDisQty1");
            }
        }

        public decimal? QtyDisQty2
        {
            get
            {
                return _qtyDisQty2;
            }
            set
            {
                _qtyDisQty2 = value;
                PropertyHasChanged("QtyDisQty2");
            }
        }

        public decimal? QtyDisQty3
        {
            get
            {
                return _qtyDisQty3;
            }
            set
            {
                _qtyDisQty3 = value;
                PropertyHasChanged("QtyDisQty3");
            }
        }

        public decimal? QtyDisQty4
        {
            get
            {
                return _qtyDisQty4;
            }
            set
            {
                _qtyDisQty4 = value;
                PropertyHasChanged("QtyDisQty4");
            }
        }

        public decimal? QtyDisQty5
        {
            get
            {
                return _qtyDisQty5;
            }
            set
            {
                _qtyDisQty5 = value;
                PropertyHasChanged("QtyDisQty5");
            }
        }

        public decimal? QtyDisRatio1
        {
            get
            {
                return _qtyDisRatio1;
            }
            set
            {
                _qtyDisRatio1 = value;
                PropertyHasChanged("QtyDisRatio1");
            }
        }

        public decimal? QtyDisRatio2
        {
            get
            {
                return _qtyDisRatio2;
            }
            set
            {
                _qtyDisRatio2 = value;
                PropertyHasChanged("QtyDisRatio2");
            }
        }

        public decimal? QtyDisRatio3
        {
            get
            {
                return _qtyDisRatio3;
            }
            set
            {
                _qtyDisRatio3 = value;
                PropertyHasChanged("QtyDisRatio3");
            }
        }

        public decimal? QtyDisRatio4
        {
            get
            {
                return _qtyDisRatio4;
            }
            set
            {
                _qtyDisRatio4 = value;
                PropertyHasChanged("QtyDisRatio4");
            }
        }

        public decimal? QtyDisRatio5
        {
            get
            {
                return _qtyDisRatio5;
            }
            set
            {
                _qtyDisRatio5 = value;
                PropertyHasChanged("QtyDisRatio5");
            }
        }

        public decimal? StandardCost1
        {
            get
            {
                return _standardCost1;
            }
            set
            {
                _standardCost1 = value;
                PropertyHasChanged("StandardCost1");
            }
        }

        public decimal? StandardCost2
        {
            get
            {
                return _standardCost2;
            }
            set
            {
                _standardCost2 = value;
                PropertyHasChanged("StandardCost2");
            }
        }

        public decimal? StandardCost3
        {
            get
            {
                return _standardCost3;
            }
            set
            {
                _standardCost3 = value;
                PropertyHasChanged("StandardCost3");
            }
        }

        public decimal? StandardCost4
        {
            get
            {
                return _standardCost4;
            }
            set
            {
                _standardCost4 = value;
                PropertyHasChanged("StandardCost4");
            }
        }

        public decimal? StandardCost5
        {
            get
            {
                return _standardCost5;
            }
            set
            {
                _standardCost5 = value;
                PropertyHasChanged("StandardCost5");
            }
        }

        public decimal? StandardCost6
        {
            get
            {
                return _standardCost6;
            }
            set
            {
                _standardCost6 = value;
                PropertyHasChanged("StandardCost6");
            }
        }

        public decimal? StandardCost7
        {
            get
            {
                return _standardCost7;
            }
            set
            {
                _standardCost7 = value;
                PropertyHasChanged("StandardCost7");
            }
        }

        public decimal? StandardCost8
        {
            get
            {
                return _standardCost8;
            }
            set
            {
                _standardCost8 = value;
                PropertyHasChanged("StandardCost8");
            }
        }

        public decimal? StandardCost9
        {
            get
            {
                return _standardCost9;
            }
            set
            {
                _standardCost9 = value;
                PropertyHasChanged("StandardCost9");
            }
        }

        public decimal? StandardCost10
        {
            get
            {
                return _standardCost10;
            }
            set
            {
                _standardCost10 = value;
                PropertyHasChanged("StandardCost10");
            }
        }

        public decimal? StandardCost11
        {
            get
            {
                return _standardCost11;
            }
            set
            {
                _standardCost11 = value;
                PropertyHasChanged("StandardCost11");
            }
        }

        public decimal? StandardCost12
        {
            get
            {
                return _standardCost12;
            }
            set
            {
                _standardCost12 = value;
                PropertyHasChanged("StandardCost12");
            }
        }

        public decimal? StandardCost13
        {
            get
            {
                return _standardCost13;
            }
            set
            {
                _standardCost13 = value;
                PropertyHasChanged("StandardCost13");
            }
        }

        public decimal? StandardCost14
        {
            get
            {
                return _standardCost14;
            }
            set
            {
                _standardCost14 = value;
                PropertyHasChanged("StandardCost14");
            }
        }

        public decimal? StandardCost15
        {
            get
            {
                return _standardCost15;
            }
            set
            {
                _standardCost15 = value;
                PropertyHasChanged("StandardCost15");
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

        public MSTItmDetPrice()
        { /* require use of factory method */ }

        public static MSTItmDetPrice New()
        {
            MSTItmDetPrice child = new MSTItmDetPrice();
            return child;
        }

        public static MSTItmDetPrice NewChild()
        {
            MSTItmDetPrice child = new MSTItmDetPrice();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        public static MSTItmDetPrice Get(SafeDataReader dr)
        {
            MSTItmDetPrice child = new MSTItmDetPrice();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static MSTItmDetPrice Get(int? itmKey)
        {
            MSTItmDetPrice child = new MSTItmDetPrice();
            child.Fetch(new Criteria(itmKey, 1));
            return child;
        }

        public static MSTItmDetPrice Get(SqlConnection cn, int? itmKey)
        {
            MSTItmDetPrice child = new MSTItmDetPrice();
            child.Fetch(cn, new Criteria(itmKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _itmKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ItmKey)
            {
                _itmKey = ItmKey;
            }

            internal Criteria(int? ItmKey, int? Option)
            {
                _itmKey = ItmKey;
                _option = Option;
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
                cm.CommandText = "MSTItmDetPrice_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
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

        internal bool Fetch(SafeDataReader dr)
        {
            _itmKey = dr.GetInt32("ItmKey");
            _standardPrice1 = dr.GetDecimal("StandardPrice1");
            _standardPrice2 = dr.GetDecimal("StandardPrice2");
            _standardPrice3 = dr.GetDecimal("StandardPrice3");
            _standardPrice4 = dr.GetDecimal("StandardPrice4");
            _standardPrice5 = dr.GetDecimal("StandardPrice5");
            _standardPrice6 = dr.GetDecimal("StandardPrice6");
            _standardPrice7 = dr.GetDecimal("StandardPrice7");
            _standardPrice8 = dr.GetDecimal("StandardPrice8");
            _standardPrice9 = dr.GetDecimal("StandardPrice9");
            _standardPrice10 = dr.GetDecimal("StandardPrice10");
            _standardPrice11 = dr.GetDecimal("StandardPrice11");
            _standardPrice12 = dr.GetDecimal("StandardPrice12");
            _standardPrice13 = dr.GetDecimal("StandardPrice13");
            _standardPrice14 = dr.GetDecimal("StandardPrice14");
            _standardPrice15 = dr.GetDecimal("StandardPrice15");
            _price0101 = dr.GetDecimal("Price0101");
            _price0102 = dr.GetDecimal("Price0102");
            _price0103 = dr.GetDecimal("Price0103");
            _price0104 = dr.GetDecimal("Price0104");
            _price0105 = dr.GetDecimal("Price0105");
            _price0106 = dr.GetDecimal("Price0106");
            _price0107 = dr.GetDecimal("Price0107");
            _price0108 = dr.GetDecimal("Price0108");
            _price0109 = dr.GetDecimal("Price0109");
            _price0110 = dr.GetDecimal("Price0110");
            _price0111 = dr.GetDecimal("Price0111");
            _price0112 = dr.GetDecimal("Price0112");
            _price0113 = dr.GetDecimal("Price0113");
            _price0114 = dr.GetDecimal("Price0114");
            _price0115 = dr.GetDecimal("Price0115");
            _price0201 = dr.GetDecimal("Price0201");
            _price0202 = dr.GetDecimal("Price0202");
            _price0203 = dr.GetDecimal("Price0203");
            _price0204 = dr.GetDecimal("Price0204");
            _price0205 = dr.GetDecimal("Price0205");
            _price0206 = dr.GetDecimal("Price0206");
            _price0207 = dr.GetDecimal("Price0207");
            _price0208 = dr.GetDecimal("Price0208");
            _price0209 = dr.GetDecimal("Price0209");
            _price0210 = dr.GetDecimal("Price0210");
            _price0211 = dr.GetDecimal("Price0211");
            _price0212 = dr.GetDecimal("Price0212");
            _price0213 = dr.GetDecimal("Price0213");
            _price0214 = dr.GetDecimal("Price0214");
            _price0215 = dr.GetDecimal("Price0215");
            _price0301 = dr.GetDecimal("Price0301");
            _price0302 = dr.GetDecimal("Price0302");
            _price0303 = dr.GetDecimal("Price0303");
            _price0304 = dr.GetDecimal("Price0304");
            _price0305 = dr.GetDecimal("Price0305");
            _price0306 = dr.GetDecimal("Price0306");
            _price0307 = dr.GetDecimal("Price0307");
            _price0308 = dr.GetDecimal("Price0308");
            _price0309 = dr.GetDecimal("Price0309");
            _price0310 = dr.GetDecimal("Price0310");
            _price0311 = dr.GetDecimal("Price0311");
            _price0312 = dr.GetDecimal("Price0312");
            _price0313 = dr.GetDecimal("Price0313");
            _price0314 = dr.GetDecimal("Price0314");
            _price0315 = dr.GetDecimal("Price0315");
            _price0401 = dr.GetDecimal("Price0401");
            _price0402 = dr.GetDecimal("Price0402");
            _price0403 = dr.GetDecimal("Price0403");
            _price0404 = dr.GetDecimal("Price0404");
            _price0405 = dr.GetDecimal("Price0405");
            _price0406 = dr.GetDecimal("Price0406");
            _price0407 = dr.GetDecimal("Price0407");
            _price0408 = dr.GetDecimal("Price0408");
            _price0409 = dr.GetDecimal("Price0409");
            _price0410 = dr.GetDecimal("Price0410");
            _price0411 = dr.GetDecimal("Price0411");
            _price0412 = dr.GetDecimal("Price0412");
            _price0413 = dr.GetDecimal("Price0413");
            _price0414 = dr.GetDecimal("Price0414");
            _price0415 = dr.GetDecimal("Price0415");
            _price0501 = dr.GetDecimal("Price0501");
            _price0502 = dr.GetDecimal("Price0502");
            _price0503 = dr.GetDecimal("Price0503");
            _price0504 = dr.GetDecimal("Price0504");
            _price0505 = dr.GetDecimal("Price0505");
            _price0506 = dr.GetDecimal("Price0506");
            _price0507 = dr.GetDecimal("Price0507");
            _price0508 = dr.GetDecimal("Price0508");
            _price0509 = dr.GetDecimal("Price0509");
            _price0510 = dr.GetDecimal("Price0510");
            _price0511 = dr.GetDecimal("Price0511");
            _price0512 = dr.GetDecimal("Price0512");
            _price0513 = dr.GetDecimal("Price0513");
            _price0514 = dr.GetDecimal("Price0514");
            _price0515 = dr.GetDecimal("Price0515");
            _price0601 = dr.GetDecimal("Price0601");
            _price0602 = dr.GetDecimal("Price0602");
            _price0603 = dr.GetDecimal("Price0603");
            _price0604 = dr.GetDecimal("Price0604");
            _price0605 = dr.GetDecimal("Price0605");
            _price0606 = dr.GetDecimal("Price0606");
            _price0607 = dr.GetDecimal("Price0607");
            _price0608 = dr.GetDecimal("Price0608");
            _price0609 = dr.GetDecimal("Price0609");
            _price0610 = dr.GetDecimal("Price0610");
            _price0611 = dr.GetDecimal("Price0611");
            _price0612 = dr.GetDecimal("Price0612");
            _price0613 = dr.GetDecimal("Price0613");
            _price0614 = dr.GetDecimal("Price0614");
            _price0615 = dr.GetDecimal("Price0615");
            _price0701 = dr.GetDecimal("Price0701");
            _price0702 = dr.GetDecimal("Price0702");
            _price0703 = dr.GetDecimal("Price0703");
            _price0704 = dr.GetDecimal("Price0704");
            _price0705 = dr.GetDecimal("Price0705");
            _price0706 = dr.GetDecimal("Price0706");
            _price0707 = dr.GetDecimal("Price0707");
            _price0708 = dr.GetDecimal("Price0708");
            _price0709 = dr.GetDecimal("Price0709");
            _price0710 = dr.GetDecimal("Price0710");
            _price0711 = dr.GetDecimal("Price0711");
            _price0712 = dr.GetDecimal("Price0712");
            _price0713 = dr.GetDecimal("Price0713");
            _price0714 = dr.GetDecimal("Price0714");
            _price0715 = dr.GetDecimal("Price0715");
            _price0801 = dr.GetDecimal("Price0801");
            _price0802 = dr.GetDecimal("Price0802");
            _price0803 = dr.GetDecimal("Price0803");
            _price0804 = dr.GetDecimal("Price0804");
            _price0805 = dr.GetDecimal("Price0805");
            _price0806 = dr.GetDecimal("Price0806");
            _price0807 = dr.GetDecimal("Price0807");
            _price0808 = dr.GetDecimal("Price0808");
            _price0809 = dr.GetDecimal("Price0809");
            _price0810 = dr.GetDecimal("Price0810");
            _price0811 = dr.GetDecimal("Price0811");
            _price0812 = dr.GetDecimal("Price0812");
            _price0813 = dr.GetDecimal("Price0813");
            _price0814 = dr.GetDecimal("Price0814");
            _price0815 = dr.GetDecimal("Price0815");
            _price0901 = dr.GetDecimal("Price0901");
            _price0902 = dr.GetDecimal("Price0902");
            _price0903 = dr.GetDecimal("Price0903");
            _price0904 = dr.GetDecimal("Price0904");
            _price0905 = dr.GetDecimal("Price0905");
            _price0906 = dr.GetDecimal("Price0906");
            _price0907 = dr.GetDecimal("Price0907");
            _price0908 = dr.GetDecimal("Price0908");
            _price0909 = dr.GetDecimal("Price0909");
            _price0910 = dr.GetDecimal("Price0910");
            _price0911 = dr.GetDecimal("Price0911");
            _price0912 = dr.GetDecimal("Price0912");
            _price0913 = dr.GetDecimal("Price0913");
            _price0914 = dr.GetDecimal("Price0914");
            _price0915 = dr.GetDecimal("Price0915");
            _price1001 = dr.GetDecimal("Price1001");
            _price1002 = dr.GetDecimal("Price1002");
            _price1003 = dr.GetDecimal("Price1003");
            _price1004 = dr.GetDecimal("Price1004");
            _price1005 = dr.GetDecimal("Price1005");
            _price1006 = dr.GetDecimal("Price1006");
            _price1007 = dr.GetDecimal("Price1007");
            _price1008 = dr.GetDecimal("Price1008");
            _price1009 = dr.GetDecimal("Price1009");
            _price1010 = dr.GetDecimal("Price1010");
            _price1011 = dr.GetDecimal("Price1011");
            _price1012 = dr.GetDecimal("Price1012");
            _price1013 = dr.GetDecimal("Price1013");
            _price1014 = dr.GetDecimal("Price1014");
            _price1015 = dr.GetDecimal("Price1015");
            _price1101 = dr.GetDecimal("Price1101");
            _price1102 = dr.GetDecimal("Price1102");
            _price1103 = dr.GetDecimal("Price1103");
            _price1104 = dr.GetDecimal("Price1104");
            _price1105 = dr.GetDecimal("Price1105");
            _price1106 = dr.GetDecimal("Price1106");
            _price1107 = dr.GetDecimal("Price1107");
            _price1108 = dr.GetDecimal("Price1108");
            _price1109 = dr.GetDecimal("Price1109");
            _price1110 = dr.GetDecimal("Price1110");
            _price1111 = dr.GetDecimal("Price1111");
            _price1112 = dr.GetDecimal("Price1112");
            _price1113 = dr.GetDecimal("Price1113");
            _price1114 = dr.GetDecimal("Price1114");
            _price1115 = dr.GetDecimal("Price1115");
            _price1201 = dr.GetDecimal("Price1201");
            _price1202 = dr.GetDecimal("Price1202");
            _price1203 = dr.GetDecimal("Price1203");
            _price1204 = dr.GetDecimal("Price1204");
            _price1205 = dr.GetDecimal("Price1205");
            _price1206 = dr.GetDecimal("Price1206");
            _price1207 = dr.GetDecimal("Price1207");
            _price1208 = dr.GetDecimal("Price1208");
            _price1209 = dr.GetDecimal("Price1209");
            _price1210 = dr.GetDecimal("Price1210");
            _price1211 = dr.GetDecimal("Price1211");
            _price1212 = dr.GetDecimal("Price1212");
            _price1213 = dr.GetDecimal("Price1213");
            _price1214 = dr.GetDecimal("Price1214");
            _price1215 = dr.GetDecimal("Price1215");
            _price1301 = dr.GetDecimal("Price1301");
            _price1302 = dr.GetDecimal("Price1302");
            _price1303 = dr.GetDecimal("Price1303");
            _price1304 = dr.GetDecimal("Price1304");
            _price1305 = dr.GetDecimal("Price1305");
            _price1306 = dr.GetDecimal("Price1306");
            _price1307 = dr.GetDecimal("Price1307");
            _price1308 = dr.GetDecimal("Price1308");
            _price1309 = dr.GetDecimal("Price1309");
            _price1310 = dr.GetDecimal("Price1310");
            _price1311 = dr.GetDecimal("Price1311");
            _price1312 = dr.GetDecimal("Price1312");
            _price1313 = dr.GetDecimal("Price1313");
            _price1314 = dr.GetDecimal("Price1314");
            _price1315 = dr.GetDecimal("Price1315");
            _price1401 = dr.GetDecimal("Price1401");
            _price1402 = dr.GetDecimal("Price1402");
            _price1403 = dr.GetDecimal("Price1403");
            _price1404 = dr.GetDecimal("Price1404");
            _price1405 = dr.GetDecimal("Price1405");
            _price1406 = dr.GetDecimal("Price1406");
            _price1407 = dr.GetDecimal("Price1407");
            _price1408 = dr.GetDecimal("Price1408");
            _price1409 = dr.GetDecimal("Price1409");
            _price1410 = dr.GetDecimal("Price1410");
            _price1411 = dr.GetDecimal("Price1411");
            _price1412 = dr.GetDecimal("Price1412");
            _price1413 = dr.GetDecimal("Price1413");
            _price1414 = dr.GetDecimal("Price1414");
            _price1415 = dr.GetDecimal("Price1415");
            _price1501 = dr.GetDecimal("Price1501");
            _price1502 = dr.GetDecimal("Price1502");
            _price1503 = dr.GetDecimal("Price1503");
            _price1504 = dr.GetDecimal("Price1504");
            _price1505 = dr.GetDecimal("Price1505");
            _price1506 = dr.GetDecimal("Price1506");
            _price1507 = dr.GetDecimal("Price1507");
            _price1508 = dr.GetDecimal("Price1508");
            _price1509 = dr.GetDecimal("Price1509");
            _price1510 = dr.GetDecimal("Price1510");
            _price1511 = dr.GetDecimal("Price1511");
            _price1512 = dr.GetDecimal("Price1512");
            _price1513 = dr.GetDecimal("Price1513");
            _price1514 = dr.GetDecimal("Price1514");
            _price1515 = dr.GetDecimal("Price1515");
            _ratio1 = dr.GetDecimal("Ratio1");
            _ratio2 = dr.GetDecimal("Ratio2");
            _ratio3 = dr.GetDecimal("Ratio3");
            _ratio4 = dr.GetDecimal("Ratio4");
            _ratio5 = dr.GetDecimal("Ratio5");
            _ratio6 = dr.GetDecimal("Ratio6");
            _ratio7 = dr.GetDecimal("Ratio7");
            _ratio8 = dr.GetDecimal("Ratio8");
            _ratio9 = dr.GetDecimal("Ratio9");
            _ratio10 = dr.GetDecimal("Ratio10");
            _ratio11 = dr.GetDecimal("Ratio11");
            _ratio12 = dr.GetDecimal("Ratio12");
            _ratio13 = dr.GetDecimal("Ratio13");
            _ratio14 = dr.GetDecimal("Ratio14");
            _ratio15 = dr.GetDecimal("Ratio15");
            _qtyDisQty1 = dr.GetDecimal("QtyDisQty1");
            _qtyDisQty2 = dr.GetDecimal("QtyDisQty2");
            _qtyDisQty3 = dr.GetDecimal("QtyDisQty3");
            _qtyDisQty4 = dr.GetDecimal("QtyDisQty4");
            _qtyDisQty5 = dr.GetDecimal("QtyDisQty5");
            _qtyDisRatio1 = dr.GetDecimal("QtyDisRatio1");
            _qtyDisRatio2 = dr.GetDecimal("QtyDisRatio2");
            _qtyDisRatio3 = dr.GetDecimal("QtyDisRatio3");
            _qtyDisRatio4 = dr.GetDecimal("QtyDisRatio4");
            _qtyDisRatio5 = dr.GetDecimal("QtyDisRatio5");
            _standardCost1 = dr.GetDecimal("StandardCost1");
            _standardCost2 = dr.GetDecimal("StandardCost2");
            _standardCost3 = dr.GetDecimal("StandardCost3");
            _standardCost4 = dr.GetDecimal("StandardCost4");
            _standardCost5 = dr.GetDecimal("StandardCost5");
            _standardCost6 = dr.GetDecimal("StandardCost6");
            _standardCost7 = dr.GetDecimal("StandardCost7");
            _standardCost8 = dr.GetDecimal("StandardCost8");
            _standardCost9 = dr.GetDecimal("StandardCost9");
            _standardCost10 = dr.GetDecimal("StandardCost10");
            _standardCost11 = dr.GetDecimal("StandardCost11");
            _standardCost12 = dr.GetDecimal("StandardCost12");
            _standardCost13 = dr.GetDecimal("StandardCost13");
            _standardCost14 = dr.GetDecimal("StandardCost14");
            _standardCost15 = dr.GetDecimal("StandardCost15");
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
            ValidationRules.CheckRules();
            return true;
            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(int? headerKey)
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

                    // Call insert method.
                    retValue = this.Insert(cn, headerKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, int? headerKey)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetPrice_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                //if (_itmKey == null)
                //    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                //else
                    cm.Parameters.AddWithValue("@ItmKey", headerKey);

                if (_standardPrice1 == null)
                    cm.Parameters.AddWithValue("@StandardPrice1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice1", _standardPrice1);

                if (_standardPrice2 == null)
                    cm.Parameters.AddWithValue("@StandardPrice2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice2", _standardPrice2);

                if (_standardPrice3 == null)
                    cm.Parameters.AddWithValue("@StandardPrice3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice3", _standardPrice3);

                if (_standardPrice4 == null)
                    cm.Parameters.AddWithValue("@StandardPrice4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice4", _standardPrice4);

                if (_standardPrice5 == null)
                    cm.Parameters.AddWithValue("@StandardPrice5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice5", _standardPrice5);

                if (_standardPrice6 == null)
                    cm.Parameters.AddWithValue("@StandardPrice6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice6", _standardPrice6);

                if (_standardPrice7 == null)
                    cm.Parameters.AddWithValue("@StandardPrice7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice7", _standardPrice7);

                if (_standardPrice8 == null)
                    cm.Parameters.AddWithValue("@StandardPrice8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice8", _standardPrice8);

                if (_standardPrice9 == null)
                    cm.Parameters.AddWithValue("@StandardPrice9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice9", _standardPrice9);

                if (_standardPrice10 == null)
                    cm.Parameters.AddWithValue("@StandardPrice10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice10", _standardPrice10);

                if (_standardPrice11 == null)
                    cm.Parameters.AddWithValue("@StandardPrice11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice11", _standardPrice11);

                if (_standardPrice12 == null)
                    cm.Parameters.AddWithValue("@StandardPrice12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice12", _standardPrice12);

                if (_standardPrice13 == null)
                    cm.Parameters.AddWithValue("@StandardPrice13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice13", _standardPrice13);

                if (_standardPrice14 == null)
                    cm.Parameters.AddWithValue("@StandardPrice14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice14", _standardPrice14);

                if (_standardPrice15 == null)
                    cm.Parameters.AddWithValue("@StandardPrice15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice15", _standardPrice15);

                if (_price0101 == null)
                    cm.Parameters.AddWithValue("@Price0101", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0101", _price0101);

                if (_price0102 == null)
                    cm.Parameters.AddWithValue("@Price0102", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0102", _price0102);

                if (_price0103 == null)
                    cm.Parameters.AddWithValue("@Price0103", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0103", _price0103);

                if (_price0104 == null)
                    cm.Parameters.AddWithValue("@Price0104", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0104", _price0104);

                if (_price0105 == null)
                    cm.Parameters.AddWithValue("@Price0105", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0105", _price0105);

                if (_price0106 == null)
                    cm.Parameters.AddWithValue("@Price0106", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0106", _price0106);

                if (_price0107 == null)
                    cm.Parameters.AddWithValue("@Price0107", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0107", _price0107);

                if (_price0108 == null)
                    cm.Parameters.AddWithValue("@Price0108", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0108", _price0108);

                if (_price0109 == null)
                    cm.Parameters.AddWithValue("@Price0109", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0109", _price0109);

                if (_price0110 == null)
                    cm.Parameters.AddWithValue("@Price0110", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0110", _price0110);

                if (_price0111 == null)
                    cm.Parameters.AddWithValue("@Price0111", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0111", _price0111);

                if (_price0112 == null)
                    cm.Parameters.AddWithValue("@Price0112", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0112", _price0112);

                if (_price0113 == null)
                    cm.Parameters.AddWithValue("@Price0113", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0113", _price0113);

                if (_price0114 == null)
                    cm.Parameters.AddWithValue("@Price0114", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0114", _price0114);

                if (_price0115 == null)
                    cm.Parameters.AddWithValue("@Price0115", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0115", _price0115);

                if (_price0201 == null)
                    cm.Parameters.AddWithValue("@Price0201", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0201", _price0201);

                if (_price0202 == null)
                    cm.Parameters.AddWithValue("@Price0202", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0202", _price0202);

                if (_price0203 == null)
                    cm.Parameters.AddWithValue("@Price0203", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0203", _price0203);

                if (_price0204 == null)
                    cm.Parameters.AddWithValue("@Price0204", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0204", _price0204);

                if (_price0205 == null)
                    cm.Parameters.AddWithValue("@Price0205", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0205", _price0205);

                if (_price0206 == null)
                    cm.Parameters.AddWithValue("@Price0206", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0206", _price0206);

                if (_price0207 == null)
                    cm.Parameters.AddWithValue("@Price0207", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0207", _price0207);

                if (_price0208 == null)
                    cm.Parameters.AddWithValue("@Price0208", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0208", _price0208);

                if (_price0209 == null)
                    cm.Parameters.AddWithValue("@Price0209", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0209", _price0209);

                if (_price0210 == null)
                    cm.Parameters.AddWithValue("@Price0210", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0210", _price0210);

                if (_price0211 == null)
                    cm.Parameters.AddWithValue("@Price0211", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0211", _price0211);

                if (_price0212 == null)
                    cm.Parameters.AddWithValue("@Price0212", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0212", _price0212);

                if (_price0213 == null)
                    cm.Parameters.AddWithValue("@Price0213", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0213", _price0213);

                if (_price0214 == null)
                    cm.Parameters.AddWithValue("@Price0214", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0214", _price0214);

                if (_price0215 == null)
                    cm.Parameters.AddWithValue("@Price0215", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0215", _price0215);

                if (_price0301 == null)
                    cm.Parameters.AddWithValue("@Price0301", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0301", _price0301);

                if (_price0302 == null)
                    cm.Parameters.AddWithValue("@Price0302", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0302", _price0302);

                if (_price0303 == null)
                    cm.Parameters.AddWithValue("@Price0303", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0303", _price0303);

                if (_price0304 == null)
                    cm.Parameters.AddWithValue("@Price0304", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0304", _price0304);

                if (_price0305 == null)
                    cm.Parameters.AddWithValue("@Price0305", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0305", _price0305);

                if (_price0306 == null)
                    cm.Parameters.AddWithValue("@Price0306", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0306", _price0306);

                if (_price0307 == null)
                    cm.Parameters.AddWithValue("@Price0307", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0307", _price0307);

                if (_price0308 == null)
                    cm.Parameters.AddWithValue("@Price0308", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0308", _price0308);

                if (_price0309 == null)
                    cm.Parameters.AddWithValue("@Price0309", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0309", _price0309);

                if (_price0310 == null)
                    cm.Parameters.AddWithValue("@Price0310", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0310", _price0310);

                if (_price0311 == null)
                    cm.Parameters.AddWithValue("@Price0311", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0311", _price0311);

                if (_price0312 == null)
                    cm.Parameters.AddWithValue("@Price0312", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0312", _price0312);

                if (_price0313 == null)
                    cm.Parameters.AddWithValue("@Price0313", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0313", _price0313);

                if (_price0314 == null)
                    cm.Parameters.AddWithValue("@Price0314", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0314", _price0314);

                if (_price0315 == null)
                    cm.Parameters.AddWithValue("@Price0315", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0315", _price0315);

                if (_price0401 == null)
                    cm.Parameters.AddWithValue("@Price0401", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0401", _price0401);

                if (_price0402 == null)
                    cm.Parameters.AddWithValue("@Price0402", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0402", _price0402);

                if (_price0403 == null)
                    cm.Parameters.AddWithValue("@Price0403", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0403", _price0403);

                if (_price0404 == null)
                    cm.Parameters.AddWithValue("@Price0404", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0404", _price0404);

                if (_price0405 == null)
                    cm.Parameters.AddWithValue("@Price0405", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0405", _price0405);

                if (_price0406 == null)
                    cm.Parameters.AddWithValue("@Price0406", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0406", _price0406);

                if (_price0407 == null)
                    cm.Parameters.AddWithValue("@Price0407", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0407", _price0407);

                if (_price0408 == null)
                    cm.Parameters.AddWithValue("@Price0408", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0408", _price0408);

                if (_price0409 == null)
                    cm.Parameters.AddWithValue("@Price0409", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0409", _price0409);

                if (_price0410 == null)
                    cm.Parameters.AddWithValue("@Price0410", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0410", _price0410);

                if (_price0411 == null)
                    cm.Parameters.AddWithValue("@Price0411", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0411", _price0411);

                if (_price0412 == null)
                    cm.Parameters.AddWithValue("@Price0412", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0412", _price0412);

                if (_price0413 == null)
                    cm.Parameters.AddWithValue("@Price0413", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0413", _price0413);

                if (_price0414 == null)
                    cm.Parameters.AddWithValue("@Price0414", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0414", _price0414);

                if (_price0415 == null)
                    cm.Parameters.AddWithValue("@Price0415", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0415", _price0415);

                if (_price0501 == null)
                    cm.Parameters.AddWithValue("@Price0501", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0501", _price0501);

                if (_price0502 == null)
                    cm.Parameters.AddWithValue("@Price0502", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0502", _price0502);

                if (_price0503 == null)
                    cm.Parameters.AddWithValue("@Price0503", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0503", _price0503);

                if (_price0504 == null)
                    cm.Parameters.AddWithValue("@Price0504", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0504", _price0504);

                if (_price0505 == null)
                    cm.Parameters.AddWithValue("@Price0505", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0505", _price0505);

                if (_price0506 == null)
                    cm.Parameters.AddWithValue("@Price0506", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0506", _price0506);

                if (_price0507 == null)
                    cm.Parameters.AddWithValue("@Price0507", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0507", _price0507);

                if (_price0508 == null)
                    cm.Parameters.AddWithValue("@Price0508", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0508", _price0508);

                if (_price0509 == null)
                    cm.Parameters.AddWithValue("@Price0509", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0509", _price0509);

                if (_price0510 == null)
                    cm.Parameters.AddWithValue("@Price0510", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0510", _price0510);

                if (_price0511 == null)
                    cm.Parameters.AddWithValue("@Price0511", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0511", _price0511);

                if (_price0512 == null)
                    cm.Parameters.AddWithValue("@Price0512", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0512", _price0512);

                if (_price0513 == null)
                    cm.Parameters.AddWithValue("@Price0513", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0513", _price0513);

                if (_price0514 == null)
                    cm.Parameters.AddWithValue("@Price0514", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0514", _price0514);

                if (_price0515 == null)
                    cm.Parameters.AddWithValue("@Price0515", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0515", _price0515);

                if (_price0601 == null)
                    cm.Parameters.AddWithValue("@Price0601", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0601", _price0601);

                if (_price0602 == null)
                    cm.Parameters.AddWithValue("@Price0602", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0602", _price0602);

                if (_price0603 == null)
                    cm.Parameters.AddWithValue("@Price0603", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0603", _price0603);

                if (_price0604 == null)
                    cm.Parameters.AddWithValue("@Price0604", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0604", _price0604);

                if (_price0605 == null)
                    cm.Parameters.AddWithValue("@Price0605", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0605", _price0605);

                if (_price0606 == null)
                    cm.Parameters.AddWithValue("@Price0606", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0606", _price0606);

                if (_price0607 == null)
                    cm.Parameters.AddWithValue("@Price0607", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0607", _price0607);

                if (_price0608 == null)
                    cm.Parameters.AddWithValue("@Price0608", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0608", _price0608);

                if (_price0609 == null)
                    cm.Parameters.AddWithValue("@Price0609", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0609", _price0609);

                if (_price0610 == null)
                    cm.Parameters.AddWithValue("@Price0610", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0610", _price0610);

                if (_price0611 == null)
                    cm.Parameters.AddWithValue("@Price0611", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0611", _price0611);

                if (_price0612 == null)
                    cm.Parameters.AddWithValue("@Price0612", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0612", _price0612);

                if (_price0613 == null)
                    cm.Parameters.AddWithValue("@Price0613", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0613", _price0613);

                if (_price0614 == null)
                    cm.Parameters.AddWithValue("@Price0614", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0614", _price0614);

                if (_price0615 == null)
                    cm.Parameters.AddWithValue("@Price0615", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0615", _price0615);

                if (_price0701 == null)
                    cm.Parameters.AddWithValue("@Price0701", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0701", _price0701);

                if (_price0702 == null)
                    cm.Parameters.AddWithValue("@Price0702", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0702", _price0702);

                if (_price0703 == null)
                    cm.Parameters.AddWithValue("@Price0703", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0703", _price0703);

                if (_price0704 == null)
                    cm.Parameters.AddWithValue("@Price0704", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0704", _price0704);

                if (_price0705 == null)
                    cm.Parameters.AddWithValue("@Price0705", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0705", _price0705);

                if (_price0706 == null)
                    cm.Parameters.AddWithValue("@Price0706", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0706", _price0706);

                if (_price0707 == null)
                    cm.Parameters.AddWithValue("@Price0707", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0707", _price0707);

                if (_price0708 == null)
                    cm.Parameters.AddWithValue("@Price0708", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0708", _price0708);

                if (_price0709 == null)
                    cm.Parameters.AddWithValue("@Price0709", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0709", _price0709);

                if (_price0710 == null)
                    cm.Parameters.AddWithValue("@Price0710", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0710", _price0710);

                if (_price0711 == null)
                    cm.Parameters.AddWithValue("@Price0711", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0711", _price0711);

                if (_price0712 == null)
                    cm.Parameters.AddWithValue("@Price0712", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0712", _price0712);

                if (_price0713 == null)
                    cm.Parameters.AddWithValue("@Price0713", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0713", _price0713);

                if (_price0714 == null)
                    cm.Parameters.AddWithValue("@Price0714", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0714", _price0714);

                if (_price0715 == null)
                    cm.Parameters.AddWithValue("@Price0715", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0715", _price0715);

                if (_price0801 == null)
                    cm.Parameters.AddWithValue("@Price0801", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0801", _price0801);

                if (_price0802 == null)
                    cm.Parameters.AddWithValue("@Price0802", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0802", _price0802);

                if (_price0803 == null)
                    cm.Parameters.AddWithValue("@Price0803", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0803", _price0803);

                if (_price0804 == null)
                    cm.Parameters.AddWithValue("@Price0804", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0804", _price0804);

                if (_price0805 == null)
                    cm.Parameters.AddWithValue("@Price0805", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0805", _price0805);

                if (_price0806 == null)
                    cm.Parameters.AddWithValue("@Price0806", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0806", _price0806);

                if (_price0807 == null)
                    cm.Parameters.AddWithValue("@Price0807", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0807", _price0807);

                if (_price0808 == null)
                    cm.Parameters.AddWithValue("@Price0808", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0808", _price0808);

                if (_price0809 == null)
                    cm.Parameters.AddWithValue("@Price0809", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0809", _price0809);

                if (_price0810 == null)
                    cm.Parameters.AddWithValue("@Price0810", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0810", _price0810);

                if (_price0811 == null)
                    cm.Parameters.AddWithValue("@Price0811", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0811", _price0811);

                if (_price0812 == null)
                    cm.Parameters.AddWithValue("@Price0812", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0812", _price0812);

                if (_price0813 == null)
                    cm.Parameters.AddWithValue("@Price0813", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0813", _price0813);

                if (_price0814 == null)
                    cm.Parameters.AddWithValue("@Price0814", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0814", _price0814);

                if (_price0815 == null)
                    cm.Parameters.AddWithValue("@Price0815", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0815", _price0815);

                if (_price0901 == null)
                    cm.Parameters.AddWithValue("@Price0901", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0901", _price0901);

                if (_price0902 == null)
                    cm.Parameters.AddWithValue("@Price0902", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0902", _price0902);

                if (_price0903 == null)
                    cm.Parameters.AddWithValue("@Price0903", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0903", _price0903);

                if (_price0904 == null)
                    cm.Parameters.AddWithValue("@Price0904", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0904", _price0904);

                if (_price0905 == null)
                    cm.Parameters.AddWithValue("@Price0905", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0905", _price0905);

                if (_price0906 == null)
                    cm.Parameters.AddWithValue("@Price0906", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0906", _price0906);

                if (_price0907 == null)
                    cm.Parameters.AddWithValue("@Price0907", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0907", _price0907);

                if (_price0908 == null)
                    cm.Parameters.AddWithValue("@Price0908", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0908", _price0908);

                if (_price0909 == null)
                    cm.Parameters.AddWithValue("@Price0909", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0909", _price0909);

                if (_price0910 == null)
                    cm.Parameters.AddWithValue("@Price0910", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0910", _price0910);

                if (_price0911 == null)
                    cm.Parameters.AddWithValue("@Price0911", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0911", _price0911);

                if (_price0912 == null)
                    cm.Parameters.AddWithValue("@Price0912", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0912", _price0912);

                if (_price0913 == null)
                    cm.Parameters.AddWithValue("@Price0913", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0913", _price0913);

                if (_price0914 == null)
                    cm.Parameters.AddWithValue("@Price0914", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0914", _price0914);

                if (_price0915 == null)
                    cm.Parameters.AddWithValue("@Price0915", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0915", _price0915);

                if (_price1001 == null)
                    cm.Parameters.AddWithValue("@Price1001", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1001", _price1001);

                if (_price1002 == null)
                    cm.Parameters.AddWithValue("@Price1002", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1002", _price1002);

                if (_price1003 == null)
                    cm.Parameters.AddWithValue("@Price1003", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1003", _price1003);

                if (_price1004 == null)
                    cm.Parameters.AddWithValue("@Price1004", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1004", _price1004);

                if (_price1005 == null)
                    cm.Parameters.AddWithValue("@Price1005", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1005", _price1005);

                if (_price1006 == null)
                    cm.Parameters.AddWithValue("@Price1006", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1006", _price1006);

                if (_price1007 == null)
                    cm.Parameters.AddWithValue("@Price1007", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1007", _price1007);

                if (_price1008 == null)
                    cm.Parameters.AddWithValue("@Price1008", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1008", _price1008);

                if (_price1009 == null)
                    cm.Parameters.AddWithValue("@Price1009", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1009", _price1009);

                if (_price1010 == null)
                    cm.Parameters.AddWithValue("@Price1010", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1010", _price1010);

                if (_price1011 == null)
                    cm.Parameters.AddWithValue("@Price1011", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1011", _price1011);

                if (_price1012 == null)
                    cm.Parameters.AddWithValue("@Price1012", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1012", _price1012);

                if (_price1013 == null)
                    cm.Parameters.AddWithValue("@Price1013", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1013", _price1013);

                if (_price1014 == null)
                    cm.Parameters.AddWithValue("@Price1014", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1014", _price1014);

                if (_price1015 == null)
                    cm.Parameters.AddWithValue("@Price1015", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1015", _price1015);

                if (_price1101 == null)
                    cm.Parameters.AddWithValue("@Price1101", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1101", _price1101);

                if (_price1102 == null)
                    cm.Parameters.AddWithValue("@Price1102", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1102", _price1102);

                if (_price1103 == null)
                    cm.Parameters.AddWithValue("@Price1103", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1103", _price1103);

                if (_price1104 == null)
                    cm.Parameters.AddWithValue("@Price1104", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1104", _price1104);

                if (_price1105 == null)
                    cm.Parameters.AddWithValue("@Price1105", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1105", _price1105);

                if (_price1106 == null)
                    cm.Parameters.AddWithValue("@Price1106", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1106", _price1106);

                if (_price1107 == null)
                    cm.Parameters.AddWithValue("@Price1107", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1107", _price1107);

                if (_price1108 == null)
                    cm.Parameters.AddWithValue("@Price1108", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1108", _price1108);

                if (_price1109 == null)
                    cm.Parameters.AddWithValue("@Price1109", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1109", _price1109);

                if (_price1110 == null)
                    cm.Parameters.AddWithValue("@Price1110", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1110", _price1110);

                if (_price1111 == null)
                    cm.Parameters.AddWithValue("@Price1111", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1111", _price1111);

                if (_price1112 == null)
                    cm.Parameters.AddWithValue("@Price1112", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1112", _price1112);

                if (_price1113 == null)
                    cm.Parameters.AddWithValue("@Price1113", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1113", _price1113);

                if (_price1114 == null)
                    cm.Parameters.AddWithValue("@Price1114", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1114", _price1114);

                if (_price1115 == null)
                    cm.Parameters.AddWithValue("@Price1115", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1115", _price1115);

                if (_price1201 == null)
                    cm.Parameters.AddWithValue("@Price1201", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1201", _price1201);

                if (_price1202 == null)
                    cm.Parameters.AddWithValue("@Price1202", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1202", _price1202);

                if (_price1203 == null)
                    cm.Parameters.AddWithValue("@Price1203", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1203", _price1203);

                if (_price1204 == null)
                    cm.Parameters.AddWithValue("@Price1204", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1204", _price1204);

                if (_price1205 == null)
                    cm.Parameters.AddWithValue("@Price1205", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1205", _price1205);

                if (_price1206 == null)
                    cm.Parameters.AddWithValue("@Price1206", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1206", _price1206);

                if (_price1207 == null)
                    cm.Parameters.AddWithValue("@Price1207", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1207", _price1207);

                if (_price1208 == null)
                    cm.Parameters.AddWithValue("@Price1208", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1208", _price1208);

                if (_price1209 == null)
                    cm.Parameters.AddWithValue("@Price1209", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1209", _price1209);

                if (_price1210 == null)
                    cm.Parameters.AddWithValue("@Price1210", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1210", _price1210);

                if (_price1211 == null)
                    cm.Parameters.AddWithValue("@Price1211", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1211", _price1211);

                if (_price1212 == null)
                    cm.Parameters.AddWithValue("@Price1212", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1212", _price1212);

                if (_price1213 == null)
                    cm.Parameters.AddWithValue("@Price1213", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1213", _price1213);

                if (_price1214 == null)
                    cm.Parameters.AddWithValue("@Price1214", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1214", _price1214);

                if (_price1215 == null)
                    cm.Parameters.AddWithValue("@Price1215", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1215", _price1215);

                if (_price1301 == null)
                    cm.Parameters.AddWithValue("@Price1301", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1301", _price1301);

                if (_price1302 == null)
                    cm.Parameters.AddWithValue("@Price1302", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1302", _price1302);

                if (_price1303 == null)
                    cm.Parameters.AddWithValue("@Price1303", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1303", _price1303);

                if (_price1304 == null)
                    cm.Parameters.AddWithValue("@Price1304", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1304", _price1304);

                if (_price1305 == null)
                    cm.Parameters.AddWithValue("@Price1305", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1305", _price1305);

                if (_price1306 == null)
                    cm.Parameters.AddWithValue("@Price1306", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1306", _price1306);

                if (_price1307 == null)
                    cm.Parameters.AddWithValue("@Price1307", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1307", _price1307);

                if (_price1308 == null)
                    cm.Parameters.AddWithValue("@Price1308", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1308", _price1308);

                if (_price1309 == null)
                    cm.Parameters.AddWithValue("@Price1309", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1309", _price1309);

                if (_price1310 == null)
                    cm.Parameters.AddWithValue("@Price1310", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1310", _price1310);

                if (_price1311 == null)
                    cm.Parameters.AddWithValue("@Price1311", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1311", _price1311);

                if (_price1312 == null)
                    cm.Parameters.AddWithValue("@Price1312", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1312", _price1312);

                if (_price1313 == null)
                    cm.Parameters.AddWithValue("@Price1313", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1313", _price1313);

                if (_price1314 == null)
                    cm.Parameters.AddWithValue("@Price1314", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1314", _price1314);

                if (_price1315 == null)
                    cm.Parameters.AddWithValue("@Price1315", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1315", _price1315);

                if (_price1401 == null)
                    cm.Parameters.AddWithValue("@Price1401", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1401", _price1401);

                if (_price1402 == null)
                    cm.Parameters.AddWithValue("@Price1402", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1402", _price1402);

                if (_price1403 == null)
                    cm.Parameters.AddWithValue("@Price1403", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1403", _price1403);

                if (_price1404 == null)
                    cm.Parameters.AddWithValue("@Price1404", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1404", _price1404);

                if (_price1405 == null)
                    cm.Parameters.AddWithValue("@Price1405", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1405", _price1405);

                if (_price1406 == null)
                    cm.Parameters.AddWithValue("@Price1406", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1406", _price1406);

                if (_price1407 == null)
                    cm.Parameters.AddWithValue("@Price1407", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1407", _price1407);

                if (_price1408 == null)
                    cm.Parameters.AddWithValue("@Price1408", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1408", _price1408);

                if (_price1409 == null)
                    cm.Parameters.AddWithValue("@Price1409", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1409", _price1409);

                if (_price1410 == null)
                    cm.Parameters.AddWithValue("@Price1410", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1410", _price1410);

                if (_price1411 == null)
                    cm.Parameters.AddWithValue("@Price1411", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1411", _price1411);

                if (_price1412 == null)
                    cm.Parameters.AddWithValue("@Price1412", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1412", _price1412);

                if (_price1413 == null)
                    cm.Parameters.AddWithValue("@Price1413", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1413", _price1413);

                if (_price1414 == null)
                    cm.Parameters.AddWithValue("@Price1414", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1414", _price1414);

                if (_price1415 == null)
                    cm.Parameters.AddWithValue("@Price1415", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1415", _price1415);

                if (_price1501 == null)
                    cm.Parameters.AddWithValue("@Price1501", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1501", _price1501);

                if (_price1502 == null)
                    cm.Parameters.AddWithValue("@Price1502", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1502", _price1502);

                if (_price1503 == null)
                    cm.Parameters.AddWithValue("@Price1503", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1503", _price1503);

                if (_price1504 == null)
                    cm.Parameters.AddWithValue("@Price1504", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1504", _price1504);

                if (_price1505 == null)
                    cm.Parameters.AddWithValue("@Price1505", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1505", _price1505);

                if (_price1506 == null)
                    cm.Parameters.AddWithValue("@Price1506", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1506", _price1506);

                if (_price1507 == null)
                    cm.Parameters.AddWithValue("@Price1507", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1507", _price1507);

                if (_price1508 == null)
                    cm.Parameters.AddWithValue("@Price1508", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1508", _price1508);

                if (_price1509 == null)
                    cm.Parameters.AddWithValue("@Price1509", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1509", _price1509);

                if (_price1510 == null)
                    cm.Parameters.AddWithValue("@Price1510", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1510", _price1510);

                if (_price1511 == null)
                    cm.Parameters.AddWithValue("@Price1511", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1511", _price1511);

                if (_price1512 == null)
                    cm.Parameters.AddWithValue("@Price1512", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1512", _price1512);

                if (_price1513 == null)
                    cm.Parameters.AddWithValue("@Price1513", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1513", _price1513);

                if (_price1514 == null)
                    cm.Parameters.AddWithValue("@Price1514", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1514", _price1514);

                if (_price1515 == null)
                    cm.Parameters.AddWithValue("@Price1515", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1515", _price1515);

                if (_ratio1 == null)
                    cm.Parameters.AddWithValue("@Ratio1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio1", _ratio1);

                if (_ratio2 == null)
                    cm.Parameters.AddWithValue("@Ratio2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio2", _ratio2);

                if (_ratio3 == null)
                    cm.Parameters.AddWithValue("@Ratio3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio3", _ratio3);

                if (_ratio4 == null)
                    cm.Parameters.AddWithValue("@Ratio4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio4", _ratio4);

                if (_ratio5 == null)
                    cm.Parameters.AddWithValue("@Ratio5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio5", _ratio5);

                if (_ratio6 == null)
                    cm.Parameters.AddWithValue("@Ratio6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio6", _ratio6);

                if (_ratio7 == null)
                    cm.Parameters.AddWithValue("@Ratio7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio7", _ratio7);

                if (_ratio8 == null)
                    cm.Parameters.AddWithValue("@Ratio8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio8", _ratio8);

                if (_ratio9 == null)
                    cm.Parameters.AddWithValue("@Ratio9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio9", _ratio9);

                if (_ratio10 == null)
                    cm.Parameters.AddWithValue("@Ratio10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio10", _ratio10);

                if (_ratio11 == null)
                    cm.Parameters.AddWithValue("@Ratio11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio11", _ratio11);

                if (_ratio12 == null)
                    cm.Parameters.AddWithValue("@Ratio12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio12", _ratio12);

                if (_ratio13 == null)
                    cm.Parameters.AddWithValue("@Ratio13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio13", _ratio13);

                if (_ratio14 == null)
                    cm.Parameters.AddWithValue("@Ratio14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio14", _ratio14);

                if (_ratio15 == null)
                    cm.Parameters.AddWithValue("@Ratio15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio15", _ratio15);

                if (_qtyDisQty1 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty1", _qtyDisQty1);

                if (_qtyDisQty2 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty2", _qtyDisQty2);

                if (_qtyDisQty3 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty3", _qtyDisQty3);

                if (_qtyDisQty4 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty4", _qtyDisQty4);

                if (_qtyDisQty5 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty5", _qtyDisQty5);

                if (_qtyDisRatio1 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio1", _qtyDisRatio1);

                if (_qtyDisRatio2 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio2", _qtyDisRatio2);

                if (_qtyDisRatio3 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio3", _qtyDisRatio3);

                if (_qtyDisRatio4 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio4", _qtyDisRatio4);

                if (_qtyDisRatio5 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio5", _qtyDisRatio5);

                if (_standardCost1 == null)
                    cm.Parameters.AddWithValue("@StandardCost1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost1", _standardCost1);

                if (_standardCost2 == null)
                    cm.Parameters.AddWithValue("@StandardCost2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost2", _standardCost2);

                if (_standardCost3 == null)
                    cm.Parameters.AddWithValue("@StandardCost3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost3", _standardCost3);

                if (_standardCost4 == null)
                    cm.Parameters.AddWithValue("@StandardCost4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost4", _standardCost4);

                if (_standardCost5 == null)
                    cm.Parameters.AddWithValue("@StandardCost5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost5", _standardCost5);

                if (_standardCost6 == null)
                    cm.Parameters.AddWithValue("@StandardCost6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost6", _standardCost6);

                if (_standardCost7 == null)
                    cm.Parameters.AddWithValue("@StandardCost7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost7", _standardCost7);

                if (_standardCost8 == null)
                    cm.Parameters.AddWithValue("@StandardCost8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost8", _standardCost8);

                if (_standardCost9 == null)
                    cm.Parameters.AddWithValue("@StandardCost9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost9", _standardCost9);

                if (_standardCost10 == null)
                    cm.Parameters.AddWithValue("@StandardCost10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost10", _standardCost10);

                if (_standardCost11 == null)
                    cm.Parameters.AddWithValue("@StandardCost11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost11", _standardCost11);

                if (_standardCost12 == null)
                    cm.Parameters.AddWithValue("@StandardCost12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost12", _standardCost12);

                if (_standardCost13 == null)
                    cm.Parameters.AddWithValue("@StandardCost13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost13", _standardCost13);

                if (_standardCost14 == null)
                    cm.Parameters.AddWithValue("@StandardCost14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost14", _standardCost14);

                if (_standardCost15 == null)
                    cm.Parameters.AddWithValue("@StandardCost15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost15", _standardCost15);

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
                cm.CommandText = "MSTItmDetPrice_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_standardPrice1 == null)
                    cm.Parameters.AddWithValue("@StandardPrice1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice1", _standardPrice1);

                if (_standardPrice2 == null)
                    cm.Parameters.AddWithValue("@StandardPrice2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice2", _standardPrice2);

                if (_standardPrice3 == null)
                    cm.Parameters.AddWithValue("@StandardPrice3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice3", _standardPrice3);

                if (_standardPrice4 == null)
                    cm.Parameters.AddWithValue("@StandardPrice4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice4", _standardPrice4);

                if (_standardPrice5 == null)
                    cm.Parameters.AddWithValue("@StandardPrice5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice5", _standardPrice5);

                if (_standardPrice6 == null)
                    cm.Parameters.AddWithValue("@StandardPrice6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice6", _standardPrice6);

                if (_standardPrice7 == null)
                    cm.Parameters.AddWithValue("@StandardPrice7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice7", _standardPrice7);

                if (_standardPrice8 == null)
                    cm.Parameters.AddWithValue("@StandardPrice8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice8", _standardPrice8);

                if (_standardPrice9 == null)
                    cm.Parameters.AddWithValue("@StandardPrice9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice9", _standardPrice9);

                if (_standardPrice10 == null)
                    cm.Parameters.AddWithValue("@StandardPrice10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice10", _standardPrice10);

                if (_standardPrice11 == null)
                    cm.Parameters.AddWithValue("@StandardPrice11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice11", _standardPrice11);

                if (_standardPrice12 == null)
                    cm.Parameters.AddWithValue("@StandardPrice12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice12", _standardPrice12);

                if (_standardPrice13 == null)
                    cm.Parameters.AddWithValue("@StandardPrice13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice13", _standardPrice13);

                if (_standardPrice14 == null)
                    cm.Parameters.AddWithValue("@StandardPrice14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice14", _standardPrice14);

                if (_standardPrice15 == null)
                    cm.Parameters.AddWithValue("@StandardPrice15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardPrice15", _standardPrice15);

                if (_price0101 == null)
                    cm.Parameters.AddWithValue("@Price0101", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0101", _price0101);

                if (_price0102 == null)
                    cm.Parameters.AddWithValue("@Price0102", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0102", _price0102);

                if (_price0103 == null)
                    cm.Parameters.AddWithValue("@Price0103", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0103", _price0103);

                if (_price0104 == null)
                    cm.Parameters.AddWithValue("@Price0104", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0104", _price0104);

                if (_price0105 == null)
                    cm.Parameters.AddWithValue("@Price0105", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0105", _price0105);

                if (_price0106 == null)
                    cm.Parameters.AddWithValue("@Price0106", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0106", _price0106);

                if (_price0107 == null)
                    cm.Parameters.AddWithValue("@Price0107", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0107", _price0107);

                if (_price0108 == null)
                    cm.Parameters.AddWithValue("@Price0108", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0108", _price0108);

                if (_price0109 == null)
                    cm.Parameters.AddWithValue("@Price0109", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0109", _price0109);

                if (_price0110 == null)
                    cm.Parameters.AddWithValue("@Price0110", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0110", _price0110);

                if (_price0111 == null)
                    cm.Parameters.AddWithValue("@Price0111", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0111", _price0111);

                if (_price0112 == null)
                    cm.Parameters.AddWithValue("@Price0112", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0112", _price0112);

                if (_price0113 == null)
                    cm.Parameters.AddWithValue("@Price0113", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0113", _price0113);

                if (_price0114 == null)
                    cm.Parameters.AddWithValue("@Price0114", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0114", _price0114);

                if (_price0115 == null)
                    cm.Parameters.AddWithValue("@Price0115", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0115", _price0115);

                if (_price0201 == null)
                    cm.Parameters.AddWithValue("@Price0201", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0201", _price0201);

                if (_price0202 == null)
                    cm.Parameters.AddWithValue("@Price0202", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0202", _price0202);

                if (_price0203 == null)
                    cm.Parameters.AddWithValue("@Price0203", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0203", _price0203);

                if (_price0204 == null)
                    cm.Parameters.AddWithValue("@Price0204", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0204", _price0204);

                if (_price0205 == null)
                    cm.Parameters.AddWithValue("@Price0205", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0205", _price0205);

                if (_price0206 == null)
                    cm.Parameters.AddWithValue("@Price0206", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0206", _price0206);

                if (_price0207 == null)
                    cm.Parameters.AddWithValue("@Price0207", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0207", _price0207);

                if (_price0208 == null)
                    cm.Parameters.AddWithValue("@Price0208", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0208", _price0208);

                if (_price0209 == null)
                    cm.Parameters.AddWithValue("@Price0209", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0209", _price0209);

                if (_price0210 == null)
                    cm.Parameters.AddWithValue("@Price0210", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0210", _price0210);

                if (_price0211 == null)
                    cm.Parameters.AddWithValue("@Price0211", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0211", _price0211);

                if (_price0212 == null)
                    cm.Parameters.AddWithValue("@Price0212", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0212", _price0212);

                if (_price0213 == null)
                    cm.Parameters.AddWithValue("@Price0213", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0213", _price0213);

                if (_price0214 == null)
                    cm.Parameters.AddWithValue("@Price0214", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0214", _price0214);

                if (_price0215 == null)
                    cm.Parameters.AddWithValue("@Price0215", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0215", _price0215);

                if (_price0301 == null)
                    cm.Parameters.AddWithValue("@Price0301", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0301", _price0301);

                if (_price0302 == null)
                    cm.Parameters.AddWithValue("@Price0302", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0302", _price0302);

                if (_price0303 == null)
                    cm.Parameters.AddWithValue("@Price0303", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0303", _price0303);

                if (_price0304 == null)
                    cm.Parameters.AddWithValue("@Price0304", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0304", _price0304);

                if (_price0305 == null)
                    cm.Parameters.AddWithValue("@Price0305", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0305", _price0305);

                if (_price0306 == null)
                    cm.Parameters.AddWithValue("@Price0306", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0306", _price0306);

                if (_price0307 == null)
                    cm.Parameters.AddWithValue("@Price0307", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0307", _price0307);

                if (_price0308 == null)
                    cm.Parameters.AddWithValue("@Price0308", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0308", _price0308);

                if (_price0309 == null)
                    cm.Parameters.AddWithValue("@Price0309", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0309", _price0309);

                if (_price0310 == null)
                    cm.Parameters.AddWithValue("@Price0310", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0310", _price0310);

                if (_price0311 == null)
                    cm.Parameters.AddWithValue("@Price0311", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0311", _price0311);

                if (_price0312 == null)
                    cm.Parameters.AddWithValue("@Price0312", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0312", _price0312);

                if (_price0313 == null)
                    cm.Parameters.AddWithValue("@Price0313", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0313", _price0313);

                if (_price0314 == null)
                    cm.Parameters.AddWithValue("@Price0314", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0314", _price0314);

                if (_price0315 == null)
                    cm.Parameters.AddWithValue("@Price0315", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0315", _price0315);

                if (_price0401 == null)
                    cm.Parameters.AddWithValue("@Price0401", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0401", _price0401);

                if (_price0402 == null)
                    cm.Parameters.AddWithValue("@Price0402", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0402", _price0402);

                if (_price0403 == null)
                    cm.Parameters.AddWithValue("@Price0403", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0403", _price0403);

                if (_price0404 == null)
                    cm.Parameters.AddWithValue("@Price0404", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0404", _price0404);

                if (_price0405 == null)
                    cm.Parameters.AddWithValue("@Price0405", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0405", _price0405);

                if (_price0406 == null)
                    cm.Parameters.AddWithValue("@Price0406", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0406", _price0406);

                if (_price0407 == null)
                    cm.Parameters.AddWithValue("@Price0407", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0407", _price0407);

                if (_price0408 == null)
                    cm.Parameters.AddWithValue("@Price0408", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0408", _price0408);

                if (_price0409 == null)
                    cm.Parameters.AddWithValue("@Price0409", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0409", _price0409);

                if (_price0410 == null)
                    cm.Parameters.AddWithValue("@Price0410", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0410", _price0410);

                if (_price0411 == null)
                    cm.Parameters.AddWithValue("@Price0411", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0411", _price0411);

                if (_price0412 == null)
                    cm.Parameters.AddWithValue("@Price0412", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0412", _price0412);

                if (_price0413 == null)
                    cm.Parameters.AddWithValue("@Price0413", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0413", _price0413);

                if (_price0414 == null)
                    cm.Parameters.AddWithValue("@Price0414", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0414", _price0414);

                if (_price0415 == null)
                    cm.Parameters.AddWithValue("@Price0415", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0415", _price0415);

                if (_price0501 == null)
                    cm.Parameters.AddWithValue("@Price0501", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0501", _price0501);

                if (_price0502 == null)
                    cm.Parameters.AddWithValue("@Price0502", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0502", _price0502);

                if (_price0503 == null)
                    cm.Parameters.AddWithValue("@Price0503", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0503", _price0503);

                if (_price0504 == null)
                    cm.Parameters.AddWithValue("@Price0504", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0504", _price0504);

                if (_price0505 == null)
                    cm.Parameters.AddWithValue("@Price0505", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0505", _price0505);

                if (_price0506 == null)
                    cm.Parameters.AddWithValue("@Price0506", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0506", _price0506);

                if (_price0507 == null)
                    cm.Parameters.AddWithValue("@Price0507", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0507", _price0507);

                if (_price0508 == null)
                    cm.Parameters.AddWithValue("@Price0508", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0508", _price0508);

                if (_price0509 == null)
                    cm.Parameters.AddWithValue("@Price0509", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0509", _price0509);

                if (_price0510 == null)
                    cm.Parameters.AddWithValue("@Price0510", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0510", _price0510);

                if (_price0511 == null)
                    cm.Parameters.AddWithValue("@Price0511", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0511", _price0511);

                if (_price0512 == null)
                    cm.Parameters.AddWithValue("@Price0512", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0512", _price0512);

                if (_price0513 == null)
                    cm.Parameters.AddWithValue("@Price0513", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0513", _price0513);

                if (_price0514 == null)
                    cm.Parameters.AddWithValue("@Price0514", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0514", _price0514);

                if (_price0515 == null)
                    cm.Parameters.AddWithValue("@Price0515", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0515", _price0515);

                if (_price0601 == null)
                    cm.Parameters.AddWithValue("@Price0601", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0601", _price0601);

                if (_price0602 == null)
                    cm.Parameters.AddWithValue("@Price0602", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0602", _price0602);

                if (_price0603 == null)
                    cm.Parameters.AddWithValue("@Price0603", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0603", _price0603);

                if (_price0604 == null)
                    cm.Parameters.AddWithValue("@Price0604", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0604", _price0604);

                if (_price0605 == null)
                    cm.Parameters.AddWithValue("@Price0605", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0605", _price0605);

                if (_price0606 == null)
                    cm.Parameters.AddWithValue("@Price0606", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0606", _price0606);

                if (_price0607 == null)
                    cm.Parameters.AddWithValue("@Price0607", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0607", _price0607);

                if (_price0608 == null)
                    cm.Parameters.AddWithValue("@Price0608", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0608", _price0608);

                if (_price0609 == null)
                    cm.Parameters.AddWithValue("@Price0609", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0609", _price0609);

                if (_price0610 == null)
                    cm.Parameters.AddWithValue("@Price0610", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0610", _price0610);

                if (_price0611 == null)
                    cm.Parameters.AddWithValue("@Price0611", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0611", _price0611);

                if (_price0612 == null)
                    cm.Parameters.AddWithValue("@Price0612", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0612", _price0612);

                if (_price0613 == null)
                    cm.Parameters.AddWithValue("@Price0613", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0613", _price0613);

                if (_price0614 == null)
                    cm.Parameters.AddWithValue("@Price0614", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0614", _price0614);

                if (_price0615 == null)
                    cm.Parameters.AddWithValue("@Price0615", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0615", _price0615);

                if (_price0701 == null)
                    cm.Parameters.AddWithValue("@Price0701", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0701", _price0701);

                if (_price0702 == null)
                    cm.Parameters.AddWithValue("@Price0702", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0702", _price0702);

                if (_price0703 == null)
                    cm.Parameters.AddWithValue("@Price0703", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0703", _price0703);

                if (_price0704 == null)
                    cm.Parameters.AddWithValue("@Price0704", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0704", _price0704);

                if (_price0705 == null)
                    cm.Parameters.AddWithValue("@Price0705", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0705", _price0705);

                if (_price0706 == null)
                    cm.Parameters.AddWithValue("@Price0706", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0706", _price0706);

                if (_price0707 == null)
                    cm.Parameters.AddWithValue("@Price0707", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0707", _price0707);

                if (_price0708 == null)
                    cm.Parameters.AddWithValue("@Price0708", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0708", _price0708);

                if (_price0709 == null)
                    cm.Parameters.AddWithValue("@Price0709", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0709", _price0709);

                if (_price0710 == null)
                    cm.Parameters.AddWithValue("@Price0710", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0710", _price0710);

                if (_price0711 == null)
                    cm.Parameters.AddWithValue("@Price0711", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0711", _price0711);

                if (_price0712 == null)
                    cm.Parameters.AddWithValue("@Price0712", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0712", _price0712);

                if (_price0713 == null)
                    cm.Parameters.AddWithValue("@Price0713", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0713", _price0713);

                if (_price0714 == null)
                    cm.Parameters.AddWithValue("@Price0714", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0714", _price0714);

                if (_price0715 == null)
                    cm.Parameters.AddWithValue("@Price0715", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0715", _price0715);

                if (_price0801 == null)
                    cm.Parameters.AddWithValue("@Price0801", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0801", _price0801);

                if (_price0802 == null)
                    cm.Parameters.AddWithValue("@Price0802", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0802", _price0802);

                if (_price0803 == null)
                    cm.Parameters.AddWithValue("@Price0803", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0803", _price0803);

                if (_price0804 == null)
                    cm.Parameters.AddWithValue("@Price0804", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0804", _price0804);

                if (_price0805 == null)
                    cm.Parameters.AddWithValue("@Price0805", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0805", _price0805);

                if (_price0806 == null)
                    cm.Parameters.AddWithValue("@Price0806", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0806", _price0806);

                if (_price0807 == null)
                    cm.Parameters.AddWithValue("@Price0807", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0807", _price0807);

                if (_price0808 == null)
                    cm.Parameters.AddWithValue("@Price0808", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0808", _price0808);

                if (_price0809 == null)
                    cm.Parameters.AddWithValue("@Price0809", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0809", _price0809);

                if (_price0810 == null)
                    cm.Parameters.AddWithValue("@Price0810", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0810", _price0810);

                if (_price0811 == null)
                    cm.Parameters.AddWithValue("@Price0811", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0811", _price0811);

                if (_price0812 == null)
                    cm.Parameters.AddWithValue("@Price0812", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0812", _price0812);

                if (_price0813 == null)
                    cm.Parameters.AddWithValue("@Price0813", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0813", _price0813);

                if (_price0814 == null)
                    cm.Parameters.AddWithValue("@Price0814", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0814", _price0814);

                if (_price0815 == null)
                    cm.Parameters.AddWithValue("@Price0815", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0815", _price0815);

                if (_price0901 == null)
                    cm.Parameters.AddWithValue("@Price0901", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0901", _price0901);

                if (_price0902 == null)
                    cm.Parameters.AddWithValue("@Price0902", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0902", _price0902);

                if (_price0903 == null)
                    cm.Parameters.AddWithValue("@Price0903", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0903", _price0903);

                if (_price0904 == null)
                    cm.Parameters.AddWithValue("@Price0904", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0904", _price0904);

                if (_price0905 == null)
                    cm.Parameters.AddWithValue("@Price0905", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0905", _price0905);

                if (_price0906 == null)
                    cm.Parameters.AddWithValue("@Price0906", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0906", _price0906);

                if (_price0907 == null)
                    cm.Parameters.AddWithValue("@Price0907", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0907", _price0907);

                if (_price0908 == null)
                    cm.Parameters.AddWithValue("@Price0908", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0908", _price0908);

                if (_price0909 == null)
                    cm.Parameters.AddWithValue("@Price0909", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0909", _price0909);

                if (_price0910 == null)
                    cm.Parameters.AddWithValue("@Price0910", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0910", _price0910);

                if (_price0911 == null)
                    cm.Parameters.AddWithValue("@Price0911", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0911", _price0911);

                if (_price0912 == null)
                    cm.Parameters.AddWithValue("@Price0912", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0912", _price0912);

                if (_price0913 == null)
                    cm.Parameters.AddWithValue("@Price0913", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0913", _price0913);

                if (_price0914 == null)
                    cm.Parameters.AddWithValue("@Price0914", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0914", _price0914);

                if (_price0915 == null)
                    cm.Parameters.AddWithValue("@Price0915", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price0915", _price0915);

                if (_price1001 == null)
                    cm.Parameters.AddWithValue("@Price1001", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1001", _price1001);

                if (_price1002 == null)
                    cm.Parameters.AddWithValue("@Price1002", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1002", _price1002);

                if (_price1003 == null)
                    cm.Parameters.AddWithValue("@Price1003", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1003", _price1003);

                if (_price1004 == null)
                    cm.Parameters.AddWithValue("@Price1004", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1004", _price1004);

                if (_price1005 == null)
                    cm.Parameters.AddWithValue("@Price1005", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1005", _price1005);

                if (_price1006 == null)
                    cm.Parameters.AddWithValue("@Price1006", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1006", _price1006);

                if (_price1007 == null)
                    cm.Parameters.AddWithValue("@Price1007", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1007", _price1007);

                if (_price1008 == null)
                    cm.Parameters.AddWithValue("@Price1008", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1008", _price1008);

                if (_price1009 == null)
                    cm.Parameters.AddWithValue("@Price1009", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1009", _price1009);

                if (_price1010 == null)
                    cm.Parameters.AddWithValue("@Price1010", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1010", _price1010);

                if (_price1011 == null)
                    cm.Parameters.AddWithValue("@Price1011", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1011", _price1011);

                if (_price1012 == null)
                    cm.Parameters.AddWithValue("@Price1012", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1012", _price1012);

                if (_price1013 == null)
                    cm.Parameters.AddWithValue("@Price1013", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1013", _price1013);

                if (_price1014 == null)
                    cm.Parameters.AddWithValue("@Price1014", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1014", _price1014);

                if (_price1015 == null)
                    cm.Parameters.AddWithValue("@Price1015", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1015", _price1015);

                if (_price1101 == null)
                    cm.Parameters.AddWithValue("@Price1101", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1101", _price1101);

                if (_price1102 == null)
                    cm.Parameters.AddWithValue("@Price1102", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1102", _price1102);

                if (_price1103 == null)
                    cm.Parameters.AddWithValue("@Price1103", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1103", _price1103);

                if (_price1104 == null)
                    cm.Parameters.AddWithValue("@Price1104", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1104", _price1104);

                if (_price1105 == null)
                    cm.Parameters.AddWithValue("@Price1105", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1105", _price1105);

                if (_price1106 == null)
                    cm.Parameters.AddWithValue("@Price1106", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1106", _price1106);

                if (_price1107 == null)
                    cm.Parameters.AddWithValue("@Price1107", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1107", _price1107);

                if (_price1108 == null)
                    cm.Parameters.AddWithValue("@Price1108", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1108", _price1108);

                if (_price1109 == null)
                    cm.Parameters.AddWithValue("@Price1109", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1109", _price1109);

                if (_price1110 == null)
                    cm.Parameters.AddWithValue("@Price1110", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1110", _price1110);

                if (_price1111 == null)
                    cm.Parameters.AddWithValue("@Price1111", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1111", _price1111);

                if (_price1112 == null)
                    cm.Parameters.AddWithValue("@Price1112", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1112", _price1112);

                if (_price1113 == null)
                    cm.Parameters.AddWithValue("@Price1113", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1113", _price1113);

                if (_price1114 == null)
                    cm.Parameters.AddWithValue("@Price1114", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1114", _price1114);

                if (_price1115 == null)
                    cm.Parameters.AddWithValue("@Price1115", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1115", _price1115);

                if (_price1201 == null)
                    cm.Parameters.AddWithValue("@Price1201", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1201", _price1201);

                if (_price1202 == null)
                    cm.Parameters.AddWithValue("@Price1202", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1202", _price1202);

                if (_price1203 == null)
                    cm.Parameters.AddWithValue("@Price1203", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1203", _price1203);

                if (_price1204 == null)
                    cm.Parameters.AddWithValue("@Price1204", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1204", _price1204);

                if (_price1205 == null)
                    cm.Parameters.AddWithValue("@Price1205", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1205", _price1205);

                if (_price1206 == null)
                    cm.Parameters.AddWithValue("@Price1206", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1206", _price1206);

                if (_price1207 == null)
                    cm.Parameters.AddWithValue("@Price1207", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1207", _price1207);

                if (_price1208 == null)
                    cm.Parameters.AddWithValue("@Price1208", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1208", _price1208);

                if (_price1209 == null)
                    cm.Parameters.AddWithValue("@Price1209", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1209", _price1209);

                if (_price1210 == null)
                    cm.Parameters.AddWithValue("@Price1210", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1210", _price1210);

                if (_price1211 == null)
                    cm.Parameters.AddWithValue("@Price1211", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1211", _price1211);

                if (_price1212 == null)
                    cm.Parameters.AddWithValue("@Price1212", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1212", _price1212);

                if (_price1213 == null)
                    cm.Parameters.AddWithValue("@Price1213", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1213", _price1213);

                if (_price1214 == null)
                    cm.Parameters.AddWithValue("@Price1214", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1214", _price1214);

                if (_price1215 == null)
                    cm.Parameters.AddWithValue("@Price1215", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1215", _price1215);

                if (_price1301 == null)
                    cm.Parameters.AddWithValue("@Price1301", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1301", _price1301);

                if (_price1302 == null)
                    cm.Parameters.AddWithValue("@Price1302", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1302", _price1302);

                if (_price1303 == null)
                    cm.Parameters.AddWithValue("@Price1303", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1303", _price1303);

                if (_price1304 == null)
                    cm.Parameters.AddWithValue("@Price1304", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1304", _price1304);

                if (_price1305 == null)
                    cm.Parameters.AddWithValue("@Price1305", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1305", _price1305);

                if (_price1306 == null)
                    cm.Parameters.AddWithValue("@Price1306", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1306", _price1306);

                if (_price1307 == null)
                    cm.Parameters.AddWithValue("@Price1307", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1307", _price1307);

                if (_price1308 == null)
                    cm.Parameters.AddWithValue("@Price1308", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1308", _price1308);

                if (_price1309 == null)
                    cm.Parameters.AddWithValue("@Price1309", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1309", _price1309);

                if (_price1310 == null)
                    cm.Parameters.AddWithValue("@Price1310", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1310", _price1310);

                if (_price1311 == null)
                    cm.Parameters.AddWithValue("@Price1311", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1311", _price1311);

                if (_price1312 == null)
                    cm.Parameters.AddWithValue("@Price1312", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1312", _price1312);

                if (_price1313 == null)
                    cm.Parameters.AddWithValue("@Price1313", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1313", _price1313);

                if (_price1314 == null)
                    cm.Parameters.AddWithValue("@Price1314", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1314", _price1314);

                if (_price1315 == null)
                    cm.Parameters.AddWithValue("@Price1315", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1315", _price1315);

                if (_price1401 == null)
                    cm.Parameters.AddWithValue("@Price1401", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1401", _price1401);

                if (_price1402 == null)
                    cm.Parameters.AddWithValue("@Price1402", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1402", _price1402);

                if (_price1403 == null)
                    cm.Parameters.AddWithValue("@Price1403", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1403", _price1403);

                if (_price1404 == null)
                    cm.Parameters.AddWithValue("@Price1404", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1404", _price1404);

                if (_price1405 == null)
                    cm.Parameters.AddWithValue("@Price1405", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1405", _price1405);

                if (_price1406 == null)
                    cm.Parameters.AddWithValue("@Price1406", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1406", _price1406);

                if (_price1407 == null)
                    cm.Parameters.AddWithValue("@Price1407", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1407", _price1407);

                if (_price1408 == null)
                    cm.Parameters.AddWithValue("@Price1408", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1408", _price1408);

                if (_price1409 == null)
                    cm.Parameters.AddWithValue("@Price1409", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1409", _price1409);

                if (_price1410 == null)
                    cm.Parameters.AddWithValue("@Price1410", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1410", _price1410);

                if (_price1411 == null)
                    cm.Parameters.AddWithValue("@Price1411", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1411", _price1411);

                if (_price1412 == null)
                    cm.Parameters.AddWithValue("@Price1412", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1412", _price1412);

                if (_price1413 == null)
                    cm.Parameters.AddWithValue("@Price1413", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1413", _price1413);

                if (_price1414 == null)
                    cm.Parameters.AddWithValue("@Price1414", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1414", _price1414);

                if (_price1415 == null)
                    cm.Parameters.AddWithValue("@Price1415", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1415", _price1415);

                if (_price1501 == null)
                    cm.Parameters.AddWithValue("@Price1501", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1501", _price1501);

                if (_price1502 == null)
                    cm.Parameters.AddWithValue("@Price1502", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1502", _price1502);

                if (_price1503 == null)
                    cm.Parameters.AddWithValue("@Price1503", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1503", _price1503);

                if (_price1504 == null)
                    cm.Parameters.AddWithValue("@Price1504", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1504", _price1504);

                if (_price1505 == null)
                    cm.Parameters.AddWithValue("@Price1505", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1505", _price1505);

                if (_price1506 == null)
                    cm.Parameters.AddWithValue("@Price1506", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1506", _price1506);

                if (_price1507 == null)
                    cm.Parameters.AddWithValue("@Price1507", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1507", _price1507);

                if (_price1508 == null)
                    cm.Parameters.AddWithValue("@Price1508", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1508", _price1508);

                if (_price1509 == null)
                    cm.Parameters.AddWithValue("@Price1509", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1509", _price1509);

                if (_price1510 == null)
                    cm.Parameters.AddWithValue("@Price1510", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1510", _price1510);

                if (_price1511 == null)
                    cm.Parameters.AddWithValue("@Price1511", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1511", _price1511);

                if (_price1512 == null)
                    cm.Parameters.AddWithValue("@Price1512", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1512", _price1512);

                if (_price1513 == null)
                    cm.Parameters.AddWithValue("@Price1513", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1513", _price1513);

                if (_price1514 == null)
                    cm.Parameters.AddWithValue("@Price1514", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1514", _price1514);

                if (_price1515 == null)
                    cm.Parameters.AddWithValue("@Price1515", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Price1515", _price1515);

                if (_ratio1 == null)
                    cm.Parameters.AddWithValue("@Ratio1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio1", _ratio1);

                if (_ratio2 == null)
                    cm.Parameters.AddWithValue("@Ratio2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio2", _ratio2);

                if (_ratio3 == null)
                    cm.Parameters.AddWithValue("@Ratio3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio3", _ratio3);

                if (_ratio4 == null)
                    cm.Parameters.AddWithValue("@Ratio4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio4", _ratio4);

                if (_ratio5 == null)
                    cm.Parameters.AddWithValue("@Ratio5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio5", _ratio5);

                if (_ratio6 == null)
                    cm.Parameters.AddWithValue("@Ratio6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio6", _ratio6);

                if (_ratio7 == null)
                    cm.Parameters.AddWithValue("@Ratio7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio7", _ratio7);

                if (_ratio8 == null)
                    cm.Parameters.AddWithValue("@Ratio8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio8", _ratio8);

                if (_ratio9 == null)
                    cm.Parameters.AddWithValue("@Ratio9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio9", _ratio9);

                if (_ratio10 == null)
                    cm.Parameters.AddWithValue("@Ratio10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio10", _ratio10);

                if (_ratio11 == null)
                    cm.Parameters.AddWithValue("@Ratio11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio11", _ratio11);

                if (_ratio12 == null)
                    cm.Parameters.AddWithValue("@Ratio12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio12", _ratio12);

                if (_ratio13 == null)
                    cm.Parameters.AddWithValue("@Ratio13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio13", _ratio13);

                if (_ratio14 == null)
                    cm.Parameters.AddWithValue("@Ratio14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio14", _ratio14);

                if (_ratio15 == null)
                    cm.Parameters.AddWithValue("@Ratio15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Ratio15", _ratio15);

                if (_qtyDisQty1 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty1", _qtyDisQty1);

                if (_qtyDisQty2 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty2", _qtyDisQty2);

                if (_qtyDisQty3 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty3", _qtyDisQty3);

                if (_qtyDisQty4 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty4", _qtyDisQty4);

                if (_qtyDisQty5 == null)
                    cm.Parameters.AddWithValue("@QtyDisQty5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisQty5", _qtyDisQty5);

                if (_qtyDisRatio1 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio1", _qtyDisRatio1);

                if (_qtyDisRatio2 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio2", _qtyDisRatio2);

                if (_qtyDisRatio3 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio3", _qtyDisRatio3);

                if (_qtyDisRatio4 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio4", _qtyDisRatio4);

                if (_qtyDisRatio5 == null)
                    cm.Parameters.AddWithValue("@QtyDisRatio5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@QtyDisRatio5", _qtyDisRatio5);

                if (_standardCost1 == null)
                    cm.Parameters.AddWithValue("@StandardCost1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost1", _standardCost1);

                if (_standardCost2 == null)
                    cm.Parameters.AddWithValue("@StandardCost2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost2", _standardCost2);

                if (_standardCost3 == null)
                    cm.Parameters.AddWithValue("@StandardCost3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost3", _standardCost3);

                if (_standardCost4 == null)
                    cm.Parameters.AddWithValue("@StandardCost4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost4", _standardCost4);

                if (_standardCost5 == null)
                    cm.Parameters.AddWithValue("@StandardCost5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost5", _standardCost5);

                if (_standardCost6 == null)
                    cm.Parameters.AddWithValue("@StandardCost6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost6", _standardCost6);

                if (_standardCost7 == null)
                    cm.Parameters.AddWithValue("@StandardCost7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost7", _standardCost7);

                if (_standardCost8 == null)
                    cm.Parameters.AddWithValue("@StandardCost8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost8", _standardCost8);

                if (_standardCost9 == null)
                    cm.Parameters.AddWithValue("@StandardCost9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost9", _standardCost9);

                if (_standardCost10 == null)
                    cm.Parameters.AddWithValue("@StandardCost10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost10", _standardCost10);

                if (_standardCost11 == null)
                    cm.Parameters.AddWithValue("@StandardCost11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost11", _standardCost11);

                if (_standardCost12 == null)
                    cm.Parameters.AddWithValue("@StandardCost12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost12", _standardCost12);

                if (_standardCost13 == null)
                    cm.Parameters.AddWithValue("@StandardCost13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost13", _standardCost13);

                if (_standardCost14 == null)
                    cm.Parameters.AddWithValue("@StandardCost14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost14", _standardCost14);

                if (_standardCost15 == null)
                    cm.Parameters.AddWithValue("@StandardCost15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@StandardCost15", _standardCost15);

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

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "MSTItmDetPrice_Delete";

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

        internal bool Validation(SqlConnection cn, Criteria criteria, bool? isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetPrice_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }
        #endregion //Data Access - Validation
    }
}


