
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTAccBranchs : Csla.BusinessListBase<MSTAccBranchs, MSTAccBranch>
    {

        #region Factory Methods

        internal MSTAccBranchs()
        {
        }

        internal static MSTAccBranchs New()
        {           
            MSTAccBranchs obj = new MSTAccBranchs();            
            return obj;
        }

        internal static MSTAccBranchs Get()
        {            
            MSTAccBranchs obj = new MSTAccBranchs();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        public static MSTAccBranchs Get(string parm_FromID, string parm_ToID)
        {
            MSTAccBranchs obj = new MSTAccBranchs();
            obj.Fetch(new Criteria(parm_FromID, parm_ToID));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _branchKey = null;
            public int? _option = null;
            public string _FromID = string.Empty;
            public string _ToID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? BranchKey, int? Option)
            {
                _branchKey = BranchKey;
                _option = Option;
                _FromID = "";
                _ToID = "";
            }
            
            internal Criteria(string FromAccID, string ToAccID)
            {
                _branchKey = 0;
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
                cm.CommandText = "MSTAccBranch_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@BranchKey", criteria._branchKey);
                cm.Parameters.AddWithValue("@AccIDFrom", criteria._FromID);
                cm.Parameters.AddWithValue("@AccIDTo", criteria._ToID);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTAccBranch.Get(dr));
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

