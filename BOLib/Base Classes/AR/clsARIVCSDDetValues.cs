
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class ARIVCSDDetValues : DataTable
    {

        #region Factory Methods

        public ARIVCSDDetValues()
        {

            this.Fetch(new Criteria(0, 1));
        }

        public ARIVCSDDetValues(SqlConnection cn)
        {
            this.Fetch(cn, new Criteria(0, 1));
        }

        internal static ARIVCSDDetValues New()
        {

            ARIVCSDDetValues obj = new ARIVCSDDetValues();

            return obj;
        }

        internal static ARIVCSDDetValues New(SqlConnection cn)
        {

            ARIVCSDDetValues obj = new ARIVCSDDetValues(cn);

            return obj;
        }

        public static ARIVCSDDetValues Get(int? headerKey)
        {

            ARIVCSDDetValues obj = new ARIVCSDDetValues();
            obj.Fetch(new Criteria(headerKey, 1));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _headerKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? HeaderKey, int? Option)
            {
                _headerKey = HeaderKey;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;

            try
            {

                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();

                    retValue = this.Fetch(cn, criteria);
                }// End of SqlConnection.


            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            try
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "ARIVCSDIVDetValues_Get";
                    cm.Parameters.AddWithValue("@Option", criteria._option);
                    cm.Parameters.AddWithValue("@DocKey", criteria._headerKey);
                    // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    try
                    {
                        sqlAdp.Fill(this);

                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }

                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                    else
                    {
                        throw new TAException(MsgID.Common.GetFail);
                    }


                }//using
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retValue;
        }

        #endregion //Data Access - Fetch

       

        

    }
}

