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
using Infragistics.Win;
using TAUtil;

namespace WinUI
{
    public partial class frmDocRelationship : Form
    {
        private int _DocCodeKey = 0;
        private int _DocKey = 0;
        private string _DocID = string.Empty;
        private bool formClose = false;
        private string ContextMenuSetting;
        DataTable dtDocRelationShip;

        //Constructor
        public frmDocRelationship(int DocCodeKey, int DocKey, string DocID)
        {
            _DocCodeKey = DocCodeKey;
            _DocKey = DocKey;
            _DocID = DocID;
            InitializeComponent();
        }
        public frmDocRelationship()
        {
            InitializeComponent();
        }

        //Form Events
        private void frmDocRelationship_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);
                lblDocID.Text = _DocID;

                tagrdDocList.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
                tagrdDocList.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                tagrdDocList.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                LoadData(false);
                if (this.tagrdDocList.Rows.Count == 0)
                {
                    MsgBox.Show("No relationship found for this document.");
                }
                else
                {
                    ((DataTable)tagrdDocList.DataSource).DefaultView.Sort = "DocDate";
                }
            }
            catch (TAException tex)
            {
                formClose = true;
                Error(tex, true);
            }
            catch (Exception ex)
            {
                formClose = true;
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void frmDocRelationship_Shown(object sender, EventArgs e)
        {
            try
            {
                if (formClose)
                {
                    this.Close();
                    this.Dispose();
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
        }
        private void frmDocRelationship_KeyDown(object sender, KeyEventArgs e)
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

        //Grid Events
        private void tagrdDocList_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            try
            {
                switch (GFunc.NEInt(e.Cell.Row.Cells["DoccodeKey"].Value, 0))
                {
                   
                    case (int)GEnum.SystemCode.Quotation:
                        frmARQO vfrmARQO = new frmARQO(GEnum.SystemCode.Quotation);
                        vfrmARQO.MdiParent = frmMain.gfrmMain;
                        vfrmARQO.Show();
                        vfrmARQO.OnDocList_OpenRecord(GFunc.NEInt(e.Cell.Row.Cells["DocKey"].Value, 0));//Open By DocKey.
                        break;
                    case (int)GEnum.SystemCode.Reserve_Order:
                        frmARRO vfrmARRO = new frmARRO(GEnum.SystemCode.Reserve_Order);
                        vfrmARRO.MdiParent = frmMain.gfrmMain;
                        vfrmARRO.Show();
                        vfrmARRO.OnDocList_OpenRecord(GFunc.NEInt(e.Cell.Row.Cells["DocKey"].Value, 0));//Open By DocKey.
                        break;
                    case (int)GEnum.SystemCode.Sales_Order:
                        //added by thettm on 11 jun 2018(start)
                        bool iscashSO = false;
                        if(e.Cell.Row.Cells["DocID"].Value.ToString().StartsWith("CSO"))
                            iscashSO = true;
                        frmARSO vfrmARSO = new frmARSO(GEnum.SystemCode.Sales_Order, iscashSO);
                        //added by thettm on 11 jun 2018(end)

                        //frmARSO vfrmARSO = new frmARSO(GEnum.SystemCode.Sales_Order,iscash); //commented by thettm on 11 jun 2018
                        vfrmARSO.MdiParent = frmMain.gfrmMain;
                        vfrmARSO.Show();
                        vfrmARSO.OnDocList_OpenRecord(GFunc.NEInt(e.Cell.Row.Cells["DocKey"].Value, 0));//Open By DocKey.
                        break;                    

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
        }

        //control Events
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsbExport_Click(object sender, EventArgs e)
        {
            try
            {
                int count = tsbExport.Tag == null ? 1 : (int)tsbExport.Tag;
                GlobalUI.Export(tagrdDocList, ref count);
                tsbExport.Tag = count;
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
        private void RunSummary_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadData(RunSummary.Checked);
                RunSummary.Text = RunSummary.Checked == false ? "Show Detail" : "Show Summary";
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

        //Functions
        public void Reload(GEnum.SystemCode DocCodeKey, int DocKey)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                _DocKey = DocKey;
                _DocCodeKey = (int)DocCodeKey;
                LoadData(RunSummary.Checked);
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
                Cursor = Cursors.Default;
            }
        }//Completed

        private void LoadData(Boolean CheckedValue)
        {
            DataTable dtResult;
            try
            {                
                RunSummary.Visible = !(_DocCodeKey == (int)GEnum.SystemCode.Purchase_Adjustment);
                
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@DocCodeKey", _DocCodeKey));
                paraList.Add(new SqlParameter("@DocKey", _DocKey));
                dtDocRelationShip = GFunc.ExecuteProc("DocumentLink_Get", paraList);




                if (CheckedValue == false)
                {
                    //Summary
                    dtResult = (from row in dtDocRelationShip.AsEnumerable()
                                orderby row.Field<string>("DocTypeNm"), row.Field<string>("DocID")
                                group row by new
                                {
                                    DocCodeKey = row.Field<int>("DocCodeKey"),
                                    DocType = row.Field<int>("DocType"),
                                    DocTypeNm = row.Field<string>("DocTypeNm"),
                                    DocKey = row.Field<int>("DocKey"),
                                    DocID = row.Field<string>("DocID"),
                                    DocDate = row.Field<DateTime>("DocDate"),
                                    DocChqNum = row.Field<string>("DocChqNum"),

                                } into grp
                                select new
                                {
                                    DocCodeKey = grp.Key.DocCodeKey,
                                    DocType = grp.Key.DocType,
                                    DocTypeNm = grp.Key.DocTypeNm,
                                    DocKey = grp.Key.DocKey,
                                    DocID = grp.Key.DocID,
                                    DocDate = grp.Key.DocDate,
                                    DocChqNum = grp.Key.DocChqNum
                                }
                                          ).AsDataTable();
                }
                else
                {
                    //Detail
                    dtResult = (from row in dtDocRelationShip.AsEnumerable()
                                orderby row.Field<decimal>("ItmSN"), row.Field<string>("DocTypeNm")
                                group row by new
                                {
                                    ItmSN = row.Field<decimal>("ItmSN"),
                                    DocCodeKey = row.Field<int>("DocCodeKey"),
                                    DocType = row.Field<int>("DocType"),
                                    DocTypeNm = row.Field<string>("DocTypeNm"),
                                    DocKey = row.Field<int>("DocKey"),
                                    DocID = row.Field<string>("DocID"),
                                    DocDate = row.Field<DateTime>("DocDate"),
                                    DocChqNum = row.Field<string>("DocChqNum"),

                                } into grp
                                select new
                                {
                                    ItmSN = grp.Key.ItmSN,
                                    DocCodeKey = grp.Key.DocCodeKey,
                                    DocType = grp.Key.DocType,
                                    DocTypeNm = grp.Key.DocTypeNm,
                                    DocKey = grp.Key.DocKey,
                                    DocID = grp.Key.DocID,
                                    DocDate = grp.Key.DocDate,
                                    DocChqNum = grp.Key.DocChqNum
                                }
                                          ).AsDataTable();
                }

                tagrdDocList.DataSource = dtResult;

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Error Methods
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
