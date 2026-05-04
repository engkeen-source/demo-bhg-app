

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using BOLib;

[Serializable()]
public class MSTItmStockCounts : Csla.BusinessListBase<MSTItmStockCounts, MSTItmStockCount>
{

    #region Factory Methods

    internal MSTItmStockCounts()
    {
    }

    internal static MSTItmStockCounts New()
    {       
        MSTItmStockCounts obj = new MSTItmStockCounts();       
        return obj;
    }

    internal static MSTItmStockCounts Get()
    {        
        MSTItmStockCounts obj = new MSTItmStockCounts();
        obj.Fetch(new Criteria(0, 0, 0, 0, 0));
        return obj;
    }

    #endregion //Factory Methods

    #region Criteria

    [Serializable()]
    internal class Criteria
    {
        public int? _itmKey = null;
        public int? _locKey = null;
        public int? _batchKey = null;
        public int? _serialKey = null;
        public int? _option = null;

        internal Criteria()
        {
        }

        internal Criteria(int? ItmKey, int? LocKey, int? BatchKey, int? SerialKey, int? Option)
        {
            _itmKey = ItmKey;
            _locKey = LocKey;
            _batchKey = BatchKey;
            _serialKey = SerialKey;
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
            cm.CommandText = "MSTItmStockCount_Get";

            cm.Parameters.AddWithValue("@Option", criteria._option);
         
            cm.Parameters.AddWithValue("@RetValue", 0);
            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            cm.Parameters.AddWithValue("@ItmKey", criteria._itmKey);
            cm.Parameters.AddWithValue("@LocKey", criteria._locKey);
            cm.Parameters.AddWithValue("@BatchKey", criteria._batchKey);
            cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);

            using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
            {
                while (dr.Read())
                    this.Add(MSTItmStockCount.Get(dr));
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


