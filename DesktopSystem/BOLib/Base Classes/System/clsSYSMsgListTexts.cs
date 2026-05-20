using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using BOLib;
using System.IO;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class SYSMsgListTexts : DataTable
    {

        #region Factory Methods
       
        public SYSMsgListTexts()
        {
            //this.Fetch(new Criteria(0, 1));
        }

        public SYSMsgListTexts(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }      

        public static SYSMsgListTexts Get(int? headerKey)
        {          
            SYSMsgListTexts obj = new SYSMsgListTexts();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static SYSMsgListTexts Get(int? headerKey, int? Option)
        {
            SYSMsgListTexts obj = new SYSMsgListTexts();
            obj.Fetch(new Criteria(headerKey, Option));
            return obj;
        }


        public static SYSMsgListTexts New()
        {           
            SYSMsgListTexts obj = new SYSMsgListTexts();
            return obj;
        }
        public static SYSMsgListTexts New(SqlConnection cn)
        {           
            SYSMsgListTexts obj = new SYSMsgListTexts();
            obj.Fetch(cn, new Criteria(0, 1));          
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
                    cm.CommandText = "SYSMsgListText_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._option);                   
                    cm.Parameters.AddWithValue("@DataGrp", criteria._headerKey);                   

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

        internal bool Insert()
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
                        retValue = this.Insert(cn);
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

        internal bool Save(SqlConnection cn, int? headerKey, DataTable dt)
        {
            bool retValue = false;
            string msgID = "RecordAddFail";
            try
            {
                using (StringWriter swStringWriter = new StringWriter())
                {

                    using (SqlCommand cm = cn.CreateCommand())
                    {

                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "SYS_MsgListText_Save";

                        // Datatable as XML format 
                        dt.TableName = "SYS_MsgListText";
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);

                        //Change Column mapping type to Attribute
                        foreach (DataColumn dc in ds.Tables["SYS_MsgListText"].Columns)
                        {
                            dc.ColumnMapping = MappingType.Attribute;
                        }

                        ds.WriteXml(swStringWriter, XmlWriteMode.WriteSchema);
                        // Datatable as XML string 
                        string strSYS_MsgListTexts = swStringWriter.ToString();
                        ds.Tables.Remove(this);
                        // Add input parameter and set its properties.
                        SqlParameter parameter = new SqlParameter();
                        // Store procedure parameter name  
                        parameter.ParameterName = "@xmlSYS_MsgListText";
                        // Parameter type as XML 
                        parameter.DbType = DbType.Xml;
                        parameter.Direction = ParameterDirection.Input; // Input Parameter  
                        parameter.Value = strSYS_MsgListTexts; // XML string as parameter value  
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
                throw new TAException(msgID);
            }
            return retValue;
        }
        internal bool Insert(SqlConnection cn)
        {
            bool retValue = false;           

            try
            {
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
                        cm.CommandText = "SYSMsgListText_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 0);
                        
                        cm.Parameters.AddWithValue("@DataGrp", dr["DataGrp"]);
                        cm.Parameters.AddWithValue("@MsgValue", dr["MsgValue"]);
                        cm.Parameters.AddWithValue("@LangText1", dr["LangText1"]);
                        cm.Parameters.AddWithValue("@LangText2", dr["LangText2"]);
                        cm.Parameters.AddWithValue("@LangText3", dr["LangText3"]);
                        cm.Parameters.AddWithValue("@LangText4", dr["LangText4"]);
                        cm.Parameters.AddWithValue("@LangText5", dr["LangText5"]);
                        cm.Parameters.AddWithValue("@LangText6", dr["LangText6"]);
                        cm.Parameters.AddWithValue("@LangText7", dr["LangText7"]);
                        cm.Parameters.AddWithValue("@LangText8", dr["LangText8"]);
                        cm.Parameters.AddWithValue("@LangText9", dr["LangText9"]);
                        cm.Parameters.AddWithValue("@LangText10", dr["LangText10"]);
                        cm.Parameters.AddWithValue("@BuildIn", dr["BuildIn"].ToString() == "" ? 0 : dr["BuildIn"]);
                        cm.Parameters.AddWithValue("@Hidden", dr["Hidden"].ToString() == "" ? 0 : dr["Hidden"]);
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

        internal bool Update(Criteria _criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Create new sql connection for this method. 
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        // Open sql connection. 
                        cn.Open();
                        retValue = this.Update(cn, _criteria, out msgID);
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

        internal bool Update(SqlConnection cn, Criteria _criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            try
            {
                foreach (DataRow dr in this.Rows)
                {
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "SYSMsgListText_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 1);
                        cm.Parameters.AddWithValue("@MsgID", msgID);

                        cm.Parameters.AddWithValue("@DataGrp", dr["DataGrp"]);
                        cm.Parameters.AddWithValue("@MsgValue", dr["MsgValue"]);
                        cm.Parameters.AddWithValue("@LangText1", dr["LangText1"]);
                        cm.Parameters.AddWithValue("@LangText2", dr["LangText2"]);
                        cm.Parameters.AddWithValue("@LangText3", dr["LangText3"]);
                        cm.Parameters.AddWithValue("@LangText4", dr["LangText4"]);
                        cm.Parameters.AddWithValue("@LangText5", dr["LangText5"]);
                        cm.Parameters.AddWithValue("@LangText6", dr["LangText6"]);
                        cm.Parameters.AddWithValue("@LangText7", dr["LangText7"]);
                        cm.Parameters.AddWithValue("@LangText8", dr["LangText8"]);
                        cm.Parameters.AddWithValue("@LangText9", dr["LangText9"]);
                        cm.Parameters.AddWithValue("@LangText10", dr["LangText10"]);
                        cm.Parameters.AddWithValue("@BuildIn", dr["BuildIn"].ToString() == "" ? 0 : dr["BuildIn"]);
                        cm.Parameters.AddWithValue("@Hidden", dr["Hidden"].ToString() == "" ? 0 : dr["Hidden"]);
                        cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"].ToString() == "" ? DateTime.Today : dr["CreateDate"]);
                        cm.Parameters.AddWithValue("@CreateUserKey", dr["CreateUserKey"]);
                        cm.Parameters.AddWithValue("@LastModifiedDate", dr["LastModifiedDate"]);
                        cm.Parameters.AddWithValue("@LastModifiedUserKey", dr["LastModifiedUserKey"]);
                        cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                        cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                        cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                        cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                        cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                        cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                        cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                        cm.Parameters["@MsgID"].Direction = ParameterDirection.InputOutput;
                        cm.Parameters.AddWithValue("@RetValue", 0);
                        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                        // cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                        // Execute command.
                        cm.ExecuteNonQuery();
                        if (cm.Parameters["@MsgID"].Value == null)
                            msgID = string.Empty;
                        else
                            msgID = cm.Parameters["@MsgID"].Value.ToString();

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
                    cm.CommandText = "SYSMsgListText_Delete";                  
                    cm.Parameters.AddWithValue("@DataGrp", criteria._headerKey);


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


//using System;
//using System.Data;
//using System.Data.SqlClient;
//using Csla;
//using Csla.Data;
//using System.Transactions;

//namespace BOLib
//{
//    [Serializable()]
//    public class SYSMsgListTexts : Csla.BusinessListBase<SYSMsgListTexts, SYSMsgListText>
//    {

//        #region Factory Methods

//        public SYSMsgListTexts()
//        {
//        }

//        internal static SYSMsgListTexts New()
//        {
//            //
//            SYSMsgListTexts obj = new SYSMsgListTexts();
//            //
//            return obj;
//        }

//        internal static SYSMsgListTexts Get()
//        {
//            //
//            SYSMsgListTexts obj = new SYSMsgListTexts();
//            obj.Fetch(new Criteria(0, string.Empty, 0));
//            return obj;
//        }

//        //internal static SYSMsgListTexts Get(int? DataGrp)
//        //{
//        //    //
//        //    SYSMsgListTexts obj = new SYSMsgListTexts();
//        //    obj.Fetch(new Criteria(DataGrp, string.Empty, 1));
//        //    return obj;
//        //}

//        public static SYSMsgListTexts Get(int? DataGrp)
//        {
//            //
//            SYSMsgListTexts obj = new SYSMsgListTexts();
//            obj.Fetch(new Criteria(DataGrp, string.Empty, 2));
//            return obj;
//        }

//        #endregion //Factory Methods

//        #region Criteria

//        [Serializable()]
//        internal class Criteria
//        {
//            internal int? _dataGrp = 0;
//            internal string _msgValue = string.Empty;
//            public int? _option = null;

//            internal Criteria()
//            {
//                _option = 0;
//            }

//            internal Criteria(int? DataGrp, string MsgValue, int? Option)
//            {
//                _dataGrp = DataGrp;
//                _msgValue = MsgValue;
//                _option = Option;
//            }
//        }

//        #endregion //Criteria

//        #region Data Access - Fetch

//        internal bool Fetch(Criteria criteria)
//        {
//            bool retValue = false;
//            using (TransactionScope scope = new TransactionScope())
//            {
//                // Create new sql connection for this method. 
//                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
//                {
//                    // Open sql connection. 
//                    cn.Open();

//                    retValue = this.Fetch(cn, criteria);
//                }// End of SqlConnection.

//                // No errors - commit transaction
//                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
//            }// End of TransactionScope
            
//            return retValue;
//        }

//        internal bool Fetch(SqlConnection cn, Criteria criteria)
//        {
//            using (SqlCommand cm = cn.CreateCommand())
//            {
//                cm.CommandType = CommandType.StoredProcedure;
//                cm.CommandText = "SYSMsgListText_Get";

//                cm.Parameters.AddWithValue("@Option", criteria._option);

//                if ((bool)GFunc.IsNE(criteria._dataGrp))
//                    cm.Parameters.AddWithValue("@DataGrp", DBNull.Value);
//                else
//                    cm.Parameters.AddWithValue("@DataGrp", criteria._dataGrp);

//                cm.Parameters.AddWithValue("@RetValue", 0);
//                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

//                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
//                {
//                    while (dr.Read())
//                        this.Add(SYSMsgListText.Get(dr));
//                }

//                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
//                    return true;
//                else
//                    return false;
//            }//using
//        }


//        #endregion //Data Access - Fetch
//    }
//}
