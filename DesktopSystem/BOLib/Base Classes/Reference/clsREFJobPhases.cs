using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFJobPhases : Csla.BusinessListBase<REFJobPhases, REFJobPhase>
    {

        #region Factory Methods

        internal REFJobPhases()
        {
        }

        internal static REFJobPhases New()
        {            
            REFJobPhases obj = new REFJobPhases();            
            return obj;
        }

        internal static REFJobPhases Get()
        {           
            REFJobPhases obj = new REFJobPhases();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobPhaseKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? JobPhaseKey, int? Option)
            {
                _jobPhaseKey = JobPhaseKey;
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
                cm.CommandText = "REFJobPhase_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
             
                cm.Parameters.AddWithValue("@JobPhaseKey", criteria._jobPhaseKey);
                

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFJobPhase.Get(dr));
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
