using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using System.Collections.Generic;

namespace BOLib
{
    [Serializable()]
    public class MSTItmLocOpenBals : DataTable
    {

        #region Factory Methods

         
        public MSTItmLocOpenBals()
        {
            this.Fetch(new Criteria(0,2));          
        }        

        public MSTItmLocOpenBals(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 2));
        }       

        public static MSTItmLocOpenBals Get( int? headerKey)
        {           
            MSTItmLocOpenBals obj = new MSTItmLocOpenBals();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static MSTItmLocOpenBals New()
        {
         
            MSTItmLocOpenBals obj = new MSTItmLocOpenBals();
            return obj;
        }
        public static MSTItmLocOpenBals New(SqlConnection cn)
        {           
            MSTItmLocOpenBals obj = new MSTItmLocOpenBals(cn);
            obj.Fetch(cn,new Criteria(0, 1));         
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _option = null;
            public int? _DocCodeKey = null;
            public string _BatchID = string.Empty;
            public string _xmlData = string.Empty;
            public int? _ItmType = null;


            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey,int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;
               
            }
            internal Criteria(int? DocCodeKey, string BatchID)
            {
                _DocCodeKey = DocCodeKey;
                _BatchID = BatchID;
            }
            internal Criteria(int? HeaderKey, int? ItmType, string xmlData, int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;
                _xmlData = xmlData;
                _ItmType = ItmType;
            }
            internal Criteria(int? DocCodeKey, int? HeaderKey, int? ItmType, int? Option, string xmlData )
            {
                _DocCodeKey = DocCodeKey;
                _ItmType = ItmType;
                _headerKey = HeaderKey;
                _option = Option;
                _xmlData = xmlData;               
                
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
                cm.CommandText = "MSTItmLocOpenBal_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey); 

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
                    retValue = this.Insert(cn, _criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        
        internal bool Insert(SqlConnection cn, Criteria _criteria)
        {
            bool retValue = false;
            
            if (this.Rows.Count == 0)
            {
                return true;
            }
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmLocOpenBal_AddUpdate";

                cm.Parameters.AddWithValue("@Option", _criteria._option);             
                cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@ItmKey", _criteria._headerKey);
                cm.Parameters.AddWithValue("@ItmType", _criteria._ItmType);
                cm.Parameters.AddWithValue("@XmlData", _criteria._xmlData);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                    retValue = false;
            }// Already close and dispose sql command.
           
               
            
            
            return retValue;
        }

        #endregion Insert

        #region Data Access - Update

        internal bool Update(Criteria _criteria)
        {
            bool retValue = false;
            
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
            
            return retValue;
        }

        internal bool Update(SqlConnection cn,Criteria _criteria)
        {
            bool retValue = false;
            
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTItmLocOpenBal_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);

                    cm.Parameters.AddWithValue("@ItmKey", dr["ItmKey"]);
                    cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"]);
                    cm.Parameters.AddWithValue("@LocKey", dr["LocKey"]);
                    cm.Parameters.AddWithValue("@DatePurchase", dr["DatePurchase"]);
                    cm.Parameters.AddWithValue("@Qty", dr["Qty"].ToString() == "" ? 0 : dr["Qty"]);
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
                    else
                        retValue = false;
                }
            }// Already close and dispose sql command.
            
            return retValue;
        }

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

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmLocOpenBal_Delete";
                cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey);
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

        #region Data Access - Validation

        internal bool Validation(Criteria criteria)
        {
            bool retValue = false;
            
            //using (TransactionScope scope = new TransactionScope())
            //{
            //    //Create new sql connection for this method. 
            //    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            //    {
            //        // Open sql connection. 
            //        cn.Open();
            //        retValue = Validation(cn, criteria,IsNotEnoughQty, IsNotEnoughLocQty, IsExistInOtherTrans);
            //    }
            //    // No errors - commit transaction
            //      if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            //}// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, ref DataTable errResult, ref int InsufficientStockQty, ref int InsufficientLocQty, ref int DuplicateLocID, ref int InsufficientBatchQty, ref int DuplicateBatchID)
        {
            DataTable  ds = new DataTable();
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmLocOpenBal_Validation";

                cm.Parameters.AddWithValue("@DocCodeKey", criteria._DocCodeKey);                
                cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@ItmType", criteria._ItmType);
                cm.Parameters.AddWithValue("@XmlData", criteria._xmlData);
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters.AddWithValue("@InsufficientStockQty", 0);
                cm.Parameters.AddWithValue("@InsufficientLocQty", 0);
                cm.Parameters.AddWithValue("@DuplicateLocID", 0);
                cm.Parameters.AddWithValue("@InsufficientBatchQty", 0);
                cm.Parameters.AddWithValue("@DuplicateBatchID", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@InsufficientStockQty"].Direction = ParameterDirection.Output;
                cm.Parameters["@InsufficientLocQty"].Direction = ParameterDirection.Output;
                cm.Parameters["@DuplicateLocID"].Direction = ParameterDirection.Output;
                cm.Parameters["@InsufficientBatchQty"].Direction = ParameterDirection.Output;
                cm.Parameters["@DuplicateBatchID"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(ds);

                errResult = ds;
                InsufficientStockQty = GFunc.NEInt(cm.Parameters["@InsufficientStockQty"].Value, 0);
                InsufficientLocQty = GFunc.NEInt(cm.Parameters["@InsufficientLocQty"].Value, 0);
                DuplicateLocID = GFunc.NEInt(cm.Parameters["@DuplicateLocID"].Value, 0);
                InsufficientBatchQty = GFunc.NEInt(cm.Parameters["@InsufficientBatchQty"].Value, 0);
                DuplicateBatchID = GFunc.NEInt(cm.Parameters["@DuplicateBatchID"].Value, 0);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;


            }// Already close and dispose sql command.
            
        }

        #endregion Validation
    }
}
