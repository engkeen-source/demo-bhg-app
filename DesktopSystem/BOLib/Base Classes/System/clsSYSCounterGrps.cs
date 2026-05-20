using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSCounterGrps : DataTable
    {

        #region Factory Methods

        public SYSCounterGrps()
        {            
            this.Fetch(new Criteria(0,1));          
        }

        public SYSCounterGrps(SqlConnection cn)
        {
            
            this.Fetch(cn, new Criteria(0, 1));
        }

        public static SYSCounterGrps Get(int? currKey)
        {
            SYSCounterGrps obj = new SYSCounterGrps();
            obj.Fetch(new Criteria(currKey, 1));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _codeKey = null;
            public int? _counterGrp = null;
            public decimal? _seq = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? CodeKey, int? Option)
            {
                _codeKey = CodeKey;               
                _option = Option;
            }
            internal Criteria(int? CodeKey, int? CounterGrp, int? Option)
            {
                _codeKey = CodeKey;
                _counterGrp = CounterGrp;
                _option = Option;
            }
            internal Criteria(int? CodeKey, int? CounterGrp, decimal? Seq, int? Option)
            {
                _codeKey = CodeKey;
                _counterGrp = CounterGrp;
                _seq = Seq;
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
                cm.CommandText = "SYSCounterGrp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);

                if (criteria._counterGrp == null)
                    cm.Parameters.AddWithValue("@CounterGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrp", criteria._counterGrp);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }//using            
        }


        #endregion //Data Access - Fetch


        #region Data Access - Insert

        internal bool Insert(int? headerKey)
        {
            bool retValue = false;

            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, headerKey);
                }
                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Insert(SqlConnection cn, int? headerKey)
        {
            bool retValue = false;

            if (this.Rows.Count == 0)
            {
                return true;
            }

            foreach (DataRow dr in this.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    retValue = true;
                    continue;
                }
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSCounterGrp_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@CodeKey", headerKey);
                    cm.Parameters.AddWithValue("@CounterGrp", dr["CounterGrp"]);
                    cm.Parameters.AddWithValue("@InitialNumber", dr["InitialNumber"]);
                    cm.Parameters.AddWithValue("@Reset", dr["Reset"]);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                    {
                        retValue = false;
                    }
                }// Already close and dispose sql command.
            }

            return retValue;
        }
        #endregion Insert
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
                    cm.CommandText = "SYSCounterGrp_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@CodeKey", dr["CodeKey"]);
                    cm.Parameters.AddWithValue("@CounterGrp", dr["CounterGrp"]);
                    cm.Parameters.AddWithValue("@InitialNumber", dr["InitialNumber"]);
                    cm.Parameters.AddWithValue("@Reset", dr["Reset"]);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                    {
                        retValue = false;
                    }
                }
            }// Already close and dispose sql command.
            return retValue;
        }
        #endregion Update
        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;

            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria);
                }
                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSCounterGrp_Delete";

                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);                

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.            
        }
        #endregion Delete
    }
}
