using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class REFUOMDetItms : DataTable
    {

        #region Factory Methods
               
        public REFUOMDetItms()
        {

            //Datatable Copy method use Parametaless construstor
            //We need to skip if this Constructor was called from Copy method
            //otherwise we need to get Structure
            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(6, true);//Get For Copy . copy stackFrame is 8

            if (!(GFunc.CompareString(stack.GetMethod().Name, "Copy") || GFunc.CompareString(stack.GetMethod().Name, "Clone")) && !GFunc.CompareString(stack.GetMethod().Name, "CreatePO"))
                this.Fetch(new Criteria(0, 1));
        }

        //public REFUOMDetItms()
        //{           
        //    this.Fetch(new Criteria(0, 1));
        //}
        public REFUOMDetItms(SqlConnection cn)
        {            
            this.Fetch(cn, new Criteria(0, 1));
        }      
        public  static REFUOMDetItms Get( int? uomKey)
        {
            REFUOMDetItms obj = new REFUOMDetItms();
            obj.Fetch(new Criteria(uomKey, 0, 1));
            return obj;
        }
      
        public static REFUOMDetItms New()
        {
            REFUOMDetItms obj = new REFUOMDetItms();
            
            return obj;
        }
        public static REFUOMDetItms New(SqlConnection cn)
        {
            REFUOMDetItms obj = new REFUOMDetItms(cn);

            return obj;
        }
        public static REFUOMDetItms Get( int? uomKey, int? uomConKey)
        {
            REFUOMDetItms obj = new REFUOMDetItms();
            obj.Fetch(new Criteria(uomKey,uomConKey,2));
            return obj;
        }
        public static REFUOMDetItms Get(SqlConnection cn, int? uomKey, int? uomConKey)
        {
            REFUOMDetItms obj = new REFUOMDetItms();
            obj.Fetch(cn, new Criteria(uomKey, uomConKey, 2));
            return obj;
        }
        public static REFUOMDetItms Get(int? uomKey, int? option, int? uomConkey)
        {
            REFUOMDetItms obj = new REFUOMDetItms();
            obj.Fetch(new Criteria(uomKey, uomConkey, option));
            return obj;
        }
        public static REFUOMDetItms Get(SqlConnection cn, int? uomKey, int? option, int? uomConkey)
        {
            REFUOMDetItms obj = new REFUOMDetItms(cn);
            obj.Fetch(cn, new Criteria(uomKey, uomConkey, option));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _uOMKey = 0;
            public int? _option = 0;
            public int? _uOMConKey = 0;
            internal Criteria()
            {
            }

            internal Criteria(int? UOMKey, int? Option)
            {
                _uOMKey = UOMKey;
                _option = Option;
            }
             internal Criteria(int? UOMKey,int? uOMConKey, int? Option)
            {
                _uOMKey = UOMKey;
                _uOMConKey = uOMConKey;
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
                cm.CommandText = "REFUOMDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@UOMKey", GFunc.NEInt(criteria._uOMKey,0));
                cm.Parameters.AddWithValue("@UOMConKey", criteria._uOMConKey);

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

        internal bool Insert( int? headerKey)
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
                    cm.CommandText = "REFUOMDetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);

                    cm.Parameters.AddWithValue("@UOMKey", headerKey);                        
                    cm.Parameters.AddWithValue("@UOMConKey", dr["UOMConKey"]);
                    cm.Parameters.AddWithValue("@UOMConRate", dr["UOMConRate"].ToString() == "" ? 1 : dr["UOMConRate"]);
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
                    cm.CommandText = "REFUOMDetItm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    
                    cm.Parameters.AddWithValue("@UOMKey", dr["UOMKey"]);
                    cm.Parameters.AddWithValue("@UOMConKey", dr["UOMConKey"]);
                    cm.Parameters.AddWithValue("@UOMConRate", dr["UOMConRate"].ToString() == "" ? 1 : dr["UOMConRate"]);
                    
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
                cm.CommandText = "REFUOMDetItm_Delete";

                cm.Parameters.AddWithValue("@UOMKey", criteria._uOMKey);

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
