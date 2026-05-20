using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json;
/* 
* Created by YST on 2024/12/05 
*/
namespace WinUI
{
    public partial class frmPopupWMSInfo : Form
    {
        string ItmID = "";            
        string CheckCol = "";       

        public frmPopupWMSInfo()
        {
            InitializeComponent();
        }        
        public frmPopupWMSInfo(string itmID, string checkCol)
        {
            InitializeComponent();
            this.ItmID = itmID;
            this.CheckCol = checkCol;
            if (checkCol.ToLower().Contains("pick")) this.Text = "WMS Picking In Progress";
            else if (checkCol.ToLower().Contains("receive")) this.Text = "WMS Receiving In Progress";
            else if (checkCol.ToLower().Contains("kit")) this.Text = "WMS Kitting Record";
            else if (checkCol.ToLower().Contains("goods")) this.Text = "WMS Inventory Allocation";

            this.MinimumSize = new Size(400, 300);
            this.MaximumSize = new Size(1000, 600);
        }    
        public void Reload(string itmID, string checkCol)
        {
            this.ItmID = itmID;
            this.CheckCol = checkCol;
            RefreshData();
        }
        private void frmPopupWMSInfo_Load(object sender, EventArgs e)
        {
            RefreshData();
        }  
        
        private void RefreshData()
        {
            string strHtml = "";
            try
            {                
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@Option", 0));
                parmList.Add(new SqlParameter("@ItmID", ItmID));
                parmList.Add(new SqlParameter("@CheckCol", CheckCol));
                strHtml = GFunc.ExecuteScalar("Get_WMSInfo", parmList);
                webBrowser1.DocumentText = strHtml;
            }
            catch(Exception ex)
            {
                webBrowser1.DocumentText = "<html>" + CheckCol + "</html>";
            }
            
        }
        
    }
}
