using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFBanks : Csla.BusinessListBase<REFBanks, REFBank>
    {

        #region Factory Methods

        internal REFBanks()
        {
        }

        internal static REFBanks New()
        {
            REFBanks obj = new REFBanks();
            return obj;
        }

        internal static REFBanks Get()
        {
            REFBanks obj = new REFBanks();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _bankKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? BankKey, int? Option)
            {
                _bankKey = BankKey;
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

                retValue= this.Fetch(cn, criteria);
            }// End of SqlConnection.

            return retValue;
        }

        private bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBank_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@BankKey", criteria._bankKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFBank.Get(dr));
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
