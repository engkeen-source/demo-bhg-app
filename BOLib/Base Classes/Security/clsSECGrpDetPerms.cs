using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SECGrpDetPerms : Csla.BusinessListBase<SECGrpDetPerms, SECGrpDetPerm>
    {

        #region Factory Methods

        internal SECGrpDetPerms()
        {
        }

        internal static SECGrpDetPerms New()
        {
            SECGrpDetPerms obj = new SECGrpDetPerms();
            return obj;
        }

        internal static SECGrpDetPerms Get()
        {
            SECGrpDetPerms obj = new SECGrpDetPerms();
            obj.Fetch(new Criteria(0, string.Empty, 0));
            return obj;
        }
        
        internal static SECGrpDetPerms Get( int? grpKey)
        {
            SECGrpDetPerms obj = new SECGrpDetPerms();
            obj.Fetch(new Criteria(grpKey, string.Empty, 0));
            return obj;
        }        

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _grpKey = null;
            internal string _permID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? GrpKey, string PermID, int? Option)
            {
                _grpKey = GrpKey;
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
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECGrpDetPerm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@GrpKey", criteria._grpKey);
                cm.Parameters.AddWithValue("@PermID", criteria._permID);


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SECGrpDetPerm.Get(dr));
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
                        this.Add(SECGrpDetPerm.Get(dr));
                }

                return true;
        }

        #endregion //Data Access - Fetch
    }
}