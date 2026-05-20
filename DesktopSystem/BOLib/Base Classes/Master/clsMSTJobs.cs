
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTJobs : Csla.BusinessListBase<MSTJobs, MSTJob>
    {

        #region Factory Methods

        internal MSTJobs()
        {
        }

        internal static MSTJobs New()
        {
            
            MSTJobs obj = new MSTJobs();
            
            return obj;
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
                    obj = FetchRecordAccess(cn, 10, string.Empty, string.Empty, 0, 0);
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }
            }
            return obj;

        }
        internal static DataTable NewRecordAccess(SqlConnection cn)
        {
            DataTable obj = null;
            // Open sql connection. 
            obj = FetchRecordAccess(cn, 10, string.Empty, string.Empty, 0, 0);
            return obj;
        }
        internal static MSTJobs Get()
        {
           
            MSTJobs obj = new MSTJobs();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? JobKey, int? Option)
            {
                _jobKey = JobKey;
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
                cm.CommandText = "MSTJob_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTJob.Get(dr));
                }

               
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;            
            }//using
            
            return retValue;
        }


        #endregion //Data Access - Fetch

        internal bool FetchForRecordAccess(SqlConnection cn, int? jobGrpKey, string fromID, string toID, int sortingTypeLevel, int sortingTypeGrp)
        {

            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJob_GetFilter";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@JobGrpKey", jobGrpKey);
                cm.Parameters.AddWithValue("@fromID", fromID);
                cm.Parameters.AddWithValue("@toID", toID);
                cm.Parameters.AddWithValue("@sortingTypeLevel", sortingTypeLevel);
                cm.Parameters.AddWithValue("@sortingTypeGrp", sortingTypeGrp);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTJob.Get(dr));
                }


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }//using

        }
        internal static DataTable FetchRecordAccess(SqlConnection cn, int? jobGrpKey, string fromID, string toID, int sortingTypeLevel, int sortingTypeGrp)
        {
            DataTable dtRecordAccess=new DataTable();
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJob_GetFilter";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@JobGrpKey", jobGrpKey);
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
    }
}