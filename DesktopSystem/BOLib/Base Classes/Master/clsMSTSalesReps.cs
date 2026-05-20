
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTSalesReps : Csla.BusinessListBase<MSTSalesReps, MSTSalesRep>
    {

        #region Factory Methods

        internal MSTSalesReps()
        {
        }

        internal static MSTSalesReps New()
        {
            
            MSTSalesReps obj = new MSTSalesReps();
            
            return obj;
        }

        internal static MSTSalesReps Get()
        {
           
            MSTSalesReps obj = new MSTSalesReps();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _emKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? EmKey, int? Option)
            {
                _emKey = EmKey;
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
                cm.CommandText = "MSTSalesRep_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@EmKey", criteria._emKey);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTSalesRep.Get(dr));
                }
                return true;
            }//using
            
        }


        #endregion //Data Access - Fetch
    }
}

