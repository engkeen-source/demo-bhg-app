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
    /// Summary description for SYSRepParas.
    /// </summary>
    [Serializable]
    public class SYSRepParas : DataTable
    {
        #region +++  Constructor  +++

        public SYSRepParas()
        {
            SYSRepPara obj = new SYSRepPara();

            this.Fetch(new Criteria(0, 1));

        }
        public SYSRepParas(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 1));
        }
     
        public static SYSRepParas Get(int RepKey)
        {
            SqlConnection cn = new SqlConnection(Database.BossDemoConnection);

            SYSRepParas obj = new SYSRepParas();
            obj.Fetch(cn, new Criteria(RepKey));
            return obj;
        }

        public static SYSRepParas Get(SqlConnection cn,int RepKey)
        {
            SYSRepParas obj = new SYSRepParas();
            obj.Fetch(cn,new Criteria(RepKey));
            return obj;
        }

        #endregion
        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepKey = null;
            public int? _Option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? RepKey)
            {
                _Option = 1;
                _RepKey = RepKey;
            }
            internal Criteria(int? RepKey, int? Option)
            {
                _RepKey = RepKey;
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
                cm.CommandText = "SYSRepPara_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
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
                    cm.CommandText = "SYS_RepPara_Save";

                    // Datatable as XML format 
                    dt.TableName = "SYS_RepPara";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["SYS_RepPara"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strSYS_RepParas = swStringWriter.ToString();

                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlSYS_RepPara";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strSYS_RepParas; // XML string as parameter value  
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

                    return true;
                }
            }

        }


        #endregion Save

        #region Data Access - Delete

        internal bool Delete(Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordDeleteFail";
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
        {
            msgID = "RecordDeleteFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRepPara_Delete";

                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);

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






