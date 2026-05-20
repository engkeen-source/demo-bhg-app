using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
namespace BOLib
{
    /// <summary>
    /// Summary description for ARQOVendors.
    /// </summary>
    [Serializable]
    public class ARQOVendors : DataTable
    {
        #region +++  Constructor  +++

        public ARQOVendors()
        {

            //Datatable Copy method use Parametaless construstor
            //We need to skip if this Constructor was called from Copy method
            //otherwise we need to get Structure
            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(6, true);//Get For Copy . copy stackFrame is 8

            if ((!GFunc.CompareString(stack.GetMethod().Name, "Copy")) && (!GFunc.CompareString(stack.GetMethod().Name, "Clone")))                
            {
                this.Fetch(new Criteria(0, 1));
                //Add Two unbound Columns Vendor ID & Name
                this.Columns.Add("ItmVendorConID");
                this.Columns.Add("ItmVendorConNm");
            }


        }
        public ARQOVendors(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
            //Add Two unbound Columns Vendor ID & Name
            this.Columns.Add("ItmVendorConID");
            this.Columns.Add("ItmVendorConNm");

        }
        #endregion
        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _DocKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? DocKey)
            {
                _DocKey = DocKey;
            }
            internal Criteria(int? DocKey, int? Option)
            {
                _DocKey = DocKey;
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
            // No errors - commit transaction


            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARQOVendor_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
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
        #region Data Access - Insert

        internal bool Insert(out string msgID)
        {
            bool retValue = false;
            msgID = "RecordAddFail";
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, out msgID);
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
                    cm.CommandText = "ARQO_Vendor_Save";

                    // Datatable as XML format 
                    dt.TableName = "AR_QO_Vendor";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["AR_QO_Vendor"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strAR_QO_Vendors = swStringWriter.ToString();
                    ds.Tables.Remove(this);
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlAR_QO_Vendor";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strAR_QO_Vendors; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@DocKey";
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

        internal bool Insert(SqlConnection cn, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordAddFail";
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "ARQOVendor_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@MsgID", msgID);


                    cm.Parameters.AddWithValue("@DocKey", dr["DocKey"]);

                    cm.Parameters.AddWithValue("@VendorKey", dr["VendorKey"]);

                    cm.Parameters.AddWithValue("@VendorCurrKey", dr["VendorCurrKey"]);

                    cm.Parameters.AddWithValue("@TransmitMode", dr["TransmitMode"]);

                    cm.Parameters.AddWithValue("@Attention", dr["Attention"]);

                    cm.Parameters.AddWithValue("@EmailAddr", dr["EmailAddr"]);

                    cm.Parameters.AddWithValue("@FaxNumber", dr["FaxNumber"]);

                    cm.Parameters.AddWithValue("@TransmitStatus", dr["TransmitStatus"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    // Execute command.
                    cm.ExecuteNonQuery();
                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }// Already close and dispose sql command.
            }
            return retValue;
        }
        #endregion Insert
        #region Data Access - Update

        internal bool Update(out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Update(SqlConnection cn, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "ARQOVendor_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@MsgID", msgID);

                    cm.Parameters.AddWithValue("@DocKey", dr["DocKey"]);
                    cm.Parameters.AddWithValue("@VendorKey", dr["VendorKey"]);
                    cm.Parameters.AddWithValue("@VendorCurrKey", dr["VendorCurrKey"]);
                    cm.Parameters.AddWithValue("@TransmitMode", dr["TransmitMode"]);
                    cm.Parameters.AddWithValue("@Attention", dr["Attention"]);
                    cm.Parameters.AddWithValue("@EmailAddr", dr["EmailAddr"]);
                    cm.Parameters.AddWithValue("@FaxNumber", dr["FaxNumber"]);
                    cm.Parameters.AddWithValue("@TransmitStatus", dr["TransmitStatus"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();
                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }// Already close and dispose sql command.
            }

            return retValue;
        }
        #endregion Update
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
                cm.CommandText = "ARQOVendor_Delete";

                cm.Parameters.AddWithValue("@MsgID", msgID);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if (cm.Parameters["@MsgID"].Value == null)
                    msgID = string.Empty;
                else
                    msgID = cm.Parameters["@MsgID"].Value.ToString();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.

        }
        #endregion Delete
    }
}