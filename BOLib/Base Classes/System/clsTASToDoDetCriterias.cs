


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
    /// Summary description for TASToDoDetCriterias.
    /// </summary>
    [Serializable]
    public class TASToDoDetCriterias : DataTable
    {
        #region +++  Constructor  +++

        public TASToDoDetCriterias()
        {
            this.Fetch(new Criteria(0, 1));

        }
        public TASToDoDetCriterias(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 1));
        }
      
        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _ToDoKey = null;
            public int? _Option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? ToDoKey)
            {
                _ToDoKey = ToDoKey;
            }
            internal Criteria(int? ToDoKey, int? Option)
            {
                _ToDoKey = ToDoKey;
                _Option = Option;
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
                cm.CommandText = "TASToDoDetCriteria_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@ToDoKey", criteria._ToDoKey);
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

        internal bool Save(int? headerKey, DataTable dt)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Save(cn, headerKey, dt);
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
                    cm.CommandText = "TASToDoDetCriteria_Save";

                    TASToDoDetCriterias dtNew = new TASToDoDetCriterias(cn);
                    GFunc.CopyDocumentDetail(dt, dtNew);
                    // Datatable as XML format 
                    dtNew.TableName = "TAS_ToDoDetCriteria";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtNew);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["TAS_ToDoDetCriteria"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strTAS_ToDoDetCriterias = swStringWriter.ToString();

                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlTAS_ToDoDetCriteria";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strTAS_ToDoDetCriterias; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@ToDoKey";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Int32;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = headerKey; // XML string as parameter value  
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
                cm.CommandText = "TASToDoDetCriteria_Delete";

                cm.Parameters.AddWithValue("@ToDoKey", criteria._ToDoKey);

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

        public new TASToDoDetCriterias Copy()
        {
            DataTable dt = base.Copy();
            dt.DefaultView.RowFilter = this.DefaultView.RowFilter;
            return (TASToDoDetCriterias)dt;
        }      
    }
}