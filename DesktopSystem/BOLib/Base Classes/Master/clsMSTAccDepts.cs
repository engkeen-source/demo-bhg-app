
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTAccDepts : Csla.BusinessListBase<MSTAccDepts, MSTAccDept>
    {

        #region Factory Methods

        internal MSTAccDepts()
        {
        }

        internal static MSTAccDepts New()
        {           
            MSTAccDepts obj = new MSTAccDepts();           
            return obj;
        }

        internal static MSTAccDepts Get()
        {           
            MSTAccDepts obj = new MSTAccDepts();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        public static MSTAccDepts Get(string parm_FromID, string parm_ToID)
        {
            MSTAccDepts obj = new MSTAccDepts();
            obj.Fetch(new Criteria(parm_FromID, parm_ToID));
            return obj;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _deptKey = null;
            public int? _option = null;
            public string _FromID = string.Empty;
            public string _ToID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? DeptKey, int? Option)
            {
                _deptKey = DeptKey;
                _option = Option;
                _FromID = "";
                _ToID = "";
            }
            internal Criteria(string FromAccID, string ToAccID)
            {
                _deptKey = 0;
                _FromID = FromAccID;
                _ToID = ToAccID;
                _option = 2;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        private bool Fetch(Criteria criteria)
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

        private bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTAccDept_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@DeptKey", criteria._deptKey);
                cm.Parameters.AddWithValue("@AccIDFrom", criteria._FromID);
                cm.Parameters.AddWithValue("@AccIDTo", criteria._ToID);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTAccDept.Get(dr));
                }

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using
            
        }


        #endregion //Data Access - Fetch
    }
}

