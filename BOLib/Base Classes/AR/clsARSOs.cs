using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;

namespace BOLib
{
    //not use ,To remove this class
    /// <summary>
    /// Summary description for ARSOs.
    /// </summary>
    [Serializable]
    public class ARSOs : DataTable
    {

        #region +++  Constructor  +++

        public ARSOs()
        {
            ARSO obj = new ARSO();
            string msgID = string.Empty;
            this.Fetch(new Criteria(0, 9999));
        }

        public ARSOs(SqlConnection cn)
        {
            string msgID = string.Empty;
            this.Fetch(cn, new Criteria(0, 9999));
        }
        #endregion


        public static ARSOs Get()
        {

            ARSOs obj = new ARSOs();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }
        public static ARSOs Get(int? CustomerKey)
        {
            CustomerKey = (CustomerKey == null) ? 0 : CustomerKey;
            ARSOs obj = new ARSOs();
            obj.Fetch(new Criteria(0, CustomerKey, 2));
            return obj;
        }

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _DocKey = null;
            public int? _option = null;
            public int? _CustomerKey = null;

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

            internal Criteria(int? DocKey, int? CustomerKey, int? Option)
            {
                _option = Option;
                _DocKey = DocKey;
                _CustomerKey = CustomerKey;
            }
        }
        #endregion //Criteria


        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
        
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
                cm.CommandText = "ARSO_GetCollection";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", criteria._DocKey);
                cm.Parameters.AddWithValue("@CusotmerKey", criteria._CustomerKey);

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
