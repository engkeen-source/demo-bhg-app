

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTCons : Csla.BusinessListBase<MSTCons, MSTCon>
    {

        #region Factory Methods 

        internal MSTCons()
        {
        }

        internal static DataTable NewRecordAccess()
        {
            DataTable obj = null;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                      cn.Open();
                      obj = FetchRecordAccess(cn,0,"zzz","aa",0,0);
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }
            }
            return obj;
            
        }
        internal static DataTable NewRecordAccess(SqlConnection cn)
        {
            DataTable obj = null;
            // Open sql connection. 
            obj = FetchRecordAccess(cn, 0, "zzz", "aa", 0, 0);
            return obj;
        }
        internal static MSTCons New()
        {
            MSTCons obj = new MSTCons();
            return obj;
        }
        internal static MSTCons Get()
        {
            MSTCons obj = new MSTCons();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _conKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ConKey, int? Option)
            {
                _conKey = ConKey;
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
                cm.CommandText = "MSTCon_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTCon.Get(dr));
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
               
            }//using
            
        }

        internal bool FetchForRecordAccess(SqlConnection cn, int? custVendType, string fromID, string toID, int sortingTypeLevel, int sortingTypeGrp)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_GetFilter";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@ConType", custVendType);
                cm.Parameters.AddWithValue("@fromID", fromID);
                cm.Parameters.AddWithValue("@toID", toID);
                cm.Parameters.AddWithValue("@sortingTypeLevel", sortingTypeLevel);
                cm.Parameters.AddWithValue("@sortingTypeGrp", sortingTypeGrp);
                cm.Parameters.AddWithValue("@RetValue", 0);

                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTCon.Get(dr));
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using

        }
        internal static DataTable FetchRecordAccess(SqlConnection cn, int? custVendType, string fromID, string toID, int sortingTypeLevel, int sortingTypeGrp)
        {
            DataTable dtRecordAccess = new DataTable();
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTCon_GetFilter";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@ConType", custVendType);
                cm.Parameters.AddWithValue("@fromID", fromID);
                cm.Parameters.AddWithValue("@toID", toID);
                cm.Parameters.AddWithValue("@sortingTypeLevel", sortingTypeLevel);
                cm.Parameters.AddWithValue("@sortingTypeGrp", sortingTypeGrp);
                cm.Parameters.AddWithValue("@RetValue", 0);

                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(dtRecordAccess);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return dtRecordAccess;
                else
                    return null;
            }//using

        }
        #endregion //Data Access - Fetch

        #region AccessLevelUpdate
        internal static bool AccessLevelUpdate(SqlConnection cn, int? recAccessType, string xmlCons)
        {
            //This SP is a common update for Record Access on Item , Customer/Vendor and Job
            //In furture we should move this to another class as it is confusing to reside in MSTCon Base Class (Not ready)
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECRecAccess_CustomUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RecAccessType", recAccessType);
                cm.Parameters.AddWithValue("@xml", xmlCons);
                cm.Parameters.AddWithValue("@RetValue", 0);

                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using

        }
        #endregion
    }
}