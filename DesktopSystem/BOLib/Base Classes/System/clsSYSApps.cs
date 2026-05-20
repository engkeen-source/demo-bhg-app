using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSApps : Csla.BusinessListBase<SYSApps, SYSApp>
    {

        #region Factory Methods

        internal SYSApps()
        {
        }

        internal static SYSApps New()
        {
            
            SYSApps obj = new SYSApps();
            
            return obj;
        }

        internal static SYSApps Get()
        {
            
            SYSApps obj = new SYSApps();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _appKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? AppKey, int? Option)
            {
                _appKey = AppKey;
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
                cm.CommandText = "SYSApp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@AppKey", criteria._appKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSApp.Get(dr));
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
