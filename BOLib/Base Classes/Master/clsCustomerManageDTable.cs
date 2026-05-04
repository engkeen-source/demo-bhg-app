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
    /// Summary description for SYSRepRpts.
    /// </summary>
    [Serializable]
    public class CustomerManageDTable : DataTable
    {
        #region Factory Methods

        public CustomerManageDTable()
        {
        }

        public CustomerManageDTable(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        public static CustomerManageDTable Get(int? headerKey)
        {
            CustomerManageDTable obj = new CustomerManageDTable();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        public static CustomerManageDTable New()
        {
            CustomerManageDTable obj = new CustomerManageDTable();
            return obj;
        }

        public static CustomerManageDTable New(SqlConnection cn)
        {
            CustomerManageDTable obj = new CustomerManageDTable();
            obj.Fetch(cn, new Criteria(0, 1));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _Option = null;
            public int? _DueCal = null;
            public int? _CCB = null;
            public string _DateV = string.Empty;

            internal Criteria()
            {
            }
            internal Criteria(int? Option)
            {
                _Option = Option;
            }
            internal Criteria(int? Option, int? DueCal)
            {
                _Option = Option;
                _DueCal = DueCal;
            }
            internal Criteria(int? Option, int? DueCal, int? CCB)
            {
                _Option = Option;
                _DueCal = DueCal;
                _CCB = CCB;
            }
            internal Criteria(int? Option, int? DueCal, int? CCB, string DateV)
            {
                _Option = Option;
                _DueCal = DueCal;
                _CCB = CCB;
                _DateV = DateV;
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
                cm.CommandTimeout = 0;
                cm.CommandText = "Rep_ConAge_Manage";
                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@DateV", criteria._DateV);
                cm.Parameters.AddWithValue("@DueCal", criteria._DueCal);
                cm.Parameters.AddWithValue("@CCB", criteria._CCB);
                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                //cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return true;
            }//using            
        }

        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(Criteria _criteria)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn);
                }
                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            return retValue;
        }

        internal bool Insert(SqlConnection cn)
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
                    cm.CommandText = "CustomerManage_Update";
                    cm.Parameters.AddWithValue("@ConKey", dr["DocConKey"]);
                    cm.Parameters.AddWithValue("@ActiveWithProb", dr["ActiveWithProblem"]);
                    cm.Parameters.AddWithValue("@OrangeCus", dr["OrangeCus"]);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                }// Already close and dispose sql command.
            }

            return retValue;
        }

        #endregion Insert       

    }
}