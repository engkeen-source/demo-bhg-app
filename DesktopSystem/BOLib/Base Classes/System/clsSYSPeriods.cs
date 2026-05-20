using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class SYSPeriods : Csla.BusinessListBase<SYSPeriods, SYSPeriod>
    {

        #region Factory Methods

        internal SYSPeriods()
        {
        }

        internal static SYSPeriods New()
        {
            //
            SYSPeriods obj = new SYSPeriods();
            //
            return obj;
        }

        public static SYSPeriods Get()
        {
           // 
            SYSPeriods obj = new SYSPeriods();
            obj.Fetch(new Criteria(0, 0));
            return obj;
        }

        public static SYSPeriods Get(int PeriodYear,int Option)
        {
            // 
            SYSPeriods obj = new SYSPeriods();
            obj.Fetch(new Criteria(PeriodYear, Option));
            return obj;
        }

        public static SYSPeriods Get(int PeriodYear,int PeriodSeq, int Option)
        {
            // 
            SYSPeriods obj = new SYSPeriods();
            obj.Fetch(new Criteria(PeriodYear,PeriodSeq, Option));
            return obj;
        }

        public static SYSPeriods Get(int PeriodYear, int PeriodSeq, int PeriodStatus, int Option)
        {
            // 
            SYSPeriods obj = new SYSPeriods();
            obj.Fetch(new Criteria(PeriodYear,PeriodSeq,PeriodStatus,Option));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _period = null;
            public int? _option = null;
            public int? _periodYear = null;
            public int? _periodSeq = 0;
            public int? _periodStatus = 0;

            internal Criteria()
            {
                _option = 0;
            }

            internal Criteria(int? PeriodYear, int? Option)
            {
                _periodYear = PeriodYear;
                _option = Option;
            }

            internal Criteria(int? Period, int? PeriodSeq, int? Option)
            {
                _period = Period;
                _periodSeq = PeriodSeq;
                _option = Option;
            }

            internal Criteria(int? Period, int? PeriodSeq, int PeriodStatus, int? Option)
            {
                _periodYear = 0;
                _period = Period;
                _periodSeq = PeriodSeq;
                _option = Option;
                _periodStatus = PeriodStatus;
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
                cm.CommandText = "SYSPeriod_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                if (criteria._period == null)
                    cm.Parameters.AddWithValue("@PeriodYear", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodYear", criteria._periodYear);

                if (criteria._period == null)
                    cm.Parameters.AddWithValue("@PeriodSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PeriodSeq", criteria._periodSeq);

                if (criteria._period == null)
                    cm.Parameters.AddWithValue("@Period", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Period", criteria._period);
               
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(SYSPeriod.Get(dr));
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
                        this.Add(SYSPeriod.Get(dr));
                }

               
                    return true;
                
        }

        #endregion //Data Access - Fetch       
    }
}

