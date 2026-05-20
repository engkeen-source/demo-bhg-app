using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSDocTypes : Csla.BusinessListBase<SYSDocTypes, SYSDocType>
    {

        #region Factory Methods

        internal SYSDocTypes()
        {
        }

        internal static SYSDocTypes New()
        {
            
            SYSDocTypes obj = new SYSDocTypes();
            
            return obj;
        }

        internal static SYSDocTypes Get()
        {
            
            SYSDocTypes obj = new SYSDocTypes();
            obj.Fetch(new Criteria(0, 0, 0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _codeKey = null;
            public int? _docType = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CodeKey, int? DocType, int? Option)
            {
                _codeKey = CodeKey;
                _docType = DocType;
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
                cm.CommandText = "SYSDocType_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@DocType", criteria._docType);
                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSDocType.Get(dr));
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