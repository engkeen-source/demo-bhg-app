using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace BOLib
{
    class DocItmTmp
    {
        #region +++  Local variables declaration for the class +++

		private int? _UID;
		private int? _UserKey;
		private bool _LocalData;
		private int? _PgmSign;
		private int? _INHisSign;
		private int? _DocCodeKey;
		private int? _DocKey;
		private int? _DocItmKey;
		private DateTime? _DocDate;
		private int? _DocPeriod;
		private short? _DocSign;
		private string _DocType;
		private float? _DocCurrRate;
		private decimal? _DocAddCostFactor;
		private int? _ItmKey;
		private float? _ItmSN;
		private int? _ItmDeptKey;
		private int? _ItmTranGrpKey;
		private string _ItmType;
		private string _ItmCostMethod;
		private int? _ItmAccINKey;
		private int? _ItmAccPHKey;
		private string _ItmDes;
		private int? _ItmLocKey;
		private decimal? _ItmQty;
		private decimal? _ItmConRate;
		private decimal? _ItmPrice;
		private decimal? _ItmAmtH;
		private decimal? _ItmAddCostF;
		private decimal? _ItmAddCostH;
		private decimal? _ItmAddAmtF;
		private decimal? _ItmAddAmtH;
		private decimal? _ItmQtyLink;
		private decimal? _ItmQtyAdj;
		private decimal? _ItmQtyShw;
		private DateTime? _ItmPrmDate;
		private int? _ItmIGrpDItm;
		private int? _ARSODK;
		private int? _ARSODItm;
		private int? _SOPeriod;
		private DateTime? _SOItmETA;
		private decimal? _SOItmConRate;
		private int? _APPODK;
		private int? _APPODItm;
		private int? _POPeriod;
		private DateTime? _POItmETA;
		private decimal? _POItmConRate;
		private int? _APPDDK;
		private int? _APPDDItm;
		private int? _PDPeriod;
		private int? _PDItmDept;
		private int? _PDItmTranGrp;
		private decimal? _PDItmQty;
		private decimal? _PDItmCostH;
		private decimal? _PDItmAmtH;
		private int? _INCPSDK;
		private int? _INCPSDItm;
		private decimal? _INCPSItmConRate;
		private int? _INCSIDK;
		private int? _INCSIDItm;
		private int? _CPODK;
		private int? _CPODItm;
		private int? _COPeriod;
		private DateTime? _COItmETA;
		private int? _LineType;
		private int? _ItmFGKey;
		private decimal? _FGOverHeadCost;
		private decimal? _FGCostRatio;
		private decimal? _wItmQty;
		private decimal? _wItmLatestCost;
		private decimal? _wItmSOQty;
		private decimal? _wItmPOQty;
		private decimal? _wItmQtyHis;
		private decimal? _wItmQtyCSP;
		private decimal? _ItmMFNQtyReq;
		private bool _wMatchLocalData;
		private bool _isDirty;

		#endregion

		#region +++  Constructor and destructor codes  +++

		/// <summary>
		/// Default constructor that will initialize all properties with default values.
		/// </summary>

		public DocItmTmp()
		{
			this._UID = null;
			this._UserKey = null;
			this._LocalData = false;
			this._PgmSign = null;
			this._INHisSign = null;
			this._DocCodeKey = null;
			this._DocKey = null;
			this._DocItmKey = null;
			this._DocDate = null;
			this._DocPeriod = null;
			this._DocSign = null;
			this._DocType = string.Empty;
			this._DocCurrRate = null;
			this._DocAddCostFactor = null;
			this._ItmKey = null;
			this._ItmSN = null;
			this._ItmDeptKey = null;
			this._ItmTranGrpKey = null;
			this._ItmType = string.Empty;
			this._ItmCostMethod = string.Empty;
			this._ItmAccINKey = null;
			this._ItmAccPHKey = null;
			this._ItmDes = string.Empty;
			this._ItmLocKey = null;
			this._ItmQty = null;
			this._ItmConRate = null;
			this._ItmPrice = null;
			this._ItmAmtH = null;
			this._ItmAddCostF = null;
			this._ItmAddCostH = null;
			this._ItmAddAmtF = null;
			this._ItmAddAmtH = null;
			this._ItmQtyLink = null;
			this._ItmQtyAdj = null;
			this._ItmQtyShw = null;
			this._ItmPrmDate = null;
			this._ItmIGrpDItm = null;
			this._ARSODK = null;
			this._ARSODItm = null;
			this._SOPeriod = null;
			this._SOItmETA = null;
			this._SOItmConRate = null;
			this._APPODK = null;
			this._APPODItm = null;
			this._POPeriod = null;
			this._POItmETA = null;
			this._POItmConRate = null;
			this._APPDDK = null;
			this._APPDDItm = null;
			this._PDPeriod = null;
			this._PDItmDept = null;
			this._PDItmTranGrp = null;
			this._PDItmQty = null;
			this._PDItmCostH = null;
			this._PDItmAmtH = null;
			this._INCPSDK = null;
			this._INCPSDItm = null;
			this._INCPSItmConRate = null;
			this._INCSIDK = null;
			this._INCSIDItm = null;
			this._CPODK = null;
			this._CPODItm = null;
			this._COPeriod = null;
			this._COItmETA = null;
			this._LineType = null;
			this._ItmFGKey = null;
			this._FGOverHeadCost = null;
			this._FGCostRatio = null;
			this._wItmQty = null;
			this._wItmLatestCost = null;
			this._wItmSOQty = null;
			this._wItmPOQty = null;
			this._wItmQtyHis = null;
			this._wItmQtyCSP = null;
			this._ItmMFNQtyReq = null;
			this._wMatchLocalData = false;
			this._isDirty = false;
		}

		/// <summary>
		/// Extended constructor that will initialize all properties with the parameter values.
		/// </summary>
		/// <param name="_UID"> System.Int32 object containing UID.</param>
		/// <param name="_UserKey"> System.Int32 object containing UserKey.</param>
		/// <param name="_LocalData"> System.Boolean object containing LocalData.</param>
		/// <param name="_PgmSign"> System.Int32 object containing PgmSign.</param>
		/// <param name="_INHisSign"> System.Int32 object containing INHisSign.</param>
		/// <param name="_DocCodeKey"> System.Int32 object containing DocCodeKey.</param>
		/// <param name="_DocKey"> System.Int32 object containing DocKey.</param>
		/// <param name="_DocItmKey"> System.Int32 object containing DocItmKey.</param>
		/// <param name="_DocDate"> System.DateTime object containing DocDate.</param>
		/// <param name="_DocPeriod"> System.Int32 object containing DocPeriod.</param>
		/// <param name="_DocSign"> System.Int16 object containing DocSign.</param>
		/// <param name="_DocType"> System.String object containing DocType.</param>
		/// <param name="_DocCurrRate"> System.Single object containing DocCurrRate.</param>
		/// <param name="_DocAddCostFactor"> System.Decimal object containing DocAddCostFactor.</param>
		/// <param name="_ItmKey"> System.Int32 object containing ItmKey.</param>
		/// <param name="_ItmSN"> System.Single object containing ItmSN.</param>
		/// <param name="_ItmDeptKey"> System.Int32 object containing ItmDeptKey.</param>
		/// <param name="_ItmTranGrpKey"> System.Int32 object containing ItmTranGrpKey.</param>
		/// <param name="_ItmType"> System.String object containing ItmType.</param>
		/// <param name="_ItmCostMethod"> System.String object containing ItmCostMethod.</param>
		/// <param name="_ItmAccINKey"> System.Int32 object containing ItmAccINKey.</param>
		/// <param name="_ItmAccPHKey"> System.Int32 object containing ItmAccPHKey.</param>
		/// <param name="_ItmDes"> System.String object containing ItmDes.</param>
		/// <param name="_ItmLocKey"> System.Int32 object containing ItmLocKey.</param>
		/// <param name="_ItmQty"> System.Decimal object containing ItmQty.</param>
		/// <param name="_ItmConRate"> System.Decimal object containing ItmConRate.</param>
		/// <param name="_ItmPrice"> System.Decimal object containing ItmPrice.</param>
		/// <param name="_ItmAmtH"> System.Decimal object containing ItmAmtH.</param>
		/// <param name="_ItmAddCostF"> System.Decimal object containing ItmAddCostF.</param>
		/// <param name="_ItmAddCostH"> System.Decimal object containing ItmAddCostH.</param>
		/// <param name="_ItmAddAmtF"> System.Decimal object containing ItmAddAmtF.</param>
		/// <param name="_ItmAddAmtH"> System.Decimal object containing ItmAddAmtH.</param>
		/// <param name="_ItmQtyLink"> System.Decimal object containing ItmQtyLink.</param>
		/// <param name="_ItmQtyAdj"> System.Decimal object containing ItmQtyAdj.</param>
		/// <param name="_ItmQtyShw"> System.Decimal object containing ItmQtyShw.</param>
		/// <param name="_ItmPrmDate"> System.DateTime object containing ItmPrmDate.</param>
		/// <param name="_ItmIGrpDItm"> System.Int32 object containing ItmIGrpDItm.</param>
		/// <param name="_ARSODK"> System.Int32 object containing ARSODK.</param>
		/// <param name="_ARSODItm"> System.Int32 object containing ARSODItm.</param>
		/// <param name="_SOPeriod"> System.Int32 object containing SOPeriod.</param>
		/// <param name="_SOItmETA"> System.DateTime object containing SOItmETA.</param>
		/// <param name="_SOItmConRate"> System.Decimal object containing SOItmConRate.</param>
		/// <param name="_APPODK"> System.Int32 object containing APPODK.</param>
		/// <param name="_APPODItm"> System.Int32 object containing APPODItm.</param>
		/// <param name="_POPeriod"> System.Int32 object containing POPeriod.</param>
		/// <param name="_POItmETA"> System.DateTime object containing POItmETA.</param>
		/// <param name="_POItmConRate"> System.Decimal object containing POItmConRate.</param>
		/// <param name="_APPDDK"> System.Int32 object containing APPDDK.</param>
		/// <param name="_APPDDItm"> System.Int32 object containing APPDDItm.</param>
		/// <param name="_PDPeriod"> System.Int32 object containing PDPeriod.</param>
		/// <param name="_PDItmDept"> System.Int32 object containing PDItmDept.</param>
		/// <param name="_PDItmTranGrp"> System.Int32 object containing PDItmTranGrp.</param>
		/// <param name="_PDItmQty"> System.Decimal object containing PDItmQty.</param>
		/// <param name="_PDItmCostH"> System.Decimal object containing PDItmCostH.</param>
		/// <param name="_PDItmAmtH"> System.Decimal object containing PDItmAmtH.</param>
		/// <param name="_INCPSDK"> System.Int32 object containing INCPSDK.</param>
		/// <param name="_INCPSDItm"> System.Int32 object containing INCPSDItm.</param>
		/// <param name="_INCPSItmConRate"> System.Decimal object containing INCPSItmConRate.</param>
		/// <param name="_INCSIDK"> System.Int32 object containing INCSIDK.</param>
		/// <param name="_INCSIDItm"> System.Int32 object containing INCSIDItm.</param>
		/// <param name="_CPODK"> System.Int32 object containing CPODK.</param>
		/// <param name="_CPODItm"> System.Int32 object containing CPODItm.</param>
		/// <param name="_COPeriod"> System.Int32 object containing COPeriod.</param>
		/// <param name="_COItmETA"> System.DateTime object containing COItmETA.</param>
		/// <param name="_LineType"> System.Int32 object containing LineType.</param>
		/// <param name="_ItmFGKey"> System.Int32 object containing ItmFGKey.</param>
		/// <param name="_FGOverHeadCost"> System.Decimal object containing FGOverHeadCost.</param>
		/// <param name="_FGCostRatio"> System.Decimal object containing FGCostRatio.</param>
		/// <param name="_wItmQty"> System.Decimal object containing wItmQty.</param>
		/// <param name="_wItmLatestCost"> System.Decimal object containing wItmLatestCost.</param>
		/// <param name="_wItmSOQty"> System.Decimal object containing wItmSOQty.</param>
		/// <param name="_wItmPOQty"> System.Decimal object containing wItmPOQty.</param>
		/// <param name="_wItmQtyHis"> System.Decimal object containing wItmQtyHis.</param>
		/// <param name="_wItmQtyCSP"> System.Decimal object containing wItmQtyCSP.</param>
		/// <param name="_ItmMFNQtyReq"> System.Decimal object containing ItmMFNQtyReq.</param>
		/// <param name="_wMatchLocalData"> System.Boolean object containing wMatchLocalData.</param>

        public DocItmTmp(int? _UID, int? _UserKey, bool _LocalData, int? _PgmSign, int? _INHisSign, int? _DocCodeKey, int? _DocKey, int? _DocItmKey, DateTime? _DocDate, int? _DocPeriod, short? _DocSign, string _DocType, float? _DocCurrRate, decimal? _DocAddCostFactor, int? _ItmKey, float? _ItmSN, int? _ItmDeptKey, int? _ItmTranGrpKey, string _ItmType, string _ItmCostMethod, int? _ItmAccINKey, int? _ItmAccPHKey, string _ItmDes, int? _ItmLocKey, decimal? _ItmQty, decimal? _ItmConRate, decimal? _ItmPrice, decimal? _ItmAmtH, decimal? _ItmAddCostF, decimal? _ItmAddCostH, decimal? _ItmAddAmtF, decimal? _ItmAddAmtH, decimal? _ItmQtyLink, decimal? _ItmQtyAdj, decimal? _ItmQtyShw, DateTime? _ItmPrmDate, int? _ItmIGrpDItm, int? _ARSODK, int? _ARSODItm, int? _SOPeriod, DateTime? _SOItmETA, decimal? _SOItmConRate, int? _APPODK, int? _APPODItm, int? _POPeriod, DateTime? _POItmETA, decimal? _POItmConRate, int? _APPDDK, int? _APPDDItm, int? _PDPeriod, int? _PDItmDept, int? _PDItmTranGrp, decimal? _PDItmQty, decimal? _PDItmCostH, decimal? _PDItmAmtH, int? _INCPSDK, int? _INCPSDItm, decimal? _INCPSItmConRate, int? _INCSIDK, int? _INCSIDItm, int? _CPODK, int? _CPODItm, int? _COPeriod, DateTime? _COItmETA, int? _LineType, int? _ItmFGKey, decimal? _FGOverHeadCost, decimal? _FGCostRatio, decimal? _wItmQty, decimal? _wItmLatestCost, decimal? _wItmSOQty, decimal? _wItmPOQty, decimal? _wItmQtyHis, decimal? _wItmQtyCSP, decimal? _ItmMFNQtyReq, bool _wMatchLocalData)
		{
			this._UID = _UID;
			this._UserKey = _UserKey;
			this._LocalData = _LocalData;
			this._PgmSign = _PgmSign;
			this._INHisSign = _INHisSign;
			this._DocCodeKey = _DocCodeKey;
			this._DocKey = _DocKey;
			this._DocItmKey = _DocItmKey;
			this._DocDate = _DocDate;
			this._DocPeriod = _DocPeriod;
			this._DocSign = _DocSign;
			this._DocType = _DocType;
			this._DocCurrRate = _DocCurrRate;
			this._DocAddCostFactor = _DocAddCostFactor;
			this._ItmKey = _ItmKey;
			this._ItmSN = _ItmSN;
			this._ItmDeptKey = _ItmDeptKey;
			this._ItmTranGrpKey = _ItmTranGrpKey;
			this._ItmType = _ItmType;
			this._ItmCostMethod = _ItmCostMethod;
			this._ItmAccINKey = _ItmAccINKey;
			this._ItmAccPHKey = _ItmAccPHKey;
			this._ItmDes = _ItmDes;
			this._ItmLocKey = _ItmLocKey;
			this._ItmQty = _ItmQty;
			this._ItmConRate = _ItmConRate;
			this._ItmPrice = _ItmPrice;
			this._ItmAmtH = _ItmAmtH;
			this._ItmAddCostF = _ItmAddCostF;
			this._ItmAddCostH = _ItmAddCostH;
			this._ItmAddAmtF = _ItmAddAmtF;
			this._ItmAddAmtH = _ItmAddAmtH;
			this._ItmQtyLink = _ItmQtyLink;
			this._ItmQtyAdj = _ItmQtyAdj;
			this._ItmQtyShw = _ItmQtyShw;
			this._ItmPrmDate = _ItmPrmDate;
			this._ItmIGrpDItm = _ItmIGrpDItm;
			this._ARSODK = _ARSODK;
			this._ARSODItm = _ARSODItm;
			this._SOPeriod = _SOPeriod;
			this._SOItmETA = _SOItmETA;
			this._SOItmConRate = _SOItmConRate;
			this._APPODK = _APPODK;
			this._APPODItm = _APPODItm;
			this._POPeriod = _POPeriod;
			this._POItmETA = _POItmETA;
			this._POItmConRate = _POItmConRate;
			this._APPDDK = _APPDDK;
			this._APPDDItm = _APPDDItm;
			this._PDPeriod = _PDPeriod;
			this._PDItmDept = _PDItmDept;
			this._PDItmTranGrp = _PDItmTranGrp;
			this._PDItmQty = _PDItmQty;
			this._PDItmCostH = _PDItmCostH;
			this._PDItmAmtH = _PDItmAmtH;
			this._INCPSDK = _INCPSDK;
			this._INCPSDItm = _INCPSDItm;
			this._INCPSItmConRate = _INCPSItmConRate;
			this._INCSIDK = _INCSIDK;
			this._INCSIDItm = _INCSIDItm;
			this._CPODK = _CPODK;
			this._CPODItm = _CPODItm;
			this._COPeriod = _COPeriod;
			this._COItmETA = _COItmETA;
			this._LineType = _LineType;
			this._ItmFGKey = _ItmFGKey;
			this._FGOverHeadCost = _FGOverHeadCost;
			this._FGCostRatio = _FGCostRatio;
			this._wItmQty = _wItmQty;
			this._wItmLatestCost = _wItmLatestCost;
			this._wItmSOQty = _wItmSOQty;
			this._wItmPOQty = _wItmPOQty;
			this._wItmQtyHis = _wItmQtyHis;
			this._wItmQtyCSP = _wItmQtyCSP;
			this._ItmMFNQtyReq = _ItmMFNQtyReq;
			this._wMatchLocalData = _wMatchLocalData;
		}

		/// <summary>
		/// Disposing objects
		/// </summary>
		public void Dispose()
		{
			if (this._UID != null )
				this._UID = null;
			if (this._UserKey != null )
				this._UserKey = null;
			if (this._PgmSign != null )
				this._PgmSign = null;
			if (this._INHisSign != null )
				this._INHisSign = null;
			if (this._DocCodeKey != null )
				this._DocCodeKey = null;
			if (this._DocKey != null )
				this._DocKey = null;
			if (this._DocItmKey != null )
				this._DocItmKey = null;
			if (this._DocDate != null )
				this._DocDate = null;
			if (this._DocPeriod != null )
				this._DocPeriod = null;
			if (this._DocSign != null )
				this._DocSign = null;
			if (this._DocCurrRate != null )
				this._DocCurrRate = null;
			if (this._DocAddCostFactor != null )
				this._DocAddCostFactor = null;
			if (this._ItmKey != null )
				this._ItmKey = null;
			if (this._ItmSN != null )
				this._ItmSN = null;
			if (this._ItmDeptKey != null )
				this._ItmDeptKey = null;
			if (this._ItmTranGrpKey != null )
				this._ItmTranGrpKey = null;
			if (this._ItmAccINKey != null )
				this._ItmAccINKey = null;
			if (this._ItmAccPHKey != null )
				this._ItmAccPHKey = null;
			if (this._ItmLocKey != null )
				this._ItmLocKey = null;
			if (this._ItmQty != null )
				this._ItmQty = null;
			if (this._ItmConRate != null )
				this._ItmConRate = null;
			if (this._ItmPrice != null )
				this._ItmPrice = null;
			if (this._ItmAmtH != null )
				this._ItmAmtH = null;
			if (this._ItmAddCostF != null )
				this._ItmAddCostF = null;
			if (this._ItmAddCostH != null )
				this._ItmAddCostH = null;
			if (this._ItmAddAmtF != null )
				this._ItmAddAmtF = null;
			if (this._ItmAddAmtH != null )
				this._ItmAddAmtH = null;
			if (this._ItmQtyLink != null )
				this._ItmQtyLink = null;
			if (this._ItmQtyAdj != null )
				this._ItmQtyAdj = null;
			if (this._ItmQtyShw != null )
				this._ItmQtyShw = null;
			if (this._ItmPrmDate != null )
				this._ItmPrmDate = null;
			if (this._ItmIGrpDItm != null )
				this._ItmIGrpDItm = null;
			if (this._ARSODK != null )
				this._ARSODK = null;
			if (this._ARSODItm != null )
				this._ARSODItm = null;
			if (this._SOPeriod != null )
				this._SOPeriod = null;
			if (this._SOItmETA != null )
				this._SOItmETA = null;
			if (this._SOItmConRate != null )
				this._SOItmConRate = null;
			if (this._APPODK != null )
				this._APPODK = null;
			if (this._APPODItm != null )
				this._APPODItm = null;
			if (this._POPeriod != null )
				this._POPeriod = null;
			if (this._POItmETA != null )
				this._POItmETA = null;
			if (this._POItmConRate != null )
				this._POItmConRate = null;
			if (this._APPDDK != null )
				this._APPDDK = null;
			if (this._APPDDItm != null )
				this._APPDDItm = null;
			if (this._PDPeriod != null )
				this._PDPeriod = null;
			if (this._PDItmDept != null )
				this._PDItmDept = null;
			if (this._PDItmTranGrp != null )
				this._PDItmTranGrp = null;
			if (this._PDItmQty != null )
				this._PDItmQty = null;
			if (this._PDItmCostH != null )
				this._PDItmCostH = null;
			if (this._PDItmAmtH != null )
				this._PDItmAmtH = null;
			if (this._INCPSDK != null )
				this._INCPSDK = null;
			if (this._INCPSDItm != null )
				this._INCPSDItm = null;
			if (this._INCPSItmConRate != null )
				this._INCPSItmConRate = null;
			if (this._INCSIDK != null )
				this._INCSIDK = null;
			if (this._INCSIDItm != null )
				this._INCSIDItm = null;
			if (this._CPODK != null )
				this._CPODK = null;
			if (this._CPODItm != null )
				this._CPODItm = null;
			if (this._COPeriod != null )
				this._COPeriod = null;
			if (this._COItmETA != null )
				this._COItmETA = null;
			if (this._LineType != null )
				this._LineType = null;
			if (this._ItmFGKey != null )
				this._ItmFGKey = null;
			if (this._FGOverHeadCost != null )
				this._FGOverHeadCost = null;
			if (this._FGCostRatio != null )
				this._FGCostRatio = null;
			if (this._wItmQty != null )
				this._wItmQty = null;
			if (this._wItmLatestCost != null )
				this._wItmLatestCost = null;
			if (this._wItmSOQty != null )
				this._wItmSOQty = null;
			if (this._wItmPOQty != null )
				this._wItmPOQty = null;
			if (this._wItmQtyHis != null )
				this._wItmQtyHis = null;
			if (this._wItmQtyCSP != null )
				this._wItmQtyCSP = null;
			if (this._ItmMFNQtyReq != null )
				this._ItmMFNQtyReq = null;
		}

		#endregion

		#region +++  Properties  +++

		/// <summary>
		/// Codes for assigning and retrieving property UID.
		/// </summary>
		public int? UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				this._UID = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property UserKey.
		/// </summary>
		public int? UserKey
		{
			get
			{
				return this._UserKey;
			}
			set
			{
				this._UserKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property LocalData.
		/// </summary>
		public bool LocalData
		{
			get
			{
				return this._LocalData;
			}
			set
			{
				this._LocalData = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property PgmSign.
		/// </summary>
		public int? PgmSign
		{
			get
			{
				return this._PgmSign;
			}
			set
			{
				this._PgmSign = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property INHisSign.
		/// </summary>
		public int? INHisSign
		{
			get
			{
				return this._INHisSign;
			}
			set
			{
				this._INHisSign = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocCodeKey.
		/// </summary>
		public int? DocCodeKey
		{
			get
			{
				return this._DocCodeKey;
			}
			set
			{
				this._DocCodeKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocKey.
		/// </summary>
		public int? DocKey
		{
			get
			{
				return this._DocKey;
			}
			set
			{
				this._DocKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocItmKey.
		/// </summary>
		public int? DocItmKey
		{
			get
			{
				return this._DocItmKey;
			}
			set
			{
				this._DocItmKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocDate.
		/// </summary>
		public DateTime? DocDate
		{
			get
			{
				return this._DocDate;
			}
			set
			{
				this._DocDate = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocPeriod.
		/// </summary>
		public int? DocPeriod
		{
			get
			{
				return this._DocPeriod;
			}
			set
			{
				this._DocPeriod = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocSign.
		/// </summary>
		public short? DocSign
		{
			get
			{
				return this._DocSign;
			}
			set
			{
				this._DocSign = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocType.
		/// </summary>
		public string DocType
		{
			get
			{
				return this._DocType;
			}
			set
			{
				this._DocType = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocCurrRate.
		/// </summary>
		public float? DocCurrRate
		{
			get
			{
				return this._DocCurrRate;
			}
			set
			{
				this._DocCurrRate = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property DocAddCostFactor.
		/// </summary>
		public decimal? DocAddCostFactor
		{
			get
			{
				return this._DocAddCostFactor;
			}
			set
			{
				this._DocAddCostFactor = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmKey.
		/// </summary>
		public int? ItmKey
		{
			get
			{
				return this._ItmKey;
			}
			set
			{
				this._ItmKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmSN.
		/// </summary>
		public float? ItmSN
		{
			get
			{
				return this._ItmSN;
			}
			set
			{
				this._ItmSN = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmDeptKey.
		/// </summary>
		public int? ItmDeptKey
		{
			get
			{
				return this._ItmDeptKey;
			}
			set
			{
				this._ItmDeptKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmTranGrpKey.
		/// </summary>
		public int? ItmTranGrpKey
		{
			get
			{
				return this._ItmTranGrpKey;
			}
			set
			{
				this._ItmTranGrpKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmType.
		/// </summary>
		public string ItmType
		{
			get
			{
				return this._ItmType;
			}
			set
			{
				this._ItmType = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmCostMethod.
		/// </summary>
		public string ItmCostMethod
		{
			get
			{
				return this._ItmCostMethod;
			}
			set
			{
				this._ItmCostMethod = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmAccINKey.
		/// </summary>
		public int? ItmAccINKey
		{
			get
			{
				return this._ItmAccINKey;
			}
			set
			{
				this._ItmAccINKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmAccPHKey.
		/// </summary>
		public int? ItmAccPHKey
		{
			get
			{
				return this._ItmAccPHKey;
			}
			set
			{
				this._ItmAccPHKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmDes.
		/// </summary>
		public string ItmDes
		{
			get
			{
				return this._ItmDes;
			}
			set
			{
				this._ItmDes = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmLocKey.
		/// </summary>
		public int? ItmLocKey
		{
			get
			{
				return this._ItmLocKey;
			}
			set
			{
				this._ItmLocKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmQty.
		/// </summary>
		public decimal? ItmQty
		{
			get
			{
				return this._ItmQty;
			}
			set
			{
				this._ItmQty = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmConRate.
		/// </summary>
		public decimal? ItmConRate
		{
			get
			{
				return this._ItmConRate;
			}
			set
			{
				this._ItmConRate = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmPrice.
		/// </summary>
		public decimal? ItmPrice
		{
			get
			{
				return this._ItmPrice;
			}
			set
			{
				this._ItmPrice = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmAmtH.
		/// </summary>
		public decimal? ItmAmtH
		{
			get
			{
				return this._ItmAmtH;
			}
			set
			{
				this._ItmAmtH = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmAddCostF.
		/// </summary>
		public decimal? ItmAddCostF
		{
			get
			{
				return this._ItmAddCostF;
			}
			set
			{
				this._ItmAddCostF = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmAddCostH.
		/// </summary>
		public decimal? ItmAddCostH
		{
			get
			{
				return this._ItmAddCostH;
			}
			set
			{
				this._ItmAddCostH = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmAddAmtF.
		/// </summary>
		public decimal? ItmAddAmtF
		{
			get
			{
				return this._ItmAddAmtF;
			}
			set
			{
				this._ItmAddAmtF = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmAddAmtH.
		/// </summary>
		public decimal? ItmAddAmtH
		{
			get
			{
				return this._ItmAddAmtH;
			}
			set
			{
				this._ItmAddAmtH = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmQtyLink.
		/// </summary>
		public decimal? ItmQtyLink
		{
			get
			{
				return this._ItmQtyLink;
			}
			set
			{
				this._ItmQtyLink = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmQtyAdj.
		/// </summary>
		public decimal? ItmQtyAdj
		{
			get
			{
				return this._ItmQtyAdj;
			}
			set
			{
				this._ItmQtyAdj = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmQtyShw.
		/// </summary>
		public decimal? ItmQtyShw
		{
			get
			{
				return this._ItmQtyShw;
			}
			set
			{
				this._ItmQtyShw = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmPrmDate.
		/// </summary>
		public DateTime? ItmPrmDate
		{
			get
			{
				return this._ItmPrmDate;
			}
			set
			{
				this._ItmPrmDate = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmIGrpDItm.
		/// </summary>
		public int? ItmIGrpDItm
		{
			get
			{
				return this._ItmIGrpDItm;
			}
			set
			{
				this._ItmIGrpDItm = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ARSODK.
		/// </summary>
		public int? ARSODK
		{
			get
			{
				return this._ARSODK;
			}
			set
			{
				this._ARSODK = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ARSODItm.
		/// </summary>
		public int? ARSODItm
		{
			get
			{
				return this._ARSODItm;
			}
			set
			{
				this._ARSODItm = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property SOPeriod.
		/// </summary>
		public int? SOPeriod
		{
			get
			{
				return this._SOPeriod;
			}
			set
			{
				this._SOPeriod = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property SOItmETA.
		/// </summary>
		public DateTime? SOItmETA
		{
			get
			{
				return this._SOItmETA;
			}
			set
			{
				this._SOItmETA = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property SOItmConRate.
		/// </summary>
		public decimal? SOItmConRate
		{
			get
			{
				return this._SOItmConRate;
			}
			set
			{
				this._SOItmConRate = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property APPODK.
		/// </summary>
		public int? APPODK
		{
			get
			{
				return this._APPODK;
			}
			set
			{
				this._APPODK = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property APPODItm.
		/// </summary>
		public int? APPODItm
		{
			get
			{
				return this._APPODItm;
			}
			set
			{
				this._APPODItm = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property POPeriod.
		/// </summary>
		public int? POPeriod
		{
			get
			{
				return this._POPeriod;
			}
			set
			{
				this._POPeriod = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property POItmETA.
		/// </summary>
		public DateTime? POItmETA
		{
			get
			{
				return this._POItmETA;
			}
			set
			{
				this._POItmETA = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property POItmConRate.
		/// </summary>
		public decimal? POItmConRate
		{
			get
			{
				return this._POItmConRate;
			}
			set
			{
				this._POItmConRate = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property APPDDK.
		/// </summary>
		public int? APPDDK
		{
			get
			{
				return this._APPDDK;
			}
			set
			{
				this._APPDDK = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property APPDDItm.
		/// </summary>
		public int? APPDDItm
		{
			get
			{
				return this._APPDDItm;
			}
			set
			{
				this._APPDDItm = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property PDPeriod.
		/// </summary>
		public int? PDPeriod
		{
			get
			{
				return this._PDPeriod;
			}
			set
			{
				this._PDPeriod = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property PDItmDept.
		/// </summary>
		public int? PDItmDept
		{
			get
			{
				return this._PDItmDept;
			}
			set
			{
				this._PDItmDept = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property PDItmTranGrp.
		/// </summary>
		public int? PDItmTranGrp
		{
			get
			{
				return this._PDItmTranGrp;
			}
			set
			{
				this._PDItmTranGrp = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property PDItmQty.
		/// </summary>
		public decimal? PDItmQty
		{
			get
			{
				return this._PDItmQty;
			}
			set
			{
				this._PDItmQty = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property PDItmCostH.
		/// </summary>
		public decimal? PDItmCostH
		{
			get
			{
				return this._PDItmCostH;
			}
			set
			{
				this._PDItmCostH = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property PDItmAmtH.
		/// </summary>
		public decimal? PDItmAmtH
		{
			get
			{
				return this._PDItmAmtH;
			}
			set
			{
				this._PDItmAmtH = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property INCPSDK.
		/// </summary>
		public int? INCPSDK
		{
			get
			{
				return this._INCPSDK;
			}
			set
			{
				this._INCPSDK = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property INCPSDItm.
		/// </summary>
		public int? INCPSDItm
		{
			get
			{
				return this._INCPSDItm;
			}
			set
			{
				this._INCPSDItm = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property INCPSItmConRate.
		/// </summary>
		public decimal? INCPSItmConRate
		{
			get
			{
				return this._INCPSItmConRate;
			}
			set
			{
				this._INCPSItmConRate = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property INCSIDK.
		/// </summary>
		public int? INCSIDK
		{
			get
			{
				return this._INCSIDK;
			}
			set
			{
				this._INCSIDK = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property INCSIDItm.
		/// </summary>
		public int? INCSIDItm
		{
			get
			{
				return this._INCSIDItm;
			}
			set
			{
				this._INCSIDItm = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property CPODK.
		/// </summary>
		public int? CPODK
		{
			get
			{
				return this._CPODK;
			}
			set
			{
				this._CPODK = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property CPODItm.
		/// </summary>
		public int? CPODItm
		{
			get
			{
				return this._CPODItm;
			}
			set
			{
				this._CPODItm = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property COPeriod.
		/// </summary>
		public int? COPeriod
		{
			get
			{
				return this._COPeriod;
			}
			set
			{
				this._COPeriod = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property COItmETA.
		/// </summary>
		public DateTime? COItmETA
		{
			get
			{
				return this._COItmETA;
			}
			set
			{
				this._COItmETA = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property LineType.
		/// </summary>
		public int? LineType
		{
			get
			{
				return this._LineType;
			}
			set
			{
				this._LineType = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmFGKey.
		/// </summary>
		public int? ItmFGKey
		{
			get
			{
				return this._ItmFGKey;
			}
			set
			{
				this._ItmFGKey = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property FGOverHeadCost.
		/// </summary>
		public decimal? FGOverHeadCost
		{
			get
			{
				return this._FGOverHeadCost;
			}
			set
			{
				this._FGOverHeadCost = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property FGCostRatio.
		/// </summary>
		public decimal? FGCostRatio
		{
			get
			{
				return this._FGCostRatio;
			}
			set
			{
				this._FGCostRatio = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property wItmQty.
		/// </summary>
		public decimal? wItmQty
		{
			get
			{
				return this._wItmQty;
			}
			set
			{
				this._wItmQty = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property wItmLatestCost.
		/// </summary>
		public decimal? wItmLatestCost
		{
			get
			{
				return this._wItmLatestCost;
			}
			set
			{
				this._wItmLatestCost = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property wItmSOQty.
		/// </summary>
		public decimal? wItmSOQty
		{
			get
			{
				return this._wItmSOQty;
			}
			set
			{
				this._wItmSOQty = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property wItmPOQty.
		/// </summary>
		public decimal? wItmPOQty
		{
			get
			{
				return this._wItmPOQty;
			}
			set
			{
				this._wItmPOQty = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property wItmQtyHis.
		/// </summary>
		public decimal? wItmQtyHis
		{
			get
			{
				return this._wItmQtyHis;
			}
			set
			{
				this._wItmQtyHis = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property wItmQtyCSP.
		/// </summary>
		public decimal? wItmQtyCSP
		{
			get
			{
				return this._wItmQtyCSP;
			}
			set
			{
				this._wItmQtyCSP = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property ItmMFNQtyReq.
		/// </summary>
		public decimal? ItmMFNQtyReq
		{
			get
			{
				return this._ItmMFNQtyReq;
			}
			set
			{
				this._ItmMFNQtyReq = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property wMatchLocalData.
		/// </summary>
		public bool wMatchLocalData
		{
			get
			{
				return this._wMatchLocalData;
			}
			set
			{
				this._wMatchLocalData = value;
				_isDirty=true;
			}
		}

		/// <summary>
		/// Codes for assigning and retrieving property isDirty.
		/// </summary>
		public bool isDirty
		{
			get
			{
				return this._isDirty;
			}
		}


		#endregion

        public static DataTable ToTable()
        {
            DataTable dt = new DataTable();
            Type tType = typeof(DocItmTmp);
            PropertyInfo[] props = tType.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name.Equals("item")) continue;
                DataColumn col = new DataColumn(prop.Name, prop.PropertyType);
                dt.Columns.Add(col);
            }
            return dt;
        }
    }
}

