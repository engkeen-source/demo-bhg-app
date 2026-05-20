using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSLogBatchs : Csla.BusinessListBase<SYSLogBatchs, SYSLogBatch>
    {

        #region Factory Methods

        internal SYSLogBatchs()
        {
        }

        internal static SYSLogBatchs New()
        {
            
            SYSLogBatchs obj = new SYSLogBatchs();
            
            return obj;
        }

        internal static SYSLogBatchs Get()
        {
            
            SYSLogBatchs obj = new SYSLogBatchs();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _uid = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? Uid, int? Option)
            {
                _uid = Uid;
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
                cm.CommandText = "SYSLogBatch_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@Uid", criteria._uid);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSLogBatch.Get(dr));
                }

                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }//using
        }


        #endregion //Data Access - Fetch
    }
}
