using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTFinRows : DataTable
    {

        #region Factory Methods

        public MSTFinRows()
        {
            this.Fetch(new Criteria(-1, 1));
        }

        public MSTFinRows(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(-1, 1));
        }

        public new MSTFinRows Clone()
        {
            MSTFinRows objCopy = (MSTFinRows)this.MemberwiseClone();
            return objCopy;
        }

        public static MSTFinRows Get(int? RepKey)
        {
            MSTFinRows obj = new MSTFinRows();
            obj.Fetch(new Criteria(RepKey, 1));
            return obj;
        }

        public static MSTFinRows Get(int? RepKey,int? RepDetKey)
        {
            MSTFinRows obj = new MSTFinRows();
            obj.Fetch(new Criteria(RepKey,RepDetKey, 2));
            return obj;
        }

        public static MSTFinRows New()
        {
            MSTFinRows obj = new MSTFinRows();
            return obj;
        }
        public static MSTFinRows New(SqlConnection cn)
        {
            MSTFinRows obj = new MSTFinRows();
            obj.Fetch(cn, new Criteria(0, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _repKey = null;
            public int? _option = null;
            public int? _repDetKey = null;
            public int? _rowNo = null;
            public decimal? _rowSeq = null;

            internal Criteria()
            {
            }

            internal Criteria(int? RepKey, int? Option)
            {
                _repKey = RepKey;
                _option = Option;
            }

            internal Criteria(int? RepKey,int? RepDetKey, int? Option)
            {
                _repKey = RepKey;
                _repDetKey = RepDetKey;
                _option = Option;
            }

            internal Criteria(int? RepKey,int? RepDetKey,int? RowNo, int? Option)
            {
                _repKey = RepKey;
                _repDetKey = RepDetKey;
                _rowNo = RowNo;
                _option = Option;
            }

            internal Criteria(int? RepKey, int? RepDetKey, int? RowNo, decimal? RowSeq, int? Option)
            {
                _repKey = RepKey;
                _repDetKey = RepDetKey;
                _rowNo = RowNo;
                _rowSeq = RowSeq;
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
                    cm.CommandText = "MSTFinRow_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@RepKey", criteria._repKey);
                    cm.Parameters.AddWithValue("@RepDetKey", criteria._repKey);
                    cm.Parameters.AddWithValue("@RowNo", criteria._repKey);
                    cm.Parameters.AddWithValue("@RowSeq", criteria._repKey);

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

        internal bool Insert(Criteria _criteria)
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
                        retValue = this.Insert(cn, _criteria);
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
            try
            {
                using (StringWriter swStringWriter = new StringWriter())
                {

                    using (SqlCommand cm = cn.CreateCommand())
                    {

                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "MST_FinRow_Save";

                        // Datatable as XML format 
                        string strMST_FinRows = GFunc.ConvertDataTableToXML(this);
                        // Add input parameter and set its properties.
                        SqlParameter parameter = new SqlParameter();
                        // Store procedure parameter name  
                        parameter.ParameterName = "@xmlMST_FinRow";
                        // Parameter type as XML 
                        parameter.DbType = DbType.Xml;
                        parameter.Direction = ParameterDirection.Input; // Input Parameter  
                        parameter.Value = strMST_FinRows; // XML string as parameter value  
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
                        parameter = new SqlParameter();
                        // Store procedure parameter name  
                        parameter.ParameterName = "@RepDetKey";
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
                        cm.CommandText = "MSTFinRow_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 0);

                        cm.Parameters.AddWithValue("@RepKey", _criteria._repKey);
                        cm.Parameters.AddWithValue("@RepDetKey", dr["RepDetKey"]);
                        cm.Parameters.AddWithValue("@RowNo", dr["RowNo"].ToString() == "" ? 0 : dr["RowNo"]);
                        cm.Parameters.AddWithValue("@RowSeq", dr["RowSeq"].ToString() == "" ? 0 : dr["RowSeq"]);
                        cm.Parameters.AddWithValue("@RowAccTypeKey", dr["RowAccTypeKey"].ToString() == "" ? 0 : dr["RowAccTypeKey"]);
                        cm.Parameters.AddWithValue("@RowAccGrpKey", dr["RowAccGrpKey"]);
                        cm.Parameters.AddWithValue("@RowAccF", dr["RowAccF"]);
                        cm.Parameters.AddWithValue("@RowAccT", dr["RowAccT"]);
                        cm.Parameters.AddWithValue("@RowDeptF", dr["RowDeptF"]);
                        cm.Parameters.AddWithValue("@RowDeptT", dr["RowDeptT"]);
                        cm.Parameters.AddWithValue("@RowBranchF", dr["RowBranchF"]);
                        cm.Parameters.AddWithValue("@RowBranchT", dr["RowBranchT"]);
                        cm.Parameters.AddWithValue("@RowRangeFilter", dr["RowRangeFilter"]);
                        cm.Parameters.AddWithValue("@RowDisplayType", dr["RowDisplayType"].ToString() == "" ? 0 : dr["RowDisplayType"]);
                        cm.Parameters.AddWithValue("@LineSummaryText", dr["LineSummaryText"]);
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
                        retValue = this.Update(cn, _criteria);
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

        internal bool Update(SqlConnection cn, Criteria _criteria)
        {
            bool retValue = false;
            try
            {
                foreach (DataRow dr in this.Rows)
                {
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "MSTFinRow_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 1);

                        cm.Parameters.AddWithValue("@RepKey", dr["RepKey"]);
                        cm.Parameters.AddWithValue("@RepDetKey", dr["RepDetKey"]);
                        cm.Parameters.AddWithValue("@RowNo", dr["RowNo"].ToString() == "" ? 0 : dr["RowNo"]);
                        cm.Parameters.AddWithValue("@RowSeq", dr["RowSeq"].ToString() == "" ? 0 : dr["RowSeq"]);
                        cm.Parameters.AddWithValue("@RowAccTypeKey", dr["RowAccTypeKey"].ToString() == "" ? 0 : dr["RowAccTypeKey"]);
                        cm.Parameters.AddWithValue("@RowAccGrpKey", dr["RowAccGrpKey"]);
                        cm.Parameters.AddWithValue("@RowAccF", dr["RowAccF"]);
                        cm.Parameters.AddWithValue("@RowAccT", dr["RowAccT"]);
                        cm.Parameters.AddWithValue("@RowDeptF", dr["RowDeptF"]);
                        cm.Parameters.AddWithValue("@RowDeptT", dr["RowDeptT"]);
                        cm.Parameters.AddWithValue("@RowBranchF", dr["RowBranchF"]);
                        cm.Parameters.AddWithValue("@RowBranchT", dr["RowBranchT"]);
                        cm.Parameters.AddWithValue("@RowRangeFilter", dr["RowRangeFilter"]);
                        cm.Parameters.AddWithValue("@RowDisplayType", dr["RowDisplayType"].ToString() == "" ? 0 : dr["RowDisplayType"]);
                        cm.Parameters.AddWithValue("@LineSummaryText", dr["LineSummaryText"]);
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
                    cm.CommandText = "MSTFinRow_Delete";

                    cm.Parameters.AddWithValue("@RepKey", criteria._repKey);                                     
                    cm.Parameters.AddWithValue("@RetValue", Convert.ToInt32(0));
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                   int i= cm.ExecuteNonQuery();

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
