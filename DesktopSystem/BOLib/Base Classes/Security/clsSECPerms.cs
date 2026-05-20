using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SECPerms : Csla.BusinessListBase<SECPerms, SECPerm>
    {

        #region Factory Methods

        internal SECPerms()
        {
        }

        internal static SECPerms New()
        {
            SECPerms obj = new SECPerms();
            return obj;
        }

        internal static SECPerms Get()
        {
            SECPerms obj = new SECPerms();
            obj.Fetch(new Criteria(string.Empty, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _permID = string.Empty;
            internal int? _permGrpKey = 0;
            internal int? _option = 0;    

            internal Criteria()
            {
            }

            internal Criteria(string PermID, int? Option)
            {
                _permID = PermID;                
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

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            string msgID = MsgID.Common.GetFail;
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECPerm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@PermID", criteria._permID);
                cm.Parameters.AddWithValue("@PermGrpKey", criteria._permGrpKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SECPerm.Get(dr));
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
                    this.Add(SECPerm.Get(dr));
            }

            return true;
        }


        #endregion //Data Access - Fetch
    }
}