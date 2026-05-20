using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOLib
{
    public class ARList
    {
        public static DataTable GetARQOs(DateTime FromDate,DateTime ToDate, out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));          
            parmList.Add(new SqlParameter("@Option", 1));                    
            parmList[0].Direction = ParameterDirection.Output;            
            parmList.Add(new SqlParameter("@FromDate", FromDate));
            parmList.Add(new SqlParameter("@ToDate", ToDate));

            // Execute method and return data table
            return GFunc.ExecuteProc("ROARQO_Get", parmList);
        }

        public static DataTable GetARPYs(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@Option", 0));
            parmList[1].Value = 0;
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROARPY_Get", parmList);
        }
    }
}
