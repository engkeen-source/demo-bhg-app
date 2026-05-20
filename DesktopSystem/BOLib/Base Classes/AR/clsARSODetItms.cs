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
    /// Summary description for ARSODetItms.
    /// </summary>
    [Serializable]
    public class ARSODetItms : DataTable
    {

        #region +++  Constructor  +++

        public ARSODetItms()
        {
            //Datatable Copy method use Parametaless construstor
            //We need to skip if this Constructor was called from Copy method
            //otherwise we need to get Structure
            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(6, true);//Get For Copy . copy stackFrame is 8

            if (!(GFunc.CompareString(stack.GetMethod().Name, "Copy") || GFunc.CompareString(stack.GetMethod().Name, "Clone")))
                this.Fetch(new Criteria(0, 1));
        }

        public ARSODetItms(SqlConnection cn)
        {
            if (this.Columns.Count == 0)
                this.Fetch(cn, new Criteria(0, 1));
            else
                this.Rows.Clear();
        }

        #endregion


        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _DocKey = null;
            public int? _option = null;
            public int? _DocCodeKey = null;
            public int? _NewDocKey = null;

            internal Criteria()
            {
            }
            internal Criteria(int? DocKey)
            {
                _DocKey = DocKey;
            }
            internal Criteria(int? DocKey, int? DocCodeKey, int? NewDocKey)
            {
                _DocKey = DocKey;
                _DocCodeKey = DocCodeKey;
                _NewDocKey = NewDocKey;
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
                cm.CommandText = "ARSODetItm_Get";

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
        internal bool Fetch_ARQODetItm(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARQODetItm_CreateDoc";

                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", criteria._NewDocKey);
                cm.Parameters.AddWithValue("@CurrentUserKey", AppInfor.CurrentUserKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch_ARRODetItm(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARRODetItm_CreateDoc";

                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", criteria._NewDocKey);
                cm.Parameters.AddWithValue("@CurrentUserKey", AppInfor.CurrentUserKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

            return retValue;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out string msgID, out int? DocKey)
        {
            bool retValue = false;
            msgID = "RecordAddFail";
            DocKey = null;
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
                    cm.CommandText = "AR_SODetItm_Save";

                    // Datatable as XML format 
                    dt.TableName = "AR_SODetItm";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    //Change Column mapping type to Attribute
                    foreach (DataColumn dc in ds.Tables["AR_SODetItm"].Columns)
                    {
                        dc.ColumnMapping = MappingType.Attribute;
                    }

                    ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                    // Datatable as XML string 
                    string strAR_QODetItms = swStringWriter.ToString();
                    ds.Tables.Remove(this);
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlAR_SODetItm";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strAR_QODetItms; // XML string as parameter value  
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
            msgID = "RecordAddFail";
            using (StringWriter swStringWriter = new StringWriter())
            {

                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "ARSODetItm_AddUpdate";

                    // Datatable as XML format 
                    this.WriteXml(swStringWriter);
                    // Datatable as XML string 
                    string strARDODetItms = swStringWriter.ToString();
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlAR_SODetItm";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strARDODetItms; // XML string as parameter value  
                    // Add the parameter in Parameters collection.
                    cm.Parameters.Add(parameter);
                    // Execute command.
                    cm.ExecuteNonQuery();

                    return true;
                }
            }
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
                    cm.CommandText = "ARSODetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@MsgID", msgID);
                    cm.Parameters.AddWithValue("@NewDocKey", 0);

                    cm.Parameters.AddWithValue("@DocKey", dr["DocKey"]);
                    cm.Parameters.AddWithValue("@DocItmKey", dr["DocItmKey"]);
                    cm.Parameters.AddWithValue("@LineType", dr["LineType"]);
                    cm.Parameters.AddWithValue("@LineLinkKey", dr["LineLinkKey"]);
                    cm.Parameters.AddWithValue("@ItmSN", dr["ItmSN"]);
                    cm.Parameters.AddWithValue("@ItmKey", dr["ItmKey"]);
                    cm.Parameters.AddWithValue("@ItmKeySelect", dr["ItmKeySelect"]);
                    cm.Parameters.AddWithValue("@ItmType", dr["ItmType"]);
                    cm.Parameters.AddWithValue("@ItmDes", dr["ItmDes"]);
                    cm.Parameters.AddWithValue("@ItmDeptKey", dr["ItmDeptKey"]);
                    cm.Parameters.AddWithValue("@ItmTranGrpKey", dr["ItmTranGrpKey"]);
                    cm.Parameters.AddWithValue("@ItmAccKey", dr["ItmAccKey"]);
                    cm.Parameters.AddWithValue("@ItmLocKey", dr["ItmLocKey"]);
                    cm.Parameters.AddWithValue("@ItmStock", dr["ItmStock"]);
                    cm.Parameters.AddWithValue("@ItmQty", dr["ItmQty"]);
                    cm.Parameters.AddWithValue("@ItmQtyLink", dr["ItmQtyLink"]);
                    cm.Parameters.AddWithValue("@ItmQtyAdj", dr["ItmQtyAdj"]);
                    cm.Parameters.AddWithValue("@ItmOrderStatus", dr["ItmOrderStatus"]);
                    cm.Parameters.AddWithValue("@ItmReqDate", dr["ItmReqDate"]);
                    cm.Parameters.AddWithValue("@ItmPrmDate", dr["ItmPrmDate"]);
                    cm.Parameters.AddWithValue("@ItmUOMKey", dr["ItmUOMKey"]);
                    cm.Parameters.AddWithValue("@ItmConRate", dr["ItmConRate"]);
                    cm.Parameters.AddWithValue("@ItmLatestCostF", dr["ItmLatestCostF"]);
                    cm.Parameters.AddWithValue("@ItmLatestCostH", dr["ItmLatestCostH"]);
                    cm.Parameters.AddWithValue("@ItmListPrice", dr["ItmListPrice"]);
                    cm.Parameters.AddWithValue("@ItmPriceBefore", dr["ItmPriceBefore"]);
                    cm.Parameters.AddWithValue("@ItmPriceAfter", dr["ItmPriceAfter"]);
                    cm.Parameters.AddWithValue("@ItmDisPercent", dr["ItmDisPercent"]);
                    cm.Parameters.AddWithValue("@ItmDisValue", dr["ItmDisValue"]);
                    cm.Parameters.AddWithValue("@ItmPrice", dr["ItmPrice"]);
                    cm.Parameters.AddWithValue("@ItmPriceUser", dr["ItmPriceUser"]);
                    cm.Parameters.AddWithValue("@ItmControlPrice", dr["ItmControlPrice"]);
                    cm.Parameters.AddWithValue("@ItmControlPriceBase", dr["ItmControlPriceBase"]);
                    cm.Parameters.AddWithValue("@ItmAmtShw", dr["ItmAmtShw"]);
                    cm.Parameters.AddWithValue("@ItmAmtF", dr["ItmAmtF"]);
                    cm.Parameters.AddWithValue("@ItmAmtH", dr["ItmAmtH"]);
                    cm.Parameters.AddWithValue("@ItmTaxable", dr["ItmTaxable"]);
                    cm.Parameters.AddWithValue("@ItmTaxGrpKey", dr["ItmTaxGrpKey"]);
                    cm.Parameters.AddWithValue("@ItmTaxGrpRate", dr["ItmTaxGrpRate"]);
                    cm.Parameters.AddWithValue("@ItmTaxGrpAmtF", dr["ItmTaxGrpAmtF"]);
                    cm.Parameters.AddWithValue("@ItmTaxGrpAmtL", dr["ItmTaxGrpAmtL"]);
                    cm.Parameters.AddWithValue("@ItmHide", dr["ItmHide"]);
                    cm.Parameters.AddWithValue("@ItmColorKey", dr["ItmColorKey"]);
                    cm.Parameters.AddWithValue("@ItmScaleSize", dr["ItmScaleSize"]);
                    cm.Parameters.AddWithValue("@ItmPacking", dr["ItmPacking"]);
                    cm.Parameters.AddWithValue("@ItmRem", dr["ItmRem"]);
                    cm.Parameters.AddWithValue("@ItmMark", dr["ItmMark"]);
                    cm.Parameters.AddWithValue("@ItmVendorKey", dr["ItmVendorKey"]);
                    cm.Parameters.AddWithValue("@ItmVendorCurrKey", dr["ItmVendorCurrKey"]);
                    cm.Parameters.AddWithValue("@ItmVendorCurrRate", dr["ItmVendorCurrRate"]);
                    cm.Parameters.AddWithValue("@ItmVendorPrice", dr["ItmVendorPrice"]);
                    cm.Parameters.AddWithValue("@ItmVendorPriceRatio", dr["ItmVendorPriceRatio"]);
                    cm.Parameters.AddWithValue("@ItmVendorPriceLock", dr["ItmVendorPriceLock"]);
                    cm.Parameters.AddWithValue("@ItmJobKey", dr["ItmJobKey"]);
                    cm.Parameters.AddWithValue("@ItmJobPhaseKey", dr["ItmJobPhaseKey"]);
                    cm.Parameters.AddWithValue("@ItmJobTaskKey", dr["ItmJobTaskKey"]);
                    cm.Parameters.AddWithValue("@ItmJobCostTypeKey", dr["ItmJobCostTypeKey"]);
                    cm.Parameters.AddWithValue("@ItmIGrpDItm", dr["ItmIGrpDItm"]);
                    cm.Parameters.AddWithValue("@ItmIGrpQtyLock", dr["ItmIGrpQtyLock"]);
                    cm.Parameters.AddWithValue("@ItmIGrpToPrint", dr["ItmIGrpToPrint"]);
                    cm.Parameters.AddWithValue("@ItmIGrpQtySet", dr["ItmIGrpQtySet"]);
                    cm.Parameters.AddWithValue("@ItmIGrpAmtSet", dr["ItmIGrpAmtSet"]);
                    cm.Parameters.AddWithValue("@ItmAttachment", dr["ItmAttachment"]);
                    cm.Parameters.AddWithValue("@NSLink", dr["NSLink"]);
                    cm.Parameters.AddWithValue("@ARQOID", dr["ARQOID"]);
                    cm.Parameters.AddWithValue("@ARQODK", dr["ARQODK"]);
                    cm.Parameters.AddWithValue("@ARQODItm", dr["ARQODItm"]);
                    cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"]);
                    cm.Parameters.AddWithValue("@CreateUserKey", dr["CreateUserKey"]);
                    cm.Parameters.AddWithValue("@LastModifiedDate", dr["LastModifiedDate"]);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);

                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                    cm.ExecuteNonQuery();
                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

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
            bool retValue = false;
            msgID = "RecordDeleteFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARSODetItm_Delete";

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
                    retValue = true;
            }// Already close and dispose sql command.

            return retValue;
        }
        #endregion Delete
    }
}