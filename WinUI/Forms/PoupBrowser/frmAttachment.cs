using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using System.IO;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using TAUtil;
using System.Data.SqlClient;

namespace WinUI
{
    public partial class frmAttachment : Form
    {
        #region Local Variables
        
        string ContextMenuSetting = string.Empty;
        int? DocCodeKey = 0;
        int? DocKey = 0;
        int? DocDItmKey=0;
        int? DocDetailItmType = 0;
        SYSAttachments objSysAttachments = null;
        Document _Doc = null;

        string emailProfile = "";
        string emailSender = "";
        string senderPassword = "";
        string emailReceiver = "";
        string emailCC = "";
        string emailBCC = "";
        string emailSubject = "";
        string emailBody = "";
        string senderDisplayName = "";
        string sentStatus = "";

        #endregion

        //Constructor
        public frmAttachment()
        {
            InitializeComponent();            
        }//Completed
        public frmAttachment(SYSAttachments objs, int? docDC, int? docDK, int? docDItmKey, int docDetailItmType)
        {
            InitializeComponent();
            objSysAttachments = objs;
            DocCodeKey = docDC;
            DocKey = docDK;
            DocDItmKey = docDItmKey;
            DocDetailItmType = docDetailItmType;
            this.Text = "Attachments";            
            this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - (gbEmail.Height + 6));
            gbEmail.Visible = false;
            tsbCopy.Visible = false;            

        }//Completed

        public frmAttachment(SYSAttachments objs, int? docDC, int? docDK, string DocStatus)
        {
            InitializeComponent();
            objSysAttachments = objs;
            DocCodeKey = docDC;
            DocKey = docDK;
           
            this.Text = "Attachments";
            this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - (gbEmail.Height + 6));
            gbEmail.Visible = false;
            tsbCopy.Visible = false;

            if (DocStatus.Equals("Approved"))
                tsbDelete.Enabled = false;
            else
                tsbDelete.Enabled = SECPermUtility.Perform("SysAttachDelete", false);

        }//Completed

        public frmAttachment(SYSAttachments objs, Document objDoc, int docDetailType)
        {
            InitializeComponent();
            objSysAttachments = objs;
            _Doc = objDoc;            
            DocCodeKey = _Doc.DocCodeKey;
            DocKey = _Doc.DocKey;
            DocDetailItmType = docDetailType;
            DocDItmKey = -1;
            if (docDetailType == 2) // Signed DO
            {
                tsbAttach.Enabled = SECPermUtility.Add("AttachSignedDO", false);
                tsbDelete.Enabled = (SECPermUtility.Perform("SysAttachDelete", false));
                this.Text = (DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice) ? "Signed DO/IV Attachments" : "Signed DO Attachments";
            }
            else // CustomerPO
            {
                /* Customer PO Attachment modified by YST */
                tsbAttach.Enabled = !objDoc.IsReadOnly;
                tsbDelete.Enabled = !objDoc.IsReadOnly;
                chkCustomerPO.Enabled = false;
                chkCustomerPO.Checked = true;
                this.Text = "Customer PO Attachments";
                if (DocCodeKey == (int)GEnum.SystemCode.Sales_Order)
                {
                    tsbCopy.Visible = false;
                    //tsbAttach.Enabled = !objDoc.IsReadOnly;
                    //tsbDelete.Enabled = !objDoc.IsReadOnly;;
                    //if (_Doc.DocState == 100)
                    //{
                    //    tsbAttach.Enabled = false;
                    //    tsbDelete.Enabled = false;
                    //}
                }
                if (DocCodeKey == (int)GEnum.SystemCode.Delivery_Order)
                {
                    if (_Doc.DocState == (int)GEnum.DocState.Invoiced && SECPermUtility.Add("AttachCustPOAfterInv", false)) /* suggested by Josie,Su San from Finance department */
                        tsbAttach.Enabled = true;
                }
                else if (DocCodeKey == (int)GEnum.SystemCode.Journal)
                {
                    this.Text = "Attachments for Journal";
                    if (_Doc.DocStatus != null && (_Doc.DocStatus.ToLower().Equals("approved") || _Doc.DocStatus.ToLower().Equals("requested")))
                    {
                        tsbAttach.Enabled = false;
                        tsbDelete.Enabled = false;
                    }
                }
                else if (DocCodeKey == (int)GEnum.SystemCode.Quotation)
                {
                    this.Text = "Quotation Attachments";
                }
                else if (DocCodeKey == (int)GEnum.SystemCode.Purchase_Order)
                {
                    this.Text = "Purchase Order Attachments";
                }
                else if (DocCodeKey == (int)GEnum.SystemCode.Purchase_Invoice)
                {
                    this.Text = "Invoice Attachments";
                }
            }

            /* to copy attached files from DO to IV, added by YST on 2022/05/18 */
            if ((DocCodeKey == (int)GEnum.SystemCode.Delivery_Order) && (SECPermUtility.Perform("ARDOTransfer", false) == true))
                tsbCopy.Visible = true;
            else
                tsbCopy.Visible = false;

            /* to send email E-invoices, added by YST on 2022/03/18 */
            if (SECPermUtility.Perform("SentEInvoice", false) == false /* check SentEInvoice setting of the subsidiary */
                || DocCodeKey != (int)GEnum.SystemCode.Sales_Invoice /* So far only Sales Invoice requires to send E-Invoice */
                || DocDetailItmType != 2) /* check SignedDO/IV */
            {
                gbEmail.Visible = false;
                this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - (gbEmail.Height + 6));
            }
            else
            {                
                DataTable dtEmailInfo = GetEInvoiceEmailInfo(DocKey);
                if (dtEmailInfo != null && dtEmailInfo.Rows.Count > 0)
                {
                    emailProfile = GFunc.NEStr(dtEmailInfo.Rows[0]["emailProfile"], "");
                    emailSender = GFunc.NEStr(dtEmailInfo.Rows[0]["emailSender"], "");
                    senderPassword = GFunc.NEStr(dtEmailInfo.Rows[0]["senderPassword"], "");
                    emailReceiver = GFunc.NEStr(dtEmailInfo.Rows[0]["emailReceiver"], "");
                    emailCC = GFunc.NEStr(dtEmailInfo.Rows[0]["emailCC"], "");
                    emailBCC = GFunc.NEStr(dtEmailInfo.Rows[0]["emailBCC"], "");
                    emailSubject = GFunc.NEStr(dtEmailInfo.Rows[0]["emailSubject"], "");
                    emailBody = GFunc.NEStr(dtEmailInfo.Rows[0]["emailBody"], "");
                    senderDisplayName = GFunc.NEStr(dtEmailInfo.Rows[0]["senderDisplayName"], "");
                    sentStatus = GFunc.NEStr(dtEmailInfo.Rows[0]["SentStatus"], "");

                    txtFromEmail.Text = emailSender;
                    txtToEmail.Text = emailReceiver;
                    txtEmailSubj.Text = emailSubject;
                    chkSentStatus.Checked = sentStatus.Contains("Sent");
                    chkSentStatus.Appearance.ForeColorDisabled = chkSentStatus.Checked ? Color.Green : Color.Gray;
                }
            }
        }//Completed
        //Form Events
        private void frmPopupAttachment_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (objSysAttachments != null)
                {
                    this.tagrdAttachment.DataSource = objSysAttachments;//test
                }
                //GlobalUI.cmnuGlobal_Set(this);
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0, this.Name);

                this.tagrdAttachment.DragDrop += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragDrop);
                this.tagrdAttachment.DragOver += new System.Windows.Forms.DragEventHandler(GlobalUI.Grid_DragOver);
                this.tagrdAttachment.SelectionDrag += new System.ComponentModel.CancelEventHandler(GlobalUI.Grid_SelectionDrag);
               
                ColumnFiltersCollection columnFilterHDR = this.tagrdAttachment.DisplayLayout.Bands[0].ColumnFilters;
                columnFilterHDR.ClearAllFilters();                
                switch((GEnum.SystemCode)DocCodeKey)
                {
                        case GEnum.SystemCode.Sales_Order:
                        case GEnum.SystemCode.Delivery_Order:
                        case GEnum.SystemCode.Sales_Invoice:
                        case GEnum.SystemCode.Cash_Sale:
                        case GEnum.SystemCode.Sales_Credit_Note:
                        case GEnum.SystemCode.Cash_Credit_Note:
                        case GEnum.SystemCode.Sales_Debit_Note:
                        case GEnum.SystemCode.Cash_Debit_Note:
                            columnFilterHDR["DocDItm"].FilterConditions.Add(FilterComparisionOperator.Equals, DocDItmKey);
                            columnFilterHDR["DocDetailType"].FilterConditions.Add(FilterComparisionOperator.Equals, DocDetailItmType);
                            columnFilterHDR.LogicalOperator = FilterLogicalOperator.And;
                            break;
                }
                tagrdAttachment.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                tagrdAttachment.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;

                tagrdAttachment.DisplayLayout.Bands[0].Columns.Add("Open", "");
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].CellButtonAppearance.Image = global::WinUI.Properties.Resources.openfolderHS;
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].Header.VisiblePosition = 1;
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].Hidden = false;
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].CellButtonAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].CellButtonAppearance.ImageHAlign = HAlign.Center;
                tagrdAttachment.DisplayLayout.Bands[0].Columns["Open"].Width = 40;

                if (DocCodeKey != (int)GEnum.SystemCode.Quotation) //  disable download button for Qutotation -- added by KKAung on 16 Feb 2023
                {
                    tagrdAttachment.DisplayLayout.Bands[0].Columns.Add("Download", "");
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].CellButtonAppearance.Image = global::WinUI.Properties.Resources.download_01;
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].Header.VisiblePosition = 2;
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].Hidden = false;
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].CellButtonAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].CellButtonAppearance.ImageHAlign = HAlign.Center;
                    tagrdAttachment.DisplayLayout.Bands[0].Columns["Download"].Width = 40;
                }
                
                bool checkPerm = true;

                if (_Doc != null)                
                    if (GFunc.NEStr(_Doc.DocStatus,"").ToLower() == "draft"|| _Doc.DocState ==(int)GEnum.DocState.Draft)
                        checkPerm = false;
                
                if(checkPerm) 
                    if (!SECPermUtility.Perform("SysAttachDelete", false))
                        tsbDelete.Enabled = false;

                tagrdAttachment.Refresh();
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
        private void frmPopupAttachment_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (_Doc != null)
                {
                    if (_Doc.IsDirty)
                    {
                        _Doc.Attachment = tagrdAttachment.Rows.Count > 0;
                        objSysAttachments.IsDirty = true;
                    }
                }

                if (tagrdAttachment.Rows.FilteredInRowCount > 0)
                    this.DialogResult = DialogResult.Yes;
                else
                    this.DialogResult = DialogResult.No;
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
        private void frmAttachment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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

        //Menu Strip Event
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool checkPerm = true;

                if (_Doc != null)
                    if (GFunc.NEStr(_Doc.DocStatus, "").ToLower() == "draft" || _Doc.DocState == (int)GEnum.DocState.Draft)
                        checkPerm = false;

                if (checkPerm)
                    if (!SECPermUtility.Perform("SysAttachDelete", true))
                        return;

                int deletedIndex = -1;

                //Need to ensure that active row is also a selected row
                if (tagrdAttachment.ActiveRow != null)
                    tagrdAttachment.ActiveRow.Selected = true;

                DataTable dtAttachment = new DataTable("Table1");

                while (tagrdAttachment.Selected.Rows.Count == 1 && dtAttachment.Rows.Count == 0)
                {
                    //if ((DocCodeKey == (int)GEnum.SystemCode.Delivery_Order
                    //|| DocCodeKey == (int)GEnum.SystemCode.Sales_Order
                    //|| DocCodeKey == (int)GEnum.SystemCode.Cash_Sale
                    //|| DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice) && SysOptionUtility.HasDMASLink)
                    if(SysOptionUtility.HasDMASLink)
                    {
                        dtAttachment.Columns.Add("DocDC", typeof(int));
                        dtAttachment.Columns.Add("DocDetailType", typeof(int));
                        dtAttachment.Columns.Add("DocDItm", typeof(int));
                        dtAttachment.Columns.Add("DocDK", typeof(int));
                        dtAttachment.Columns.Add("AttachDes", typeof(string));
                        dtAttachment.Columns.Add("Seq", typeof(int));

                        dtAttachment.Rows.Add(new object[] { tagrdAttachment.Selected.Rows[0].Cells["DocDC"].Value
                                            , tagrdAttachment.Selected.Rows[0].Cells["DocDetailType"].Value
                                            , tagrdAttachment.Selected.Rows[0].Cells["DocDItm"].Value
                                            , tagrdAttachment.Selected.Rows[0].Cells["DocDK"].Value
                                            , tagrdAttachment.Selected.Rows[0].Cells["AttachDes"].Value
                                            , tagrdAttachment.Selected.Rows[0].Cells["Seq"].Value });
                    }
                    deletedIndex = tagrdAttachment.Selected.Rows[0].Index;
                    tagrdAttachment.DeleteSelectedRows(false);
                    //objSysAttachments.RemoveAt(deletedIndex); 
                    /* 
                     * commented by YST on 2022/10/04 because if objSysAttachments also includes PO Atttachments, deletedIndex will be wrong 
                     * and added while condition dtAttachment.Rows.Count == 0 
                     */
                }
                if (tagrdAttachment.Rows.GetFilteredInNonGroupByRows().Count() > 0)
                    tagrdAttachment.ActiveRow = tagrdAttachment.Rows.GetFilteredInNonGroupByRows()[0];
                if (SysOptionUtility.HasDMASLink)
                {
                    DeleteAttachmentWithDMAS(dtAttachment);
                }
                else if(_Doc!=null)
                    _Doc.IsDirty = true;
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
        private void tsbAttach_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                openFileDialog.Title = "Attachment";
                
                openFileDialog.Multiselect = true;
                openFileDialog.FileName = "";
                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.Cancel) 
                    return;

                openFileDialog.OpenFile();  
                string[] files = openFileDialog.FileNames;

                SYSAttachments objtempAttachments = new SYSAttachments();
                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);

                    SYSAttachment objAttachment = SYSAttachment.NewChild();
                    objAttachment.DocDC = DocCodeKey;
                    objAttachment.DocDK = DocKey;
                    objAttachment.DocDItm = DocDItmKey;
                    objAttachment.DocDetailType = DocDetailItmType;
                    if (objSysAttachments.Count > 0)
                        objAttachment.Seq = objSysAttachments.Max(o => o.Seq) + 1;
                    else
                        objAttachment.Seq = 1;
                    objAttachment.AttachPath = info.DirectoryName; //File Path 
                    objAttachment.AttachSize = GFunc.NEInt(info.Length,0); //File Size
                    objAttachment.AttachFileType = info.Extension; //File Extension
                    objAttachment.AttachDes = info.Name;      //File Name
                    objAttachment.Custom2 = AppInfor.CurrentUserID;
                    objAttachment.Custom3 = DateTime.Today.ToString("MMM dd yyyy ")+DateTime.Now.ToString("h:mm tt").ToUpper();

                    objSysAttachments.Add(objAttachment);
                    objtempAttachments.Add(objAttachment);
                }

                //if ((DocCodeKey == (int)GEnum.SystemCode.Delivery_Order 
                //    || DocCodeKey == (int)GEnum.SystemCode.Sales_Order 
                //    || DocCodeKey == (int)GEnum.SystemCode.Cash_Sale 
                //    || DocCodeKey == (int)GEnum.SystemCode.Sales_Invoice) 
                //    && SysOptionUtility.HasDMASLink)
                if(SysOptionUtility.HasDMASLink)
                    SaveAttachmentWithDMAS(objtempAttachments);
                else if(_Doc!=null)
                    _Doc.IsDirty = true;
                tagrdAttachment.DataSource = objSysAttachments;
               
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
        private void tsbClose_Click(object sender, EventArgs e)
        {            
            this.Close();
        }//Completed

        //Grid Common Events
        private void tagrdAttachment_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {               
                if (tagrdAttachment.ActiveRow != null)
                {
                    OpenFile(tagrdAttachment.ActiveRow);
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
        }//Completed
        private void tagrdAttachment_ClickCellButton(object sender, CellEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (GFunc.CompareString(e.Cell.Column.Key, "Open"))
                {
                    tagrdAttachment.ActiveRow = e.Cell.Row;
                    OpenFile(tagrdAttachment.ActiveRow);
                }
                if (GFunc.CompareString(e.Cell.Column.Key, "Download"))
                {
                    tagrdAttachment.ActiveRow = e.Cell.Row;
                    DownloadFile(tagrdAttachment.ActiveRow, false,"");
                }
            }
            catch (TAException tex)
            {
                Error(tex, true); // Custom Msg
            }
            catch (Exception ex)
            {
                Error(ex, true); // System Msg
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void OpenFile(UltraGridRow row)
        {
            string fName = "";
            string filePath = row.Cells["AttachPath"].Value.ToString() + "\\" + row.Cells["AttachDes"].Value;

          
                string SecGrp = SysOptionUtility.DatabaseBranchCode + "Sales";
                string DocID = "";// GFunc.DocID_Get(DocCodeKey, 0, DocKey);
                int? AttDocDK = DocKey;
                int? AttDocDC = DocCodeKey;

                if (_Doc != null)
                    if (DocCodeKey == (int)GEnum.SystemCode.Cash_Sale)
                    {
                        AttDocDK = GFunc.DocKey_Get((int)GEnum.SystemCode.Delivery_Order, _Doc.DocID.Replace("CI", "CDO"));
                        AttDocDC = (int)GEnum.SystemCode.Delivery_Order;
                    }
            
                //fName = DocHDRUtil.GetFileFromDMAS(SecGrp, DocName, DocID, GFunc.NEInt(row.Cells["Seq"].Value, 0));
                fName = DocHDRUtil.GetFileFromDMASByDocKey(AttDocDC, AttDocDK,false, Path.GetTempPath(), GFunc.NEInt(row.Cells["Seq"].Value, 0));
                fName = fName.Replace("#", "");
                if (System.IO.File.Exists(fName))
                    {
                        var process = System.Diagnostics.Process.Start(fName);
                        if (process != null)
                        {
                            process.Exited += delegate (object s, EventArgs ev)
                            {
                                if (System.IO.File.Exists(fName))
                                {
                                    System.IO.File.Delete(fName);
                                }
                            };
                        }
                    }
                //}
        }        
        private bool SaveAttachmentWithDMAS(SYSAttachments attachments)
        {
            //Saving or Deleting Attachments in Documents
            try
            {
                #region Declaration
                List<SqlParameter> list = new List<SqlParameter>();
                List<SqlParameter> list2 = new List<SqlParameter>();

                DataTable dtAttachment = new DataTable("Table1");
                dtAttachment.Columns.Add("AttachDes", typeof(string));
                dtAttachment.Columns.Add("AttachFileType", typeof(string));
                dtAttachment.Columns.Add("AttachPath", typeof(string));
                dtAttachment.Columns.Add("AttachSize", typeof(int));
                dtAttachment.Columns.Add("Custom1", typeof(string));
                dtAttachment.Columns.Add("Custom2", typeof(string));
                dtAttachment.Columns.Add("Custom3", typeof(string));
                dtAttachment.Columns.Add("DocDC", typeof(int));
                dtAttachment.Columns.Add("DocDetailType", typeof(int));
                dtAttachment.Columns.Add("DocDItm", typeof(int));
                dtAttachment.Columns.Add("DocDK", typeof(int));
                dtAttachment.Columns.Add("Seq", typeof(int));
                //2 columns extra for DMAS
                dtAttachment.Columns.Add("DocFile", typeof(byte[]));
                dtAttachment.Columns.Add("DocFileName", typeof(string));
                dtAttachment.Columns.Add("DocID", typeof(string));

                #endregion

                foreach (SYSAttachment attach in attachments)
                {
                    FileStream fs = null;
                    BinaryReader br = null;

                    byte[] buffer = null;
                    fs = new FileStream(attach.AttachPath + @"\" + attach.AttachDes, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);
                    long numBytes = new FileInfo(attach.AttachPath + @"\" + attach.AttachDes).Length;
                    buffer = br.ReadBytes((int)numBytes);
                    br.Close();
                    fs.Close();
                    string DocFileName = ((GEnum.SystemCode)DocCodeKey).ToString() + " DocNum=" +(_Doc!=null? _Doc.DocID:"") + " DocDes=" + attach.AttachDes;
                    dtAttachment.Rows.Add(new object[] { attach.AttachDes, attach.AttachFileType, attach.AttachPath, attach.AttachSize, attach.Custom1, attach.Custom2, attach.Custom3, attach.DocDC, attach.DocDetailType, attach.DocDItm, attach.DocDK, attach.Seq, buffer, DocFileName,(_Doc!=null?_Doc.DocID:"") });
                }

                string xmlAttachment = GFunc.ConvertDataTableToXML(dtAttachment);
                list2.Add(new SqlParameter("@DocDC", DocCodeKey));
                list2.Add(new SqlParameter("@DocDK", DocKey));
                list2.Add(new SqlParameter("@Attachment", xmlAttachment));
                list2.Add(new SqlParameter("@RetValue", 0));
                list2[3].Direction = ParameterDirection.Output;                
                list2.Add(new SqlParameter("@SecGrp", SysOptionUtility.DatabaseBranchCode + "Sales"));
                GFunc.ExecuteNonQueryProc("SYSAttachment_SaveWithDMAS", list2);

                if (GFunc.NEInt(list2[3].Value, 0) == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            catch (TAException ex)
            {
                throw Error(ex,true);
            }
            catch (Exception ex)
            {
                throw Error(ex, true);
            }
        }//Completed
        private bool DeleteAttachmentWithDMAS( DataTable dtAttachment)
        {
            //Saving or Deleting Attachments in Documents
            try
            {         
                List<SqlParameter> list2 = new List<SqlParameter>();                     
                string xmlAttachment = GFunc.ConvertDataTableToXML(dtAttachment);            
                list2.Add(new SqlParameter("@Attachment", xmlAttachment));
                list2.Add(new SqlParameter("@RetValue", 0));
                list2[1].Direction = ParameterDirection.Output;
                list2.Add(new SqlParameter("@SecGrp", SysOptionUtility.DatabaseBranchCode + "Sales"));
                list2.Add(new SqlParameter("@UserID", AppInfor.CurrentUserID));                 

                GFunc.ExecuteNonQueryProc("SYSAttachment_DeleteWithDMAS", list2);

                if (GFunc.NEInt(list2[1].Value, 0) == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
            catch (TAException ex)
            {
                throw Error(ex, true);
            }
            catch (Exception ex)
            {
                throw Error(ex, true);
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

        /* added by YST on 2022/03/18 */
        /* Modified by MTS on 2022/08/30 to download merged files */
        private void DownloadFile(UltraGridRow row, bool sendEmail, string AllFilesDownloadPath)
        {
            string DownloadPath = Path.GetTempPath();
            string tempDownloadPath = DownloadPath + SysOptionUtility.DatabaseBranchCode + "\\TempDownloads\\";
            string realDownloadPath = "";

            realDownloadPath = AllFilesDownloadPath == "" ? DownloadPath.Substring(0, DownloadPath.IndexOf("AppData")) + "Downloads\\" : AllFilesDownloadPath + "\\";

            if (!Directory.Exists(tempDownloadPath))
            {
                Directory.CreateDirectory(tempDownloadPath);
            }

            if (!Directory.Exists(realDownloadPath))
            {
                Directory.CreateDirectory(realDownloadPath);
            }

            string fName = "", DocName = "", DocID = "", downloadtime = "";
            string SecGrp = SysOptionUtility.DatabaseBranchCode + "Sales";
            string Msg = "";

            if (_Doc != null)
                if (_Doc.DocCodeKey == (int)GEnum.SystemCode.Cash_Sale)
                    DocID = _Doc.DocID.Replace("CI", "CDO");
                else
                    DocID = _Doc.DocID;
            else
                DocID = GFunc.DocID_Get(DocCodeKey, 0, DocKey);

            DocName = GFunc.NEStr(row.Cells["AttachDes"].Value, "");

            try
            {
                if (sendEmail)
                {
                    frmMain.gfrmMain.SetNotifyStatus("Sending Email.... Please wait.");

                    string fileAttachment = "";

                    if (chkMergePDF.Checked)
                        fileAttachment = DocHDRUtil.GetPDFMergeFileFromDMASByDocKey(DocCodeKey, DocKey, DocID, chkCustomerPO.Checked, tempDownloadPath);
                    else
                        fileAttachment = DocHDRUtil.GetFileFromDMASByDocKey(DocCodeKey, DocKey, chkCustomerPO.Checked, tempDownloadPath);

                    if (txtToEmail.Text.Trim() == "") Msg = "Please key email address that you want to send!";
                    else if (txtEmailSubj.Text.Trim() == "") Msg = "Please key email subject!";
                    else if (fileAttachment == "") Msg = "There is no attached files to send email. <br/> Please check again.";
                    else
                    {
                        try
                        {
                            if (!fileAttachment.ToUpper().Contains("DO")) Msg = "Signed DO";
                            else if (!fileAttachment.ToUpper().Contains("IV")) Msg = "Signed IV";
                            else if (chkCustomerPO.Checked && !fileAttachment.ToUpper().Contains("PO")) Msg = "Customer PO";
                            if (Msg != "")
                            {
                                Msg = "<font color = 'red'>" + Msg + " attachment is missing .</font ><br/><br/>Are you sure to send email to the customer ? ";
                                if (MsgBox.Show(Msg, GEnum.MsgBoxIcon.Serious, GEnum.MsgBoxButton.Cancel, GEnum.MsgBoxButton.Yes) == GEnum.MsgBoxButton.Yes)
                                {
                                    Msg = "";
                                }
                                else
                                {
                                    frmMain.gfrmMain.SetNormalStaus("");
                                    return;
                                }
                            }
                            if (Msg == "")
                            {
                                //bool HasError = GEmail.SendEmail(emailSender, senderPassword, txtToEmail.Text.Trim(), emailCC, txtEmailSubj.Text.Trim(), emailBody, senderDisplayName, fileAttachment);
                                bool HasError = GEmail.SendDBMail(emailProfile, emailSender, senderPassword, txtToEmail.Text.Trim(), emailCC, emailBCC, txtEmailSubj.Text.Trim(), emailBody, senderDisplayName, fileAttachment);
                                if (!HasError)
                                {
                                    bool sourceIsDirty = _Doc.IsDirty;
                                    _Doc.Custom3 = UpdateEInvoiceEmailStatus(DocKey);
                                    _Doc.IsDirty = sourceIsDirty;

                                    chkSentStatus.Checked = true;
                                    chkSentStatus.Appearance.ForeColorDisabled = Color.Green;
                                    frmMain.gfrmMain.SetNormalStaus("");
                                    Msg = "System sent email successfully!";
                                }

                                #region  Delete attach files
                                string[] files = Directory.GetFiles(tempDownloadPath);
                                foreach (string file in files)
                                {
                                    File.Delete(file);
                                }

                                /* commented by YST  
                                string DBMailAttachmentFilePath = "\\\\172.16.0.55\\Temp\\eInvoice";
                                System.IO.File.Delete(DBMailAttachmentFilePath + "\\bhglobal.png");
                                System.IO.File.Delete(DBMailAttachmentFilePath + "\\EmailSignature2020(new).jpg");

                                string[] fileAttachmentDB = fileAttachment.Trim().Split('#');
                                for (int i = 0; i < fileAttachmentDB.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(fileAttachmentDB[i]))
                                    {
                                        FileInfo fi = new FileInfo(fileAttachmentDB[i].ToString());
                                        //System.IO.File.Delete(DBMailAttachmentFilePath + "\\" + fi.Name); 
                                    }
                                }
                                */
                                #endregion
                            }
                        }
                        catch (IOException ex)
                        {
                            Msg = ex.ToString();
                        }
                        catch (Exception ex)
                        {
                            Msg = "System cannot send email. <br/> Please check again. <br/>" + ex.ToString();
                        }
                    }
                    if (Msg != "")
                    {
                        MsgBox.Show(Msg, (Msg.Contains("successfully") ? GEnum.MsgBoxIcon.Information : GEnum.MsgBoxIcon.Warning), GEnum.MsgBoxButton.OK);
                    }
                }
                else
                {
                    frmMain.gfrmMain.SetNotifyStatus("Downloading.... Please wait.");

                    if (AllFilesDownloadPath == "")
                    {
                        // fName = DocHDRUtil.GetFileFromDMAS(SecGrp, DocName, DocID, GFunc.NEInt(row.Cells["Seq"].Value, 0), realDownloadPath);
                        fName = DocHDRUtil.GetFileFromDMASByDocKey(DocCodeKey, DocKey, false, realDownloadPath, GFunc.NEInt(row.Cells["Seq"].Value, 0));
                        Msg = "Download successfully as " + "<br/>" + fName.Replace("#", "<br/>");
                    }
                    else
                    {
                        if (chkMergePDF.Checked)
                        {
                            fName = DocHDRUtil.GetPDFMergeFileFromDMASByDocKey(DocCodeKey, DocKey, DocID, chkCustomerPO.Checked, realDownloadPath);
                            Msg = "Download successfully as " + "<br/>" + fName.Replace("#", "<br/>");
                        }
                        else
                        {
                            DocHDRUtil.GetFileFromDMASByDocKey(DocCodeKey, DocKey, chkCustomerPO.Checked, realDownloadPath);
                            Msg = "All files downloaded successfully under " + realDownloadPath;
                        }
                    }

                    frmMain.gfrmMain.SetNormalStaus("");
                    if (Msg != "")
                        MsgBox.Show(Msg, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.ToString());
            }
            frmMain.gfrmMain.SetNormalStaus("");
        }
        private DataTable GetEInvoiceEmailInfo(int? DocKey)
        {
            using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
            {
                DataTable dt = null;
                cn.Open();
                try
                {
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@DocCodeKey", DocCodeKey));
                    parmList.Add(new SqlParameter("@DocKey", DocKey));
                    dt = GFunc.ExecuteProc(cn, "ARIV_GetEInvoiceEmailInfo", parmList);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.ToString());
                }
                return dt;
            }
        }
        private string UpdateEInvoiceEmailStatus(int? DocKey)
        {
            string result = "";
            using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
            {
                cn.Open();
                try
                {
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@DocKey", DocKey));
                    parmList.Add(new SqlParameter("@SentUserKey", AppInfor.CurrentUserKey));
                    result = GFunc.ExecuteScalar(cn, "ARIV_UpdateEInvoiceEmailStatus", parmList);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.ToString());
                }
            }
            return result;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tagrdAttachment.ActiveRow != null)
            {
                DownloadFile(tagrdAttachment.ActiveRow, true,"");
            }
        }
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            string Result = "";
            try
            {
                if (tagrdAttachment.Selected.Rows.Count > 0)
                {
                    object DocFileName = tagrdAttachment.Selected.Rows[0].Cells["AttachDes"].Value;
                    List<SqlParameter> parmList = new List<SqlParameter>();
                    parmList.Add(new SqlParameter("@DocDC", DocCodeKey));
                    parmList.Add(new SqlParameter("@DocDK", DocKey));
                    parmList.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                    parmList.Add(new SqlParameter("@DocType", (this.Text == "Customer PO Attachments" ? "PO" : "DO")));
                    parmList.Add(new SqlParameter("@DocFileName", DocFileName != null ? DocFileName.ToString() : ""));
                    Result = GFunc.ExecuteScalar("SYSAttachments_CopyDOToIV", parmList);
                }
                else
                {
                    MsgBox.Show("Please select the row that you want to copy the file!");
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.ToString());
            }

            if ( Result != "")
            {
                MsgBox.Show(Result);
            }
        }
        /*end by YST */
        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                using(FolderBrowserDialog fBDlg = new FolderBrowserDialog())
                {
                    fBDlg.Description = "Please select a location to download the file";
                    fBDlg.SelectedPath = SysOptionUtility.GetSysOpStr("DefaultInvoiceDownloadFilePath");
                    fBDlg.ScrollSelectedPathIntoView();
                    if (fBDlg.ShowDialog() == DialogResult.OK)
                    {
                        DownloadFile(tagrdAttachment.ActiveRow, false, fBDlg.SelectedPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.ToString());
            }
        }        
    }
    public static class FolderBrowserDialogExt
    {
        public static void ScrollSelectedPathIntoView(this FolderBrowserDialog fbd)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                SendKeys.SendWait("{TAB}");
                SendKeys.SendWait("{TAB}");
                SendKeys.SendWait("{RIGHT}");
            });
        }
    }
}
