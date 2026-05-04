using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSMsgApps : Csla.BusinessListBase<SYSMsgApps, SYSMsgApp>
    {

        #region Factory Methods

        internal SYSMsgApps()
        {
        }

        internal static SYSMsgApps New()
        {
            
            SYSMsgApps obj = new SYSMsgApps();
            
            return obj;
        }

        internal static SYSMsgApps Get()
        {
            
            SYSMsgApps obj = new SYSMsgApps();
            obj.Fetch(new Criteria(string.Empty, 0));
            return obj;
        }

        //internal static SYSMsgApps Get(SafeDataReader dr)
        //{
        //    
        //    SYSMsgApps obj = new SYSMsgApps();
        //    obj.Fetch(new Criteria(0, 0));
        //    return obj;
        //}

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _msgID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(string MsgID, int? Option)
            {
                _msgID = MsgID;
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
                cm.CommandText = "SYSMsgApp_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@MsgID", criteria._msgID);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSMsgApp.Get(dr));
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
