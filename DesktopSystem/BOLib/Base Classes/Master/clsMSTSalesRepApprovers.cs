using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    public class MSTSalesRepApprovers : DataTable
    {
        #region Factory Methods

        public MSTSalesRepApprovers()
        {
            this.Fetch(new Criteria(0, 1));
        }

        public MSTSalesRepApprovers(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        public static MSTSalesRepApprovers Get(int? currKey)
        {
            MSTSalesRepApprovers obj = new MSTSalesRepApprovers();
            obj.Fetch(new Criteria(currKey, 1));
            return obj;
        }


        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? __saleRepKey = null;
            public int? _approverKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? SaleRepKey, int? Option)
            {
                __saleRepKey = SaleRepKey;
                _approverKey = 0;
                _option = Option;
            }
            internal Criteria(int? SaleRepKey, int? ApproverKey, int? Option)
            {
                __saleRepKey = SaleRepKey;
                _approverKey = ApproverKey;
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
        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepApprovers_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@EMKey", criteria.__saleRepKey);
                


                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
             
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


            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, headerKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

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
                    cm.CommandText = "MSTSalesRepApprover_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);

                    cm.Parameters.AddWithValue("@SaleRepKey", headerKey);
                    cm.Parameters.AddWithValue("@ApproverKey", dr["ApproverKey"]);
                    cm.Parameters.AddWithValue("@SaleLimit", dr["SaleLimit"].ToString() == "" ? 0 : dr["SaleLimit"]);
                    cm.Parameters.AddWithValue("@ProfitMarginLimit", dr["ProfitMarginLimit"].ToString() == "" ? 0 : dr["ProfitMarginLimit"]);               

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
          

                    // Execute command.
                    cm.ExecuteNonQuery();                    
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                }// Already close and dispose sql command.
            }

            return retValue;           

        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update(int? headerKey)
        {
            bool retValue = false;


            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue = this.Update(cn, headerKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Update(SqlConnection cn,int? headerKey)
        {

            bool retValue = false;
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTSalesRepApprover_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    
                    cm.Parameters.AddWithValue("@SaleRepKey", headerKey);
                    cm.Parameters.AddWithValue("@ApproverKey", dr["ApproverKey"]);
                    cm.Parameters.AddWithValue("@SaleLimit", dr["SaleLimit"].ToString() == "" ? 0 : dr["SaleLimit"]);
                    cm.Parameters.AddWithValue("@ProfitMarginLimit", dr["ProfitMarginLimit"].ToString() == "" ? 0 : dr["ProfitMarginLimit"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();


                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }
            }// Already close and dispose sql command.

            return retValue;

            
        }
        #endregion //Data Access - Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call delete method.
                    retValue = this.Delete(cn, criteria);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesApprover_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@EMKey", criteria.__saleRepKey);
                cm.Parameters.AddWithValue("@ApproverKey", criteria._approverKey);
                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }// Already close and dispose sql connection.

        }

        #endregion //Data Access - Delete


    }
}
