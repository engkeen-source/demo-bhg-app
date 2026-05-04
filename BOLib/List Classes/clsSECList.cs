using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOLib
{
    public class SECList
    {
        public static DataTable GetGroups()
        {
            // Initialize output 
           

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            
            parmList.Add(new SqlParameter("@Option", 0));
            parmList[0].Value = 0;           

            // Execute method and return data table
            return GFunc.ExecuteProc("ROSECGrp_Get", parmList);
        }

        public static DataTable GetPermissions()
        {

            // Initialize output 
           

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            
            parmList.Add(new SqlParameter("@Option", 0));
            parmList[0].Value = 0;

            

            // Execute method and return data table
            return GFunc.ExecuteProc("ROSECPerm_Get", parmList);
        }

        public static DataTable GetUsers()
        {

            // Initialize output 
           

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            
            parmList.Add(new SqlParameter("@Option", 0));
            parmList[0].Value = 0;            

            // Execute method and return data table
            return GFunc.ExecuteProc("ROSECUser_Get", parmList);
        }
    }
}
