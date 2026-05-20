using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSAppDetItms : Csla.BusinessListBase<SYSAppDetItms, SYSAppDetItm>
    {

        #region Factory Methods

        internal SYSAppDetItms()
        {
        }

        internal static SYSAppDetItms New()
        {
            
            SYSAppDetItms obj = new SYSAppDetItms();
            
            return obj;
        }

        internal static SYSAppDetItms Get()
        {
            
            SYSAppDetItms obj = new SYSAppDetItms();
            obj.Fetch(new Criteria(0, string.Empty, string.Empty, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _appKey = null;
            internal string _appObjSub = string.Empty;
            internal string _appObjItm = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? AppKey, string AppObjSub, string AppObjItm, int? Option)
            {
                _appKey = AppKey;
                _appObjSub = AppObjSub;
                _appObjItm = AppObjItm;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        private bool? Fetch(Criteria criteria)
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
                cm.CommandText = "SYSAppDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@AppKey", criteria._appKey);
                cm.Parameters.AddWithValue("@AppObjSub", criteria._appObjSub);
                cm.Parameters.AddWithValue("@AppObjItm", criteria._appObjItm);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSAppDetItm.Get(dr));
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
