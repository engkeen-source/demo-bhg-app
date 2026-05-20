using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFShipVias : Csla.BusinessListBase<REFShipVias, REFShipVia>
    {

        #region Factory Methods

        internal REFShipVias()
        {
        }

        internal static REFShipVias New()
        {
            REFShipVias obj = new REFShipVias();
            return obj;
        }

        internal static REFShipVias Get()
        {
            REFShipVias obj = new REFShipVias();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _shipViaKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ShipViaKey, int? Option)
            {
                _shipViaKey = ShipViaKey;
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
                cm.CommandText = "REFShipVia_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ShipViaKey", criteria._shipViaKey);

                

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFShipVia.Get(dr));
                }

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using           
        }


        #endregion //Data Access - Fetch
    }
}
