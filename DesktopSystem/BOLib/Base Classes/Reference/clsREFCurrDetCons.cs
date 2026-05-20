using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.IO;
namespace BOLib
{
    [Serializable()]
    public class REFCurrDetCons : DataTable
    {

        #region Factory Methods

        public REFCurrDetCons()
        {
            this.Fetch(new Criteria(0, 1));          
        }

        public REFCurrDetCons(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }
      
        public static REFCurrDetCons Get(int? currKey)
        {
            REFCurrDetCons obj = new REFCurrDetCons();
            obj.Fetch(new Criteria(currKey, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _currKey = 0;
            public DateTime? _currDate = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? CurrKey,int? Option)
            {
                _currKey = CurrKey;                
                _option = Option;
            }
            internal Criteria(int? CurrKey, DateTime? CurrDate, int? Option)
            {
                _currKey = CurrKey;
                _currDate = CurrDate;
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
                cm.CommandText = "REFCurrDetCon_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);

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

        internal bool Insert( int? headerKey)
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
            using (StringWriter swStringWriter = new StringWriter())
            {

                using (SqlCommand cm = cn.CreateCommand())
                {

                    //Update CreateUserKey and ModifiedUserKey
                    foreach (DataRow dr in this.Rows)
                    {
                        dr["CurrKey"] = headerKey;
                        dr["ConCurrRate"] = dr["ConCurrRate"].ToString() == "" ? 1 : dr["ConCurrRate"];
                        dr["ConCustomRate1"] = dr["ConCustomRate1"].ToString() == "" ? 1 : dr["ConCustomRate1"];
                        dr["ConCustomRate2"]= dr["ConCustomRate2"].ToString() == "" ? 1 : dr["ConCustomRate2"];
                        dr["ConCustomRate3"] = dr["ConCustomRate3"].ToString() == "" ? 1 : dr["ConCustomRate3"];

                        dr["CreateUserKey"] = AppInfor.currentUserKey;
                        dr["LastModifiedUserKey"] = AppInfor.currentUserKey;
                    }
                    this.AcceptChanges();

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFCurrDetCon_Save";

                    // Datatable as XML format 
                    this.TableName = "REF_CurrDetCon";
                    string strRefCurrDetCons = GFunc.ConvertDataTableToXML(this);
                    
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlREF_CurrDetCon";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strRefCurrDetCons; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    // Execute command.
                     int eff = cm.ExecuteNonQuery();
                     if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                     else
                        return false;

                }
            }            
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
                    cm.CommandText = "ARDODetCon_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);

                    cm.Parameters.AddWithValue("@ConCurrKey", dr["ConCurrKey"]);
                    cm.Parameters.AddWithValue("@ConCurrDate", dr["ConCurrDate"]);
                    cm.Parameters.AddWithValue("@ConCurrRate", dr["ConCurrRate"]);
                    cm.Parameters.AddWithValue("@ConKey", dr["ConKey"]);
                    cm.Parameters.AddWithValue("@ConCustomRate1", dr["ConCustomRate1"]);
                    cm.Parameters.AddWithValue("@ConCustomRate2", dr["ConCustomRate2"]);
                    cm.Parameters.AddWithValue("@ConCustomRate3", dr["ConCustomRate3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "REFCurrDetCon_Delete";

                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);

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