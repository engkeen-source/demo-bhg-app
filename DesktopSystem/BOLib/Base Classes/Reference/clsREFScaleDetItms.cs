using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFScaleDetItms : Csla.BusinessListBase<REFScaleDetItms, REFScaleDetItm>
    {

        #region Factory Methods

        internal REFScaleDetItms()
        {
        }

        internal static REFScaleDetItms New()
        {
            REFScaleDetItms obj = new REFScaleDetItms();
            return obj;
        }

        internal static REFScaleDetItms Get(int? scaleKey)
        {
            REFScaleDetItms obj = new REFScaleDetItms();
            obj.Fetch(new Criteria(scaleKey, 0, 1));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _scaleKey = null;
            public short? _sizeNum = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ScaleKey, short? SizeNum, int? Option)
            {
                _scaleKey = ScaleKey;
                _sizeNum = SizeNum;
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
                cm.CommandText = "REFScaleDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ScaleKey", criteria._scaleKey);
                cm.Parameters.AddWithValue("@SizeNum", criteria._sizeNum);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFScaleDetItm.Get(dr));
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
