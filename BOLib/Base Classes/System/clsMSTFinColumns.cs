using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTFinColumns : DataTable
    {

        #region Factory Methods

        public MSTFinColumns()
        {
            this.Fetch(new Criteria(-1, 1));
        }        

        public MSTFinColumns(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(-1, 1));
        }

        public new MSTFinColumns Clone()
        {
             MSTFinColumns objCopy = (MSTFinColumns)this.MemberwiseClone();
            return objCopy;
        }

        public static MSTFinColumns Get(int? RepKey)
        {
            MSTFinColumns obj = new MSTFinColumns();
            obj.Fetch(new Criteria(RepKey, 1));
            return obj;
        }

        public static MSTFinColumns Get(int? RepKey, bool FormualCol)
        {
            MSTFinColumns obj = new MSTFinColumns();
            obj.Fetch(new Criteria(RepKey, FormualCol, 1));
            return obj;
        }

        public static MSTFinColumns New()
        {
            MSTFinColumns obj = new MSTFinColumns();
            return obj;
        }
        public static MSTFinColumns New(SqlConnection cn)
        {
            MSTFinColumns obj = new MSTFinColumns();
            obj.Fetch(cn,new Criteria(0, 1));
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
            public decimal? _colNo = null;
            public bool? _FormulaColGet = false;

            internal Criteria()
            {
            }

            internal Criteria(int? RepKey, int? Option)
            {
                _repKey = RepKey;
                _option = Option;
                _repDetKey = 0;
                _colNo = 0;
                _FormulaColGet = false;
            }

            internal Criteria(int? RepKey,bool FormulaColSearch, int? Option)
            {
                _repKey = RepKey;
                _option = Option;
                _repDetKey = 0;
                _colNo = 0;
                _FormulaColGet = FormulaColSearch;
            }

            internal Criteria(int? RepKey, int? RepDetKey, int? Option)
            {
                _repKey = RepKey;
                _repDetKey = RepDetKey;
                _option = Option;
                _colNo = 0;
                _FormulaColGet = false;
            }

            internal Criteria(int? RepKey, int? RepDetKey, decimal? ColNo, int? Option)
            {
                _repKey = RepKey;
                _repDetKey = RepDetKey;
                _colNo = ColNo;
                _option = Option;
                _FormulaColGet = false;
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
                    cm.CommandText = "MSTFinColumn_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@RepKey", criteria._repKey);
                    cm.Parameters.AddWithValue("@RepDetKey", criteria._repKey);
                    cm.Parameters.AddWithValue("@ColNo", criteria._repKey);

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

        internal bool Insert(Criteria _criteria )
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
                        retValue = this.Insert(cn, _criteria );
                    }
                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// Already close and dispose sql connection.
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        internal bool Save(SqlConnection cn, int? headerKey,DataTable dt)
        {
        bool retValue = false;
        try
        {
            using (StringWriter swStringWriter = new StringWriter())
            {

                using (SqlCommand cm = cn.CreateCommand())
                {

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MST_FinColumn_Save";

                    // Datatable as XML format 
                    string strMST_FinColumns = GFunc.ConvertDataTableToXML(this);
                    // Add input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();
                    // Store procedure parameter name  
                    parameter.ParameterName = "@xmlMST_FinColumn";
                    // Parameter type as XML 
                    parameter.DbType = DbType.Xml;
                    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                    parameter.Value = strMST_FinColumns; // XML string as parameter value  
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
                    return  true;
                    
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
                        cm.CommandText = "MSTFinColumn_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 0);

                        cm.Parameters.AddWithValue("@RepKey", _criteria._repKey);
                        cm.Parameters.AddWithValue("@RepDetKey", dr["RepDetKey"]);
                        cm.Parameters.AddWithValue("@ColNo", dr["ColNo"].ToString() == "" ? 0 : dr["ColNo"]);
                        cm.Parameters.AddWithValue("@ColType", dr["ColType"].ToString() == "" ? 0 : dr["ColType"]);
                        cm.Parameters.AddWithValue("@ColText", dr["ColText"]);
                        cm.Parameters.AddWithValue("@ColDisplay", dr["ColDisplay"].ToString() == "" ? 0 : dr["ColDisplay"]);
                        cm.Parameters.AddWithValue("@ColWidth", dr["ColWidth"].ToString() == "" ? 0 : dr["ColWidth"]);
                        cm.Parameters.AddWithValue("@ColDetailFormat", dr["ColDetailFormat"].ToString() == "" ? "": dr["ColDetailFormat"]);
                        cm.Parameters.AddWithValue("@ColBalanceExp", dr["ColBalanceExp"]);
                        cm.Parameters.AddWithValue("@ColFormulaExp", dr["ColFormulaExp"]);
                        cm.Parameters.AddWithValue("@ColIgnoreRowReverse", dr["ColIgnoreRowReverse"].ToString() == "" ? 0 : dr["ColIgnoreRowReverse"]);
                        cm.Parameters.AddWithValue("@TotalExp", dr["TotalExp"]);
                        cm.Parameters.AddWithValue("@ColBranchKey", dr["ColBranchKey"]);
                        cm.Parameters.AddWithValue("@ColDeptKey", dr["ColDeptKey"]);
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
                throw (ex);
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
                        retValue = this.Update(cn,_criteria);
                    }
                    // No errors - commit transaction
                      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
                }// Already close and dispose sql connection.
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        internal bool Update(SqlConnection cn,Criteria _criteria)
        {
            bool retValue = false;
            try
            {
                foreach (DataRow dr in this.Rows)
                {
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "MSTFinColumn_AddUpdate";

                        cm.Parameters.AddWithValue("@Option", 1);

                        cm.Parameters.AddWithValue("@RepKey", dr["RepKey"]);
                        cm.Parameters.AddWithValue("@RepDetKey", dr["RepDetKey"]);
                        cm.Parameters.AddWithValue("@ColNo", dr["ColNo"].ToString() == "" ? 0 : dr["ColNo"]);
                        cm.Parameters.AddWithValue("@ColType", dr["ColType"].ToString() == "" ? 0 : dr["ColType"]);
                        cm.Parameters.AddWithValue("@ColText", dr["ColText"]);
                        cm.Parameters.AddWithValue("@ColDisplay", dr["ColDisplay"].ToString() == "" ? 0 : dr["ColDisplay"]);
                        cm.Parameters.AddWithValue("@ColWidth", dr["ColWidth"].ToString() == "" ? 0 : dr["ColWidth"]);
                        cm.Parameters.AddWithValue("@ColDetailFormat", dr["ColDetailFormat"].ToString() == "" ? "Right" : dr["ColDetailFormat"]);
                        cm.Parameters.AddWithValue("@ColBalanceExp", dr["ColBalanceExp"]);
                        cm.Parameters.AddWithValue("@ColFormulaExp", dr["ColFormulaExp"]);
                        cm.Parameters.AddWithValue("@ColIgnoreRowReverse", dr["ColIgnoreRowReverse"].ToString() == "" ? 0 : dr["ColIgnoreRowReverse"]);
                        cm.Parameters.AddWithValue("@TotalExp", dr["TotalExp"]);
                        cm.Parameters.AddWithValue("@ColBranchKey", dr["ColBranchKey"]);
                        cm.Parameters.AddWithValue("@ColDeptKey", dr["ColDeptKey"]);
                        cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"].ToString() == "" ? DateTime.Now: GFunc.NEDateTime(dr["CreateDate"],DateTime.Now));
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
                throw (ex);
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
                throw (ex);
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
                    cm.CommandText = "MSTFinColumn_Delete";

                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@RepKey", criteria._repKey);
                    cm.Parameters.AddWithValue("@RepDetKey", criteria._repDetKey);
                    cm.Parameters.AddWithValue("@ColNo", criteria._colNo);

                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    int i=cm.ExecuteNonQuery();


                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                }// Already close and dispose sql command.
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        #endregion Delete

    }
}
