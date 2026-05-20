using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Collections.Generic;

namespace BOLib
{
    [Serializable()]
    public class SYSDocTypeDetNms : DataTable
    {
        #region Factory Methods

        public SYSDocTypeDetNms()
        {
            this.Fetch(new Criteria(0,1));   
        }

        public SYSDocTypeDetNms(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 0));
        }

        public static SYSDocTypeDetNms Get(int? codeKey,int? counterGrp)
        {
            SYSDocTypeDetNms obj = new SYSDocTypeDetNms();
            obj.Fetch(new Criteria(codeKey, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _codeKey = null;
            internal string _docTypeNm = string.Empty;
            public int? _option = null;
            public int? _counterGrp = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CodeKey,  int? Option)
            {
                _codeKey = CodeKey;
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
                cm.CommandText = "SYSDocTypeDetNm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);

                if (criteria._docTypeNm == null)
                    cm.Parameters.AddWithValue("@DocTypeNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocTypeNm", criteria._docTypeNm);

                if (criteria._counterGrp == null)
                    cm.Parameters.AddWithValue("@CounterGrp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CounterGrp", criteria._counterGrp);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                this.Clear();
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();

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
                    cm.CommandText = "SYSDocTypeDetNm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);

                    cm.Parameters.AddWithValue("@CodeKey", headerKey);
                    cm.Parameters.AddWithValue("@DocTypeNm", dr["DocTypeNm"]);
                    cm.Parameters.AddWithValue("@DocType", dr["DocType"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang1", dr["DocTypeNmLang1"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang2", dr["DocTypeNmLang2"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang3", dr["DocTypeNmLang3"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang4", dr["DocTypeNmLang4"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang5", dr["DocTypeNmLang5"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang6", dr["DocTypeNmLang6"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang7", dr["DocTypeNmLang7"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang8", dr["DocTypeNmLang8"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang9", dr["DocTypeNmLang9"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang10", dr["DocTypeNmLang10"]);                    
                    cm.Parameters.AddWithValue("@SystemReq", dr["SystemReq"]);
                    cm.Parameters.AddWithValue("@SetAsDefault", dr["SetAsDefault"]);
                    cm.Parameters.AddWithValue("@Hidden", dr["Hidden"]);
                    cm.Parameters.AddWithValue("@CounterGrp", dr["CounterGrp"]);
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
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
                    cm.CommandText = "SYSDocTypeDetNm_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    
                    cm.Parameters.AddWithValue("@DocTypeNm", dr["DocTypeNm"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang1", dr["DocTypeNmLang1"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang2", dr["DocTypeNmLang2"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang3", dr["DocTypeNmLang3"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang4", dr["DocTypeNmLang4"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang5", dr["DocTypeNmLang5"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang6", dr["DocTypeNmLang6"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang7", dr["DocTypeNmLang7"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang8", dr["DocTypeNmLang8"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang9", dr["DocTypeNmLang9"]);
                    cm.Parameters.AddWithValue("@DocTypeNmLang10", dr["DocTypeNmLang10"]);
                    cm.Parameters.AddWithValue("@SystemReq", dr["SystemReq"]);
                    cm.Parameters.AddWithValue("@SetAsDefault", dr["SetAsDefault"]);
                    cm.Parameters.AddWithValue("@Hidden", dr["Hidden"]);
                    cm.Parameters.AddWithValue("@CounterGrp	", dr["CounterGrp"]);
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSDocTypeDetNm_Delete";

                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                // Additional Parameter for Return Value From StoredProcedure 
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
