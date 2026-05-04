using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BOLib
{
    public static class AppInfor
    {
        #region Member variables and constants

        public const string DatabaseVersion = "1.0.0.9";
        //version 1.0.1.0 - Pauk fixed Crystal report viewer printing option.
        //public const string ProgramVersion = "1.3.40";
        internal static string programVersion = "1.0.0.0";

        internal static string bossSystemMasterConnectionStr = string.Empty;
        internal static Guid securityKey;
        internal static int currentUserKey = 0;
        internal static string currentUserID = string.Empty;
        internal static string currentUserName = string.Empty;
        internal static string currentDBConnectionStr = string.Empty;
        internal static string cultureInfo = string.Empty;

        internal static string currentDBID = string.Empty;
        internal static string currentIdentifier = string.Empty;
        internal static string currentTimeStamp = string.Empty;
        internal static int conAccessLevel = 0;
        internal static int conAccessGroup = 0;
        internal static int jobAccessLevel = 0;
        internal static int jobAccessGroup = 0;
        internal static int itemAccessLevel = 0;
        internal static int itemAccessGroup = 0;

        internal static int branchKey = 0;
        internal static int deptKey = 0;
        internal static int tranGrpKey = 0;
        internal static int emKey = 0;
        #endregion

        #region Properties
        public static string ProgramVersion
        {
            get
            {
                return programVersion;
            }
        }
        public static Guid SecurityKey
        {
            get
            {
                return securityKey;
            }
        }

        public static int CurrentUserKey
        {
            get
            {
                return currentUserKey;
            }
        }


        public static string CurrentUserID
        {
            get
            {
                return currentUserID;
            }
        }

        public static string CurrentUserName
        {
            get
            {
                return currentUserName;
            }
        }
        public static string CurrentDBID
        {
            get
            {
                return currentDBID;
            }
        }

        public static string BossSystemMasterConnectionStr
        {
            get
            {
                return bossSystemMasterConnectionStr;
            }
            set
            {
                bossSystemMasterConnectionStr = value;
            }
        }

        public static string CurrentDBConnectionStr
        {
            get
            {
                return currentDBConnectionStr;
            }
        }
        public static string CurrentIdentifier
        {
            get
            {
                return currentIdentifier;
            }
        }
        public static int ConAccessLevel
        {
            get
            {
                return conAccessLevel;
            }
        }

        public static int ConAccessGroup
        {
            get
            {
                return conAccessGroup;
            }
        }
        public static int JobAccessLevel
        {
            get
            {
                return jobAccessLevel;
            }
        }

        public static int JobAccessGroup
        {
            get
            {
                return jobAccessGroup;
            }
        }
        public static int ItemAccessLevel
        {
            get
            {
                return itemAccessLevel;
            }
        }

        public static int ItemAccessGroup
        {
            get { return itemAccessGroup; }
        }

        public static int BranchKey
        {
            get { return branchKey; }
        }
        public static int DeptKey
        {
            get { return deptKey; }
        }
        public static int TranGrpKey
        {
            get { return tranGrpKey; }
        }
        public static int EMKey
        {
            get { return emKey; }
        }

        public static string CultureInfo
        {           
            get { return cultureInfo; }
        }

        #endregion

        #region Internal Methods
        internal static bool Initialisation()
        {            
            try
            {
                currentIdentifier = System.Net.Dns.GetHostName();
                /* added by YST on 2025-08-13 */
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                string abbreviatedMonthNames = "", gcCalendarType = "", gcAlgorithmType = "";

                foreach (string month in ci.DateTimeFormat.AbbreviatedMonthNames)
                {
                    abbreviatedMonthNames += month + (month.Contains("Dec") ? "" : ",");
                }

                if (ci.Calendar is System.Globalization.GregorianCalendar gc)
                {
                    gcAlgorithmType = gc.AlgorithmType.ToString();
                    gcCalendarType = gc.CalendarType.ToString();
                }
                cultureInfo = ci.Name + " // " + ci.DisplayName + " // " + ci.DateTimeFormat.Calendar.GetType().Name + " // " + gcAlgorithmType + " // " + gcCalendarType + " // " + abbreviatedMonthNames;
                /* end by YST */

            }
            catch { }

            return true;
        }

        internal static string GetMachineIP()
        {
            try
            {
                string ip = "";

                System.Net.IPAddress[] a =
                System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());

                for (int i = 0; i < a.Length; i++)
                {
                    ip += a[i].ToString();
                }
                return ip;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        internal static string GetVersion()
        {
            try
            {
                string filePath = @"c:\techace\TECHACEUpdater.exe.config";

                if(!File.Exists(filePath))
                    filePath = @"c:\techace\BossUpdater.exe.config";

                if (File.Exists(filePath))
                {
                    XDocument config = XDocument.Load(filePath);
                    string appVersion = config.Descendants("add")
                    .Where(x => (string)x.Attribute("key") == "ApplicationVersion")
                    .Select(x => (string)x.Attribute("value"))
                    .FirstOrDefault();
                    string reportVersion = config.Descendants("add")
                    .Where(x => (string)x.Attribute("key") == "ReportVersion")
                    .Select(x => (string)x.Attribute("value"))
                    .FirstOrDefault();

                    if (appVersion != null && appVersion.Length >= 3)
                        appVersion = appVersion.Substring(appVersion.Length - 3);
                    if (reportVersion != null && reportVersion.Length >= 3)
                        reportVersion = reportVersion.Substring(reportVersion.Length - 3);

                    programVersion = "1.0." + appVersion + "." + reportVersion + " ";
                   
                }

                return ProgramVersion;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        #endregion

        private static Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        
    }
}
