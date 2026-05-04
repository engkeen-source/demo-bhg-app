using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFJobGrps : Csla.BusinessListBase<REFJobGrps, REFJobGrp>
    {

        #region Factory Methods

        internal REFJobGrps()
        {
        }

        internal static REFJobGrps New()
        {           
            REFJobGrps obj = new REFJobGrps();           
            return obj;
        }

        internal static REFJobGrps Get()
        {            
            REFJobGrps obj = new REFJobGrps();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobGrpKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? JobGrpKey, int? Option)
            {
                _jobGrpKey = JobGrpKey;
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
                cm.CommandText = "REFJobGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);               
                cm.Parameters.AddWithValue("@JobGrpKey", criteria._jobGrpKey);                   

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFJobGrp.Get(dr));
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
