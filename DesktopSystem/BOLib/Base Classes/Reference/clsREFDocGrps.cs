using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFDocGrps : Csla.BusinessListBase<REFDocGrps, REFDocGrp>
    {

        #region Factory Methods

        internal REFDocGrps()
        {
        }

        internal static REFDocGrps New()
        {           
            REFDocGrps obj = new REFDocGrps();           
            return obj;
        }

        internal static REFDocGrps Get()
        {            
            REFDocGrps obj = new REFDocGrps();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _docGrpKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? DocGrpKey, int? Option)
            {
                _docGrpKey = DocGrpKey;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        private bool Fetch(Criteria criteria)
        {
            bool retValue = false;
           
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();

                    retValue = this.Fetch(cn, criteria);
                }// End of SqlConnection.

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        private bool Fetch(SqlConnection cn, Criteria criteria)
        {           
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFDocGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@DocGrpKey", criteria._docGrpKey);

                

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(REFDocGrp.Get(dr));
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
