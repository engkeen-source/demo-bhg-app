using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFOverHeads : Csla.BusinessListBase<REFOverHeads, REFOverHead>
    {

        #region Factory Methods

        internal REFOverHeads()
        {
        }

        internal static REFOverHeads New()
        {           
            REFOverHeads obj = new REFOverHeads();           
            return obj;
        }

        internal static REFOverHeads Get()
        {            
            REFOverHeads obj = new REFOverHeads();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _overHeadKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? OverHeadKey, int? Option)
            {
                _overHeadKey = OverHeadKey;
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
                cm.CommandText = "REFOverHead_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                
                cm.Parameters.AddWithValue("@OverHeadKey", criteria._overHeadKey);                  

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFOverHead.Get(dr));
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
