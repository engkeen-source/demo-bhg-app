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
    /// Summary description for SYSFinRepCols.
    /// </summary>
    [Serializable]
    public class SYSFinRepCols : DataTable
    {
        #region +++  Constructor  +++

        public SYSFinRepCols()
        {
            SYSFinRepCol obj = new SYSFinRepCol();

            this.Fetch(new Criteria(0, 1));

        }
        public SYSFinRepCols(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 1));
        }
        public new SYSFinRepCols Clone()
        {
            SYSFinRepCols objCopy = (SYSFinRepCols)this.MemberwiseClone();
            return objCopy;
        }
        #endregion

        //Fectory Fetch
        public static SYSFinRepCols Get(int FinRepKey)
        {
            SYSFinRepCols obj = new SYSFinRepCols();
            obj.Fetch(new Criteria(FinRepKey, 1));
            return obj;
        }

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _ColKey = null;
            public int? _FinRepKey = null;
            public int? _RepDetKey = null;
            public int? _RepFootKey = null;
            public int? _Option = null;
            public int? _ColType = null;

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
                _RepDetKey = 0;
                _ColType = 0;
                _ColKey = 0;
                _Option = Option;
            }
            internal Criteria(int? ColKey,int FinRepKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _RepFootKey = 0;
                _RepDetKey = 0;
                _ColType = 0;
                _ColKey = ColKey;
                _Option = Option;
            }
            internal Criteria(int? ColKey,int FinRepKey,int? RepDetKey, int? RepFootKey, int? Option)
            {
                _FinRepKey = FinRepKey;
                _ColType = 0;
                _ColKey = ColKey;
                _RepFootKey = RepFootKey;
                _RepDetKey = RepDetKey;
                _Option = Option;
            }
            internal Criteria(int? ColKey,int FinRepKey, int? RepDetKey, int? RepFootKey, int? ColType, int? Option)
            {
                _FinRepKey = FinRepKey;
                _ColKey = ColKey;
                _RepDetKey = RepDetKey;
                _RepFootKey = RepFootKey;
                _ColType = ColType;
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
                cm.CommandText = "SYSFinRepCol_Get";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                cm.Parameters.AddWithValue("@ColKey", criteria._ColKey);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._RepDetKey);
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

        internal bool Save(int? FinRepKey, int? FinRepDetKey, int? FinRepFootKey, DataTable dt)
        {
            bool retValue = false;
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn, FinRepKey,FinRepDetKey, FinRepFootKey, dt);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Insert(SqlConnection cn, int? FinRepKey, int? FinRepDetKey, int? FinRepFootKey, DataTable dt)
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
                    cm.CommandText = "SYSFinRepCol_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@NewColKey", 0);

                    cm.Parameters.AddWithValue("@FinRepKey", FinRepKey);
                    cm.Parameters.AddWithValue("@ColKey", dr["ColKey"]);
                    cm.Parameters.AddWithValue("@RepDetKey", FinRepDetKey);
                    cm.Parameters.AddWithValue("@RepFootKey", FinRepFootKey);
                    cm.Parameters.AddWithValue("@ColType", dr["ColType"]);
                    cm.Parameters.AddWithValue("@ColDesc", dr["ColDesc"]);
                    cm.Parameters.AddWithValue("@ColTitle", dr["ColTitle"]);
                    cm.Parameters.AddWithValue("@ColTitleRTF", dr["ColTitleRTF"]);
                    cm.Parameters.AddWithValue("@ColSeq", dr["ColSeq"]);
                    cm.Parameters.AddWithValue("@ColTypeExp", dr["ColTypeExp"]);
                    cm.Parameters.AddWithValue("@ColFormulaExp", dr["ColFormulaExp"]);
                    cm.Parameters.AddWithValue("@ColDisplay", dr["ColDisplay"]);
                    cm.Parameters.AddWithValue("@Format", dr["Format"]);
                    cm.Parameters.AddWithValue("@Width", dr["Width"]);
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
                cm.CommandText = "SYSFinRepCol_Delete";

                cm.Parameters.AddWithValue("@Option", criteria._Option);
                cm.Parameters.AddWithValue("@FinRepKey", criteria._FinRepKey);
                cm.Parameters.AddWithValue("@ColKey", criteria._ColKey);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._RepDetKey);
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






