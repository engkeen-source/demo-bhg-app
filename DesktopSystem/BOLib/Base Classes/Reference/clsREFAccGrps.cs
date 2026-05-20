using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class REFAccGrps : Csla.BusinessListBase<REFAccGrps, REFAccGrp>
    {

        #region Factory Methods

        internal REFAccGrps()
        {
        }

        internal static REFAccGrps New()
        {            
            REFAccGrps obj = new REFAccGrps();          
            return obj;
        }

        public static REFAccGrps Get()
        {
            
            REFAccGrps obj = new REFAccGrps();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _accGrpKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? AccGrpKey, int? Option)
            {
                _accGrpKey = AccGrpKey;
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
            
            bool retValue = false;

            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFAccGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@AccGrpKey", criteria._accGrpKey);

                

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFAccGrp.Get(dr));
                }

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    throw new TAException(MsgID.Common.GetFail);
                }                                                       
            }//using            
            return retValue;
        }
        #endregion //Data Access - Fetch
    }
}