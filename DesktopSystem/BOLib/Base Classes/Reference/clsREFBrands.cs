using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFBrands : Csla.BusinessListBase<REFBrands, REFBrand>
    {

        #region Factory Methods

        internal REFBrands()
        {
        }

        internal static REFBrands New()
        {
           
            REFBrands obj = new REFBrands();
            
            return obj;
        }

        internal static REFBrands Get()
        {
            
            REFBrands obj = new REFBrands();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        internal static REFBrands Get(string BrandIDFrom,string BrandIDTo)
        {

            REFBrands obj = new REFBrands();
            obj.Fetch(new Criteria(BrandIDFrom, BrandIDTo, 2));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _brandKey = null;
            public int? _option = null;
            public string _BrandIDFrom = "N'%'";
            public string _BrandIDTo = "N'%'";

            internal Criteria()
            {
            }

            internal Criteria(int? BrandKey, int? Option)
            {
                _brandKey = BrandKey;
                _option = Option;
            }

            internal Criteria(string BrandIDFrom,string BrandIDTo, int? Option)
            {
                _BrandIDFrom = BrandIDFrom+"%";
                _BrandIDTo =  BrandIDTo + "%";
                _brandKey = -9999;
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
                cm.CommandText = "REFBrand_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@BrandIDFrom", criteria._BrandIDFrom);
                cm.Parameters.AddWithValue("@BrandIDTo", criteria._BrandIDTo);
                cm.Parameters.AddWithValue("@BrandKey", criteria._brandKey);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFBrand.Get(dr));
                }

                

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }//using
            
            return retValue;
        }


        #endregion //Data Access - Fetch
    }
}
