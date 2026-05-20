using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFIndustrys : Csla.BusinessListBase<REFIndustrys, REFIndustry>
    {

        #region Factory Methods

        internal REFIndustrys()
        {
        }

        internal static REFIndustrys New()
        {          
            REFIndustrys obj = new REFIndustrys();           
            return obj;
        }

        internal static REFIndustrys Get()
        {           
            REFIndustrys obj = new REFIndustrys();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _industryKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? IndustryKey, int? Option)
            {
                _industryKey = IndustryKey;
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
                cm.CommandText = "REFIndustry_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
               
                cm.Parameters.AddWithValue("@IndustryKey", criteria._industryKey);

                

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFIndustry.Get(dr));
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
