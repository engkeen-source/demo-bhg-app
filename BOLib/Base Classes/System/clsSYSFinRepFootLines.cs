using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSFinRepFootLines.
    /// </summary>
    [Serializable]
    public class SYSFinRepFootLines : DataTable
    {
        #region +++  Constructor  +++

        public SYSFinRepFootLines()
        {
            SYSFinRepFootLine obj = new SYSFinRepFootLine();

            this.Fetch(new Criteria(0, 1));

        }
        public SYSFinRepFootLines(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 1));
        }       

        #endregion
        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepFootLineKey = null;
            public int? _FinRepKey = null;
            public int? _RepFootKey = null;
            public int? _ColKey = null;
            public int? _Option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? FinRepKey)
            {
                _FinRepKey = FinRepKey;
            }
            internal Criteria(int? FinRepKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepFootKey = 0;
                _RepFootLineKey = 0;
                _Option = Option;
            }
            internal Criteria(int? RepFootLineKey,int FinRepKey, int? Option)
            {
                _RepFootKey = 0;
                _FinRepKey = FinRepKey;
                _RepFootLineKey = RepFootLineKey;
                _Option = Option;
            }
            internal Criteria(int? RepFootLineKey,int FinRepKey, int? RepFootKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepFootLineKey = RepFootLineKey;
                _RepFootKey = RepFootKey; 
                _Option = Option;
            }
            internal Criteria(int? RepFootLineKey,int FinRepKey, int? RepFootKey, int? ColKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepFootLineKey = RepFootLineKey;
                _ColKey = ColKey;
                _RepFootKey = RepFootKey;
                _Option = Option;
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
                

            return retValue;
        }
        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSFinRepFootLine_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                cm.Parameters.AddWithValue("@RepFootLineKey", criteria._RepFootLineKey);
                cm.Parameters.AddWithValue("@RepFootKey", criteria._RepFootKey);
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
        #region Data Access - Save

        internal bool Save(int? FinRepKey, int? FooterKey, DataTable dt)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, FinRepKey, FooterKey, dt);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Insert(SqlConnection cn, int? FinRepKey, int? FooterKey, DataTable dt)
        {
            bool retValue = false;

            if (dt.Rows.Count == 0)
            {
                return true;

            }

            //Change Column mapping type to Attribute
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    retValue = true;
                    continue;
                }
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSFinRepFootLine_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@NewRepFootLineKey", 0);

                    cm.Parameters.AddWithValue("@FinRepKey", FinRepKey);
                    cm.Parameters.AddWithValue("@RepFootLineKey", dr["RepFootLineKey"]);
                    cm.Parameters.AddWithValue("@RepFootKey", FooterKey);
                    cm.Parameters.AddWithValue("@ColKey", dr["ColKey"]);
                    cm.Parameters.AddWithValue("@AccType", dr["AccType"]);
                    cm.Parameters.AddWithValue("@FromAccID", dr["FromAccID"]);
                    cm.Parameters.AddWithValue("@ToAccID", dr["ToAccID"]);
                    cm.Parameters.AddWithValue("@FromDept", dr["FromDept"]);
                    cm.Parameters.AddWithValue("@ToDept", dr["ToDept"]);
                    cm.Parameters.AddWithValue("@FromBranch", dr["FromBranch"]);
                    cm.Parameters.AddWithValue("@ToBranch", dr["ToBranch"]);
                    cm.Parameters.AddWithValue("@TransGroup", dr["TransGroup"]);
                    cm.Parameters.AddWithValue("@SummaryExp", dr["SummaryExp"]);
                    cm.Parameters.AddWithValue("@TotalExp", dr["TotalExp"]);
                    cm.Parameters.AddWithValue("@FormulaExp", dr["FormulaExp"]);
                    cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                    cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                    cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }
            }

            return retValue;
        }


        #endregion Save

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
                cm.CommandText = "SYSFinRepFootLine_Delete";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                cm.Parameters.AddWithValue("@RepFootLineKey", criteria._RepFootLineKey);
                cm.Parameters.AddWithValue("@RepFootKey", criteria._RepFootKey);

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






