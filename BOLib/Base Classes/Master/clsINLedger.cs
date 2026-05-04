using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;

namespace BOLib
{
    [Serializable()]
    public class INLedgers : DataTable
    {

        #region Factory Methods
     
        public INLedgers()
        {
            this.Fetch(new Criteria(0,1));          
        }        

        public INLedgers(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }      

        public static INLedgers Get(int? headerKey)
        {            
            INLedgers obj = new INLedgers();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static INLedgers New()
        {           
            INLedgers obj = new INLedgers();
            return obj;
        }
        public static INLedgers New(SqlConnection cn)
        {           
            INLedgers obj = new INLedgers();
            obj.Fetch(cn,new Criteria(0, 1));          
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _headerKey = HeaderKey;
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
                cm.CommandText = "INLedger_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@TranDate", criteria._headerKey);
                cm.Parameters.AddWithValue("@TranCodeKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@TranType", criteria._headerKey);
                cm.Parameters.AddWithValue("@TranID", criteria._headerKey);
                cm.Parameters.AddWithValue("@TranDetID", criteria._headerKey);
                cm.Parameters.AddWithValue("@StockID", criteria._headerKey);
                cm.Parameters.AddWithValue("@BatchID", criteria._headerKey);                    

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
                    retValue = this.Insert(cn, _criteria );
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
                    cm.CommandText = "IN_Ledger_Save";

                    // Datatable as XML format 
                    dt.TableName = "IN_Ledger";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["IN_Ledger"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strIN_Ledgers = swStringWriter.ToString();
                    ds.Tables.Remove(this);
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlIN_Ledger";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strIN_Ledgers; // XML string as parameter value  
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
                return  true;
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
                    cm.CommandText = "INLedger_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);                      

                    cm.Parameters.AddWithValue("@TranDate", dr["TranDate"]);
                    cm.Parameters.AddWithValue("@TranType", dr["TranType"]);
                    cm.Parameters.AddWithValue("@TranID", dr["TranID"]);
                    cm.Parameters.AddWithValue("@TranDetID", dr["TranDetID"]);
                    cm.Parameters.AddWithValue("@StockID", dr["StockID"]);
                    cm.Parameters.AddWithValue("@Batch", dr["Batch"]);
                    cm.Parameters.AddWithValue("@AccIN", dr["AccIN"]);
                    cm.Parameters.AddWithValue("@AccCOS", dr["AccCOS"]);
                    cm.Parameters.AddWithValue("@AccAdj", dr["AccAdj"]);
                    cm.Parameters.AddWithValue("@AccMFNOH", dr["AccMFNOH"]);
                    cm.Parameters.AddWithValue("@AccRnd", dr["AccRnd"]);
                    cm.Parameters.AddWithValue("@Reference", dr["Reference"]);
                    cm.Parameters.AddWithValue("@StockQuantity", dr["StockQuantity"]);
                    cm.Parameters.AddWithValue("@StockPrice", dr["StockPrice"]);
                    cm.Parameters.AddWithValue("@StockAmount", dr["StockAmount"]);
                    cm.Parameters.AddWithValue("@StockBalance", dr["StockBalance"]);
                    cm.Parameters.AddWithValue("@FIFOStack", dr["FIFOStack"]);
                    cm.Parameters.AddWithValue("@FIFOAdjustAmount", dr["FIFOAdjustAmount"]);
                    cm.Parameters.AddWithValue("@FIFOClosingAmount", dr["FIFOClosingAmount"]);
                    cm.Parameters.AddWithValue("@FIFOCostAmount", dr["FIFOCostAmount"]);
                    cm.Parameters.AddWithValue("@LIFOStack", dr["LIFOStack"]);
                    cm.Parameters.AddWithValue("@LIFOAdjustAmount", dr["LIFOAdjustAmount"]);
                    cm.Parameters.AddWithValue("@LIFOClosingAmount", dr["LIFOClosingAmount"]);
                    cm.Parameters.AddWithValue("@LIFOCostAmount", dr["LIFOCostAmount"]);
                    cm.Parameters.AddWithValue("@AVGCostPrice", dr["AVGCostPrice"]);
                    cm.Parameters.AddWithValue("@AVGAdjustAmount", dr["AVGAdjustAmount"]);
                    cm.Parameters.AddWithValue("@AVGClosingAmount", dr["AVGClosingAmount"]);
                    cm.Parameters.AddWithValue("@AVGCostAmount", dr["AVGCostAmount"]);
                    cm.Parameters.AddWithValue("@CostPercent", dr["CostPercent"]);
                    cm.Parameters.AddWithValue("@ReferenceID", dr["ReferenceID"]);
                    cm.Parameters.AddWithValue("@TranCodeKey", dr["TranCodeKey"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                   
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();
                    // Check Return Value -- Changed By Richard
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;                            
                    }
                    else
                    {
                        retValue=false;
                    }          
                }// Already close and dispose sql command.
            }
            return retValue;
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
                    retValue = this.Update(cn,_criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Update(SqlConnection cn,Criteria _criteria)
        {
            bool retValue = false;
            
            
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "INLedger_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    

                    cm.Parameters.AddWithValue("@TranDate", dr["TranDate"]);
                    cm.Parameters.AddWithValue("@TranType", dr["TranType"]);
                    cm.Parameters.AddWithValue("@TranID", dr["TranID"]);
                    cm.Parameters.AddWithValue("@TranDetID", dr["TranDetID"]);
                    cm.Parameters.AddWithValue("@StockID", dr["StockID"]);
                    cm.Parameters.AddWithValue("@Batch", dr["Batch"]);
                    cm.Parameters.AddWithValue("@AccIN", dr["AccIN"]);
                    cm.Parameters.AddWithValue("@AccCOS", dr["AccCOS"]);
                    cm.Parameters.AddWithValue("@AccAdj", dr["AccAdj"]);
                    cm.Parameters.AddWithValue("@AccMFNOH", dr["AccMFNOH"]);
                    cm.Parameters.AddWithValue("@AccRnd", dr["AccRnd"]);
                    cm.Parameters.AddWithValue("@Reference", dr["Reference"]);
                    cm.Parameters.AddWithValue("@StockQuantity", dr["StockQuantity"]);
                    cm.Parameters.AddWithValue("@StockPrice", dr["StockPrice"]);
                    cm.Parameters.AddWithValue("@StockAmount", dr["StockAmount"]);
                    cm.Parameters.AddWithValue("@StockBalance", dr["StockBalance"]);
                    cm.Parameters.AddWithValue("@FIFOStack", dr["FIFOStack"]);
                    cm.Parameters.AddWithValue("@FIFOAdjustAmount", dr["FIFOAdjustAmount"]);
                    cm.Parameters.AddWithValue("@FIFOClosingAmount", dr["FIFOClosingAmount"]);
                    cm.Parameters.AddWithValue("@FIFOCostAmount", dr["FIFOCostAmount"]);
                    cm.Parameters.AddWithValue("@LIFOStack", dr["LIFOStack"]);
                    cm.Parameters.AddWithValue("@LIFOAdjustAmount", dr["LIFOAdjustAmount"]);
                    cm.Parameters.AddWithValue("@LIFOClosingAmount", dr["LIFOClosingAmount"]);
                    cm.Parameters.AddWithValue("@LIFOCostAmount", dr["LIFOCostAmount"]);
                    cm.Parameters.AddWithValue("@AVGCostPrice", dr["AVGCostPrice"]);
                    cm.Parameters.AddWithValue("@AVGAdjustAmount", dr["AVGAdjustAmount"]);
                    cm.Parameters.AddWithValue("@AVGClosingAmount", dr["AVGClosingAmount"]);
                    cm.Parameters.AddWithValue("@AVGCostAmount", dr["AVGCostAmount"]);
                    cm.Parameters.AddWithValue("@CostPercent", dr["CostPercent"]);
                    cm.Parameters.AddWithValue("@ReferenceID", dr["ReferenceID"]);
                    cm.Parameters.AddWithValue("@TranCodeKey", dr["TranCodeKey"]);
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
                        retValue = true;
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
            bool retValue = false;
            
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "INLedger_Delete";

                
                
                cm.Parameters.AddWithValue("@TranDate", criteria._headerKey);


                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();

               

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
            }// Already close and dispose sql command.
            
            return retValue;
        }

        #endregion Delete

    }
}
