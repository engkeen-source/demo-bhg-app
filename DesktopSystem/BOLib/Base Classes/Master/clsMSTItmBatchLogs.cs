using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;

namespace BOLib
{
    [Serializable()]
    public class MSTItmBatchLogs : DataTable
    {

        #region Factory Methods

        public MSTItmBatchLogs()
        {
            this.Fetch(new Criteria(0, 1));
        }

        public MSTItmBatchLogs(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        public static MSTItmBatchLogs Get(int? headerKey)
        {
            MSTItmBatchLogs obj = new MSTItmBatchLogs();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static MSTItmBatchLogs New()
        {
            MSTItmBatchLogs obj = new MSTItmBatchLogs();
            return obj;
        }
        public static MSTItmBatchLogs New(SqlConnection cn)
        {
            MSTItmBatchLogs obj = new MSTItmBatchLogs();
            obj.Fetch(cn, new Criteria(0, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _option = null;
            public int _BatchKey = 0;

            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;

            }
            internal Criteria(int? HeaderKey, int? Option, int BatchKey)
            {
                _headerKey = HeaderKey;
                _option = Option;
                _BatchKey = BatchKey;
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
                cm.CommandText = "MSTItmBatchLog_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@BatchKey", criteria._headerKey);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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

        internal bool Insert(Criteria _criteria)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, _criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Save(SqlConnection cn, int? headerKey, DataTable dt)
        {
            using (StringWriter swStringWriter = new StringWriter())
            {
                using (SqlCommand cm = cn.CreateCommand())
                {

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MST_ItmBatchLog_Save";

                    // Datatable as XML format 
                    dt.TableName = "MST_ItmBatchLog";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["MST_ItmBatchLog"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strMST_ItmBatchLogs = swStringWriter.ToString();
                    ds.Tables.Remove(this);
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlMST_ItmBatchLog";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strMST_ItmBatchLogs; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    // Execute command.
                    int eff = cm.ExecuteNonQuery();
                    return true;
                }
            }
        }
        internal bool Insert(SqlConnection cn, Criteria _criteria)
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
                    cm.CommandText = "MSTItmBatchLog_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);


                    cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"]);
                    cm.Parameters.AddWithValue("@LogDC", GFunc.CompareString(dr["LogDC"].ToString(), "") ? 0 : dr["LogDC"]);
                    cm.Parameters.AddWithValue("@LogDK", GFunc.CompareString(dr["LogDK"].ToString(), "") ? 0 : dr["LogDK"]);
                    cm.Parameters.AddWithValue("@LogDItm", GFunc.CompareString(dr["LogDItm"].ToString(), "") ? 0 : dr["LogDItm"]);
                    cm.Parameters.AddWithValue("@LogType", dr["LogType"]);
                    cm.Parameters.AddWithValue("@LogSign", dr["LogSign"].ToString() == "" ? 1 : dr["LogSign"]);
                    cm.Parameters.AddWithValue("@LogDocDate", dr["LogDocDate"]);
                    cm.Parameters.AddWithValue("@BatchQty", dr["BatchQty"].ToString() == "" ? 0 : dr["BatchQty"]);
                    cm.Parameters.AddWithValue("@PurgeKeep", dr["PurgeKeep"].ToString() == "" ? 0 : dr["PurgeKeep"]);
                    cm.Parameters.AddWithValue("@PurgeData", dr["PurgeData"].ToString() == "" ? 0 : dr["PurgeData"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);


                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }// Already close and dispose sql command.
            }

            return retValue;
        }
        internal bool Insert(SqlConnection cn, DataRow dr)
        {
            if (this.Rows.Count == 0)
            {
                return true;
            }

            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmBatchLog_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"]);
                cm.Parameters.AddWithValue("@LogDC", dr["LogDC"].ToString() == "" ? 0 : dr["LogDC"]);
                cm.Parameters.AddWithValue("@LogDK", dr["LogDK"].ToString() == "" ? 0 : dr["LogDK"]);
                cm.Parameters.AddWithValue("@LogDItm", dr["LogDItm"].ToString() == "" ? 0 : dr["LogDItm"]);
                cm.Parameters.AddWithValue("@LogType", dr["LogType"]);
                cm.Parameters.AddWithValue("@LogSign", dr["LogSign"].ToString() == "" ? 1 : dr["LogSign"]);
                cm.Parameters.AddWithValue("@LogDocDate", dr["LogDocDate"]);
                cm.Parameters.AddWithValue("@BatchQty", dr["BatchQty"].ToString() == "" ? 0 : dr["BatchQty"]);
                cm.Parameters.AddWithValue("@PurgeKeep", dr["PurgeKeep"].ToString() == "" ? 0 : dr["PurgeKeep"]);
                cm.Parameters.AddWithValue("@PurgeData", dr["PurgeData"].ToString() == "" ? 0 : dr["PurgeData"]);

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
        #endregion Insert

        #region Data Access - Update

        internal bool Update(Criteria _criteria)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn, _criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Update(SqlConnection cn, Criteria _criteria)
        {
            bool retVal = false;

            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTItmBatchLog_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);


                    cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"]);
                    cm.Parameters.AddWithValue("@LogDC", dr["LogDC"].ToString() == "" ? 0 : dr["LogDC"]);
                    cm.Parameters.AddWithValue("@LogDK", dr["LogDK"].ToString() == "" ? 0 : dr["LogDK"]);
                    cm.Parameters.AddWithValue("@LogDItm", dr["LogDItm"].ToString() == "" ? 0 : dr["LogDItm"]);
                    cm.Parameters.AddWithValue("@LogType", dr["LogType"]);
                    cm.Parameters.AddWithValue("@LogSign", dr["LogSign"].ToString() == "" ? 1 : dr["LogSign"]);
                    cm.Parameters.AddWithValue("@LogDocDate", dr["LogDocDate"]);
                    cm.Parameters.AddWithValue("@BatchQty", dr["BatchQty"].ToString() == "" ? 0 : dr["BatchQty"]);
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
                        retVal = true;
                    else
                        retVal = false;
                }
            }// Already close and dispose sql command.

            return retVal;
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
                cm.CommandText = "MSTItmBatchLog_Delete";
                cm.Parameters.AddWithValue("@BatchKey", criteria._BatchKey);
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
