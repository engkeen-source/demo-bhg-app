using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSRepRpts.
    /// </summary>
    [Serializable]
    public class SYSRepRpts : DataTable
    {

        #region Factory Methods
        
        public SYSRepRpts()
        {           
        }

        public SYSRepRpts(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }      

        public static SYSRepRpts Get(int? headerKey)
        {            
            SYSRepRpts obj = new SYSRepRpts();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static SYSRepRpts New()
        {            
            SYSRepRpts obj = new SYSRepRpts();
            return obj;
        }
        public static SYSRepRpts New(SqlConnection cn)
        {            
            SYSRepRpts obj = new SYSRepRpts();
            obj.Fetch(cn, new Criteria(0, 1));            
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepKey = null;
            public int? _option = null;           

            internal Criteria()
            {
            }
            internal Criteria(int? RepKey)
            {
                _RepKey = RepKey;
            }
            internal Criteria(int? RepKey, int? Option)
            {
                _RepKey = RepKey;
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
                cm.CommandText = "SYSRepRpt_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);

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
                    retValue = this.Insert(cn);
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
                    cm.CommandText = "SYS_RepRpt_Save";

                    // Datatable as XML format 
                    dt.TableName = "SYS_RepRpt";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["SYS_RepRpt"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strSYS_RepRpts = swStringWriter.ToString();
                    ds.Tables.Remove(this);
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlSYS_RepRpt";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strSYS_RepRpts; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    // Execute command.
                    int eff = cm.ExecuteNonQuery();

                    return true;
                }
            }            
        }

        internal bool Insert(SqlConnection cn)
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
                    cm.CommandText = "SYSRepRpt_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);

                    cm.Parameters.AddWithValue("@NewUID", 0);
                    cm.Parameters.AddWithValue("@UID", dr["UID"]);
                    cm.Parameters.AddWithValue("@RepKey", dr["RepKey"]);
                    cm.Parameters.AddWithValue("@RptNm", dr["RptNm"]);
                    cm.Parameters.AddWithValue("@RptDes", dr["RptDes"]);
                    cm.Parameters.AddWithValue("@RptDesLang1", dr["RptDesLang1"]);
                    cm.Parameters.AddWithValue("@RptDesLang2", dr["RptDesLang2"]);
                    cm.Parameters.AddWithValue("@RptDesLang3", dr["RptDesLang3"]);
                    cm.Parameters.AddWithValue("@RptDesLang4", dr["RptDesLang4"]);
                    cm.Parameters.AddWithValue("@RptDesLang5", dr["RptDesLang5"]);
                    cm.Parameters.AddWithValue("@RptDesLang6", dr["RptDesLang6"]);
                    cm.Parameters.AddWithValue("@RptDesLang7", dr["RptDesLang7"]);
                    cm.Parameters.AddWithValue("@RptDesLang8", dr["RptDesLang8"]);
                    cm.Parameters.AddWithValue("@RptDesLang9", dr["RptDesLang9"]);
                    cm.Parameters.AddWithValue("@RptDesLang10", dr["RptDesLang10"]);
                    cm.Parameters.AddWithValue("@RptTitle", dr["RptTitle"]);
                    cm.Parameters.AddWithValue("@RptTitleLang1", dr["RptTitleLang1"]);
                    cm.Parameters.AddWithValue("@RptTitleLang2", dr["RptTitleLang2"]);
                    cm.Parameters.AddWithValue("@RptTitleLang3", dr["RptTitleLang3"]);
                    cm.Parameters.AddWithValue("@RptTitleLang4", dr["RptTitleLang4"]);
                    cm.Parameters.AddWithValue("@RptTitleLang5", dr["RptTitleLang5"]);
                    cm.Parameters.AddWithValue("@RptTitleLang6", dr["RptTitleLang6"]);
                    cm.Parameters.AddWithValue("@RptTitleLang7", dr["RptTitleLang7"]);
                    cm.Parameters.AddWithValue("@RptTitleLang8", dr["RptTitleLang8"]);
                    cm.Parameters.AddWithValue("@RptTitleLang9", dr["RptTitleLang9"]);
                    cm.Parameters.AddWithValue("@RptTitleLang10", dr["RptTitleLang10"]);
                    cm.Parameters.AddWithValue("@RptLayOut", dr["RptLayOut"].ToString() == "" ? 0 : dr["RptLayOut"]);
                    cm.Parameters.AddWithValue("@RptAltRecordSource", dr["RptAltRecordSource"]);
                    cm.Parameters.AddWithValue("@RptPermission", dr["RptPermission"]);
                    cm.Parameters.AddWithValue("@RptPrintCondition", dr["RptPrintCondition"]);
                    cm.Parameters.AddWithValue("@ShwItmCount", dr["ShwItmCount"].ToString() == "" ? 0 : dr["ShwItmCount"]);
                    cm.Parameters.AddWithValue("@ShwLetterHead", dr["ShwLetterHead"].ToString() == "" ? 0 : dr["ShwLetterHead"]);
                    cm.Parameters.AddWithValue("@PrtCopies", dr["PrtCopies"].ToString() == "" ? 1 : dr["PrtCopies"]);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                    cm.Parameters.AddWithValue("@BuildIn", dr["BuildIn"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                   
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.Parameters["@NewUID"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if (!GFunc.IsNEZ(cm.Parameters["@NewUID"].Value))
                        dr["UID"] = cm.Parameters["@NewUID"].Value;
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                }// Already close and dispose sql command.
            }
            
            return retValue;
        }

        #endregion Insert        
       
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
                cm.CommandText = "SYSRepRpt_Delete";

                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
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