

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using BOLib;

[Serializable()]
public class MSTItmDetPrices : Csla.BusinessListBase<MSTItmDetPrices, MSTItmDetPrice>
{

    #region Factory Methods

    internal MSTItmDetPrices()
    {
    }

    internal static MSTItmDetPrices New()
    {
        MSTItmDetPrices obj = new MSTItmDetPrices();
        return obj;
    }

    internal static MSTItmDetPrices Get()
    {
        MSTItmDetPrices obj = new MSTItmDetPrices();
        obj.Fetch(new Criteria(0, 0));
        return obj;
    }

    #endregion //Factory Methods

    #region Criteria

    [Serializable()]
    internal class Criteria
    {
        public int? _itmKey = null;
        public int? _option = null;

        internal Criteria()
        {
        }

        internal Criteria(int? ItmKey, int? Option)
        {
            _itmKey = ItmKey;
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
            cm.CommandText = "MSTItmDetPrice_Get";

            cm.Parameters.AddWithValue("@Option", criteria._option);

            cm.Parameters.AddWithValue("@RetValue", 0);
            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);

            using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
            {
                while (dr.Read())
                    this.Add(MSTItmDetPrice.Get(dr));
            }
            // Check Return Value -- Changed By Richard
            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                return true;
            else
                return false;
        }//using

    }

    #endregion //Data Access - Fetch
}


