using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using System.Data.OleDb;
using System.Data;
using BOLib;
using System.Runtime.InteropServices;

namespace WinUI
{
    
    /// <summary>
    /// This class implements support for context sensitive help, including runtime 
    /// behavior of looking up the appropriate help file and launching help  
    /// ------
    /// Assume development time behavior of creating mappings between
    /// a user control context and an HTML help page should be done in struct documentation or in some datasource.
    /// </summary>
    public static class HelpUtility
    {
        /// <summary>
        /// The local cache of ContextID to HTMLHelp Paths mapping.
        /// </summary>
        //private const string mc_sHTMLPATH_ATTRIBUTE_NAME = "HTMLPath";
        private const string mc_sHELPFILE = "BossEasyHelp.chm";

        /// <summary>
        /// Return the path to the CHM file.
        /// </summary>
        private static string HelpFilePath { get { return Path.Combine(System.Windows.Forms.Application.StartupPath, mc_sHELPFILE); } }

        /// <summary>
        /// Display the specified help page.
        /// </summary>
        /// <param name="sHTMLHelp"></param>
        private static bool ShowHelp(string sHTMLHelp)
        {
            try
            {
                if (string.IsNullOrEmpty(sHTMLHelp))
                    Help.ShowHelp(frmMain.gfrmMain, HelpUtility.HelpFilePath);
                else
                    Help.ShowHelp(frmMain.gfrmMain, HelpUtility.HelpFilePath, HelpNavigator.Topic, sHTMLHelp);
            }
            catch (ArgumentException)
            {
                // Ideally, we would return false when the HTML file isn't found in the CHM file.
                // Unfortunately, there doesn't seem to be a way to do this.  
                return false;
            }
            return true;
        }             

        /// <summary>
        /// Process an F1 Request
        /// </summary>
        /// <param name="ctrContext"></param>
        public static void ProcessHelpRequest(Control ctrContext)
        {
            ActiveFormInfo activeFormInfo = GlobalUI.GetControlNmHierarchy();

            if (activeFormInfo != null)
            {
                //Retrieve the selected control's related help page
                string helPage = RetrieveHelpPageByControl(activeFormInfo);
                if (!GFunc.IsNE(helPage))
                    ShowHelp(helPage);
            }
        }

        private static string RetrieveHelpPageByControl(ActiveFormInfo activeFormInfo)
        {
            string helpPageNm = string.Empty;
            System.Data.OleDb.OleDbConnection conn = new 
            System.Data.OleDb.OleDbConnection();            
            conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            @"Data source= D:\BossHelp.mdb;"; //connection information Can be stored inside appconfig file
            try
            {
                //For some type of forms, we will use default single home page for entire form. so we make them here to refer to specific single help page
                switch (activeFormInfo.formNm )
                {
                    case "frmSYSOption":
                    case "frmReportDirectory":
                        activeFormInfo.controlOrCellNm = "NA";
                        activeFormInfo.gridNm = "NA";
                        break;
                }


                conn.Open();
                OleDbCommand command=new OleDbCommand("Select * from controlHelpMatching where CodeKey=@codeKey " +
                    " and FormNm=@formNm and GridNm=@gridNm and ControlOrCellNm=@controlOrCellNm",conn);
                command.Parameters.AddWithValue("codeKey", activeFormInfo.codeKey);
                command.Parameters.AddWithValue("formNm",activeFormInfo.formNm);
                command.Parameters.AddWithValue("gridNm",activeFormInfo.gridNm);
                command.Parameters.AddWithValue("controlOrCellNm", activeFormInfo.controlOrCellNm);
                OleDbDataAdapter adapter=new OleDbDataAdapter(command);
                DataTable dt=new DataTable();
                adapter.Fill(dt);
                if(dt.Rows.Count>0)
                    helpPageNm= dt.Rows[0]["HelpPage"].ToString();            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source : "+ex);
            }
            finally
            {
                conn.Close();
            }
            return helpPageNm;
        }
        
        
    }

    public sealed class StringLogicalComparer : IComparer
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct DllVersionInfo
        {
            public int cbSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformID;
        }

        [DllImport("shlwapi.dll", EntryPoint = "DllGetVersion", SetLastError = true)]
        private static extern int GetShlWapiVersion(ref DllVersionInfo version);

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        private static extern int StrCmpLogicalW(string strA, string strB);

        private static readonly bool _isSupported;
        private static readonly IComparer _default;

        static StringLogicalComparer()
        {
            DllVersionInfo ver = new DllVersionInfo();
            ver.cbSize = Marshal.SizeOf(ver);
            try
            {
                GetShlWapiVersion(ref ver);
            }
            catch
            {
            }

            _isSupported = 5 < ver.dwMajorVersion || (5 == ver.dwMajorVersion && 5 <= ver.dwMinorVersion);

            if (_isSupported)
                _default = new StringLogicalComparer();
            else
                _default = CaseInsensitiveComparer.DefaultInvariant;
        }

        public StringLogicalComparer()
        {
        }

        public static IComparer Default
        {
            get { return _default; }
        }

        public static bool IsSupported
        {
            get { return _isSupported; }
        }

        public int Compare(object x, object y)
        {
            string left = x as string;
            if (null != left && 0 != left.Length)
            {
                string right = y as string;
                if (null != right && 0 != right.Length)
                {
                    try
                    {
                        return StrCmpLogicalW(left, right);
                    }
                    catch
                    {
                    }
                }
            }

            return Comparer.Default.Compare(x, y);
        }
    }

}
