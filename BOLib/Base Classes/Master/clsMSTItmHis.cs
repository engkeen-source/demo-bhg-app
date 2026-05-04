

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
    public class MSTItmHis : Csla.BusinessBase<MSTItmHis>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _itmKey = null;
        internal int? _locKey = null;
        internal int? _period = null;
        internal decimal? _aRSOQty = null;
        internal decimal? _aRSOAmt = null;
        internal decimal? _aRSOQtyLink = null;
        internal decimal? _aRSOAmtLink = null;
        internal decimal? _aRDOQty = null;
        internal decimal? _aRDOAmt = null;
        internal decimal? _aRDOQtyInvoiced = null;
        internal decimal? _aRDOAmtInvoiced = null;
        internal decimal? _aRIVQty = null;
        internal decimal? _aRIVAmt = null;
        internal decimal? _aRDNQty = null;
        internal decimal? _aRDNAmt = null;
        internal decimal? _aRCNQty = null;
        internal decimal? _aRCNAmt = null;
        internal decimal? _aRPOSQty = null;
        internal decimal? _aRPOSAmt = null;
        internal decimal? _aPPNQty = null;
        internal decimal? _aPPNAmt = null;
        internal decimal? _aPPNQtyPosted = null;
        internal decimal? _aPPNAmtPosted = null;
        internal decimal? _aPPOQty = null;
        internal decimal? _aPPOAmt = null;
        internal decimal? _aPPOQtyLink = null;
        internal decimal? _aPPOAmtLink = null;
        internal decimal? _aPPDQty = null;
        internal decimal? _aPPDAmt = null;
        internal decimal? _aPPDQtyInvoiced = null;
        internal decimal? _aPPDAmtInvoiced = null;
        internal decimal? _aPBLQty = null;
        internal decimal? _aPBLAmt = null;
        internal decimal? _aPDNQty = null;
        internal decimal? _aPDNAmt = null;
        internal decimal? _aPCNQty = null;
        internal decimal? _aPCNAmt = null;
        internal decimal? _iNADJQty = null;
        internal decimal? _iNADJAmt = null;
        internal decimal? _iNPDTFGQty = null;
        internal decimal? _iNPDTFGAmt = null;
        internal decimal? _iNPDTFGQtyPosted = null;
        internal decimal? _iNPDTFGAmtPosted = null;
        internal decimal? _iNPDTRMQty = null;
        internal decimal? _iNPDTRMAmt = null;
        internal decimal? _iNPDTRMQtyPosted = null;
        internal decimal? _iNPDTRMAmtPosted = null;
        internal decimal? _iNPDTPMQty = null;
        internal decimal? _iNPDTPMAmt = null;
        internal decimal? _iNPDTPMQtyPosted = null;
        internal decimal? _iNPDTPMAmtPosted = null;
        internal decimal? _iNTRNQty = null;
        internal decimal? _iNTRNAmt = null;
        internal decimal? _iNCSIQty = null;
        internal decimal? _iNCSIAmt = null;
        internal decimal? _iNCSIQtyLink = null;
        internal decimal? _iNCSIAmtLink = null;
        internal decimal? _iNCSRQty = null;
        internal decimal? _iNCSRAmt = null;
        internal decimal? _iNCPOQty = null;
        internal decimal? _iNCPOAmt = null;
        internal decimal? _iNCPOQtyLink = null;
        internal decimal? _iNCPOAmtLink = null;
        internal decimal? _iNCPDQty = null;
        internal decimal? _iNCPDAmt = null;
        internal decimal? _totalQty = null;
        internal decimal? _totalValue = null;

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

        public int? LocKey
        {
            get
            {
                return _locKey;
            }
            set
            {
                _locKey = value;
                PropertyHasChanged("LocKey");
            }
        }

        public int? Period
        {
            get
            {
                return _period;
            }
            set
            {
                _period = value;
                PropertyHasChanged("Period");
            }
        }

        public decimal? ARSOQty
        {
            get
            {
                return _aRSOQty;
            }
            set
            {
                _aRSOQty = value;
                PropertyHasChanged("ARSOQty");
            }
        }

        public decimal? ARSOAmt
        {
            get
            {
                return _aRSOAmt;
            }
            set
            {
                _aRSOAmt = value;
                PropertyHasChanged("ARSOAmt");
            }
        }

        public decimal? ARSOQtyLink
        {
            get
            {
                return _aRSOQtyLink;
            }
            set
            {
                _aRSOQtyLink = value;
                PropertyHasChanged("ARSOQtyLink");
            }
        }

        public decimal? ARSOAmtLink
        {
            get
            {
                return _aRSOAmtLink;
            }
            set
            {
                _aRSOAmtLink = value;
                PropertyHasChanged("ARSOAmtLink");
            }
        }

        public decimal? ARDOQty
        {
            get
            {
                return _aRDOQty;
            }
            set
            {
                _aRDOQty = value;
                PropertyHasChanged("ARDOQty");
            }
        }

        public decimal? ARDOAmt
        {
            get
            {
                return _aRDOAmt;
            }
            set
            {
                _aRDOAmt = value;
                PropertyHasChanged("ARDOAmt");
            }
        }

        public decimal? ARDOQtyInvoiced
        {
            get
            {
                return _aRDOQtyInvoiced;
            }
            set
            {
                _aRDOQtyInvoiced = value;
                PropertyHasChanged("ARDOQtyInvoiced");
            }
        }

        public decimal? ARDOAmtInvoiced
        {
            get
            {
                return _aRDOAmtInvoiced;
            }
            set
            {
                _aRDOAmtInvoiced = value;
                PropertyHasChanged("ARDOAmtInvoiced");
            }
        }

        public decimal? ARIVQty
        {
            get
            {
                return _aRIVQty;
            }
            set
            {
                _aRIVQty = value;
                PropertyHasChanged("ARIVQty");
            }
        }

        public decimal? ARIVAmt
        {
            get
            {
                return _aRIVAmt;
            }
            set
            {
                _aRIVAmt = value;
                PropertyHasChanged("ARIVAmt");
            }
        }

        public decimal? ARDNQty
        {
            get
            {
                return _aRDNQty;
            }
            set
            {
                _aRDNQty = value;
                PropertyHasChanged("ARDNQty");
            }
        }

        public decimal? ARDNAmt
        {
            get
            {
                return _aRDNAmt;
            }
            set
            {
                _aRDNAmt = value;
                PropertyHasChanged("ARDNAmt");
            }
        }

        public decimal? ARCNQty
        {
            get
            {
                return _aRCNQty;
            }
            set
            {
                _aRCNQty = value;
                PropertyHasChanged("ARCNQty");
            }
        }

        public decimal? ARCNAmt
        {
            get
            {
                return _aRCNAmt;
            }
            set
            {
                _aRCNAmt = value;
                PropertyHasChanged("ARCNAmt");
            }
        }

        public decimal? ARPOSQty
        {
            get
            {
                return _aRPOSQty;
            }
            set
            {
                _aRPOSQty = value;
                PropertyHasChanged("ARPOSQty");
            }
        }

        public decimal? ARPOSAmt
        {
            get
            {
                return _aRPOSAmt;
            }
            set
            {
                _aRPOSAmt = value;
                PropertyHasChanged("ARPOSAmt");
            }
        }

        public decimal? APPNQty
        {
            get
            {
                return _aPPNQty;
            }
            set
            {
                _aPPNQty = value;
                PropertyHasChanged("APPNQty");
            }
        }

        public decimal? APPNAmt
        {
            get
            {
                return _aPPNAmt;
            }
            set
            {
                _aPPNAmt = value;
                PropertyHasChanged("APPNAmt");
            }
        }

        public decimal? APPNQtyPosted
        {
            get
            {
                return _aPPNQtyPosted;
            }
            set
            {
                _aPPNQtyPosted = value;
                PropertyHasChanged("APPNQtyPosted");
            }
        }

        public decimal? APPNAmtPosted
        {
            get
            {
                return _aPPNAmtPosted;
            }
            set
            {
                _aPPNAmtPosted = value;
                PropertyHasChanged("APPNAmtPosted");
            }
        }

        public decimal? CSCPOQty
        {
            get
            {
                return _aPPOQty;
            }
            set
            {
                _aPPOQty = value;
                PropertyHasChanged("CSCPOQty");
            }
        }

        public decimal? CSCPOAmt
        {
            get
            {
                return _aPPOAmt;
            }
            set
            {
                _aPPOAmt = value;
                PropertyHasChanged("CSCPOAmt");
            }
        }

        public decimal? CSCPOQtyLink
        {
            get
            {
                return _aPPOQtyLink;
            }
            set
            {
                _aPPOQtyLink = value;
                PropertyHasChanged("CSCPOQtyLink");
            }
        }

        public decimal? CSCPOAmtLink
        {
            get
            {
                return _aPPOAmtLink;
            }
            set
            {
                _aPPOAmtLink = value;
                PropertyHasChanged("CSCPOAmtLink");
            }
        }

        public decimal? APPDQty
        {
            get
            {
                return _aPPDQty;
            }
            set
            {
                _aPPDQty = value;
                PropertyHasChanged("APPDQty");
            }
        }

        public decimal? APPDAmt
        {
            get
            {
                return _aPPDAmt;
            }
            set
            {
                _aPPDAmt = value;
                PropertyHasChanged("APPDAmt");
            }
        }

        public decimal? APPDQtyInvoiced
        {
            get
            {
                return _aPPDQtyInvoiced;
            }
            set
            {
                _aPPDQtyInvoiced = value;
                PropertyHasChanged("APPDQtyInvoiced");
            }
        }

        public decimal? APPDAmtInvoiced
        {
            get
            {
                return _aPPDAmtInvoiced;
            }
            set
            {
                _aPPDAmtInvoiced = value;
                PropertyHasChanged("APPDAmtInvoiced");
            }
        }

        public decimal? APBLQty
        {
            get
            {
                return _aPBLQty;
            }
            set
            {
                _aPBLQty = value;
                PropertyHasChanged("APBLQty");
            }
        }

        public decimal? APBLAmt
        {
            get
            {
                return _aPBLAmt;
            }
            set
            {
                _aPBLAmt = value;
                PropertyHasChanged("APBLAmt");
            }
        }

        public decimal? APDNQty
        {
            get
            {
                return _aPDNQty;
            }
            set
            {
                _aPDNQty = value;
                PropertyHasChanged("APDNQty");
            }
        }

        public decimal? APDNAmt
        {
            get
            {
                return _aPDNAmt;
            }
            set
            {
                _aPDNAmt = value;
                PropertyHasChanged("APDNAmt");
            }
        }

        public decimal? APCNQty
        {
            get
            {
                return _aPCNQty;
            }
            set
            {
                _aPCNQty = value;
                PropertyHasChanged("APCNQty");
            }
        }

        public decimal? APCNAmt
        {
            get
            {
                return _aPCNAmt;
            }
            set
            {
                _aPCNAmt = value;
                PropertyHasChanged("APCNAmt");
            }
        }

        public decimal? INADJQty
        {
            get
            {
                return _iNADJQty;
            }
            set
            {
                _iNADJQty = value;
                PropertyHasChanged("INADJQty");
            }
        }

        public decimal? INADJAmt
        {
            get
            {
                return _iNADJAmt;
            }
            set
            {
                _iNADJAmt = value;
                PropertyHasChanged("INADJAmt");
            }
        }

        public decimal? INPDTFGQty
        {
            get
            {
                return _iNPDTFGQty;
            }
            set
            {
                _iNPDTFGQty = value;
                PropertyHasChanged("INPDTFGQty");
            }
        }

        public decimal? INPDTFGAmt
        {
            get
            {
                return _iNPDTFGAmt;
            }
            set
            {
                _iNPDTFGAmt = value;
                PropertyHasChanged("INPDTFGAmt");
            }
        }

        public decimal? INPDTFGQtyPosted
        {
            get
            {
                return _iNPDTFGQtyPosted;
            }
            set
            {
                _iNPDTFGQtyPosted = value;
                PropertyHasChanged("INPDTFGQtyPosted");
            }
        }

        public decimal? INPDTFGAmtPosted
        {
            get
            {
                return _iNPDTFGAmtPosted;
            }
            set
            {
                _iNPDTFGAmtPosted = value;
                PropertyHasChanged("INPDTFGAmtPosted");
            }
        }

        public decimal? INPDTRMQty
        {
            get
            {
                return _iNPDTRMQty;
            }
            set
            {
                _iNPDTRMQty = value;
                PropertyHasChanged("INPDTRMQty");
            }
        }

        public decimal? INPDTRMAmt
        {
            get
            {
                return _iNPDTRMAmt;
            }
            set
            {
                _iNPDTRMAmt = value;
                PropertyHasChanged("INPDTRMAmt");
            }
        }

        public decimal? INPDTRMQtyPosted
        {
            get
            {
                return _iNPDTRMQtyPosted;
            }
            set
            {
                _iNPDTRMQtyPosted = value;
                PropertyHasChanged("INPDTRMQtyPosted");
            }
        }

        public decimal? INPDTRMAmtPosted
        {
            get
            {
                return _iNPDTRMAmtPosted;
            }
            set
            {
                _iNPDTRMAmtPosted = value;
                PropertyHasChanged("INPDTRMAmtPosted");
            }
        }

        public decimal? INPDTPMQty
        {
            get
            {
                return _iNPDTPMQty;
            }
            set
            {
                _iNPDTPMQty = value;
                PropertyHasChanged("INPDTPMQty");
            }
        }

        public decimal? INPDTPMAmt
        {
            get
            {
                return _iNPDTPMAmt;
            }
            set
            {
                _iNPDTPMAmt = value;
                PropertyHasChanged("INPDTPMAmt");
            }
        }

        public decimal? INPDTPMQtyPosted
        {
            get
            {
                return _iNPDTPMQtyPosted;
            }
            set
            {
                _iNPDTPMQtyPosted = value;
                PropertyHasChanged("INPDTPMQtyPosted");
            }
        }

        public decimal? INPDTPMAmtPosted
        {
            get
            {
                return _iNPDTPMAmtPosted;
            }
            set
            {
                _iNPDTPMAmtPosted = value;
                PropertyHasChanged("INPDTPMAmtPosted");
            }
        }

        public decimal? INTRNQty
        {
            get
            {
                return _iNTRNQty;
            }
            set
            {
                _iNTRNQty = value;
                PropertyHasChanged("INTRNQty");
            }
        }

        public decimal? INTRNAmt
        {
            get
            {
                return _iNTRNAmt;
            }
            set
            {
                _iNTRNAmt = value;
                PropertyHasChanged("INTRNAmt");
            }
        }

        public decimal? INCSIQty
        {
            get
            {
                return _iNCSIQty;
            }
            set
            {
                _iNCSIQty = value;
                PropertyHasChanged("INCSIQty");
            }
        }

        public decimal? INCSIAmt
        {
            get
            {
                return _iNCSIAmt;
            }
            set
            {
                _iNCSIAmt = value;
                PropertyHasChanged("INCSIAmt");
            }
        }

        public decimal? INCSIQtyLink
        {
            get
            {
                return _iNCSIQtyLink;
            }
            set
            {
                _iNCSIQtyLink = value;
                PropertyHasChanged("INCSIQtyLink");
            }
        }

        public decimal? INCSIAmtLink
        {
            get
            {
                return _iNCSIAmtLink;
            }
            set
            {
                _iNCSIAmtLink = value;
                PropertyHasChanged("INCSIAmtLink");
            }
        }

        public decimal? INCSRQty
        {
            get
            {
                return _iNCSRQty;
            }
            set
            {
                _iNCSRQty = value;
                PropertyHasChanged("INCSRQty");
            }
        }

        public decimal? INCSRAmt
        {
            get
            {
                return _iNCSRAmt;
            }
            set
            {
                _iNCSRAmt = value;
                PropertyHasChanged("INCSRAmt");
            }
        }

        public decimal? INCPOQty
        {
            get
            {
                return _iNCPOQty;
            }
            set
            {
                _iNCPOQty = value;
                PropertyHasChanged("INCPOQty");
            }
        }

        public decimal? INCPOAmt
        {
            get
            {
                return _iNCPOAmt;
            }
            set
            {
                _iNCPOAmt = value;
                PropertyHasChanged("INCPOAmt");
            }
        }

        public decimal? INCPOQtyLink
        {
            get
            {
                return _iNCPOQtyLink;
            }
            set
            {
                _iNCPOQtyLink = value;
                PropertyHasChanged("INCPOQtyLink");
            }
        }

        public decimal? INCPOAmtLink
        {
            get
            {
                return _iNCPOAmtLink;
            }
            set
            {
                _iNCPOAmtLink = value;
                PropertyHasChanged("INCPOAmtLink");
            }
        }

        public decimal? INCPDQty
        {
            get
            {
                return _iNCPDQty;
            }
            set
            {
                _iNCPDQty = value;
                PropertyHasChanged("INCPDQty");
            }
        }

        public decimal? INCPDAmt
        {
            get
            {
                return _iNCPDAmt;
            }
            set
            {
                _iNCPDAmt = value;
                PropertyHasChanged("INCPDAmt");
            }
        }

        public decimal? TotalQty
        {
            get
            {
                return _totalQty;
            }
            set
            {
                _totalQty = value;
                PropertyHasChanged("TotalQty");
            }
        }

        public decimal? TotalValue
        {
            get
            {
                return _totalValue;
            }
            set
            {
                _totalValue = value;
                PropertyHasChanged("TotalValue");
            }
        }

        protected override object GetIdValue()
        {
            return _itmKey.ToString() + _locKey.ToString() + _period.ToString();
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

        internal MSTItmHis()
        { /* require use of factory method */ }

        internal static MSTItmHis New()
        {           
            MSTItmHis child = new MSTItmHis();         
            return child;
        }

        internal static MSTItmHis NewChild()
        {          
            MSTItmHis child = new MSTItmHis();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();         
            return child;
        }

        internal static MSTItmHis Get(SafeDataReader dr)
        {           
            MSTItmHis child = new MSTItmHis();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTItmHis Get(int? itmKey, int? locKey, int? period)
        {           
            MSTItmHis child = new MSTItmHis();
            child.Fetch(new Criteria(itmKey, locKey, period, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _itmKey = null;
            public int? _locKey = null;
            public int? _period = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ItmKey, int? LocKey, int? Period)
            {
                _itmKey = ItmKey;
                _locKey = LocKey;
                _period = Period;
            }

            internal Criteria(int? ItmKey, int? LocKey, int? Period, int? Option)
            {
                _itmKey = ItmKey;
                _locKey = LocKey;
                _period = Period;
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
                cm.CommandText = "MSTItmHis_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);               
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
                cm.Parameters.AddWithValue("@Period", criteria._period);
             

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
                    retValue=false;
                }            
              
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _itmKey = dr.GetInt32("ItmKey");
            _locKey = dr.GetInt32("LocKey");
            _period = dr.GetInt32("Period");
            _aRSOQty = dr.GetDecimal("ARSOQty");
            _aRSOAmt = dr.GetDecimal("ARSOAmt");
            _aRSOQtyLink = dr.GetDecimal("ARSOQtyLink");
            _aRSOAmtLink = dr.GetDecimal("ARSOAmtLink");
            _aRDOQty = dr.GetDecimal("ARDOQty");
            _aRDOAmt = dr.GetDecimal("ARDOAmt");
            _aRDOQtyInvoiced = dr.GetDecimal("ARDOQtyInvoiced");
            _aRDOAmtInvoiced = dr.GetDecimal("ARDOAmtInvoiced");
            _aRIVQty = dr.GetDecimal("ARIVQty");
            _aRIVAmt = dr.GetDecimal("ARIVAmt");
            _aRDNQty = dr.GetDecimal("ARDNQty");
            _aRDNAmt = dr.GetDecimal("ARDNAmt");
            _aRCNQty = dr.GetDecimal("ARCNQty");
            _aRCNAmt = dr.GetDecimal("ARCNAmt");
            _aRPOSQty = dr.GetDecimal("ARPOSQty");
            _aRPOSAmt = dr.GetDecimal("ARPOSAmt");
            _aPPNQty = dr.GetDecimal("APPNQty");
            _aPPNAmt = dr.GetDecimal("APPNAmt");
            _aPPNQtyPosted = dr.GetDecimal("APPNQtyPosted");
            _aPPNAmtPosted = dr.GetDecimal("APPNAmtPosted");
            _aPPOQty = dr.GetDecimal("CSCPOQty");
            _aPPOAmt = dr.GetDecimal("CSCPOAmt");
            _aPPOQtyLink = dr.GetDecimal("CSCPOQtyLink");
            _aPPOAmtLink = dr.GetDecimal("CSCPOAmtLink");
            _aPPDQty = dr.GetDecimal("APPDQty");
            _aPPDAmt = dr.GetDecimal("APPDAmt");
            _aPPDQtyInvoiced = dr.GetDecimal("APPDQtyInvoiced");
            _aPPDAmtInvoiced = dr.GetDecimal("APPDAmtInvoiced");
            _aPBLQty = dr.GetDecimal("APBLQty");
            _aPBLAmt = dr.GetDecimal("APBLAmt");
            _aPDNQty = dr.GetDecimal("APDNQty");
            _aPDNAmt = dr.GetDecimal("APDNAmt");
            _aPCNQty = dr.GetDecimal("APCNQty");
            _aPCNAmt = dr.GetDecimal("APCNAmt");
            _iNADJQty = dr.GetDecimal("INADJQty");
            _iNADJAmt = dr.GetDecimal("INADJAmt");
            _iNPDTFGQty = dr.GetDecimal("INPDTFGQty");
            _iNPDTFGAmt = dr.GetDecimal("INPDTFGAmt");
            _iNPDTFGQtyPosted = dr.GetDecimal("INPDTFGQtyPosted");
            _iNPDTFGAmtPosted = dr.GetDecimal("INPDTFGAmtPosted");
            _iNPDTRMQty = dr.GetDecimal("INPDTRMQty");
            _iNPDTRMAmt = dr.GetDecimal("INPDTRMAmt");
            _iNPDTRMQtyPosted = dr.GetDecimal("INPDTRMQtyPosted");
            _iNPDTRMAmtPosted = dr.GetDecimal("INPDTRMAmtPosted");
            _iNPDTPMQty = dr.GetDecimal("INPDTPMQty");
            _iNPDTPMAmt = dr.GetDecimal("INPDTPMAmt");
            _iNPDTPMQtyPosted = dr.GetDecimal("INPDTPMQtyPosted");
            _iNPDTPMAmtPosted = dr.GetDecimal("INPDTPMAmtPosted");
            _iNTRNQty = dr.GetDecimal("INTRNQty");
            _iNTRNAmt = dr.GetDecimal("INTRNAmt");
            _iNCSIQty = dr.GetDecimal("INCSIQty");
            _iNCSIAmt = dr.GetDecimal("INCSIAmt");
            _iNCSIQtyLink = dr.GetDecimal("INCSIQtyLink");
            _iNCSIAmtLink = dr.GetDecimal("INCSIAmtLink");
            _iNCSRQty = dr.GetDecimal("INCSRQty");
            _iNCSRAmt = dr.GetDecimal("INCSRAmt");
            _iNCPOQty = dr.GetDecimal("INCPOQty");
            _iNCPOAmt = dr.GetDecimal("INCPOAmt");
            _iNCPOQtyLink = dr.GetDecimal("INCPOQtyLink");
            _iNCPOAmtLink = dr.GetDecimal("INCPOAmtLink");
            _iNCPDQty = dr.GetDecimal("INCPDQty");
            _iNCPDAmt = dr.GetDecimal("INCPDAmt");
            _totalQty = dr.GetDecimal("TotalQty");
            _totalValue = dr.GetDecimal("TotalValue");
            ValidationRules.CheckRules();
            return false;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? itmKey, out int? locKey, out int? period)
        {
            bool retValue = false;
            itmKey = null;
            locKey = null;
            period = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out itmKey, out locKey, out period);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? itmKey, out int? locKey, out int? period)
        {
            itmKey = 0;
            locKey = 0;
            period = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmHis_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", itmKey);
                cm.Parameters.AddWithValue("@NewLocKey", locKey);
                cm.Parameters.AddWithValue("@NewPeriod", period);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_locKey == null)
                    cm.Parameters.AddWithValue("@LocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocKey", _locKey);

                if (_period == null)
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Period", _period);

                if (_aRSOQty == null)
                    cm.Parameters.AddWithValue("@ARSOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOQty", _aRSOQty);

                if (_aRSOAmt == null)
                    cm.Parameters.AddWithValue("@ARSOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOAmt", _aRSOAmt);

                if (_aRSOQtyLink == null)
                    cm.Parameters.AddWithValue("@ARSOQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOQtyLink", _aRSOQtyLink);

                if (_aRSOAmtLink == null)
                    cm.Parameters.AddWithValue("@ARSOAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOAmtLink", _aRSOAmtLink);

                if (_aRDOQty == null)
                    cm.Parameters.AddWithValue("@ARDOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOQty", _aRDOQty);

                if (_aRDOAmt == null)
                    cm.Parameters.AddWithValue("@ARDOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOAmt", _aRDOAmt);

                if (_aRDOQtyInvoiced == null)
                    cm.Parameters.AddWithValue("@ARDOQtyInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOQtyInvoiced", _aRDOQtyInvoiced);

                if (_aRDOAmtInvoiced == null)
                    cm.Parameters.AddWithValue("@ARDOAmtInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOAmtInvoiced", _aRDOAmtInvoiced);

                if (_aRIVQty == null)
                    cm.Parameters.AddWithValue("@ARIVQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARIVQty", _aRIVQty);

                if (_aRIVAmt == null)
                    cm.Parameters.AddWithValue("@ARIVAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARIVAmt", _aRIVAmt);

                if (_aRDNQty == null)
                    cm.Parameters.AddWithValue("@ARDNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDNQty", _aRDNQty);

                if (_aRDNAmt == null)
                    cm.Parameters.AddWithValue("@ARDNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDNAmt", _aRDNAmt);

                if (_aRCNQty == null)
                    cm.Parameters.AddWithValue("@ARCNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARCNQty", _aRCNQty);

                if (_aRCNAmt == null)
                    cm.Parameters.AddWithValue("@ARCNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARCNAmt", _aRCNAmt);

                if (_aRPOSQty == null)
                    cm.Parameters.AddWithValue("@ARPOSQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARPOSQty", _aRPOSQty);

                if (_aRPOSAmt == null)
                    cm.Parameters.AddWithValue("@ARPOSAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARPOSAmt", _aRPOSAmt);

                if (_aPPNQty == null)
                    cm.Parameters.AddWithValue("@APPNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNQty", _aPPNQty);

                if (_aPPNAmt == null)
                    cm.Parameters.AddWithValue("@APPNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNAmt", _aPPNAmt);

                if (_aPPNQtyPosted == null)
                    cm.Parameters.AddWithValue("@APPNQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNQtyPosted", _aPPNQtyPosted);

                if (_aPPNAmtPosted == null)
                    cm.Parameters.AddWithValue("@APPNAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNAmtPosted", _aPPNAmtPosted);

                if (_aPPOQty == null)
                    cm.Parameters.AddWithValue("@CSCPOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOQty", _aPPOQty);

                if (_aPPOAmt == null)
                    cm.Parameters.AddWithValue("@CSCPOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOAmt", _aPPOAmt);

                if (_aPPOQtyLink == null)
                    cm.Parameters.AddWithValue("@CSCPOQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOQtyLink", _aPPOQtyLink);

                if (_aPPOAmtLink == null)
                    cm.Parameters.AddWithValue("@CSCPOAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOAmtLink", _aPPOAmtLink);

                if (_aPPDQty == null)
                    cm.Parameters.AddWithValue("@APPDQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDQty", _aPPDQty);

                if (_aPPDAmt == null)
                    cm.Parameters.AddWithValue("@APPDAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDAmt", _aPPDAmt);

                if (_aPPDQtyInvoiced == null)
                    cm.Parameters.AddWithValue("@APPDQtyInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDQtyInvoiced", _aPPDQtyInvoiced);

                if (_aPPDAmtInvoiced == null)
                    cm.Parameters.AddWithValue("@APPDAmtInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDAmtInvoiced", _aPPDAmtInvoiced);

                if (_aPBLQty == null)
                    cm.Parameters.AddWithValue("@APBLQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APBLQty", _aPBLQty);

                if (_aPBLAmt == null)
                    cm.Parameters.AddWithValue("@APBLAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APBLAmt", _aPBLAmt);

                if (_aPDNQty == null)
                    cm.Parameters.AddWithValue("@APDNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APDNQty", _aPDNQty);

                if (_aPDNAmt == null)
                    cm.Parameters.AddWithValue("@APDNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APDNAmt", _aPDNAmt);

                if (_aPCNQty == null)
                    cm.Parameters.AddWithValue("@APCNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APCNQty", _aPCNQty);

                if (_aPCNAmt == null)
                    cm.Parameters.AddWithValue("@APCNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APCNAmt", _aPCNAmt);

                if (_iNADJQty == null)
                    cm.Parameters.AddWithValue("@INADJQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INADJQty", _iNADJQty);

                if (_iNADJAmt == null)
                    cm.Parameters.AddWithValue("@INADJAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INADJAmt", _iNADJAmt);

                if (_iNPDTFGQty == null)
                    cm.Parameters.AddWithValue("@INPDTFGQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGQty", _iNPDTFGQty);

                if (_iNPDTFGAmt == null)
                    cm.Parameters.AddWithValue("@INPDTFGAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGAmt", _iNPDTFGAmt);

                if (_iNPDTFGQtyPosted == null)
                    cm.Parameters.AddWithValue("@INPDTFGQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGQtyPosted", _iNPDTFGQtyPosted);

                if (_iNPDTFGAmtPosted == null)
                    cm.Parameters.AddWithValue("@INPDTFGAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGAmtPosted", _iNPDTFGAmtPosted);

                if (_iNPDTRMQty == null)
                    cm.Parameters.AddWithValue("@INPDTRMQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMQty", _iNPDTRMQty);

                if (_iNPDTRMAmt == null)
                    cm.Parameters.AddWithValue("@INPDTRMAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMAmt", _iNPDTRMAmt);

                if (_iNPDTRMQtyPosted == null)
                    cm.Parameters.AddWithValue("@INPDTRMQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMQtyPosted", _iNPDTRMQtyPosted);

                if (_iNPDTRMAmtPosted == null)
                    cm.Parameters.AddWithValue("@INPDTRMAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMAmtPosted", _iNPDTRMAmtPosted);

                if (_iNPDTPMQty == null)
                    cm.Parameters.AddWithValue("@INPDTPMQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMQty", _iNPDTPMQty);

                if (_iNPDTPMAmt == null)
                    cm.Parameters.AddWithValue("@INPDTPMAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMAmt", _iNPDTPMAmt);

                if (_iNPDTPMQtyPosted == null)
                    cm.Parameters.AddWithValue("@INPDTPMQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMQtyPosted", _iNPDTPMQtyPosted);

                if (_iNPDTPMAmtPosted == null)
                    cm.Parameters.AddWithValue("@INPDTPMAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMAmtPosted", _iNPDTPMAmtPosted);

                if (_iNTRNQty == null)
                    cm.Parameters.AddWithValue("@INTRNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTRNQty", _iNTRNQty);

                if (_iNTRNAmt == null)
                    cm.Parameters.AddWithValue("@INTRNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTRNAmt", _iNTRNAmt);

                if (_iNCSIQty == null)
                    cm.Parameters.AddWithValue("@INCSIQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIQty", _iNCSIQty);

                if (_iNCSIAmt == null)
                    cm.Parameters.AddWithValue("@INCSIAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIAmt", _iNCSIAmt);

                if (_iNCSIQtyLink == null)
                    cm.Parameters.AddWithValue("@INCSIQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIQtyLink", _iNCSIQtyLink);

                if (_iNCSIAmtLink == null)
                    cm.Parameters.AddWithValue("@INCSIAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIAmtLink", _iNCSIAmtLink);

                if (_iNCSRQty == null)
                    cm.Parameters.AddWithValue("@INCSRQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSRQty", _iNCSRQty);

                if (_iNCSRAmt == null)
                    cm.Parameters.AddWithValue("@INCSRAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSRAmt", _iNCSRAmt);

                if (_iNCPOQty == null)
                    cm.Parameters.AddWithValue("@INCPOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOQty", _iNCPOQty);

                if (_iNCPOAmt == null)
                    cm.Parameters.AddWithValue("@INCPOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOAmt", _iNCPOAmt);

                if (_iNCPOQtyLink == null)
                    cm.Parameters.AddWithValue("@INCPOQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOQtyLink", _iNCPOQtyLink);

                if (_iNCPOAmtLink == null)
                    cm.Parameters.AddWithValue("@INCPOAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOAmtLink", _iNCPOAmtLink);

                if (_iNCPDQty == null)
                    cm.Parameters.AddWithValue("@INCPDQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPDQty", _iNCPDQty);

                if (_iNCPDAmt == null)
                    cm.Parameters.AddWithValue("@INCPDAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPDAmt", _iNCPDAmt);

                if (_totalQty == null)
                    cm.Parameters.AddWithValue("@TotalQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalQty", _totalQty);

                if (_totalValue == null)
                    cm.Parameters.AddWithValue("@TotalValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalValue", _totalValue);

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewLocKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewPeriod"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

             

                itmKey = (int)cm.Parameters["@NewItmKey"].Value;
                locKey = (int)cm.Parameters["@NewLocKey"].Value;
                period = (int)cm.Parameters["@NewPeriod"].Value;
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
                cm.CommandText = "MSTItmHis_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewItmKey", 0);
                cm.Parameters.AddWithValue("@NewLocKey", 0);
                cm.Parameters.AddWithValue("@NewPeriod", 0);

                if (_itmKey == null)
                    cm.Parameters.AddWithValue("@ItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ItmKey", _itmKey);

                if (_locKey == null)
                    cm.Parameters.AddWithValue("@LocKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LocKey", _locKey);

                if (_period == null)
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Period", _period);

                if (_aRSOQty == null)
                    cm.Parameters.AddWithValue("@ARSOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOQty", _aRSOQty);

                if (_aRSOAmt == null)
                    cm.Parameters.AddWithValue("@ARSOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOAmt", _aRSOAmt);

                if (_aRSOQtyLink == null)
                    cm.Parameters.AddWithValue("@ARSOQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOQtyLink", _aRSOQtyLink);

                if (_aRSOAmtLink == null)
                    cm.Parameters.AddWithValue("@ARSOAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARSOAmtLink", _aRSOAmtLink);

                if (_aRDOQty == null)
                    cm.Parameters.AddWithValue("@ARDOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOQty", _aRDOQty);

                if (_aRDOAmt == null)
                    cm.Parameters.AddWithValue("@ARDOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOAmt", _aRDOAmt);

                if (_aRDOQtyInvoiced == null)
                    cm.Parameters.AddWithValue("@ARDOQtyInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOQtyInvoiced", _aRDOQtyInvoiced);

                if (_aRDOAmtInvoiced == null)
                    cm.Parameters.AddWithValue("@ARDOAmtInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDOAmtInvoiced", _aRDOAmtInvoiced);

                if (_aRIVQty == null)
                    cm.Parameters.AddWithValue("@ARIVQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARIVQty", _aRIVQty);

                if (_aRIVAmt == null)
                    cm.Parameters.AddWithValue("@ARIVAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARIVAmt", _aRIVAmt);

                if (_aRDNQty == null)
                    cm.Parameters.AddWithValue("@ARDNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDNQty", _aRDNQty);

                if (_aRDNAmt == null)
                    cm.Parameters.AddWithValue("@ARDNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARDNAmt", _aRDNAmt);

                if (_aRCNQty == null)
                    cm.Parameters.AddWithValue("@ARCNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARCNQty", _aRCNQty);

                if (_aRCNAmt == null)
                    cm.Parameters.AddWithValue("@ARCNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARCNAmt", _aRCNAmt);

                if (_aRPOSQty == null)
                    cm.Parameters.AddWithValue("@ARPOSQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARPOSQty", _aRPOSQty);

                if (_aRPOSAmt == null)
                    cm.Parameters.AddWithValue("@ARPOSAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ARPOSAmt", _aRPOSAmt);

                if (_aPPNQty == null)
                    cm.Parameters.AddWithValue("@APPNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNQty", _aPPNQty);

                if (_aPPNAmt == null)
                    cm.Parameters.AddWithValue("@APPNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNAmt", _aPPNAmt);

                if (_aPPNQtyPosted == null)
                    cm.Parameters.AddWithValue("@APPNQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNQtyPosted", _aPPNQtyPosted);

                if (_aPPNAmtPosted == null)
                    cm.Parameters.AddWithValue("@APPNAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPNAmtPosted", _aPPNAmtPosted);

                if (_aPPOQty == null)
                    cm.Parameters.AddWithValue("@CSCPOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOQty", _aPPOQty);

                if (_aPPOAmt == null)
                    cm.Parameters.AddWithValue("@CSCPOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOAmt", _aPPOAmt);

                if (_aPPOQtyLink == null)
                    cm.Parameters.AddWithValue("@CSCPOQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOQtyLink", _aPPOQtyLink);

                if (_aPPOAmtLink == null)
                    cm.Parameters.AddWithValue("@CSCPOAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CSCPOAmtLink", _aPPOAmtLink);

                if (_aPPDQty == null)
                    cm.Parameters.AddWithValue("@APPDQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDQty", _aPPDQty);

                if (_aPPDAmt == null)
                    cm.Parameters.AddWithValue("@APPDAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDAmt", _aPPDAmt);

                if (_aPPDQtyInvoiced == null)
                    cm.Parameters.AddWithValue("@APPDQtyInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDQtyInvoiced", _aPPDQtyInvoiced);

                if (_aPPDAmtInvoiced == null)
                    cm.Parameters.AddWithValue("@APPDAmtInvoiced", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APPDAmtInvoiced", _aPPDAmtInvoiced);

                if (_aPBLQty == null)
                    cm.Parameters.AddWithValue("@APBLQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APBLQty", _aPBLQty);

                if (_aPBLAmt == null)
                    cm.Parameters.AddWithValue("@APBLAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APBLAmt", _aPBLAmt);

                if (_aPDNQty == null)
                    cm.Parameters.AddWithValue("@APDNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APDNQty", _aPDNQty);

                if (_aPDNAmt == null)
                    cm.Parameters.AddWithValue("@APDNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APDNAmt", _aPDNAmt);

                if (_aPCNQty == null)
                    cm.Parameters.AddWithValue("@APCNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APCNQty", _aPCNQty);

                if (_aPCNAmt == null)
                    cm.Parameters.AddWithValue("@APCNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@APCNAmt", _aPCNAmt);

                if (_iNADJQty == null)
                    cm.Parameters.AddWithValue("@INADJQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INADJQty", _iNADJQty);

                if (_iNADJAmt == null)
                    cm.Parameters.AddWithValue("@INADJAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INADJAmt", _iNADJAmt);

                if (_iNPDTFGQty == null)
                    cm.Parameters.AddWithValue("@INPDTFGQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGQty", _iNPDTFGQty);

                if (_iNPDTFGAmt == null)
                    cm.Parameters.AddWithValue("@INPDTFGAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGAmt", _iNPDTFGAmt);

                if (_iNPDTFGQtyPosted == null)
                    cm.Parameters.AddWithValue("@INPDTFGQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGQtyPosted", _iNPDTFGQtyPosted);

                if (_iNPDTFGAmtPosted == null)
                    cm.Parameters.AddWithValue("@INPDTFGAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTFGAmtPosted", _iNPDTFGAmtPosted);

                if (_iNPDTRMQty == null)
                    cm.Parameters.AddWithValue("@INPDTRMQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMQty", _iNPDTRMQty);

                if (_iNPDTRMAmt == null)
                    cm.Parameters.AddWithValue("@INPDTRMAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMAmt", _iNPDTRMAmt);

                if (_iNPDTRMQtyPosted == null)
                    cm.Parameters.AddWithValue("@INPDTRMQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMQtyPosted", _iNPDTRMQtyPosted);

                if (_iNPDTRMAmtPosted == null)
                    cm.Parameters.AddWithValue("@INPDTRMAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTRMAmtPosted", _iNPDTRMAmtPosted);

                if (_iNPDTPMQty == null)
                    cm.Parameters.AddWithValue("@INPDTPMQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMQty", _iNPDTPMQty);

                if (_iNPDTPMAmt == null)
                    cm.Parameters.AddWithValue("@INPDTPMAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMAmt", _iNPDTPMAmt);

                if (_iNPDTPMQtyPosted == null)
                    cm.Parameters.AddWithValue("@INPDTPMQtyPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMQtyPosted", _iNPDTPMQtyPosted);

                if (_iNPDTPMAmtPosted == null)
                    cm.Parameters.AddWithValue("@INPDTPMAmtPosted", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INPDTPMAmtPosted", _iNPDTPMAmtPosted);

                if (_iNTRNQty == null)
                    cm.Parameters.AddWithValue("@INTRNQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTRNQty", _iNTRNQty);

                if (_iNTRNAmt == null)
                    cm.Parameters.AddWithValue("@INTRNAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INTRNAmt", _iNTRNAmt);

                if (_iNCSIQty == null)
                    cm.Parameters.AddWithValue("@INCSIQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIQty", _iNCSIQty);

                if (_iNCSIAmt == null)
                    cm.Parameters.AddWithValue("@INCSIAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIAmt", _iNCSIAmt);

                if (_iNCSIQtyLink == null)
                    cm.Parameters.AddWithValue("@INCSIQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIQtyLink", _iNCSIQtyLink);

                if (_iNCSIAmtLink == null)
                    cm.Parameters.AddWithValue("@INCSIAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSIAmtLink", _iNCSIAmtLink);

                if (_iNCSRQty == null)
                    cm.Parameters.AddWithValue("@INCSRQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSRQty", _iNCSRQty);

                if (_iNCSRAmt == null)
                    cm.Parameters.AddWithValue("@INCSRAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCSRAmt", _iNCSRAmt);

                if (_iNCPOQty == null)
                    cm.Parameters.AddWithValue("@INCPOQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOQty", _iNCPOQty);

                if (_iNCPOAmt == null)
                    cm.Parameters.AddWithValue("@INCPOAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOAmt", _iNCPOAmt);

                if (_iNCPOQtyLink == null)
                    cm.Parameters.AddWithValue("@INCPOQtyLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOQtyLink", _iNCPOQtyLink);

                if (_iNCPOAmtLink == null)
                    cm.Parameters.AddWithValue("@INCPOAmtLink", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPOAmtLink", _iNCPOAmtLink);

                if (_iNCPDQty == null)
                    cm.Parameters.AddWithValue("@INCPDQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPDQty", _iNCPDQty);

                if (_iNCPDAmt == null)
                    cm.Parameters.AddWithValue("@INCPDAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@INCPDAmt", _iNCPDAmt);

                if (_totalQty == null)
                    cm.Parameters.AddWithValue("@TotalQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalQty", _totalQty);

                if (_totalValue == null)
                    cm.Parameters.AddWithValue("@TotalValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalValue", _totalValue);

                cm.Parameters["@NewItmKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewLocKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewPeriod"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTItmHis_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
                cm.Parameters.AddWithValue("@Period", criteria._period);

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
                cm.CommandText = "MSTItmHis_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
                cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
                cm.Parameters.AddWithValue("@Period", criteria._period);

                cm.ExecuteNonQuery();
                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            
        }
        #endregion //Data Access - Validation

        private void Clear()
        {
            _itmKey = null;
            _locKey = null;
            _period = null;
            _aRSOQty = null;
            _aRSOAmt = null;
            _aRSOQtyLink = null;
            _aRSOAmtLink = null;
            _aRDOQty = null;
            _aRDOAmt = null;
            _aRDOQtyInvoiced = null;
            _aRDOAmtInvoiced = null;
             _aRIVQty = null;
             _aRIVAmt = null;
             _aRDNQty = null;
             _aRDNAmt = null;
             _aRCNQty = null;
             _aRCNAmt = null;
             _aRPOSQty = null;
             _aRPOSAmt = null;
             _aPPNQty = null;
             _aPPNAmt = null;
             _aPPNQtyPosted = null;
             _aPPNAmtPosted = null;
             _aPPOQty = null;
             _aPPOAmt = null;
             _aPPOQtyLink = null;
             _aPPOAmtLink = null;
             _aPPDQty = null;
             _aPPDAmt = null;
             _aPPDQtyInvoiced = null;
             _aPPDAmtInvoiced = null;
             _aPBLQty = null;
             _aPBLAmt = null;
             _aPDNQty = null;
             _aPDNAmt = null;
             _aPCNQty = null;
             _aPCNAmt = null;
             _iNADJQty = null;
             _iNADJAmt = null;
             _iNPDTFGQty = null;
             _iNPDTFGAmt = null;
             _iNPDTFGQtyPosted = null;
             _iNPDTFGAmtPosted = null;
             _iNPDTRMQty = null;
             _iNPDTRMAmt = null;
             _iNPDTRMQtyPosted = null;
             _iNPDTRMAmtPosted = null;
             _iNPDTPMQty = null;
             _iNPDTPMAmt = null;
             _iNPDTPMQtyPosted = null;
             _iNPDTPMAmtPosted = null;
             _iNTRNQty = null;
             _iNTRNAmt = null;
             _iNCSIQty = null;
             _iNCSIAmt = null;
             _iNCSIQtyLink = null;
             _iNCSIAmtLink = null;
             _iNCSRQty = null;
             _iNCSRAmt = null;
             _iNCPOQty = null;
             _iNCPOAmt = null;
             _iNCPOQtyLink = null;
             _iNCPOAmtLink = null;
             _iNCPDQty = null;
             _iNCPDAmt = null;
             _totalQty = null;
             _totalValue = null;
            
        }
    
    }
}


