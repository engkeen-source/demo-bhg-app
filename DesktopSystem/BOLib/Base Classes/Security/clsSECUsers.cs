using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SECUsers : Csla.BusinessListBase<SECUsers, SECUser>
    {

        #region Factory Methods

        internal SECUsers()
        {
        }

        internal static SECUsers New()
        {
            SECUsers obj = new SECUsers();
            return obj;
        }

        internal static SECUsers Get()
        {
            SECUsers obj = new SECUsers();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _userKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? UserKey, int? Option)
            {
                _userKey = UserKey;
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
                cm.CommandText = "SECUser_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
                
                cm.Parameters.AddWithValue("@UserID", DBNull.Value);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SECUser.Get(dr));
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