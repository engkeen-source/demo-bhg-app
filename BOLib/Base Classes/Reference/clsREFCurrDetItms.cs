using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFCurrDetItms : DataTable
    {

        #region Factory Methods

        public REFCurrDetItms()
        {            
            this.Fetch(new Criteria(0,1));          
        }
        
        public REFCurrDetItms(SqlConnection cn)
        {
            
            this.Fetch(cn, new Criteria(0, 1));
        }     

        public static REFCurrDetItms Get(int? currKey)
        {
            REFCurrDetItms obj = new REFCurrDetItms();
            obj.Fetch(new Criteria(currKey, 1));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _currKey = 0;
            public DateTime? _currDate = null;
            public int? _option = 0;

            internal Criteria()
            {
            }

            internal Criteria(int? CurrKey, int? Option)
            {
                _currKey = CurrKey;                
                _option = Option;
            }
            internal Criteria(int? CurrKey, DateTime? CurrDate, int? Option)
            {
                _currKey = CurrKey;
                _currDate = CurrDate;
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
                cm.CommandText = "REFCurrDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);
                
                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }//using            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

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
                    cm.CommandText = "REFCurrDetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);

                    cm.Parameters.AddWithValue("@CurrKey", headerKey);
                    cm.Parameters.AddWithValue("@CurrDate", dr["CurrDate"]);
                    cm.Parameters.AddWithValue("@CurrRate", dr["CurrRate"].ToString() == "" ? 1 : dr["CurrRate"]);
                    cm.Parameters.AddWithValue("@CountryRate", dr["CountryRate"].ToString() == "" ? 1 : dr["CountryRate"]);
                   
                    cm.Parameters.AddWithValue("@CustomRate1", dr["CustomRate1"].ToString() == "" ? 1 : dr["CustomRate1"]);
                   
                    cm.Parameters.AddWithValue("@CustomRate2", dr["CustomRate2"].ToString() == "" ? 1 : dr["CustomRate2"]);
                    
                    cm.Parameters.AddWithValue("@CustomRate3", dr["CustomRate3"].ToString() == "" ? 1 : dr["CustomRate3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);                        

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                    {
                        retValue=false;
                    }
                }// Already close and dispose sql command.
            }
            
            return retValue;
        }
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
        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;
            
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "ARDODetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@CurrKey", dr["CurrKey"]);
                    cm.Parameters.AddWithValue("@CurrDate", dr["CurrDate"]);
                    cm.Parameters.AddWithValue("@CurrRate", dr["CurrRate"]);
                    cm.Parameters.AddWithValue("@CountryRate", dr["CountryRate"]);
                    cm.Parameters.AddWithValue("@CustomRate1", dr["CustomRate1"]);
                    cm.Parameters.AddWithValue("@CustomRate2", dr["CustomRate2"]);
                    cm.Parameters.AddWithValue("@CustomRate3", dr["CustomRate3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);    

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
                    {
                        retValue = false;
                    }
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
                cm.CommandText = "REFCurrDetItm_Delete";

                cm.Parameters.AddWithValue("@CurrKey", criteria._currKey);

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