using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSCmpSetUps : Csla.BusinessListBase<SYSCmpSetUps, SYSCmpSetUp>
    {

        #region Factory Methods

        internal SYSCmpSetUps()
        {
        }

        internal static SYSCmpSetUps New()
        {
            
            SYSCmpSetUps obj = new SYSCmpSetUps();
            
            return obj;
        }

        internal static SYSCmpSetUps Get()
        {
            
            SYSCmpSetUps obj = new SYSCmpSetUps();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _taskSeq = null;
            public int? _option = null;

            internal Criteria()
            {
                _option = 0;
                _taskSeq = 0;
            }

            internal Criteria(int? TaskSeq, int? Option)
            {
                _taskSeq = TaskSeq;
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
                cm.CommandText = "SYSCmpSetUp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                                    
                cm.Parameters.AddWithValue("@TaskSeq", criteria._taskSeq);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                                    


                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSCmpSetUp.Get(dr));
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
