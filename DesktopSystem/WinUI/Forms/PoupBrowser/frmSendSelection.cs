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
using TAUtil;

namespace WinUI
{
    public partial class frmSendSelection : Form
    {
        //Declaration
        private string ContextMenuSetting = string.Empty;
        private int jobKey = 0;
        private int RepKey = 0;
        DataTable _dtCallerDetEstimate = null;
        private ReportLoader _ReportLoader = null;
        DataTable dtSendSelection = null;
        DataSet vComboDS = null;
        string vSaveFilePath = string.Empty;
       
        //Property
        public DataTable dtDetEstimate
        {
            get { return _dtCallerDetEstimate; }
            set { _dtCallerDetEstimate = value; }
        }//Completed

        //Initialize
        public frmSendSelection()
        {
            InitializeComponent();
        }//Completed
        public frmSendSelection(UltraGrid grdJobDetEst, int pjobKey)
        {
            //Call From Mstjob when click btnSend_Click
            InitializeComponent();
            jobKey = pjobKey;
            dtDetEstimate =(DataTable) grdJobDetEst.DataSource;
        }//Completed

        //Form Event
        private void frmSendSelection_Load(object sender, EventArgs e) 
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //Set ContextMenu & Grid Setting & Grid Formatting
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);
                GlobalUI.BindComboValue(RptFile, "SYSRepRpt%2330"); //Repkey: 2330 is Job BOM RFQ Rpx

                SetGridData();
                SetGridComboData(); //Emai,Fax ect.
                FormLayout(true);
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
        private void frmSendSelection_KeyDown(object sender, KeyEventArgs e)
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
        }//Completed

        //Button Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                RepKey = 2330; //Job BOM RFQ

                if (form_CanValidate())
                {
                    dtDetEstimate.TableName = "dtJobDetEst";
                    string xmlDetEst = GFunc.ConvertDataTableToXML(dtDetEstimate);

                    switch (GFunc.NEInt(SendMode.Value, 0))
                    {
                        case (int)GEnum.TransmitMode.Email:
                            foreach (DataRow dr in dtSendSelection.Rows)
                            {
                                if (GFunc.NEBool(dr["Selected"], false))
                                {
                                    //load report  
                                    _ReportLoader = new ReportLoader();
                                    List<SqlParameter> paraList = new List<SqlParameter>();
                                    paraList.Add(new SqlParameter("@JobKey", jobKey));
                                    paraList.Add(new SqlParameter("@VendorKey", GFunc.NEInt(dr["DocVendorKey"], 0)));
                                    paraList.Add(new SqlParameter("@JobDetEst", xmlDetEst));

                                    _ReportLoader.ReportSqlParameter = paraList;
                                    _ReportLoader.ReportName = RptFile.Text;// +".rpx";
                                    _ReportLoader.LoadReport(RepKey);

                                    //send email
                                    _ReportLoader.SendEmail(GFunc.NEStr(dr["emailAddr"], string.Empty), Subject.Text, message.Text, ReportFileType.AcrobatPDFFile);
                                    dr["TransmitStatus"] = (int)GEnum.TransmitStatus.Sucessful;
                                }
                            }

                            break;
                        case (int)GEnum.TransmitMode.Print:
                            foreach (DataRow dr in dtDetEstimate.Rows)
                            {
                                if (GFunc.NEBool(dr["Selected"], false))
                                {
                                    //load report  
                                    _ReportLoader = new ReportLoader();
                                    List<SqlParameter> paraList = new List<SqlParameter>();
                                    paraList.Add(new SqlParameter("@JobKey", jobKey));
                                    paraList.Add(new SqlParameter("@VendorKey", GFunc.NEInt(dr["DocVendorKey"], 0)));
                                    paraList.Add(new SqlParameter("@JobDetEst", xmlDetEst));

                                    _ReportLoader.ReportSqlParameter = paraList;
                                    _ReportLoader.ReportName = RptFile.Text;// + ".rpx";
                                    _ReportLoader.LoadReport(RepKey);

                                    CreatePdfFile(vSaveFilePath);
                                    dr["TransmitStatus"] = (int)GEnum.TransmitStatus.Sucessful;
                                }
                            }
                            break;
                        case (int)GEnum.TransmitMode.Fax:
                            foreach (DataRow dr in dtDetEstimate.Rows)
                            {
                                if (GFunc.NEBool(dr["Selected"], false))
                                {
                                    //load report
                                    _ReportLoader = new ReportLoader();
                                    List<SqlParameter> paraList = new List<SqlParameter>();
                                    paraList.Add(new SqlParameter("@JobKey", jobKey));
                                    paraList.Add(new SqlParameter("@VendorKey", GFunc.NEInt(dr["DocVendorKey"], 0)));
                                    paraList.Add(new SqlParameter("@JobDetEst", xmlDetEst));

                                    _ReportLoader.ReportSqlParameter = paraList;
                                    _ReportLoader.ReportName = RptFile.Text;// +".rpx";
                                    _ReportLoader.LoadReport(RepKey);

                                    //send fax
                                    _ReportLoader.SendFax(GFunc.NEStr(dr["FaxNumber"], string.Empty), GFunc.NEStr(dr["DocVendorKey"].ToString(), "Job"));
                                    dr["TransmitStatus"] = (int)GEnum.TransmitStatus.Sucessful;
                                }
                            }
                            break;
                    }
                    if (chkPreview.Checked)
                    {
                        _ReportLoader.PrintPreview();
                    }

                    UpdateSendInformation();
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
        private void btnGetPath_Click(object sender, EventArgs e)
        {
            try
            {
                //folderBrowserDialog1.Title = "Path to keep printed document";
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    vSaveFilePath = folderBrowserDialog1.SelectedPath;
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

        //Control Event
        private void SendMode_CustomUpdate(object sender, CancelEventArgs e)
        {
            try
            {
                //default to email if empty
                this.SendMode.SetValueTrigger(GFunc.NEInt(this.SendMode.Value, (int)GEnum.TransmitMode.Email), false);
                FormLayout(false);
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

        //Grid Event
        private void tagrdSendSelection_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (tagrdSendSelection.ActiveRow != null)
                {
                    if (GFunc.IsNEZ(tagrdSendSelection.ActiveRow.Cells["DocVendorKey"].Value) == false && GFunc.IsNEZ(tagrdSendSelection.ActiveRow.Cells["TransmitMode"].Value) == false)
                    {
                        int vVendorKey = GFunc.NEInt(tagrdSendSelection.ActiveRow.Cells["DocVendorKey"].Value, 0);

                        vComboDS.Tables[0].DefaultView.RowFilter = "DocVendorKey = " + vVendorKey;
                        vComboDS.Tables[1].DefaultView.RowFilter = "DocVendorKey = " + vVendorKey;
                    }
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

        //Methods
        private void SetGridData()
        {
            try
            {
                //filtered with selected row             
               // dtDetEstimate.DefaultView.RowFilter = "Selected = true AND DocVendorKey > 0";
                var _dt = from row in dtDetEstimate.AsEnumerable()
                          where row.Field<bool?>("Selected") == true && row.Field<int?>("DocVendorKey")>0
                          group row by new
                          {
                              DocVendorKey = row.Field<int>("DocVendorKey"),
                              DocCurrKey = row.Field<int>("DocCurrKey"),
                              DocVendorID = row.Field<string>("DocVendorID"),
                              DocVendorNm = row.Field<string>("DocVendorNm"),
                              TransmitMode = row.Field<int>("TransmitMode"),
                              Attention = row.Field<string>("Attention"),
                              EmailAddr = row.Field<string>("EmailAddr"),
                              FaxNumber = row.Field<string>("FaxNumber"),
                              Selected = row.Field<bool>("Selected"),
                          } into grp

                          let DocVendorID = grp.First()

                          select new
                          {
                              DocVendorKey = grp.Key.DocVendorKey,
                              DocCurrKey = grp.Key.DocCurrKey,
                              DocVendorID = grp.Key.DocVendorID,
                              DocVendorNm = grp.Key.DocVendorNm,
                              TransmitMode = grp.Key.TransmitMode,
                              Attention = grp.Key.Attention,
                              EmailAddr = grp.Key.EmailAddr,
                              FaxNumber = grp.Key.FaxNumber,
                              Selected = grp.Key.Selected,
                              TransmitStatus = (int)GEnum.TransmitStatus.Pending
                          };
                dtSendSelection = _dt.AsDataTable();

                tagrdSendSelection.DataSource = dtSendSelection;
                tagrdSendSelection.Rows.Refresh(RefreshRow.ReloadData);

            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        private void SetGridComboData()
        {
            try
            {
                dtSendSelection.TableName = "dtSendConList";
                string xmlStr = GFunc.ConvertDataTableToXML(dtSendSelection);
                List<SqlParameter> vparmList = new List<SqlParameter>();
                vparmList.Add(new SqlParameter("@XmlJobSend", xmlStr));

                SqlParameter vReval = new SqlParameter("@RetValue", 0);
                vReval.Direction = ParameterDirection.Output;
                vparmList.Add(vReval);

                vComboDS = GFunc.ExecuteProcDataSet("MstConContactNumber_Get", vparmList);
                ((TAUtil.TAComboBox)tagrdSendSelection.DisplayLayout.Bands[0].Columns["FaxNumber"].EditorComponent).DataSource = vComboDS.Tables[0];
                ((TAUtil.TAComboBox)tagrdSendSelection.DisplayLayout.Bands[0].Columns["EmailAddr"].EditorComponent).DataSource = vComboDS.Tables[1];
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        private void FormLayout(bool formLoad)
        {
            try
            {
                if (formLoad)
                {
                    //Set Default
                    this.SendMode.SetValueTrigger((int)GEnum.TransmitMode.Email, false);    //email
                    tagrdSendSelection.DisplayLayout.Bands[0].Columns["DocVendorID"].CellActivation = Activation.ActivateOnly;
                    tagrdSendSelection.DisplayLayout.Bands[0].Columns["DocVendorNm"].CellActivation = Activation.ActivateOnly;
                    tagrdSendSelection.DisplayLayout.Bands[0].Columns["TransmitStatus"].CellActivation = Activation.ActivateOnly;
                    tagrdSendSelection.DisplayLayout.Bands[0].Columns["DocCurrKey"].CellActivation = Activation.ActivateOnly;
                }

                switch (GFunc.NEInt(SendMode.Value, 0))
                {
                    case (int)GEnum.TransmitMode.Email:
                        lblSubject.Text = "Subject";
                        lblSubject.Visible = true;
                        Subject.Visible = true;
                        lblMessage.Visible = true;
                        message.Visible = true;
                        btnGetPath.Visible = false;
                        break;

                    case (int)GEnum.TransmitMode.Print:
                        lblSubject.Text = "Path";
                        lblSubject.Visible = true;
                        Subject.Visible = false;
                        lblMessage.Visible = false;
                        message.Visible = false;
                        btnGetPath.Visible = true;
                        break;

                    case (int)GEnum.TransmitMode.Fax:
                        lblSubject.Visible = false;
                        Subject.Visible = false;
                        lblMessage.Visible = false;
                        message.Visible = false;
                        btnGetPath.Visible = false;
                        break;
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
        }//Completed
        private string CreatePdfFile(string ExportLocation)
        {
            try
            {
                UltraGridRow row = RptFile.SelectedRow;
                int i = 0;
                bool fail = true;
                string tmpFileName = ExportLocation + _ReportLoader.ReportName.Remove(_ReportLoader.ReportName.LastIndexOf(".")) + ".pdf";

                while (fail && i < 10) //Maximan 10 time to send?
                {
                    try
                    {
                        DataDynamics.ActiveReports.Export.Pdf.PdfExport pdfExport = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
                        pdfExport.Export(_ReportLoader.rpxDoc.Document, tmpFileName);
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileName += i.ToString();
                    }
                }
                return tmpFileName;
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        private bool form_CanValidate()
        {
            try
            {
                //this function check if we can validates all the header.controls
                //all grid is able to commit its changes to its datasource
                TAUtil.ControlGVar.FormValidateFail = false;
                this.Validate();
                tagrdSendSelection.PerformAction(UltraGridAction.ExitEditMode);
                tagrdSendSelection.UpdateData();

                //we need to check if the active row data cannot be commited 
                //if it cannot be commited, the IsGridDirty would return a false
                //thus saving should not be perform and the user needs to be inform of the data error
                if (IsGridsDirty(false) || TAUtil.ControlGVar.FormValidateFail)
                    return false;
                else
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

        }//Completed
        private bool IsGridsDirty(bool undoChangesInGrid)
        {
            //This function check if the grid has uncommited data in its active orw
            //it also has an option to undo those uncommited changes. 
            try
            {
                #region tagrdDetItms
                if (tagrdSendSelection.ActiveRow != null)
                {
                    if (tagrdSendSelection.ActiveRow.DataChanged && !tagrdSendSelection.ActiveRow.IsUnmodifiedTemplateAddRow)
                    {
                        if (undoChangesInGrid)
                        {
                            //Need to perform undo twice, to simulate pressing ESC key twice to undo cell and undo row
                            this.tagrdSendSelection.PerformAction(UltraGridAction.UndoCell);
                            this.tagrdSendSelection.PerformAction(UltraGridAction.UndoRow);
                        }
                        return true;
                    }
                }
                #endregion

                return false;
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
        private bool UpdateSendInformation()
        {
            try
            {
                //Update UI
                foreach (DataRow vRow in dtSendSelection.Rows)
                {
                    //dtDetEstimate.DefaultView.RowFilter = "DocVendorKey = " + GFunc.NEInt(vRow["DocVendorKey"], 0);
                    IEnumerable<DataRow> dtFilter = dtDetEstimate.AsEnumerable().Where(r => r.Field<int?>("DocVendorKey") == GFunc.NEInt(vRow["DocVendorKey"], 0));

                    for (int i = 0; i < dtFilter.Count(); i++)
                    {
                        dtFilter.ElementAt(i)["TransmitMode"] = vRow["TransmitMode"];
                        dtFilter.ElementAt(i)["Attention"] = vRow["Attention"];
                        dtFilter.ElementAt(i)["emailAddr"] = vRow["EmailAddr"];
                        dtFilter.ElementAt(i)["FaxNumber"] = vRow["FaxNumber"];
                        dtFilter.ElementAt(i)["TransmitStatus"] = vRow["TransmitStatus"];
                    }
                }
                dtDetEstimate.AcceptChanges();

                //Update to Server
                dtDetEstimate.TableName = "UpdateSendInfo";
                string UpdateSendInfo = GFunc.ConvertDataTableToXML(dtDetEstimate);

                List<SqlParameter> vparmList = new List<SqlParameter>();
                vparmList.Add(new SqlParameter("@XmlJobSendInfo", UpdateSendInfo));

                SqlParameter vReval = new SqlParameter("@RetValue", 0);
                vReval.Direction = ParameterDirection.Output;
                vparmList.Add(vReval);

                GFunc.ExecuteProc("MstJobEstSend_Update", vparmList);

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
