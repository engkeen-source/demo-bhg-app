using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSProcesss.
    /// </summary>
    [Serializable]
    public class SYSProcesss : ObservableCollection<SYSProcess>, IEnumerable<SYSProcess>
    {

        #region +++  Constructor  +++

        public SYSProcesss()
        {
        }
        public SYSProcesss Clone()
        {

            SYSProcesss objCopy = (SYSProcesss)this.MemberwiseClone();

            return objCopy;
        }
        #endregion


        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _CodeKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? CodeKey)
            {
                _CodeKey = CodeKey;
            }
            internal Criteria(int? CodeKey, int? Option)
            {
                _CodeKey = CodeKey;
                _option = Option;
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
            }

            return retValue;
        }
        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSProcess_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._CodeKey);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    while (dr.Read())
                    {
                        this.Add(SYSProcess.Get(dr));
                    }
                }// Already close and dispose data reader.
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.
            
        }
        #endregion //Data Access - Fetch

    }
}