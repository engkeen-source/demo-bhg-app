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
    /// Summary description for SYSFinRepFooters.
    /// </summary>
    [Serializable]
    public class SYSFinRepFooters : DataTable
    {
        #region +++  Constructor  +++

        public SYSFinRepFooters()
        {
            SYSFinRepFooter obj = new SYSFinRepFooter();

            this.Fetch(new Criteria(0, 1));

        }
        public SYSFinRepFooters(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 1));
        }
        
        public static SYSFinRepFooters Get(int SysFinRepKey)
        {
            SYSFinRepFooters obj = new SYSFinRepFooters();
            obj.Fetch(new Criteria(SysFinRepKey, 1));
            return obj;
        }
        #endregion
        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepFootKey = null;
            public int? _FinRepKey = null;
            public int? _LineSeq = null;
            public int? _Option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? FinRepKey)
            {
                _FinRepKey = FinRepKey;
                _Option = 0;
            }
            internal Criteria(int? FinRepKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _LineSeq = 0;
                _RepFootKey = 0;
                _Option = Option;
            }
            internal Criteria(int? RepFootKey,int FinRepKey, int? Option)
            {
                _RepFootKey = RepFootKey;
                _FinRepKey = FinRepKey;
                _Option = Option;
            }
            internal Criteria(int? RepFootKey, int FinRepKey, int LineSeq, int? Option)
            {
                _RepFootKey = RepFootKey;
                _FinRepKey = FinRepKey;
                _LineSeq = LineSeq;
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
                cm.CommandText = "SYSFinRepFooter_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@RepFootKey", criteria._RepFootKey);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
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

        internal bool Save(int? FinRepKey, DataTable dt)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, FinRepKey, dt);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Insert(SqlConnection cn, int? FinRepKey, DataTable dt)
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
                    cm.CommandText = "[SYSFinRepFooter_AddUpdate]";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@NewRepFootKey", 0);

                    cm.Parameters.AddWithValue("@FinRepKey", FinRepKey);
                    cm.Parameters.AddWithValue("@RepFootKey", dr["RepFootKey"]);
                    cm.Parameters.AddWithValue("@FootLineType", dr["FootLineType"]);
                    cm.Parameters.AddWithValue("@FootLineDesc", dr["FootLineDesc"]);
                    cm.Parameters.AddWithValue("@LineSeq", dr["LineSeq"]);
                    cm.Parameters.AddWithValue("@LineText", dr["LineText"]);
                    cm.Parameters.AddWithValue("@LineTextRTF", dr["LineTextRTF"]);
                    cm.Parameters.AddWithValue("@FormulaExp", dr["FormulaExp"]);
                    cm.Parameters.AddWithValue("@SummaryExp", dr["SummaryExp"]);
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                    cm.Parameters.AddWithValue("@Format", dr["Format"]);
                    cm.Parameters.AddWithValue("@Height", dr["Height"]);
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
                cm.CommandText = "SYSFinRepFooter_Delete";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@RepFootKey", criteria._RepFootKey);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);

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






