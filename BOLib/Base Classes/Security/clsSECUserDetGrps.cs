using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SECUserDetGrps : Csla.BusinessListBase<SECUserDetGrps, SECUserDetGrp>
    {

        #region Factory Methods

        internal SECUserDetGrps()
        {
        }

        internal static SECUserDetGrps New()
        {
            SECUserDetGrps obj = new SECUserDetGrps();
            return obj;
        }

        internal static SECUserDetGrps Get()
        {
            SECUserDetGrps obj = new SECUserDetGrps();
            obj.Fetch(new Criteria(0, 0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _userKey = null;
            public int? _grpKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? UserKey, int? GrpKey, int? Option)
            {
                _userKey = UserKey;
                _grpKey = GrpKey;
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
                cm.CommandText = "SECUserDetGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SECUserDetGrp.Get(dr));
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using            
        }
        internal bool Fetch(DataTable table)
        {
            

                using (SafeDataReader dr = new SafeDataReader(table.CreateDataReader()))
                {
                    while (dr.Read())
                        this.Add(SECUserDetGrp.Get(dr));
                }

                
                    return true ;
            
        }

        #endregion //Data Access - Fetch
    }
}
