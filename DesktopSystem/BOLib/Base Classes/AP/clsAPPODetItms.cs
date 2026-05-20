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
    /// Summary description for APPODetItms.
    /// </summary>
    [Serializable]
    public class APPODetItms : DataTable
    {
        #region +++  Constructor  +++

        public APPODetItms()
        {

            //Datatable Copy method use Parametaless construstor
            //We need to skip if this Constructor was called from Copy method
            //otherwise we need to get Structure
            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(6, true);//Get For Copy . copy stackFrame is 8

            if (!(GFunc.CompareString(stack.GetMethod().Name, "Copy") || GFunc.CompareString(stack.GetMethod().Name, "Clone")))
                this.Fetch(new Criteria(0, 1));
        }

        public APPODetItms(SqlConnection cn)
        {

            this.Fetch(cn, new Criteria(0, 1));
        }     

        public static APPODetItms Get(int? DocKey)
        {
            return Get(new Criteria(DocKey, 1));
        }

        #endregion
        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _DocKey = null;
            public int? _Option = null;
            public int? _DocCodeKey = null;
            public int? _NewDocKey = null;
            public string _Xml = string.Empty;
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
                _Option = Option;
            }
            internal Criteria(int? DocKey, int? DocCodeKey, int? NewDocKey)
            {
                _DocKey = DocKey;
                _DocCodeKey = DocCodeKey;
                _NewDocKey = NewDocKey;
            }

            internal Criteria(int? DocKey, int? DocCodeKey, int? NewDocKey,string xml)
            {
                _DocKey = DocKey;
                _DocCodeKey = DocCodeKey;
                _NewDocKey = NewDocKey;
                _Xml = xml;
            }
        }
        #endregion //Criteria
        #region Data Access - Fetch
        private static APPODetItms Get(Criteria criteria)
        {
            SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
            APPODetItms l_reval = new APPODetItms();
           
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cn.Open();

                    

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "APPODetItm_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._Option);
                    cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    sqlAdp.Fill(l_reval);

                    //if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    //    return true;
                    //else
                    //    return false;

                    return l_reval;
                }// Already close and dispose sql connection.
            

        }

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
                cm.CommandText = "APPODetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
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
        internal bool Fetch_MstJobEst(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MstJobDetEst_CreatePO";

                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);
                cm.Parameters.AddWithValue("@NewDocKey", criteria._NewDocKey);
                cm.Parameters.AddWithValue("@xml", criteria._Xml);
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

        /* added by KKAung on 04 Jun 2023  */
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

        /* added by KKAung on 09 Jun 2023  */
        internal bool Fetch_ARSODetItm(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARSODetItm_CreateDoc";

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

        //#region Data Access - Save

        ////internal bool Save(int? headerKey, DataTable dt)
        ////{
        ////    bool retValue = false;
        ////    using (TransactionScope scope = new TransactionScope())
        ////    {
        ////        // Create new sql connection for this method. 
        ////        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        ////        {
        ////            // Open sql connection. 
        ////            cn.Open();
        ////            retValue = this.Save(cn, headerKey, dt);
        ////        }
        ////        // No errors - commit transaction
        ////          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        ////    }// Already close and dispose sql connection.

        ////    return retValue;
        ////}

        ////internal bool Save(SqlConnection cn, int? headerKey, DataTable dt)
        ////{
        ////    using (StringWriter swStringWriter = new StringWriter())
        ////    {

        ////        using (SqlCommand cm = cn.CreateCommand())
        ////        {

        ////            cm.CommandType = CommandType.StoredProcedure;
        ////            cm.CommandText = "AP_PODetItm_Save";

        ////            // Datatable as XML format 
        ////            dt.TableName = "AP_PODetItm";
        ////            DataSet ds = new DataSet();
        ////            ds.Tables.Add(dt);

        ////            //Change Column mapping type to Attribute
        ////            foreach (DataColumn dc in ds.Tables["AP_PODetItm"].Columns)
        ////            {
        ////                dc.ColumnMapping = MappingType.Attribute;
        ////            }

        ////            ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
        ////            // Datatable as XML string 
        ////            string strAP_PODetItms = swStringWriter.ToString();

        ////            // Add input parameter and set its properties.
        ////            SqlParameter parameter = new SqlParameter();
        ////            // Store procedure parameter name  
        ////            parameter.ParameterName = "@xmlAP_PODetItm";
        ////            // Parameter type as XML 
        ////            parameter.DbType = DbType.Xml;
        ////            parameter.Direction = ParameterDirection.Input; // Input Parameter  
        ////            parameter.Value = strAP_PODetItms; // XML string as parameter value  
        ////            // Add the parameter in Parameters collection.
        ////            cm.Parameters.Add(parameter);
        ////            parameter = new SqlParameter();
        ////            // Store procedure parameter name  
        ////            parameter.ParameterName = "@DocKey";
        ////            // Parameter type as XML 
        ////            parameter.DbType = DbType.Int32;
        ////            parameter.Direction = ParameterDirection.Input; // Input Parameter  
        ////            parameter.Value = headerKey; // XML string as parameter value  
        ////            // Add the parameter in Parameters collection.
        ////            cm.Parameters.Add(parameter);
        ////            // Execute command.
        ////            int eff = cm.ExecuteNonQuery();

        ////            return true;
        ////        }
        ////    }

        ////}


        //#endregion Save

        //#region Data Access - Delete

        ////internal bool Delete(Criteria criteria, out string msgID)
        ////{
        ////    bool retValue = false;
        ////    msgID = "RecordDeleteFail";
        ////    using (TransactionScope scope = new TransactionScope())
        ////    {
        ////        //Create new sql connection for this method. 
        ////        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        ////        {
        ////            // Open sql connection. 
        ////            cn.Open();
        ////            retValue = this.Delete(cn, criteria, out msgID);
        ////        }
        ////        // No errors - commit transaction
        ////          if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        ////    }// Already close and dispose sql connection.

        ////    return retValue;
        ////}
        ////internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
        ////{
        ////    msgID = "RecordDeleteFail";
        ////    using (SqlCommand cm = cn.CreateCommand())
        ////    {
        ////        cm.CommandType = CommandType.StoredProcedure;
        ////        cm.CommandText = "APPODetItm_Delete";

        ////        cm.Parameters.AddWithValue("@MsgID", msgID);
        ////        cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);

        ////        cm.Parameters.AddWithValue("@RetValue", 0);
        ////        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
        ////        // Execute command.
        ////        cm.ExecuteNonQuery();
        ////        if (cm.Parameters["@MsgID"].Value == null)
        ////            msgID = string.Empty;
        ////        else
        ////            msgID = cm.Parameters["@MsgID"].Value.ToString();

        ////        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        ////            return true;
        ////        else
        ////            return false;
        ////    }// Already close and dispose sql command.            
        ////}
        //#endregion Delete

    }
}

