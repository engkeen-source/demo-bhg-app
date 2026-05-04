using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;

namespace BOLib
{
    /// <summary>
    /// Summary description for MSTConRemarkDTable.
    /// </summary>
    [Serializable]
    public class MSTConRemarkDTable : DataTable
    {

        #region Factory Methods

        public MSTConRemarkDTable()
        {
        }

        public MSTConRemarkDTable(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        public static MSTConRemarkDTable Get(int? headerKey)
        {
            MSTConRemarkDTable obj = new MSTConRemarkDTable();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static MSTConRemarkDTable New()
        {
            MSTConRemarkDTable obj = new MSTConRemarkDTable();
            return obj;
        }

        public static MSTConRemarkDTable New(SqlConnection cn)
        {
            MSTConRemarkDTable obj = new MSTConRemarkDTable();
            obj.Fetch(cn, new Criteria(0, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _ConKey = null;
            public int? _option = null;
            public string _Remark = string.Empty;
            public bool _ActionClose = false;

            internal Criteria()
            {
            }
            internal Criteria(int? ConKey)
            {
                _ConKey = ConKey;
            }
            internal Criteria(int? ConKey, int? Option)
            {
                _ConKey = ConKey;
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
                cm.CommandText = "MSTConRemark_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ConKey", criteria._ConKey);
                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                //Additional Parameter for Return Value From StoredProcedure
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return true;

            }//using            
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert Update

        internal bool InsertUpdate(Criteria criteria)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.InsertUpdate(cn, criteria);
                }
                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            return retValue;
        }

        internal bool InsertUpdate(SqlConnection cn, Criteria criteria)
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
                    cm.CommandText = "MSTConRemark_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@ConKey", criteria._ConKey);
                    //cm.Parameters.AddWithValue("@NewRemarkID", 0);
                    cm.Parameters.AddWithValue("@Remark", dr["Remark"]);
                    cm.Parameters.AddWithValue("@ActionClose", dr["ActionClose"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    //cm.Parameters["@NewRemarkID"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();
                    //if (criteria._option == 0)
                    //{
                    //    if (!GFunc.IsNEZ(cm.Parameters["@NewRemarkID"].Value))
                    //        dr["ConRemarkID"] = cm.Parameters["@NewRemarkID"].Value;
                    //}
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                }// Already close and dispose sql command.
            }

            return retValue;
        }

        #endregion Insert        

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
                cm.CommandText = "MSTConRemark_Delete";

                cm.Parameters.AddWithValue("@RepKey", criteria._ConKey);
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