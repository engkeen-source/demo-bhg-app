using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOLib
{
    public class MASList
    {
        internal static DataTable GetItemList()
        {
            // Initialize output 
            string msgID = MsgID.Common.GetFail;
            // Execute method and return data table
            return GFunc.ExecuteProc("RO_Items_Get", null);
        }

        public static DataTable GetMSTAccs(out string msgID, GEnum.AccTypeKey accType, bool inactive)
        {
            int option = 0;

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", option));
            parmList.Add(new SqlParameter("@AccType", (int)accType));
            parmList.Add(new SqlParameter("@AccTypeKey", 0));
            parmList.Add(new SqlParameter("@Inactive", inactive));
            //parmList[1].Value = option;
            //parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTAcc_Get", parmList);
        }

        public static DataTable GetMSTAccs(out string msgID, int accTypeKey, bool inactive)
        {
            int option = 1;

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();

            parmList.Add(new SqlParameter("@Option", option));
            parmList.Add(new SqlParameter("@AccType", null));
            parmList.Add(new SqlParameter("@AccTypeKey", accTypeKey));
            parmList.Add(new SqlParameter("@Inactive", inactive));
            parmList[1].Value = option;
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTAcc_Get", parmList);
        }
        public static DataTable GetMSTAccDepts(int option)
        {

            // Initialize output 
            string msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", option));
            //parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTAccDept_Get", parmList);
        }
        public static DataTable GetMSTAccDepts(out string msgID, int option)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", option));
            //parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTAccDept_Get", parmList);
        }

        public static DataTable GetMSTAccBranchs()
        {

            // Initialize output 
            string msgID = string.Empty;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            //parmList.Add(new SqlParameter("@MsgID", msgID));
            //parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTAccBranch_Get", parmList);
        }

         public static DataTable GetAccessGrp()
        {

            // Initialize output 
            string msgID = string.Empty;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            //parmList.Add(new SqlParameter("@MsgID", msgID));
            //parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("SEC_Grp_Get", parmList);
        }
        
        public static DataTable GetMSTConCusts(out string msgID, GEnum.ConTypeCust conType, GEnum.CCBType ccbType, bool inactive)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            //parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@ConType", conType.ToString()));
            parmList.Add(new SqlParameter("@CCBType", (int)ccbType));
            parmList.Add(new SqlParameter("@UserAccessLevel", AppInfor.conAccessLevel));
            parmList.Add(new SqlParameter("@UserAccessGroup", AppInfor.conAccessGroup));
            parmList[2].Value = 0;
            parmList.Add(new SqlParameter("@Inactive", inactive));
            //parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTConCust_Get", parmList);
        }
        public static DataTable GetMSTConVends(out string msgID, GEnum.ConTypeVend conType, GEnum.CCBType ccbType, bool inactive)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            //parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@ConType", conType.ToString()));
            parmList.Add(new SqlParameter("@CCBType", (int)ccbType));
            parmList.Add(new SqlParameter("@UserAccessLevel", AppInfor.conAccessLevel));
            parmList.Add(new SqlParameter("@UserAccessGroup", AppInfor.conAccessGroup));
            parmList.Add(new SqlParameter("@Inactive", inactive));
            //parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTConVend_Get", parmList);
        }

        public static DataTable GetMSTEqpts(out string msgID, bool templateYN, int eqptConKey, string eqptStatus, int option)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@TemplateYN", templateYN));
            parmList.Add(new SqlParameter("@EqptConKey", eqptConKey));
            parmList.Add(new SqlParameter("@EqptStatus", eqptStatus));
            parmList.Add(new SqlParameter("@Option", option));
            if (option == 0)
                parmList[4].Value = 0;
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTEqpt_Get", parmList);
        }

        public static DataTable GetMSTFinMains(out string msgID)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTFinMain_Get", parmList);
        }

        public static DataTable GetMSTConOpenBalCusts(out string msgID, int docCodeKey, int docConKey)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@DocCodeKey", docCodeKey));
            parmList.Add(new SqlParameter("@DocConKey", docConKey));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTConOpenBalCust_Get", parmList);
        }

        public static DataTable GetMSTConOpenBalBVends(out string msgID, int docCodeKey, int docConKey)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@DocCodeKey", docCodeKey));
            parmList.Add(new SqlParameter("@DocConKey", docConKey));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTConOpenBalVend_Get", parmList);
        }

        public static DataTable GetMSTItms(out string msgID, GEnum.INType itmType, bool inactive)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@ItmType", itmType.ToString()));
            parmList.Add(new SqlParameter("@Inactive", inactive));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTItm_Get", parmList);
        }

        public static DataTable GetMSTItmBatchs(out string msgID, int batchItmKey, bool batchStatus)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@BatchItmKey", batchItmKey));
            parmList.Add(new SqlParameter("@BatchStatus", batchStatus));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTItmBatch_Get", parmList);
        }

        public static DataTable GetMSTItmSerials(out string msgID, int itmKey, int batchKey, int itmStatus)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@ItmKey", itmKey));
            parmList.Add(new SqlParameter("@BatchKey", batchKey));
            parmList.Add(new SqlParameter("@ItmStatus", itmStatus));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTItmBatch_Get", parmList);
        }

        public static DataTable GetMSTJobs(out string msgID, int option, int jobGrpKey, int jobConKey, string jobClass, int jobStatus)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", option));
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@JobGrpKey", jobGrpKey));
            parmList.Add(new SqlParameter("@JobConKey", jobConKey));
            parmList.Add(new SqlParameter("@JobClass", jobClass));
            parmList.Add(new SqlParameter("@JobStatus", jobStatus));
            parmList[1].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTJob_Get", parmList);
        }

        public static DataTable GetMSTPriceLists(out string msgID, int priceType, int currKey, int buildInCode)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@PriceType", priceType));
            parmList.Add(new SqlParameter("@CurrKey", currKey));
            parmList.Add(new SqlParameter("@BuildInCode", buildInCode));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTPriceList_Get", parmList);
        }

        public static DataTable GetMSTSalesReps(out string msgID, string emClass, int userKey, bool inactive)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@EmClass", emClass));
            parmList.Add(new SqlParameter("@UserKey", userKey));
            parmList.Add(new SqlParameter("@Inactive", inactive));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTSalesRep_Get", parmList);
        }

        public static DataTable GetMSTAccTranGrps(int option, int? tranGrpKey)
        {

            // Initialize output 
            string msgID = string.Empty;
            bool retValue = false;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", option));
            parmList.Add(new SqlParameter("@TranGrpKey", tranGrpKey));
            parmList.Add(new SqlParameter("@RetValue", retValue));

            parmList[2].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("MSTAccTranGrp_Get", parmList);
        }

        public static DataTable GetMSTShipNames(out string msgID, int? conKey)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@Option", 0));
            parmList[1].Value = 0;
            parmList.Add(new SqlParameter("@Key", conKey));
            //parmList.Add(new SqlParameter("@EmClass", emClass));
            //parmList.Add(new SqlParameter("@UserKey", userKey));
            //parmList.Add(new SqlParameter("@Inactive", inactive));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTShip_Get", parmList);
        }
        public static DataTable GetMSTShipMarks(out string msgID, int? ShipNameKey)
        {

            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@Option", 1));
            parmList.Add(new SqlParameter("@Key", ShipNameKey));

            //parmList.Add(new SqlParameter("@EmClass", emClass));
            //parmList.Add(new SqlParameter("@UserKey", userKey));
            //parmList.Add(new SqlParameter("@Inactive", inactive));
            parmList[0].Direction = ParameterDirection.Output;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTShip_Get", parmList);
        }

        public static DataTable GetVehicle(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTVehicle_Get", parmList);
        }
    }
}