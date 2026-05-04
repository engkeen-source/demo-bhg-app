using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSLocks : Csla.BusinessListBase<SYSLocks, SYSLock>
    {

        #region Factory Methods

        internal SYSLocks()
        {
        }

        internal static SYSLocks New()
        {
            
            SYSLocks obj = new SYSLocks();
            
            return obj;
        }

        internal static SYSLocks Get()
        {
            
            SYSLocks obj = new SYSLocks();
            obj.Fetch(new Criteria(0, 0, 0, 0,0));
            return obj;
        }

        internal static SYSLocks Get(Criteria criteria)
        {

            SYSLocks obj = new SYSLocks();
            obj.Fetch(criteria);
            return obj;
        }

        internal static SYSLocks Get(SqlConnection cn,Criteria criteria)
        {

            SYSLocks obj = new SYSLocks();
            obj.Fetch(cn, criteria);
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _codeKey = null;
            public int? _dataKey = null;
            public int? _inprogressKey = null;
            public int? _UserKey = null;
            public int? _GUID = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CodeKey, int? DataKey, int? InprogressKey, int? GUID,int? Option)
            {
                _codeKey = CodeKey;
                _dataKey = DataKey;
                _inprogressKey = InprogressKey;
                _UserKey = AppInfor.CurrentUserKey;
                _GUID = GUID;
                _option = Option;
            }
            internal Criteria(int? CodeKey, int? DataKey, int? InprogressKey,int? GUID, int? UserKey, int? Option)
            {
                _codeKey = CodeKey;
                _dataKey = DataKey;
                _inprogressKey = InprogressKey;
                _UserKey = UserKey;
                _GUID = GUID;
                _option = Option;
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
                cm.CommandText = "SYSLock_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@DataKey", criteria._dataKey);
                cm.Parameters.AddWithValue("@InprogressKey", criteria._inprogressKey);
                cm.Parameters.AddWithValue("@Guid", criteria._GUID);
                cm.Parameters.AddWithValue("@UserKey", criteria._UserKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSLock.Get(dr));
                }

                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    
            }//using
        }


        #endregion //Data Access - Fetch
    }
}
