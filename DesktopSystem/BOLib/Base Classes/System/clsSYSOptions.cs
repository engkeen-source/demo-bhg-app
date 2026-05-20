using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSOptions : Csla.BusinessListBase<SYSOptions, SYSOption>
    {

        #region Factory Methods

        internal SYSOptions()
        {
        }

        internal static SYSOptions New()
        {

            SYSOptions obj = new SYSOptions();

            return obj;
        }

        internal static SYSOptions Get()
        {
            SYSOptions obj = new SYSOptions();
            obj.Fetch(new Criteria(string.Empty, 0, 0));
            return obj;
        }

        internal static SYSOptions Get(int userKey)
        {
            SYSOptions obj = new SYSOptions();
            obj.Fetch(new Criteria(string.Empty, userKey, 1));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _opID = string.Empty;
            public int? _opUserKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(string OpID, int? OpUserKey, int? Option)
            {
                _opID = OpID;
                _opUserKey = OpUserKey;
                _option = Option;
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
            }// End of SqlConnection.
 
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSOption_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@OpID", criteria._opID);
                cm.Parameters.AddWithValue("@OpUserKey", criteria._opUserKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSOption.Get(dr));
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using
        }
        internal bool Fetch(DataTable table)
        {


            using (SafeDataReader dr = new SafeDataReader(table.CreateDataReader()))
            {
                while (dr.Read())
                    this.Add(SYSOption.Get(dr));
            }
            return true;

        }
        internal bool Fetch(SqlConnection cn, int UserKey)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSOption_Add";

                cm.Parameters.AddWithValue("@OpUserKey", UserKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using
        }

        internal static DataTable GetOptions(SqlConnection cn, Criteria criteria)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSOption_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@OpID", criteria._opID);
                cm.Parameters.AddWithValue("@OpUserKey", criteria._opUserKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SqlDataAdapter adap = new SqlDataAdapter(cm))
                {
                    adap.Fill(dt);
                }               
            }//using
            return dt;
        }
        #endregion //Data Access - Fetch
    }
}
