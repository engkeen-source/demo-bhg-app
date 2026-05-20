using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSIDcounters : Csla.BusinessListBase<SYSIDcounters, SYSIDcounter>
    {

        #region Factory Methods

        internal SYSIDcounters()
        {
        }

        internal static SYSIDcounters New()
        {
            
            SYSIDcounters obj = new SYSIDcounters();
            
            return obj;
        }

        internal static SYSIDcounters Get()
        {
            
            SYSIDcounters obj = new SYSIDcounters();
            obj.Fetch(new Criteria(0, 0, string.Empty, 0, 0, 0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _codeKey = null;
            public int? _period = null;
            internal string _counterGrpStr = string.Empty;
            public int? _docGrpKey = null;
            public int? _conKey = null;
            public int? _eMKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CodeKey, int? Period, string CounterGrpStr, int? DocGrpKey, int? ConKey, int? EMKey, int? Option)
            {
                _codeKey = CodeKey;
                _period = Period;
                _counterGrpStr = CounterGrpStr;
                _docGrpKey = DocGrpKey;
                _conKey = ConKey;
                _eMKey = EMKey;
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
                cm.CommandText = "SYSIDcounter_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@Period", criteria._period);
                cm.Parameters.AddWithValue("@CounterGrpStr", criteria._counterGrpStr);
                cm.Parameters.AddWithValue("@DocGrpKey", criteria._docGrpKey);
                cm.Parameters.AddWithValue("@ConKey", criteria._conKey);
                cm.Parameters.AddWithValue("@EMKey", criteria._eMKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSIDcounter.Get(dr));
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
