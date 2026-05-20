using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFCats : Csla.BusinessListBase<REFCats, REFCat>
    {

        #region Factory Methods

        internal REFCats()
        {
        }

        internal static REFCats New()
        {
           
            REFCats obj = new REFCats();            
            return obj;
        }

        internal static REFCats Get()
        {            
            REFCats obj = new REFCats();
            obj.Fetch(new Criteria(0, 0, 0));
            return obj;
        }

        internal static REFCats Get(Int16? catNum)
        {
            
            REFCats obj = new REFCats();
            obj.Fetch(new Criteria(0, catNum, 1));
            return obj;
        }

        internal static REFCats Get(string CatIDFrom, string CatIDTo)
        {

            REFCats obj = new REFCats();
            obj.Fetch(new Criteria(CatIDFrom, CatIDTo, 3));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _catKey = null;
            public Int16? _catNum = null;
            public int? _option = null;
            public string _CatIDFrom = "N'%'";
            public string _CatIDTo = "N'%'";

            internal Criteria()
            {
            }

            internal Criteria(int? CatKey, Int16? CatNum, int? Option)
            {
                _catKey = CatKey;
                _catNum = CatNum;
                _option = Option;
            }

            internal Criteria(string CatIDFrom, string CatIDTo, int? Option)
            {
                _CatIDFrom = CatIDFrom+"%";
                _CatIDTo = CatIDTo + "%";
                _catKey = -9999;
                _catNum = -9999;
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
                cm.CommandText = "REFCat_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CatKey", criteria._catKey);
                cm.Parameters.AddWithValue("@CatNum", criteria._catNum);
                cm.Parameters.AddWithValue("@CatIDForm", criteria._CatIDFrom);
                cm.Parameters.AddWithValue("@CatIDTo", criteria._CatIDTo);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFCat.Get(dr));
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
}
