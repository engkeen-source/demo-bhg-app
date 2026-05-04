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
    /// Summary description for APPYDetItms.
    /// </summary>
    [Serializable]
    public class APPYDetItms : DataTable
    {
        #region +++  Constructor  +++


        public APPYDetItms()
        { 
            //Datatable Copy method use Parametaless construstor
            //We need to skip if this Constructor was called from Copy method
            //otherwise we need to get Structure
            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(6, true);//Get For Copy . copy stackFrame is 8

            if (!(GFunc.CompareString(stack.GetMethod().Name, "Copy") || GFunc.CompareString(stack.GetMethod().Name, "Clone")))
                this.Fetch(new Criteria(0, 1));

        }
        public APPYDetItms(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }
        #endregion


        #region Criteria
        [Serializable()]
        public class Criteria
        {
            public int? _DocKey = null;
            public int? _option = null;
            public int? _ConKey = null;
            public int? _CodeKey = null;
            public int? _UserKey = null;
            public int? _GUID = null;
            public int? _ConKeyChange = null;

            public Criteria()
            {
            }
            public Criteria(int? DocKey)
            {
                _DocKey = DocKey;
            }
            public Criteria(int? DocKey, int? Option)
            {
                _DocKey = DocKey;
                _option = Option;
            }
            public Criteria(int? Option, int? ConKey, int? ConKeyChange)
            {
                _option = Option;
                _ConKey = ConKey;
                _ConKeyChange = ConKeyChange;
            }
            public Criteria(int? GUID, int? CodeKey, int? DocKey, int? ConKey, int? UserKey, int? ConKeyChange, int? Option)
            {
                _option = Option;
                _GUID = GUID;
                _CodeKey = CodeKey;
                _DocKey = DocKey;
                _ConKey = ConKey;
                _UserKey = UserKey;
                _ConKeyChange = ConKeyChange;

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
                cm.CommandText = "APPYDetItm_Get";

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


        public bool GetApplyList(SqlConnection cn, Criteria criteria)
        {

            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "APPY_ApplyList";

                cm.Parameters.AddWithValue("@DC", criteria._CodeKey);
                cm.Parameters.AddWithValue("@DK", criteria._DocKey);
                cm.Parameters.AddWithValue("@pCV", criteria._ConKey);
                cm.Parameters.AddWithValue("@GUID", criteria._GUID);
                cm.Parameters.AddWithValue("@UserKey", criteria._UserKey);
                // cm.Parameters.AddWithValue("@PYCVNoChange", criteria._ConKeyChange);  

                cm.Parameters.AddWithValue("@RetVal", 0);
                cm.Parameters["@RetVal"].Direction = ParameterDirection.Output;
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetVal"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.            
        }


        #endregion //Data Access - Fetch

    }
}