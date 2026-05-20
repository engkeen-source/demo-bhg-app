using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSMsgLists : Csla.BusinessListBase<SYSMsgLists, SYSMsgList>
    {

        #region Factory Methods

        internal SYSMsgLists()
        {
        }

        internal static SYSMsgLists New()
        {
            
            SYSMsgLists obj = new SYSMsgLists();
            
            return obj;
        }

        internal static SYSMsgLists Get()
        {
            
            SYSMsgLists obj = new SYSMsgLists();
            obj.Fetch(new Criteria(string.Empty, 0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _dataGrp = string.Empty;
            public int? _msgValue = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(string DataGrp, int? MsgValue, int? Option)
            {
                _dataGrp = DataGrp;
                _msgValue = MsgValue;
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
                cm.CommandText = "SYSMsgList_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@DataGrp", criteria._dataGrp);
                cm.Parameters.AddWithValue("@MsgValue", criteria._msgValue);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSMsgList.Get(dr));
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
