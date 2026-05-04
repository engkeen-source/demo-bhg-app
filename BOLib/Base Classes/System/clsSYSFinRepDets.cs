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
    /// Summary description for SYSFinRepDets.
    /// </summary>
    [Serializable]
    public class SYSFinRepDets : DataTable
    {
        #region +++  Constructor  +++

        public SYSFinRepDets()
        {
            SYSFinRepDet obj = new SYSFinRepDet();

            this.Fetch(new Criteria(0, 1));

        }
        public SYSFinRepDets(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 1));
        }
        public new SYSFinRepDets Clone()
        {
            SYSFinRepDets objCopy = (SYSFinRepDets)this.MemberwiseClone();
            return objCopy;
        }

        public static SYSFinRepDets Get(int RepKey)
        {
            SYSFinRepDets obj = new SYSFinRepDets();
            obj.Fetch(new Criteria(RepKey, 1));
            return obj;
        }

        #endregion
        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepDetKey = null;
            public int? _FinRepKey = null;
            public int? _DetLineType = null;
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
                _RepDetKey = 0;
                _DetLineType = 0;
                _FinRepKey = FinRepKey;
                _Option = Option;
            }
            internal Criteria(int? FinRepKey, int RepDetKey, int? Option)
            {
                _DetLineType = 0;
                _FinRepKey = FinRepKey;
                _RepDetKey = RepDetKey;
                _Option = Option;
            }
            internal Criteria(int? FinRepKey, int RepDetKey, int DetLineType, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepDetKey = RepDetKey;
                _DetLineType = DetLineType;
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
                cm.CommandText = "SYSFinRepDet_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._RepDetKey);
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
                    cm.CommandText = "SYSFinRepDet_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@NewRepDetKey", 0);

                    cm.Parameters.AddWithValue("@FinRepKey", FinRepKey);
                    cm.Parameters.AddWithValue("@RepDetKey", dr["RepDetKey"]);
                    cm.Parameters.AddWithValue("@DetLineType", dr["DetLineType"]);
                    cm.Parameters.AddWithValue("@DetLineDesc", dr["DetLineDesc"]);
                    cm.Parameters.AddWithValue("@LineSeq", dr["LineSeq"]);
                    cm.Parameters.AddWithValue("@Remark", dr["Remark"]);
                    cm.Parameters.AddWithValue("@FormatExp", dr["FormatExp"]);
                    cm.Parameters.AddWithValue("@FormulaExp", dr["FormulaExp"]);
                    cm.Parameters.AddWithValue("@SummaryExp", dr["SummaryExp"]);
                    cm.Parameters.AddWithValue("@TotalExp", dr["TotalExp"]);
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
                cm.CommandText = "SYSFinRepDet_Delete";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._RepDetKey);
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






