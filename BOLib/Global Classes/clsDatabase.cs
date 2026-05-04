using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace BOLib
{
    public static class Database
    {
        public static string BossDemoConnection
        {
            get
            {
                return AppInfor.CurrentDBConnectionStr;
            }
            set
            {
                AppInfor.currentDBConnectionStr = value;
            }
        }

        public static string BOSSSystemMasterConnection
        {
            get
            {
                return AppInfor.BossSystemMasterConnectionStr;                
            }
        }

        //public static string BossMFGConnection
        //{
        //    get
        //    {
        //        return ConfigurationManager.ConnectionStrings["BossMFGConnection"].ConnectionString;
        //    }
        //}        

        public static string GetDMASConnection()
        {
            string encrypted = "false";
            string DMASConStr = System.Configuration.ConfigurationManager.ConnectionStrings["DMASDocConnection"].ConnectionString;
            if (System.Configuration.ConfigurationManager.AppSettings["Encrypted"] != null)
                encrypted = System.Configuration.ConfigurationManager.AppSettings["Encrypted"].ToString();

            if (encrypted.ToLower().Equals("true"))
                DMASConStr = TAUtil.Decoder.Decode(DMASConStr);

            return DMASConStr;

        }
    }
}
