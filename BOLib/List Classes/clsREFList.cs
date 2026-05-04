using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOLib
{
    public class REFList
    {
        public static DataTable GetAccGroups(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFAccGrp_Get", parmList);
        }

        public static DataTable GetBanks(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFBank_Get", parmList);
        }

        public static DataTable GetWorkOrderTypes(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFWorkOrderType_Get", parmList);
        }

        public static DataTable GetWORequestTypes(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFRequestType_Get", parmList);
        }

        public static DataTable GetColors(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFColor_Get", parmList);
        }

        public static DataTable GetDocGrps(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFDocGrp_Get", parmList);
        }

        public static DataTable GetEqptTypes(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFEqptType_Get", parmList);
        }

        public static DataTable GetIndustrys(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFIndustry_Get", parmList);
        }

        public static DataTable GetJobCostTypes(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFJobCostType_Get", parmList);
        }

        public static DataTable GetJobGrps(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFJobGrp_Get", parmList);
        }

        public static DataTable GetJobPhases(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFJobPhase_Get", parmList);
        }

        public static DataTable GetJobTasks(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFJobTask_Get", parmList);
        }

        public static DataTable GetJobPhaseTasks(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFJobPhaseTask_Get", parmList);
        }

        public static DataTable GetLocations(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFLoc_Get", parmList);
        }
        public static DataTable GetLocations(SqlConnection cn,out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc(cn,"ROREFLoc_Get", parmList);
        }
        public static DataTable GetBatchs(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTBatch_Get", parmList);
        }
        public static DataTable GetBatchs(SqlConnection cn, out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc(cn, "ROMSTBatch_Get", parmList);
        }
        public static DataTable GetSerial(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROMSTSerial_Get", parmList);
        }
        public static DataTable GetSerial(SqlConnection cn, out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc(cn, "ROMSTSerial_Get", parmList);
        }
        public static DataTable GetPackingTypes(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFPackingType_Get", parmList);
        }

        public static DataTable GetPayModes(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFPayMode_Get", parmList);
        }

        public static DataTable GetShipVias(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFShipVia_Get", parmList);
        }

        public static DataTable GetTerritorys(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFTerritory_Get", parmList);
        }

        public static DataTable GetIDFormats(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFIDFormat_Get", parmList);
        }

        public static DataTable GetOverHeads(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFOverHead_Get", parmList);
        }

        public static DataTable GetInterests(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFInterest_Get", parmList);
        }

        public static DataTable GetTerms(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFTerm_Get", parmList);
        }

        public static DataTable GetBrands(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFBrand_Get", parmList);
        }

        public static DataTable GetBrandDetItms(out string msgID, int BrandKey)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@BrankKey", BrandKey));
            parmList[1].Value = BrandKey;
            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFBrandDetItm_Get", parmList);
        }

        public static DataTable GetTaxAs(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFTaxA_Get", parmList);
        }

        public static DataTable GetTaxGrps(out string msgID, int option)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@Option", option));
            parmList[1].Value = option;
            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFTaxGrp_Get", parmList);
        }

        public static DataTable GetScales(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFScale_Get", parmList);
        }

        public static DataTable GetScaleDetItms(out string msgID, int ScaleKey)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@ScaleKey", ScaleKey));
            parmList[1].Value = ScaleKey;

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFScaleDetItm_Get", parmList);
        }

        public static DataTable GetREFAddr(out string msgID, int AddrLinkType)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@AddrLinkType", AddrLinkType));
            parmList[1].Value = AddrLinkType;

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFAddrItm_Get", parmList);
        }

        public static DataTable GetCurrs(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFCurr_Get", parmList);
        }

        public static DataTable GetUOMs(out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFUOM_Get", parmList);
        }
        public static DataTable GetUOMs(SqlConnection cn, out string msgID)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc(cn,"ROREFUOM_Get", parmList);
        }

        public static DataTable GetCats(out string msgID, int option, int CatNum)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", option));            
            parmList.Add(new SqlParameter("@CatNum", CatNum));
            parmList.Add(new SqlParameter("@MsgID", msgID));            
            parmList.Add(new SqlParameter("@RetValue", 0));
            parmList[0].Value = option;
            parmList[1].Value = CatNum;            

            parmList[2].Direction = ParameterDirection.InputOutput;
            parmList[3].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFCat_Get", parmList);
        }

        public static DataTable GetAddrs( GEnum.AddrLinkType AddrLinkType, int? AddrLinkKey)
        {
            // Initialize output 
            string msgID = string.Empty;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@Option", 0));
            parmList[0].Value = 0;
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@AddrLinkType", (int)AddrLinkType));
            parmList.Add(new SqlParameter("@AddrLinkKey", AddrLinkKey));        
            parmList[1].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFAddr_Get", parmList);
        }

        public static DataTable GetContactInfors(out string msgID, int ContactLinkType, int ContactLinkKey)
        {
            // Initialize output 
            msgID = MsgID.Common.GetFail;

            // Preapre parameter list (Require parameter at least @MsgID)
            List<SqlParameter> parmList = new List<SqlParameter>();
            parmList.Add(new SqlParameter("@MsgID", msgID));
            parmList.Add(new SqlParameter("@ContactLinkType", ContactLinkType));
            parmList.Add(new SqlParameter("@ContactLinkKey", ContactLinkKey));

            parmList[1].Value = ContactLinkType;
            parmList[2].Value = ContactLinkKey;

            parmList[0].Direction = ParameterDirection.InputOutput;

            // Execute method and return data table
            return GFunc.ExecuteProc("ROREFContactInfor_Get", parmList);
        }
    }
}