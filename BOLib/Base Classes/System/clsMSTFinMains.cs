using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTFinMains : DataTable
    {
        #region Factory Methods

        public MSTFinMains()
        {
            this.Fetch(new Criteria(0,1));          
        }        

        public MSTFinMains(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        public new MSTFinMains Clone()
        {
             MSTFinMains objCopy = (MSTFinMains)this.MemberwiseClone();
            return objCopy;
        }

        public static MSTFinMains Get(int? repKey)
        {
            MSTFinMains obj = new MSTFinMains();
            obj.Fetch(new Criteria(repKey, 1));
            return obj;
        }

        public static MSTFinMains New()
        {
            MSTFinMains obj = new MSTFinMains();
            return obj;
        }
        public static MSTFinMains New(SqlConnection cn)
        {
            MSTFinMains obj = new MSTFinMains();
            obj.Fetch(cn,new Criteria(0, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _repKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? RepKey, int? Option)
            {
                _repKey = RepKey;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
            try
            {                
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();

                    retValue = this.Fetch(cn, criteria);
                }// End of SqlConnection.
                   
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTFinMain_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@RepKey", criteria._repKey);

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        sqlAdp.Fill(this);
                       
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;

                }//using
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(Criteria _criteria )
        {
            bool retValue = false;
            try
            {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Save(SqlConnection cn, int? headerKey,DataTable dt)
        {
        bool retValue = false;
        try
        {
            using (StringWriter swStringWriter = new StringWriter())
            {

                using (SqlCommand cm = cn.CreateCommand())
                {

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MST_FinMain_Save";

                    // Datatable as XML format 
                    string strMST_FinMains = GFunc.ConvertDataTableToXML(this);
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlMST_FinMain";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strMST_FinMains; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@RepKey";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Int32;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = headerKey; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    // Execute command.
                   int eff = cm.ExecuteNonQuery();

                    retValue = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retValue;
        }
        internal bool Insert(SqlConnection cn, Criteria _criteria)
        {
            bool retValue = false;
            
            try
            {
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
                        cm.CommandText = "MSTFinMain_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 0);

                        cm.Parameters.AddWithValue("@RepKey", dr["RepKey"]);
                        cm.Parameters.AddWithValue("@RepName", dr["RepName"]);
                        cm.Parameters.AddWithValue("@RepType", dr["RepType"]);
                        cm.Parameters.AddWithValue("@Remarks", dr["Remarks"]);
                        cm.Parameters.AddWithValue("@PaperSize", dr["PaperSize"].ToString() == "" ? "A4" : GFunc.NEStr(dr["PaperSize"],"A4"));
                        cm.Parameters.AddWithValue("@MarginTop", dr["MarginTop"].ToString() == "" ? 1 : dr["MarginTop"]);
                        cm.Parameters.AddWithValue("@MarginBottom", dr["MarginBottom"].ToString() == "" ? 1 : dr["MarginBottom"]);
                        cm.Parameters.AddWithValue("@MarginLeft", dr["MarginLeft"].ToString() == "" ? 1 : dr["MarginLeft"]);
                        cm.Parameters.AddWithValue("@MarginRight", dr["MarginRight"].ToString() == "" ? 1 : dr["MarginRight"]);
                        cm.Parameters.AddWithValue("@Hidden", dr["Hidden"].ToString() == "" ? 0 : dr["Hidden"]);
                        cm.Parameters.AddWithValue("@BuildIn", dr["BuildIn"].ToString() == "" ? 0 : dr["BuildIn"]);
                        cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                        cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                        cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
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
                    }// Already close and dispose sql command.
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        #endregion Insert

        #region Data Access - Update

        internal bool Update(Criteria _criteria)
        {
            bool retValue = false;
            try
            {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Update(SqlConnection cn,Criteria _criteria)
        {
            bool retValue = false;
            try
            {
                foreach (DataRow dr in this.Rows)
                {
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "MSTFinMain_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 1);

                        cm.Parameters.AddWithValue("@RepKey", dr["RepKey"]);
                        cm.Parameters.AddWithValue("@RepName", dr["RepName"]);
                        cm.Parameters.AddWithValue("@RepType", dr["RepType"]);
                        cm.Parameters.AddWithValue("@Remarks", dr["Remarks"]);
                        cm.Parameters.AddWithValue("@PaperSize", dr["PaperSize"].ToString() == "" ? "A4" : GFunc.NEStr(dr["PaperSize"],"A4"));
                        cm.Parameters.AddWithValue("@MarginTop", dr["MarginTop"].ToString() == "" ? 1 : dr["MarginTop"]);
                        cm.Parameters.AddWithValue("@MarginBottom", dr["MarginBottom"].ToString() == "" ? 1 : dr["MarginBottom"]);
                        cm.Parameters.AddWithValue("@MarginLeft", dr["MarginLeft"].ToString() == "" ? 1 : dr["MarginLeft"]);
                        cm.Parameters.AddWithValue("@MarginRight", dr["MarginRight"].ToString() == "" ? 1 : dr["MarginRight"]);
                        cm.Parameters.AddWithValue("@Hidden", dr["Hidden"].ToString() == "" ? 0 : dr["Hidden"]);
                        cm.Parameters.AddWithValue("@BuildIn", dr["BuildIn"].ToString() == "" ? 0 : dr["BuildIn"]);
                        cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"].ToString() == "" ? DateTime.Now : GFunc.NEDateTime(dr["CreateDate"],DateTime.Now));
                        cm.Parameters.AddWithValue("@CreateUserKey", dr["CreateUserKey"]);
                        cm.Parameters.AddWithValue("@LastModifiedDate", dr["LastModifiedDate"].ToString() == "" ? DateTime.Now : GFunc.NEDateTime(dr["LastModifiedDate"],DateTime.Now));
                        cm.Parameters.AddWithValue("@LastModifiedUserKey", dr["LastModifiedUserKey"]);
                        cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                        cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                        cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
            try
            {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTFinMain_Delete";

                    cm.Parameters.AddWithValue("@RepKey", criteria._repKey);

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();


                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                }// Already close and dispose sql command.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        #endregion Delete

    }
}
