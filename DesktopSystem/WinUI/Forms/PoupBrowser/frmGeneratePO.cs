using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using Infragistics.Win.UltraWinTabbedMdi;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Documents.Excel;
using System.Text.RegularExpressions;
using Infragistics.Win.UltraWinMaskedEdit;
using System.Transactions;
using TAUtil;

namespace WinUI
{
    public partial class frmGeneratePO : Form
    {
        //Declaration
        DataTable _dtDetEstimate = null;
        private string ContextMenuSetting = string.Empty;
        int DocCodeKey = 0;
        int DocKey = 0;
       
        //Property
        public DataTable dtDetEstimate
        {
            get { return _dtDetEstimate; }
            set { _dtDetEstimate = value; }
        }

        public frmGeneratePO()
        {
            InitializeComponent();
        }
        public frmGeneratePO(int CodeKey,int JobKey, TAUtil.TAGridEditor tagrdDetEstimate)
        {
            InitializeComponent();
            dtDetEstimate =(DataTable) tagrdDetEstimate.DataSource;
            DocCodeKey = CodeKey;
            DocKey = JobKey;
           
        }

        //Form Event
        private void frmGeneratePO_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);

                //Fill the list of all combos in Form and Grid / Clear ErrorProvider
                GlobalUI.Combos_Fill(this, 0);

                LoadData();          
                GridCellLock(true);               
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
        }
        private void frmGeneratePO_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //(F9) - Refresh all combo list
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, 0);
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
        }
      
        //Button Events
        private void bntOK_Click(object sender, EventArgs e)
        {
            try
            {
                int TotalPO = 0;

                IEnumerable<DataRow> dtDetEstFilter = dtDetEstimate.AsEnumerable().Where(r => r.Field<bool?>("Selected")==true && r.Field<int?>("DocVendorKey") > 0);

                frmAPPO FrmAPPO = new frmAPPO(GEnum.SystemCode.Purchase_Order);
                FrmAPPO.MdiParent = frmMain.gfrmMain;
                FrmAPPO.Show();
                FrmAPPO.Hide();

                if (!FrmAPPO.CreatePO(DocCodeKey, DocKey, "", dtDetEstFilter.CopyToDataTable()))
                {
                    MsgBox.Show("One or more PO cannot be created, please check your entries");
                }               

                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@JobKey", DocKey));
                paraList.Add(new SqlParameter("@Option", 1));
                SqlParameter RetValue = new SqlParameter();
                RetValue.ParameterName = "@RetValue";
                RetValue.Value = 0;
                RetValue.Direction = ParameterDirection.InputOutput;
                paraList.Add(RetValue);
                DataTable dt = GFunc.ExecuteProcDataSet("MSTJobDetEst_Get", paraList).Tables[0];
                if (GFunc.NEInt(RetValue.Value, 0) == (int)GEnum.SpState.Fail)
                {
                    MsgBox.Show("fail.");
                }

                foreach (DataRow _dr in dt.Rows)
                {
                    foreach (DataRow dr in dtDetEstimate.Rows)
                    {
                        //header
                        if (GFunc.NEInt(dr["Selected"], 0) == 1 && GFunc.NEInt(dr["JobEstKey"], 0) == GFunc.NEInt(_dr["JobEstKey"], 0))
                        {
                            dr["DocID"] = GFunc.NEStr(_dr["DocID"], string.Empty);
                            dr["DocDK"] = GFunc.NEInt(_dr["DocDK"], 0);
                            dr["DocDItm"] = GFunc.NEInt(_dr["DocDItm"], 0);
                        }
                    }
                    TotalPO++;
                }
                LoadData();

                GridCellLock(false);
                tagrdGeneratePO.UpdateData();

                this.bntOK.Enabled = false;
            }
            catch (TAException ex)
            {
                Error(ex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            dtDetEstimate.AcceptChanges();
            this.DialogResult = DialogResult.Cancel;
        }

        //Custom function
        private void LoadData()
        {
            try
            {
                dtDetEstimate.AcceptChanges();

                var _dt = from row in dtDetEstimate.AsEnumerable()
                          where !GFunc.IsNE(row.Field<int?>("DocVendorKey"))  && (row.Field<Boolean?>("Selected") == true )                      
                          group row by new
                          {
                              DocVendorKey = row.Field<int>("DocVendorKey"),
                              DocVendorName = row.Field<string>("DocVendorNm"),
                              DocCurrKey = row.Field<int>("DocCurrKey") 
                          } into grp

                          let DocVendorID = grp.Max(a => a["DocVendorID"])                                        
                          let DocID = grp.Max(a => a["DocID"])

                          select new
                          {
                              DocVendorKey = grp.Key.DocVendorKey,
                              DocCurrKey = grp.Key.DocCurrKey,
                              DocVendorID = DocVendorID,
                              DocVendorNm = grp.Key.DocVendorName,                              
                              DocID = DocID 
                          };

                tagrdGeneratePO.DataSource = _dt.AsDataTable();
                tagrdGeneratePO.DataBind();

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
        }
        private void GridCellLock(Boolean IsFormLoad)
        {
            if (IsFormLoad)
            {
                tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocCurrKey"].Hidden = false;
                tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocID"].Hidden = true;
            }
            else
            {
                tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocCurrKey"].Hidden = true;
                tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocID"].Hidden = false;
            }

            tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocVendorID"].CellActivation = Activation.ActivateOnly;
            tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocVendorNm"].CellActivation = Activation.ActivateOnly;           
            tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocCurrKey"].CellActivation = Activation.ActivateOnly;
            tagrdGeneratePO.DisplayLayout.Bands[0].Columns["DocID"].CellActivation = Activation.ActivateOnly;

            


        }

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
