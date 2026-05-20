using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using BOLib;

namespace WinUI.Forms
{
    partial class frmAboutBox : Form
    {
        public frmAboutBox()
        {
            InitializeComponent();            
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void AboutBox1_Load(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("ComponentName");
            dt.Columns.Add("Version");
            dt.Rows.Add(new object[] { "Program Version", AppInfor.ProgramVersion });
            dt.Rows.Add(new object[] { "Database Version", AppInfor.DatabaseVersion });
            tagrdVersionInfo.DataSource = dt;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopyInfo_Click(object sender, EventArgs e)
        {
            DataTable dtSource=tagrdVersionInfo.DataSource as DataTable;
            String verInfo = string.Empty;
            foreach (DataRow row in dtSource.Rows)
            {
                verInfo += row["ComponentName"].ToString() + " \t " + row["Version"] + " \r\n";
            }
            System.Windows.Forms.Clipboard.SetText(verInfo,TextDataFormat.Text);
        }
    }
}
