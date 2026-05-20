using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using System.Transactions;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
namespace WinUI
{
    public partial class frmDocVendSendListSelect : Form
    {
        //Variables Declaration 
        private string ContextMenuSetting = string.Empty;
        private Document CallerobjDoc;        
        DataTable dtCallerItmVendor = null;     //Caller ARQODetItmVendor
        DataTable dtCallerItm = null;           //Caller ARQODetItm
        
        //Initialization
        public frmDocVendSendListSelect()
        {
            InitializeComponent();
        }//Completed
        public frmDocVendSendListSelect(Document doc, TAUtil.TAGridEditor tagrdDetItm,TAUtil.TAGridEditor tagrdDetItmVendors)
        {
            //Call from ARQO EditSentList button
            //this form allows the user to select the ARQODetItm to Send for each Vendor
            InitializeComponent();

            this.CallerobjDoc = doc; 
            this.dtCallerItmVendor= (DataTable) tagrdDetItmVendors.DataSource;
            this.dtCallerItm = (DataTable) tagrdDetItm.DataSource;
        }//Completed

        //Form Events
        private void frmDocVendSendListSelect_Load(object sender, EventArgs e)
        {
            try
            {
                //Set ContextMenu & Grid Setting   
                GlobalUI.FormGrids_Set(this, (int)CallerobjDoc.DocCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)CallerobjDoc.DocCodeKey);
                GlobalUI.Combos_Fill(this, (int)CallerobjDoc.DocCodeKey);

                ((DataTable)tagrdVendors.DataSource).Clear();
                ((DataTable)tagrdVendors.DataSource).Merge(dtCallerItmVendor,false,MissingSchemaAction.Ignore);

                FormLayout();
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void frmDocVendSendListSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed
        private void frmDocVendSendListSelect_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)CallerobjDoc.DocCodeKey);
                }
                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed

        //Control Events
        private void Vendor_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (GFunc.NEInt(Vendor.Value, 0) > 0)
                    ((DataTable)tagrdVendors.DataSource).DefaultView.RowFilter = "VendorKey = " + GFunc.NEInt(Vendor.Value, 0).ToString();
                else
                    ((DataTable)tagrdVendors.DataSource).DefaultView.RowFilter = "";
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed

        //Button Events
        private void btnOk_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                dtCallerItmVendor.Clear();
                dtCallerItmVendor.Merge(((DataTable)tagrdVendors.DataSource), false, MissingSchemaAction.Ignore);
                this.DialogResult = DialogResult.OK;
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void btnClose_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                this.DialogResult = DialogResult.Cancel;
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
     
        //Methods
        private void FormLayout()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                //Setup Column Activation in list
                if (tagrdVendors.DisplayLayout.Bands.Count > 0)
                {
                    foreach (UltraGridColumn vCol in tagrdVendors.DisplayLayout.Bands[0].Columns)
                    {
                        if (vCol.Key != "Selected")
                            vCol.CellActivation = Activation.ActivateOnly;
                    }
                }

                //Update Item Information
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow col_ItmVend in tagrdVendors.Rows)
                {
                    //dtCallerItm.DefaultView.RowFilter = "DocItmKey = '" + col_ItmVend.Cells["DocItmKey"].Value + "'";
                    IEnumerable<DataRow> dtCallerItmFilter = dtCallerItm.AsEnumerable().Where(r => r.Field<int>("DocItmKey") ==GFunc.NEInt( col_ItmVend.Cells["DocItmKey"].Value,0));
                    if (dtCallerItmFilter.Count() > 0)
                    {
                        col_ItmVend.Cells["ItmID"].Value = dtCallerItmFilter.ElementAt(0)["ItmID"];
                        col_ItmVend.Cells["ItmDes"].Value = dtCallerItmFilter.ElementAt(0)["ItmDes"];
                    }
                   // dtCallerItm.DefaultView.RowFilter = "";
                }
                tagrdVendors.UpdateData();

                //Fill Vendor Combo
                var vendors = (from row in dtCallerItmVendor.AsEnumerable()
                               select new
                               {
                                   VendorKey = row.Field<int>("VendorKey"),
                                   VendorID = row.Field<string>("VendorID"),
                               }).Distinct();

                Vendor.DataSource = vendors.AsDataTable();
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed

        //Set Error Methods
        private Exception Error(Exception ex, bool ShowMessage)
        {
            Exception l_tmpex = ex;
            try
            {
                if (this.ActiveControl != null)
                {
                    if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                    }
                    else
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
                    }
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { });
                }
                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }

                return l_tmpex;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return l_tmpex;
        }
        private TAException Error(TAException ex, bool ShowMessage)
        {
            try
            {
                TAException l_tmpex = ex;
                if (this.ActiveControl != null)
                {
                    if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                    }
                    else
                    {
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
                    }
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { });
                }
                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }

                return l_tmpex;
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }

        
        
    }
}
