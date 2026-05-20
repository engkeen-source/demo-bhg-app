

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using BOLib;

[Serializable()]
public class MSTItmHiss : Csla.BusinessListBase<MSTItmHiss, MSTItmHis>
{

    #region Factory Methods

    internal MSTItmHiss()
    {
    }

    internal static MSTItmHiss New()
    {      
        MSTItmHiss obj = new MSTItmHiss();       
        return obj;
    }

    internal static MSTItmHiss Get()
    {        
        MSTItmHiss obj = new MSTItmHiss();
        obj.Fetch(new Criteria(0, 0, 0, 0));
        return obj;
    }

    #endregion //Factory Methods

    #region Criteria

    [Serializable()]
    internal class Criteria
    {
        public int? _itmKey = null;
        public int? _locKey = null;
        public int? _period = null;
        public int? _option = null;

        internal Criteria()
        {
        }

        internal Criteria(int? ItmKey, int? LocKey, int? Period, int? Option)
        {
            _itmKey = ItmKey;
            _locKey = LocKey;
            _period = Period;
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
            cm.CommandText = "MSTItmHis_Get";

            cm.Parameters.AddWithValue("@Option", criteria._option);
 
            cm.Parameters.AddWithValue("@RetValue", 0);
            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
            cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
            cm.Parameters.AddWithValue("@Period", criteria._period);

            using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
            {
                while (dr.Read())
                    this.Add(MSTItmHis.Get(dr));
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


