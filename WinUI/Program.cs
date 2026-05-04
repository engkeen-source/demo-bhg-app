using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BOLib;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic.ApplicationServices;
using System.Globalization;

namespace WinUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string AppToRun = string.Empty;                
                DTCSettings dtSetting = DTCUtility.ReadDTCSettings();
               
                string encrypted = "false";
                if (System.Configuration.ConfigurationManager.AppSettings["Encrypted"] != null)
                    encrypted = System.Configuration.ConfigurationManager.AppSettings["Encrypted"].ToString();
                AppInfor.BossSystemMasterConnectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["BOSSSystemMasterConnection"].ConnectionString;

                if (encrypted.ToLower().Equals("true"))
                    AppInfor.BossSystemMasterConnectionStr = TAUtil.Decoder.Decode(AppInfor.BossSystemMasterConnectionStr);

                //  args = new string[] { "AgentRun", "233", "1", "false", "17-11-2021", "16:23", "Database=BossBHM;Server=BHGSGPDB01;" };
                if (args.Length > 0)
                    AppToRun = args[0].ToString().ToUpper();

                AppDomain.CurrentDomain.UnhandledException +=
             new System.UnhandledExceptionEventHandler(OnUnhandledException);

                // Unhandled Forms exceptions will be delivered to our ThreadException handler
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(AppThreadException);
                // Thread Exit 
                Application.ThreadExit += new EventHandler(Application_ThreadExit);
                // Application Exit
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

                if (GFunc.CompareString(AppToRun, "AgentRun".ToUpper()))
                {
                    try
                    {
                        LoginFactory objLoginFactory = new BOLib.LoginFactory();
                        ////To Trace Code-----------------------------------
                        //int todoKey = 202;
                        //int userKey = 1;
                        //bool reminder = false;
                        //DateTime RunDateTime = DateTime.Now;

                        //objLoginFactory.ConnectionString = "Database=BossDemo;Server=TA-SVR01;user id=sa;Password=";
                        ////------------------------------------------------------------
                        int todoKey = Convert.ToInt32(args[1]);
                        int userKey = Convert.ToInt32(args[2]);
                        bool reminder = GFunc.NEBool(args[3], false);
                        DateTime RunDateTime = GFunc.NEDateTime(args[4] + " " + args[5], DateTime.Now);

                        objLoginFactory.ConnectionString = GFunc.NEStr(args[6].Trim(), "") + "user id=DevAdmin;password=BH609189";
                      //  objLoginFactory.ConnectionString = GFunc.NEStr(args[6].Trim(), "") + "user id=sa;Password=";
                        objLoginFactory.CallFromAgent = true;                       

                        //if (objLoginFactory.Login(userKey, true))
                        //{
                        //    ExternalAgent.Start(todoKey, reminder, RunDateTime);
                        //}
                        LoginFactory.Logoff();
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        FileStream fs = new FileStream(@"C:\Temp\TAAgentServiceErrorLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter m_streamWriter = new StreamWriter(fs);
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        m_streamWriter.WriteLine("Error:" + ex.Message + " - " + ex.StackTrace);
                        m_streamWriter.Flush();
                        m_streamWriter.Close();
                    }
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //Add message filter
                    //MessageFilter oFilter = new MessageFilter();
                    // System.Windows.Forms.Application.AddMessageFilter((IMessageFilter)oFilter);

                    // Clone the current culture so we can modity it
                    CultureInfo customCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();

                    // Fix abbreviated month names
                    customCulture.DateTimeFormat.AbbreviatedMonthNames =
                    new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };

                    // Also fix Genitive abbreviated month names
                    customCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames =
                    new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };

                    // Apply the custom culture to current thread
                    Thread.CurrentThread.CurrentCulture = customCulture;
                    Thread.CurrentThread.CurrentUICulture = customCulture;

                    Application.Run(new frmMain());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        // CLR unhandled exception
        private static void OnUnhandledException(Object sender, System.UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException(e.ExceptionObject);
        }

        /// <summary>
        /// Displays dialog with information about exceptions that occur in the application. 
        /// </summary>
        private static void AppThreadException(object source, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
        }

        static void HandleUnhandledException(Object o)
        {
            try
            {
                string error;
                Exception ex = o as Exception;
                if (ex != null)
                { // Report System.Exception info
                    error = ex.Message;
                }
                else
                { // Report exception Object info
                    error = o.ToString();
                }
                string errorMsg = error;

                MessageBox.Show(error, "Unexpected Error");

                if (AppInfor.SecurityKey != null && AppInfor.SecurityKey != Guid.Empty)
                    frmMain.gfrmMain.LogOff();
                Application.Exit(); // Shutting down
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (AppInfor.SecurityKey != null && AppInfor.SecurityKey != Guid.Empty)
                    frmMain.gfrmMain.LogOff();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static void Application_ThreadExit(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && AppInfor.SecurityKey != null && AppInfor.SecurityKey != Guid.Empty)
                    frmMain.gfrmMain.LogOff();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
