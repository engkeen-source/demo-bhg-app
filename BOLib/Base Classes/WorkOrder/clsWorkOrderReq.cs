using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class WorkOrderReq : DataTable
    {
        #region +++  Constructor  +++

        public WorkOrderReq()
        {
            //Datatable Copy method use Parametaless construstor
            //We need to skip if this Constructor was called from Copy method
            //otherwise we need to get Structure
            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(6, true);//Get For Copy . copy stackFrame is 8

            if (!(GFunc.CompareString(stack.GetMethod().Name, "Copy") || GFunc.CompareString(stack.GetMethod().Name, "Clone")))
                this.Fetch(new Criteria(0, 1));
        }

        public WorkOrderReq(SqlConnection cn)
        {
            if (this.Columns.Count == 0)
                this.Fetch(cn, new Criteria(0, 1));
            else
                this.Rows.Clear();
        }

        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _workOrderKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? WorkOrderKey)
            {
                _workOrderKey = WorkOrderKey;
            }
            internal Criteria(int? WorkOrderKey, int? Option)
            {
                _workOrderKey = WorkOrderKey;
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
            }
            // No errors - commit transaction


            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "WOReq_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@WorkOrderKey", criteria._workOrderKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.                       
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out string msgID)
        {
            bool retValue = false;
            msgID = "RecordAddFail";
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, out msgID);
                }
                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordAddFail";
            foreach (DataRow dr in this.Rows)
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "WOReq_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@MsgID", msgID);


                    cm.Parameters.AddWithValue("@WorkOrderKey", dr["WorkOrderKey"]);

                    cm.Parameters.AddWithValue("@DetKey", dr["DetKey"]);

                    cm.Parameters.AddWithValue("@ReqTypeKey", dr["ReqTypeKey"]);

                    cm.Parameters.AddWithValue("@ReqTypeDes", dr["ReqTypeDes"]);

                    cm.Parameters.AddWithValue("@ReqItemKey", dr["ReqItemKey"]);

                    cm.Parameters.AddWithValue("@EstCost", dr["EstCost"]);

                    cm.Parameters.AddWithValue("@QuotedAmt", dr["QuotedAmt"]);

                    cm.Parameters.AddWithValue("@BilledAmt", dr["BilledAmt"]);

                    cm.Parameters.AddWithValue("@ToInvoice", dr["ToInvoice"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    // Execute command.
                    cm.ExecuteNonQuery();
                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }// Already close and dispose sql command.
            }
            return retValue;
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
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
                    cm.CommandText = "WOReq_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 1);
                    cm.Parameters.AddWithValue("@MsgID", msgID);

                    cm.Parameters.AddWithValue("@WorkOrderKey", dr["WorkOrderKey"]);
                    cm.Parameters.AddWithValue("@DetKey", dr["DetKey"]);
                    cm.Parameters.AddWithValue("@ReqTypeKey", dr["ReqTypeKey"]);
                    cm.Parameters.AddWithValue("@ReqTypeDes", dr["ReqTypeDes"]);
                    cm.Parameters.AddWithValue("@ReqItemKey", dr["ReqItemKey"]);
                    cm.Parameters.AddWithValue("@EstCost", dr["EstCost"]);
                    cm.Parameters.AddWithValue("@QuotedAmt", dr["QuotedAmt"]);
                    cm.Parameters.AddWithValue("@BilledAmt", dr["BilledAmt"]);
                    cm.Parameters.AddWithValue("@ToInvoice", dr["ToInvoice"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    cm.Parameters["@NewWOKey"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();
                    if (cm.Parameters["@MsgID"].Value == null)
                        msgID = string.Empty;
                    else
                        msgID = cm.Parameters["@MsgID"].Value.ToString();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }// Already close and dispose sql command.
            }

            return retValue;
        }
        #endregion Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria, out string msgID)
        {
            bool retValue = false;
            msgID = "RecordDeleteFail";
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria, out string msgID)
        {
            msgID = "RecordDeleteFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "WOReq_Delete";

                cm.Parameters.AddWithValue("@MsgID", msgID);
                cm.Parameters.AddWithValue("@WorkOrderKey", criteria._workOrderKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if (cm.Parameters["@MsgID"].Value == null)
                    msgID = string.Empty;
                else
                    msgID = cm.Parameters["@MsgID"].Value.ToString();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.

        }
        #endregion Delete
    }
}
