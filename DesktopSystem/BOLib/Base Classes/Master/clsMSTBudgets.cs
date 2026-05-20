

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
    public class MSTBudgets : DataTable
    {

        #region Factory Methods

        internal MSTBudgets()
        {
        }

        internal static MSTBudgets New() 
        {            
            MSTBudgets obj = new MSTBudgets();            
            return obj;
        }

        internal static MSTBudgets New(SqlConnection cn)
        {
            MSTBudgets obj = new MSTBudgets();
            obj.Fetch(cn, new Criteria(0, 0, 1));
            return obj;
        }

        internal static MSTBudgets Get()
        {
            MSTBudgets obj = new MSTBudgets();
            obj.Fetch(new Criteria(0, 0, 0, 0, 0, 0,0,0));
            return obj;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _budgetType = 0;
            public int? _budgetBranchKey = 0;
            public int? _budgetDeptKey = 0;
            public int? _budgetRecKey = 0;
            public int? _budgetRecSubKey = 0;
            public int? _periodFrom = 0;
            public int? _periodTo = 0;
            public int? _option = 0;

            internal Criteria(int? periodFrom, int? periodTo,int? option)
            {
                _periodFrom = periodFrom;
                _periodTo = periodTo;
                _option = option;
            }

            internal Criteria(int? BudgetType, int? BudgetBranchKey, int? BudgetDeptKey, int? BudgetRecKey, int? BudgetRecSubKey,int? periodFrom,int? periodTo, int? Option)
            {
                _budgetType = BudgetType;
                _budgetBranchKey = BudgetBranchKey;
                _budgetDeptKey = BudgetDeptKey;
                _budgetRecKey = BudgetRecKey;
                _budgetRecSubKey = BudgetRecSubKey;
                _periodFrom = periodFrom;
                _periodTo = periodTo;
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
            }// End of SqlConnection.
            
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTBudget_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);                   
                cm.Parameters.AddWithValue("@BudgetType", criteria._budgetType);
                cm.Parameters.AddWithValue("@BudgetBranchKey", criteria._budgetBranchKey);             
                cm.Parameters.AddWithValue("@BudgetDeptKey", criteria._budgetDeptKey);
                cm.Parameters.AddWithValue("@BudgetRecKey", criteria._budgetRecKey);
                cm.Parameters.AddWithValue("@BudgetRecSubKey", criteria._budgetRecSubKey);
                cm.Parameters.AddWithValue("@BudgetPeriod", criteria._periodFrom);
                cm.Parameters.AddWithValue("@BudgetPeriodTo", criteria._periodTo);

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

        internal bool Save(SqlConnection cn, Criteria criteria, int CodeKey)
        {
            DataTable dt = this.Copy();
            dt.TableName = "dtMST_Budget";
            string XMLMST_Budget = GFunc.ConvertDataTableToXML(dt);

            //Save the Detail Grid
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", Convert.ToInt16(criteria._option)));
            //parmList.Add(new SqlParameter("@DocConKey", criteria._conKey));
            //parmList.Add(new SqlParameter("@DocCodeKey", CodeKey));
            //parmList.Add(new SqlParameter("@xmlDetail", XMLMST_ConOpenBal));
            parmList.Add(new SqlParameter("@RetValue", 0));
            parmList[4].Direction = ParameterDirection.Output;

            GFunc.ExecuteNonQueryProc(cn, "MSTConOpenBal_Save", parmList);

            return true;
        }
    }
}
