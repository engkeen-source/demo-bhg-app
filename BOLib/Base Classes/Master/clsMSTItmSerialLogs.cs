using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;

namespace BOLib
{
    [Serializable()]
    public class MSTItmSerialLogs : DataTable
    {

        #region Factory Methods
       
        public MSTItmSerialLogs()
        {
            this.Fetch(new Criteria(0,1));          
        }        

        public MSTItmSerialLogs(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }
      
        public static MSTItmSerialLogs Get(int? headerKey)
        {           
            MSTItmSerialLogs obj = new MSTItmSerialLogs();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static MSTItmSerialLogs New()
        {          
            MSTItmSerialLogs obj = new MSTItmSerialLogs();
            return obj;
        }
        public static MSTItmSerialLogs New(SqlConnection cn)
        {            
            MSTItmSerialLogs obj = new MSTItmSerialLogs();
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
                cm.CommandText = "MSTItmSerialLog_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                  
                cm.Parameters.AddWithValue("@SerialKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@DocDC", 0);
                cm.Parameters.AddWithValue("@DocDK", 0);
                cm.Parameters.AddWithValue("@DocDItm", 0);
                
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

        internal bool Save(SqlConnection cn, int? headerKey,DataTable dt)
        {        
        using (StringWriter swStringWriter = new StringWriter())
        {

            using (SqlCommand cm = cn.CreateCommand())
            {

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_ItmSerial_Log_Save";

                // Datatable as XML format 
                dt.TableName = "MST_ItmSerial_Log";
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                //Change Column mapping type to Attribute
                foreach (DataColumn dc in ds.Tables["MST_ItmSerial_Log"].Columns)
                {
                    dc.ColumnMapping=MappingType.Attribute;
                }

                ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                // Datatable as XML string 
                string strMST_ItmSerial_Logs = swStringWriter.ToString();
                ds.Tables.Remove(this);
                // Add input parameter and set its properties.
                SqlParameter parameter = new SqlParameter();
                // Store procedure parameter name  
                parameter.ParameterName = "@xmlMST_ItmSerial_Log";
                // Parameter type as XML 
                parameter.DbType = DbType.Xml;
                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                parameter.Value = strMST_ItmSerial_Logs; // XML string as parameter value  
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
                    cm.CommandText = "MSTItmSerialLog_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    

                    cm.Parameters.AddWithValue("@SerialKey", dr["SerialKey"]);
                    cm.Parameters.AddWithValue("@DocDC", dr["DocDC"]);
                    cm.Parameters.AddWithValue("@DocDK", dr["DocDK"]);
                    cm.Parameters.AddWithValue("@DocDItm", dr["DocDItm"]);
                    cm.Parameters.AddWithValue("@LogType", dr["LogType"]);
                    cm.Parameters.AddWithValue("@Qty", dr["Qty"].ToString() == "" ? 1 : dr["Qty"]);
                    cm.Parameters.AddWithValue("@Warranty", dr["Warranty"]);
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
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmSerialLog_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
               

                cm.Parameters.AddWithValue("@SerialKey", dr["SerialKey"]);
                cm.Parameters.AddWithValue("@DocDC", dr["DocDC"]);
                cm.Parameters.AddWithValue("@DocDK", dr["DocDK"]);
                cm.Parameters.AddWithValue("@DocDItm", dr["DocDItm"]);
                cm.Parameters.AddWithValue("@LogType", dr["LogType"]);
                cm.Parameters.AddWithValue("@Qty", dr["Qty"].ToString() == "" ? 1 : dr["Qty"]);
                cm.Parameters.AddWithValue("@Warranty", dr["Warranty"]);                   

                
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
                    cm.CommandText = "MSTItmSerialLog_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    

                    cm.Parameters.AddWithValue("@SerialKey", dr["SerialKey"]);
                    cm.Parameters.AddWithValue("@DocDC", dr["DocDC"]);
                    cm.Parameters.AddWithValue("@DocDK", dr["DocDK"]);
                    cm.Parameters.AddWithValue("@DocDItm", dr["DocDItm"]);
                    cm.Parameters.AddWithValue("@LogType", dr["LogType"]);
                    cm.Parameters.AddWithValue("@Qty", dr["Qty"].ToString() == "" ? 1 : dr["Qty"]);
                    cm.Parameters.AddWithValue("@Warranty", dr["Warranty"]);
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
                    else
                        retValue =false;
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
                cm.CommandText = "MSTItmSerialLog_Delete";

                
                
                cm.Parameters.AddWithValue("@SerialKey", criteria._headerKey);


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
