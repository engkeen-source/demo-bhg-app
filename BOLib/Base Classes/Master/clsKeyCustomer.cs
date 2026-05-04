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
    public class KeyCustomer : Csla.BusinessBase<KeyCustomer>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _grpKey = 0;
        internal int? _budgetYear = 0;
        internal string _team = string.Empty;
        internal int? _custOrder = 0;
        internal string _sALESREP = string.Empty;
        internal string _custGroup = string.Empty;
        internal int? _conKey1 = 0;
        internal int? _conKey2 = 0;
        internal int? _conKey3 = 0;
        internal int? _conKey4 = 0;
        internal int? _conKey5 = 0;
        internal int? _conKey6 = 0;
        internal int? _conKey7 = 0;
        internal int? _conKey8 = 0;
        internal int? _conKey9 = 0;
        internal int? _conKey10 = 0;
        internal string _cUSTOMER1 = string.Empty;
        internal string _cUSTOMER2 = string.Empty;
        internal string _cUSTOMER3 = string.Empty;
        internal string _cUSTOMER4 = string.Empty;
        internal string _cUSTOMER5 = string.Empty;
        internal string _cUSTOMER6 = string.Empty;
        internal string _cUSTOMER7 = string.Empty;
        internal string _cUSTOMER8 = string.Empty;
        internal string _cUSTOMER9 = string.Empty;
        internal string _cUSTOMER10 = string.Empty;
        internal decimal? _bUDGET = 0;
        internal decimal? _bUDGETCABLES = 0;
        internal decimal? _bUDGETLIGHTINGS = 0;
        internal decimal? _bUDGETLAMPS = 0;
        internal decimal? _bUDGETOTHERS = 0;
        internal bool? _bossCustomer = false;        


        public int? GrpKey
        {
            get
            {
                return _grpKey;
            }
            set
            {
                _grpKey = value;
                PropertyHasChanged("GrpKey");
            }
        }

        public int? BudgetYear
        {
            get
            {
                return _budgetYear;
            }
            set
            {
                _budgetYear = value;
                PropertyHasChanged("BudgetYear");
            }
        }

        public string Team
        {
            get
            {
                return _team;
            }
            set
            {
                _team = value;
                PropertyHasChanged("Team");

            }
        }

        public int? CustOrder
        {
            get
            {
                return _custOrder;
            }
            set
            {

                _custOrder = value;
                PropertyHasChanged("CustOrder");

            }
        }

        public string SALESREP
        {
            get
            {
                return _sALESREP;
            }
            set
            {
                _sALESREP = value;
                PropertyHasChanged("SALESREP");
            }
        }

        public string CustGroup
        {
            get
            {
                return _custGroup;
            }
            set
            {
                _custGroup = value;
                PropertyHasChanged("CustGroup");
            }
        }

        public int? ConKey1
        {
            get
            {
                return _conKey1;
            }
            set
            {
                _conKey1 = value;
                PropertyHasChanged("ConKey1");
            }
        }
        public int? ConKey2
        {
            get
            {
                return _conKey2;
            }
            set
            {
                _conKey2 = value;
                PropertyHasChanged("ConKey2");
            }
        }
        public int? ConKey3
        {
            get
            {
                return _conKey3;
            }
            set
            {
                _conKey3 = value;
                PropertyHasChanged("ConKey3");
            }
        }
        public int? ConKey4
        {
            get
            {
                return _conKey4;
            }
            set
            {
                _conKey4 = value;
                PropertyHasChanged("ConKey4");
            }
        }
        public int? ConKey5
        {
            get
            {
                return _conKey5;
            }
            set
            {
                _conKey5 = value;
                PropertyHasChanged("ConKey5");
            }
        }
        public int? ConKey6
        {
            get
            {
                return _conKey6;
            }
            set
            {
                _conKey6 = value;
                PropertyHasChanged("ConKey6");
            }
        }
        public int? ConKey7
        {
            get
            {
                return _conKey7;
            }
            set
            {
                _conKey7 = value;
                PropertyHasChanged("ConKey7");
            }
        }
        public int? ConKey8
        {
            get
            {
                return _conKey8;
            }
            set
            {
                _conKey8 = value;
                PropertyHasChanged("ConKey8");
            }
        }
        public int? ConKey9
        {
            get
            {
                return _conKey9;
            }
            set
            {
                _conKey9 = value;
                PropertyHasChanged("ConKey9");
            }
        }
        public int? ConKey10
        {
            get
            {
                return _conKey10;
            }
            set
            {
                _conKey10 = value;
                PropertyHasChanged("ConKey10");
            }
        }


        public string CUSTOMER1
        {
            get
            {
                return _cUSTOMER1;
            }
            set
            {
                _cUSTOMER1 = value;
                PropertyHasChanged("CUSTOMER1");
            }
        }
        public string CUSTOMER2
        {
            get
            {
                return _cUSTOMER2;
            }
            set
            {
                _cUSTOMER2 = value;
                PropertyHasChanged("CUSTOMER2");
            }
        }
        public string CUSTOMER3
        {
            get
            {
                return _cUSTOMER3;
            }
            set
            {
                _cUSTOMER3 = value;
                PropertyHasChanged("CUSTOMER3");
            }
        }
        public string CUSTOMER4
        {
            get
            {
                return _cUSTOMER4;
            }
            set
            {
                _cUSTOMER4 = value;
                PropertyHasChanged("CUSTOMER4");
            }
        }
        public string CUSTOMER5
        {
            get
            {
                return _cUSTOMER5;
            }
            set
            {
                _cUSTOMER5 = value;
                PropertyHasChanged("CUSTOMER5");
            }
        }
        public string CUSTOMER6
        {
            get
            {
                return _cUSTOMER6;
            }
            set
            {
                _cUSTOMER6 = value;
                PropertyHasChanged("CUSTOMER6");
            }
        }
        public string CUSTOMER7
        {
            get
            {
                return _cUSTOMER7;
            }
            set
            {
                _cUSTOMER7 = value;
                PropertyHasChanged("CUSTOMER7");
            }
        }
        public string CUSTOMER8
        {
            get
            {
                return _cUSTOMER8;
            }
            set
            {
                _cUSTOMER8 = value;
                PropertyHasChanged("CUSTOMER8");
            }
        }
        public string CUSTOMER9
        {
            get
            {
                return _cUSTOMER9;
            }
            set
            {
                _cUSTOMER9 = value;
                PropertyHasChanged("CUSTOMER9");
            }
        }
        public string CUSTOMER10
        {
            get
            {
                return _cUSTOMER10;
            }
            set
            {
                _cUSTOMER10 = value;
                PropertyHasChanged("CUSTOMER10");
            }
        }

        public decimal? BUDGET
        {
            get
            {
                return _bUDGET;
            }
            set
            {
                _bUDGET = value;
                PropertyHasChanged("BUDGET");
            }
        }
        public decimal? BUDGETCABLES
        {
            get
            {
                return _bUDGETCABLES;
            }
            set
            {
                _bUDGETCABLES = value;
                PropertyHasChanged("BUDGETCABLES");
            }
        }
        public decimal? BUDGETLIGHTINGS
        {
            get
            {
                return _bUDGETLIGHTINGS;
            }
            set
            {
                _bUDGETLIGHTINGS = value;
                PropertyHasChanged("BUDGETLIGHTINGS");
            }
        }
        public decimal? BUDGETLAMPS
        {
            get
            {
                return _bUDGETLAMPS;
            }
            set
            {
                _bUDGETLAMPS = value;
                PropertyHasChanged("BUDGETLAMPS");
            }
        }
        public decimal? BUDGETOTHERS
        {
            get
            {
                return _bUDGETOTHERS;
            }
            set
            {
                _bUDGETOTHERS = value;
                PropertyHasChanged("BUDGETOTHERS");
            }
        }

        public bool? BossCustomer
        {
            get
            {
                return _bossCustomer;
            }
            set
            {
                _bossCustomer = value;
                PropertyHasChanged("BossCustomer");
            }
        }
        


        protected override object GetIdValue()
        {
            return _grpKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            ////
            //// Uomid
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "Uomid");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Uomid", 50));
            ////
            //// UOMShw
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "UOMShw");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UOMShw", 50));
            ////
            //// Custom1
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
            ////
            //// Custom2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
            ////
            //// Custom3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal KeyCustomer()
        { /* require use of factory method */ }

        internal static KeyCustomer New()
        {
            KeyCustomer child = new KeyCustomer();
            return child;
        }

        internal static KeyCustomer NewChild()
        {
            KeyCustomer child = new KeyCustomer();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static KeyCustomer Get(SafeDataReader dr)
        {
            KeyCustomer child = new KeyCustomer();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static KeyCustomer Get(int? grpKey, int? budgetYear)
        {
            KeyCustomer child = new KeyCustomer();
            child.Fetch(new Criteria(grpKey, budgetYear,1));            
            return child;
        }
        internal static KeyCustomer Get(SqlConnection cn, int? grpKey, int? budgetYear)
        {
            KeyCustomer child = new KeyCustomer();
            child.Fetch(cn, new Criteria(grpKey, budgetYear, 1));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _grpKey = null;
            public int? _budgetYear = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? GrpKey, int? BudgetYear)
            {
                _grpKey = GrpKey;
                _budgetYear = BudgetYear;
            }

            internal Criteria(int? GrpKey, int? BudgetYear, int? Option)
            {
                _grpKey = GrpKey;
                _budgetYear = BudgetYear;
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
                cm.CommandText = "BHKeyCustomer_Get";

              

         	/* 0-Expected Fail,1-Pass,2-UnExpected Fail*/


               cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
                cm.Parameters.AddWithValue("@BudgetYear", criteria._budgetYear);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        this.Fetch(dr);
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
               

                // Fill data to entity object
                _grpKey = dr.GetInt32("GrpKey");
                _budgetYear = dr.GetInt32("BudgetYear");
                _team = dr.GetString("Team");
                _custOrder = dr.GetInt32("CustOrder");
                _sALESREP = dr.GetString("SALESREP");
                _custGroup = dr.GetString("CustGroup");
                _conKey1 = dr.GetInt32("ConKey1");
                _conKey2 = dr.GetInt32("ConKey2");
                _conKey3 = dr.GetInt32("ConKey3");
                _conKey4 = dr.GetInt32("ConKey4");
                _conKey5 = dr.GetInt32("ConKey5");
                _conKey6 = dr.GetInt32("ConKey6");
                _conKey7 = dr.GetInt32("ConKey7");
                _conKey8 = dr.GetInt32("ConKey8");
                _conKey9 = dr.GetInt32("ConKey9");
                _conKey10 = dr.GetInt32("ConKey10");
                _cUSTOMER1 = dr.GetString("CUSTOMER1");
                _cUSTOMER2 = dr.GetString("CUSTOMER2");
                _cUSTOMER3 = dr.GetString("CUSTOMER3");
                _cUSTOMER4 = dr.GetString("CUSTOMER4");
                _cUSTOMER5 = dr.GetString("CUSTOMER5");
                _cUSTOMER6 = dr.GetString("CUSTOMER6");
                _cUSTOMER7 = dr.GetString("CUSTOMER7");
                _cUSTOMER8 = dr.GetString("CUSTOMER8");
                _cUSTOMER9 = dr.GetString("CUSTOMER9");
                _cUSTOMER10 = dr.GetString("CUSTOMER10");
                _bUDGET = dr.GetDecimal("BUDGET");
                _bUDGETCABLES = dr.GetDecimal("BUDGETCABLES");
                _bUDGETLIGHTINGS = dr.GetDecimal("BUDGETLIGHTINGS");
                _bUDGETLAMPS = dr.GetDecimal("BUDGETLAMPS");
                _bUDGETOTHERS = dr.GetDecimal("BUDGETOTHERS");
                _bossCustomer = dr.GetBoolean("BossCustomer");
                 return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? keyCustomer)
        {
            bool retValue = false;
            keyCustomer = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn);
                    
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
                    
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "BHKeyCustomer_AddUpdate"; 

                cm.Parameters.AddWithValue("@Option", 0);
                
                if (_grpKey == null)
                    cm.Parameters.AddWithValue("@GrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpKey", _grpKey);
                if (_budgetYear == null)
                    cm.Parameters.AddWithValue("@BudgetYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetYear", _budgetYear);
                if (_team == null)
                    cm.Parameters.AddWithValue("@Team", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Team", _team);
                if (_custOrder == null)
                    cm.Parameters.AddWithValue("@CustOrder", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustOrder", _custOrder);
                if (_sALESREP == null)
                    cm.Parameters.AddWithValue("@SALESREP", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SALESREP", _sALESREP);
                if (_custGroup == null)
                    cm.Parameters.AddWithValue("@CustGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustGroup", _custGroup);
                if (_conKey1 == null)
                    cm.Parameters.AddWithValue("@ConKey1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey1", _conKey1);
                if (_conKey2 == null)
                    cm.Parameters.AddWithValue("@ConKey2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey2", _conKey2);
                if (_conKey3 == null)
                    cm.Parameters.AddWithValue("@ConKey3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey3", _conKey3);
                if (_conKey4 == null)
                    cm.Parameters.AddWithValue("@ConKey4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey4", _conKey4);
                if (_conKey5 == null)
                    cm.Parameters.AddWithValue("@ConKey5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey5", _conKey5);
                if (_conKey6 == null)
                    cm.Parameters.AddWithValue("@ConKey6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey6", _conKey6);
                if (_conKey7 == null)
                    cm.Parameters.AddWithValue("@ConKey7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey7", _conKey7);
                if (_conKey8 == null)
                    cm.Parameters.AddWithValue("@ConKey8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey8", _conKey8);
                if (_conKey9 == null)
                    cm.Parameters.AddWithValue("@ConKey9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey9", _conKey9);
                if (_conKey10 == null)
                    cm.Parameters.AddWithValue("@ConKey10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey10", _conKey10);
                if (_cUSTOMER1 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER1", _cUSTOMER1);
                if (_cUSTOMER2 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER2", _cUSTOMER2);
                if (_cUSTOMER3 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER3", _cUSTOMER3);
                if (_cUSTOMER4 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER4", _cUSTOMER4);
                if (_cUSTOMER5 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER5", _cUSTOMER5);
                if (_cUSTOMER6 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER6", _cUSTOMER6);
                if (_cUSTOMER7 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER7", _cUSTOMER7);
                if (_cUSTOMER8 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER8", _cUSTOMER8);
                if (_cUSTOMER9 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER9", _cUSTOMER9);
                if (_cUSTOMER10 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER10", _cUSTOMER10);
                if (_bUDGET == null)
                    cm.Parameters.AddWithValue("@BUDGET", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGET", _bUDGET);
                if (_bUDGETCABLES == null)
                    cm.Parameters.AddWithValue("@BUDGETCABLES", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETCABLES", _bUDGETCABLES);
                if (_bUDGETLIGHTINGS == null)
                    cm.Parameters.AddWithValue("@BUDGETLIGHTINGS", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETLIGHTINGS", _bUDGETLIGHTINGS);
                if (_bUDGETLAMPS == null)
                    cm.Parameters.AddWithValue("@BUDGETLAMPS", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETLAMPS", _bUDGETLAMPS);
                if (_bUDGETOTHERS == null)
                    cm.Parameters.AddWithValue("@BUDGETOTHERS", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETOTHERS", _bUDGETOTHERS);
                if (_bossCustomer == null)
                    cm.Parameters.AddWithValue("@BossCustomer", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BossCustomer", _bossCustomer);
               
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                
                // Execute command.
                cm.ExecuteNonQuery();

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "BHKeyCustomer_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                
                
                if (_grpKey == null)
                    cm.Parameters.AddWithValue("@GrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@GrpKey", _grpKey);
                if (_budgetYear == null)
                    cm.Parameters.AddWithValue("@BudgetYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BudgetYear", _budgetYear);
                if (_team == null)
                    cm.Parameters.AddWithValue("@Team", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Team", _team);
                if (_custOrder == null)
                    cm.Parameters.AddWithValue("@CustOrder", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustOrder", _custOrder);
                if (_sALESREP == null)
                    cm.Parameters.AddWithValue("@SALESREP", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SALESREP", _sALESREP);
                if (_custGroup == null)
                    cm.Parameters.AddWithValue("@CustGroup", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CustGroup", _custGroup);
                if (_conKey1 == null)
                    cm.Parameters.AddWithValue("@ConKey1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey1", _conKey1);
                if (_conKey2 == null)
                    cm.Parameters.AddWithValue("@ConKey2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey2", _conKey2);
                if (_conKey3 == null)
                    cm.Parameters.AddWithValue("@ConKey3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey3", _conKey3);
                if (_conKey4 == null)
                    cm.Parameters.AddWithValue("@ConKey4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey4", _conKey4);
                if (_conKey5 == null)
                    cm.Parameters.AddWithValue("@ConKey5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey5", _conKey5);
                if (_conKey6 == null)
                    cm.Parameters.AddWithValue("@ConKey6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey6", _conKey6);
                if (_conKey7 == null)
                    cm.Parameters.AddWithValue("@ConKey7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey7", _conKey7);
                if (_conKey8 == null)
                    cm.Parameters.AddWithValue("@ConKey8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey8", _conKey8);
                if (_conKey9 == null)
                    cm.Parameters.AddWithValue("@ConKey9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey9", _conKey9);
                if (_conKey10 == null)
                    cm.Parameters.AddWithValue("@ConKey10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConKey10", _conKey10);
                if (_cUSTOMER1 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER1", _cUSTOMER1);
                if (_cUSTOMER2 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER2", _cUSTOMER2);
                if (_cUSTOMER3 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER3", _cUSTOMER3);
                if (_cUSTOMER4 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER4", _cUSTOMER4);
                if (_cUSTOMER5 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER5", _cUSTOMER5);
                if (_cUSTOMER6 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER6", _cUSTOMER6);
                if (_cUSTOMER7 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER7", _cUSTOMER7);
                if (_cUSTOMER8 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER8", _cUSTOMER8);
                if (_cUSTOMER9 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER9", _cUSTOMER9);
                if (_cUSTOMER10 == null)
                    cm.Parameters.AddWithValue("@CUSTOMER10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CUSTOMER10", _cUSTOMER10);
                if (_bUDGET == null)
                    cm.Parameters.AddWithValue("@BUDGET", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGET", _bUDGET);
                if (_bUDGETCABLES == null)
                    cm.Parameters.AddWithValue("@BUDGETCABLES", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETCABLES", _bUDGETCABLES);
                if (_bUDGETLIGHTINGS == null)
                    cm.Parameters.AddWithValue("@BUDGETLIGHTINGS", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETLIGHTINGS", _bUDGETLIGHTINGS);
                if (_bUDGETLAMPS == null)
                    cm.Parameters.AddWithValue("@BUDGETLAMPS", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETLAMPS", _bUDGETLAMPS);
                if (_bUDGETOTHERS == null)
                    cm.Parameters.AddWithValue("@BUDGETOTHERS", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BUDGETOTHERS", _bUDGETOTHERS);
                if (_bossCustomer == null)
                    cm.Parameters.AddWithValue("@BossCustomer", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BossCustomer", _bossCustomer);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                
                // Execute command.
                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.            
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "BHKeyCustomer_Delete";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
                cm.Parameters.AddWithValue("@BudgetYear", criteria._budgetYear);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.            
        }

        #endregion //Data Access - Delete
       

    }
}
