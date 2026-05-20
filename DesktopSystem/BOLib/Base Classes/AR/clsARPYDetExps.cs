using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace BOLib
{
    /// <summary>
    /// Summary description for ARPYDetExps.
    /// </summary>
    [Serializable]
    public class ARPYDetExps : DataTable
    {

        #region +++  Constructor  +++

        public ARPYDetExps()
        {

            //Datatable Copy method use Parametaless construstor
            //We need to skip if this Constructor was called from Copy method
            //otherwise we need to get Structure
            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(6, true);//Get For Copy . copy stackFrame is 8

            if (!(GFunc.CompareString(stack.GetMethod().Name, "Copy") || GFunc.CompareString(stack.GetMethod().Name, "Clone")))
                this.Fetch(new Criteria(0, 1));
        }
        public ARPYDetExps(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }
        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _DocKey = null;
            public int? _option = null;
            public int _CodeKey = 0;
            internal Criteria()
            {
            }
            internal Criteria(int? DocKey)
            {
                _DocKey = DocKey;
            }
            internal Criteria(int? DocKey, int? Option)
            {
                _DocKey = DocKey;
                _option = Option;
            }

            internal Criteria(int CodeKey, int? DocKey, int? Option)
            {
                _option = Option;

                _CodeKey = CodeKey;
                _DocKey = DocKey;
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
            string msgID = "RecordGetFail";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "ARPYDetExp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);

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

    }
}