using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTJobDetOthers : DataTable
    {

        #region Factory Methods

        public MSTJobDetOthers()
        {
            this.Fetch(new Criteria(0, 1));
        }       

        public MSTJobDetOthers(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }      

        public static MSTJobDetOthers Get( int? headerKey)
        {
            
            MSTJobDetOthers obj = new MSTJobDetOthers();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static MSTJobDetOthers New()
        {            
            MSTJobDetOthers obj = new MSTJobDetOthers();
            return obj;
        }
        public static MSTJobDetOthers New(SqlConnection cn)
        {
            
            MSTJobDetOthers obj = new MSTJobDetOthers(cn);
            obj.Fetch(cn,new Criteria (0,1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobKey = null;
            public int? _emKey = null;
            public int? _supervisor = null;
            public int? _costGrp = null;
            public int? _option = null;
            public DateTime? _docDate = null;
            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _jobKey = HeaderKey;
                _option = Option;
            }
            internal Criteria(int? EmKey, DateTime? DocDate, int? Option)
            {
                _emKey = EmKey;
                _docDate = DocDate;
                _option = Option;
            }
            

            internal Criteria(int? EmKey,int? supervisor,int? costGrp,  int? Option)
            {
                _emKey = EmKey;
                _supervisor = supervisor;
                _costGrp = costGrp;
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
                cm.CommandText = "MSTJobDetOther_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
                cm.Parameters.AddWithValue("@EmKey", criteria._emKey);
                cm.Parameters.AddWithValue("@DocDate", criteria._docDate);
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

        internal bool FetchFromTimeSheet(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJobDetOther_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
                cm.Parameters.AddWithValue("@JobOtherKey", 0);
                cm.Parameters.AddWithValue("@EmKey", criteria._emKey);
                if (GFunc.IsNE(criteria._docDate))
                    criteria._docDate = DateTime.Now;
                else
                    cm.Parameters.AddWithValue("@DocDate", criteria._docDate);
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }//using            
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(Criteria _criteria)
        {
            bool retValue = false;
            
            
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, _criteria);
                }
                
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
                    cm.CommandText = "MSTJobDetOther_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@JobKey", _criteria._jobKey);
                    cm.Parameters.AddWithValue("@JobOtherKey", dr["JobOtherKey"]);
                    cm.Parameters.AddWithValue("@JobPhaseKey", dr["JobPhaseKey"].ToString() == "" ? 0 : dr["JobPhaseKey"]);
                    cm.Parameters.AddWithValue("@JobTaskKey", dr["JobTaskKey"].ToString() == "" ? 0 : dr["JobTaskKey"]);
                    cm.Parameters.AddWithValue("@JobCostTypeKey", dr["JobCostTypeKey"].ToString() == "" ? 0 : dr["JobCostTypeKey"]);
                    cm.Parameters.AddWithValue("@OthLineType", dr["OthLineType"]);
                    cm.Parameters.AddWithValue("@Supervisor", dr["Supervisor"]);
                    cm.Parameters.AddWithValue("@EMKey", dr["EMKey"]);
                    cm.Parameters.AddWithValue("@CostGrp", dr["CostGrp"]);
                    cm.Parameters.AddWithValue("@OthItmKey", dr["OthItmKey"]);
                    cm.Parameters.AddWithValue("@OthItmType", dr["OthItmType"]);
                    cm.Parameters.AddWithValue("@OthItmKeySelect", dr["OthItmKeySelect"]);
                    cm.Parameters.AddWithValue("@OthItmDes", dr["OthItmDes"]);
                    cm.Parameters.AddWithValue("@OthItmRem", dr["OthItmRem"]);
                    cm.Parameters.AddWithValue("@OthQty", dr["OthQty"]);
                    cm.Parameters.AddWithValue("@OthUOMKey", dr["OthUOMKey"]);
                    cm.Parameters.AddWithValue("@OthConRate", dr["OthConRate"]);
                    cm.Parameters.AddWithValue("@OthPriceF", dr["OthPriceF"]);
                    cm.Parameters.AddWithValue("@OthPriceH", dr["OthPriceH"]);
                    cm.Parameters.AddWithValue("@OthExpAmtF", dr["OthExpAmtF"].ToString() == "" ? 0 : dr["OthExpAmtF"]);
                    cm.Parameters.AddWithValue("@OthExpAmtH", dr["OthExpAmtH"].ToString() == "" ? 0 : dr["OthExpAmtH"]);
                    cm.Parameters.AddWithValue("@OthRevAmtF", dr["OthRevAmtF"].ToString() == "" ? 0 : dr["OthRevAmtF"]);
                    cm.Parameters.AddWithValue("@OthRevAmtH", dr["OthRevAmtH"].ToString() == "" ? 0 : dr["OthRevAmtH"]);
                    cm.Parameters.AddWithValue("@OthPaidAmtF", dr["OthPaidAmtF"].ToString() == "" ? 0 : dr["OthPaidAmtF"]);
                    cm.Parameters.AddWithValue("@OthPaidAmtH", dr["OthPaidAmtH"].ToString() == "" ? 0 : dr["OthPaidAmtH"]);
                    cm.Parameters.AddWithValue("@DocID", dr["DocID"]);
                    cm.Parameters.AddWithValue("@DocDate", dr["DocDate"]);
                    cm.Parameters.AddWithValue("@DocDes", dr["DocDes"]);
                    cm.Parameters.AddWithValue("@DocCurrKey", dr["DocCurrKey"].ToString() == "" ? 1 : dr["DocCurrKey"]);
                    cm.Parameters.AddWithValue("@DocCurrRate", dr["DocCurrRate"].ToString() == "" ? 1 : dr["DocCurrRate"]);
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
                        retValue = true;
                    }  

                }// Already close and dispose sql command.
            }
            
            return retValue;
        }

        internal bool InsertFromTimeSheet(SqlConnection cn, Criteria _criteria)
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
                    cm.CommandText = "MSTJobDetOther_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@JobKey", dr["JobKey"]);
                    cm.Parameters.AddWithValue("@JobOtherKey", dr["JobOtherKey"]);
                    cm.Parameters.AddWithValue("@JobPhaseKey", dr["JobPhaseKey"].ToString() == "" ? 0 : dr["JobPhaseKey"]);
                    cm.Parameters.AddWithValue("@JobTaskKey", dr["JobTaskKey"].ToString() == "" ? 0 : dr["JobTaskKey"]);
                    cm.Parameters.AddWithValue("@JobCostTypeKey", dr["JobCostTypeKey"].ToString() == "" ? 0 : dr["JobCostTypeKey"]);
                    cm.Parameters.AddWithValue("@OthLineType", 10);
                    //cm.Parameters.AddWithValue("@OthLineType", dr["OthLineType"]);
                    cm.Parameters.AddWithValue("@Supervisor", _criteria._supervisor );
                    cm.Parameters.AddWithValue("@EMKey", _criteria._emKey);
                    cm.Parameters.AddWithValue("@CostGrp", _criteria._costGrp);
                    cm.Parameters.AddWithValue("@OthItmKey", dr["OthItmKey"]);                    
                    cm.Parameters.AddWithValue("@OthItmKeySelect", dr["OthItmKeySelect"]);
                    cm.Parameters.AddWithValue("@OthItmDes", dr["OthItmDes"]);
                    cm.Parameters.AddWithValue("@OthItmType", dr["OthItmType"]);
                    cm.Parameters.AddWithValue("@OthItmRem", dr["OthItmRem"]);
                    cm.Parameters.AddWithValue("@OthQty", dr["OthQty"]);
                    cm.Parameters.AddWithValue("@OthUOMKey", dr["OthUOMKey"]);
                    cm.Parameters.AddWithValue("@OthConRate", dr["OthConRate"]);
                    cm.Parameters.AddWithValue("@OthPriceF", dr["OthPriceF"]);
                    cm.Parameters.AddWithValue("@OthPriceH", dr["OthPriceH"]);
                    cm.Parameters.AddWithValue("@OthExpAmtF", dr["OthExpAmtF"].ToString() == "" ? 0 : dr["OthExpAmtF"]);
                    cm.Parameters.AddWithValue("@OthExpAmtH", dr["OthExpAmtH"].ToString() == "" ? 0 : dr["OthExpAmtH"]);
                    cm.Parameters.AddWithValue("@OthRevAmtF", dr["OthRevAmtF"].ToString() == "" ? 0 : dr["OthRevAmtF"]);
                    cm.Parameters.AddWithValue("@OthRevAmtH", dr["OthRevAmtH"].ToString() == "" ? 0 : dr["OthRevAmtH"]);
                    cm.Parameters.AddWithValue("@OthPaidAmtF", dr["OthPaidAmtF"].ToString() == "" ? 0 : dr["OthPaidAmtF"]);
                    cm.Parameters.AddWithValue("@OthPaidAmtH", dr["OthPaidAmtH"].ToString() == "" ? 0 : dr["OthPaidAmtH"]);
                    cm.Parameters.AddWithValue("@DocID", dr["DocID"]);
                    cm.Parameters.AddWithValue("@DocDate", dr["DocDate"]);
                    cm.Parameters.AddWithValue("@DocDes", dr["DocDes"]);
                    cm.Parameters.AddWithValue("@DocCurrKey", dr["DocCurrKey"].ToString() == "" ? 1 : dr["DocCurrKey"]);
                    cm.Parameters.AddWithValue("@DocCurrRate", dr["DocCurrRate"].ToString() == "" ? 1 : dr["DocCurrRate"]);
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

        internal bool Update(SqlConnection cn, Criteria _criteria)
        {
            bool retValue = false;
            
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTJobDetOther_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 1); 
                    cm.Parameters.AddWithValue("@JobKey", _criteria._jobKey);
                    cm.Parameters.AddWithValue("@JobOtherKey", dr["JobOtherKey"]);
                    cm.Parameters.AddWithValue("@JobPhaseKey", dr["JobPhaseKey"].ToString() == "" ? 0 : dr["JobPhaseKey"]);
                    cm.Parameters.AddWithValue("@JobTaskKey", dr["JobTaskKey"].ToString() == "" ? 0 : dr["JobTaskKey"]);
                    cm.Parameters.AddWithValue("@JobCostTypeKey", dr["JobCostTypeKey"].ToString() == "" ? 0 : dr["JobCostTypeKey"]);
                    cm.Parameters.AddWithValue("@OthLineType", dr["OthLineType"]);
                    cm.Parameters.AddWithValue("@Supervisor", dr["Supervisor"]);
                    cm.Parameters.AddWithValue("@EMKey", dr["EMKey"]);
                    cm.Parameters.AddWithValue("@CostGrp", dr["CostGrp"]);
                    cm.Parameters.AddWithValue("@OthItmKey", dr["OthItmKey"]);
                    cm.Parameters.AddWithValue("@OthItmKeySelect", dr["OthItmKeySelect"]);
                    cm.Parameters.AddWithValue("@OthItmDes", dr["OthItmDes"]);
                    cm.Parameters.AddWithValue("@OthItmType", dr["OthItmType"]);
                    cm.Parameters.AddWithValue("@OthItmRem", dr["OthItmRem"]);
                    cm.Parameters.AddWithValue("@OthQty", dr["OthQty"]);
                    cm.Parameters.AddWithValue("@OthUOMKey", dr["OthUOMKey"]);
                    cm.Parameters.AddWithValue("@OthConRate", dr["OthConRate"]);
                    cm.Parameters.AddWithValue("@OthPriceF", dr["OthPriceF"]);
                    cm.Parameters.AddWithValue("@OthPriceH", dr["OthPriceH"]);
                    cm.Parameters.AddWithValue("@OthExpAmtF", dr["OthExpAmtF"].ToString() == "" ? 0 : dr["OthExpAmtF"]);
                    cm.Parameters.AddWithValue("@OthExpAmtH", dr["OthExpAmtH"].ToString() == "" ? 0 : dr["OthExpAmtH"]);
                    cm.Parameters.AddWithValue("@OthRevAmtF", dr["OthRevAmtF"].ToString() == "" ? 0 : dr["OthRevAmtF"]);
                    cm.Parameters.AddWithValue("@OthRevAmtH", dr["OthRevAmtH"].ToString() == "" ? 0 : dr["OthRevAmtH"]);
                    cm.Parameters.AddWithValue("@OthPaidAmtF", dr["OthPaidAmtF"].ToString() == "" ? 0 : dr["OthPaidAmtF"]);
                    cm.Parameters.AddWithValue("@OthPaidAmtH", dr["OthPaidAmtH"].ToString() == "" ? 0 : dr["OthPaidAmtH"]);
                    cm.Parameters.AddWithValue("@DocID", dr["DocID"]);
                    cm.Parameters.AddWithValue("@DocDate", dr["DocDate"]);
                    cm.Parameters.AddWithValue("@DocDes", dr["DocDes"]);
                    cm.Parameters.AddWithValue("@DocCurrKey", dr["DocCurrKey"].ToString() == "" ? 1 : dr["DocCurrKey"]);
                    cm.Parameters.AddWithValue("@DocCurrRate", dr["DocCurrRate"].ToString() == "" ? 1 : dr["DocCurrRate"]);
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
                        retValue=false;
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
                cm.CommandText = "MSTJobDetOther_Delete";   
                cm.Parameters.AddWithValue("@JobKey", criteria._jobKey);
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

        internal bool DeleteFromTimeSheet(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJobDetOther_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@EMKey", criteria._emKey);
                if (GFunc.IsNE(criteria._docDate))
                    criteria._docDate = DateTime.Now;
                else
                    cm.Parameters.AddWithValue("@DocDate", criteria._docDate);
                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.ExecuteNonQuery();                   

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql connection.
            
        }

        #endregion Delete

    }
}
