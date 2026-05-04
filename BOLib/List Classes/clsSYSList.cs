using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace BOLib
{
    public class SYSList
    {
        public static DataTable GetMsgList(GEnum.SYSMsgList? option)
        {
            // Initialize output 
           
            int retValue = 0;
            DataTable retDataTable = null;

            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            p = new SqlParameter("@Option", (int)option);            
            parmList.Add(p);           
          

            p = new SqlParameter("@RetValue", retValue);
            p.Direction = ParameterDirection.Output;
            parmList.Add(p);

            retDataTable = GFunc.ExecuteProc("ROSysMsgList_Get", parmList);

            

            if (!GFunc.IsNE(parmList[1].Value))
                retValue = (int)parmList[1].Value;
            
            return retDataTable;
        }

        public static DataTable GetMsgListText( GEnum.MsgListTextGrp option)
        {
            // Initialize output 
            
            int retValue = 0;
            DataTable retDataTable = null;

            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            p = new SqlParameter("@Option", (int)option);
            parmList.Add(p);

          

            p = new SqlParameter("@RetValue", retValue);
            p.Direction = ParameterDirection.Output;
            parmList.Add(p);

            retDataTable = GFunc.ExecuteProc("ROSysMsgListText_Get", parmList);

           

            if (!GFunc.IsNE(parmList[1].Value))
                retValue = (int)parmList[1].Value;

            return retDataTable;
        }      

        public static DataTable GetSystemCodeList()
        {
            // Initialize output 
            List<SqlParameter> parmList = new List<SqlParameter>();
            SqlParameter p;
            p = new SqlParameter("@Option", 1);
            parmList.Add(p); 
            return GFunc.ExecuteProc("ROSystem_Get", parmList);

        }

        public static DataTable GetOptionListByUser(int option)
        {
          

            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            //Non sense,Option is used for future purpose
            p = new SqlParameter("@Option", option);
            parmList.Add(p);

            p = new SqlParameter("@OpUserKey", AppInfor.currentUserKey);
            parmList.Add(p);

            return GFunc.ExecuteProc("ROUserOption_Get", parmList);

        }
        public static DataTable GetDocTypesByCodeKey(int? codeKey,int? Option)
        {
            

            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            p = new SqlParameter("@Option", Option);
            p.Value = Option;
            parmList.Add(p);           

            p = new SqlParameter("@CodeKey", codeKey);            
            parmList.Add(p);
            return GFunc.ExecuteProc("RODocTypes_Get", parmList);

        }

        public static DataTable GetCounterGrpsByCodeKey(int? codeKey)
        {
           

            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            p = new SqlParameter("@Option", 1);            
            parmList.Add(p);

          
            p = new SqlParameter("@CodeKey", codeKey);
            parmList.Add(p);
            return GFunc.ExecuteProc("ROSysCode_Get", parmList);

        }

        public static DataTable GetDataGroups()
        {
           
            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            p = new SqlParameter("@Option", 0);
            p.Value = 0;
            parmList.Add(p);

           
            return GFunc.ExecuteProc("ROSystem_Get", parmList);

        }

        public static DataTable GetSystemPeriodMonths(int? PeriodStatus, int Option)
        {
            int retValue = 0;

            

            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            p = new SqlParameter("@Option", Option);
            parmList.Add(p);

            p = new SqlParameter("@PeriodStatus", PeriodStatus);
            parmList.Add(p);

            

            p = new SqlParameter("@RetValue", retValue);
            p.Direction = ParameterDirection.Output;
            parmList.Add(p);

            return GFunc.ExecuteProc("ROSysPeriod_Get", parmList);
        }

        public static bool GetFiscalPeriod(ref DateTime StartDate,ref DateTime EndDate)
        {          
            
            DataTable dt = GFunc.ExecuteProc("ROFiscalYear_Get", null);
           
            if (dt == null)
                return false;
            else
            {
                if(dt.Rows.Count>0)
                {
                    StartDate = GFunc.IsNE(dt.Rows[0]["StartDate"]) ?
                        new DateTime(1900, 1, 1) : Convert.ToDateTime(dt.Rows[0]["StartDate"]);
                    EndDate = GFunc.IsNE(dt.Rows[0]["EndDate"]) ?
                        new DateTime(1900, 1, 1) : Convert.ToDateTime(dt.Rows[0]["EndDate"]);

                    return true;
                }
            }
            return false;
        }

        //Option 3 will pass Security Key as RepKey
        public static DataTable GetReports( int option,int RepKey)
        {           

            System.Data.DataSet dsResult = new System.Data.DataSet();

            using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(BOLib.Database.BossDemoConnection))
            {
                System.Data.SqlClient.SqlCommand cm = sqlCon.CreateCommand();
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.CommandText = "ROSYSRep_Get";
                
                cm.Parameters.AddWithValue("@Option", option);
                cm.Parameters.AddWithValue("@RepKey", RepKey);
                

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                try
                {
                    sqlCon.Open();
                    sqlAdp.Fill(dsResult);
                    
                    return dsResult.Tables[0];
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public static DataTable GetExcelTemplateList(out string msgID, int option)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            List<SqlParameter> parmList = new List<SqlParameter>();

            SqlParameter p;
            p = new SqlParameter("@Option", option);
            parmList.Add(p);

            p = new SqlParameter("@msgID", msgID);
            p.Direction = ParameterDirection.Output;
            parmList.Add(p);

            p = new SqlParameter("@TemplateKey", 0);
            p.Value = 0;
            parmList.Add(p);

            p = new SqlParameter("@RetValue", msgID);
            p.Direction = ParameterDirection.Output;

            parmList.Add(p);

            return GFunc.ExecuteProc("SYSExcelTemplate_Get", parmList);

        }
    }
}
