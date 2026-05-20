using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSCodes : DataTable
    {

        #region Factory Methods

        internal SYSCodes()
        {
        }

        internal static SYSCodes New()
        {
            SYSCodes obj = new SYSCodes();
            return obj;
        }

        internal static SYSCodes Get()
        {
            SYSCodes obj = new SYSCodes();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        //public static SYSCodes Get(int _option)
        //{
        //    SYSCodes obj = new SYSCodes();
        //    obj.Fetch(new Criteria(0, 2));
        //    return obj;
        //}

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            //public GEnum.SystemCode? _codeKey = null;
            public int? _codeKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CodeKey, int? Option)
            {
                _codeKey = CodeKey;
                _option = Option;
            }
            
            internal Criteria(int? Option)
            {
                _codeKey = 0;
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
                cm.CommandText = "SYSCode_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;


                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using
        }
        
        #endregion //Data Access - Fetch

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;

            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn);
                }
                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;

            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSCode_Update";                    
                    cm.Parameters.AddWithValue("@CodeKey", dr["CodeKey"]);
                    cm.Parameters.AddWithValue("@CodeID", dr["CodeID"]);                                        
                    cm.ExecuteNonQuery();

                    retValue = true;
                }
            }// Already close and dispose sql command.
            return retValue;
        }
        #endregion Update

    }
}
