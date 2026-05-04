

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTEqptDetSubs : Csla.BusinessListBase<MSTEqptDetSubs, MSTEqptDetSub>
    {

        #region Factory Methods

        internal MSTEqptDetSubs()
        {
        }

        internal static MSTEqptDetSubs New()
        {
            
            MSTEqptDetSubs obj = new MSTEqptDetSubs();
            
            return obj;
        }

        internal static MSTEqptDetSubs Get()
        {
            string msgID = "RecordGetFail";
            MSTEqptDetSubs obj = new MSTEqptDetSubs();
            obj.Fetch(new Criteria(0, 0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _eqptKey = null;
            public int? _eqptSubKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? EqptKey, int? EqptSubKey, int? Option)
            {
                _eqptKey = EqptKey;
                _eqptSubKey = EqptSubKey;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
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

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqptDetSub_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@EqptSubKey", criteria._eqptSubKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTEqptDetSub.Get(dr));
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
