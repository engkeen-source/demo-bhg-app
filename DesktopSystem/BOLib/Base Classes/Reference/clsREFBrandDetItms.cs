using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class REFBrandDetItms : DataTable
    {

        #region Factory Methods

        public REFBrandDetItms()
        {
            
            try
            {
                this.Fetch(new Criteria(0, 1));
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public REFBrandDetItms(SqlConnection cn)
        {
            
            try
            {
                this.Fetch(cn, new Criteria(0, 1));
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public static REFBrandDetItms Get( int? brandKey)
        {
            
            REFBrandDetItms obj = new REFBrandDetItms();
            try
            {
                obj.Fetch(new Criteria(brandKey, 1));
                return obj;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;            
            public int? _option = null;
            public string _model = "";
            internal Criteria()
            {
            }

            internal Criteria(int? headerKey, int? Option)
            {
                _headerKey = headerKey;
                _option = Option;
            }
            internal Criteria(int? headerKey,string model, int? Option)
            {
                _headerKey = headerKey;
                _option = Option;
                _model = model;
            } 
        }

        #endregion //Criteria

        #region Data Access - Fetch

        //internal bool Fetch(Criteria criteria)
        //{
        //    bool retValue = false;
        //    
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            // Create new sql connection for this method. 
        //            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //            {
        //                // Open sql connection. 
        //                cn.Open();

        //                retValue = this.Fetch(cn, criteria);
        //            }// End of SqlConnection.

        //            // No errors - commit transaction
        //              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //        }// End of TransactionScope
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retValue;
        //}

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

        //internal bool Fetch(SqlConnection cn, Criteria criteria)
        //{
        //    
        //    bool retValue = false;
        //    try
        //    {
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrandDetItm_Get";

        //            cm.Parameters.AddWithValue("@Option", criteria._option);
        //            
        //            cm.Parameters.AddWithValue("@BrandKey", criteria._headerKey);
        //            cm.Parameters.AddWithValue("@Model", criteria._model);

        //            

        //            // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
        //            cm.Parameters.AddWithValue("@RetValue", 0);
        //            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

        //            System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
        //            try
        //            {
        //                sqlAdp.Fill(this);

        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);
        //            }

        //            if (cm.Parameters["@MsgID"].Value == null)
        //                msgID = string.Empty;
        //            else
        //                msgID = cm.Parameters["@MsgID"].Value.ToString();

        //            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //                retValue = true;

        //        }//using
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retValue;
        //}

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrandDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                  
                cm.Parameters.AddWithValue("@BrandKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@Model", criteria._model);
               

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

        //internal bool Insert( int? headerKey)
        //{
        //    bool retValue = false;
        //    msgID = "RecordAddFail";
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            // Create new sql connection for this method. 
        //            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //            {
        //                // Open sql connection. 
        //                cn.Open();
        //                retValue = this.Insert(cn, headerKey);
        //            }
        //            // No errors - commit transaction
        //              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //        }// Already close and dispose sql connection.
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        internal bool Insert(int? headerKey)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, headerKey);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Insert(SqlConnection cn, int? headerKey)
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
                    cm.CommandText = "REFBrandDetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);                      

                    cm.Parameters.AddWithValue("@BrandKey", headerKey);
                    cm.Parameters.AddWithValue("@Model", dr["Model"]);
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
                    else
                        retValue = false;
                }// Already close and dispose sql command.
            }
            return retValue;
        }

        //internal bool Insert(SqlConnection cn, int? headerKey)
        //{
        //    bool retValue = false;
        //    msgID = "RecordAddFail";

        //    try
        //    {
        //        if (this.Rows.Count == 0)
        //        {
        //            return true;

        //        }
        //        foreach (DataRow dr in this.Rows)
        //        {
        //            if (dr.RowState == DataRowState.Deleted)
        //            {
        //                retValue = true;
        //                continue;
        //            }
        //            using (SqlCommand cm = cn.CreateCommand())
        //            {
        //                cm.CommandType = CommandType.StoredProcedure;
        //                cm.CommandText = "REFBrandDetItm_AddUpdate";

        //                cm.Parameters.AddWithValue("@Option", 0);
        //                

        //                cm.Parameters.AddWithValue("@BrandKey", headerKey);                        
        //                cm.Parameters.AddWithValue("@Model", dr["Model"]);
        //                cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
        //                cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
        //                cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
        //                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
        //                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
        //                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
        //                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

        //                
        //                cm.Parameters.AddWithValue("@RetValue", 0);
        //                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
        //                // Execute command.
        //                cm.ExecuteNonQuery();
        //                if (cm.Parameters["@MsgID"].Value == null)
        //                    msgID = string.Empty;
        //                else
        //                    msgID = cm.Parameters["@MsgID"].Value.ToString();

        //                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //                    retValue = true;
        //            }// Already close and dispose sql command.
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        #endregion Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
                                    
            return retValue;
        }

        //internal bool Update()
        //{
        //    bool retValue = false;
        //    msgID = "RecordUpdateFail";
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            //Create new sql connection for this method. 
        //            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //            {
        //                // Open sql connection. 
        //                cn.Open();
        //                retValue = this.Update(cn);
        //            }
        //            // No errors - commit transaction
        //              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //        }// Already close and dispose sql connection.
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;            
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "REFBrandDetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);                      

                    cm.Parameters.AddWithValue("@BrandKey", dr["BrandKey"]);
                    cm.Parameters.AddWithValue("@Model", dr["Model"]);
                    cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"]);
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

                  
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;                        
                    }
                    else
                        retValue=false;
                }
            }// Already close and dispose sql command.
            
            return retValue;
        }

        //internal bool Update(SqlConnection cn)
        //{
        //    bool retValue = false;
        //    msgID = "RecordUpdateFail";
        //    try
        //    {
        //        foreach (DataRow dr in this.Rows)
        //        {
        //            using (SqlCommand cm = cn.CreateCommand())
        //            {
        //                cm.CommandType = CommandType.StoredProcedure;
        //                cm.CommandText = "REFBrandDetItm_AddUpdate";

        //                cm.Parameters.AddWithValue("@Option", 1);
        //                

        //                cm.Parameters.AddWithValue("@BrandKey", dr["BrandKey"]);                        
        //                cm.Parameters.AddWithValue("@Model", dr["Model"]);
        //                cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"]);
        //                cm.Parameters.AddWithValue("@CreateUserKey", dr["CreateUserKey"]);
        //                cm.Parameters.AddWithValue("@LastModifiedDate", dr["LastModifiedDate"]);
        //                cm.Parameters.AddWithValue("@LastModifiedUserKey", dr["LastModifiedUserKey"]);
        //                cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
        //                cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
        //                cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
        //                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
        //                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
        //                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
        //                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

        //                
        //                cm.Parameters.AddWithValue("@RetValue", 0);
        //                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
        //                // cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
        //                // Execute command.
        //                cm.ExecuteNonQuery();
        //                if (cm.Parameters["@MsgID"].Value == null)
        //                    msgID = string.Empty;
        //                else
        //                    msgID = cm.Parameters["@MsgID"].Value.ToString();

        //                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //                    retValue = true;
        //            }
        //        }// Already close and dispose sql command.
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        #endregion Update

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

        //internal bool Delete(Criteria criteria)
        //{
        //    bool retValue = false;
        //    msgID = "RecordDeleteFail";
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            //Create new sql connection for this method. 
        //            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        //            {
        //                // Open sql connection. 
        //                cn.Open();
        //                retValue = this.Delete(cn, criteria);
        //            }
        //            // No errors - commit transaction
        //              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        //        }// Already close and dispose sql connection.
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrandDetItm_Delete";
       
                cm.Parameters.AddWithValue("@BrandKey", criteria._headerKey);                 

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

        //internal bool Delete(SqlConnection cn, Criteria criteria)
        //{
        //    bool retValue = false;
        //    msgID = "RecordDeleteFail";
        //    try
        //    {
        //        using (SqlCommand cm = cn.CreateCommand())
        //        {
        //            cm.CommandType = CommandType.StoredProcedure;
        //            cm.CommandText = "REFBrandDetItm_Delete";

        //            
        //            cm.Parameters.AddWithValue("@BrandKey", criteria._headerKey);

        //            

        //            // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
        //            cm.Parameters.AddWithValue("@RetValue", 0);
        //            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
        //            // Execute command.
        //            cm.ExecuteNonQuery();

        //            if (cm.Parameters["@MsgID"].Value == null)
        //                msgID = string.Empty;
        //            else
        //                msgID = cm.Parameters["@MsgID"].Value.ToString();

        //            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
        //                retValue = true;
        //        }// Already close and dispose sql command.
        //    }
        //    catch (Exception ex)
        //    {
        //        retValue = false;
        //    }
        //    return retValue;
        //}

        #endregion Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = Validation(cn, criteria, isNew);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria,bool isNew)
        {            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFBrandDetItm_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);                  
                cm.Parameters.AddWithValue("@BrandKey", criteria._headerKey);
                
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

        #endregion Validation
    }
}
