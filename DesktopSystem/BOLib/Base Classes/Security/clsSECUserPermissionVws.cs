
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SECUserPermissionVws : Csla.BusinessListBase<SECUserPermissionVws, SECUserPermissionVw>
    {

        #region Factory Methods

        internal SECUserPermissionVws()
        {
        }

        internal static SECUserPermissionVws New()
        {
            SECUserPermissionVws obj = new SECUserPermissionVws();
            return obj;
        }

        internal static SECUserPermissionVws Get(Criteria criteria)
        {
            SECUserPermissionVws obj = new SECUserPermissionVws();
            obj.Fetch(criteria);
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public Guid? _securityKey = Guid.Empty;
            internal string _permID = string.Empty;
            public int? _option = 0;

            internal Criteria()
            {
            }

            internal Criteria(Guid? SecurityKey, string PermID, int? Option)
            {
                _securityKey = SecurityKey;
                _permID = PermID;
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
            string msgID = MsgID.Common.GetFail;

            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECUserPermissionVw_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@SecurityKey", criteria._securityKey);                    
                cm.Parameters.AddWithValue("@PermID", criteria._permID);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SECUserPermissionVw.Get(dr));
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
