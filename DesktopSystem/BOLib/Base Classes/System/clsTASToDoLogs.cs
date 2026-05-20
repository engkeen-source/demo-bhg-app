


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
namespace BOLib
{
    /// <summary>
    /// Summary description for TASToDoLogs.
    /// </summary>
    [Serializable]
    public class TASToDoLogs : DataTable
    {
        #region +++  Constructor  +++

        public TASToDoLogs()
        {
            this.Fetch(new Criteria(0,0, 1));
        }
        public TASToDoLogs(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0,0, 1));
        }    

        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _UserKey = null;
            public int? _AlertKey = null;
            public int? _option = null;
            public string _DocID = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? AlertKey)
            {
                _AlertKey = AlertKey;
                _UserKey = 0;
            }
            internal Criteria(int? AlertKey, int? Option)
            {
                _AlertKey = AlertKey;
                _UserKey = 0;
                _option = Option;
            }
            internal Criteria(int? AlertKey, int UserKey, int? Option)
            {
                _UserKey = UserKey;
                _AlertKey = AlertKey;
                _option = Option;
            }
            internal Criteria(int? AlertKey, int? UserKey, string DocID, int? Option)
            {
                _UserKey = UserKey;
                _AlertKey = AlertKey;
                _DocID = DocID;
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
            }
               

            return retValue;
        }
        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "TASToDoLog_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ToDoLogKey", criteria._AlertKey);
                //cm.Parameters.AddWithValue("@UserKey", criteria._UserKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

        }
        #endregion //Data Access - Fetch

        #region Data Access - Save

        internal bool Save( DataTable dt)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Save(cn,dt);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Save(SqlConnection cn, DataTable dt)
        {
            using (StringWriter swStringWriter = new StringWriter())
            {

                using (SqlCommand cm = cn.CreateCommand())
                {

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "TASToDoLog_Save";

                    TASToDoLogs dtNew = new TASToDoLogs(cn);
                    GFunc.CopyDocumentDetail(dt,dtNew);
                    // Datatable as XML format 
                    dtNew.TableName = "dtTASToDoLog";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtNew);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["dtTASToDoLog"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strTAS_AlertDetSubs = swStringWriter.ToString();

                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlTAS_ToDoLog";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strTAS_AlertDetSubs; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);                    
                    // Execute command.
                    int eff = cm.ExecuteNonQuery();

                    return true;
                }
            }

        }


        #endregion Save

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
                cm.CommandText = "TASToDoLog_Delete";
                cm.Parameters.AddWithValue("@ToDoLogKey", criteria._AlertKey);
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






