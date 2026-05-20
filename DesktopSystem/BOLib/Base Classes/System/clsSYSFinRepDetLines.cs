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
    /// Summary description for SYSFinRepDetLines.
    /// </summary>
    [Serializable]
    public class SYSFinRepDetLines : DataTable
    {
        #region +++  Constructor  +++

        public SYSFinRepDetLines()
        {
            SYSFinRepDetLine obj = new SYSFinRepDetLine();

            this.Fetch(new Criteria(0, 1));

        }
        public SYSFinRepDetLines(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 1));
        }
      
        #endregion

        //Factory Fetch
        public static SYSFinRepDetLines Get(int FinRepKey)
        {
            SYSFinRepDetLines obj = new SYSFinRepDetLines();
            obj.Fetch(new Criteria(FinRepKey,1));
            return obj;
        }

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepDetLineKey = null;
            public int? _FinRepKey = null;
            public int? _RepDetKey = null;
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
                _RepDetKey = 0;
                _ColKey = 0;
                _RepDetLineKey = 0;
                _Option = Option;
            }
            internal Criteria(int? RepDetLineKey,int FinRepKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepDetKey = 0;
                _ColKey = 0;
                _RepDetLineKey = RepDetLineKey;
                _Option = Option;
            }
            internal Criteria(int? RepDetLineKey,int FinRepKey,int ColKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepDetKey = 0;
                _RepDetLineKey = RepDetLineKey;
                _ColKey = ColKey;
                _Option = Option;
            }
            internal Criteria(int? RepDetLineKey, int FinRepKey, int ColKey,int RepDetKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepDetLineKey = RepDetLineKey;
                _ColKey = ColKey;
                _RepDetKey = RepDetKey;
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
                cm.CommandText = "SYSFinRepDetLine_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                cm.Parameters.AddWithValue("@RepDetLineKey", criteria._RepDetLineKey);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._RepDetKey);
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

        internal bool Save(int? FinRepKey, int? FinRepDetKey, DataTable dt)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, FinRepKey, FinRepDetKey, dt);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Insert(SqlConnection cn,int? FinRepKey, int? FinRepDetKey, DataTable dt)
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
                    cm.CommandText = "SYSFinRepDetLine_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@NewRepDetLineKey", 0);

                    cm.Parameters.AddWithValue("@FinRepKey", FinRepKey);
                    cm.Parameters.AddWithValue("@RepDetLineKey", dr["RepDetLineKey"]);
                    cm.Parameters.AddWithValue("@RepDetKey", FinRepDetKey);
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
                cm.CommandText = "SYSFinRepDetLine_Delete";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                cm.Parameters.AddWithValue("@RepDetLineKey", criteria._RepDetLineKey);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._RepDetKey);

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






