

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Collections.Generic;

namespace BOLib
{
    [Serializable()]
    public class MSTConOpenBals : DataTable
    {

        #region Factory Methods

        internal MSTConOpenBals()
        {
        }

        internal static MSTConOpenBals New()
        {            
            MSTConOpenBals obj = new MSTConOpenBals();            
            return obj;
        }
        internal static MSTConOpenBals New(SqlConnection cn,GEnum.SystemCode codeKey)
        {
            MSTConOpenBals obj = new MSTConOpenBals();
            obj.Fetch(cn,new Criteria(0, (int)codeKey, 1));
            return obj;
        }
        internal static MSTConOpenBals Get()
        {            
            MSTConOpenBals obj = new MSTConOpenBals();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        #endregion //Factory Methods 

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _conKey = null;
            public int? _option = null;
            public int? _codeKey = null;
            internal Criteria()
            {
            }

            internal Criteria(int? ConKey, int? Option)
            {
                _conKey = ConKey;
                _option = Option;
            }
            internal Criteria(int? ConKey,int? CodeKey, int? Option)
            {
                _conKey = ConKey;
                _option = Option;
                _codeKey=CodeKey;
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
                cm.CommandText = "MSTConOpenBal_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DocKey", 0);
                cm.Parameters.AddWithValue("@DocConKey", criteria._conKey);
                cm.Parameters.AddWithValue("@DocCodeKey", criteria._codeKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }//using            
        }

        #endregion //Data Access - Fetch



        internal bool Save(SqlConnection cn, Criteria criteria,int CodeKey)
        {
            DataTable dt = this.Copy();
            dt.TableName = "dtMST_ConOpenBal";
            string XMLMST_ConOpenBal = GFunc.ConvertDataTableToXML(dt);

            //Save the Detail Grid
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", Convert.ToInt16(criteria._option)));
            parmList.Add(new SqlParameter("@DocConKey", criteria._conKey));
            parmList.Add(new SqlParameter("@DocCodeKey", CodeKey));
            parmList.Add(new SqlParameter("@xmlDetail", XMLMST_ConOpenBal));
            parmList.Add(new SqlParameter("@RetValue", 0));
            parmList[4].Direction = ParameterDirection.Output;

            GFunc.ExecuteNonQueryProc(cn,"MSTConOpenBal_Save", parmList);

            return true;            
        }
    }
}

