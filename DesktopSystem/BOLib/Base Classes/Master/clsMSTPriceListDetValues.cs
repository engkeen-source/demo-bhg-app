

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class MSTPriceListDetValues : DataTable
    {

      #region Factory Methods

        public MSTPriceListDetValues()
        {
            
            this.Fetch(new Criteria(0,1));          
        }

        public MSTPriceListDetValues(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        internal static MSTPriceListDetValues New()
        {
            
            MSTPriceListDetValues obj = new MSTPriceListDetValues();
            
            return obj;
        }

        internal static MSTPriceListDetValues New(SqlConnection cn)
        {

            MSTPriceListDetValues obj = new MSTPriceListDetValues(cn);

            return obj;
        }

        public static MSTPriceListDetValues Get( int? headerKey)
        {
            
            MSTPriceListDetValues obj = new MSTPriceListDetValues();
            obj.Fetch(new Criteria(headerKey, 1));
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
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw (ex);
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
                    cm.CommandText = "MSTPriceListDetValue_Get";

                    cm.Parameters.AddWithValue("@Option", criteria._option);

                    cm.Parameters.AddWithValue("@ItmKey",0);

                    cm.Parameters.AddWithValue("@PriceKey", criteria._headerKey);
                    

                    

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
                    {
                        retValue = true;
                    }
                    else
                    {
                        throw new TAException(MsgID.Common.GetFail);
                    }


                }//using
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
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
                    cm.CommandText = "MSTPriceListDetValue_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@PriceKey", headerKey);
                    cm.Parameters.AddWithValue("@ItmKey", dr["ItmKey"]);
                    cm.Parameters.AddWithValue("@ItmType", dr["ItmType"]);
                    cm.Parameters.AddWithValue("@ItmDes", dr["ItmDes"]);
                    cm.Parameters.AddWithValue("@ItmQty", dr["ItmQty"]);
                    cm.Parameters.AddWithValue("@ItmPrice", dr["ItmPrice"].ToString() == "" ? 0 : dr["ItmPrice"]);
                    cm.Parameters.AddWithValue("@CustomPrice", dr["CustomPrice"].ToString() == "" ? 0 : dr["CustomPrice"]);
                    cm.Parameters.AddWithValue("@LastUpdatedDate", dr["LastUpdatedDate"]);
                    cm.Parameters.AddWithValue("@IgnorePriceUpdate", dr["IgnorePriceUpdate"].ToString() == "" ? 0 : dr["IgnorePriceUpdate"]);
                    cm.Parameters.AddWithValue("@EffStartDate", dr["EffStartDate"]);
                    cm.Parameters.AddWithValue("@EffEndDate", dr["EffEndDate"]);
                    cm.Parameters.AddWithValue("@EffItmQty", dr["EffItmQty"]);
                    cm.Parameters.AddWithValue("@EffItmPrice", dr["EffItmPrice"].ToString() == "" ? 0 : dr["EffItmPrice"]);
                    cm.Parameters.AddWithValue("@VendorKey", dr["VendorKey"]);
                    cm.Parameters.AddWithValue("@VendorPrice", dr["VendorPrice"]);
                    cm.Parameters.AddWithValue("@VendorItmDes", dr["VendorItmDes"]);
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
                    {
                        retValue = true;
                    }
                    else
                    {
                        retValue = false;
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
                    cm.CommandText = "MSTPriceListDetValue_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 1); 
                    cm.Parameters.AddWithValue("@PriceKey", dr["PriceKey"]);
                    cm.Parameters.AddWithValue("@ItmKey", dr["ItmKey"]);
                    cm.Parameters.AddWithValue("@ItmType", dr["ItmType"]);
                    cm.Parameters.AddWithValue("@ItmDes", dr["ItmDes"]);
                    cm.Parameters.AddWithValue("@ItmQty", dr["ItmQty"]);
                    cm.Parameters.AddWithValue("@ItmPrice", dr["ItmPrice"].ToString() == "" ? 0 : dr["ItmPrice"]);
                    cm.Parameters.AddWithValue("@CustomPrice", dr["CustomPrice"].ToString() == "" ? 0 : dr["CustomPrice"]);
                    cm.Parameters.AddWithValue("@LastUpdatedDate", dr["LastUpdatedDate"]);
                    cm.Parameters.AddWithValue("@IgnorePriceUpdate", dr["IgnorePriceUpdate"].ToString() == "" ? 0 : dr["IgnorePriceUpdate"]);
                    cm.Parameters.AddWithValue("@EffStartDate", dr["EffStartDate"]);
                    cm.Parameters.AddWithValue("@EffEndDate", dr["EffEndDate"]);
                    cm.Parameters.AddWithValue("@EffItmQty", dr["EffItmQty"]);
                    cm.Parameters.AddWithValue("@EffItmPrice", dr["EffItmPrice"].ToString() == "" ? 0 : dr["EffItmPrice"]);
                    cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"].ToString());
                    cm.Parameters.AddWithValue("@CreateUserKey", dr["CreateUserKey"]);
                    cm.Parameters.AddWithValue("@LastModifiedDate", dr["LastModifiedDate"].ToString() );
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", dr["LastModifiedUserKey"]);
                    cm.Parameters.AddWithValue("@VendorKey", dr["VendorKey"]);
                    cm.Parameters.AddWithValue("@VendorPrice", dr["VendorPrice"]);
                    cm.Parameters.AddWithValue("@VendorItmDes", dr["VendorItmDes"]);
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
            bool retValue = false;
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTPriceListDetValue_Delete";                    
                cm.Parameters.AddWithValue("@PriceKey", criteria._headerKey);
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

