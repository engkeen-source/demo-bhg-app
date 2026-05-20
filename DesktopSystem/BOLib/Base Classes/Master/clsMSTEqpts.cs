
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTEqpts : Csla.BusinessListBase<MSTEqpts, MSTEqpt>
    {

        #region Factory Methods

        internal MSTEqpts()
        {
        }

        internal static MSTEqpts New()
        {
            
            MSTEqpts obj = new MSTEqpts();
            
            return obj;
        }

        internal static MSTEqpts Get()
        {
            string msgID = "RecordGetFail";
            MSTEqpts obj = new MSTEqpts();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _eqptKey = null;
            public int? _option = null;
            internal int? _eqptConKey = null;
            internal bool? _templateYN = null;

            internal Criteria()
            {
            }

            internal Criteria(int? EqptKey, int? Option)
            {
                _eqptKey = EqptKey;
                _option = Option;
            }
            internal Criteria(bool? TemplateYN, int? EqptConKey, int? Option)
            {
                _templateYN = TemplateYN;
                _eqptConKey = EqptConKey;
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
            string msgID = "";
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqpt_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@EqptKey", criteria._eqptKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTEqpt.Get(dr));
                }

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
                         
            }//using
            
        }

        internal bool FetchTemplate(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTEqpt_GetTemplate";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@TemplateYN", criteria._templateYN);
                cm.Parameters.AddWithValue("@EqptConKey", criteria._eqptConKey);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(MSTEqpt.Get(dr));
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
                    this.Add(MSTEqpt.Get(dr));
            }

            return true;
        }

        #endregion //Data Access - Fetch
    }
}