using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class KeyCustomers : Csla.BusinessListBase<KeyCustomers, KeyCustomer> 
    {

        #region Factory Methods

        internal KeyCustomers()
        {
        }

        internal static KeyCustomers New()
        {
            KeyCustomers obj = new KeyCustomers();
            return obj;
        }

        internal static KeyCustomers Get()
        {
            KeyCustomers obj = new KeyCustomers();
            obj.Fetch(new Criteria(0, 0, 0));
            return obj;
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

            internal Criteria(int? GrpKey, int? BudgetYear, int? Option)
            {
                _grpKey = GrpKey;
                _budgetYear = BudgetYear;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        private bool Fetch(Criteria criteria)
        {
            bool retValue = false;

            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria);
            }// End of SqlConnection.

            return retValue;
        }

        private bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "BHKeyCustomer_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
                cm.Parameters.AddWithValue("@BudgetYear", criteria._budgetYear);


                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(KeyCustomer.Get(dr));                    
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using            
        }


        #endregion //Data Access - Fetch
    }
}
