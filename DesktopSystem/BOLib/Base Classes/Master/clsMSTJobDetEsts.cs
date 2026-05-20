using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTJobDetEsts : DataTable
    {

        #region Factory Methods

        
        public MSTJobDetEsts()
        {
            this.Fetch(new Criteria(0, 1));
        }

        public MSTJobDetEsts(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }     

        public static MSTJobDetEsts Get(int? headerKey)
        {
            
            MSTJobDetEsts obj = new MSTJobDetEsts();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }
        public static MSTJobDetEsts Get(int? headerKey,int Option)
        {

            MSTJobDetEsts obj = new MSTJobDetEsts();
            obj.Fetch(new Criteria(headerKey, Option));
            return obj;
        }
        public static MSTJobDetEsts New()
        {
            
            MSTJobDetEsts obj = new MSTJobDetEsts();
            return obj;
        }
        public static MSTJobDetEsts New(SqlConnection cn )
        {
            
            MSTJobDetEsts obj = new MSTJobDetEsts(cn);
            
           
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

        internal bool Fetch(SqlConnection cn, Criteria criteria )
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJobDetEst_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@JobKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@JobEstKey", 0);
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

        internal bool Insert(Criteria _criteria )
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
                    cm.CommandText = "MSTJobDetEst_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@JobKey", _criteria._headerKey);
                    cm.Parameters.AddWithValue("@JobEstKey", dr["JobEstKey"]);
                    cm.Parameters.AddWithValue("@JobPhaseKey", dr["JobPhaseKey"].ToString() == "" ? 0 : dr["JobPhaseKey"]);
                    cm.Parameters.AddWithValue("@JobTaskKey", dr["JobTaskKey"].ToString() == "" ? 0 : dr["JobTaskKey"]);
                    cm.Parameters.AddWithValue("@JobCostTypeKey", dr["JobCostTypeKey"].ToString() == "" ? 0 : dr["JobCostTypeKey"]);
                    cm.Parameters.AddWithValue("@EstSN", dr["EstSN"].ToString() == "" ? 1 : dr["EstSN"]);
                    cm.Parameters.AddWithValue("@EstItmKey", dr["EstItmKey"]);
                    cm.Parameters.AddWithValue("@EstItmKeySelect", dr["EstItmKeySelect"]);
                    cm.Parameters.AddWithValue("@EstItmDes", dr["EstItmDes"]);
                    cm.Parameters.AddWithValue("@EstItmType", dr["EstItmType"]);
                    cm.Parameters.AddWithValue("@EstItmRem", dr["EstItmRem"]);
                    cm.Parameters.AddWithValue("@EstQty", dr["EstQty"]);
                    cm.Parameters.AddWithValue("@EstUOMKey", dr["EstUOMKey"]);
                    cm.Parameters.AddWithValue("@EstConRate", dr["EstConRate"]);
                    cm.Parameters.AddWithValue("@EstCostF", GFunc.NEDec(dr["EstCostF"],0));
                    cm.Parameters.AddWithValue("@EstCostH", GFunc.NEDec(dr["EstCostH"],0));
                    cm.Parameters.AddWithValue("@EstAmtF", GFunc.NEDec(dr["EstAmtF"], 0));
                    cm.Parameters.AddWithValue("@EstAmtH", GFunc.NEDec(dr["EstAmtH"], 0));
                    cm.Parameters.AddWithValue("@DocDK", dr["DocDK"]);
                    cm.Parameters.AddWithValue("@DocDItm", dr["DocDItm"]);
                    cm.Parameters.AddWithValue("@DocID", dr["DocID"]);
                    cm.Parameters.AddWithValue("@DocDes", dr["DocDes"]);
                    cm.Parameters.AddWithValue("@DocVendorKey", dr["DocVendorKey"]);
                    cm.Parameters.AddWithValue("@DocCurrKey",GFunc.NEInt(dr["DocCurrKey"],1));
                    cm.Parameters.AddWithValue("@DocCurrRate",GFunc.NEDec( dr["DocCurrRate"],1));
                    cm.Parameters.AddWithValue("@DocETD", dr["DocETD"]);
                    cm.Parameters.AddWithValue("@TransmitMode", dr["TransmitMode"].ToString() == "" ? 0 : dr["TransmitMode"]);
                    cm.Parameters.AddWithValue("@Attention", dr["Attention"]);
                    cm.Parameters.AddWithValue("@emailAddr", dr["emailAddr"]);
                    cm.Parameters.AddWithValue("@FaxNumber", dr["FaxNumber"]);
                    cm.Parameters.AddWithValue("@TransmitStatus", dr["TransmitStatus"].ToString());
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.Add("@PrjCostRate",SqlDbType.Decimal);
                    cm.Parameters["@PrjCostRate"].Precision = 6;
                    cm.Parameters["@PrjCostRate"].Scale = 4;
                    cm.Parameters["@PrjCostRate"].Value = dr["PrjCostRate"];
                    cm.Parameters.AddWithValue("@PrjCost", dr["PrjCost"]);
                    cm.Parameters.AddWithValue("@ItmStock", dr["ItmStock"]);
                    cm.Parameters.AddWithValue("@Selected", dr["Selected"]);
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

        internal bool Save(SqlConnection cn,int? JobKey, string JobID, string xmlEst)
        {           

            if (this.Rows.Count == 0)
            {
                return true;
            }
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJobDetEst_Save";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@JobKey", JobKey);
                cm.Parameters.AddWithValue("@JobID", JobID);
                cm.Parameters.AddWithValue("@xmlMST_JobDetEst", xmlEst);

                // Execute command.
                cm.ExecuteNonQuery();

            }// Already close and dispose sql command.

            return true;
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
                    retValue = this.Update(cn, _criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Update(SqlConnection cn, Criteria _criteria )
        {
            bool retValue = false;
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTJobDetEst_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@JobKey", _criteria._headerKey);
                    cm.Parameters.AddWithValue("@JobEstKey", dr["JobEstKey"]);
                    cm.Parameters.AddWithValue("@JobPhaseKey", dr["JobPhaseKey"].ToString() == "" ? 0 : dr["JobPhaseKey"]);
                    cm.Parameters.AddWithValue("@JobTaskKey", dr["JobTaskKey"].ToString() == "" ? 0 : dr["JobTaskKey"]);
                    cm.Parameters.AddWithValue("@JobCostTypeKey", dr["JobCostTypeKey"].ToString() == "" ? 0 : dr["JobCostTypeKey"]);
                    cm.Parameters.AddWithValue("@EstSN", dr["EstSN"].ToString() == "" ? 1 : dr["EstSN"]);
                    cm.Parameters.AddWithValue("@EstItmKey", dr["EstItmKey"]);
                    cm.Parameters.AddWithValue("@EstItmKeySelect", dr["EstItmKeySelect"]);
                    cm.Parameters.AddWithValue("@EstItmDes", dr["EstItmDes"]);
                    cm.Parameters.AddWithValue("@EstItmType", dr["EstItmType"]);
                    cm.Parameters.AddWithValue("@EstItmRem", dr["EstItmRem"]);
                    cm.Parameters.AddWithValue("@EstQty", dr["EstQty"]);
                    cm.Parameters.AddWithValue("@EstUOMKey", dr["EstUOMKey"]);
                    cm.Parameters.AddWithValue("@EstConRate", dr["EstConRate"]);
                    cm.Parameters.AddWithValue("@EstCostF", dr["EstCostF"].ToString() == "" ? 0 : dr["EstCostF"]);
                    cm.Parameters.AddWithValue("@EstCostH", dr["EstCostH"].ToString() == "" ? 0 : dr["EstCostH"]);
                    cm.Parameters.AddWithValue("@EstAmtF", dr["EstAmtF"].ToString() == "" ? 0 : dr["EstAmtF"]);
                    cm.Parameters.AddWithValue("@EstAmtH", dr["EstAmtH"].ToString() == "" ? 0 : dr["EstAmtH"]);
                    cm.Parameters.AddWithValue("@DocDK", dr["DocDK"]);
                    cm.Parameters.AddWithValue("@DocDItm", dr["DocDItm"]);
                    cm.Parameters.AddWithValue("@DocID", dr["DocID"]);
                    cm.Parameters.AddWithValue("@DocDes", dr["DocDes"]);
                    cm.Parameters.AddWithValue("@DocVendorKey", dr["DocVendorKey"]);
                    cm.Parameters.AddWithValue("@DocCurrKey", dr["DocCurrKey"].ToString() == "" ? 1 : dr["DocCurrKey"]);
                    cm.Parameters.AddWithValue("@DocCurrRate", dr["DocCurrRate"].ToString() == "" ? 1 : dr["DocCurrRate"]);
                    cm.Parameters.AddWithValue("@DocETD", dr["DocETD"]);
                    cm.Parameters.AddWithValue("@TransmitMode", dr["TransmitMode"].ToString() == "" ? 0 : dr["TransmitMode"]);
                    cm.Parameters.AddWithValue("@Attention", dr["Attention"]);
                    cm.Parameters.AddWithValue("@emailAddr", dr["emailAddr"]);
                    cm.Parameters.AddWithValue("@FaxNumber", dr["FaxNumber"]);
                    cm.Parameters.AddWithValue("@TransmitStatus", dr["TransmitStatus"].ToString() == "" ? 0 : dr["TransmitStatus"]);
                    cm.Parameters.AddWithValue("@CreateDate", dr["CreateDate"].ToString());
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
                cm.CommandText = "MSTJobDetEst_Delete";
                cm.Parameters.AddWithValue("@JobKey", criteria._headerKey);
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
