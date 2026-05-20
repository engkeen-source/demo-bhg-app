using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.Data.SqlClient;
using TAUtil;

namespace WinUI
{
    public partial class frmDOIVConfirm : Form
    {
        #region Local Variable

        //DataTable dtDo;
        DataTable dtDeliveryOrder;
        DataTable dtDOIV;
        string ContextMenuSetting = string.Empty;
        DateTime DeliveryDate;
        #endregion

        //Initialize
        public frmDOIVConfirm()
        {
            InitializeComponent();
        }
        public frmDOIVConfirm(DataTable pdtDeliveryOrder, DateTime deliveryOrderDate)
        {
            try
            {
                InitializeComponent(); 
                //Assign the passed parameter
                dtDeliveryOrder = pdtDeliveryOrder;
                DeliveryDate = deliveryOrderDate;
                
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

        //From Event
        private void frmDOIVConfirm_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);

                var DOIV = from row in dtDeliveryOrder.AsEnumerable()
                           group row by new
                           {
                               DocDate = row.Field<DateTime>("DocDate"),
                               DocID = row.Field<string>("DocID"),
                               DocConNm = row.Field<string>("DocConNm"),
                           } into grp
                           select new
                           {
                               DocDate = grp.Key.DocDate,
                               DocID = grp.Key.DocID,
                               DocConNm = grp.Key.DocConNm

                           };

                dtDOIV = DOIV.AsDataTable();
                tagrdDeliveryOrder.DataSource = dtDOIV;
                IVDate.DateValue = DeliveryDate;

                //Size Grid
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn col in tagrdDeliveryOrder.DisplayLayout.Bands[0].Columns)
                {
                    col.Hidden = true;
                }
                tagrdDeliveryOrder.DisplayLayout.Bands[0].Columns["DocID"].Hidden = false;
                tagrdDeliveryOrder.DisplayLayout.Bands[0].Columns["DocDate"].Hidden = false;
                tagrdDeliveryOrder.DisplayLayout.Bands[0].Columns["DocConNm"].Hidden = false;
                tagrdDeliveryOrder.DisplayLayout.Bands[0].Columns["DocID"].Width = 100;
                tagrdDeliveryOrder.DisplayLayout.Bands[0].Columns["DocDate"].Width = 100;
                tagrdDeliveryOrder.DisplayLayout.Bands[0].Columns["DocConNm"].Width = 240;
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
        private void frmDOIVConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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

        //Button Event
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                #region Declaration
                DataTable dtDoBelowMinMarkup = null;
                DataTable dtDoWithoutCostPrice = null;
                List<SqlParameter> vParameters = new List<SqlParameter>();
                string XmlData = string.Empty;
                DataSet dataSet = new DataSet();
                #endregion

                
                vParameters.Add(new SqlParameter("@Option", 3));
                dtDeliveryOrder.TableName = "dtDOTransferList";
                XmlData = GFunc.ConvertDataTableToXML(dtDeliveryOrder);
                vParameters.Add(new SqlParameter("@XmlData", XmlData));                  
                vParameters.Add(new SqlParameter("@RetValue", 0));
                dataSet = GFunc.ExecuteProcDataSet("DOIV_PrepareDOTransfer", vParameters);
                dtDoBelowMinMarkup = dataSet.Tables[0];
                dtDoWithoutCostPrice = dataSet.Tables[1];               
                

                if (dtDoBelowMinMarkup.Rows.Count == 0 && dtDoWithoutCostPrice.Rows.Count == 0)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    frmDOIVCheckFail f = new frmDOIVCheckFail(dtDoBelowMinMarkup, dtDoWithoutCostPrice); 
                    f.ShowDialog();
                    this.DialogResult = f.DialogResult;
                }
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
