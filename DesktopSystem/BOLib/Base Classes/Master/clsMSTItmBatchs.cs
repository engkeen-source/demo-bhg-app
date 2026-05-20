

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTItmBatchs : DataTable
    {

        #region Factory Methods

        public MSTItmBatchs()
        {

        }

        public MSTItmBatchs(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        public static MSTItmBatchs Get(int? headerKey)
        {
            MSTItmBatchs obj = new MSTItmBatchs();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }
        public static MSTItmBatchs GetWtihBalance(int? headerKey)//Call from Batch Entry
        {
            MSTItmBatchs obj = new MSTItmBatchs();
            obj.Fetch(new Criteria(headerKey, 3));
            return obj;
        }

        public static MSTItmBatchs New()
        {

            MSTItmBatchs obj = new MSTItmBatchs();
            return obj;
        }
        public static MSTItmBatchs New(SqlConnection cn)
        {

            MSTItmBatchs obj = new MSTItmBatchs();
            obj.Fetch(cn, new Criteria(0, 1));

            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _BatchItmKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;
            }

            internal Criteria(int? BatchKey, int? BatchItmKey, int? Option)
            {
                _headerKey = BatchKey;
                _BatchItmKey = BatchItmKey;
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
                cm.CommandText = "MSTItmBatch_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey);

                cm.Parameters.AddWithValue("@BatchKey", 0);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
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
                    cm.CommandText = "MSTItmBatch_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 2);
                    cm.Parameters.AddWithValue("@BatchItmKey", headerKey);
                    cm.Parameters.AddWithValue("@NewBatchKey", 0);
                    cm.Parameters.AddWithValue("@BatchID", dr["BatchID"]);
                    cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"].ToString() == "" ? 0 : dr["BatchKey"]);
                    cm.Parameters.AddWithValue("@BatchExpDate", dr["BatchExpDate"]);
                    cm.Parameters.AddWithValue("@BatchMfgDate", dr["BatchMfgDate"]);
                    cm.Parameters.AddWithValue("@BatchQty", dr["BatchQty"].ToString() == "" ? 0 : dr["BatchQty"]);
                    cm.Parameters.AddWithValue("@BatchQtyBal", dr["BatchQtyBal"].ToString() == "" ? 0 : dr["BatchQtyBal"]);
                    cm.Parameters.AddWithValue("@BatchCost", dr["BatchCost"].ToString() == "" ? 0 : dr["BatchCost"]);
                    cm.Parameters.AddWithValue("@BatchStatus", dr["BatchStatus"].ToString() == "" ? 0 : dr["BatchStatus"]);
                    cm.Parameters.AddWithValue("@LogDC", dr["LogDC"].ToString() == "" ? 0 : dr["LogDC"]);
                    cm.Parameters.AddWithValue("@LogDK", dr["LogDK"].ToString() == "" ? 0 : dr["LogDK"]);
                    cm.Parameters.AddWithValue("@LogDItm", dr["LogDItm"].ToString() == "" ? 0 : dr["LogDItm"]);
                    cm.Parameters.AddWithValue("@LogDocDate", dr["LogDocDate"]);
                    cm.Parameters.AddWithValue("@PurgeKeep", dr["PurgeKeep"].ToString() == "" ? 0 : dr["PurgeKeep"]);
                    cm.Parameters.AddWithValue("@PurgeData", dr["PurgeData"].ToString() == "" ? 0 : dr["PurgeData"]);
                    //cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    //cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    //cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    //cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);


                    cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.InputOutput;
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();


                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                        retValue = false;
                }// Already close and dispose sql command.
            }
            return retValue;
        }
        internal bool Insert(SqlConnection cn, int? headerKey, DataRow dr, out int? NewBatchKey, int option)
        {
            NewBatchKey = 0;

            if (this.Rows.Count == 0)
            {
                return true;
            }
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatch_AddUpdate";

                cm.Parameters.AddWithValue("@Option", option);
                cm.Parameters.AddWithValue("@BatchItmKey", headerKey);
                cm.Parameters.AddWithValue("@NewBatchKey", 0);
                cm.Parameters.AddWithValue("@BatchID", dr["BatchID"]);
                cm.Parameters.AddWithValue("@BatchKey", 0);
                cm.Parameters.AddWithValue("@BatchExpDate", dr["BatchExpDate"]);
                cm.Parameters.AddWithValue("@BatchMfgDate", dr["BatchMfgDate"]);
                cm.Parameters.AddWithValue("@BatchQty", dr["BatchQty"].ToString() == "" ? 0 : dr["BatchQty"]);
                cm.Parameters.AddWithValue("@BatchQtyBal", dr["BatchQtyBal"].ToString() == "" ? 0 : dr["BatchQtyBal"]);
                cm.Parameters.AddWithValue("@BatchCost", dr["BatchCost"].ToString() == "" ? 0 : dr["BatchCost"]);
                cm.Parameters.AddWithValue("@BatchStatus", dr["BatchStatus"].ToString() == "" ? 0 : dr["BatchStatus"]);
                cm.Parameters.AddWithValue("@LogDC", dr["LogDC"].ToString() == "" ? 0 : dr["LogDC"]);
                cm.Parameters.AddWithValue("@LogDK", dr["LogDK"].ToString() == "" ? 0 : dr["LogDK"]);
                cm.Parameters.AddWithValue("@LogDItm", dr["LogDItm"].ToString() == "" ? 0 : dr["LogDItm"]);
                cm.Parameters.AddWithValue("@LogDocDate", dr["LogDocDate"]);
                cm.Parameters.AddWithValue("@PurgeKeep", dr["PurgeKeep"].ToString() == "" ? 0 : dr["PurgeKeep"]);
                cm.Parameters.AddWithValue("@PurgeData", dr["PurgeData"].ToString() == "" ? 0 : dr["PurgeData"]);
                //cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                //cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                //cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                //cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);


                cm.Parameters["@NewBatchKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                NewBatchKey = (int)cm.Parameters["@NewBatchKey"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                    return false;
            }// Already close and dispose sql command.
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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
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
                    cm.CommandText = "MSTItmBatch_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);

                    cm.Parameters.AddWithValue("@BatchItmKey", dr["BatchItmKey"]);
                    cm.Parameters.AddWithValue("@BatchID", dr["BatchID"]);
                    cm.Parameters.AddWithValue("@BatchExpDate", dr["BatchExpDate"]);
                    cm.Parameters.AddWithValue("@BatchMfgDate", dr["BatchMfgDate"]);
                    cm.Parameters.AddWithValue("@BatchQty", dr["BatchQty"].ToString() == "" ? 0 : dr["BatchQty"]);
                    cm.Parameters.AddWithValue("@BatchQtyBal", dr["BatchQtyBal"].ToString() == "" ? 0 : dr["BatchQtyBal"]);
                    cm.Parameters.AddWithValue("@BatchCost", dr["BatchCost"].ToString() == "" ? 0 : dr["BatchCost"]);
                    cm.Parameters.AddWithValue("@BatchStatus", dr["BatchStatus"].ToString() == "" ? 0 : dr["BatchStatus"]);
                    cm.Parameters.AddWithValue("@LogDC", dr["LogDC"].ToString() == "" ? 0 : dr["LogDC"]);
                    cm.Parameters.AddWithValue("@LogDK", dr["LogDK"].ToString() == "" ? 0 : dr["LogDK"]);
                    cm.Parameters.AddWithValue("@LogDItm", dr["LogDItm"].ToString() == "" ? 0 : dr["LogDItm"]);
                    cm.Parameters.AddWithValue("@LogDocDate", dr["LogDocDate"]);
                    cm.Parameters.AddWithValue("@PurgeKeep", dr["PurgeKeep"].ToString() == "" ? 0 : dr["PurgeKeep"]);
                    cm.Parameters.AddWithValue("@PurgeData", dr["PurgeData"].ToString() == "" ? 0 : dr["PurgeData"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);


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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatch_Delete";

                cm.Parameters.AddWithValue("@BatchKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@BatchItmKey", criteria._BatchItmKey);
                cm.Parameters.AddWithValue("@Option", criteria._option);


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

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
        {
            bool retValue = false;

            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = Validation(cn, criteria, isNew);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatch_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);
                cm.Parameters.AddWithValue("@BatchItmKey", criteria._headerKey);
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

        #endregion Validation
    }
}

