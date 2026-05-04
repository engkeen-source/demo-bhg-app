using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using BOLib;

namespace BOLib
{
    [Serializable()]
    public class MSTItmSerials : DataTable
    {

        #region Factory Methods

     
        public MSTItmSerials()
        {
            this.Fetch(new Criteria(0, 2));
        }

        public MSTItmSerials(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }       

        public static MSTItmSerials Get(int? headerKey)
        {            
            MSTItmSerials obj = new MSTItmSerials();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static MSTItmSerials New()
        {           
            MSTItmSerials obj = new MSTItmSerials();
            return obj;
        }
        public static MSTItmSerials New(SqlConnection cn, out string msgID)
        {
            msgID = MsgID.Common.NewFail;
            MSTItmSerials obj = new MSTItmSerials();
            obj.Fetch(cn,new Criteria(0, 1));
            msgID = string.Empty;
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _option = null;
            public int? _ItmKey = null;

            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;
            }
            internal Criteria(int? SerialKey, int? ItmKey, int? Option)
            {
                _headerKey = SerialKey;
                _ItmKey = ItmKey;
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
                cm.CommandText = "MSTItmSerial_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                  
                cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@SerialKey", 0);  
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

        internal bool Insert(int? headerKey,int option)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, headerKey,option);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn, int? headerKey,int Option)
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
                    cm.CommandText = "MSTItmSerial_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", Option);
                    cm.Parameters.AddWithValue("@ItmKey", headerKey);
                    cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"].ToString() == "" ? 0 : dr["BatchKey"]);
                    cm.Parameters.AddWithValue("@SerialKey", GFunc.IsNE(dr["SerialKey"])? 0 :dr["SerialKey"]);
                    cm.Parameters.AddWithValue("@SerialID", dr["SerialID"]);
                    cm.Parameters.AddWithValue("@MACAddress", dr["MACAddress"]);
                    cm.Parameters.AddWithValue("@NewSerialkey", 0);
                    cm.Parameters.AddWithValue("@MfgDate", dr["MfgDate"]);
                    cm.Parameters.AddWithValue("@ExpiryDate", dr["ExpiryDate"]);
                    cm.Parameters.AddWithValue("@ItmStatus", GFunc.IsNE(dr["ItmStatus"])? 0 :dr["ItmStatus"]) ;
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@BatchID", dr["BatchID"]);
                    cm.Parameters["@NewSerialKey"].Direction = ParameterDirection.Output;
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                        retValue=false;
                }// Already close and dispose sql command.
            }
            
            return retValue;
        }
        internal bool Insert(SqlConnection cn, DataRow dr,out int? NewSerialKey,int Option)
        {
            NewSerialKey = 0;

                using (SqlCommand cm = cn.CreateCommand())
                {
                    
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTItmSerial_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", Option);
                    cm.Parameters.AddWithValue("@ItmKey", dr["ItmKey"]);
                    cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"].ToString() == "" ? 0 : dr["BatchKey"]);
                    cm.Parameters.AddWithValue("@SerialKey", 0);
                    cm.Parameters.AddWithValue("@SerialID", dr["SerialID"]);
                    cm.Parameters.AddWithValue("@MACAddress", dr["MACAddress"]);
                    cm.Parameters.AddWithValue("@NewSerialkey", 0);
                    cm.Parameters.AddWithValue("@MfgDate", dr["MfgDate"]);
                    cm.Parameters.AddWithValue("@ExpiryDate", dr["ExpiryDate"]);
                    cm.Parameters.AddWithValue("@ItmStatus", GFunc.IsNE(dr["ItmStatus"]) ? 0 : dr["ItmStatus"]);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters["@NewSerialKey"].Direction = ParameterDirection.Output;
                    cm.Parameters.AddWithValue("@BatchID", 0);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    NewSerialKey = (int)cm.Parameters["@NewSerialkey"].Value;
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;
                }// Already close and dispose sql command.
                      
        }
        #endregion Insert

        #region Data Access - Update

        internal bool Update(out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Update(SqlConnection cn, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordUpdateFail";
            
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "MSTItmSerial_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@MsgID", msgID);

                    cm.Parameters.AddWithValue("@ItmKey", dr["ItmKey"]);
                    cm.Parameters.AddWithValue("@BatchKey", dr["BatchKey"].ToString() == "" ? 0 : dr["BatchKey"]);
                    cm.Parameters.AddWithValue("@SerialID", dr["SerialID"]);
                    cm.Parameters.AddWithValue("@MACAddress", dr["MACAddress"]);
                    cm.Parameters.AddWithValue("@MfgDate", dr["MfgDate"]);
                    cm.Parameters.AddWithValue("@ExpiryDate", dr["ExpiryDate"]);
                    cm.Parameters.AddWithValue("@ItmStatus", dr["ItmStatus"]);
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
                        retValue = true;
                    else
                        retValue=false;
                }
            }// Already close and dispose sql command.
            
            return retValue;
        }

        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria, out string msgID)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria, out msgID);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
        {
            msgID = "RecordDeleteFail";
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmSerial_Delete";

                cm.Parameters.AddWithValue("@MsgID", msgID);
                cm.Parameters.AddWithValue("@SerialKey", criteria._headerKey);
                cm.Parameters.AddWithValue("@ItmKey", criteria._ItmKey);
                cm.Parameters.AddWithValue("@Option", criteria._option);

                

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

