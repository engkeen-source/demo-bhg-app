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
using BOLib;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;
using TAUtil;

namespace WinUI
{
    public partial class frmInsertPD : Form
    {
        #region Local Variable
        private string ContextMenuSetting = string.Empty;
        private UltraGrid tagrdDetItms = null;
        Document objDoc = null;
        int ConKey = 0;
        int CurrKey = 0;
        #endregion

        //Initialize
        public frmInsertPD()
        {
            InitializeComponent();
        }//Completed
        public frmInsertPD(Document parmObjDoc, UltraGrid tagrdDetItms)
        {
            InitializeComponent();
            this.tagrdDetItms = tagrdDetItms;                
            objDoc = parmObjDoc;
        }//Completed
        
        //From Events
        private void frmInsertPD_Load(object sender, EventArgs e)
        {
            try
            {
                DocDate.DateValue = DateTime.Today;
                CurrKey = (int)GFunc.GetIntPropertyValue("DocCurrKey", this.objDoc);
                ConKey = (int)GFunc.GetIntPropertyValue("DocConKey", objDoc);
            
                GlobalUI.FormGrids_Set(this, (int)objDoc.DocCodeKey, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)objDoc.DocCodeKey);
                GlobalUI.Combos_Fill(this, (int)objDoc.DocCodeKey);

               // ((DataTable)tagrdInsertPD.DataSource).DefaultView.RowFilter = "LineLinkKey = 0 ";

                ComboDocID_Fill();
                Grid_Filter();
                GlobalUI.GridAllColumnsActivateOnlySet(tagrdInsertPD);
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
        private void frmInsertPD_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F9)
                {
                    GlobalUI.Combos_Fill(this, (int)objDoc.DocCodeKey);
                }

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
        private void frmInsertPD_FormClosed(object sender, FormClosedEventArgs e)
        {

            Hashtable docDet = new Hashtable();
            docDet.Add(GEnum.Details.Doc_Itm, tagrdDetItms);
            DocComUtility.CalForm(objDoc, docDet, true, false);

        }
        //Button Events
        private void btnAppend_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = ((DataTable)tagrdInsertPD.DataSource).DefaultView.ToTable();

                IEnumerable<DataRow> dtFilter = ((DataTable)tagrdInsertPD.DataSource).AsEnumerable().Where(r=>r.Field<int>("LineLinkKey")==0
                    && r.Field<int>("DocKey")>0);

                if (GFunc.NEInt(DocKey.Value, 0) > 0)
                {
                    //dt.DefaultView.RowFilter = "DocKey = " + GFunc.NEInt(DocKey.Value, 0) + "";
                    dtFilter = dtFilter.Where(r=>r.Field<int>("DocKey") == GFunc.NEInt(DocKey.Value, 0));
                }

               // if (this.AppendData(dt.DefaultView.ToTable(false, "DocKey", "DocItmKey")))
                if (this.AppendData(dtFilter.Select(r => new { DocKey = r.Field<int>("DocKey"), DocItmKey = r.Field<int>("DocItmKey"),QtySelect = 0,PickSeq = 0}).AsDataTable()))
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
            this.DialogResult = DialogResult.Cancel;
        }//Completed
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Grid_Refresh();
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

        //Control Events
        private void DocDate_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                Grid_Refresh();
                Grid_Filter();                   
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
        private void DocKey_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                Grid_Filter();     
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
        private void Combo_NotInList(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, false, null);
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
        private void Combo_NotInListAdd(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, true, 0);
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

        //Methods
        private bool AppendData(DataTable dtSelected)
        {
            try
            {
                int soureceDocCodeKey = (int)GEnum.SystemCode.Purchase_Delivery;

                int SourceDocKey = GFunc.NEInt(DocKey.Value, 0);
                int SourceDocConKey = 0;

                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@GUID", objDoc.GUID));
                paraList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                paraList.Add(new SqlParameter("@SourceDocCodeKey", soureceDocCodeKey));
                paraList.Add(new SqlParameter("@SourceDocKey", SourceDocKey));
                SqlParameter para = new SqlParameter("@SourceConKey", SourceDocConKey);
                para.Direction = ParameterDirection.Output;
                paraList.Add(para);
                paraList.Add(new SqlParameter("@DetailType", GEnum.Details.Doc_Itm));
                paraList.Add(new SqlParameter("@InsertAction", GEnum.InsertAction.InsertPD));
                string xmlData = GFunc.ConvertDataTableToXML(dtSelected);
                paraList.Add(new SqlParameter("@xmlDocDetail", xmlData));

                DataTable dtInsertData = GFunc.ExecuteProc("Document_DataTransfer", paraList);

                DocHDRUtil.DocTransferData(soureceDocCodeKey, SourceDocKey, SourceDocConKey,
                    dtInsertData, objDoc, tagrdDetItms, (int)GEnum.InsertAction.InsertPD, "", false, false);

                return true;
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
                GlobalUI.bRuningImport = false;
                DocUtility.bRuningImport = false;
            }
        }//Completed
        private void ComboDocID_Fill()
        {
            try
            {  
                if ((!GFunc.IsNE(DocDate.DateValue)))
                {
                    if (tagrdInsertPD.DataSource != null)
                    {
                        /* added by yst on 29 dec 2018 to eliminate used DocID  */
                        DataTable dtSource = (DataTable)tagrdDetItms.DataSource;
                        int RowCount = dtSource.Rows.Count;
                        string[] UsedDocID = new string[RowCount];

                        if (RowCount > 0)
                        {
                            for (int i = 0; i < RowCount; i++)
                            {
                                UsedDocID[i] = dtSource.Rows[i]["APPDID"].ToString();
                            }
                        }

                        DataTable dt = ((DataTable)tagrdInsertPD.DataSource);
                       // dt = ((DataTable)tagrdInsertPD.DataSource).DefaultView.ToTable();

                        DataTable dtPurchaseDeliveryNo = (from row in dt.AsEnumerable().
                                                       Where (row => row.Field<int>("DocKey") > 0 && row.Field<int>("LineLinkKey")==0 && !UsedDocID.Contains(row.Field<string>("DocID")))
                                                       select new
                                                       {
                                                           DocKey = row.Field<int>("DocKey"),
                                                           DocIVNum = row.Field<string>("DocIVNum"), //added by thettm on 27 jan 2018
                                                           DocID = row.Field<string>("DocID"),
                                                           DocDate = row.Field<DateTime>("DocDate")
                                                       }).Distinct().AsDataTable();

                        DocKey.DataSource = dtPurchaseDeliveryNo;
                    }

                    this.DocKey.Enabled = true;
                }
                else
                {
                    this.DocKey.DataSource = null;
                }
                

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
        private void Grid_Refresh()
        {
            try
            {
                string listSetingID = "frmInsertPDGrid";
                GlobalUI.Grid_Format(tagrdInsertPD, listSetingID, true, false);
               // ((DataTable)tagrdInsertPD.DataSource).DefaultView.RowFilter = "LineLinkKey =0 ";
                ComboDocID_Fill();
                Grid_Filter();
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            
        }//Completed
        private void Grid_Filter()
        {
            try
            {
                //Prepare Filter parameters                
                int docKey = GFunc.NEInt(DocKey.Value, 0);               

                //Filter Grid               
                //GridFilterToDefaultView   
                if (docKey > 0)
                    ((DataTable)tagrdInsertPD.DataSource).DefaultView.RowFilter = "LineLinkKey=0 And DocKey=" + docKey;
                else
                    ((DataTable)tagrdInsertPD.DataSource).DefaultView.RowFilter = "LineLinkKey=0 And DocKey=-1";

                ((DataTable)tagrdInsertPD.DataSource).DefaultView.Sort = "ItmSN";
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
