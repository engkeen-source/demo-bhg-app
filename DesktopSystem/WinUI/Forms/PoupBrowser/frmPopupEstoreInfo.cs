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
    public partial class frmPopupEstoreInfo : Form
    {
        int ItmKey = 0;
        int entity_id = 0;       
        string ItmID = "";
        string mainUrl = "https://bh-estore.com/";
        string searchUrl = "https://bh-estore.com/searchanise/result?q=";
        string targetUrl;
        decimal estoreprice_website = 0;
        decimal estoreprice_synctable = 0;
        decimal estoreprice_controlprice = 0;
        DataTable dtSearchItm = null;

        public frmPopupEstoreInfo()
        {
            InitializeComponent();
        }        
        public frmPopupEstoreInfo(int itmKey, string itmID)
        {
            InitializeComponent();
            this.ItmKey = itmKey;
            this.ItmID = itmID;            
        }    
        public void Reload(int itmKey, string itmID)
        {
            this.ItmKey = itmKey;
            this.ItmID = ItmID;
            RefreshData();
        }
        private void frmPopupEstoreInfo_Load(object sender, EventArgs e)
        {
            //Get_eStoreWebsiteInfo(ItmID);
            //GetEstorePrice();
            RefreshData();
        }                      
        private void chkWebsitePrice_CustomUpdate(object sender, EventArgs e)
        {
            //numEstorePriceWeb.Visible = chkWebsitePrice.Checked;
            //if (chkWebsitePrice.Checked == true) Get_eStoreWebsiteInfo();
        }
        private void linkEstore_Click(object sender, EventArgs e)
        {
            //https://bh-estore.com/searchanise/result?q=B8011810
            //https://bh-estore.com/moflash-ae40m-24r-dc10-30v-ip65-112db-tone-sounder.html
            //https://bh-estore.com/

            //targetUrl = "https://bh-estore.com/moflash-ae40m-24r-dc10-30v-ip65-112db-tone-sounder.html"; /* testing */

            try
            {
                if (string.IsNullOrEmpty(targetUrl))
                {
                    GEnum.MsgBoxButton btnSelect = MsgBox.Show("This BH code cannot be found on the eStore.<br/>Whould you like to search for similar items?",
                                                       GEnum.MsgBoxIcon.Question,
                                                       GEnum.MsgBoxButton.Yes,
                                                       GEnum.MsgBoxButton.No);

                    if (btnSelect == GEnum.MsgBoxButton.Yes)
                        targetUrl = searchUrl + ItmID;
                    else
                        return;
                }
                else
                {
                    targetUrl = mainUrl + targetUrl;
                }

                // Open the URL in the default browser
                Process.Start(new ProcessStartInfo
                {
                    FileName = targetUrl,
                    UseShellExecute = true // UseShellExecute is required to open URLs in .NET Core/.NET 5+.
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open the link. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshData()
        {
            try
            {
                //Get_eStoreWebApiInfo(ItmID);

                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@Option", 0));
                parmList.Add(new SqlParameter("@ItmKey", ItmKey));
                parmList.Add(new SqlParameter("@ItmID", ItmID));
                parmList.Add(new SqlParameter("@Estoreprice_website", estoreprice_website));
                dtSearchItm = GFunc.ExecuteProcReader("Get_EstoreInfo", parmList);

                if (dtSearchItm != null && dtSearchItm.Rows.Count > 0)
                {
                    DataRow drSearchItm = dtSearchItm.Rows[0];
                    entity_id = GFunc.NEInt(drSearchItm["entity_id"], 0);
                    targetUrl = GFunc.NEStr(drSearchItm["producturl"], "");
                    lblItmID.Text = GFunc.NEStr(drSearchItm["ItmID"], "");
                    linkEstore.Text = linkEstore.Text.Replace("bhcode", GFunc.NEStr(drSearchItm["ItmID"], ""));
                    numEstorePriceSynTable.SetValueTrigger(drSearchItm["Price"], false);
                    numEstorePriceControlPrice.SetValueTrigger(drSearchItm["ControlPriceH"], false);
                    string color = GFunc.NEStr(drSearchItm["color"], "").ToLower();

                    if (lblMsgOrange.Tag.ToString().ToLower() == color) DisplayBold(lblMsgOrange);
                    else if (lblMsgKhaki.Tag.ToString().ToLower() == color) DisplayBold(lblMsgKhaki);
                    else if (lblMsgRed.Tag.ToString().ToLower() == color) DisplayBold(lblMsgRed);
                    else if (lblMsgTransparent.Tag.ToString().ToLower() == color) DisplayBold(lblMsgTransparent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DisplayBold(Label lbl)
        {
            if (lbl.Name.ToLower().Contains("red"))
            {
                lbl.Font = new Font(lbl.Font.FontFamily, 11, FontStyle.Regular);
                lbl.ForeColor = Color.Red;
            }
            else
            {
                lbl.Font = new Font(lbl.Font.FontFamily, 11, FontStyle.Bold);
                lbl.ForeColor = Color.Black;
            }
        }
        private void Get_eStoreWebsiteInfo()
        {
            string strSql = "";
            //strSql = "select ur.request_path,format(p.price,2) price from catalog_product_index_price p  inner join url_rewrite ur on ur.entity_id = p.entity_id  WHERE ur.entity_type = 'product' and customer_group_id = 0 and ur.entity_id = " + entity_id + " limit 1;";
            strSql = "SELECT format(p.price,2) as price from catalog_product_index_price p WHERE customer_group_id = 0 and p.entity_id = " + entity_id + ";";
            string strCon = "userid=afjmsnfvpe;password=nJUXgzG6PQ;server=172.104.41.102;database=afjmsnfvpe;connection timeout=1800";
            MySqlConnection mysqlcn = new MySqlConnection(strCon);
            MySqlCommand cmd = new MySqlCommand(strSql, mysqlcn);

            try
            {
                mysqlcn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    estoreprice_website = reader.GetDecimal("price");
                }
                else
                {
                    estoreprice_website = 0;
                }
                mysqlcn.Close();

                //if (entity_id == 0)
                //    numEstorePriceWeb.Clear();
                //else
                //    numEstorePriceWeb.SetValueTrigger(estoreprice_website, false);

            }
            catch (Exception ex)
            {
                mysqlcn.Close();
                MessageBox.Show("Connection Error");
            }
        }
        private async void Get_eStoreWebApiInfo(string bhCode)
        {
            try
            {
                //string apiUrl = "https://monitor.bh-estore.com/api/estore-product/H4744150";
                string apiUrl = "https://monitor.bh-estore.com/api/estore-product/" + bhCode;
                await FetchData(apiUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private async Task FetchData(string apiUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "WindowsFormsApp");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonData = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonData);

                    entity_id = GFunc.NEInt(jsonObject["entity_id"], 0);
                    targetUrl = GFunc.NEStr(jsonObject["url"],"") ;
                    estoreprice_website = GFunc.NEDec(jsonObject["price"],0);
                    lblEstorePriceWeb.Text = jsonObject["price"] != null ? Math.Round(estoreprice_website,4).ToString() : "N/A";

                    //string sku = (string)jsonObject["sku"];
                    //string name = (string)jsonObject["name"];
                    //int status = (int)jsonObject["status"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void GetEstorePrice()
        {
            try
            {
                //string apiUrl = "https://monitor.bh-estore.com/api/estore-products";
                string apiUrl = "https://monitor.bh-estore.com/api/estore-product/H4744150";
                DataTable dataTable = await FetchDataAndCreateDataTable(apiUrl);

                // Example: Display the data in a DataGridView
                DataGridView gridView = new DataGridView
                {
                    DataSource = dataTable,
                    Dock = DockStyle.Fill
                };
                Controls.Add(gridView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private async Task<DataTable> FetchDataAndCreateDataTable(string apiUrl)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();

            try
            {               
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "WindowsFormsApp");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();                    

                    string jsonData = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(jsonData);
                    JArray jData = (JArray)jsonObject["data"];

                    //XDocument xml = JsonConvert.DeserializeXNode(jData.ToString(), "Root"); //test

                    // Parse JSON into JArray
                    //JArray products = JArray.Parse(jsonData);

                    // Assume all items have the same keys
                    if (jData.Count > 0)
                    {
                        // Add columns dynamically based on the keys in the first JSON object
                        foreach (JProperty prop in ((JObject)jData[0]).Properties())
                        {
                            dataTable.Columns.Add(prop.Name, typeof(string));
                        }

                        // Add rows dynamically
                        foreach (JObject product in jData)
                        {
                            DataRow row = dataTable.NewRow();
                            foreach (JProperty prop in product.Properties())
                            {
                                row[prop.Name] = prop.Value?.ToString() ?? string.Empty;
                            }
                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            return dataTable;
        }
        private void chkVerifyWebsitePrice_CustomUpdate(object sender, EventArgs e)
        {
            lblEstorePriceWeb.Visible = chkVerifyWebsitePrice.Checked;
        }
    }
}
