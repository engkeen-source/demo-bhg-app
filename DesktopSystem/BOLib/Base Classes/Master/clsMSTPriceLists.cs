

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTPriceLists : Csla.BusinessListBase<MSTPriceLists, MSTPriceList>
    {

        #region Factory Methods

        internal MSTPriceLists()
        {
        }

        internal static MSTPriceLists New()
        {
           
            MSTPriceLists obj = new MSTPriceLists();
            
            return obj;
        }

        internal static MSTPriceLists Get()
        {
            
            MSTPriceLists obj = new MSTPriceLists();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _priceKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? PriceKey, int? Option)
            {
                _priceKey = PriceKey;
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
                cm.CommandText = "MSTPriceList_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                    

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@PriceKey", criteria._priceKey);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTPriceList.Get(dr));
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

