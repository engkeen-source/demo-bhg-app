
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTAccs : Csla.BusinessListBase<MSTAccs, MSTAcc>
    {

        #region Factory Methods

        internal MSTAccs()
        {
        }

        public static MSTAccs New()
        {            
            MSTAccs obj = new MSTAccs();           
            return obj;
        }

        public static MSTAccs Get()
        {            
            MSTAccs obj = new MSTAccs();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        public static MSTAccs Get(string parm_FromID, string parm_ToID)
        {
            MSTAccs obj = new MSTAccs();
            obj.Fetch(new Criteria(parm_FromID,parm_ToID));
            return obj;
        }

        public static MSTAccs Get(int AccType)
        {
            MSTAccs obj = new MSTAccs();
            obj.Fetch(new Criteria(AccType));
            return obj;
        }

        public static MSTAccs Get(int parm_AccType,string parm_FromID, string parm_ToID)
        {
            MSTAccs obj = new MSTAccs();
            obj.Fetch(new Criteria(parm_AccType,parm_FromID,parm_ToID));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _accKey = null;
            public int? _option = null;
            public int _accType = 0;
            public string _FromID = string.Empty;
            public string _ToID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? AccKey, int? Option)
            {
                _accKey = AccKey;
                _accType = 0;
                _FromID = string.Empty;
                _ToID = string.Empty;
                _option = Option;
            }
            internal Criteria(int AccType)
            {
                _accKey = 0;
                _accType= AccType;
                _FromID = string.Empty;
                _ToID = string.Empty;
                _option = 3;
            }
            internal Criteria(string FromAccID,string ToAccID)
            {
                _accKey = 0;
                _accType = 0;
                _FromID = FromAccID;
                _ToID = ToAccID;
                _option = 4;
            }
            internal Criteria(int Acctype,string FromAccID, string ToAccID)
            {
                _accKey = 0;
                _accType = Acctype;
                _FromID = FromAccID;
                _ToID = ToAccID;
                _option = 5;
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
                cm.CommandText = "MSTAcc_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                    
                cm.Parameters.AddWithValue("@AccKey", criteria._accKey);

                cm.Parameters.AddWithValue("@AccType", criteria._accType);
                cm.Parameters.AddWithValue("@AccIDFrom", criteria._FromID);
                cm.Parameters.AddWithValue("@AccIDTo", criteria._ToID);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTAcc.Get(dr));
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