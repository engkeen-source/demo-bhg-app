

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using BOLib;
using TAUtil;

[Serializable()]
public class MSTItms : Csla.BusinessListBase<MSTItms, MSTItm>
{

    #region Factory Methods

    internal MSTItms()
    {
    }

    internal static MSTItms New()
    {
       
        MSTItms obj = new MSTItms();      
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
                obj = FetchRecordAccess(cn,0, "zzz", "aa", 0, 0);
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
    public static MSTItms Get()
    {
        MSTItms obj = new MSTItms();
        obj.Fetch(new Criteria(0, 0));
        return obj;
    }
    internal static MSTItms Get(int? MasterItmKey)
    {
        MSTItms obj = new MSTItms();
        obj.Fetch(new Criteria(MasterItmKey, 6));
        return obj;
    }

    internal static MSTItms Get(string MSTItmIDFrom, string MSTItmIDTo)
    {
        MSTItms obj = new MSTItms();
        obj.Fetch(new Criteria(MSTItmIDFrom, MSTItmIDTo, 7));
        return obj;
    }
    #endregion //Factory Methods

    #region Criteria

    [Serializable()]
    internal class Criteria
    {
        public int? _itmKey = null;      
        public int? _option = null;
        public string _ItmIDFrom = "";
        public string _tmIDTo = "";

        internal Criteria()
        {
        }

        internal Criteria(int? ItmKey, int? Option)
        {
            _itmKey = ItmKey;
            _option = Option;
        }

        internal Criteria(string ItmIDFrom, string ItmIDTo, int? Option)
        {
            _ItmIDFrom = ItmIDFrom;
            _tmIDTo = ItmIDTo;
            _itmKey = -9999;
            _option = Option;
        }
       
    }

    #endregion //Criteria

    #region Data Access - Fetch

    internal bool Fetch(Criteria criteria)
    {
        bool retValue = false;        
        try
        {  
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();

                retValue = this.Fetch(cn, criteria);
            }// End of SqlConnection.
 
        }
        catch (TAException tex)
        {
            throw tex;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        return retValue;
    }

    internal bool Fetch(SqlConnection cn, Criteria criteria)
    {
        bool retValue = false;
        
        try
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);             

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@ItmID", string.Empty);
                cm.Parameters.AddWithValue("@ItmIDFrom", criteria._ItmIDFrom);
                cm.Parameters.AddWithValue("@ItmIDTo", criteria._tmIDTo);
                cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTItm.Get(dr));
                }

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    throw new TAException(MsgID.Common.GetFail);
                }            

            }//using
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        return retValue;
    }

    internal bool FetchForRecordAccess(SqlConnection cn, int? itmType, string fromID, string toID, int sortingTypeLevel, int sortingTypeGrp)
    {

        bool retValue = false;
        try
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_GetFilter";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@ItmType", itmType);
                cm.Parameters.AddWithValue("@fromID", fromID);
                cm.Parameters.AddWithValue("@toID", toID);
                cm.Parameters.AddWithValue("@sortingTypeLevel", sortingTypeLevel);
                cm.Parameters.AddWithValue("@sortingTypeGrp", sortingTypeGrp);
                cm.Parameters.AddWithValue("@RetValue", 0);

                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTItm.Get(dr));
                }

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    throw new TAException(MsgID.Common.GetFail);
                }
            }//using
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retValue;
    }
    internal static DataTable FetchRecordAccess(SqlConnection cn, int? itmType, string fromID, string toID, int sortingTypeLevel, int sortingTypeGrp)
    {

        try
        {
            DataTable dtRecordAccess = new DataTable();
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItm_GetFilter";

                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@ItmType", itmType);
                cm.Parameters.AddWithValue("@fromID", fromID);
                cm.Parameters.AddWithValue("@toID", toID);
                cm.Parameters.AddWithValue("@sortingTypeLevel", sortingTypeLevel);
                cm.Parameters.AddWithValue("@sortingTypeGrp", sortingTypeGrp);
                cm.Parameters.AddWithValue("@RetValue", 0);

                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(dtRecordAccess);

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return dtRecordAccess;
                }
                else
                {
                    return null;
                }
            }//using
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion //Data Access - Fetch
}


