

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTPriceListDetRatios : DataTable
    {
       #region Factory Methods

        public MSTPriceListDetRatios()
        {
            
            this.Fetch(new Criteria(0,1));          
        }

        public MSTPriceListDetRatios(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        internal static MSTPriceListDetRatios New()
        {
            
            MSTPriceListDetRatios obj = new MSTPriceListDetRatios();
            
            return obj;
        }

        internal static MSTPriceListDetRatios New(SqlConnection cn)
        {

            MSTPriceListDetRatios obj = new MSTPriceListDetRatios(cn);

            return obj;
        }

        public static MSTPriceListDetRatios Get( int? headerKey)
        {
           
            MSTPriceListDetRatios obj = new MSTPriceListDetRatios();
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
                cm.CommandText = "MSTPriceListDetRatio_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@PriceKey", criteria._headerKey);
                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                    retValue = this.Insert(cn, headerKey );
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
                    cm.CommandText = "MSTPriceListDetRatio_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@PriceKey", headerKey);
                    cm.Parameters.AddWithValue("@Cat1", dr["Cat1"].ToString() == "" ? 0 : dr["Cat1"]);
                    cm.Parameters.AddWithValue("@Cat2", dr["Cat2"].ToString() == "" ? 0 : dr["Cat2"]);
                    cm.Parameters.AddWithValue("@Cat3", dr["Cat3"].ToString() == "" ? 0 : dr["Cat3"]);
                    cm.Parameters.AddWithValue("@Cat4", dr["Cat4"].ToString() == "" ? 0 : dr["Cat4"]);
                    cm.Parameters.AddWithValue("@Cat5", dr["Cat5"].ToString() == "" ? 0 : dr["Cat5"]);
                    cm.Parameters.AddWithValue("@RatioType", dr["RatioType"].ToString() == "" ? 0 : dr["RatioType"]);
                    cm.Parameters.AddWithValue("@Percentage", dr["Percentage"].ToString() == "" ? 0 : dr["Percentage"]);
                    cm.Parameters.AddWithValue("@Ratio", dr["Ratio"].ToString() == "" ? 1 : dr["Ratio"]);
                    cm.Parameters.AddWithValue("@EffStartDate", dr["EffStartDate"]);
                    cm.Parameters.AddWithValue("@EffEndDate", dr["EffEndDate"]);
                    cm.Parameters.AddWithValue("@EffPercentage", dr["EffPercentage"].ToString() == "" ? 0 : dr["EffPercentage"]);
                    cm.Parameters.AddWithValue("@EffRatio", dr["EffRatio"].ToString() == "" ? 1 : dr["EffRatio"]);
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
                    cm.CommandText = "MSTPriceListDetRatio_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@PriceKey", dr["PriceKey"]);
                    cm.Parameters.AddWithValue("@Cat1", dr["Cat1"].ToString() == "" ? 0 : dr["Cat1"]);
                    cm.Parameters.AddWithValue("@Cat2", dr["Cat2"].ToString() == "" ? 0 : dr["Cat2"]);
                    cm.Parameters.AddWithValue("@Cat3", dr["Cat3"].ToString() == "" ? 0 : dr["Cat3"]);
                    cm.Parameters.AddWithValue("@Cat4", dr["Cat4"].ToString() == "" ? 0 : dr["Cat4"]);
                    cm.Parameters.AddWithValue("@Cat5", dr["Cat5"].ToString() == "" ? 0 : dr["Cat5"]);
                    cm.Parameters.AddWithValue("@RatioType", dr["RatioType"].ToString() == "" ? 0 : dr["RatioType"]);
                    cm.Parameters.AddWithValue("@Percentage", dr["Percentage"].ToString() == "" ? 0 : dr["Percentage"]);
                    cm.Parameters.AddWithValue("@Ratio", dr["Ratio"].ToString() == "" ? 1 : dr["Ratio"]);
                    cm.Parameters.AddWithValue("@EffStartDate", dr["EffStartDate"]);
                    cm.Parameters.AddWithValue("@EffEndDate", dr["EffEndDate"]);
                    cm.Parameters.AddWithValue("@EffPercentage", dr["EffPercentage"].ToString() == "" ? 0 : dr["EffPercentage"]);
                    cm.Parameters.AddWithValue("@EffRatio", dr["EffRatio"].ToString() == "" ? 1 : dr["EffRatio"]);
                    cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"].ToString());
                    cm.Parameters.AddWithValue("@CreateUserKey", dr["CreateUserKey"]);
                    cm.Parameters.AddWithValue("@LastModifiedDate", dr["LastModifiedDate"].ToString());
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
                cm.CommandText = "MSTPriceListDetRatio_Delete";
                cm.Parameters.AddWithValue("@PriceKey", criteria._headerKey);
                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }// Already close and dispose sql command.            
        }

        #endregion Delete

    }
}

