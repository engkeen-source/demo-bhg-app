using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using BOLib;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;
using TAUtil;
using WinUI.Forms.PoupBrowser;
using System.Drawing;
using System.Data.SqlClient;
using System.Collections;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinGrid.DocumentExport;
using System.Runtime.InteropServices;
using Infragistics.Win.UltraWinTabControl;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Linq.Expressions;
using Infragistics.Win.SupportDialogs.FilterUIProvider;

using Microsoft.Win32;
using System.Threading;

namespace WinUI
{

    public class ActiveFormInfo
    {
        public int codeKey;
        public string formNm;
        public string gridNm;
        public string controlOrCellNm;
    }

    public static class GlobalUI
    {
        public static DateTime ListDefaultFromDate=DateTime.Today.AddDays(-30);
        public static DateTime ListDefaultToDate=DateTime.Today;

        public static DialogResult PasswordInputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
            textBox.PasswordChar = '*';

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        #region Variables
        public static ContextMenuStrip cmnuGlobal;//ContextMenuStrip
        public static DataTable dtTransferLogicData;
        #endregion

        #region Delegate for List Refresh Events & others

        public static GVar.ListUpdateEvent MSTAccListEvent;
        public static GVar.RecordSelectedEvent MSTAccSelectedEvent;

        public static GVar.ListUpdateEvent MSTConListEvent;
        public static GVar.RecordSelectedEvent MSTConSelectedEvent;

        public static GVar.ListUpdateEvent MSTPriceListEvent;
        public static GVar.RecordSelectedEvent MSTPriceSelectedEvent;

        public static GVar.ListUpdateEvent MSTJobListEvent;
        public static GVar.RecordSelectedEvent MSTJobSelectedEvent;

        public static GVar.ListUpdateEvent MSTItmListEvent;
        public static GVar.RecordSelectedEvent MSTItmSelectedEvent;

        public static GVar.ListUpdateEvent MSTEqptListEvent;
        public static GVar.RecordSelectedEvent MSTEqptSelectedEvent;

        public static GVar.ListUpdateEvent ARQOListEvent;
        public static GVar.ListUpdateEvent ARQOSelectedEvent;

        public static GVar.ListUpdateEvent ARSOListEvent;
        public static GVar.ListUpdateEvent ARSOSelectedEvent;

        public static GVar.ListUpdateEvent APPJListEvent;
        public static GVar.ListUpdateEvent APPJSelectedEvent;

        public delegate void DependentFillEvent(string thing);
        public static DependentFillEvent dependenFillEvent = null;
        #endregion

        #region DLL Import Functions & Variables
        /* This dll Import For To Find Focused Control which The Control Is Inside At Container(panal,splitContainer,groupBox)
         * and Use at SelectNextControl() --Albert       */
        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();
        public static DataTable SelectGridRowData { get; set; } //keep data from recordsearch from selected row. --Pauk

        //this function will prevent form Painting
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        public const int WM_SETREDRAW = 11;
        public static bool bRuningImport = false;
        /* Before drawing:
            SendMessage(ObjectControlPanel.Handle, WM_SETREDRAW, false, 0);
        After drawing:
            SendMessage(ObjectControlPanel.Handle, WM_SETREDRAW, true, 0);
        You may then need to call Refresh ().
        */

        #endregion

        #region Form Control Methods
        public class Form_Name
        {
            //REFERENCE FORMS

            //MASTER FORMS
            public const string FRM_MST_ACCOUNT = "frmMstAcc";
            public const string FRM_MST_ACCOUNT_LIST = "frmMstAccList";
            public const string FRM_MST_CON_LIST = "frmMstConList";
            public const string FRM_MST_PRICE_INFO = "frmMSTPriceInfo";
            public const string FRM_MST_PRICE_LIST = "frmMSTPriceList";
            public const string FRM_MST_ITEM = "frmMstItm";
            public const string FRM_MST_ITEM_LIST = "frmMstItmList";
            public const string FRM_MST_EQUIPMENT = "frmMSTEqpt";
            public const string FRM_MST_EQUIPMENT_LIST = "frmMstEqptList";

            //LIST FORMS
            public const string FRM_DOCUMENT_LIST = "frmDocList";
            public const string FRM_LIST = "frmList";
            public const string FRM_REPORTVIEWER = "frmReportViewer";

            //POPUP FORMS
            public const string FRM_ITMENQUIRY = "frmItmEnquiry";
            public const string FRM_ITMHISSUMMARY = "frmItmHisSummary";
            public const string FRM_ITMHISTORYPURCHASE = "frmItmHistoryPurchase";
            public const string FRM_ITMHISTORYSALE = "frmItmHistorySale";
            public const string FRM_DOCLISTDET = "frmDocListDet";
            public const string FRM_BATCHENTRY = "frmBatchEntry";
            public const string FRM_BATCHSELECTION = "frmBatchSelection";
            public const string FRM_ASSEMBLYENTRY = "frmAssemblyEntry";
            public const string FRM_DOCRELATIONSHIP = "frmDocRelationship";


        }
        public interface ISearchable
        {
            void Search(SearchInfo objSearchInfo);
        }
        public class SearchInfo
        {
            public string searchString;
            public string lookIn;
            public SearchDirectionEnum searchDirection;
            public SearchContentEnum searchContent;
            public bool matchCase = false;
            public Infragistics.Win.UltraWinGrid.UltraGrid gridToSeach = null;

            public SearchInfo()
            {
                this.searchContent = SearchContentEnum.AnyPartOfField;
                this.lookIn = "";
                this.matchCase = false;
                this.searchDirection = SearchDirectionEnum.Down;
                this.searchString = "";
            }
        }
        public enum SearchDirectionEnum
        {
            Down = 0,
            Up = 1,
            All = 2
        }
        public enum SearchContentEnum
        {
            AnyPartOfField = 0,
            WholeField = 1,
            StartOfField = 2
        }
        public static void Search(GlobalUI.SearchInfo objSearchInfo)
        {
            GlobalUI.SearchInfo searchInfo = objSearchInfo;
            UltraGridColumn searchCol;
            //   See if there is an active row; if there is, use it, otherwise
            //   activate the first row and start the search from there
            UltraGrid grdList = objSearchInfo.gridToSeach;


            if (grdList.ActiveCell != null)
            {
                searchCol = grdList.ActiveCell.Column;

                UltraGridRow oRow = grdList.ActiveRow;

                if (oRow == null)
                    oRow = grdList.GetRow(Infragistics.Win.UltraWinGrid.ChildRow.First);

                //   Use the row object//s GetSibling method to iterate through the rows
                //   and check the appropriate cell values

                //   Downward search
                if (searchInfo.searchDirection == GlobalUI.SearchDirectionEnum.Down)
                {
                    while (oRow != null)
                    {
                        oRow = oRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);
                        if (MatchText(grdList, oRow, searchInfo, ref searchCol))
                        {
                            grdList.ActiveRow = oRow;
                            if (searchCol != null)
                            {
                                grdList.ActiveCell = oRow.Cells[searchCol.Key];
                                grdList.Selected.Rows.Clear();
                                grdList.ActiveCell.Selected = true;
                                grdList.ActiveRowScrollRegion.ScrollRowIntoView(oRow);
                                grdList.ActiveColScrollRegion.ScrollColIntoView(searchCol, true);
                            }
                            return;
                        }
                    }
                }
                //   Upward search
                else if (searchInfo.searchDirection == GlobalUI.SearchDirectionEnum.Up)
                {
                    while (oRow != null)
                    {
                        oRow = oRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Previous);
                        if (MatchText(grdList, oRow, searchInfo, ref searchCol))
                        {
                            grdList.ActiveRow = oRow;
                            if (searchCol != null)
                            {
                                grdList.ActiveCell = oRow.Cells[searchCol.Key];
                                grdList.Selected.Rows.Clear();
                                grdList.ActiveCell.Selected = true;
                                grdList.ActiveRowScrollRegion.ScrollRowIntoView(oRow);
                                grdList.ActiveColScrollRegion.ScrollColIntoView(searchCol, true);
                            }
                            return;
                        }
                    }
                }
                //   Search all rows. First, we start with the active row. If we don//t find
                //   it by the time we hit the  last row, try again starting from the first row
                else if (searchInfo.searchDirection == GlobalUI.SearchDirectionEnum.All)
                {
                    while (oRow != null)
                    {
                        oRow = oRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);
                        if (MatchText(grdList, oRow, searchInfo, ref searchCol))
                        {
                            grdList.ActiveRow = oRow;

                            if (searchCol != null)
                            {
                                grdList.ActiveCell = oRow.Cells[searchCol.Key];
                                grdList.Selected.Rows.Clear();
                                grdList.ActiveCell.Selected = true;

                                // Call the ScrollRowIntoView on the active col-scroll-region to scroll the row into view 
                                // vertically.
                                grdList.ActiveRowScrollRegion.ScrollRowIntoView(oRow);
                                grdList.ActiveColScrollRegion.ScrollColIntoView(searchCol, true);

                            }
                            return;
                        }
                    }

                    //   We didn't find it the first time around, so start again from the first row
                    oRow = grdList.GetRow(Infragistics.Win.UltraWinGrid.ChildRow.First);
                    while (oRow != null)
                    {
                        if (MatchText(grdList, oRow, searchInfo, ref searchCol))
                        {
                            grdList.ActiveRow = oRow;
                            if (searchCol != null)
                            {
                                grdList.ActiveCell = oRow.Cells[searchCol.Key];
                                grdList.Selected.Rows.Clear();
                                grdList.ActiveCell.Selected = true;
                                grdList.ActiveRowScrollRegion.ScrollRowIntoView(oRow);
                                grdList.ActiveColScrollRegion.ScrollColIntoView(searchCol, true);
                            }
                            return;
                        }
                        oRow = oRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);
                    }

                }

                //   If we get this far, we didn//t find the string, so show a message box
                Form f = GetActiveControlAtActiveForm().FindForm();
                if (f != null)
                {
                    f.Hide();
                    if (grdList.ActiveCell != null && objSearchInfo!=null)
                    {
                        if (grdList.ActiveCell.Text.ToLower().Contains(searchInfo.searchString.ToLower()))
                            MsgBox.Show("No more occurrence found for '" + searchInfo.searchString + "'.", GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                        else
                            MsgBox.Show("Could not find '" + searchInfo.searchString + "'.", GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                    }
                    else
                        MsgBox.Show("Could not find '" + searchInfo.searchString + "'.", GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);

                    f.Show();
                }
            }

        }
        private static bool Match(string userString, string cellValue, SearchInfo searchInfo)
        {
            //   If our search is case insensitive, make both strings uppercase
            if (!searchInfo.matchCase)
            {
                userString = userString.ToUpper();
                cellValue = cellValue.ToUpper();
            }

            //   If we are searching any part of the cell value...
            if (searchInfo.searchContent == GlobalUI.SearchContentEnum.AnyPartOfField)
            {
                //   If the user string is larger than the cell value, it is by definition
                //   a mismatch, so return false
                if (userString.Length > cellValue.Length)
                    return false;
                else if (userString.Length == cellValue.Length)
                {
                    //   If the lengths are equal, the strings must be equal as well
                    if (userString == cellValue)
                        return true;
                    else
                        return false;
                }
                else
                {
                    //   There is probably an easier way to do this
                    for (int i = 0; i <= (cellValue.Length - userString.Length); i++)
                    {
                        if (userString == cellValue.Substring(i, userString.Length))
                            return true;
                    }

                    return false;

                }
            }
            else if (searchInfo.searchContent == GlobalUI.SearchContentEnum.WholeField)
            {
                if (userString == cellValue)
                    return true;
                else
                    return false;
            }
            else if (searchInfo.searchContent == GlobalUI.SearchContentEnum.StartOfField)
            {
                if (userString.Length >= cellValue.Length)
                {
                    if (userString.Substring(0, cellValue.Length) == cellValue)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (cellValue.Substring(0, userString.Length) == userString)
                        return true;
                    else
                        return false;
                }

            }

            return false;
        }
        private static bool MatchText(UltraGrid gridToSeach, UltraGridRow oRow, SearchInfo searchInfo, ref UltraGridColumn searchCol)
        {
            UltraGrid grdList = gridToSeach;
            if (oRow == null)
                return false;

            string strColumnKey = searchInfo.lookIn;
            //string strCellValue = "";

            //   Determine whether we are searching the current column or all columns
            bool bSearchAllColumns = true;

            if (grdList.DisplayLayout.Bands[0].Columns.Exists(strColumnKey))
                bSearchAllColumns = false;
            //   If we are searching all columns then we must iterate through all the cells
            //    in this row, which we can do by using the band//s Columns collection
            if (bSearchAllColumns)
            {
                foreach (UltraGridColumn oCol in grdList.DisplayLayout.Bands[0].Columns)
                {
                    if (oCol.Hidden) continue;
                    if (oRow.Cells[oCol.Key].Value != null)
                    {
                        if (Match(searchInfo.searchString, oRow.Cells[oCol.Key].Value.ToString(), searchInfo))
                        {
                            searchCol = oCol;
                            return true;
                        }
                    }
                }
            }
            else
            {
                UltraGridColumn oCol = grdList.DisplayLayout.Bands[0].Columns[strColumnKey];
                if (oRow.Cells[oCol.Key].Value != null)
                {
                    if (Match(searchInfo.searchString, oRow.Cells[oCol.Key].Value.ToString(), searchInfo))
                    {
                        searchCol = oCol;
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion


        public static void Export(UltraGrid grid, ref int exportCount)
        {
            UltraGridExcelExporter excelExporter = new UltraGridExcelExporter();
            UltraGridDocumentExporter docExporter = new UltraGridDocumentExporter();
            try
            {
                SaveFileDialog ofDlg = new SaveFileDialog();
                ofDlg.Filter = "Microsoft Excel (*.xls)|*.xls|Portable Document Files (*.pdf)|*.pdf|XML Paper Files (*.xps)|*.xps";
                ofDlg.Title = "Export Data";
                ofDlg.FileName = "ExportFile" + exportCount++.ToString() + ".xls";
                DialogResult dlg = ofDlg.ShowDialog();
                if (dlg == DialogResult.OK)
                {
                    switch (ofDlg.FileName.Substring(ofDlg.FileName.Length - 4, 4))
                    {
                        case ".pdf": docExporter.Export(grid, ofDlg.FileName, Infragistics.Win.UltraWinGrid.DocumentExport.GridExportFileFormat.PDF); break;
                        case ".xls": excelExporter.Export(grid, ofDlg.FileName); break;
                        case ".xps": docExporter.Export(grid, ofDlg.FileName, Infragistics.Win.UltraWinGrid.DocumentExport.GridExportFileFormat.XPS); break;
                    }
                    // Check Option Value is True (or) False
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.QuestionOpenExportFile))
                        System.Diagnostics.Process.Start(ofDlg.FileName);                   
                }
            }

            catch (Exception ex)
            {
                MsgBox.Show("The system cannot export to the specific file. The file is opened." + ex.Message, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                throw Error(ex, false, false);
            }
        }      

        public static List<ReportParameter> GetReportParameters()
        {
            try
            {
                List<ReportParameter> l_Reval = new List<ReportParameter>();

                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                string opCmpRegValue = SysOptionUtility.GetStr("GSTRegNumber");
                int opaddrKey = SysOptionUtility.GetInt("DefaultLetterHeadAddr");
                string opCmpAddrValue = GFunc.AddrGet(opaddrKey, true);

                // cann't use objRepListFactory.ObjRepParameters. because there is no records in sys_repCriteria for printing.
                ReportParameter l_ParampCmpName = new ReportParameter("pCmpName", opCmpValue);
                ReportParameter l_ParampCmpAddr = new ReportParameter("pCmpAddr", opCmpAddrValue);
                ReportParameter l_ParampGSTRegNum = new ReportParameter("pGSTRegNum", opCmpRegValue);

                l_Reval.Add(l_ParampCmpName);
                l_Reval.Add(l_ParampCmpAddr);
                l_Reval.Add(l_ParampGSTRegNum);


                return l_Reval;
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

        //Context Menu Related Functions
        public static ContextMenuStrip GlobalContextMenuStrip_Get()
        {
            return cmnuGlobal;
        }
        public static void cmnuGlobal_Initialize()
        {
            try
            {
                cmnuGlobal = new ContextMenuStrip();

                cmnuGlobal.Opening += new System.ComponentModel.CancelEventHandler(cmnuGlobal_Opening);

                //Index => 0
                ToolStripMenuItem cmnuHideColumn = new ToolStripMenuItem("Hide Column");
                cmnuHideColumn.Name = "HideColumn";
                cmnuHideColumn.Click += new EventHandler(cmnuHideColumn_Click);
                cmnuGlobal.Items.Add(cmnuHideColumn);
                //Index => 1
                ToolStripMenuItem cmnuShowColumn = new ToolStripMenuItem("Show Column");
                cmnuShowColumn.Name = "ShowColumn";
                cmnuShowColumn.Click += new EventHandler(cmnuShowColumn_Click);
                cmnuGlobal.Items.Add(cmnuShowColumn);
                //Index => 2
                ToolStripMenuItem cmnuFind = new ToolStripMenuItem("Find");
                cmnuFind.Name = "Find";
                cmnuFind.ShortcutKeys = Keys.Control | Keys.F;
                cmnuFind.Click += new EventHandler(cmnuFind_Click);
                cmnuGlobal.Items.Add(cmnuFind);
                //Index => 3
                ToolStripMenuItem cmnuWhereEqual = new ToolStripMenuItem("Where (Equal)");
                cmnuWhereEqual.Name = "WhereEqual";
                cmnuWhereEqual.Click += new EventHandler(cmnuWhereEqual_Click);
                cmnuGlobal.Items.Add(cmnuWhereEqual);
                //Index => 4
                ToolStripMenuItem cmnuWhereNotEqual = new ToolStripMenuItem("Where (Not Equal)");
                cmnuWhereNotEqual.Name = "WhereNotEqual";
                cmnuWhereNotEqual.Click += new EventHandler(cmnuWhereNotEqual_Click);
                cmnuGlobal.Items.Add(cmnuWhereNotEqual);
                //Index => 5
                ToolStripMenuItem cmnuZoom = new ToolStripMenuItem("Zoom");
                cmnuZoom.Name = "Zoom";
                cmnuZoom.Click += new EventHandler(cmnuZoom_Click);
                cmnuGlobal.Items.Add(cmnuZoom);
                //Index => 6
                ToolStripMenuItem cmnuRemoveFilter = new ToolStripMenuItem("Remove Filter");
                cmnuRemoveFilter.Name = "RemoveFilter";
                cmnuRemoveFilter.Click += new EventHandler(cmnuRemoveFilter_Click);
                cmnuGlobal.Items.Add(cmnuRemoveFilter);
                //Index => 7
                ToolStripMenuItem cmnuAdd = new ToolStripMenuItem("Add");
                cmnuAdd.Name = "Add";
                cmnuAdd.Click += new EventHandler(cmnuAdd_Click);
                cmnuGlobal.Items.Add(cmnuAdd);
                //Index => 8
                ToolStripMenuItem cmnuEdit = new ToolStripMenuItem("Edit");
                cmnuEdit.Name = "Edit";
                cmnuEdit.Click += new EventHandler(cmnuEdit_Click);
                cmnuGlobal.Items.Add(cmnuEdit);
                //Index => 9
                ToolStripMenuItem cmnuInsertRow = new ToolStripMenuItem("Insert Row");
                cmnuInsertRow.Name = "InsertRow";
                cmnuInsertRow.Click += new EventHandler(cmnuInsertRow_Click);
                cmnuGlobal.Items.Add(cmnuInsertRow);
                //Index => 10
                ToolStripMenuItem cmnuInsertData = new ToolStripMenuItem("Insert Data");
                cmnuInsertData.Name = "InsertData";
                cmnuInsertData.Click += new EventHandler(cmnuInsertData_Click);
                cmnuGlobal.Items.Add(cmnuInsertData);
                //Index => 11
                ToolStripMenuItem cmnuInsertSO = new ToolStripMenuItem("Insert SO");
                cmnuInsertSO.Name = "InsertSO";
                cmnuInsertSO.Click += new EventHandler(cmnuInsertSO_Click);
                cmnuGlobal.Items.Add(cmnuInsertSO);
                //Index => 12
                ToolStripMenuItem cmnuInsertPO = new ToolStripMenuItem("Insert PO");
                cmnuInsertPO.Name = "InsertPO";
                cmnuInsertPO.Click += new EventHandler(cmnuInsertPO_Click);
                cmnuGlobal.Items.Add(cmnuInsertPO);
                //Index => 13
                ToolStripMenuItem cmnuInsertPD = new ToolStripMenuItem("Insert PD");
                cmnuInsertPD.Name = "InsertPD";
                cmnuInsertPD.Click += new EventHandler(cmnuInsertPD_Click);
                cmnuGlobal.Items.Add(cmnuInsertPD);
                //Index => 14
                ToolStripMenuItem cmnuInsertCO = new ToolStripMenuItem("Insert CO");
                cmnuInsertCO.Name = "InsertCO";
                cmnuInsertCO.Click += new EventHandler(cmnuInsertCO_Click);
                cmnuGlobal.Items.Add(cmnuInsertCO);
                //Index => 15
                ToolStripMenuItem cmnuInsertCS = new ToolStripMenuItem("Insert CS");
                cmnuInsertCS.Name = "InsertCS";
                cmnuInsertCS.Click += new EventHandler(cmnuInsertCS_Click);
                cmnuGlobal.Items.Add(cmnuInsertCS);
                //Index => 16
                ToolStripMenuItem cmnuPreviousPrice = new ToolStripMenuItem("Previous Price");
                cmnuPreviousPrice.Name = "PreviousPrice";
                cmnuPreviousPrice.Click += new EventHandler(cmnuPreviousPrice_Click);
                cmnuGlobal.Items.Add(cmnuPreviousPrice);
                //Index => 17
                ToolStripMenuItem cmnuAlternateItemList = new ToolStripMenuItem("Alternate Item List");
                cmnuAlternateItemList.Name = "AlternateItemList";
                cmnuAlternateItemList.Click += new EventHandler(cmnuAlternateItemList_Click);
                cmnuGlobal.Items.Add(cmnuAlternateItemList);
                //Index => 18
                ToolStripMenuItem cmnuDataMatrix = new ToolStripMenuItem("Data Matrix");
                cmnuDataMatrix.Name = "Data Matrix";
                cmnuDataMatrix.Click += new EventHandler(cmnuDataMatrix_Click);
                cmnuGlobal.Items.Add(cmnuDataMatrix);
                //Index => 18
                ToolStripMenuItem cmnuOpenDocument = new ToolStripMenuItem("Open Document");
                cmnuOpenDocument.Name = "Open Document";
                cmnuOpenDocument.Click += new EventHandler(cmnuOpenDocument_Click);
                cmnuGlobal.Items.Add(cmnuOpenDocument);

                //Index => 19
                ToolStripMenuItem cmnuExportGrid = new ToolStripMenuItem("Export All Rows to Excel");
                cmnuExportGrid.Name = "Export";
                cmnuExportGrid.Click += new EventHandler(cmnuExportGrid_Click);
                cmnuGlobal.Items.Add(cmnuExportGrid);

            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false, true);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, true);
            }
        }

        public static void cmnuGlobal_Set(Form f)
        {
            f.Cursor = Cursors.WaitCursor;
            try
            {
                System.Collections.ArrayList toDo = new System.Collections.ArrayList();
                bool working = true;
                int index = 0;
                Control current = null;

                //Loop whole form's controls to set Context Menu
                //Loop Top Level Controls
                foreach (Control c in f.Controls)
                {
                    toDo.Add(c);
                }
                while (working)
                {
                    current = (Control)toDo[index];
                    //Loop Below Top Level Controls
                    foreach (Control c2 in current.Controls)
                    {
                        toDo.Add(c2);
                    }
                    if (current.HasChildren == false && (current.GetType() == typeof(TAUtil.TAComboBox) || current.GetType() == typeof(TAUtil.TAGridEditor)))
                    {
                        switch (f.Name.ToLower())
                        {
                            #region For Transaction Forms
                            #region For AR Forms
                            case "frmarqo":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "docquotestatus":
                                    case "defjobkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                    case "rfqvendorsendmode":
                                    case "rfqvendorrpt":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmarso":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmardo":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmarpl":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "packingtype":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmariv":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmarpy":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "docbaddrcountry":
                                    case "docbaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmappy":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "docbaddrcountry":
                                    case "docbaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmaradj":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion For AR Forms

                            #region For AP Forms
                            case "frmapbl":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "defbaddrkey":
                                    case "docbaddrcountry":
                                    case "docbaddrregion":
                                        // case "deflockey":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            //case "frmapbd":
                            //    switch (current.Name.ToLower())
                            //    {
                            //        case "docstate":
                            //        case "doctypenm":
                            //        case "defjobkey":
                            //        case "approveuserkey":
                            //        case "disapproveuserkey":
                            //        case "defbaddrkey":
                            //        case "docbaddrcountry":
                            //        case "docbaddrregion":
                            //            break;
                            //        default:
                            //            current.ContextMenuStrip = cmnuGlobal;
                            //            break;
                            //    }
                            //    break;
                            case "frmappo":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                    case "deflockey":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmappn":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "planmthrange":
                                    case "plandistributeinterval":
                                    case "qtymultiplier":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmappd":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmaprq":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion For AP Forms
                            #region For Consignment
                            case "frmcscsi":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmcscps":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmcscpo":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmcscpd":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "defbaddrkey":
                                    case "defsaddrkey":
                                    case "docbaddrcountry":
                                    case "docsaddrcountry":
                                    case "docbaddrregion":
                                    case "docsaddrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion For Consignment
                            #region Inventory Forms
                            case "frminmfn":
                                switch (current.Name.ToLower())
                                {
                                    case "details":
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frminadj":
                                switch (current.Name.ToLower())
                                {
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmintrn":
                                switch (current.Name.ToLower())
                                {
                                    case "doctypenm":
                                    case "defjobkey":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion Inventory Forms
                            #region For Account Forms
                            case "frmgljnl":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmglrv":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmgldp":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion

                            #region For Adjustment
                            case "frmappj":
                                switch (current.Name.ToLower())
                                {
                                    case "docstate":
                                    case "doctypenm":
                                    case "approveuserkey":
                                    case "disapproveuserkey":
                                    case "":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion
                            #endregion Transaction Forms

                            #region For MASTER Forms
                            case "frmmstitm":
                                switch (current.Name.ToLower())
                                {
                                    case "masteritmkey":
                                    case "itmtype":
                                    case "masteritmtype":
                                    case "costmethod":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmmstcon":
                                switch (current.Name.ToLower())
                                {
                                    case "contype":
                                    case "ccbtype":
                                    case "congender":
                                    case "conmarital":
                                    case "connationality":
                                    case "cdefaultbilladdr":
                                    case "cdefaultshipaddr":
                                    case "cdefaultstatetype":
                                    case "cdefaultstateaddr":
                                    case "cdefaultcontact":
                                    case "vdefaultbilladdr":
                                    case "vdefaultshipaddr":
                                    case "vdefaultcontact":
                                    case "addrtype":
                                    case "addrcountry":
                                    case "addrregion":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmmstacc":
                                switch (current.Name.ToLower())
                                {
                                    case "acctypekey":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmmstjob":
                                switch (current.Name.ToLower())
                                {
                                    case "jobstatus":
                                    case "jobshipname":
                                    case "jobshipmark":
                                    case "costtype":
                                    case "transmitstatus":
                                    case "othlinetype":
                                    case "costgrp":
                                    case "estcopyptct":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmmstpriceinfo":
                                switch (current.Name.ToLower())
                                {
                                    case "pricetype":
                                    case "buildincode":
                                    case "ratiotype":
                                        { break; }
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmmstsalesrep":
                                switch (current.Name.ToLower())
                                {
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmmstbudget":
                                switch (current.Name.ToLower())
                                {
                                    case "budgettype":
                                    case "periodfrom":
                                    case "acckey":
                                    case "periodto":
                                    case "budgetitmmode":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;

                            case "frmmsttimesheet":
                                switch (current.Name.ToLower())
                                {
                                    case "openmonth":
                                    case "period":
                                    case "openemployee":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmitemstocktake":
                                switch (current.Name.ToLower())
                                {
                                    case "counttype":

                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion MASTER Forms

                            #region For System Forms
                            case "frmtasalert":
                                switch (current.Name.ToLower())
                                {
                                    case "alertapplygrp":
                                    case "alertapplyto":
                                    case "alertcondition":
                                    case "alerttype":
                                    case "userkey1":
                                    case "userkey2":
                                    case "userkey3":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmtastodo":
                                switch (current.Name.ToLower())
                                {
                                    case "todopriority":
                                    case "todotype":
                                    case "recurtype":
                                    case "remindtype":
                                    case "recurintweekday":
                                    case "recurintmthweek":
                                    case "recurintyearmthnum":
                                    case "recurintyearmthweek":
                                    case "docdc":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmsyscodehdr":
                                switch (current.Name.ToLower())
                                {
                                    case "codegrp":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmsysoptiondaddr":
                                switch (current.Name.ToLower())
                                {
                                    case "addrtype":
                                    case "addrcountry":
                                    case "addrregion":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion System Forms

                            #region For Security Forms
                            case "frmsecgroup":
                                switch (current.Name.ToLower())
                                {
                                    case "filterby":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            #endregion Security Forms

                            #region Reference Forms
                            case "frmrefuom":
                                switch (current.Name.ToLower())
                                {
                                    case "uomtype":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmrefjobcosttype":
                                switch (current.Name.ToLower())
                                {
                                    case "jobcostheadertype":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            case "frmreftaxgrp":
                                switch (current.Name.ToLower())
                                {
                                    case "reportcode":
                                        break;
                                    default:
                                        current.ContextMenuStrip = cmnuGlobal;
                                        break;
                                }
                                break;
                            //case "":
                            //    switch (current.Name)
                            //    {
                            //        case "":
                            //            break;
                            //        default:
                            //            current.ContextMenuStrip = cmnuGlobal;
                            //            break;
                            //    }
                            //    break;
                            default:
                                current.ContextMenuStrip = cmnuGlobal;
                                break;
                            #endregion Reference Forms

                        }
                    }
                    index++;
                    if (toDo.Count == index)
                    {
                        working = false;
                    }
                }
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                f.Cursor = Cursors.Default;
            }
        }

        internal static ActiveFormInfo GetControlNmHierarchy()
        {
            try
            {
                ActiveFormInfo activFormInfo = new ActiveFormInfo();
                Control control = null;
                string gridColumnBindingFieldName = string.Empty;
                string listSettingID = string.Empty;
                Control activeControl = GetActiveControlAtActiveForm();
                #region Get CodeKey
                int codeKey = ActiveFormDocCodeAll_Get();
                if (codeKey == 0)
                {
                    Form frm = activeControl.FindForm();
                    FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    //declaring one should be 'declaredobjectName'
                    object objFactory = myFieldInfo.First(o => o.Name.ToLower().Contains("Factory".ToLower())).GetValue(frm);
                    if (objFactory != null)
                    {
                        FieldInfo[] moreFieldInfo = objFactory.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        //declaring one should be 'declaredobjectName'
                        if (moreFieldInfo.Count(o => o.Name.ToLower().Contains("CodeKey".ToLower())) > 0)
                        {
                            object objCodeKey = moreFieldInfo.First(o => o.Name.ToLower().Contains("CodeKey".ToLower())).GetValue(objFactory);
                            if (objCodeKey != null)
                            {
                                codeKey = GFunc.NEInt(objCodeKey, 0);
                            }
                        }
                        else // For such a case there's no codeKey enum inside
                        {
                            //FutureFeature; Implement later
                        }
                    }
                    else  // For such a case there's no factory object inside
                    {
                        //FutureFeature; Implement later
                    }
                }
                #endregion
                activFormInfo.codeKey = codeKey;
                activFormInfo.formNm = activeControl.FindForm().Name;

                if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor)) //Grid
                {
                    control = (UltraGrid)activeControl.Parent;
                    activFormInfo.controlOrCellNm = control.Name;
                }
                else if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    control = (UltraGrid)activeControl;
                    activFormInfo.controlOrCellNm = control.Name;
                }
                else if (
                    activeControl.Parent.GetType() == typeof(TAUtil.TAComboBox) ||  //Combo or ComboEditor
                    activeControl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor) ||  //Textbox or TextboxEditor
                    activeControl.Parent.GetType() == typeof(TAUtil.TANumericEditor) ||
                    activeControl.Parent.GetType() == typeof(TAUtil.TADateEditor) ||
                    activeControl.Parent.GetType() == typeof(Button)
                    )
                {
                    control = activeControl.Parent;
                    activFormInfo.gridNm = "NA";
                    activFormInfo.controlOrCellNm = control.Name;
                    return activFormInfo;
                }
                else
                {
                    //Not supported controls
                    return null;
                }

                if (control.GetType() == typeof(TAGridEditor))
                {
                    UltraGrid grid = (UltraGrid)control;
                    if (grid.ActiveCell == null)
                    {
                        UltraGridColumn FirstVisCol = grid.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                        if (FirstVisCol != null)
                        {
                            gridColumnBindingFieldName = FirstVisCol.Key;
                            activFormInfo.controlOrCellNm = gridColumnBindingFieldName;
                            return activFormInfo;
                        }
                    }

                    if (grid.ActiveCell != null)
                    {
                        gridColumnBindingFieldName = grid.DisplayLayout.Bands[0].Columns[grid.ActiveCell.Column.Index].Key;
                        activFormInfo.controlOrCellNm = gridColumnBindingFieldName;
                        return activFormInfo;
                    }
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
            return null;
        }

        internal static bool cmnuGlobal_Bind()
        {
            string contextMenuFlagList = "";
            string poupFormName = "";

            try
            {
                Control control = null;
                string gridColumnBindingFieldName = string.Empty;
                string listSettingID = string.Empty;
                Control activeControl = GetActiveControlAtActiveForm();

                if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor)) //Grid
                {
                    control = (UltraGrid)activeControl.Parent;
                }
                else if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    control = (UltraGrid)activeControl;
                }
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAComboBox))//Combo or ComboEditor
                {
                    control = activeControl.Parent;
                }
                else
                {
                    //Other Controls
                    control = activeControl.Parent;
                }

                //if Combo Or Combo Ediotr should have Bindings , otherwise if it's grid, should have gridKey's name (gridColBindingField Name)
                if ((control.DataBindings == null ? false : control.DataBindings.Count > 0) || gridColumnBindingFieldName.Length > 0)
                { //Header Control with Binding Field Nm.
                    string bindingFieldNm;
                    //Get the Binding Field Name of Control
                    if (gridColumnBindingFieldName.Length > 0)
                        bindingFieldNm = gridColumnBindingFieldName;
                    else
                        bindingFieldNm = control.DataBindings[0].BindingMemberInfo.BindingField;

                    ControlFormatFlag_Set(bindingFieldNm, 0, out contextMenuFlagList, out poupFormName, out listSettingID);//Check with Code Key

                }
                else
                {
                    if (control.GetType() == typeof(TAGridEditor))
                    {
                        UltraGrid grid = (UltraGrid)control;
                        if (grid.ActiveCell == null)
                        {
                            UltraGridColumn FirstVisCol = grid.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                            if (FirstVisCol != null)
                            {
                                gridColumnBindingFieldName = FirstVisCol.Key;
                            }
                        }

                        if (grid.ActiveCell != null)
                        {
                            gridColumnBindingFieldName = grid.DisplayLayout.Bands[0].Columns[grid.ActiveCell.Column.Index].Key;
                        }
                        ControlFormatFlag_Set(gridColumnBindingFieldName + "#" + control.Name, 1, out contextMenuFlagList, out poupFormName, out listSettingID);

                    }
                    else//Header Control that hasn't BindingField.
                        //ControlFormatFlag_Set("0BF#NA#" + control.Name, 0, out contextMenuFlagList, out poupFormName, out listSettingID);
                        ControlFormatFlag_Set(control.Name, 0, out contextMenuFlagList, out poupFormName, out listSettingID);
                }


                if (contextMenuFlagList != "0")
                {
                    cmnuGlobal.AccessibleDescription = poupFormName;//Retain Popup Form Name To be displayed
                    cmnuGlobal.Tag = listSettingID; //Retain List ID for Combo Rebind
                    MenuItm_Set(control, contextMenuFlagList);
                }
                else
                {
                    MenuItm_Set(control, contextMenuFlagList);
                }


            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

            return true;
        }
        static void cmnuGlobal_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cmnuGlobal_Bind() == false)
            { e.Cancel = true; }

        }
        private static void MenuItm_Set(Control ctrl, string contextMenuFlagList)
        {

            //Create Available Menus
            ToolStripMenuItem cmnuHideColumn = new ToolStripMenuItem("Hide Column");
            cmnuHideColumn.Name = "HideColumn";
            cmnuHideColumn.Click += new EventHandler(cmnuHideColumn_Click);

            ToolStripMenuItem cmnuShowColumn = new ToolStripMenuItem("Show Column");
            cmnuShowColumn.Name = "ShowColumn";
            cmnuShowColumn.Click += new EventHandler(cmnuShowColumn_Click);

            ToolStripMenuItem cmnuFind = new ToolStripMenuItem("Find");
            cmnuFind.Name = "Find";
            cmnuFind.ShortcutKeys = Keys.Control|Keys.F;
            cmnuFind.Click += new EventHandler(cmnuFind_Click);

            ToolStripMenuItem cmnuWhere = new ToolStripMenuItem("Where ???");
            cmnuWhere.Name = "Where";
            cmnuWhere.Click += new EventHandler(cmnuWhere_Click);

            ToolStripMenuItem cmnuWhereEqual = new ToolStripMenuItem("Where (Equal)");
            cmnuWhereEqual.Name = "WhereEqual";
            cmnuWhereEqual.Click += new EventHandler(cmnuWhereEqual_Click);

            ToolStripMenuItem cmnuWhereNotEqual = new ToolStripMenuItem("Where (Not Equal)");
            cmnuWhereNotEqual.Name = "WhereNotEqual";
            cmnuWhereNotEqual.Click += new EventHandler(cmnuWhereNotEqual_Click);

            ToolStripMenuItem cmnuZoomItm = new ToolStripMenuItem("Zoom");
            cmnuZoomItm.Name = "Zoom";
            cmnuZoomItm.Click += new EventHandler(cmnuZoom_Click);

            ToolStripMenuItem cmnuRemoveFilter = new ToolStripMenuItem("Remove Filter");
            cmnuRemoveFilter.Name = "RemoveFilter";
            cmnuRemoveFilter.Click += new EventHandler(cmnuRemoveFilter_Click);

            ToolStripMenuItem cmnuAdd = new ToolStripMenuItem("Add");
            cmnuAdd.Name = "Add";
            cmnuAdd.Click += new EventHandler(cmnuAdd_Click);

            ToolStripMenuItem cmnuEdit = new ToolStripMenuItem("Edit");
            cmnuEdit.Name = "Edit";
            cmnuEdit.Click += new EventHandler(cmnuEdit_Click);

            ToolStripMenuItem cmnuInsertRow = new ToolStripMenuItem("Insert Row");
            cmnuInsertRow.Name = "InsertRow";
            cmnuInsertRow.Click += new EventHandler(cmnuInsertRow_Click);

            ToolStripMenuItem cmnuInsertData = new ToolStripMenuItem("Insert Data");
            cmnuInsertData.Name = "InsertData";
            cmnuInsertData.Click += new EventHandler(cmnuInsertData_Click);

            ToolStripMenuItem cmnuInsertSO = new ToolStripMenuItem("Insert SO");
            cmnuInsertSO.Name = "InsertSO";
            cmnuInsertSO.Click += new EventHandler(cmnuInsertSO_Click);

            ToolStripMenuItem cmnuInsertPO = new ToolStripMenuItem("Insert PO");
            cmnuInsertPO.Name = "InsertPO";
            cmnuInsertPO.Click += new EventHandler(cmnuInsertPO_Click);

            ToolStripMenuItem cmnuInsertPD = new ToolStripMenuItem("Insert PD");
            cmnuInsertPD.Name = "InsertPD";
            cmnuInsertPD.Click += new EventHandler(cmnuInsertPD_Click);

            ToolStripMenuItem cmnuInsertCO = new ToolStripMenuItem("Insert CO");
            cmnuInsertCO.Name = "InsertCO";
            cmnuInsertCO.Click += new EventHandler(cmnuInsertCO_Click);

            ToolStripMenuItem cmnuInsertCS = new ToolStripMenuItem("Insert CS");
            cmnuInsertCS.Name = "InsertCS";
            cmnuInsertCS.Click += new EventHandler(cmnuInsertCS_Click);

            ToolStripMenuItem cmnuPreviousPrice = new ToolStripMenuItem("Previous Price");
            cmnuPreviousPrice.Name = "PreviousPrice";
            cmnuPreviousPrice.Click += new EventHandler(cmnuPreviousPrice_Click);

            ToolStripMenuItem cmnuAlternateItemList = new ToolStripMenuItem("Alternate Item List");
            cmnuAlternateItemList.Name = "AlternateItemList";
            cmnuAlternateItemList.Click += new EventHandler(cmnuAlternateItemList_Click);

            ToolStripMenuItem cmnuDataMatrix = new ToolStripMenuItem("Data Matrix");
            cmnuDataMatrix.Name = "Data Matrix";
            cmnuDataMatrix.Click += new EventHandler(cmnuDataMatrix_Click);

            ToolStripMenuItem cmnuOpenDocument = new ToolStripMenuItem("Open Document");
            cmnuOpenDocument.Name = "Open Document";
            cmnuOpenDocument.Click += new EventHandler(cmnuOpenDocument_Click);

            ToolStripMenuItem cmnuExportGrid = new ToolStripMenuItem("Export All Rows to Excel");
            cmnuExportGrid.Name = "Export";
            cmnuExportGrid.Click+=new EventHandler(cmnuExportGrid_Click);

            ToolStripMenuItem cmnuErr = new ToolStripMenuItem("Menu haven't set!");
            cmnuDataMatrix.Name = "Err";


            int i = cmnuGlobal.Items.Count - 1;
            while (i >= 0)
            {
                cmnuGlobal.Items.RemoveAt(i--);
            }

            switch (contextMenuFlagList.ToUpper().Trim())
            {
                case "ADDEDIT":
                    cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit);
                    break;
                case "APBLITM":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertPO); cmnuGlobal.Items.Add(cmnuInsertPD); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "APBLITMA":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertPO); cmnuGlobal.Items.Add(cmnuInsertPD); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "APPDITM":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertPO); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "APPDITMA":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertPO); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "ARDOITM":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertSO); cmnuGlobal.Items.Add(cmnuInsertCS); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "ARDOITMA":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertSO); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "ARIVITM":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertSO); cmnuGlobal.Items.Add(cmnuInsertCS); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "ARIVITMA":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertSO); cmnuGlobal.Items.Add(cmnuInsertCS); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "CSCPDITM":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertCO); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "CSCPDITMA":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuInsertCO); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "GRD000A":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit);
                    break;
                case "GRDR000":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertRow);
                    break;
                case "GRDR00A":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow);
                    break;
                case "GRDR0MA":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "GRDRD00":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuRemoveFilter); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData);
                    break;
                case "GRDRD0A":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData);
                    break;
                case "GRDRDM0":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "GRDRDMA":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuAdd); cmnuGlobal.Items.Add(cmnuEdit); cmnuGlobal.Items.Add(cmnuInsertRow); cmnuGlobal.Items.Add(cmnuInsertData); cmnuGlobal.Items.Add(cmnuPreviousPrice); cmnuGlobal.Items.Add(cmnuAlternateItemList); cmnuGlobal.Items.Add(cmnuDataMatrix);
                    break;
                case "LISTFILTER":
                    cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuWhere); cmnuGlobal.Items.Add(cmnuWhereEqual); cmnuGlobal.Items.Add(cmnuWhereNotEqual); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuRemoveFilter);
                    break;
                case "LISTFILTERDGN":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuWhere); cmnuGlobal.Items.Add(cmnuWhereEqual); cmnuGlobal.Items.Add(cmnuWhereNotEqual); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuRemoveFilter);
                    break;
                case "LISTSEARCH":
                    cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm);
                    break;
                case "LISTSEARCHDGN":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm);
                    break;
                case "LISTNORMALVIEW":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm);
                    break;
                case "ZOOM":
                    cmnuGlobal.Items.Add(cmnuZoomItm);
                    break;
                case "LISTFINDE":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuOpenDocument);
                    break;
                case "LISTFILTERDGNE":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuWhere); cmnuGlobal.Items.Add(cmnuWhereEqual); cmnuGlobal.Items.Add(cmnuWhereNotEqual); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuRemoveFilter); cmnuGlobal.Items.Add(cmnuOpenDocument);
                    break;
                default:
                    cmnuGlobal.Items.Add(cmnuErr);                   
                    break;
                case "GRDPOCSG":
                    cmnuGlobal.Items.Add(cmnuShowColumn); cmnuGlobal.Items.Add(cmnuHideColumn); cmnuGlobal.Items.Add(cmnuFind); cmnuGlobal.Items.Add(cmnuZoomItm); cmnuGlobal.Items.Add(cmnuInsertCS);
                    break;
            }
            if(ctrl.GetType()==typeof(UltraGrid) ||ctrl.GetType()==typeof(TAGridEditor))
                cmnuGlobal.Items.Add(cmnuExportGrid);
        }
        internal static string ContextMenuSetting_GetNew(int codeKey)
        {
            string formNm = string.Empty;
            try
            {
                //if (codeKey == 0)
                //{
                //For Popup Dialog
                if (Application.OpenForms[Application.OpenForms.Count - 1].Modal || Application.OpenForms[Application.OpenForms.Count - 1].TopLevel)
                {
                    formNm = Application.OpenForms[Application.OpenForms.Count - 1].Name;
                }
                else
                {
                    formNm = frmMain.gfrmMain.MdiChildren[frmMain.gfrmMain.MdiChildren.Length - 1].Name;
                }
                //}
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CodeKey", codeKey));
                parmList.Add(new SqlParameter("@FormNm", formNm));
                DataTable dt = GFunc.ExecuteProc("SYSFormSetting_ControlFormatFlag_Get", parmList);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    throw new TAException("ERROR: Control Format Flag Not Set in Database");
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }
        internal static string ContextMenuSetting_GetNew(SqlConnection cn, int codeKey)
        {
            string formNm = string.Empty;
            try
            {
                if (codeKey == 0)
                {
                    //For Popup Dialog
                    if (Application.OpenForms[Application.OpenForms.Count - 1].Modal)
                    {
                        formNm = Application.OpenForms[Application.OpenForms.Count - 1].Name;
                    }
                    else
                    {
                        formNm = frmMain.gfrmMain.MdiChildren[frmMain.gfrmMain.MdiChildren.Length - 1].Name;
                    }
                }
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CodeKey", codeKey));
                parmList.Add(new SqlParameter("@FormNm", formNm));
                DataTable dt = GFunc.ExecuteProc(cn, "SYSFormSetting_ControlFormatFlag_Get", parmList);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    throw new TAException("ERROR: Control Format Flag Not Set in Database");
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }
        public static string ContextMenuSettingGrid_GetNew(string gridNm, string listID)
        {
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@GridNm", gridNm));
                parmList.Add(new SqlParameter("@ListID", listID.Split('%')[0]));
                DataTable dt = GFunc.ExecuteProc("SYSFormSettingDetID_GridFormatFlag_Get", parmList);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    throw new TAException("ERROR: Control Format Flag Not Set in Database");
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }
        public static string ContextMenuSettingGrid_GetNew(SqlConnection cn, string gridNm, string listID)
        {
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@GridNm", gridNm));
                parmList.Add(new SqlParameter("@ListID", listID.Split('%')[0]));
                DataTable dt = GFunc.ExecuteProc(cn, "SYSFormSettingDetID_GridFormatFlag_Get", parmList);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    throw new TAException("ERROR: Control Format Flag Not Set in Database");
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }
        internal static string ContextMenuSetting_GetNew(int codeKey, string formNm)
        {
            try
            {
                List<SqlParameter> parmList = new List<SqlParameter>();
                parmList.Add(new SqlParameter("@CodeKey", codeKey));
                parmList.Add(new SqlParameter("@FormNm", formNm));
                DataTable dt = GFunc.ExecuteProc("SYSFormSetting_ControlFormatFlag_Get", parmList);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    throw new TAException("ERROR: Control Format Flag Not Set in Database");
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        //ContextMenuStrip Click Events
        static void cmnuHideColumn_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGrid grid = null;
                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl.Parent;
                else if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl;

                grid.DisplayLayout.Bands[0].Columns[grid.ActiveCell.Column.Index].Hidden = true;
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuShowColumn_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGrid grid = null;
                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl.Parent;
                else if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl;
                if (grid != null)
                {
                    string formSettingID;
                    if (grid.FindForm().Name.ToUpper() == "FRMINSERTDATA")
                    {
                        TAUtil.TAComboBox cbo = ((TAUtil.TAComboBox)grid.FindForm().Controls.Find("DocCode", true)[0]);
                        formSettingID = "frmInsertDataGet" + GFunc.NEStr(cbo.SelectedRow.Cells["TblNm"].Value, string.Empty).ToString().Replace("_", "") + "Detail";
                    }
                    else
                        //int docCodeKey = ActiveFormDocCodeAll_Get();
                        formSettingID = FormSettingID_Get(grid);

                    frmGrdFormat frmFormat = new frmGrdFormat(grid, formSettingID); frmFormat.ShowDialog();
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuFind_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridColumn searchCol;
                UltraGrid grid = null;
                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl;
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl.Parent;
                if (grid.ActiveCell != null)
                {
                    searchCol = grid.ActiveCell.Column;
                    frmSearchInfo searchForm = new frmSearchInfo();
                    if (GFunc.IsNE(searchCol))
                        searchForm.ShowMe(grid,"","", string.Empty);
                    else
                        searchForm.ShowMe(grid, searchCol.Key, searchCol.Header.Caption, grid.ActiveRow.Cells[searchCol].Text.ToString());
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuWhereEqual_Click(object sender, EventArgs e)
        {
            frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.WaitCursor;

            try
            {
                UltraGrid grid = null;
                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl;
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl.Parent;
                ColumnFiltersCollection columnFilterHDR = grid.DisplayLayout.Bands[0].ColumnFilters;
                if (!GFunc.IsNE(grid.ActiveCell))
                {
                    //columnFilterHDR.ClearAllFilters();
                    if (GFunc.IsNE(grid.ActiveRow.Cells[grid.ActiveCell.Column.Key].Text.ToString()))
                        columnFilterHDR[grid.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.Equals, FilterCondition.BlankCellValue);
                    else
                        columnFilterHDR[grid.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.Equals, grid.ActiveRow.Cells[grid.ActiveCell.Column.Key].Text.ToString());
                    if (grid.Rows.FirstVisibleCardRow != null)
                    {
                        // ugrdConList.Rows.FirstVisibleCardRow.Selected = true;
                        grid.ActiveRow = grid.Rows.FirstVisibleCardRow;
                        grid.ActiveRow.Selected = true;
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            finally
            {
                frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.Default;
            }
        }
        static void cmnuWhereNotEqual_Click(object sender, EventArgs e)
        {
            frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.WaitCursor;

            try
            {
                UltraGrid grid = null;
                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl;
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl.Parent;
                ColumnFiltersCollection columnFilterHDR = grid.DisplayLayout.Bands[0].ColumnFilters;
                if (!GFunc.IsNE(grid.ActiveCell))
                {
                    //columnFilterHDR.ClearAllFilters();
                    if (GFunc.IsNE(grid.ActiveRow.Cells[grid.ActiveCell.Column.Key].Text.ToString()))
                        columnFilterHDR[grid.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.Equals, FilterCondition.NonBlankCellValue);
                    else
                        columnFilterHDR[grid.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.NotEquals, grid.ActiveRow.Cells[grid.ActiveCell.Column.Key].Text.ToString());
                    if (grid.Rows.FirstVisibleCardRow != null)
                    {
                        // ugrdConList.Rows.FirstVisibleCardRow.Selected = true;
                        grid.ActiveRow = grid.Rows.FirstVisibleCardRow;
                        grid.ActiveRow.Selected = true;
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            finally
            {
                frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.Default;
            }
        }
        static void cmnuZoom_Click(object sender, EventArgs e)
        {
            frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.WaitCursor;
            try
            {
                UltraGrid grid = null;
                TAUtil.TATextBoxEditor txt = null;
                frmZoom fZoom = null;

                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl;
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl.Parent;
                else if (activeControl.GetType() == typeof(TAUtil.TATextBoxEditor))
                    txt = (TAUtil.TATextBoxEditor)activeControl;
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                    txt = (TAUtil.TATextBoxEditor)activeControl.Parent;

                if (grid != null)
                {
                    if (!GFunc.IsNE(grid.ActiveRow) && !GFunc.IsNE(grid.ActiveCell))
                    {
                        if (grid.ActiveCell.Column.EditorComponent != null)
                        {
                            if (grid.ActiveCell.Column.EditorComponent.GetType() != typeof(TAUtil.TANumericEditor))
                            {
                                fZoom = new frmZoom(grid.ActiveRow.Cells[grid.ActiveCell.Column].Text, grid.ActiveCell.Column.CellMultiLine == DefaultableBoolean.True ? true : false);
                                fZoom.ShowDialog();

                                if (fZoom.DialogResult == DialogResult.OK)
                                    grid.ActiveRow.Cells[grid.ActiveCell.Column].Value = fZoom.ZoomText;
                            }

                        }
                        else
                        {
                            fZoom = new frmZoom(grid.ActiveRow.Cells[grid.ActiveCell.Column].Text, grid.ActiveCell.Column.CellMultiLine == DefaultableBoolean.True ? true : false);
                            fZoom.ShowDialog();

                            if (fZoom.DialogResult == DialogResult.OK)
                                grid.ActiveRow.Cells[grid.ActiveCell.Column].Value = fZoom.ZoomText;
                        }

                    }
                }
                else if (txt != null)
                {
                    fZoom = new frmZoom(txt.Text, txt.Multiline);
                    fZoom.ShowDialog();
                    if (fZoom.DialogResult == DialogResult.OK)
                        grid.ActiveRow.Cells[grid.ActiveCell.Column].Value = fZoom.ZoomText;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            finally
            {
                frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.Default;
            }
        }
        static void cmnuRemoveFilter_Click(object sender, EventArgs e)
        {
            frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.WaitCursor;

            try
            {
                Control grd = GetActiveControlAtActiveForm();
                if (grd.GetType() != typeof(TAUtil.TAGridEditor))
                    if (grd.Parent != null && grd.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                        grd = grd.Parent;

                frmMain.gfrmMain.ActiveMdiChild.Validate();
                if (grd.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    UltraGrid grdList = (UltraGrid)grd;
                    ColumnFiltersCollection columnFilterHDR = grdList.DisplayLayout.Bands[0].ColumnFilters;

                    if (!GFunc.IsNE(columnFilterHDR))
                    {
                        columnFilterHDR.ClearAllFilters();

                        if (grdList.Rows.FirstVisibleCardRow != null)
                        {
                            grdList.ActiveRow = grdList.Rows.FirstVisibleCardRow;
                            grdList.ActiveRow.Selected = true;
                        }
                    }
                    grdList.DisplayLayout.Bands[0].SortedColumns.Clear();
                    grdList.DataBind();
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            finally
            {
                frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.Default;
            }
        }
        static void cmnuAdd_Click(object sender, EventArgs e)
        {
            try
            {


                TAUtil.TAComboBox cboControl = null;

                Control activeControl = GetActiveControlAtActiveForm();


                string activeCrlText = activeControl.Parent.Text;//For string accepted constructor
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();


                string popupFormParameter = "";
                string formName = cmnuGlobal.AccessibleDescription;

                if (cmnuGlobal.AccessibleDescription.IndexOf("%") != -1)
                {
                    formName = cmnuGlobal.AccessibleDescription.Split('%')[0];
                    popupFormParameter = cmnuGlobal.AccessibleDescription.Split('%')[1];
                }

                //get control obj and control value
                if (activeControl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                {
                    cboControl = (TAUtil.TAComboBox)activeControl.Parent;
                    activeCrlText = cboControl.Text;
                }
                else if (activeControl.GetType() == typeof(TAUtil.TAComboBox))
                {
                    cboControl = (TAUtil.TAComboBox)activeControl;
                    activeCrlText = cboControl.Text;
                }
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    if (((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell != null)
                    {

                        if (!GFunc.IsNE(((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.EditorComponentResolved))
                        {
                            cboControl = (TAUtil.TAComboBox)((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.EditorComponentResolved;
                            activeCrlText = cboControl.Text;
                        }
                        else
                            activeCrlText = ((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.Text;

                    }
                }
                else if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    if (((TAUtil.TAGridEditor)activeControl).ActiveCell != null)
                    {

                        if (!GFunc.IsNE(((TAUtil.TAGridEditor)activeControl).ActiveCell.EditorComponentResolved))
                        {
                            cboControl = (TAUtil.TAComboBox)((TAUtil.TAGridEditor)activeControl).ActiveCell.EditorComponentResolved;
                            activeCrlText = cboControl.Text;
                        }
                        else
                            activeCrlText = ((TAUtil.TAGridEditor)activeControl).ActiveCell.Text;
                    }
                }

                //Get Form to Open
                Form objectForm = null;
                switch (formName.ToLower())
                {
                    case "frmrefcat":
                        //Limitation: This Code needs the control name be followed by 1,2,3,4,5 bcos it use the last char of control to pass them to Category Form as constructor param
                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText, Convert.ToInt16(cboControl.Name.Substring(cboControl.Name.Length - 1)) }, null, null);
                        break;
                    case "frmmstcon":
                        GEnum.SystemCode _conType = GEnum.SystemCode.Customer;
                        //to split Customer or Vendor ( C or V) in Popupform value that set in cmnuGlobal.AccessibleDescription
                        string ConType = popupFormParameter;
                        if (GFunc.CompareString(ConType, "C"))
                            _conType = GEnum.SystemCode.Customer;
                        else
                            _conType = GEnum.SystemCode.Vendor;

                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText, _conType }, null, null);
                        break;

                    case "frmmstitm":
                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText, false }, null, null);
                        break;

                    case "frmsysmsglisttext":
                        //to split Country or region ( 40 or 90) in Popupform value that set in cmnuGlobal.AccessibleDescription
                        int MsgListTextDataGrp = GFunc.NEInt(popupFormParameter, 40);
                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { MsgListTextDataGrp, activeCrlText, GEnum.formInitMode.Add }, null, null);
                        break;
                    default:
                        if (formName != string.Empty)
                            objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText }, null, null);
                        else
                        {
                            MsgBox.Show("Please set the popup form name in form setting of the control");
                            return;
                        }
                        break;
                }

                //Close the Form if it is open as we do not allow multi-instance for ref and master form (not sure if this function is use for document, if so we will need to amend this part)
                if (objectForm != null)
                {
                    Form currentForm = frmMain.gfrmMain.ExistingForm(objectForm.Name);
                    if (GFunc.IsNE(currentForm) == false)
                        currentForm.Close();

                    //Open the Form as dialog

                    objectForm.ShowDialog();
                }

                //Update the DataSource Again
                if (cboControl != null)
                    BindComboValue(cboControl, cmnuGlobal.Tag.ToString());
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuEdit_Click(object sender, EventArgs e)
        {
            try
            {
                TAUtil.TAComboBox cboControl = null;
                int activeCrlkey = 0;//For int accepted constructor
                string activeCrlText = string.Empty;
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                Control activeControl = GetActiveControlAtActiveForm();
                //Control activeControl = ((Infragistics.Win.EmbeddableTextBoxWithUIPermissions)((System.Windows.Forms.ContextMenuStrip)(((System.Windows.Forms.ToolStripMenuItem)(sender)).Owner)).SourceControl).Parent;

                string popupFormParameter = "";
                string formName = cmnuGlobal.AccessibleDescription;
                string listSettingID = cmnuGlobal.Tag.ToString();
                //to split Customer or Vendor ( C or V) in Popupform value that set in cmnuGlobal.AccessibleDescription
                if (cmnuGlobal.AccessibleDescription.IndexOf("%") != -1)
                {
                    formName = cmnuGlobal.AccessibleDescription.Split('%')[0];
                    popupFormParameter = cmnuGlobal.AccessibleDescription.Split('%')[1];
                }

                //get control obj and control value
                if (activeControl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                {
                    cboControl = (TAUtil.TAComboBox)activeControl.Parent;
                    if (cboControl.DisplayLayout.Bands[0].Columns[cboControl.ValueMember].DataType == typeof(int))
                        if (GFunc.IsNE(cboControl.Value) == false)
                            int.TryParse(cboControl.Value.ToString(), out activeCrlkey);

                        //else
                            activeCrlText = GFunc.NEStr(cboControl.Value, "");
                }
                else if (activeControl.GetType() == typeof(TAUtil.TAComboBox))
                {
                    cboControl = (TAUtil.TAComboBox)activeControl;
                    if (cboControl.DisplayLayout.Bands[0].Columns[cboControl.ValueMember].DataType == typeof(int))
                        if (GFunc.IsNE(cboControl.Value) == false)
                            int.TryParse(cboControl.Value.ToString(), out activeCrlkey);
                        
                        activeCrlText = GFunc.NEStr(cboControl.Value, "");
                }
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    if (((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell != null)
                    {

                        if (!GFunc.IsNE(((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.EditorComponentResolved))
                        {
                            cboControl = (TAUtil.TAComboBox)((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.EditorComponentResolved;
                            if (GFunc.IsNE(cboControl.Value) == false)
                                int.TryParse(cboControl.Value.ToString(), out activeCrlkey);                            
                        }
                    
                       activeCrlText = ((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.Text;

                    }
                }
                else if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    if (((TAUtil.TAGridEditor)activeControl).ActiveCell != null)
                    {
                        if (!GFunc.IsNE(((TAUtil.TAGridEditor)activeControl).ActiveCell.EditorComponentResolved))
                        {
                            cboControl = (TAUtil.TAComboBox)((TAUtil.TAGridEditor)activeControl).ActiveCell.EditorComponentResolved;
                            if (GFunc.IsNE(cboControl.Value) == false)
                                int.TryParse(cboControl.Value.ToString(), out activeCrlkey);
                        }
                        //else
                            activeCrlText = ((TAUtil.TAGridEditor)activeControl).ActiveCell.Text;

                    }
                }
                if ((activeCrlkey == null || activeCrlkey == 0) && activeCrlText == string.Empty)
                {
                    MsgBox.Show("Please select a value to edit");
                    return;
                }

                //Get Form to Open
                int codeKey = ActiveFormDocCodeAll_Get();
               
                Form objectForm = null;
                switch (formName.ToLower())
                {
                    case "frmrefcat":
                        //Limitation: This Code needs the control name be followed by 1,2,3,4,5 bcos it use the last char of control to pass them to Category Form as constructor param
                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlkey, Convert.ToInt16(cboControl.Name.Substring(cboControl.Name.Length - 1)) }, null, null);
                        break;

                    case "frmmstcon":
                        GEnum.SystemCode _conType = GEnum.SystemCode.Customer;
                        //to split Customer or Vendor ( C or V) in Popupform value that set in cmnuGlobal.AccessibleDescription
                        string ConType = popupFormParameter;
                        if (GFunc.CompareString(ConType, "C"))
                            _conType = GEnum.SystemCode.Customer;
                        else
                            _conType = GEnum.SystemCode.Vendor;

                        if (GFunc.CompareString(ConType, "C"))
                            _conType = GEnum.SystemCode.Customer;
                        else
                            _conType = GEnum.SystemCode.Vendor;

                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlkey, _conType }, null, null);
                        break;

                    case "frmmstitm":
                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText, true }, null, null);
                        break;
                    case "frmsysmsglisttext":
                        //to split Country or region ( 40 or 90) in Popupform value that set in cmnuGlobal.AccessibleDescription
                        int MsgListTextDataGrp = GFunc.NEInt(popupFormParameter, 40);

                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { MsgListTextDataGrp, activeCrlText, GEnum.formInitMode.Edit }, null, null);
                        break;

                    case "frmmstshipname": //Mic Check -- Jack Modified /Added 9 Nov 12
                        object objFactory = ActiveFormDocument_Get("objFactory");
                        object objDoc=GFunc.GetPropertyValue("Doc",objFactory);
                        int docConKey = GFunc.NEInt(GFunc.GetPropertyValue("DocConKey", objDoc), 0);
                        if (docConKey == 0)
                        {
                            MsgBox.Show("Customer or vendor cannot be empty!");
                            return;
                        }
                        MSTShipName objShipNm = MSTShipName.Get(activeCrlText, docConKey);
                        objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { objShipNm.ShipNameKey }, null, null);
                        break;

                    default:
                        if (activeCrlkey >0)
                            objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlkey }, null, null);
                        else
                            objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText }, null, null);
                        break;
                }

                //Close the Form if it is open as we do not allow multi-instance for ref and master form (not sure if this function is use for document, if so we will need to amend this part)
                if (objectForm != null)
                {
                    Form currentForm = frmMain.gfrmMain.ExistingForm(objectForm.Name);
                    if (GFunc.IsNE(currentForm) == false)
                        currentForm.Close();

                    //Open the Form as dialog                 
                    objectForm.ShowDialog();
                }

                //Update the DataSource Again
                if (cboControl != null)
                {
                    if (activeCrlkey > 0 || cboControl.LimitToList == false)
                    {
                        //BindComboValue(cboControl, cmnuGlobal.Tag.ToString(), cboControl.DisplayMember, cboControl.ValueMember, codeKey);
                        //cannot use static cmnuGlobal.Tag, if user click Edit in one of Combo Boxes in the popup form(third level edit), it will be overriden
                        BindComboValue(cboControl, listSettingID, cboControl.DisplayMember, cboControl.ValueMember, codeKey);

                    }
                }

            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuInsertRow_Click(object sender, EventArgs e)
        {
            Control control = GetActiveControlAtActiveForm();
            Form Activefrm = control.FindForm();

            try
            {

                bool isDocument = false;
                UltraGrid grid = null;
                int docCodeKey = ActiveFormDocCodeAll_Get();


                if (control.GetType() == typeof(TAUtil.TAGridEditor) || control.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    if (docCodeKey == 0 && control.FindForm().Name.ToLower() == "frmmstjob")
                    {
                        object objFactory = ActiveFormDocument_Get("objMSTJobFactory");
                        docCodeKey = (int)GEnum.SystemCode.Job;
                        if (((MSTJobFactory)objFactory).IsReadOnly == true)
                            return;
                    }
                    else if (docCodeKey == 0 && control.FindForm().Name.ToLower() == "frmmstitm")
                    {
                        object objFactory = ActiveFormDocument_Get("objMstItmFactory");
                        docCodeKey = (int)GEnum.SystemCode.Inventory;
                        if (((MSTItmFactory)objFactory).IsReadOnly == true)
                            return;
                    }
                    else
                    {
                        Document objDoc = ActiveFormDocument_Get();
                        if (objDoc.IsReadOnly == true)
                            return;
                    }
                }
                
                FreezeControl(Activefrm.Handle, false);

                if (control.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    grid = (UltraGrid)control.Parent;
                }
                else if (control.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    grid = (UltraGrid)control;
                }
                int Index = grid.ActiveRow.Index;
                switch (docCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Works_Order:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Bank_Revaluation:
                        isDocument = true;
                        break;
                    default:
                        isDocument = false;
                        break;
                }
                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                    {
                        if (grid.ActiveRow.Update() == false)
                            return;
                    }
                }
                if (isDocument)
                {
                    AddRowForDocument(grid);
                }
                else
                {
                    AddRowForMasterRef((int)docCodeKey, grid);
                    
                }

                if (grid.ActiveRow != null)
                    grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.Rows[Index]);

                if (grid.Name.Equals("tagrdMSTJobDetEst"))
                {
                    ((DataTable)grid.DataSource).DefaultView.Sort = "EstSN";
                }


            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            finally
            {
                FreezeControl(Activefrm.Handle, true);
                Activefrm.Refresh(); //previous code use  control.FindForm(). It raised error when Insert Row in MST_ItemAssembly
            }

        }//Completed
        static void cmnuInsertData_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                    default:
                        return;
                }
                #endregion

                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                    {
                        if (grid.ActiveRow.Update() == false)
                            return;
                    }
                }
                frmInsertData frmDocSelection = new frmInsertData(objDoc, grid);
                frmDocSelection.ShowDialog();

                DocComUtility.CalForm(objDoc, details, true, false);

                if (grid.ActiveRow != null)
                    grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);

                
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuInsertSO_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                }
                #endregion
                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                    {
                        if (grid.ActiveRow.Update() == false)
                            return;
                    }
                }
                frmInsertSO frmInsertSO = new frmInsertSO(objDoc, grid);
                frmInsertSO.ShowDialog();
                //DocDetUtil.UpdateItmAccFromMaster(objDoc, grid);//added by Jane 06-Feb-2025
                DocComUtility.CalForm(objDoc, details, true, false);

                if (grid.ActiveRow != null)
                    grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);
            }//End Try
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }
        static void cmnuInsertPO_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                }
                #endregion
                int VendorKey = GFunc.NEInt((int)GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);
                if (VendorKey > 0)
                {
                    if (grid.ActiveRow != null)
                    {
                        if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                        {
                            if (grid.ActiveRow.Update() == false)
                                return;
                        }
                    }
                    frmInsertPO frmInsePO = new frmInsertPO(objDoc, grid, VendorKey);
                    frmInsePO.StartPosition = FormStartPosition.CenterScreen;
                    frmInsePO.ShowDialog();

                    Hashtable htDetailGrd = new Hashtable();
                    htDetailGrd.Add(GEnum.Details.Doc_Itm, grid);
                    DocComUtility.CalForm(objDoc, htDetailGrd, true, false);
                    if (grid.ActiveRow != null)
                        grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);
                }
                else
                {
                    MsgBox.Show("Vendor Name is empty.  Please choose a vendor");
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuInsertPD_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                }
                #endregion

                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                    {
                        if (grid.ActiveRow.Update() == false)
                            return;
                    }
                }
                frmInsertPD frmDocSelection = new frmInsertPD(objDoc, grid);
                frmDocSelection.ShowDialog();
                DocComUtility.CalForm(objDoc, details, true, false);
                if (grid.ActiveRow != null)
                    grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }
        static void cmnuInsertCO_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                }
                #endregion

                int VendorKey = GFunc.NEInt((int)GFunc.GetIntPropertyValue("DocConKey", objDoc), 0);
                if (VendorKey <= 0)
                {
                    MsgBox.Show(MsgID.Common.CannotBeEmpty + "%Document VendorKey", GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                    return;
                }
                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                    {
                        if (grid.ActiveRow.Update() == false)
                            return;
                    }
                }
                frmInsertCPO frmInsertCPO = new frmInsertCPO(objDoc, grid);
                frmInsertCPO.ShowDialog();

                DocComUtility.CalForm(objDoc, details, true, false);

                if (grid.ActiveRow != null)
                    grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);
            }//End Try
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuInsertCS_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                bool POCsgItems = false;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (TAGridEditor)activeControl;
                else if(activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (TAGridEditor)activeControl.Parent;

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:                
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:                        
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;
                    case (int)GEnum.SystemCode.Purchase_Order:
                        if (grid.Name.Equals("tagrdCsgItems"))                        
                            POCsgItems = true; 
                        else
                            DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;
                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                }
                #endregion
                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                    {
                        if (grid.ActiveRow.Update() == false)
                            return;
                    }
                }
                frmInsertCSI frmInsertCS = new frmInsertCSI(objDoc, grid);
                frmInsertCS.ShowDialog();
                DocComUtility.CalForm(objDoc, details, true, false,POCsgItems);

                if (grid.ActiveRow != null)
                    grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);
            }//End Try
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuPreviousPrice_Click(object sender, EventArgs e)
        {
            string searchType = "";
            Document objDoc;
            Hashtable details;
            TAUtil.TAGridEditor grid = new TAGridEditor();
            Form f = frmMain.gfrmMain.ActiveMdiChild;
            ((DocInterface)f).GetDocInfor(out objDoc, out details);

            if (objDoc.IsReadOnly)
                return;

            #region check Detail DataTable
            switch (objDoc.DocCodeKey)
            {
               
                case (int)GEnum.SystemCode.Purchase_Order:
                case (int)GEnum.SystemCode.Purchase_Shipment:
                case (int)GEnum.SystemCode.Purchase_Delivery:
                case (int)GEnum.SystemCode.Purchase_Invoice:
                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                case (int)GEnum.SystemCode.Order_Consignment:
                case (int)GEnum.SystemCode.Received_Consignment:
                case (int)GEnum.SystemCode.Consignment_Settlement:                 
                    DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                    searchType = "AP";
                    break;
                case (int)GEnum.SystemCode.Quotation:
                case (int)GEnum.SystemCode.Sales_Order:
                case (int)GEnum.SystemCode.Reserve_Order:
                case (int)GEnum.SystemCode.Delivery_Order:
                case (int)GEnum.SystemCode.Sales_Invoice:
                case (int)GEnum.SystemCode.Sales_Debit_Note:
                case (int)GEnum.SystemCode.Sales_Credit_Note:
                case (int)GEnum.SystemCode.Cash_Sale:
                case (int)GEnum.SystemCode.Cash_Debit_Note:
                case (int)GEnum.SystemCode.Cash_Credit_Note:
                case (int)GEnum.SystemCode.Contra:
                case (int)GEnum.SystemCode.Cash_Contra:
                case (int)GEnum.SystemCode.Issue_Consignment:
                case (int)GEnum.SystemCode.Return_Consignment:
                case (int)GEnum.SystemCode.Packing_List:
                    DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                    searchType = "AR";
                    break;              
                default:
                    return;
            }
            
            #endregion

            int? ConKey = GFunc.GetIntPropertyValue("DocConKey", objDoc);
            int ItmKey = 0;

            if (grid.ActiveRow != null)
            {
                if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                {
                    ItmKey=GFunc.NEInt(grid.ActiveRow.Cells["ItmKey"].Value,0);
                }
            }
            if (searchType == "AP")
                if (SECPermUtility.Perform("ItemViewCost", false) == false)
                    return;

            frmItmPrevPrice fPrev = new frmItmPrevPrice(ItmKey,(int)ConKey,searchType);
            fPrev.ShowDialog();            
        }
        static void cmnuAlternateItemList_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                }
                #endregion

                if (grid.ActiveRow != null)
                {
                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                    {
                        if (grid.ActiveRow.Update() == false)
                            return;
                    }
                }
                frmInsertAlternateItem frmDocSelection = new frmInsertAlternateItem(objDoc, grid);
                frmDocSelection.ShowDialog();
                if (grid.ActiveRow != null)
                    grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        static void cmnuDataMatrix_Click(object sender, EventArgs e)
        {
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                if (objDoc.IsReadOnly)
                    return;

                #region check Detail DataTable
                switch (objDoc.DocCodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Request:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Reserve_Order:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Purchase_Shipment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Inventory_Adjustment:
                    case (int)GEnum.SystemCode.Inventory_Production:
                    case (int)GEnum.SystemCode.Inventory_Transfer:
                    case (int)GEnum.SystemCode.Journal:
                    case (int)GEnum.SystemCode.Deposit:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Packing_List:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Itm, details, ref grid);
                        break;

                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.Payment_Issue:
                        DocComUtility.DocDetail_Get(GEnum.Details.Doc_Exp, details, ref grid);
                        break;
                }
                #endregion

                frmInsertDataMatrix frmInsertDataMatrix = new frmInsertDataMatrix(objDoc, grid);
                frmInsertDataMatrix.ShowDialog();
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }
        static void cmnuWhere_Click(object sender, EventArgs e)
        {
            frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.WaitCursor;

            try
            {
                UltraGrid grdList = null;
                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grdList = (UltraGrid)activeControl;
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grdList = (UltraGrid)activeControl.Parent;

                ColumnFiltersCollection columnFilterHDR = grdList.DisplayLayout.Bands[0].ColumnFilters;

                if (!GFunc.IsNE(grdList.ActiveCell))
                {
                    frmFilter filter = new frmFilter();
                    if (filter.ShowDialog() == DialogResult.OK)
                    {
                        //columnFilterHDR.ClearAllFilters();

                        switch (filter.SelectedOperator)//0- >;1- >=;2- <;3- <=;4 -Between
                        {
                            case 1:
                                columnFilterHDR[grdList.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.GreaterThan, filter.Val1);
                                break;
                            case 2:
                                columnFilterHDR[grdList.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.GreaterThanOrEqualTo, filter.Val1);
                                break;
                            case 3:
                                columnFilterHDR[grdList.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.LessThan, filter.Val1);
                                break;
                            case 4:
                                columnFilterHDR[grdList.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.LessThanOrEqualTo, filter.Val1);
                                break;
                            case 5:
                                if(GFunc.IsNE(filter.Val1)==false)
                                    columnFilterHDR[grdList.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.GreaterThanOrEqualTo,filter.Val1);
                                columnFilterHDR[grdList.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.LessThanOrEqualTo, filter.Val2);
                                break;
                            case 14:
                                columnFilterHDR[grdList.ActiveCell.Column.Key].FilterConditions.Add(FilterComparisionOperator.Contains, filter.Val1);
                                break;

                        }
                    }

                    if (grdList.Rows.FirstVisibleCardRow != null)
                    {
                        grdList.ActiveRow = grdList.Rows.FirstVisibleCardRow;
                        grdList.ActiveRow.Selected = true;
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
            finally
            {
                frmMain.gfrmMain.ActiveMdiChild.Cursor = Cursors.Default;
            }
        }
        static void cmnuOpenDocument_Click(object sender, EventArgs e)
        {
            try
            {
                Control activeControl = ((ContextMenuStrip)(((ToolStripDropDownItem)(sender)).Owner)).SourceControl;
                int CodeKey = 0, DocKey = 0;
                string docID = string.Empty;

                TAUtil.TAGridEditor grd = null;

                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    grd = (TAUtil.TAGridEditor)activeControl;
                }
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                {
                    grd = (TAUtil.TAGridEditor)activeControl.Parent;
                }
                if (grd == null)
                    return;
                //Mic Check; Jack Added ; On list form Open Doc menu will perform case statements; When use from Item sale Report Default case will be fired
                
                docID = GFunc.NEStr(grd.ActiveCell.Value, string.Empty);
                if (docID == string.Empty) return;
                switch (grd.ActiveCell.Column.Key)
                {
                    case "DocQONum":
                        DocKey = GFunc.DocKey_Get((int)GEnum.SystemCode.Quotation, docID);
                        if (DocKey == 0)
                        {
                            MsgBox.Show("Document ID doesn't exist");
                            return;
                        }
                        frmARQO ARQO = new frmARQO(GEnum.SystemCode.Quotation);
                        ARQO.Show();
                        ARQO.MdiParent = frmMain.gfrmMain;
                        ARQO.OnDocList_OpenRecord(DocKey);
                        ARQO.Activate();
                        break;
                    case "DocSONum":
                        DocKey = GFunc.DocKey_Get((int)GEnum.SystemCode.Sales_Order, docID);
                        if (DocKey == 0)
                        {
                            MsgBox.Show("Document ID doesn't exist");
                            return;
                        }
                        frmARSO ARSO = new frmARSO(GEnum.SystemCode.Sales_Order);
                        ARSO.Show();
                        ARSO.MdiParent = frmMain.gfrmMain;
                        ARSO.OnDocList_OpenRecord(DocKey);
                        ARSO.Activate();
                        break;
                  
                                        

                    default:
                        if (grd.DisplayLayout.Bands[0].Columns.Exists("DocDC") || grd.DisplayLayout.Bands[0].Columns.Exists("DocCodeKey"))
                        {
                            if (grd.DisplayLayout.Bands[0].Columns.Exists("DocDC"))
                                CodeKey = GFunc.NEInt(grd.ActiveCell.Row.Cells["DocDC"].Value, 0);
                            if (grd.DisplayLayout.Bands[0].Columns.Exists("DocCodeKey"))
                                CodeKey = GFunc.NEInt(grd.ActiveCell.Row.Cells["DocCodeKey"].Value, 0);

                            DocKey = GFunc.NEInt(grd.ActiveCell.Row.Cells["DocKey"].Value, 0);
                        }
                        else if (grd.DisplayLayout.Bands[0].Columns.Exists("AccKey"))
                        {
                            CodeKey = (int)GEnum.SystemCode.Account;
                            DocKey = GFunc.NEInt(grd.ActiveCell.Row.Cells["AccKey"].Value, 0);
                        }
                        else
                            return;
                        if (DocKey == 0)
                        {
                            MsgBox.Show("Document ID doesn't exist");
                            return;
                        }
                        OpenDocument(CodeKey, DocKey);
                        break;
                }


            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }


        public static void OpenDocument(int CodeKey, int DocKey)
        {
            try
            {
                switch (CodeKey)
                {
                    case (int)GEnum.SystemCode.Quotation:
                        frmARQO ARQO = new frmARQO((GEnum.SystemCode)CodeKey);
                        ARQO.Show();
                        ARQO.MdiParent = frmMain.gfrmMain;
                        ARQO.OnDocList_OpenRecord(DocKey);
                        ARQO.Activate();
                        break;
                   
                    case (int)GEnum.SystemCode.Sales_Order:
                        frmARSO ARSO = new frmARSO((GEnum.SystemCode)CodeKey);
                        ARSO.Show();
                        ARSO.MdiParent = frmMain.gfrmMain;
                        ARSO.OnDocList_OpenRecord(DocKey);
                        ARSO.Activate();
                        break;
                    case (int)GEnum.SystemCode.Reserve_Order:
                        frmARRO ARRO = new frmARRO((GEnum.SystemCode)CodeKey);
                        ARRO.Show();
                        ARRO.MdiParent = frmMain.gfrmMain;
                        ARRO.OnDocList_OpenRecord(DocKey);
                        ARRO.Activate();
                        break;
                    


                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }


        static void cmnuExportGrid_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGrid grid = null;
                Control activeControl = GetActiveControlAtActiveForm();
                if (activeControl.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl;
                else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                    grid = (UltraGrid)activeControl.Parent;

                if (grid.Rows.Count == 0)
                {
                    MsgBox.Show("No data to export.");
                    return;
                }
                else if (grid.Rows.Count == 1 && grid.Rows[0].IsUnmodifiedTemplateAddRow)
                {
                    MsgBox.Show("No data to export.");
                    return;
                }
                UltraGridExcelExporter excelExporter = new UltraGridExcelExporter();               
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + grid.Name + ".xls";

                    int i = 1;
                    while (File.Exists(path) && i <= 10)
                    {
                        try
                        {
                            excelExporter.Export(grid, path,Infragistics.Documents.Excel.WorkbookFormat.Excel97To2003);
                            break;
                        }
                        catch (Exception ex)
                        {
                            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + grid.Name + i + ".xls";
                            i++;
                        }                        
                    }

                    if(i<=10)
                        excelExporter.Export(grid, path, Infragistics.Documents.Excel.WorkbookFormat.Excel97To2003);

                    // Check Option Value is True (or) False
                    if (SysOptionUtility.GetBool(GVar.SystemOption.SystemWarning.QuestionOpenExportFile))
                        System.Diagnostics.Process.Start(path);

                }
                catch (Exception ex)
                {
                    MsgBox.Show("The system cannot export to the specific file. The file is opened." + ex.Message, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                    throw Error(ex, false, false);
                }
                finally
                {
                    excelExporter.Dispose();
                    excelExporter = null;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }
        //Get popup formNm & listSettingID & Menu Display or hide value(contextMenuFlagList) based on particular bindingfieldName or controlName retrieving from  contextmenusetting of active form
        private static void ControlFormatFlag_Set(string bindingFieldNmorControlNm, int option, out string contextMenuFlagList, out string poupFormName, out string listSettingID)
        {
            try
            {
                bindingFieldNmorControlNm = bindingFieldNmorControlNm;//.ToLower();                
                //Optin 0=>Binding Field Name Search
                //Optin 1=>Control Name Search
                object obj = ActiveFormObject_Get("contextmenusetting");
                if(obj==null)
                {
                    ToolStripMenuItem cmnuExportGrid = new ToolStripMenuItem("Export All Rows to Excel");
                    cmnuExportGrid.Name = "Export";
                    cmnuExportGrid.Click += new EventHandler(cmnuExportGrid_Click);
                    cmnuGlobal.Items.Add(cmnuExportGrid);
                    contextMenuFlagList = "";
                    poupFormName = "";
                    listSettingID = "";
                    return;
                }

                string controlSettingList = obj.ToString();//eg;=>"|ApproveDate#NA#0##|ApproveUserKey#NA#0##SECUserSortID|";
                //bindingFieldNmorControlNm = bindingFieldNmorControlNm;
                if (option == 0)
                {
                    char[] charArray = controlSettingList.ToCharArray();
                    // char ss = charArray[9703];
                    Array.Reverse(charArray);

                    string controlSettingListReverse = new string(charArray);
                    int foundIndex = controlSettingList.ToLower().IndexOf('|' + bindingFieldNmorControlNm.ToLower() + "#na#" + bindingFieldNmorControlNm.ToLower());

                    if (foundIndex == -1)
                    {//find for no BindingFieldNm
                        foundIndex = controlSettingList.ToLower().IndexOf("|0bf#na#" + bindingFieldNmorControlNm.ToLower());
                        if (foundIndex == -1)
                        {
                            MsgBox.Show("ERROR: Menu has not yet set to control=" + bindingFieldNmorControlNm); contextMenuFlagList = poupFormName = listSettingID = string.Empty;
                            //Although Menu string for this control can't be found, we may continue the loading process, so we don't throw Error here
                            return;
                        }
                    }
                    int endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('|'+bindingFieldNmorControlNm) ;Remark=> extra separator='|'
                    foundIndex = controlSettingList.Length - foundIndex;
                    int startIndex = controlSettingList.Length - controlSettingListReverse.IndexOf('|', foundIndex);

                    string st = controlSettingList.Substring(startIndex, endIndex - startIndex);
                    bindingFieldNmorControlNm = st.Split('#')[0];
                    contextMenuFlagList = st.Split('#')[3];
                    poupFormName = st.Split('#')[4];
                    listSettingID = st.Split('#')[5];


                }
                //   BindingFieldName#ControlName#ShortCut#PopupFormNm#ListSetting
                else if (option == 1) //eg; ContactType#tagrdREFContact#0000000110000000000##SYSMsgListContactTypeSortID
                {                     //    |GF#tagrdREFContact#1110010000000000000##MstRefContactList|
                    char[] charArray = controlSettingList.ToCharArray();
                    Array.Reverse(charArray);
                    string controlSettingListReverse = new string(charArray);
                    int foundIndex = controlSettingList.ToLower().IndexOf((bindingFieldNmorControlNm.ToLower().Contains('#') ? '|' : '#') + bindingFieldNmorControlNm.ToLower());
                    if (foundIndex < 0)
                    {
                        string grdNm = bindingFieldNmorControlNm;
                        bindingFieldNmorControlNm = "0gf" + grdNm;
                        foundIndex = controlSettingList.ToLower().IndexOf((bindingFieldNmorControlNm.ToLower().Contains('#') ? '|' : '#') + bindingFieldNmorControlNm.ToLower());
                        if (foundIndex < 0)
                        {
                            MsgBox.Show("ERROR: Menu was not set to control=" + bindingFieldNmorControlNm);
                            contextMenuFlagList = poupFormName = listSettingID = string.Empty;
                            //Although Menu string for this control can't be found, we may continue the loading process, so we don't throw Error here
                            return;
                        }
                    }
                    int endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('#'+bindingFieldNmorControlNm) ;Remark extra separator='#'
                    foundIndex = controlSettingList.Length - foundIndex;
                    int startIndex = controlSettingList.Length - controlSettingListReverse.IndexOf('|', foundIndex);
                    string st = controlSettingList.Substring(startIndex, endIndex - startIndex);
                    bindingFieldNmorControlNm = st.Split('#')[1];
                    contextMenuFlagList = st.Split('#')[3];
                    poupFormName = st.Split('#')[4];
                    listSettingID = st.Split('#')[5];
                }
                else
                {
                    throw new TAUtil.TAException("ControlListSetting Not Initialized");
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void AddRowForDocument(UltraGrid grid)
        {
            try
            {

                string sortSNColumn = string.Empty;
                GEnum.Details detailType = 0;

                switch (grid.Name.ToLower())
                {
                    case "tagrddetitms":
                        detailType = GEnum.Details.Doc_Itm;
                        sortSNColumn = "ItmSN ASC";

                        break;
                    case "tagrddetexps":
                        detailType = GEnum.Details.Doc_Exp;
                        sortSNColumn = "ExpSN ASC";

                        break;
                    case "tagrddetpack":
                        detailType = GEnum.Details.Doc_Pack;
                        sortSNColumn = "ItmSN ASC";

                        break;
                    case "tagrddetpackitm":
                        detailType = GEnum.Details.Doc_Itm;
                        sortSNColumn = "DetItmSN ASC";

                        break;
                    case "tagrddetitmvendors":
                        detailType = GEnum.Details.Doc_ItmVendor;
                        sortSNColumn = "VendorKey ASC";

                        break;
                    case "tagrddetvendor":
                        detailType = GEnum.Details.Doc_Vendor;
                        sortSNColumn = "DocItmKey ASC";

                        break;
                }


                Document objDoc;
                Hashtable details;
                Form f = grid.FindForm();
                ((DocInterface)f).GetDocInfor(out objDoc, out details);
                DocDetUtil.ItmRow_AddBlankRow(objDoc, details, detailType);
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }
        public static void GridSequenceSort(int? docCodeKey, UltraGrid grid)
        {
            try
            {

                string sortSNColumn = string.Empty;

                switch (grid.Name.ToLower())
                {
                    case "tagrddetitms":
                    case "tagrddetraw":
                    case "tagrddetpacking":
                    case "tagrddetfinished":
                        switch (docCodeKey)
                        {
                            case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                            case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                            case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                                sortSNColumn = "ItmID,ItmPrmDate,ItmLinkDocID,ItmLinkItmSN";
                                break;
                            //return; //No need to sort. Sorting done in store proc
                            case (int)GEnum.SystemCode.Payment_Issue:
                            case (int)GEnum.SystemCode.Consignment_Settlement:
                            case (int)GEnum.SystemCode.Contra:
                            case (int)GEnum.SystemCode.Cash_Contra:
                                sortSNColumn = "DocItmKey ASC";
                                break;
                            default:
                                sortSNColumn = "ItmSN ASC";
                                break;
                        }
                        break;
                    case "tagrddetexps":
                        sortSNColumn = "ExpSN ASC";
                        break;
                    case "tagrddetpack":
                        sortSNColumn = "ItmSN ASC";
                        break;
                    case "tagrddetpackitm":
                        sortSNColumn = "DetItmSN ASC";
                        break;
                    case "tagrddetitmvendors":
                        sortSNColumn = "VendorKey ASC";
                        break;
                    case "tagrddetvendor":
                        sortSNColumn = "DocItmKey ASC";
                        break;
                    case "tagrdmstjobdetest":
                        sortSNColumn = "EstSN ASC";
                        break;
                    default:
                        MsgBox.Show("ERROR: Sort order has was not set to grid : " + grid.Name);
                        //Although sorting cannot be done, we still can continue process, so we don't throw Error here
                        break;
                }
                (grid.DataSource as DataTable).DefaultView.Sort = sortSNColumn;

            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }
        public static void AddRowForMasterRef(int docCodeKey, UltraGrid grid)
        {
            try
            {
                string sortSNColumn = string.Empty;
                DataTable dt = grid.DataSource as DataTable;

                switch (docCodeKey)
                {

                    case (int)GEnum.SystemCode.Account:
                    case (int)GEnum.SystemCode.Account_Opening_Balance:
                    case (int)GEnum.SystemCode.Branch:
                    case (int)GEnum.SystemCode.Department:
                    case (int)GEnum.SystemCode.Sales_Representative:
                    case (int)GEnum.SystemCode.Budget:
                    case (int)GEnum.SystemCode.Transaction_Group:
                    case (int)GEnum.SystemCode.Customer:
                    case (int)GEnum.SystemCode.Vendor:
                    case (int)GEnum.SystemCode.Price_List:
                    case (int)GEnum.SystemCode.Ship_Name:
                    case (int)GEnum.SystemCode.Inventory_Opening_Balance:
                    case (int)GEnum.SystemCode.Job_Opening_Balance:
                    case (int)GEnum.SystemCode.Job_Timesheet:
                    case (int)GEnum.SystemCode.Machine_List:
                    case (int)GEnum.SystemCode.Cash_Flow:
                        MsgBox.Show("Cannot perform Insert Row function here", GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                        break;
                    case (int)GEnum.SystemCode.Job:
                        if (GFunc.CompareString(grid.Name, "tagrdMSTJobDetEst"))
                        {

                            if (grid.ActiveRow != null)
                            {
                                decimal LastSN = 0;
                                if (grid.ActiveRow.IsAddRow)
                                {
                                    if (dt.Rows.Count == 0)
                                        LastSN = 1;
                                    else
                                        LastSN = dt.AsEnumerable().Max(p => p.Field<decimal>("ItmSN")) + 1;

                                    if (grid.ActiveRow.IsUnmodifiedTemplateAddRow == false)
                                    {
                                        //save the uncommmitted new row before we start to insert the new row
                                        if (grid.ActiveRow.Update() == false)
                                            return;
                                    }
                                }
                                else
                                {
                                    LastSN = GFunc.NEDec(grid.ActiveRow.Cells["EstSN"].Value, 0);
                                }

                               
                                int JobEstKey= DocComUtility.GridAutoID_Get(grid, "JobKey", "JobEstKey");
                                DataRow dr = dt.NewRow();
                                
                                dr["JobEstKey"] = JobEstKey;
                                dr["EstSN"] = LastSN-0.5M;
                                if (SysOptionUtility.DatabaseBranchCode.Equals(DBCode.BHM))
                                {
                                    dr["EstItmKey"] = 3652;//Remark Item
                                    dr["EstItmKeySelect"] = 3652;//Remark Item
                                    dr["EstItmID"] = "R";
                                    dr["EstItmDes"] = "Remark";
                                    dr["Selected"] = false;
                                }

                                dt.Rows.Add(dr);

                                dt.AcceptChanges();
                                sortSNColumn = "EstSN";
                                ReSequenceGridSN(grid, sortSNColumn);

                                UltraGridRow grdRow = grid.Rows.OfType<UltraGridRow>().ToList().Find(r => r.Cells["JobEstKey"].Value.ToString().Equals(JobEstKey.ToString()));
                                grid.ActiveRow = grdRow;
                                return;
                            }

                            
                        }
                        break;
                    case (int)GEnum.SystemCode.Inventory:
                        if (GFunc.CompareString(grid.Name, "tagrdDetAssembly"))
                        {
                            if (grid.ActiveRow != null)
                            {
                                DataRow dr = dt.NewRow();

                                //Grid_MaxSeqInt((TAGridEditor)grid, "JobEstKey");//dt.Rows.Count > 0 ? dt.AsEnumerable().Max(k => k.Field<int>("JobEstKey")) + 1 : 1;
                                DocDetUtil.AutoIncrement(docCodeKey, grid);
                                dr["AssSN"] = GFunc.NEDec(grid.ActiveRow.Cells["AssSN"].Value, 0) - 0.5m;

                                dt.Rows.Add(dr);
                            }
                            dt.AcceptChanges();
                            sortSNColumn = "AssSN";
                        }
                        break;
                }


                ReSequenceGridSN(grid, sortSNColumn);
                grid.UpdateData();                
                grid.Refresh();

            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }

        }

        //Form Readonly & Enable Set Functions
        public static void FreezeControl(IntPtr controlHandler, bool bFreeze)
        {
            SendMessage(controlHandler, WM_SETREDRAW, bFreeze, 0);
        }
        public static void FormReadOnlyClean_Set(Control cc)
        {
            int counter = 0;
            FormReadOnlyClean_Loop(cc, ref counter);
        }
        private static void FormReadOnlyClean_Loop(Control cc, ref int counter)
        {
            //Recusive call
            Boolean RunLocking = false;
            if (cc.HasChildren && counter < 100)
            {
                if (cc.Controls[0].Name == string.Empty && cc.GetType() != typeof(SplitContainer))//SplitContainer panel's Name may be empty and has child controls
                {
                    RunLocking = true;
                }
                else
                {
                    counter++;
                    foreach (Control moreControl in cc.Controls)
                    {
                        FormReadOnlyClean_Loop(moreControl, ref counter);
                    }
                }
            }
            else RunLocking = true;

            if (RunLocking == true)
            {

                switch (cc.GetType().ToString().ToLower())
                {
                    case "infragistics.win.ultrawineditors.ultracheckeditor":
                    case "infragistics.win.ultrawintree.ultratree":
                    case "infragistics.win.ultrawinlistview.ultralistview":
                    case "system.windows.forms.radiobutton":
                    case "infragistics.win.ultrawineditors.ultraoptionset":
                    case "system.windows.forms.checkbox":
                    case "tautil.tacheckboxeditor":
                        cc.Enabled = true;
                        break;
                    case "infragistics.win.ultrawineditors.ultranumericeditor":
                        Infragistics.Win.UltraWinEditors.UltraNumericEditor ulnue = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)cc;
                        ulnue.ReadOnly = false;
                        break;
                    case "system.windows.forms.numericupdown":
                        System.Windows.Forms.NumericUpDown nud = (System.Windows.Forms.NumericUpDown)cc;
                        nud.ReadOnly = false;
                        break;
                    case "system.windows.forms.richtextbox":
                        System.Windows.Forms.RichTextBox embRicTxt = (System.Windows.Forms.RichTextBox)cc;
                        embRicTxt.ReadOnly = false;
                        break;
                    case "infragistics.win.formattedlinklabel.ultraformattedtexteditor":
                        Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor uftxt = (Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor)cc;
                        uftxt.ReadOnly = false;
                        break;
                    case "infragistics.win.ultrawinschedule.ultracalendarcombo":
                        Infragistics.Win.UltraWinSchedule.UltraCalendarCombo ulcbo = (Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)cc;
                        ulcbo.ReadOnly = false;
                        break;
                    case "tautil.tatextboxeditor":
                    case "tautil.tanumericeditor":
                    case "tautil.tacomboboxeditor":
                    case "infragistics.win.ultrawineditors.ultracomboeditor":
                    case "infragistics.win.ultrawineditors.ultratexteditor":
                        TextEditorControlBase txtEdtrCtrlBase = (TextEditorControlBase)cc;
                        txtEdtrCtrlBase.ReadOnly = false;
                        if (txtEdtrCtrlBase.ButtonsRight.Count > 0)
                            txtEdtrCtrlBase.ButtonsRight[0].Enabled = true;
                        break;
                    case "tautil.tacombobox":
                        TAUtil.TAComboBox taCbo = (TAUtil.TAComboBox)cc;
                        taCbo.ReadOnly = false;
                        if (taCbo.ButtonsRight.Count > 0)
                            taCbo.ButtonsRight[0].Enabled = true;
                        break;
                    case "tautil.tadateeditor":
                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)cc;
                        taDate.ReadOnly = false;
                        if (taDate.ButtonsRight.Count > 0)
                            taDate.ButtonsRight[0].Enabled = true;
                        break;
                    case "infragistics.win.ultrawinmaskededit.ultramaskededit":
                        Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit mask = (Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit)cc;
                        mask.ReadOnly = false;
                        break;
                    case "system.windows.forms.button":
                        cc.Enabled = true;
                        break;
                    case "tautil.tagrideditor":
                        TAUtil.TAGridEditor taGrid = (TAUtil.TAGridEditor)cc;                      
                        taGrid.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                        taGrid.DisplayLayout.Override.AllowDelete = DefaultableBoolean.True;
                        //foreach (UltraGridColumn gcol in taGrid.DisplayLayout.Bands[0].Columns)
                        //{
                        //    gcol.CellActivation = Activation.AllowEdit;                           
                        //}
                        break;
                    case "infragistics.win.ultrawingrid.ultragrid":
                    case "system.windows.forms.toolstrip":
                    case "infragistics.win.ultrawintabcontrol.ultratab":
                        //Ignore all button, tab control, grid and TextControl for Opening document
                        break;
                    default:
                        break;
                }
            }
        }
        public static void FormReadOnly_Set(Control cc)
        {
            int counter = 0;
            FormReadOnly_Loop(cc, ref counter);
        }
        private static void FormReadOnly_Loop(Control cc, ref int counter)
        {   //Recusive call
            Boolean RunLocking = false;
            if (cc.HasChildren && counter < 100)
            {
                if (cc.Controls[0].Name == string.Empty)
                {
                    RunLocking = true;
                }
                else
                {
                    counter++;
                    foreach (Control moreControl in cc.Controls)
                    {

                        FormReadOnly_Loop(moreControl, ref counter);
                    }
                }
            }
            else RunLocking = true;

            if (RunLocking == true)
            {
                switch (cc.GetType().ToString().ToLower())
                {
                    case "tautil.tacheckboxeditor":
                    case "infragistics.win.ultrawineditors.ultracheckeditor":
                    case "infragistics.win.ultrawintree.ultratree":
                    case "infragistics.win.ultrawinlistview.ultralistview":
                    case "system.windows.forms.radiobutton":
                    case "infragistics.win.ultrawineditors.ultraoptionset":
                    case "system.windows.forms.checkbox":
                        cc.Enabled = false;
                        break;
                    case "infragistics.win.ultrawineditors.ultranumericeditor":
                        Infragistics.Win.UltraWinEditors.UltraNumericEditor ulnue = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)cc;
                        ulnue.ReadOnly = true;
                        break;
                    case "system.windows.forms.numericupdown":
                        System.Windows.Forms.NumericUpDown nud = (System.Windows.Forms.NumericUpDown)cc;
                        nud.ReadOnly = true;
                        break;
                    case "system.windows.forms.richtextbox":
                        System.Windows.Forms.RichTextBox embRicTxt = (System.Windows.Forms.RichTextBox)cc;
                        embRicTxt.ReadOnly = true;
                        break;
                    case "infragistics.win.formattedlinklabel.ultraformattedtexteditor":
                        Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor uftxt = (Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor)cc;
                        uftxt.ReadOnly = true;
                        break;
                    case "infragistics.win.ultrawinschedule.ultracalendarcombo":
                        Infragistics.Win.UltraWinSchedule.UltraCalendarCombo ulcbo = (Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)cc;
                        ulcbo.ReadOnly = true;
                        break;

                    case "tautil.tatextboxeditor":
                    case "tautil.tanumericeditor":
                    case "tautil.tacomboboxeditor":
                    case "infragistics.win.ultrawineditors.ultracomboeditor":
                    case "infragistics.win.ultrawineditors.ultratexteditor":
                        TextEditorControlBase txtEdtrCtrlBase = (TextEditorControlBase)cc;
                        string name = txtEdtrCtrlBase.Name;
                        if (name.ToLower() != "openid")
                        {
                            txtEdtrCtrlBase.ReadOnly = true;
                            if (txtEdtrCtrlBase.ButtonsRight.Count > 0)
                                txtEdtrCtrlBase.ButtonsRight[0].Enabled = false;
                        }
                        break;
                    case "infragistics.win.embeddabletextboxwithuipermissions":
                        TextBox txtEmbeddable = (TextBox)cc;
                        txtEmbeddable.ReadOnly = true;
                        break;
                    case "tautil.tacombobox":
                        TAUtil.TAComboBox taCbo = (TAUtil.TAComboBox)cc;
                        taCbo.ReadOnly = true;
                        if (taCbo.ButtonsRight.Count > 0)
                            taCbo.ButtonsRight[0].Enabled = false;
                        break;
                    case "tautil.tadateeditor":
                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)cc;
                        taDate.ReadOnly = true;
                        if (taDate.ButtonsRight.Count > 0)
                            taDate.ButtonsRight[0].Enabled = false;
                        break;
                    case "infragistics.win.ultrawinmaskededit.ultramaskededit":
                        Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit mask = (Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit)cc;
                        mask.ReadOnly = true;
                        break;
                    case "infragistics.win.misc.ultrabutton":
                    case "system.windows.forms.button":
                        if(!cc.Name.ToLower().Equals("btnattachmentedit"))
                            cc.Enabled = false;
                        break;
                    case "tautil.tagrideditor":
                        TAUtil.TAGridEditor taGrid = (TAUtil.TAGridEditor)cc;
                        taGrid.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                        taGrid.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;                        


                        //Mic - we are trying this new method of locking the grid when the document is readonly instead of using column.activateonly
                        //by using the grid.allowupdate tpo control during when a document is open for readonly or editmode.
                        //this method is easier to implement as we do not need to loop thru all column and there's no need to implement the logic
                        //to determine which column to allow edit which is difficult to implement.


                        //foreach (UltraGridColumn gcol in taGrid.DisplayLayout.Bands[0].Columns)
                        //{
                        //    gcol.CellActivation = Activation.ActivateOnly;
                        //    if (gcol.Style == Infragistics.Win.UltraWinGrid.ColumnStyle.Button)
                        //    {
                        //        gcol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                        //    }
                        //}

                        break;
                    case "infragistics.win.ultrawingrid.ultragrid":
                    case "system.windows.forms.toolstrip":
                    case "infragistics.win.ultrawintabcontrol.ultratabcontrol":
                        //Ignore all button, tab control, grid and TextControl for Opening document
                        break;

                    default:
                        break;
                }
                if (cc.HasChildren && counter < 100)
                {
                    counter++;
                    foreach (Control moreControl in cc.Controls)
                    {
                        FormReadOnly_Loop(moreControl, ref counter);
                    }
                }
            }
        }
        public static void ControlReadOnly_Set(Control cc, bool readOnly)
        {   //Recusive call

            switch (cc.GetType().ToString().ToLower())
            {
                case "tautil.tacheckboxeditor":
                case "infragistics.win.ultrawineditors.ultracheckeditor":
                case "infragistics.win.ultrawintree.ultratree":
                case "infragistics.win.ultrawinlistview.ultralistview":
                case "system.windows.forms.radiobutton":
                case "infragistics.win.ultrawineditors.ultraoptionset":
                case "system.windows.forms.checkbox":
                    cc.Enabled = !readOnly;
                    break;
                case "infragistics.win.ultrawineditors.ultranumericeditor":
                    Infragistics.Win.UltraWinEditors.UltraNumericEditor ulnue = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)cc;
                    ulnue.ReadOnly = readOnly;
                    break;
                case "system.windows.forms.numericupdown":
                    System.Windows.Forms.NumericUpDown nud = (System.Windows.Forms.NumericUpDown)cc;
                    nud.ReadOnly = readOnly;
                    break;
                case "system.windows.forms.richtextbox":
                    System.Windows.Forms.RichTextBox embRicTxt = (System.Windows.Forms.RichTextBox)cc;
                    embRicTxt.ReadOnly = readOnly;
                    break;
                case "infragistics.win.formattedlinklabel.ultraformattedtexteditor":
                    Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor uftxt = (Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor)cc;
                    uftxt.ReadOnly = readOnly;
                    break;
                case "infragistics.win.ultrawinschedule.ultracalendarcombo":
                    Infragistics.Win.UltraWinSchedule.UltraCalendarCombo ulcbo = (Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)cc;
                    ulcbo.ReadOnly = readOnly;
                    break;

                case "tautil.tatextboxeditor":
                case "tautil.tanumericeditor":
                case "tautil.tacomboboxeditor":
                case "infragistics.win.ultrawineditors.ultracomboeditor":
                case "infragistics.win.ultrawineditors.ultratexteditor":
                    TextEditorControlBase txtEdtrCtrlBase = (TextEditorControlBase)cc;
                    if (txtEdtrCtrlBase.ButtonsRight.Count > 0)
                    {
                        txtEdtrCtrlBase.ButtonsRight[0].Enabled = !readOnly;
                    }
                    txtEdtrCtrlBase.ReadOnly = readOnly;
                    break;
                case "infragistics.win.embeddabletextboxwithuipermissions":
                    TextBox txtEmbeddable = (TextBox)cc;
                    txtEmbeddable.ReadOnly = readOnly;
                    break;
                case "tautil.tacombobox":
                    TAUtil.TAComboBox taCbo = (TAUtil.TAComboBox)cc;
                    taCbo.ReadOnly = readOnly;
                    break;
                case "tautil.tadateeditor":
                    TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)cc;
                    taDate.ReadOnly = readOnly;
                    break;
                case "infragistics.win.ultrawinmaskededit.ultramaskededit":
                    Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit mask = (Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit)cc;
                    mask.ReadOnly = readOnly;
                    break;
                case "infragistics.win.misc.ultrbutton":
                case "system.windows.forms.button":
                    cc.Enabled = true;
                    break;
                case "tautil.tagrideditor":
                    TAUtil.TAGridEditor taGrid = (TAUtil.TAGridEditor)cc;
                    foreach (UltraGridColumn gcol in taGrid.DisplayLayout.Bands[0].Columns)
                    {
                        gcol.CellActivation = readOnly ? Activation.ActivateOnly : Activation.AllowEdit;
                    }

                    break;
                case "infragistics.win.ultrawingrid.ultragrid":
                case "system.windows.forms.toolstrip":
                case "infragistics.win.ultrawintabcontrol.ultratabcontrol":
                    //Ignore all button, tab control, grid and TextControl for Opening document
                    break;
                default:
                    break;
            }
        }
        public static void FormEnable_Set(Control cc, Boolean Enable)
        {
            int counter = 0;
            FormEnable_Loop(cc, ref counter, Enable);
        }
        private static void FormEnable_Loop(Control cc, ref int counter, Boolean Enable)
        {
            string aa;
            //Recusive call
            Boolean RunLocking = false;
            aa = cc.GetType().ToString().ToLower();
            if (cc.HasChildren && counter < 100)
            {
                if (cc.Controls[0].GetType().Name == string.Empty)
                {
                    RunLocking = true;
                }
                else
                {
                    counter++;
                    foreach (Control moreControl in cc.Controls)
                    {
                        FormEnable_Loop(moreControl, ref counter, Enable);
                    }
                }
            }
            else RunLocking = true;


            if (RunLocking == true)
            {
                switch (cc.GetType().ToString().ToLower())
                {
                    case "tautil.tacheckboxeditor":
                    case "infragistics.win.ultrawineditors.ultracheckeditor":
                    case "infragistics.win.ultrawintree.ultratree":
                    case "infragistics.win.ultrawinlistview.ultralistview":
                    case "system.windows.forms.radiobutton":
                    case "infragistics.win.ultrawineditors.ultraoptionset":
                    case "system.windows.forms.checkbox":
                    case "infragistics.win.misc.ultrabutton":
                        cc.Enabled = Enable;
                        break;
                    case "system.windows.forms.toolstrip":
                        ToolStrip tsp = (ToolStrip)cc;
                        foreach (ToolStripItem button in tsp.Items)
                        {
                            if (GFunc.CompareString(button.Name.ToUpper(), "TSBCLOSE") == false)
                            {
                                button.Enabled = Enable;
                            }
                            if (button.GetType() == typeof(ToolStripLabel))
                            {
                                if (GFunc.CompareString(button.Name.ToUpper(), "TSLREADONLY"))
                                {
                                    button.Text = Enable ? "" : "Read Only";
                                }
                            }
                        }
                        break;
                    case "infragistics.win.ultrawineditors.ultranumericeditor":
                        Infragistics.Win.UltraWinEditors.UltraNumericEditor ulnue = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)cc;
                        ulnue.Enabled = Enable;
                        break;
                    case "system.windows.forms.numericupdown":
                        System.Windows.Forms.NumericUpDown nud = (System.Windows.Forms.NumericUpDown)cc;
                        nud.Enabled = Enable;
                        break;
                    case "system.windows.forms.richtextbox":
                        System.Windows.Forms.RichTextBox embRicTxt = (System.Windows.Forms.RichTextBox)cc;
                        embRicTxt.Enabled = Enable;
                        break;
                    case "infragistics.win.formattedlinklabel.ultraformattedtexteditor":
                        Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor uftxt = (Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor)cc;
                        uftxt.Enabled = Enable;
                        break;
                    case "infragistics.win.ultrawinschedule.ultracalendarcombo":
                        Infragistics.Win.UltraWinSchedule.UltraCalendarCombo ulcbo = (Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)cc;
                        ulcbo.Enabled = Enable;
                        break;
                    case "tautil.tatextboxeditor":
                    case "tautil.tanumericeditor":
                    case "tautil.tacomboboxeditor":
                    case "infragistics.win.ultrawineditors.ultracomboeditor":
                    case "infragistics.win.ultrawineditors.ultratexteditor":
                        TextEditorControlBase txtEdtrCtrlBase = (TextEditorControlBase)cc;
                        txtEdtrCtrlBase.Enabled = Enable; ;
                        break;
                    case "tautil.tacombobox":
                        TAUtil.TAComboBox taCbo = (TAUtil.TAComboBox)cc;
                        taCbo.Enabled = Enable; ;
                        break;
                    case "tautil.tadateeditor":
                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)cc;
                        taDate.Enabled = Enable; ;
                        break;

                    case "system.windows.forms.datetimepicker":
                        System.Windows.Forms.DateTimePicker taDatePicker = (System.Windows.Forms.DateTimePicker)cc;
                        taDatePicker.Enabled = Enable; ;
                        break;
                    case "infragistics.win.ultrawinmaskededit.ultramaskededit":
                        Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit mask = (Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit)cc;
                        mask.Enabled = Enable; ;
                        break;
                    case "infragistics.win.misc.ultrbutton":
                    case "system.windows.forms.button":
                        cc.Enabled = Enable;
                        break;
                    case "tautil.tagrideditor":
                        TAGridEditor grid = (TAGridEditor)cc;
                        int c = grid.DisplayLayout.Bands[0].Columns.Count;
                        for (int i = 0; i < c; i++)
                        {
                            grid.DisplayLayout.Bands[0].Columns[i].CellActivation = Enable ? Activation.AllowEdit : Activation.Disabled;
                        }

                        grid.DisplayLayout.Override.AllowDelete = Enable ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False;
                        break;
                    case "infragistics.win.ultrawingrid.ultragrid":
                    case "infragistics.win.ultrawintabcontrol.ultratab":
                        //Ignore all button, tab control, grid and TextControl for Opening document
                        break;
                    default:
                        break;
                }
            }
        }
        //this function is Not used by anywhere. required? --Jack
        public static void ControlTrigerValue_Set(Form form, object controlValue)
        {

            Control activeControl = GlobalUI.GetActiveControlAtActiveForm();

            if (activeControl.GetType() == typeof(TAUtil.TAComboBox))
            {
                ((TAUtil.TAComboBox)activeControl).SetValueTrigger(controlValue, true);
            }
            else if (activeControl.GetType() == typeof(TAUtil.TADateEditor))
            {
                ((TAUtil.TADateEditor)activeControl).SetValueTrigger(controlValue, true);
            }
            else if (activeControl.GetType() == typeof(TAUtil.TANumericEditor))
            {
                ((TAUtil.TANumericEditor)activeControl.Parent).SetValueTrigger(controlValue, true);
            }
            else if (activeControl.GetType() == typeof(TAUtil.TATextBoxEditor))
            {
                ((TAUtil.TATextBoxEditor)activeControl).SetValueTrigger(controlValue, true);
            }
            //Parent
            else if (activeControl.Parent.GetType() == typeof(TAUtil.TAComboBox))
            {
                ((TAUtil.TAComboBox)activeControl.Parent).SetValueTrigger(controlValue, true);
            }
            else if (activeControl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
            {
                ((TAUtil.TATextBoxEditor)activeControl.Parent).SetValueTrigger(controlValue, true);
            }
            else if (activeControl.Parent.GetType() == typeof(TAUtil.TADateEditor))
            {
                ((TAUtil.TADateEditor)activeControl.Parent).SetValueTrigger(controlValue, true);
            }
            else if (activeControl.Parent.GetType() == typeof(TAUtil.TANumericEditor))
            {
                ((TAUtil.TANumericEditor)activeControl.Parent).SetValueTrigger(controlValue, true);
            }
            else if (activeControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
            {
                if (((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell != null)
                {
                    if (!GFunc.IsNE(((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.EditorComponentResolved))
                    {
                        Control control = (TAUtil.TAComboBox)((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.EditorComponentResolved;
                        if (control.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            ((TAUtil.TAComboBox)control).SetValueTrigger(controlValue, true);
                        }
                        else if (control.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            ((TAUtil.TANumericEditor)control).SetValueTrigger(controlValue, true);
                        }
                        else if (control.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            ((TAUtil.TADateEditor)control).SetValueTrigger(controlValue, true);
                        }
                    }
                    else
                    {
                        string key = ((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.Column.Key.ToString();
                        ((TAUtil.TAGridEditor)activeControl.Parent).ActiveCell.Row.Cells[key].Value = controlValue;
                    }
                }
            }

        }
        public static void ResetControlDirty(Form frm)
        {
            try
            {
                TAUtil.ControlGVar.DirtyReset = true;
                frm.Validate();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                TAUtil.ControlGVar.DirtyReset = false;
            }
        }
        public static void ReSequenceGridSN(UltraGrid grd, string SNColumn)
        {
            //assume grid already update
            try
            {
                DataTable dt = grd.DataSource as DataTable;
                int SN = 0;
                dt.DefaultView.Sort = SNColumn + " ASC";

                foreach (DataRowView dr in dt.DefaultView)
                {
                    SN++;
                    dr[SNColumn] = SN;
                }
                dt.AcceptChanges();
                dt.DefaultView.Sort = SNColumn + " ASC";
                dt = dt.DefaultView.ToTable();
                grd.DataSource = dt;
                grd.Refresh();
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
            finally
            {
                TAUtil.ControlGVar.DirtyReset = false;
            }
        }

        public static T CreateGeneric<T>(params object[] args)
        {
            Type generic = typeof(T);
            return (T)Activator.CreateInstance(generic, args);
        }

        //Active form object get function
        //To get an object from active mdi child Form
        public static object ActiveFormObject_Get(string declaredobjectName)
        {
            try
            {
                Form f = GetActiveControlAtActiveForm().FindForm();
                FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                //declaring one should be 'declaredobjectName'
                object obj = myFieldInfo.FirstOrDefault(o => o.Name.ToLower().Contains(declaredobjectName.ToLower())).GetValue(f);
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //To get Document object from active Form--Pauk         
        public static Document ActiveFormDocument_Get()
        {
            Document objDoc = null;
            try
            {
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                ((DocInterface)f).GetDocInfor(out objDoc, out details);

                return objDoc;


            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        //To get an object from active mdi child Form --Pauk        
        public static object ActiveFormDocument_Get(string objectName)
        {
            object objDocVal = null;
            try
            {
                Form f = frmMain.gfrmMain.ActiveMdiChild;
                FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                objDocVal = myFieldInfo.First(o => o.Name.ToLower().Contains(objectName.ToLower())).GetValue(f);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
            return objDocVal;
        }
        public static int ActiveFormDocCodeAll_Get()
        {
            //To be able to retrieve the Document object when a Document form is active
            //This function has limitations.
            //Limitation 1, this funtion assumes that the form has factory object (eg. objARQOFactory)
            //Limitation 2, in factory property region, Document object must be the first index
            //The current boss project's code match with those limitations.
            //Tested in ARQO form and Factory.
            try
            {
                Document objDoc;
                Hashtable details;
                TAUtil.TAGridEditor grid = new TAGridEditor();
                Form f = GetActiveControlAtActiveForm().FindForm();//frmMain.gfrmMain.ActiveMdiChild;

                if (f.GetType().GetInterface("DocInterface", true) != null)
                {
                    ((DocInterface)f).GetDocInfor(out objDoc, out details);

                    return (int)objDoc.DocCodeKey;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        public static Control GetActiveControlAtActiveForm()
        {
            try
            {
                //We Can't Use frm.ActiveControl, The frm.ActiveControl is wrong for the control is place inside at 'splitContainer/groupbox/Panel'
                Control FocusControl = null;
                IntPtr handle = GetFocus();
                if (handle != IntPtr.Zero)
                {
                    FocusControl = Control.FromHandle(handle);
                }

                return FocusControl;
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        public static bool Ctrl_UpdateError(Control ctrl, ErrorProvider ep)
        {
            ep.SetError(ctrl, string.Empty);
            return true;
        }

        public static bool CloseOpenForms()
        {
            try
            {
                int frmCounts = Application.OpenForms.Count;
                if (frmCounts <= 1 && Application.OpenForms[0].Name.Contains("frmMain"))
                    return true;

                for (int i = frmCounts - 1; i >= 0; i--)
                {
                    if (Application.OpenForms[i].Name.Contains("frmMain"))
                        continue;

                    Application.OpenForms[i].Focus();
                    Application.OpenForms[i].Close();
                    if (frmMain.gfrmMain.Tag != null && (string)frmMain.gfrmMain.Tag == GVar.CancelMainFormClosing)
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public static bool Ctrl_Update(System.Windows.Forms.Form frm, string controlName, GEnum.CtlPropertyUpdate propNm, object value)
        {
            //note: can only set all control property (Enabled, Visible) but for (Readonly, Value) is only for Custom Control
            try
            {

                Control[] ctrls = frm.Controls.Find(controlName, true);
                if (ctrls.Length == 0) //Find in ToolStrip
                {
                    ctrls = frm.Controls.Find("tspBar", true);
                    if (ctrls.Length > 0)
                    {
                        ToolStripItem[] items = ((ToolStrip)ctrls[0]).Items.Find(controlName, true);
                        if (items.Length > 0)
                        {
                            switch (propNm)
                            {
                                case GEnum.CtlPropertyUpdate.Enabled:

                                    items[0].Enabled = (bool)value;
                                    break;

                                case GEnum.CtlPropertyUpdate.Visible:

                                    items[0].Visible = (bool)value;
                                    break;
                                case GEnum.CtlPropertyUpdate.Value:

                                    items[0].Text = value.ToString();
                                    items[0].Visible = value.ToString().Length > 0 ? true : false;
                                    break;
                            }
                        }
                    }
                    return true; //no need to find anymore
                }

                if (ctrls.Length > 0)
                {
                    switch (propNm)
                    {
                        case GEnum.CtlPropertyUpdate.Enabled:
                            ctrls[0].Enabled = (bool)value;
                            break;

                        case GEnum.CtlPropertyUpdate.Visible:
                            ctrls[0].Visible = (bool)value;
                            //if (ctrls[0].GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl))                                
                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)ctrls[0]).Tabs["tsbApplyInvoice"].Visible = (bool)value;
                            break;

                        case GEnum.CtlPropertyUpdate.Readonly:
                            if (ctrls[0].GetType() == typeof(TAUtil.TATextBoxEditor))
                                ((TAUtil.TATextBoxEditor)ctrls[0]).ReadOnly = (bool)value;

                            else if (ctrls[0].GetType() == typeof(TAUtil.TANumericEditor))
                                ((TAUtil.TANumericEditor)ctrls[0]).ReadOnly = (bool)value;

                            else if (ctrls[0].GetType() == typeof(TAUtil.TADateEditor))
                                ((TAUtil.TADateEditor)ctrls[0]).ReadOnly = (bool)value;

                            else if (ctrls[0].GetType() == typeof(TAUtil.TAComboBox))
                                ((TAUtil.TAComboBox)ctrls[0]).ReadOnly = (bool)value;

                            else if (ctrls[0].GetType() == typeof(TAUtil.TACheckBoxEditor))
                                ((TAUtil.TACheckBoxEditor)ctrls[0]).Enabled = (bool)value;

                            break;

                        case GEnum.CtlPropertyUpdate.Value:
                            if (ctrls[0].GetType() == typeof(TAUtil.TATextBoxEditor))
                                ((TAUtil.TATextBoxEditor)ctrls[0]).Text = value.ToString();

                            else if (ctrls[0].GetType() == typeof(TAUtil.TANumericEditor))
                                ((TAUtil.TANumericEditor)ctrls[0]).Text = value.ToString();

                            else if (ctrls[0].GetType() == typeof(TAUtil.TADateEditor))
                                ((TAUtil.TADateEditor)ctrls[0]).DateValue = Convert.ToDateTime(value);

                            else if (ctrls[0].GetType() == typeof(TAUtil.TAComboBox))
                                ((TAUtil.TAComboBox)ctrls[0]).Text = value.ToString();

                            else if (ctrls[0].GetType() == typeof(TAUtil.TACheckBoxEditor))
                                ((TAUtil.TACheckBoxEditor)ctrls[0]).Checked = (bool)value;

                            break;
                    }
                }
                return true;
            }
            catch (TAUtil.TAException taex)
            {
                throw Error(taex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        //Popup Display Function
        public static void PopupDisplay(string popoupFormName, UltraGrid ugrd,bool forceRefresh)
        {   //This function covers for 3 popups (Enquiry,HistorySale,HistoryPurchase)    

            try
            {
                int itmKey = 0;
                
                if (ugrd != null)
                {
                    switch (ugrd.FindForm().Name.ToLower())
                    {
                        case "frmarqo":
                        case "frmardo":
                        case "frmariv":
                        case "frmarpl":
                        case "frmarso":
                        case "frmarro":

                        case "frmapbl":
                        case "frmappd":
                        case "frmappj":
                        case "frmappn":
                        case "frmappo":
                        case "frmaprq":

                        case "frminadj":
                        case "frminmfn":
                        case "frmintrn":

                        case "frmcscpd":
                        case "frmcscpo":
                        case "frmcscps":
                        case "frmcscsi":

                            if (ugrd.ActiveRow != null)

                                if (ugrd.DisplayLayout.Bands[0].Columns.IndexOf("ItmKey") > 0)
                                {
                                    Int32.TryParse(ugrd.ActiveRow.Cells["ItmKey"].Value.ToString(), out itmKey);
                                }
                            break;
                    }
                }
                else
                {
                    AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();
                    Control activeControl = GetActiveControlAtActiveForm();
                    Form f = (Form)activeControl.TopLevelControl;

                    if (f == frmMain.gfrmMain || GFunc.IsNE(f))
                    {
                        f = frmMain.gfrmMain.ActiveMdiChild;
                    }
                    if (f != null)
                    {
                        switch (f.Name.ToLower())
                        {
                            case "frmitmenquiry":
                                TATextBoxEditor itmid = (TATextBoxEditor)f.Controls.Find("ItmID", true)[0];
                                MSTItm objItm = MSTItm.Get(itmid.Text);
                                itmKey = GFunc.NEInt(objItm.ItmKey, 0);

                                break;
                        }
                    }
                }

                //Show Popup
                frmItmEnquiry fItmEnquiry = null;
                frmItmHisSummary fItmHisSummary = null;
                frmItmHistorySale fItmHistory = null;
                frmItmHistoryPurchase fItmHistoryPur = null;

                //If it is already loaded, take that one
                foreach (Form forms in Application.OpenForms[0].OwnedForms)
                {
                    if (forms.Name == Form_Name.FRM_ITMENQUIRY && forms.Name == popoupFormName)
                        fItmEnquiry = (frmItmEnquiry)forms;
                    if (forms.Name == Form_Name.FRM_ITMHISSUMMARY && forms.Name == popoupFormName)
                        fItmHisSummary = (frmItmHisSummary)forms;
                    if (forms.Name == Form_Name.FRM_ITMHISTORYSALE && forms.Name == popoupFormName)
                        fItmHistory = (frmItmHistorySale)forms;
                    if (forms.Name == Form_Name.FRM_ITMHISTORYPURCHASE && forms.Name == popoupFormName)
                        fItmHistoryPur = (frmItmHistoryPurchase)forms;
                }

                //If it's not loaded yet, create new, else reload existing one
                //For ItmEnquiry Popup
                if (fItmEnquiry == null && popoupFormName == Form_Name.FRM_ITMENQUIRY)
                {
                    fItmEnquiry = new frmItmEnquiry(itmKey);
                    fItmEnquiry.TopLevel = true;

                    fItmEnquiry.Show(Application.OpenForms[0]);
                    Application.OpenForms[0].AddOwnedForm(fItmEnquiry);
                }
                else if (fItmEnquiry != null && popoupFormName == Form_Name.FRM_ITMENQUIRY)
                {
                    fItmEnquiry.Reload(itmKey,forceRefresh);
                    fItmEnquiry.WindowState = FormWindowState.Normal;
                }
                if (fItmHisSummary == null && popoupFormName == Form_Name.FRM_ITMHISSUMMARY)
                {
                    fItmHisSummary = new frmItmHisSummary(itmKey);
                    fItmHisSummary.TopLevel = true;

                    fItmHisSummary.Show(Application.OpenForms[0]);
                    Application.OpenForms[0].AddOwnedForm(fItmHisSummary);
                }
                else if (fItmHisSummary != null && popoupFormName == Form_Name.FRM_ITMHISSUMMARY)
                {
                    fItmHisSummary.Reload(itmKey);
                    fItmHisSummary.WindowState = FormWindowState.Normal;

                }
                //For ItmHistory Popup
                else if (fItmHistory == null && popoupFormName == Form_Name.FRM_ITMHISTORYSALE)
                {
                    fItmHistory = new frmItmHistorySale(itmKey);
                    fItmHistory.TopLevel = true;
                    fItmHistory.Show(Application.OpenForms[0]);
                    Application.OpenForms[0].AddOwnedForm(fItmHistory);
                }
                else if (fItmHistory != null && popoupFormName == Form_Name.FRM_ITMHISTORYSALE)
                {
                    fItmHistory.Reload(itmKey, forceRefresh);
                    fItmHistory.WindowState = FormWindowState.Normal;

                }
                else if (fItmHistoryPur == null && popoupFormName == Form_Name.FRM_ITMHISTORYPURCHASE)
                {
                    fItmHistoryPur = new frmItmHistoryPurchase(itmKey);
                    fItmHistoryPur.TopLevel = true;
                    fItmHistoryPur.Show(Application.OpenForms[0]);
                    Application.OpenForms[0].AddOwnedForm(fItmHistoryPur);
                }
                else if (fItmHistoryPur != null && popoupFormName == Form_Name.FRM_ITMHISTORYPURCHASE)
                {
                    fItmHistoryPur.Reload(itmKey, forceRefresh);
                    fItmHistoryPur.WindowState = FormWindowState.Normal;
                }

            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }


        }
        public static void PopupDisplayBA(string popoupFormName, UltraGrid ugrd)
        {
            try
            {
                int? keyValue = null;
                Document doc = GlobalUI.ActiveFormDocument_Get();
                if (ugrd != null)
                {
                    if (ugrd.ActiveRow != null)
                    {
                        if (GFunc.IsNE(ugrd.Rows[ugrd.ActiveRow.Index].Cells["DocItmKey"].Value))
                            keyValue = null;
                        else
                            keyValue = Convert.ToInt32(ugrd.Rows[ugrd.ActiveRow.Index].Cells["DocItmKey"].Value);
                    }
                }

                if ((GEnum.SystemCode)doc.DocCodeKey == GEnum.SystemCode.Inventory_Production)
                {
                    frmBatchSelection fItmBatch = null;
                    frmAssemblyEntry fItmAssembly = null;

                    foreach (Form forms in Application.OpenForms[0].OwnedForms)
                    {
                        if (forms.Name == Form_Name.FRM_BATCHSELECTION && forms.Name == popoupFormName)

                            fItmBatch = (frmBatchSelection)forms;

                        else if (forms.Name == Form_Name.FRM_ASSEMBLYENTRY && forms.Name == popoupFormName)

                            fItmAssembly = (frmAssemblyEntry)forms;

                    }

                    if (fItmBatch != null && popoupFormName == Form_Name.FRM_BATCHSELECTION)
                    {
                        fItmBatch.Reload(doc, ugrd, keyValue);
                    }
                    else if (fItmBatch == null && popoupFormName == Form_Name.FRM_BATCHSELECTION)
                    {
                        fItmBatch = new frmBatchSelection(doc, ugrd, true);
                        fItmBatch.TopLevel = true;
                        fItmBatch.StartPosition = FormStartPosition.WindowsDefaultLocation;
                        fItmBatch.Show(Application.OpenForms[0]);
                        Application.OpenForms[0].AddOwnedForm(fItmBatch);
                    }
                    else if (fItmAssembly != null && popoupFormName == Form_Name.FRM_ASSEMBLYENTRY)
                    {
                        fItmAssembly.Reload(keyValue);
                    }
                    else if (fItmBatch == null && popoupFormName == Form_Name.FRM_ASSEMBLYENTRY)
                    {
                        fItmAssembly = new frmAssemblyEntry(doc, ugrd, (int)keyValue, false);
                        fItmAssembly.TopLevel = true;
                        fItmAssembly.Show(Application.OpenForms[0]);
                        fItmAssembly.StartPosition = FormStartPosition.WindowsDefaultLocation;
                        Application.OpenForms[0].AddOwnedForm(fItmAssembly);

                    }
                }
                else
                {
                    frmBatchEntry fItmBatch = null;
                    frmAssemblyEntry fItmAssembly = null;

                    foreach (Form forms in Application.OpenForms[0].OwnedForms)
                    {
                        if (forms.Name == Form_Name.FRM_BATCHENTRY && forms.Name == popoupFormName)

                            fItmBatch = (frmBatchEntry)forms;

                        else if (forms.Name == Form_Name.FRM_ASSEMBLYENTRY && forms.Name == popoupFormName)

                            fItmAssembly = (frmAssemblyEntry)forms;

                    }




                    if (fItmBatch != null && popoupFormName == Form_Name.FRM_BATCHENTRY)
                    {
                        fItmBatch.Reload(doc, ugrd, keyValue);
                    }
                    else if (fItmBatch == null && popoupFormName == Form_Name.FRM_BATCHENTRY)
                    {
                        fItmBatch = new frmBatchEntry(doc, ugrd, true);
                        fItmBatch.TopLevel = true;
                        fItmBatch.StartPosition = FormStartPosition.WindowsDefaultLocation;
                        fItmBatch.Show(Application.OpenForms[0]);
                        Application.OpenForms[0].AddOwnedForm(fItmBatch);
                    }
                    else if (fItmAssembly != null && popoupFormName == Form_Name.FRM_ASSEMBLYENTRY)
                    {
                        fItmAssembly.Reload(keyValue);
                    }
                    else if (fItmBatch == null && popoupFormName == Form_Name.FRM_ASSEMBLYENTRY)
                    {
                        fItmAssembly = new frmAssemblyEntry(doc, ugrd, (int)keyValue, false);
                        fItmAssembly.TopLevel = true;
                        fItmAssembly.Show(Application.OpenForms[0]);
                        fItmAssembly.StartPosition = FormStartPosition.WindowsDefaultLocation;
                        Application.OpenForms[0].AddOwnedForm(fItmAssembly);

                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }


        }
        public static void PopupDisplay(string popoupFormName, GEnum.SystemCode docCodeKey, int? docKey)
        {
            try
            {
                frmDocListDet fItmInfo = null;

                foreach (Form forms in Application.OpenForms[0].OwnedForms)
                {
                    if (forms.Name == GlobalUI.Form_Name.FRM_DOCLISTDET && forms.Name == popoupFormName)
                        fItmInfo = (frmDocListDet)forms;
                }

                //If it's not loaded yet, create new; else reload existing one
                //For ItmInfo Popup
                if (fItmInfo == null && popoupFormName == GlobalUI.Form_Name.FRM_DOCLISTDET)
                {
                    fItmInfo = new frmDocListDet(docCodeKey, (int)docKey);
                    fItmInfo.TopLevel = true;
                    fItmInfo.Show(Application.OpenForms[0]);
                    Application.OpenForms[0].AddOwnedForm(fItmInfo);
                }
                else if (fItmInfo != null && popoupFormName == Form_Name.FRM_DOCLISTDET)
                {
                    fItmInfo.Reload(docCodeKey, (int)docKey);
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, true, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, false);
            }
        }
        public static void PopupDisplay(string popoupFormName, GEnum.SystemCode docCodeKey, int? docKey, string DocID)
        {
            try
            {

                frmDocRelationship fDocRelationship = null;

                foreach (Form forms in Application.OpenForms[0].OwnedForms)
                {

                    if (forms.Name == GlobalUI.Form_Name.FRM_DOCRELATIONSHIP && forms.Name == popoupFormName)
                        fDocRelationship = (frmDocRelationship)forms;
                }


                //If it's not loaded yet, create new; else reload existing one
                //For ItmInfo Popup
                if (fDocRelationship == null && popoupFormName == GlobalUI.Form_Name.FRM_DOCRELATIONSHIP)
                {
                    fDocRelationship = new frmDocRelationship((int)docCodeKey, (int)docKey, DocID);
                    fDocRelationship.TopLevel = true;
                    fDocRelationship.Show(Application.OpenForms[0]);
                    Application.OpenForms[0].AddOwnedForm(fDocRelationship);
                }
                else if (fDocRelationship != null && popoupFormName == Form_Name.FRM_DOCRELATIONSHIP)
                {
                    fDocRelationship.Reload(docCodeKey, (int)docKey);
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, true, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, false);
            }
        }
        public static void PopupRefresh(UltraGrid ugrd)
        {
            bool frm_ItmEnquiry = false;
            bool frm_ItmHistoryPurchase = false;
            bool frm_ItmHistorySale = false;

            bool frm_BatchEntry = false;
            bool frm_AssemblyEntry = false;

            //Check existing popups
            foreach (Form forms in Application.OpenForms[0].OwnedForms)
            {
                if (forms.WindowState != FormWindowState.Minimized)
                    switch (forms.Name)
                    {
                        case Form_Name.FRM_ITMENQUIRY:
                            frm_ItmEnquiry = true;
                            break;
                        case Form_Name.FRM_ITMHISTORYSALE:
                            frm_ItmHistorySale = true;
                            break;
                        case Form_Name.FRM_ITMHISTORYPURCHASE:
                            frm_ItmHistoryPurchase = true;
                            break;
                        case Form_Name.FRM_ASSEMBLYENTRY:
                            frm_AssemblyEntry = true;
                            break;
                        case Form_Name.FRM_BATCHENTRY:
                            frm_BatchEntry = true;
                            break;
                        default:
                            break;
                    }
            }

            if (frm_ItmEnquiry)
                GlobalUI.PopupDisplay(Form_Name.FRM_ITMENQUIRY, ugrd,false);
            if (frm_AssemblyEntry)
                GlobalUI.PopupDisplayBA(Form_Name.FRM_ASSEMBLYENTRY, ugrd);
            if (frm_BatchEntry)
                GlobalUI.PopupDisplayBA(Form_Name.FRM_BATCHENTRY, ugrd);

            //NOTE: Currently we do not allow the Item Sales/Purchase report to refresh when a row is activated
            //in the event if we were to add this feature, we will just need to uncomment the below code
            if (frm_ItmHistoryPurchase)
                GlobalUI.PopupDisplay(Form_Name.FRM_ITMHISTORYPURCHASE, ugrd, false);
            if (frm_ItmHistorySale)
                GlobalUI.PopupDisplay(Form_Name.FRM_ITMHISTORYSALE, ugrd, false);

        }//Completed
        public static void PopupRefresh(GEnum.SystemCode docCodeKey, int? docKey)
        {
            bool FRM_DOCLISTDET = false;
            try
            {
                //Check existing popups
                foreach (Form forms in Application.OpenForms[0].OwnedForms)
                {
                    switch (forms.Name)
                    {
                        case Form_Name.FRM_DOCLISTDET:
                            FRM_DOCLISTDET = true;
                            break;
                        default:
                            break;
                    }
                }

                if (FRM_DOCLISTDET)
                    GlobalUI.PopupDisplay(Form_Name.FRM_DOCLISTDET, docCodeKey, docKey);
            }
            catch (TAException taex)
            {
                throw Error(taex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }


        //Use when create new,open record, after a read Only record is opened
        public static void GridAllColumnsActivateOnlySet(TAUtil.TAGridEditor grd)
        {
            for (int i = 0; i < grd.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                 grd.DisplayLayout.Bands[0].Columns[i].CellActivation = Activation.ActivateOnly;
            }
        }

        //Grid   ->Format all grids in a FORM 
        public static void Grid_Format(TAUtil.TAGridEditor tagrd, string listSettingID, bool fillData, bool readOnly)
        {
            try
            {
                int documentCodeKey = 0;
                SYSFormSettingID objListSettingDet;

                if (GFunc.CompareString(GVar.ListSettingID.frmListGridConCust, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridConVend, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridItm, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridJob, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridAcc, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridAlert, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridPrice, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridShipName, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridToDo, listSettingID))
                {
                    listSettingID += "%%%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                }
                else if (GFunc.CompareString(GVar.ListSettingID.frmGLFinChargeGrid, listSettingID))
                {
                    listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                }
                else if (GFunc.CompareString(GVar.ListSettingID.frmInsertDataMatrixMasterGrid, listSettingID))
                {
                    listSettingID += "%%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                }
                else if (GFunc.CompareString("frmItemCopyGrid", listSettingID))
                {
                    listSettingID += "%%" + AppInfor.CurrentUserKey + "%" + AppInfor.ItemAccessLevel;
                }
                else if (GFunc.CompareString(GVar.ListSettingID.frmDocCopyGridAPRQ, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmMSTBudgetCopyGrid, listSettingID))
                {
                    object obj = ActiveFormObject_Get("_budgetType");
                    listSettingID += "%" + GFunc.NEInt(obj, 0) + "%''%''";
                }
                else if (GFunc.CompareString(GVar.ListSettingID.frmAccSelectionGrid, listSettingID))
                {
                    listSettingID += "%%" + AppInfor.CurrentUserKey + "%" + AppInfor.ConAccessLevel;
                }
                else if (GFunc.CompareString("frmREFCatGridList", listSettingID))
                {
                    if (!listSettingID.Contains("%"))
                        listSettingID += "%1%";
                }
                else if (GFunc.CompareString("frmRepAccTrialBalGrid", listSettingID) ||
                GFunc.CompareString("frmSysShortCutGridShortCutList", listSettingID))
                {
                    listSettingID += "%" + AppInfor.CurrentUserKey;
                }               

                //ATL
                Form f = tagrd.FindForm();
                if (f != null)
                {
                    if (GFunc.CompareString(GVar.FormName.frmDocList, f.Name))
                    {
                        TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateFrom", true)[0]);
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                        listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                        listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                    }
                    else if(f.Name.Equals("frmDocListNoAttachments"))
                    {
                        TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateFrom", true)[0]);
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                        RadioButton opt = ((RadioButton)tagrd.FindForm().Controls.Find("optNoAttachSignedDO", true)[0]);
                        listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                        if(opt!=null)
                            listSettingID += "%" +(opt.Checked?2:1);
                    }
                    else if (GFunc.CompareString("frmDocListSOOut", f.Name))
                    {                        
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);                       
                        listSettingID += "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy 23:59:59");
                    }
                    else if (GFunc.CompareString("frmDocListPOOut", f.Name))
                    {
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                        listSettingID += "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy 23:59:59");
                    }                    
                    ////ttm on 01 dec 2016
                    //else if (GFunc.CompareString(GVar.FormName.frmList, f.Name))
                    //{
                    //    TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateFrom", true)[0]);
                    //    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                    //    listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                    //}
                    else if (GFunc.CompareString(GVar.FormName.frmDocSearch, f.Name))
                    {
                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                        ////declaring one should be 'declaredobjectName'
                        object objCurrKey = myFieldInfo.First(o => o.Name.ToLower().Contains("DocCurrKey".ToLower())).GetValue(frm);
                        object objConKey = myFieldInfo.First(o => o.Name.ToLower().Contains("DocConKey".ToLower())).GetValue(frm);
                        TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("FromDate", true)[0]);
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("ToDate", true)[0]);
                        listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                        listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy") + "%" + GFunc.NEInt(objConKey, 0) + "%" + GFunc.NEInt(objCurrKey, 0);
                    }
                    else if (GFunc.CompareString(GVar.FormName.frmWOSearch, f.Name))
                    {
                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                        ////declaring one should be 'declaredobjectName'
                        //object objCurrKey = myFieldInfo.First(o => o.Name.ToLower().Contains("DocCurrKey".ToLower())).GetValue(frm);
                        //object objConKey = myFieldInfo.First(o => o.Name.ToLower().Contains("DocConKey".ToLower())).GetValue(frm);
                        TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("FromDate", true)[0]);
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("ToDate", true)[0]);
                        listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                        listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                    }
                    else if (GFunc.CompareString(GVar.FormName.frmSYSMsgListText, f.Name))
                    {
                        TAComboBox cbo = ((TAComboBox)tagrd.FindForm().Controls.Find("tacboMsgGroup", true)[0]);
                        listSettingID += "%" + GFunc.NEInt(cbo.Value, 0);
                    }
                    else if (GFunc.CompareString("frmstockcountitmadj", f.Name))
                    {
                        listSettingID += "%2%" + AppInfor.CurrentUserKey;
                    }
                    else if (GFunc.CompareString(GVar.FormName.frmSYSCodeSYS_IDCounter, f.Name))
                    {
                        object obj = ActiveFormObject_Get("CodeKey");
                        listSettingID += "%" + GFunc.NEStr(obj, "0");
                    }
                    else if (GFunc.CompareString(GVar.FormName.frmDocCopy, f.Name))
                    {
                        TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("StartDate", true)[0]);
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("EndDate", true)[0]);
                        TAComboBox DocCode = ((TAComboBox)tagrd.FindForm().Controls.Find("DocCode", true)[0]);

                        if (GFunc.IsNEZ(DocCode.Value))
                            return;
                        switch ((int)DocCode.SelectedRow.Cells["CustomInt1"].Value)
                        {
                            case (int)GEnum.SystemCode.Purchase_Request:
                            case (int)GEnum.SystemCode.Inventory_Adjustment:
                            case (int)GEnum.SystemCode.Inventory_Production:
                            case (int)GEnum.SystemCode.Inventory_Transfer:                            
                                listSettingID = "frmDocCopyGridAPRQ";
                                break;
                            case (int)GEnum.SystemCode.Journal:
                                listSettingID = "frmDocCopyGridGLJNL";
                                break;
                            case (int)GEnum.SystemCode.Packing_List:

                                listSettingID = "frmDocCopyGridARPL";
                                break;
                        }

                        listSettingID += "%" + GFunc.NEStr(DocCode.SelectedRow.Cells["TblNm"].Value, string.Empty);
                        listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                        listSettingID += "%" + GFunc.NEInt(DocCode.SelectedRow.Cells["CustomInt1"].Value, 0);
                    }
                    else if (GFunc.CompareString(GVar.FormName.frmDocSelection, f.Name))
                    {
                        TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("FromDate", true)[0]);
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("ToDate", true)[0]);
                        TAComboBox DocCode = ((TAComboBox)tagrd.FindForm().Controls.Find("DocCode", true)[0]);

                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        object objConKey = myFieldInfo.First(o => o.Name.ToLower().Contains("ConKey".ToLower())).GetValue(frm);

                        listSettingID += "%" + GFunc.NEInt(DocCode.SelectedRow == null ? 0 : DocCode.SelectedRow.Cells["CustomInt1"].Value, 0);
                        listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                        listSettingID += "%" + GFunc.NEInt(objConKey, 0);


                    }
                    else if (GFunc.CompareString(GVar.FormName.frmInsertPackingList, f.Name))
                    {
                        bool IncludeBatch = ((frmInsertPackingList)tagrd.FindForm()).Batch;
                        string DocIDList = ((frmInsertPackingList)tagrd.FindForm()).DocIDList;
                        int DocCodeKey = ((frmInsertPackingList)tagrd.FindForm()).DocCodeKey;
                        listSettingID = listSettingID + "%" + DocCodeKey;
                        listSettingID += "%" + DocIDList + "%" + IncludeBatch;

                    }
                    else if (GFunc.CompareString(GVar.FormName.frmPrintSelectionList, f.Name))
                    {
                        TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("FromDate", true)[0]);
                        TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("ToDate", true)[0]);
                        TATextBoxEditor docRangeFrom = ((TATextBoxEditor)tagrd.FindForm().Controls.Find("DocRangeFrom", true)[0]);
                        TATextBoxEditor docRangeTo = ((TATextBoxEditor)tagrd.FindForm().Controls.Find("DocRangeTo", true)[0]);

                        listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                        listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                        listSettingID += "%" + docRangeFrom.Text + "%" + docRangeTo.Text;
                    }
                    else if (f.Name.ToLower() == "frminsertsalespo" || listSettingID.ToLower() == "frminsertsalespogrid"
                        || f.Name.ToLower() == "frminsertsalesbl" || listSettingID.ToLower() == "frminsertsalesblgrid"
                        )
                    {
                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        TADateEditor DocDate = null;
                        //declaring one should be 'declaredobjectName'                        
                        object objConKey = myFieldInfo.First(o => o.Name.ToLower().Contains("ConKey".ToLower())).GetValue(frm);
                        object objPOKey = myFieldInfo.First(o => o.Name.ToLower().Contains("PODocKey".ToLower())).GetValue(frm);
                        object objItmKey = myFieldInfo.First(o => o.Name.ToLower().Contains("FilterItmKey".ToLower())).GetValue(frm); 
                        DocDate = ((TADateEditor)tagrd.FindForm().Controls.Find("PrmDate", true)[0]);

                        listSettingID += "%'" + GFunc.NEDateTime(DocDate.DateValue, DateTime.Today.Date).ToString("d-MMM-yyyy") + "'%" + GFunc.NEInt(objConKey, 0) + "%" + GFunc.NEInt(objItmKey, 0) + "%" + GFunc.NEInt(objPOKey, 0);

                    }
                    else if (f.Name.ToLower() == "frminsertsalesso" || listSettingID.ToLower() == "frminsertsalessogrid")
                    {
                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        TADateEditor DocDate = null;
                        //declaring one should be 'declaredobjectName'                        
                        object objConKey = myFieldInfo.First(o => o.Name.ToLower().Contains("ConKey".ToLower())).GetValue(frm);
                        object objSOKey = myFieldInfo.First(o => o.Name.ToLower().Contains("SODocKey".ToLower())).GetValue(frm);
                        object objItmKey = myFieldInfo.First(o => o.Name.ToLower().Contains("FilterItmKey".ToLower())).GetValue(frm);
                        DocDate = ((TADateEditor)tagrd.FindForm().Controls.Find("PrmDate", true)[0]);

                        listSettingID += "%'" + GFunc.NEDateTime(DocDate.DateValue, DateTime.Today.Date).ToString("d-MMM-yyyy") + "'%" + GFunc.NEInt(objConKey, 0) + "%" + GFunc.NEInt(objItmKey, 0) + "%" + GFunc.NEInt(objSOKey, 0);

                    }
                    else if (f.Name.ToLower() == "frminsertso" || f.Name.ToLower() == "frminsertcsi" || f.Name.ToLower() == "frminsertcpo" || f.Name.ToLower() == "frminsertpo" || f.Name.ToLower() == "frminsertpd")
                    {
                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        TADateEditor DocDate = null;
                        //declaring one should be 'declaredobjectName'
                        object objCurrKey = myFieldInfo.First(o => o.Name.ToLower().Contains("CurrKey".ToLower())).GetValue(frm);
                        object objConKey = myFieldInfo.First(o => o.Name.ToLower().Contains("ConKey".ToLower())).GetValue(frm);
                        if (f.Name.ToLower() == "frminsertcsi" || f.Name.ToLower() == "frminsertpd")
                            DocDate = ((TADateEditor)tagrd.FindForm().Controls.Find("DocDate", true)[0]);
                        else
                            DocDate = ((TADateEditor)tagrd.FindForm().Controls.Find("PrmDate", true)[0]);

                        listSettingID += "%'" + GFunc.NEDateTime(DocDate.DateValue, DateTime.Today.Date).ToString("d-MMM-yyyy") + "'%" + GFunc.NEInt(objConKey, 0) + "%" + GFunc.NEInt(objCurrKey, 0) + "";

                    }
                    else if (f.Name.ToLower() == "frminsertalternateitem")
                    {
                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        TADateEditor DocDate = null;
                        //declaring one should be 'declaredobjectName'
                        object objItmKey = myFieldInfo.First(o => o.Name.ToLower().Contains("ItmKey".ToLower())).GetValue(frm);

                        listSettingID += "%" + GFunc.NEInt(objItmKey, 0) + "";

                    }
                    else if (f.Name.ToLower() == "frminsertdatamatrix")
                    {
                        Document objDoc = null;
                        Hashtable details = null;
                        ((DocInterface)f).GetDocInfor(out objDoc, out details);
                        switch ((int)objDoc.DocCodeKey)
                        {
                            case (int)GEnum.SystemCode.Order_Consignment:
                            case (int)GEnum.SystemCode.Received_Consignment:
                                listSettingID = GVar.ListSettingID.MSTItmMCSG_id + "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                                break;
                        }

                    }
                    else if (f.Name.ToLower() == "frmdocrelationship")
                    {
                        Form frm = tagrd.FindForm();
                        FieldInfo[] myFieldInfo = frm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                        object objDocCodeKey = myFieldInfo.First(o => o.Name.ToLower().Contains("_DocCodeKey".ToLower())).GetValue(frm);
                        object objDocKey = myFieldInfo.First(o => o.Name.ToLower().Contains("_DocKey".ToLower())).GetValue(frm);

                        listSettingID += "%'" + objDocCodeKey.ToString() + "'%" + 0;

                    }
                    else if (GFunc.CompareString("frmRepKPICmpTgt", f.Name) || GFunc.CompareString("frmAR_OBNPPRJ", f.Name))
                    {                        
                        FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        object obj = myFieldInfo.First(o => o.Name.ToLower().Contains("tacboyear")).GetValue(f);//tacboMth
                        object obj1 = myFieldInfo.First(o => o.Name.ToLower().Contains("tacbomth")).GetValue(f);//tacboMth
                        if (obj.GetType() == typeof(TAUtil.TAComboBox) && obj1.GetType() == typeof(TAUtil.TAComboBox))
                            listSettingID += "%" + GFunc.NEInt(((TAUtil.TAComboBox)obj).Value, DateTime.Today.Year)+"%" + GFunc.NEInt(((TAUtil.TAComboBox)obj1).Value, DateTime.Today.Month);

                    }
                }
                string[] ListParameters = listSettingID.Split('%');
                SYSFormSettingIDDet objFormSettingDet = null;
                if (ListParameters.Length > 0)
                {
                    //Use index 0 to List Setting.
                    objListSettingDet = SYSFormSettingID.Get(ListParameters[0]);
                    objFormSettingDet = SYSFormSettingIDDet.Get(ListParameters[0]);
                }
                else
                    objListSettingDet = SYSFormSettingID.Get(listSettingID);

                //Set Grid Row Height   
                double pixel = (96 * objListSettingDet.RowHeightCM) / 2.54;
                tagrd.DisplayLayout.Bands[0].Override.RowSizing = RowSizing.Sychronized;
                tagrd.DisplayLayout.Bands[0].Override.DefaultRowHeight = GFunc.NEInt(pixel,0);

                DataTable dtdet = (DataTable)objFormSettingDet;

                if (!GFunc.IsNE(objListSettingDet))
                {
                    for (int i = 1; i < ListParameters.Length; i++)
                    {
                        string oldVal = "{" + (i - 1) + "}";
                        objListSettingDet.ListSQL = objListSettingDet.ListSQL.Replace(oldVal, ListParameters[i]);
                    }
                }

                if (!GFunc.IsNE(objListSettingDet.ListSQL))
                {
                    tagrd.DataSource = GFunc.ExecuteQuery(objListSettingDet.ListSQL);
                }

                if (dtdet == null)
                {
                    MsgBox.Show("FormSettingIDDet haven't filled!");
                    //Not error, don't throw just show the information
                }
                else
                {
                    switch (listSettingID)
                    {
                        ////Skip ListID that don't want to format grid
                        //case "frmMSTItmGridPrice":
                        //    break;
                        default:
                            Grid_Format(tagrd, dtdet, readOnly);
                            break;
                    }

                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void Grid_Format(TAUtil.TAGridEditor tagrd, string listSettingID, bool fillData)
        {
            try
            {
                SYSFormSettingID objListSettingDet;
                //ATL Need to update to SG*

                if (GFunc.CompareString(GVar.ListSettingID.frmListGridConCust, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridConVend, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridItm, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridJob, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridAcc, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridAlert, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridPrice, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridShipName, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmListGridToDo, listSettingID) ||
                GFunc.CompareString(GVar.ListSettingID.frmGLFinChargeGrid, listSettingID))
                {
                    listSettingID += "%%%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                }

                //ATL
                if (GFunc.CompareString(GVar.FormName.frmDocList, tagrd.FindForm().Name))
                {
                    TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateFrom", true)[0]);
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                    listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                    listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                }
                else if (GFunc.CompareString("frmDocListSOOut", tagrd.FindForm().Name))
                {
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                    listSettingID += "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy 23:59:59");
                }
                else if (GFunc.CompareString("frmDocListPOOut", tagrd.FindForm().Name))
                {
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                    listSettingID += "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy 23:59:59");
                }
                if (GFunc.CompareString(tagrd.FindForm().Name, "frmDocSearch"))
                {
                    TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("FromDate", true)[0]);
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("ToDate", true)[0]);
                    listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                    listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                }
                if (GFunc.CompareString(tagrd.FindForm().Name, "frmAssemblyEntry"))
                {
                    Infragistics.Win.Misc.UltraLabel ItmID = ((Infragistics.Win.Misc.UltraLabel)tagrd.FindForm().Controls.Find("ItmID", true)[0]);
                    listSettingID += "%" + ItmID.Text;

                }
                string[] ListParameters = listSettingID.Split('%');
                SYSFormSettingIDDet objFormSettingDet = null;
                if (ListParameters.Length > 0)
                {
                    //Use index 0 to List Setting.
                    objListSettingDet = SYSFormSettingID.Get(ListParameters[0]);
                    objFormSettingDet = SYSFormSettingIDDet.Get(ListParameters[0]);
                }
                else
                    objListSettingDet = SYSFormSettingID.Get(listSettingID);

                DataTable dtdet = (DataTable)objFormSettingDet;

                if (!GFunc.IsNE(objListSettingDet))
                {
                    for (int i = 1; i < ListParameters.Length; i++)
                    {
                        string oldVal = "{" + (i - 1) + "}";
                        objListSettingDet.ListSQL = objListSettingDet.ListSQL.Replace(oldVal, ListParameters[i]);
                    }
                }
                if (objListSettingDet.ListSQL != "")
                {
                    if (objListSettingDet.ListSQL != string.Empty || objListSettingDet.ListSQL != null)
                    {
                        tagrd.DataSource = GFunc.ExecuteQuery(objListSettingDet.ListSQL);
                    }
                    else
                    {
                        throw Error(new Exception("ERROR: The list SQL is has not been set in database"), false, false);
                    }
                }
                if (dtdet == null)
                {
                    throw Error(new Exception("FormSettingIDDet haven't filled!"), false, false);
                }
                else
                {
                    switch (listSettingID)
                    {
                        default:
                            Grid_Format(tagrd, dtdet, false);
                            break;
                    }
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void Grid_Format(TAUtil.TAGridEditor tagrd, string[] colHeadersItmDet, string[] colWidthsItmDet, string[] colFormatsItmDet, bool[] colTabStop, string colDataFormat, bool readOnly, double rowHeightCM)
        {
            try
            {
                int defaultWidth = 100;
                int c = tagrd.DisplayLayout.Bands[0].Columns.Count;

                int totalColWidth = 0;
                double pixel = (96 * rowHeightCM) / 2.54;
                if(tagrd.Rows.Count>0)
                    tagrd.Rows[0].Height = GFunc.NEInt(pixel,0);

                for (int i = 0; i < c; i++)
                {
                    // Check column width 0 which is temporarily hidden & Check -99 included column which is never shown to user
                    if (tagrd.DisplayLayout.Bands[0].Columns[i].Key.ToLower() == "itmdeptkey"
                       || tagrd.DisplayLayout.Bands[0].Columns[i].Key.ToLower() == "expdeptkey"
                       || tagrd.DisplayLayout.Bands[0].Columns[i].Key.ToLower() == "detitmdeptkey"
                       || tagrd.DisplayLayout.Bands[0].Columns[i].Key.ToLower() == "itmdocdeptkey")// added by jane on 10-Jun-2014 - if usedepartment option is false, hide department combo in grid.
                    {
                        if (SysOptionUtility.GetBool("UseDept") == false)
                            tagrd.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                        else
                            tagrd.DisplayLayout.Bands[0].Columns[i].Hidden = false;
                    }
                    else
                    {
                        tagrd.DisplayLayout.Bands[0].Columns[i].Hidden = ((i < colWidthsItmDet.Length) ? (colWidthsItmDet[i].Equals("0") || ((i < colHeadersItmDet.Length) ? colHeadersItmDet[i].Contains("-99") : false)) : false || (i < colHeadersItmDet.Length) ? colHeadersItmDet[i].Contains("-99") : false) ? true : false;
                    }
                    tagrd.DisplayLayout.Bands[0].Columns[i].Header.Caption = (i < colHeadersItmDet.Length) && colHeadersItmDet[i] != string.Empty ? colHeadersItmDet[i] : tagrd.DisplayLayout.Bands[0].Columns[i].Key.ToString(); //Set Default if Over
                    tagrd.DisplayLayout.Bands[0].Columns[i].Width = (i < colWidthsItmDet.Length) && colWidthsItmDet[i] != string.Empty ? Convert.ToInt16(colWidthsItmDet[i]) : defaultWidth;//Set Default if Over                
                    tagrd.DisplayLayout.Bands[0].Columns[i].TabStop = (i < colWidthsItmDet.Length) && colTabStop[i] != null ? colTabStop[i] : true;//Set Default if Over                

                    if (!tagrd.DisplayLayout.Bands[0].Columns[i].Hidden)
                        totalColWidth += tagrd.DisplayLayout.Bands[0].Columns[i].Width;
                    //tagrd.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition = (i < colFormatsItmDet.Length) && colFormatsItmDet[i] != string.Empty ? Convert.ToInt32(colFormatsItmDet[i]) : i;//Set Default if Over                                       
                    if (readOnly)
                    {
                        tagrd.DisplayLayout.Bands[0].Columns[i].CellActivation = Activation.ActivateOnly;
                    }
                }

                #region Set Visible Position
                //clear array and assign to another blank array and sort according to position
                DataTable dt = new DataTable();
                dt.Columns.Add("Index", typeof(int));
                dt.Columns.Add("Position", typeof(int));
                dt.PrimaryKey = new DataColumn[] { dt.Columns["Position"] };
                for (int i = 0; i < colFormatsItmDet.Length; i++)
                {
                    dt.Rows.Add(i, Convert.ToInt32(colFormatsItmDet[i]));
                }
                dt.DefaultView.Sort = "Position";
                foreach (DataRowView row in dt.DefaultView)
                {
                    tagrd.DisplayLayout.Bands[0].Columns[Convert.ToInt32(row["Index"])].Header.VisiblePosition = Convert.ToInt32(row["Position"]);
                }
                #endregion

                #region Set Data Format Of Grid Columns
                DataTable dtDataFormat = new DataTable();
                dtDataFormat.Columns.Add("FieldNm", typeof(string));
                dtDataFormat.Columns.Add("Format", typeof(string));
                string[] aryDataFormat = colDataFormat.Split('@');
                if (GFunc.IsNE(aryDataFormat[0]) == false)
                    foreach (string fieldNmFormat in aryDataFormat)
                    {   //FieldNm,Format Number
                        string format = string.Empty;
                        switch (Convert.ToInt16(fieldNmFormat.Split(':')[1]))
                        {
                            case 2: format = "#,##0.00"; break;
                            case 4: format = "#,##0.00##"; break;
                            case 6: format = "#,##0.00####"; break;
                            case 8: format = "#,##0.00######"; break;
                            case 0: format = "#,##0"; break;
                            default:
                                break;


                        }
                        dtDataFormat.Rows.Add(new object[] { fieldNmFormat.Split(':')[0], format });
                    }

                foreach (UltraGridColumn col in tagrd.DisplayLayout.Bands[0].Columns)
                {
                    DataRow[] rowFound = dtDataFormat.Select("FieldNm='" + col.Key + "'");
                    if (rowFound.Length > 0)
                    {
                        string format = rowFound[0]["Format"].ToString();

                        if (format.Trim().StartsWith("?"))
                        {
                            col.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null; //null store as DBNull
                            format = format.Trim().Replace("?", "");
                        }
                        col.Format = format;
                    }

                }

                #endregion

                if (tagrd.Parent.GetType() == typeof(SplitterPanel) && (((SplitterPanel)tagrd.Parent)).Location.X < 100//Checking left or right side panel, Left Panel loction is always 0, less than 100
                && ((System.Windows.Forms.SplitContainer)(((System.Windows.Forms.SplitterPanel)(tagrd.Parent)).Parent)).Orientation == Orientation.Vertical)
                {
                    ((SplitContainer)(tagrd.Parent.Parent)).SplitterDistance = totalColWidth;
                }

                //Set Appearence
                //Header
                Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
                appearence_Header.AlphaLevel = ((short)(255));
                appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.FontData.ItalicAsString = "True";
                appearence_Header.FontData.Name = "Calibri";
                appearence_Header.FontData.SizeInPoints = 10F;
                appearence_Header.ForeColor = System.Drawing.Color.Black;
                // appearence_Header.TextHAlignAsString = "LEFT";
                tagrd.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrd.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrd.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrd.DisplayLayout.Appearance = appearence;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrd.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                tagrd.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;


                tagrd.TextRenderingMode = TextRenderingMode.GDI;
                // tagrd.AddCustomControlsInCells();
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        //----New Grid_Format----//
        /*Formt the grid's cells accoding to the settings in the formsetting tables including colors, number precision etc
         This function will be called from FormGrid_Set function which formats control other than grid
         */
        public static void Grid_Format(TAUtil.TAGridEditor grd, DataView dvDet, bool readOnly)
        {
            try
            {
                bool hidden = false;
                string header = string.Empty;
                int width = 0;
                string format = string.Empty;
                string alignment = string.Empty;

                int totalColWidth = 0;

                //hide all grid column before format - pauk
                //foreach (UltraGridColumn dc in grd.DisplayLayout.Bands[0].Columns)
                //{
                //    dc.Hidden = true;
                //}

                foreach (DataRowView dr in dvDet)
                {
                    if (grd.DisplayLayout.Bands[0].Columns.Exists(GFunc.NEStr(dr["ColFldNm"], "")))
                    {
                        UltraGridColumn c = grd.DisplayLayout.Bands[0].Columns[GFunc.NEStr(dr["ColFldNm"], "")];
                        header = GFunc.NEStr(dr["ColHeader"], c.Key);
                        hidden = GFunc.NEBool(dr["ColHide"], false);

                        width = GFunc.NEInt(dr["ColWidth"], 0);
                        format = GFunc.NEStr(dr["ColDataFormat"], "");
                        alignment = GFunc.NEStr(dr["ColAlignment"], "");


                        c.Hidden = hidden || (width == 0);
                        c.Header.Caption = header;
                        c.Width = width;
                        if (!c.Hidden)
                            totalColWidth += c.Width;
                        //Format string is from the FormSetting tables
                        if (format != string.Empty)
                        {
                            if (format.Trim().StartsWith("?"))
                            {
                                c.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null; //null store as DBNull

                                format = format.Trim().Replace("?", "");
                            }
                            else
                                c.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow;
                            c.Format = format;
                        }

                        if (alignment != string.Empty)
                        {
                            switch (alignment.ToLower())
                            {
                                case "l":
                                    c.CellAppearance.TextHAlign = HAlign.Left;
                                    break;
                                case "r":
                                    c.CellAppearance.TextHAlign = HAlign.Right;
                                    break;
                                case "c":
                                    c.CellAppearance.TextHAlign = HAlign.Center;
                                    break;
                            }
                            c.Header.Appearance = c.CellAppearance;
                        }

                        if (readOnly)
                        {
                            c.CellActivation = Activation.ActivateOnly;
                            if (c.DataType == typeof(bool))
                            {
                                c.CellAppearance.BackColorDisabled = System.Drawing.Color.Gray;
                            }
                        }

                        if (GFunc.NEInt(dr["ColPosition"], 0) > 0) //if dr["ColPosition"] is DBNull, will not change Visible Position
                            c.Header.VisiblePosition = Convert.ToInt32(dr["ColPosition"]);

                        c.TabStop = Convert.ToBoolean(GFunc.NEBool(dr["ColTabStop"], true));
                    }
                }

                if (grd.Parent.GetType() == typeof(SplitterPanel) && (((SplitterPanel)grd.Parent)).Location.X < 100//Checking left or right side panel, Left Panel loction is always 0, less than 100
                   && ((System.Windows.Forms.SplitContainer)(((System.Windows.Forms.SplitterPanel)(grd.Parent)).Parent)).Orientation == Orientation.Vertical)
                {
                    ((SplitContainer)(grd.Parent.Parent)).SplitterDistance = totalColWidth;
                }

                //Set Appearence
                //Header
                Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
                appearence_Header.AlphaLevel = ((short)(255));
                appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.FontData.ItalicAsString = "True";
                appearence_Header.FontData.Name = "Calibri";
                appearence_Header.FontData.SizeInPoints = 10F;
                appearence_Header.ForeColor = System.Drawing.Color.Black;
                //appearence_Header.TextHAlignAsString = "LEFT";
                grd.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;

                //Active Row Appearence            
                grd.DisplayLayout.Override.ActiveRowAppearance = null;

                //Selected Row Appearence
                Infragistics.Win.Appearance appearence_SelectedRow = new Infragistics.Win.Appearance();
                appearence_SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
                appearence_SelectedRow.BackColor2 = SystemColors.Window;
                appearence_SelectedRow.ForeColor = Color.White;
                //appearence_SelectedRow.BackGradientStyle = GradientStyle.GlassBottom50Bright;
                grd.DisplayLayout.Override.SelectedRowAppearance = appearence_SelectedRow;

                grd.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                grd.TextRenderingMode = TextRenderingMode.GDI;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }


        }


        //----New Grid_Format----//
        /*Formt the grid's cells accoding to the settings in the formsetting tables including colors, number precision etc
         This function will be called from FormGrid_Set function which formats control other than grid
         */
        public static void Grid_Format(TAUtil.TAGridEditor grd, DataTable dtDet, bool readOnly)
        {
            try
            {
                bool hidden = false;
                string header = string.Empty;
                int width = 0;
                string format = string.Empty;
                string alignment = string.Empty;

                int totalColWidth = 0;

                //hide all grid column before format - pauk
                foreach (UltraGridColumn dc in grd.DisplayLayout.Bands[0].Columns)
                {
                    dc.Hidden = true;
                }

                foreach (DataRow dr in dtDet.Rows)
                {
                    if (grd.DisplayLayout.Bands[0].Columns.Exists(GFunc.NEStr(dr["ColFldNm"], "")))
                    {
                        UltraGridColumn c = grd.DisplayLayout.Bands[0].Columns[GFunc.NEStr(dr["ColFldNm"], "")];
                        header = GFunc.NEStr(dr["ColHeader"], c.Key);
                        hidden = GFunc.NEBool(dr["ColHide"], false);

                        width = GFunc.NEInt(dr["ColWidth"], 0);
                        format = GFunc.NEStr(dr["ColDataFormat"], "");
                        alignment = GFunc.NEStr(dr["ColAlignment"], "");


                        c.Hidden = hidden || (width == 0);
                        c.Header.Caption = header;
                        c.Width = width;
                        if (!c.Hidden)
                            totalColWidth += c.Width;
                        //Format string is from the FormSetting tables
                        if (format != string.Empty)
                        {
                            if (format.Trim().StartsWith("?"))
                            {
                                c.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null; //null store as DBNull

                                format = format.Trim().Replace("?", "");
                            }
                            else
                                c.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow;
                            c.Format = format;
                        }

                        if (alignment != string.Empty)
                        {
                            switch (alignment.ToLower())
                            {
                                case "l":
                                    c.CellAppearance.TextHAlign = HAlign.Left;
                                    break;
                                case "r":
                                    c.CellAppearance.TextHAlign = HAlign.Right;
                                    break;
                                case "c":
                                    c.CellAppearance.TextHAlign = HAlign.Center;
                                    break;
                            }
                            c.Header.Appearance = c.CellAppearance;
                        }

                        if (readOnly)
                        {
                            c.CellActivation = Activation.ActivateOnly;
                            if (c.DataType == typeof(bool))
                            {
                                c.CellAppearance.BackColorDisabled = System.Drawing.Color.Gray;
                            }
                        }

                        if (GFunc.NEInt(dr["ColPosition"], 0) > 0) //if dr["ColPosition"] is DBNull, will not change Visible Position
                            c.Header.VisiblePosition = Convert.ToInt32(dr["ColPosition"]);

                        c.TabStop = Convert.ToBoolean(GFunc.NEBool(dr["ColTabStop"], true));
                    }
                }

                if (grd.Parent.GetType() == typeof(SplitterPanel) && (((SplitterPanel)grd.Parent)).Location.X < 100//Checking left or right side panel, Left Panel loction is always 0, less than 100
                   && ((System.Windows.Forms.SplitContainer)(((System.Windows.Forms.SplitterPanel)(grd.Parent)).Parent)).Orientation == Orientation.Vertical)
                {
                    ((SplitContainer)(grd.Parent.Parent)).SplitterDistance = totalColWidth;
                }

                //Set Appearence
                //Header
                Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
                appearence_Header.AlphaLevel = ((short)(255));
                appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.FontData.ItalicAsString = "True";
                appearence_Header.FontData.Name = "Calibri";
                appearence_Header.FontData.SizeInPoints = 10F;
                appearence_Header.ForeColor = System.Drawing.Color.Black;
                //appearence_Header.TextHAlignAsString = "LEFT";
                grd.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;

                //Active Row Appearence            
                grd.DisplayLayout.Override.ActiveRowAppearance = null;

                //Selected Row Appearence
                Infragistics.Win.Appearance appearence_SelectedRow = new Infragistics.Win.Appearance();
                appearence_SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
                appearence_SelectedRow.BackColor2 = SystemColors.Window;
                appearence_SelectedRow.ForeColor = Color.White;
                //appearence_SelectedRow.BackGradientStyle = GradientStyle.GlassBottom50Bright;
                grd.DisplayLayout.Override.SelectedRowAppearance = appearence_SelectedRow;

                grd.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                grd.TextRenderingMode = TextRenderingMode.GDI;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }


        }

        public static void Grid_FormatAppearance(TAUtil.TAGridEditor grd)
        {
            try
            {
                //Set Appearence
                //Header
                Infragistics.Win.Appearance appearence_Header = new Infragistics.Win.Appearance();
                appearence_Header.AlphaLevel = ((short)(255));
                appearence_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Header.FontData.ItalicAsString = "True";
                appearence_Header.FontData.Name = "Calibri";
                appearence_Header.FontData.SizeInPoints = 10F;
                appearence_Header.ForeColor = System.Drawing.Color.Black;
                //appearence_Header.TextHAlignAsString = "LEFT";
                grd.DisplayLayout.Override.HeaderAppearance = appearence_Header;

                //Row
                Infragistics.Win.Appearance appearence_Row = new Infragistics.Win.Appearance();
                appearence_Row.FontData.Name = "Calibri";
                appearence_Row.FontData.SizeInPoints = 11F;
                appearence_Row.ForeColor = System.Drawing.Color.Black;
                appearence_Row.TextHAlignAsString = "LEFT";
                appearence_Row.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Row.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowAppearance = appearence_Row;

                //Cell
                Infragistics.Win.Appearance appearence_Cell = new Infragistics.Win.Appearance();
                appearence_Cell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_Cell.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.CellAppearance = appearence_Cell;

                //Row Selector
                Infragistics.Win.Appearance appearence_RowSelector = new Infragistics.Win.Appearance();
                appearence_RowSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorAppearance = appearence_RowSelector;

                //Appearence
                Infragistics.Win.Appearance appearence = new Infragistics.Win.Appearance();
                appearence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Appearance = appearence;

                //Row Header Selector
                Infragistics.Win.Appearance appearence_RowHeaderSelector = new Infragistics.Win.Appearance();
                appearence_RowHeaderSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                appearence_RowHeaderSelector.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
                grd.DisplayLayout.Override.RowSelectorHeaderAppearance = appearence_RowHeaderSelector;

                //Active Row Appearence            
                grd.DisplayLayout.Override.ActiveRowAppearance = null;

                //Selected Row Appearence
                Infragistics.Win.Appearance appearence_SelectedRow = new Infragistics.Win.Appearance();
                appearence_SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
                appearence_SelectedRow.BackColor2 = SystemColors.Window;
                appearence_SelectedRow.ForeColor = Color.White;
                //appearence_SelectedRow.BackGradientStyle = GradientStyle.GlassBottom50Bright;
                grd.DisplayLayout.Override.SelectedRowAppearance = appearence_SelectedRow;

                grd.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
                grd.TextRenderingMode = TextRenderingMode.GDI;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }


        }
        public static void Grid_Format(SqlConnection cn, TAUtil.TAGridEditor tagrd, string listSettingID, bool fillData)
        {
            try
            {
                SYSFormSettingID objListSettingDet;
                switch (listSettingID.ToLower())
                {
                    case "frmlistgridconcust":
                    case "frmlistgridconvend":
                    case "frmlistgriditm":
                    case "frmlistgridjob":
                    case "frmlistgridacc":
                    case "frmlistgridalert":
                    case "frmlistgridprice":
                    case "frmlistgridshipname":
                    case "frmlistgridtodo":
                    case "frmglfinchargegrid":
                        listSettingID += "%%%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                        break;
                }
                if (GFunc.CompareString(tagrd.FindForm().Name, "frmDocList"))
                {
                    TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateFrom", true)[0]);
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                    listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                    listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                }
                else if (GFunc.CompareString("frmDocListSOOut", tagrd.FindForm().Name))
                {
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                    listSettingID += "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy 23:59:59");
                }
                else if (GFunc.CompareString("frmDocListPOOut", tagrd.FindForm().Name))
                {
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("pDateTo", true)[0]);
                    listSettingID += "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy 23:59:59");
                }
                if (GFunc.CompareString(tagrd.FindForm().Name, "frmDocSearch"))
                {
                    TADateEditor dtFrom = ((TADateEditor)tagrd.FindForm().Controls.Find("FromDate", true)[0]);
                    TADateEditor dtTo = ((TADateEditor)tagrd.FindForm().Controls.Find("ToDate", true)[0]);
                    listSettingID += "%" + AppInfor.ConAccessLevel + "%" + AppInfor.CurrentUserKey;
                    listSettingID += "%" + dtFrom.DateValue.Value.ToString("d-MMM-yyyy") + "%" + dtTo.DateValue.Value.ToString("d-MMM-yyyy");
                }
                string[] ListParameters = listSettingID.Split('%');
                SYSFormSettingIDDet objFormSettingDet = null;
                if (ListParameters.Length > 0)
                {
                    //Use index 0 to List Setting.
                    objListSettingDet = SYSFormSettingID.GetFormList(cn, ListParameters[0]);
                    objFormSettingDet = SYSFormSettingIDDet.GetFormList(cn, ListParameters[0]);
                }
                else
                    objListSettingDet = SYSFormSettingID.GetFormList(cn, listSettingID);


                DataTable dtdet = (DataTable)objFormSettingDet;



                if (!GFunc.IsNE(objListSettingDet))
                {
                    for (int i = 1; i < ListParameters.Length; i++)
                    {
                        string oldVal = "{" + (i - 1) + "}";
                        objListSettingDet.ListSQL = objListSettingDet.ListSQL.Replace(oldVal, ListParameters[i]);
                    }
                }

                if (objListSettingDet.ListSQL != "")
                {
                    if (objListSettingDet.ListSQL != string.Empty || objListSettingDet.ListSQL != null)
                    {
                        tagrd.DataSource = GFunc.ExecuteQuery(cn, objListSettingDet.ListSQL);
                    }
                    else
                    {
                        throw Error(new Exception("ERROR: The list SQL is has not been set in database"), false, false);
                    }
                }

                if (dtdet == null)
                {
                    throw Error(new Exception("FormSettingIDDet haven't filled!"), false, false);
                }
                else
                {
                    switch (listSettingID)
                    {
                        default:
                            Grid_Format(tagrd, dtdet, false);
                            break;
                    }

                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        private static string CheckAndGetLocalFormSetting(int codeKey,string frmName,bool reloadRefCombo)
        {
            string settings = "";           
            string filePath =Application.StartupPath+ @"\FormSettings\"+ SysOptionUtility.DatabaseBranchCode + @"\" + frmName + ".txt";
            

            if (!File.Exists(filePath) || reloadRefCombo)
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CodeKey", codeKey));
                list.Add(new SqlParameter("@FormName", frmName));
                list.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                settings = GFunc.ExecuteScalar("SYSFormSetting_GetJSon", list);
                
                //string[] settingsArr = settings.Split(new string[]{ "@@#@@" },StringSplitOptions.None);

                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                File.WriteAllText(filePath, settings);

                /*if (settingsArr.Length > 0)
                {                    
                    File.WriteAllText(filePath, settingsArr[0]);
                }

                if (settingsArr.Length > 1)
                {
                    File.WriteAllText(Path.GetDirectoryName(filePath) + @"\MSTNREF.txt", settingsArr[1]);
                }*/
            }            
            /*else if(reloadRefCombo)
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CodeKey", codeKey));
                list.Add(new SqlParameter("@FormName", frmName));
                list.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                string refdata = GFunc.ExecuteScalar("SYSFormSetting_GetMstRefJson", list);
                File.WriteAllText(Path.GetDirectoryName(filePath) + @"\MSTNREF.txt", refdata);
                settings = File.ReadAllText(filePath) + "@@#@@" + refdata;
            }*/
            else
            {
                settings = File.ReadAllText(filePath);
            }

            settings = "{" + settings + "}";

            return settings;
        }

        //Copy FormGrids_Set function and modified by May on 14-Feb-2024 to reduce database traffics
        public static bool FormGrids_SetNew(Form frm, int codeKey,bool reloadRefCombos, out string ContextMenu)
        {
            string frmName = frm.Name;
            ContextMenu = string.Empty;
            int option = 0;
            if (codeKey == 0)
                option = 2;

            try
            {
                string settings = CheckAndGetLocalFormSetting(codeKey,frmName, reloadRefCombos);                

                DataSet dsAllSettings = new DataSet();
                
                try
                {
                    dsAllSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(settings);
                }
                catch(Exception ex)
                {
                    return false;
                }                

                DataRow[] drs = dsAllSettings.Tables["frmSetting"].Select("BindingFieldNm='0GF'");//Filter Grids

                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string listSettingID = dr["ListSettingID"].ToString().Trim();
                        if (listSettingID == string.Empty) continue;

                        if (frm.Controls.Find(dr["GridNm"].ToString(), true).Length > 0)
                        {
                            //Find Grid in Form
                            TAGridEditor grd = (TAGridEditor)frm.Controls.Find(dr["GridNm"].ToString(), true)[0];
                            //string font = dr["ControlFont"].ToString().Trim();
                            //float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"].ToString(), 0);
                            //if (font.Length > 0)
                            //    grd.Font = new Font(font, fontSize);

                            grd.ContextMenuStrip = cmnuGlobal;

                            if (dsAllSettings.Tables.Contains(listSettingID))
                            {
                                if (dsAllSettings.Tables[listSettingID].Rows.Count > 0)
                                    grd.DataSource = dsAllSettings.Tables[listSettingID];
                            }

                            dsAllSettings.Tables["frmSettingGrids"].DefaultView.RowFilter = "ListID = '" + listSettingID + "'";
                            dsAllSettings.Tables["frmSettingGrids"].DefaultView.Sort = "ColPosition";
                            //dsAllSettings.Tables["frmSettingGrids"].PrimaryKey = new DataColumn[] { dsAllSettings.Tables["frmSettingGrids"].Columns["ListID"], dsAllSettings.Tables["frmSettingGrids"].Columns["ColFldNm"] };

                            //using (DataTable dt = dsAllSettings.Tables["frmSettingGrids"].DefaultView.ToTable())
                            //{
                                Grid_Format(grd, dsAllSettings.Tables["frmSettingGrids"].DefaultView, false);

                                //Set Grid Controls and format
                                GridControl_SetNew(grd, dsAllSettings.Tables["frmSettingGrids"].DefaultView, dsAllSettings, codeKey);
                            //}
                            //Commented by May on 16-Feb-2024, The combined ContextMenu for all grids are assigned after this for loop
                            //append contextMenu for each grid
                            // ContextMenu += ContextMenuSettingGrid_GetNew(grd.Name, listSettingID);                               

                        }//End ForEach
                    }
                    //Added by May on 16 - Feb - 2024, The combined ContextMenu for all grids 
                    if (dsAllSettings.Tables["frmGridContextMenuSetting"].Rows.Count > 0)
                        ContextMenu = GFunc.NEStr(dsAllSettings.Tables["frmGridContextMenuSetting"].Rows[0][0], "");
                }// End if Filter Grids

                if (dsAllSettings.Tables["frmCtrlsContextMenuSetting"].Rows.Count > 0)
                    ContextMenu += GFunc.NEStr(dsAllSettings.Tables["frmCtrlsContextMenuSetting"].Rows[0][0], "");

                drs = dsAllSettings.Tables["frmSetting"].Select("GridNm='NA'");

                if (drs.Length > 0)
                    {
                        foreach (DataRow dr in drs)
                        {
                            //string font = GFunc.NEStr(dr["ControlFont"], "");
                            //float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"], 0);
                            //bool fontEnable = GFunc.NEBool(dr["ControlFontEnabled"], false);
                            string alignment = GFunc.NEStr(dr["ControlAlignment"], "");
                            string format = GFunc.NEStr(dr["ControlFormat"], "");
                            //bool inActive = GFunc.NEBool(dr["InActive"], false);
                            bool IsMultiline = GFunc.NEBool(dr["ControlMultiline"], false);

                            if (frm.Controls.Find(dr["ControlNm"].ToString(), true).Length > 0)
                            {
                                string ctrlType = GFunc.NEStr(dr["ControlType"], "");
                                

                                switch (ctrlType.ToLower())
                                {
                                    case "tautil.tatextboxeditor":
                                        string aa = dr["ControlNm"].ToString();
                                        TATextBoxEditor tatxt = (TAUtil.TATextBoxEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        //if (fontEnable)
                                        //{
                                        //    if (font.Length > 0)
                                        //        tatxt.Font = new Font(font, fontSize);
                                        //}
                                        tatxt.Multiline = IsMultiline;
                                        tatxt.TextRenderingMode = TextRenderingMode.GDI;
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            tatxt.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tadateeditor":
                                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];
                                        if (format != string.Empty)
                                        {
                                            taDate.Format = format;

                                            if (taDate.DateValue != null)
                                                taDate.Text = taDate.DateValue.Value.ToString(format);//to format the existing value especially for unbound Date Controls

                                            if (taDate.DataBindings.Count > 0)
                                                taDate.DataBindings[0].FormatString = format;
                                        }
                                        taDate.TextRenderingMode = TextRenderingMode.GDI;
                                        taDate.Font = new Font("Calibri", 11);
                                        taDate.Appearance.BackColor = Color.White;
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            taDate.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tanumericeditor":

                                        TANumericEditor tanum = (TAUtil.TANumericEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];


                                        //if (fontEnable)
                                        //{
                                        //    if (font.Length > 0)
                                        //        tanum.Font = new Font(font, fontSize);
                                        //}

                                        #region Alignment
                                        if (alignment != string.Empty)
                                        {
                                            switch (alignment.ToLower())
                                            {
                                                case "l":
                                                    tanum.Appearance.TextHAlign = HAlign.Left;
                                                    break;
                                                case "r":
                                                    tanum.Appearance.TextHAlign = HAlign.Right;
                                                    break;
                                                case "c":
                                                    tanum.Appearance.TextHAlign = HAlign.Center;
                                                    break;
                                                default:
                                                    tanum.Appearance.TextHAlign = HAlign.Default;
                                                    break;
                                            }

                                        }

                                        #endregion

                                        #region Format
                                        if (format != string.Empty)
                                        {
                                            if (format.Trim().StartsWith("?"))
                                            {
                                                tanum.Nullable = true;
                                                format = format.Trim().Replace("?", "");
                                            }
                                            else
                                            {
                                                tanum.Nullable = false;
                                                tanum.NullText = decimal.Parse("0.0").ToString(format);
                                            }
                                            tanum.Format = format;
                                            if (tanum.DataBindings.Count > 0)
                                                tanum.DataBindings[0].FormatString = format;

                                        }
                                        tanum.TextRenderingMode = TextRenderingMode.GDI;
                                        tanum.Font = new Font("Calibri", 11);
                                        tanum.Appearance.BackColor = Color.White;
                                        #endregion

                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            tanum.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tacombobox":
                                        TAUtil.TAComboBox tacbo = (TAUtil.TAComboBox)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            tacbo.ContextMenuStrip = cmnuGlobal;
                                        }
                                    if (GFunc.NEStr(dr["ListSettingID"], "").ToLower() == "refaddrbyCon")
                                        continue;

                                        BindComboValueNew(tacbo, dr, dsAllSettings,codeKey);
                                        break;


                                }//End switch                        


                            }//End if


                        }//End ForEach
                    }
                return true;    
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        //----Form & Grid together format ----//
        /*Formats the control outside of the grid and also format the grid which is accomplished by calling Grid_Format function; meaning that 
         this function formats the entire form including all controls */
        public static void FormGrids_Set(Form frm, int codeKey, out string ContextMenu)
        {
            string frmName = frm.Name;

            ContextMenu = string.Empty;
            int option = 0;
            if (codeKey == 0)
                option = 2;

            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Option", option));
                list.Add(new SqlParameter("@CodeKey", codeKey));
                list.Add(new SqlParameter("@FormName", frmName));
                DataTable dtGrdList = GFunc.ExecuteProc("SYSFormSetting_Get", list);

                DataRow[] drs = dtGrdList.Select("BindingFieldNm='0GF'");//Filter Grids

                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {

                        string listSettingID = dr["ListSettingID"].ToString().Trim();
                        if (listSettingID == string.Empty) continue;

                        if (frm.Controls.Find(dr["GridNm"].ToString(), true).Length > 0)
                        {
                            //Find Grid in Form
                            TAGridEditor grd = (TAGridEditor)frm.Controls.Find(dr["GridNm"].ToString(), true)[0];
                            string font = dr["ControlFont"].ToString().Trim();
                            float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"].ToString(), 0);
                            if (font.Length > 0)
                                grd.Font = new Font(font, fontSize);

                            grd.ContextMenuStrip = cmnuGlobal;
                            //Format Grid  //Here grid column's format (eg.#,###.00##) is done regardless of control type
                            GlobalUI.Grid_Format(grd, listSettingID, false, false);

                            //Set Grid Controls and format
                            GridControl_Set(grd, listSettingID, codeKey,frm);
                            // grd.AddCustomControlsInCells();
                            //append contextMenu for each grid
                            ContextMenu += ContextMenuSettingGrid_GetNew(grd.Name, listSettingID);
                        }

                    }//End ForEach
                }
                drs = dtGrdList.Select("GridNm='NA'");//Filter Controls
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {

                        string font = GFunc.NEStr(dr["ControlFont"], "");
                        float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"], 0);
                        bool fontEnable = GFunc.NEBool(dr["ControlFontEnabled"], false);
                        string alignment = GFunc.NEStr(dr["ControlAlignment"], "");
                        string format = GFunc.NEStr(dr["ControlFormat"], "");
                        bool inActive = GFunc.NEBool(dr["InActive"], false);
                        bool IsMultiline = GFunc.NEBool(dr["ControlMultiline"], false);

                        if (!inActive)
                        {
                            if (frm.Controls.Find(dr["ControlNm"].ToString(), true).Length > 0)
                            {
                                string ctrlType = GFunc.NEStr(dr["ControlType"], "");


                                switch (ctrlType.ToLower())
                                {
                                    case "tautil.tatextboxeditor":
                                        string aa = dr["ControlNm"].ToString();
                                        TATextBoxEditor tatxt = (TAUtil.TATextBoxEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tatxt.Font = new Font(font, fontSize);
                                        }
                                        tatxt.Multiline = IsMultiline;
                                        tatxt.TextRenderingMode = TextRenderingMode.GDI;
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            tatxt.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tadateeditor":
                                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];
                                        if (format != string.Empty)
                                        {
                                            taDate.Format = format;

                                            if (taDate.DateValue != null)
                                                taDate.Text = taDate.DateValue.Value.ToString(format);//to format the existing value especially for unbound Date Controls

                                            if (taDate.DataBindings.Count > 0)
                                                taDate.DataBindings[0].FormatString = format;
                                        }
                                        taDate.TextRenderingMode = TextRenderingMode.GDI;
                                        taDate.Font = new Font("Calibri", 11);
                                        taDate.Appearance.BackColor = Color.White;
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            taDate.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tanumericeditor":

                                        TANumericEditor tanum = (TAUtil.TANumericEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];


                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tanum.Font = new Font(font, fontSize);
                                        }

                                        #region Alignment
                                        if (alignment != string.Empty)
                                        {
                                            switch (alignment.ToLower())
                                            {
                                                case "l":
                                                    tanum.Appearance.TextHAlign = HAlign.Left;
                                                    break;
                                                case "r":
                                                    tanum.Appearance.TextHAlign = HAlign.Right;
                                                    break;
                                                case "c":
                                                    tanum.Appearance.TextHAlign = HAlign.Center;
                                                    break;
                                                default:
                                                    tanum.Appearance.TextHAlign = HAlign.Default;
                                                    break;
                                            }

                                        }

                                        #endregion

                                        #region Format
                                        if (format != string.Empty)
                                        {
                                            if (format.Trim().StartsWith("?"))
                                            {
                                                tanum.Nullable = true;
                                                format = format.Trim().Replace("?", "");
                                            }
                                            else
                                            {
                                                tanum.Nullable = false;
                                                tanum.NullText = decimal.Parse("0.0").ToString(format);
                                            }
                                            tanum.Format = format;
                                            if (tanum.DataBindings.Count > 0)
                                                tanum.DataBindings[0].FormatString = format;

                                        }
                                        tanum.TextRenderingMode = TextRenderingMode.GDI;
                                        tanum.Font = new Font("Calibri", 11);
                                        tanum.Appearance.BackColor = Color.White;
                                        #endregion

                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            tanum.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tacombobox":
                                        TAUtil.TAComboBox tacbo = (TAUtil.TAComboBox)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            tacbo.ContextMenuStrip = cmnuGlobal;
                                        }

                                        break;


                                }//End switch                        


                            }//End if
                        }//End if

                    }//End ForEach
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void FormGrids_Set(SqlConnection cn, Form frm, int codeKey, out string ContextMenu)
        {
            string frmName = frm.Name;

            ContextMenu = string.Empty;


            int option = 0;

            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Option", option));
                list.Add(new SqlParameter("@CodeKey", codeKey));
                list.Add(new SqlParameter("@FormName", frmName));
                DataTable dtGrdList = GFunc.ExecuteProc(cn, "SYSFormSetting_Get", list);

                DataRow[] drs = dtGrdList.Select("BindingFieldNm='0GF'");//Filter Grids
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string listSettingID = dr["ListSettingID"].ToString().Trim();
                        if (listSettingID == string.Empty) continue;

                        if (frm.Controls.Find(dr["GridNm"].ToString(), true).Length > 0)
                        {
                            //Find Grid in Form
                            TAGridEditor grd = (TAGridEditor)frm.Controls.Find(dr["GridNm"].ToString(), true)[0];
                            string font = dr["ControlFont"].ToString().Trim();
                            float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"].ToString(), 0);
                            if (font.Length > 0)
                                grd.Font = new Font(font, fontSize);

                            //string font = dr["Font"].ToString().Trim();
                            //float fontSize = (float)Convert.ToDecimal(dr["FontSize"].ToString());
                            //if (font.Length > 0)
                            //    grd.Font = new Font(font, fontSize);

                            //Format Grid
                            GlobalUI.Grid_Format(cn, grd, listSettingID, false);

                            //Set Grid Controls and format
                            GridControl_Set(cn, grd, listSettingID, codeKey);
                            // grd.AddCustomControlsInCells();

                            //append contextMenu for each grid
                            ContextMenu += ContextMenuSettingGrid_GetNew(cn, grd.Name, listSettingID);

                        }

                    }//End ForEach
                }

                drs = dtGrdList.Select("GridNm='NA'");//Filter Controls
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string font = GFunc.NEStr(dr["ControlFont"], "");
                        float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"], 0);
                        bool fontEnable = GFunc.NEBool(dr["ControlFontEnabled"], false);
                        string alignment = GFunc.NEStr(dr["ControlAlignment"], "");
                        string format = GFunc.NEStr(dr["ControlFormat"], "");
                        bool inActive = GFunc.NEBool(dr["InActive"], false);
                        bool IsMultiline = GFunc.NEBool(dr["ControlMultiline"], false);
                        if (!inActive)
                        {
                            if (frm.Controls.Find(dr["ControlNm"].ToString(), true).Length > 0)
                            {
                                string ctrlType = GFunc.NEStr(dr["ControlType"], "");


                                switch (ctrlType.ToLower())
                                {
                                    case "tautil.tatextboxeditor":
                                        TATextBoxEditor tatxt = (TAUtil.TATextBoxEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tatxt.Font = new Font(font, fontSize);
                                        }
                                        tatxt.Multiline = IsMultiline;
                                        break;
                                    case "tautil.tadateeditor":
                                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];
                                        if (format != string.Empty)
                                        {
                                            taDate.Format = format;

                                            if (taDate.DateValue != null)
                                                taDate.Text = taDate.DateValue.Value.ToString(format);//to format the existing value especially for unbound Date Controls

                                            if (taDate.DataBindings.Count > 0)
                                                taDate.DataBindings[0].FormatString = format;
                                        }
                                        taDate.TextRenderingMode = TextRenderingMode.GDI;
                                        taDate.Font = new Font("Calibri", 11);
                                        taDate.Appearance.BackColor = Color.White;
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            taDate.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tanumericeditor":
                                        TANumericEditor tanum = (TAUtil.TANumericEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tanum.Font = new Font(font, fontSize);
                                        }

                                        #region Alignment
                                        if (alignment != string.Empty)
                                        {
                                            switch (alignment)
                                            {
                                                case "l":
                                                    tanum.Appearance.TextHAlign = HAlign.Left;
                                                    break;
                                                case "r":
                                                    tanum.Appearance.TextHAlign = HAlign.Right;
                                                    break;
                                                case "c":
                                                    tanum.Appearance.TextHAlign = HAlign.Center;
                                                    break;
                                                default:
                                                    tanum.Appearance.TextHAlign = HAlign.Default;
                                                    break;
                                            }
                                        }
                                        #endregion

                                        #region Format
                                        if (format != string.Empty)
                                        {
                                            if (format.Trim().StartsWith("?"))
                                            {
                                                tanum.Nullable = true;
                                                format = format.Trim().Replace("?", "");
                                            }
                                            tanum.Format = format;
                                            if (tanum.DataBindings.Count > 0)
                                                tanum.DataBindings[0].FormatString = format;
                                        }
                                        #endregion
                                        break;


                                }//End switch                        


                            }//End if
                        }//End if

                    }//End ForEach
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void FormGrids_Set(Form frm, int codeKey, bool readOnly, out string ContextMenu)
        {
            string frmName = frm.Name;

            ContextMenu = string.Empty;


            int option = 0;

            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Option", option));
                list.Add(new SqlParameter("@CodeKey", codeKey));
                list.Add(new SqlParameter("@FormName", frmName));
                DataTable dtGrdList = GFunc.ExecuteProc("SYSFormSetting_Get", list);
                DataRow[] drs = dtGrdList.Select("BindingFieldNm='0GF'");//Filter Grids

                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string listSettingID = dr["ListSettingID"].ToString().Trim();
                        if (listSettingID == string.Empty) continue;

                        if (frm.Controls.Find(dr["GridNm"].ToString(), true).Length > 0)
                        {
                            //Find Grid in Form
                            TAGridEditor grd = (TAGridEditor)frm.Controls.Find(dr["GridNm"].ToString(), true)[0];
                            string font = dr["ControlFont"].ToString().Trim();
                            float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"].ToString(), 0);
                            if (font.Length > 0)
                                grd.Font = new Font(font, fontSize);

                            if (GFunc.CompareString(listSettingID, "frmItmEnquiryGridPending") || GFunc.CompareString(listSettingID, "frmItmEnquiryGridActual") || GFunc.CompareString(listSettingID, "frmItmEnquiryGridPrice"))
                                continue;

                            //Format Grid
                            GlobalUI.Grid_Format(grd, listSettingID, false, readOnly);

                            //Set Grid Controls and format
                            GridControl_Set(grd, listSettingID, codeKey);

                            //append contextMenu for each grid
                            ContextMenu += ContextMenuSettingGrid_GetNew(grd.Name, listSettingID);

                        }

                    }//End ForEach
                }

                drs = dtGrdList.Select("GridNm='NA'");//Filter Controls
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string font = GFunc.NEStr(dr["ControlFont"], "");
                        float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"], 0);
                        bool fontEnable = GFunc.NEBool(dr["ControlFontEnabled"], false);
                        string alignment = GFunc.NEStr(dr["ControlAlignment"], "");
                        string format = GFunc.NEStr(dr["ControlFormat"], "");
                        bool inActive = GFunc.NEBool(dr["InActive"], false);
                        bool IsMultiline = GFunc.NEBool(dr["ControlMultiline"], false);
                        if (!inActive)
                        {
                            if (frm.Controls.Find(dr["ControlNm"].ToString(), true).Length > 0)
                            {
                                string ctrlType = GFunc.NEStr(dr["ControlType"], "");


                                switch (ctrlType.ToLower())
                                {
                                    case "tautil.tatextboxeditor":
                                        TATextBoxEditor tatxt = (TAUtil.TATextBoxEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tatxt.Font = new Font(font, fontSize);
                                        }
                                        tatxt.Multiline = IsMultiline;
                                        break;
                                    case "tautil.tadateeditor":
                                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];
                                        if (format != string.Empty)
                                        {
                                            taDate.Format = format;

                                            if (taDate.DateValue != null)
                                                taDate.Text = taDate.DateValue.Value.ToString(format);//to format the existing value especially for unbound Date Controls

                                            if (taDate.DataBindings.Count > 0)
                                                taDate.DataBindings[0].FormatString = format;

                                        }
                                        taDate.TextRenderingMode = TextRenderingMode.GDI;
                                        taDate.Font = new Font("Calibri", 11);
                                        taDate.Appearance.BackColor = Color.White;
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            taDate.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tanumericeditor":
                                        TANumericEditor tanum = (TAUtil.TANumericEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tanum.Font = new Font(font, fontSize);
                                        }

                                        #region Alignment
                                        if (alignment != string.Empty)
                                        {
                                            switch (alignment.ToLower())
                                            {
                                                case "l":
                                                    tanum.Appearance.TextHAlign = HAlign.Left;
                                                    break;
                                                case "r":
                                                    tanum.Appearance.TextHAlign = HAlign.Right;
                                                    break;
                                                case "c":
                                                    tanum.Appearance.TextHAlign = HAlign.Center;
                                                    break;
                                                default:
                                                    tanum.Appearance.TextHAlign = HAlign.Default;
                                                    break;
                                            }
                                        }
                                        #endregion

                                        #region Format
                                        if (format != string.Empty)
                                        {
                                            if (format.Trim().StartsWith("?"))
                                            {
                                                tanum.Nullable = true;
                                                format = format.Trim().Replace("?", "");
                                            }
                                            tanum.Format = format;
                                            if (tanum.DataBindings.Count > 0)
                                                tanum.DataBindings[0].FormatString = format;
                                        }
                                        #endregion
                                        break;


                                }//End switch                        


                            }//End if
                        }//End if

                    }//End ForEach
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void FormGrids_Set(Form frm, int codeKey, out string ContextMenu, bool fillData)
        {
            string frmName = frm.Name;

            ContextMenu = string.Empty;


            int option = 0;

            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Option", option));
                list.Add(new SqlParameter("@CodeKey", codeKey));
                list.Add(new SqlParameter("@FormName", frmName));
                DataTable dtGrdList = GFunc.ExecuteProc("SYSFormSetting_Get", list);

                DataRow[] drs = dtGrdList.Select("BindingFieldNm='0GF'");//Filter Grids
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        string listSettingID = dr["ListSettingID"].ToString().Trim();
                        if (listSettingID == string.Empty) continue;

                        if (frm.Controls.Find(dr["GridNm"].ToString(), true).Length > 0)
                        {
                            //Find Grid in Form
                            TAGridEditor grd = (TAGridEditor)frm.Controls.Find(dr["GridNm"].ToString(), true)[0];
                            string font = GFunc.NEStr(dr["ControlFont"], "");
                            float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"].ToString(), 0);
                            if (font.Length > 0)
                                grd.Font = new Font(font, fontSize);


                            //Format Grid

                            GlobalUI.Grid_Format(grd, listSettingID, fillData);

                            //Set Grid Controls and format
                            GridControl_Set(grd, listSettingID, codeKey);

                            //append contextMenu for each grid
                            ContextMenu += ContextMenuSettingGrid_GetNew(grd.Name, listSettingID);

                        }

                    }//End ForEach
                }


                drs = dtGrdList.Select("GridNm='NA'");//Filter Controls
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {

                        string font = GFunc.NEStr(dr["ControlFont"], "");
                        float fontSize = (float)GFunc.NEDec(dr["ControlFontSize"], 0);
                        bool fontEnable = GFunc.NEBool(dr["ControlFontEnabled"], false);
                        string alignment = GFunc.NEStr(dr["ControlAlignment"], "");
                        string format = GFunc.NEStr(dr["ControlFormat"], "");
                        bool inActive = GFunc.NEBool(dr["InActive"], false);
                        bool IsMultiline = GFunc.NEBool(dr["ControlMultiline"], false);

                        if (!inActive)
                        {
                            if (frm.Controls.Find(dr["ControlNm"].ToString(), true).Length > 0)
                            {
                                string ctrlType = GFunc.NEStr(dr["ControlType"], "");


                                switch (ctrlType.ToLower())
                                {
                                    case "tautil.tatextboxeditor":
                                        TATextBoxEditor tatxt = (TAUtil.TATextBoxEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tatxt.Font = new Font(font, fontSize);
                                        }
                                        tatxt.Multiline = IsMultiline;
                                        break;
                                    case "tautil.tadateeditor":
                                        TAUtil.TADateEditor taDate = (TAUtil.TADateEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];
                                        if (format != string.Empty)
                                        {
                                            taDate.Format = format;

                                            if (taDate.DateValue != null)
                                                taDate.Text = taDate.DateValue.Value.ToString(format);//to format the existing value especially for unbound Date Controls

                                            if (taDate.DataBindings.Count > 0)
                                                taDate.DataBindings[0].FormatString = format;
                                        }
                                        taDate.TextRenderingMode = TextRenderingMode.GDI;
                                        taDate.Font = new Font("Calibri", 11);
                                        taDate.Appearance.BackColor = Color.White;
                                        if (GFunc.NEStr(dr["ShortCutMenu"], "") != string.Empty && GFunc.NEStr(dr["ShortCutMenu"], "") != "0")
                                        {
                                            taDate.ContextMenuStrip = cmnuGlobal;
                                        }
                                        break;
                                    case "tautil.tanumericeditor":
                                        TANumericEditor tanum = (TAUtil.TANumericEditor)frm.Controls.Find(dr["ControlNm"].ToString(), true)[0];

                                        if (fontEnable)
                                        {
                                            if (font.Length > 0)
                                                tanum.Font = new Font(font, fontSize);
                                        }

                                        #region Alignment
                                        if (alignment != string.Empty)
                                        {
                                            switch (alignment.ToLower())
                                            {
                                                case "l":
                                                    tanum.Appearance.TextHAlign = HAlign.Left;
                                                    break;
                                                case "r":
                                                    tanum.Appearance.TextHAlign = HAlign.Right;
                                                    break;
                                                case "c":
                                                    tanum.Appearance.TextHAlign = HAlign.Center;
                                                    break;
                                                default:
                                                    tanum.Appearance.TextHAlign = HAlign.Default;
                                                    break;
                                            }
                                        }
                                        #endregion

                                        #region Format
                                        if (format != string.Empty)
                                        {
                                            if (format.Trim().StartsWith("?"))
                                            {
                                                tanum.Nullable = true;
                                                format = format.Trim().Replace("?", "");
                                            }
                                            tanum.Format = format;
                                            if (tanum.DataBindings.Count > 0)
                                                tanum.DataBindings[0].FormatString = format;
                                        }
                                        #endregion
                                        break;


                                }//End switch



                            }//End if
                        }//End if

                    }//End ForEach
                }

            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static object FormObject_Get(Form f, string declaredobjectName)
        {
            FieldInfo myFieldInfo = f.GetType().GetField(declaredobjectName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            //declaring one should be 'declaredobjectName'
            if (myFieldInfo != null)
                return myFieldInfo.GetValue(f);

            return null;
        }
        //---- Save Grid Format ----//
        public static bool SaveGrid(UltraGrid grid, string listSettingID,double rowHeightCM)
        {
            try
            {
                SYSFormSettingID objFormSetting;
                SYSFormSettingIDDet objListSetting;

                objFormSetting = SYSFormSettingID.Get(listSettingID);
                objFormSetting.RowHeightCM = rowHeightCM;
                if (objFormSetting.Update() == false)
                    throw new Exception("Form Setting ID Update Fail!");
                objListSetting = SYSFormSettingIDDet.Get(listSettingID);
               
                SaveGrid(grid, objListSetting);
                if (listSettingID == "frmSECGrpGridPerm")
                {
                    grid.DisplayLayout.Override.CellAppearance.BackColorDisabled = Color.Gray;
                    grid.DisplayLayout.Override.CellAppearance.BackColorDisabled2 = Color.Gray;
                }
                //added by May on 22 Feb 2024
                if(listSettingID.ToLower().StartsWith("frmarqo"))
                {
                    Form frm = grid.FindForm();
                    if(frm!=null)
                    {
                        string filePath = Application.StartupPath+@"\FormSettings\" + SysOptionUtility.DatabaseBranchCode + @"\" + frm.Name + ".txt";

                        List<SqlParameter> list = new List<SqlParameter>();
                        list.Add(new SqlParameter("@CodeKey", 11100));
                        list.Add(new SqlParameter("@FormName", frm.Name));
                        list.Add(new SqlParameter("@UserKey", AppInfor.CurrentUserKey));
                        string settings = GFunc.ExecuteScalar("SYSFormSetting_GetGridSettingJson", list);

                        //StringBuilder sb = new StringBuilder();
                        //sb.Append(File.ReadAllText(filePath));
                        string fileContent = File.ReadAllText(filePath);
                        int posStart = fileContent.IndexOf("\"frmSettingGrids\":[{");
                        int posEnd = fileContent.IndexOf("]", posStart);
                        int length = posEnd - posStart;
                        string strTobeReplaced = fileContent.Substring(posStart, length+1);
                        string strToReplace = settings.Substring(1, settings.Length -2);
                        fileContent=fileContent.Replace(strTobeReplaced, strToReplace);

                        File.WriteAllText(filePath, fileContent);
                    }
                }
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }


        }
        public static bool SaveGrid(UltraGrid grid, SYSFormSettingIDDet objListSetting)
        {
            try
            {
                #region Old Style
                /* Old Style
                //string msgID = string.Empty;
                //objListSetting.ColWidth = string.Empty;
                //objListSetting.ColPosition = string.Empty;
                //objListSetting.ColHeader = string.Empty;

                //for (int i = 0; i < grid.DisplayLayout.Bands[0].Columns.Count; i++)
                //{
                //    if (grid.DisplayLayout.Bands[0].Columns[i].Hidden)
                //        objListSetting.ColWidth += "0";
                //    else
                //        objListSetting.ColWidth += grid.DisplayLayout.Bands[0].Columns[i].Width;

                //    objListSetting.ColPosition += grid.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition;
                //    objListSetting.ColHeader += grid.DisplayLayout.Bands[0].Columns[i].Header.Caption;

                //    if (i + 1 != grid.DisplayLayout.Bands[0].Columns.Count)
                //    {
                //        objListSetting.ColWidth += ",";
                //        objListSetting.ColPosition += ",";
                //        objListSetting.ColHeader += ",";
                //    }
                //}
                 */
                #endregion

                objListSetting.PrimaryKey = new DataColumn[] { objListSetting.Columns["ColFldNm"] };
                for (int i = 0; i < grid.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    DataRow matchRow = objListSetting.Rows.Find(grid.DisplayLayout.Bands[0].Columns[i].Key);
                    if (matchRow == null)
                        continue;
                    if (grid.DisplayLayout.Bands[0].Columns[i].Hidden)
                        matchRow["ColWidth"] = "0";
                    else
                        matchRow["ColWidth"] = grid.DisplayLayout.Bands[0].Columns[i].Width;
                    matchRow["ColPosition"] = grid.DisplayLayout.Bands[0].Columns[i].Header.VisiblePosition + 1;
                    matchRow["ColTabStop"] = grid.DisplayLayout.Bands[0].Columns[i].TabStop;
                    matchRow["ColHeader"] = grid.DisplayLayout.Bands[0].Columns[i].Header.Caption;
                    matchRow["ColHide"] = grid.DisplayLayout.Bands[0].Columns[i].Hidden;
                }

                bool processOk = objListSetting.Update();
               
                return processOk;


            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }

        public static decimal Grid_MaxSeqDec(TAUtil.TAGridEditor grd, string SeqColKeyName)
        {
            decimal maxSeq = 0M;
            try
            {
                if (grd.DataSource.GetType() == typeof(DataTable))
                    maxSeq = (grd.DataSource as DataTable).Rows.Count == 0 ? 0M :
                        (grd.DataSource as DataTable).AsEnumerable().Max(o => o.Field<decimal>(SeqColKeyName));

                return maxSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int Grid_MaxSeqInt(TAUtil.TAGridEditor grd, string SeqColKeyName)
        {
            int maxSeq = 0;
            try
            {
                maxSeq = (grd.DataSource as DataTable).Rows.Count == 0 ? 0 :
                    (grd.DataSource as DataTable).AsEnumerable().Max(o => o.Field<int>(SeqColKeyName));

                return maxSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //this function is used from nowhere, required ? --Jack
        public static void Get_FormSettingColumns(DataRow drListSetting, DataTable dtNumberformat)
        {
            int colPosition = 0;
            try
            {
                DataTable dtSchema = null, dtParameters = null;
                DataSet dsParameters = null;
                string listSettingID = drListSetting["ListID"].ToString();

                string listSQL = drListSetting["SPName"].ToString();
                string parameter = string.Empty;
                colPosition = dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == listSettingID).Count() == 0 ? 0 : dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == listSettingID).Max(p => p.Field<int>("ColPosition")) + 1;

                if (drListSetting != null && !GFunc.IsNE(drListSetting["SPName"]))
                {
                    dsParameters = GFunc.ExecuteQueryDataSet("Exec sp_help " + drListSetting["SPName"].ToString());
                    if (dsParameters != null && dsParameters.Tables.Count > 1)
                    {
                        dtParameters = dsParameters.Tables[1];
                        //if (listSQL == "ARCTDetItm_Get")
                        //    MsgBox.Show("ARCTDetItm_Get");
                        foreach (DataRow drPara in dtParameters.Rows)
                        {
                            if (GFunc.CompareString(drPara["Type"].ToString(), "DateTime") || GFunc.CompareString(drPara["Type"].ToString(), "Date"))
                                parameter = "1/1/1900";
                            else if (GFunc.CompareString(drPara["Type"].ToString(), "xml"))
                                parameter = "'<a/>'";
                            else
                                parameter = "0";

                            listSQL = listSQL.Contains(" ") ? listSQL + " ," + parameter : listSQL + " " + parameter;
                        }
                    }
                }

                if (listSQL != string.Empty || listSQL != null)
                {
                    dtSchema = GFunc.ExecuteQuery("Exec " + listSQL);
                }


                if (dtSchema != null)
                {
                    foreach (DataColumn dcol in dtSchema.Columns)
                    {
                        if (!GFunc.IsNE(drListSetting["ListID"]))
                        {
                            DataRow dr = dtNumberformat.NewRow();
                            dr["ListID"] = drListSetting["ListID"];
                            dr["ColName"] = dcol.ColumnName;
                            dr["Match"] = false;
                            dr["ColPosition"] = dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == listSettingID).Count() == 0 ? 0 : dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == listSettingID).Max(p => p.Field<int>("ColPosition")) + 1; ;
                            dtNumberformat.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    if (!GFunc.IsNE(drListSetting["ListID"]))
                    {
                        DataRow dr = dtNumberformat.NewRow();
                        dr["ListID"] = drListSetting["ListID"];
                        dr["ColName"] = "No Data";
                        dr["Match"] = false;

                        dr["ColPosition"] = dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == listSettingID).Count() == 0 ? 0 : dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == listSettingID).Max(p => p.Field<int>("ColPosition")) + 1;
                        dtNumberformat.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                if (GFunc.CompareString(drListSetting["ListID"].ToString(), "frmARCTGridItm"))
                    MsgBox.Show(drListSetting["ListID"].ToString());

                //DataRow dr = dtNumberformat.NewRow();
                //dr["ListID"] = GFunc.IsNE(drListSetting["ListID"]) ? "" : drListSetting["ListID"];
                //dr["ColName"] = "Error";              
                //dr["Match"] = false;                    
                //dr["ColPosition"] = dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == drListSetting["ListID"].ToString()).Count() == 0 ? 0 : dtNumberformat.AsEnumerable().Where(p => p.Field<string>("ListID") == drListSetting["ListID"].ToString()).Max(p => p.Field<int>("ColPosition")) + 1;
                //dtNumberformat.Rows.Add(dr);
            }
        }

        public static void CellLockTabStop_Set(UltraGrid grd, string colName, bool Lock)
        {
            if (Lock)
            {
                grd.ActiveRow.Cells[colName].Activation = Activation.ActivateOnly;
                grd.ActiveRow.Cells[colName].TabStop = DefaultableBoolean.False;
            }
            else
            {
                grd.ActiveRow.Cells[colName].Activation = Activation.AllowEdit;
                grd.ActiveRow.Cells[colName].TabStop = DefaultableBoolean.True;
            }
        }//Completed
        public static void CellLock_Set(UltraGrid grd, string colName, bool Lock)
        {
            if (Lock)
            {
                grd.ActiveRow.Cells[colName].Activation = Activation.ActivateOnly;
            }
            else
            {
                grd.ActiveRow.Cells[colName].Activation = Activation.AllowEdit;
            }
        }//Completed

        //Grid Drag & Drop Related Functions
        internal static void Grid_SelectionDrag(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TAUtil.TAGridEditor grid = sender as TAUtil.TAGridEditor;
            try
            {
                grid.DoDragDrop(grid.Selected.Rows, DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        internal static void Grid_DragOver(object sender, DragEventArgs e)
        {
            TAUtil.TAGridEditor grid = sender as TAUtil.TAGridEditor;

            try
            {
                e.Effect = DragDropEffects.Move;

                Point pointInGridCoords = grid.PointToClient(new Point(e.X, e.Y));
                if (pointInGridCoords.Y < 20)
                    // Scroll up.
                    grid.ActiveRowScrollRegion.Scroll(RowScrollAction.LineUp);
                else if (pointInGridCoords.Y > grid.Height - 20)
                    // Scroll down.
                    grid.ActiveRowScrollRegion.Scroll(RowScrollAction.LineDown);

            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }
        internal static void Grid_DragDropDocItm(object sender, DragEventArgs e)
        {
            decimal preSN = 0;
            int dropIndex;
            string ColSNName = "";
            TAUtil.TAGridEditor grid = sender as TAUtil.TAGridEditor;

            try
            {
                // Get the position on the grid where the dragged row(s) are to be dropped.
                //get the grid coordinates of the row (the drop zone)
                UIElement uieOver = grid.DisplayLayout.UIElement.ElementFromPoint(grid.PointToClient(new Point(e.X, e.Y)));

                //get the row that is the drop zone/or where the dragged row is to be dropped
                UltraGridRow ugrOver = uieOver.GetContext(typeof(UltraGridRow), true) as UltraGridRow;
                if (ugrOver != null)
                {
                    //get the dragged row(s)which are to be dragged to another position in the grid
                    //Check for any row selected for drag and drop if not return
                    //this drag and drop is only for row and not for columns. so it will return when there are no row selected
                    SelectedRowsCollection SelRows = (SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection)) as SelectedRowsCollection;
                    if (SelRows.Count == 0)
                        return;

                    SelRows.Sort();

                    //Get the SN number on the rows to be drop
                    ColSNName = GetDetSNColumnName(grid);
                    dropIndex = ugrOver.Index;    //index/position of drop zone in grid
                    if (dropIndex == -1)
                        preSN = grid.Rows.Count;
                    else

                        preSN = decimal.Parse(grid.Rows[dropIndex].Cells[ColSNName].Value.ToString()) - 1;

                    //Update the SN number of the rows to be drop
                    for (int i = 0; i <= SelRows.Count - 1; i++)
                    {
                        grid.Rows[SelRows[i].Index].Cells[ColSNName].Value = preSN + GFunc.NEDec(grid.Rows[SelRows[i].Index].Cells[ColSNName].Value, 0) / 10000M;
                    }
                    grid.UpdateData();//this will call Grid Before & After RowUpdate events   
                    grid.Refresh();
                }
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }

        internal static void Grid_DragDrop(object sender, DragEventArgs e)
        {
            int dropIndex;
            TAUtil.TAGridEditor grid = sender as TAUtil.TAGridEditor;
            try
            {
                // Get the position on the grid where the dragged row(s) are to be dropped.
                //get the grid coordinates of the row (the drop zone)
                UIElement uieOver = grid.DisplayLayout.UIElement.ElementFromPoint(grid.PointToClient(new Point(e.X, e.Y)));

                //get the row that is the drop zone/or where the dragged row is to be dropped
                UltraGridRow ugrOver = uieOver.GetContext(typeof(UltraGridRow), true) as UltraGridRow;
                if (ugrOver != null)
                {
                    dropIndex = ugrOver.Index;    //index/position of drop zone in grid

                    //get the dragged row(s)which are to be dragged to another position in the grid
                    SelectedRowsCollection SelRows = (SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection)) as SelectedRowsCollection;

                    SelRows.Sort();
                    if (dropIndex < SelRows[0].Index)//scroll up
                    {
                        ////get the count of selected rows and drop each starting at the dropIndex                  
                        for (int i = SelRows.Count - 1; i >= 0; i--)
                        {
                            grid.Rows.Move(SelRows[i], dropIndex);
                        }
                    }
                    else
                    {
                        ////get the count of selected rows and drop each starting at the dropIndex                  
                        for (int i = 0; i <= SelRows.Count - 1; i++)
                        {
                            grid.Rows.Move(SelRows[i], dropIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }

        private static string GetDetSNColumnName(TAUtil.TAGridEditor grid)
        {
            ColumnsCollection cols = grid.DisplayLayout.Bands[0].Columns;

            //May added on 27-Jun-2012, To allow drag and drop in pop up Batch Item entry
            if (grid.ActiveRow != null)
            {
                if (cols.Exists("LineLinkKey"))
                    if (GFunc.NEInt(grid.ActiveRow.Cells["LineLinkKey"].Value, 0) > 0 && cols.Exists("ItmDetSN"))
                        return "ItmDetSN";
            }

            if (cols.Exists("DetItmSN"))
                return "DetItmSN";
            else if (cols.Exists("ItmSN"))
                return "ItmSN";
            else if (cols.Exists("ExpSN"))
                return "ExpSN";

            return "";
        }

        /// <summary>
        /// Add Value 0 to combo box             
        /// <param name="cboSource">ComboBox object to which start index type will be added.</param>
        /// </summary>        
        public static void AddComboEmptyValue(TAUtil.TAComboBox cboSource)
        {
            try
            {
                if (cboSource.DataSource == null)
                    return;

                if (cboSource.DisplayMember.Trim() == string.Empty ||
                        cboSource.ValueMember.Trim() == string.Empty)
                {
                    MsgBox.Show("System Error! Value and Diplay Member must be set to add empty row in Combobox >>>. " + cboSource.Name);
                    return;
                }
                DataTable dtTemp = (DataTable)cboSource.DataSource;
                DataRow drSelect = dtTemp.NewRow();
                dtTemp.DefaultView.Sort = cboSource.DisplayMember;
                drSelect[cboSource.ValueMember] = 0;
                drSelect[cboSource.DisplayMember] = " ";
                dtTemp.Rows.Add(drSelect);
                cboSource.DataSource = dtTemp;
                cboSource.Refresh();
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        /// <summary>
        /// Add empty row to combo box with 0 Value 
        /// Give a choice to user to hide empty row or not 
        /// If hideEmptyRow value is false, he user will see empty row at 0 index (top of list)
        /// <param name="cboSource">ComboBox object to which start index type will be added.</param>
        /// <param name="hideEmptyRow">bool value .</param>
        /// </summary>        
        public static void AddComboEmptyValue(UltraCombo cboSource, bool hideEmptyRow)
        {
            try
            {
                if (cboSource.DataSource == null)
                    return;
                if (cboSource.DisplayMember.Trim() == string.Empty ||
                    cboSource.ValueMember.Trim() == string.Empty)
                {
                    MsgBox.Show("System Error! Value and Diplay Member must be set to add empty row in Combobox >>>. " + cboSource.Name);
                    return;
                }
                DataTable dtTemp = cboSource.DataSource as DataTable;

                if (dtTemp == null) //To prevent error if the combo has not been set DataSource
                    return;
                DataRow drSelect = dtTemp.NewRow();
                dtTemp.DefaultView.Sort = cboSource.DisplayMember;


                if (dtTemp.Columns[cboSource.ValueMember].DataType == typeof(string))
                    drSelect[cboSource.ValueMember] = string.Empty;
                else
                    drSelect[cboSource.ValueMember] = 0;

                drSelect[cboSource.DisplayMember] = DBNull.Value;
                dtTemp.Rows.Add(drSelect);


                cboSource.DataSource = dtTemp;
                cboSource.Rows[0].Hidden = hideEmptyRow;

                cboSource.Refresh();

                dtTemp = cboSource.DataSource as DataTable;
                dtTemp.DefaultView.Sort = "";
                cboSource.DataSource = dtTemp;
            }
            catch (Exception ex)
            {
                Error(ex, false, false);
            }
        }

        public static void Combo_Format(TAUtil.TAComboBox cbo, DataView dvDet)
        {
            try
            {
                bool hidden = false;
                string header = string.Empty;
                int width = 0;
                if (dvDet == null || dvDet.Count == 0)
                    return;
                dvDet.Sort = "ColFldNm";
                int vc = 0;
                string vcName = "";
                foreach (UltraGridColumn c in cbo.DisplayLayout.Bands[0].Columns)
                {
                  
                    DataRowView[] drs = dvDet.FindRows(c.Key);
                    //c.Hidden = true;
                    if (drs.Length > 0)
                    {
                        header = GFunc.NEStr(drs[0]["ColHeader"], "");
                        hidden = GFunc.NEBool(drs[0]["ColHide"], false);

                        width = GFunc.NEInt(drs[0]["ColWidth"], 0);

                        c.Header.Caption = header;
                        c.Width = width;
                        c.Hidden = hidden || width == 0;
                        if (!c.Hidden)
                        {
                            vc++;
                            vcName = c.Key;
                        }
                        c.Header.VisiblePosition = GFunc.NEInt(drs[0]["ColPosition"], 0);
                    }
                }
                if (vc == 1)
                {
                    cbo.DisplayLayout.Bands[0].ColHeadersVisible = false;

                    if (cbo.DisplayLayout.Bands[0].Columns[vcName].Width < cbo.Width)//Added by May. To make the width of column same as combo box if there is only one column and the column width is less than combo width
                        cbo.DisplayLayout.Bands[0].Columns[vcName].Width = cbo.Width;
                }
                cbo.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToFill;
                cbo.DisplayLayout.ScrollStyle = ScrollStyle.Immediate;

            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }

        //----------New Combo_Format--------//
        public static void Combo_Format(TAUtil.TAComboBox cbo, DataTable dtDet)
        {
            try
            {
                bool hidden = false;
                string header = string.Empty;
                int width = 0;
                if (dtDet == null || dtDet.Rows.Count == 0)
                    return;

                int vc = 0;
                string vcName = "";
                foreach (UltraGridColumn c in cbo.DisplayLayout.Bands[0].Columns)
                {

                    DataRow[] drs = dtDet.Select("ColFldNm='" + c.Key + "'");
                    c.Hidden = true;
                    if (drs.Length > 0)
                    {
                        header = GFunc.NEStr(drs[0]["ColHeader"], "");
                        hidden = GFunc.NEBool(drs[0]["ColHide"], false);

                        width = GFunc.NEInt(drs[0]["ColWidth"], 0);

                        c.Header.Caption = header;
                        c.Width = width;
                        c.Hidden = hidden || width == 0;
                        if (!c.Hidden)
                        {
                            vc++;
                            vcName = c.Key;
                        }
                        c.Header.VisiblePosition = GFunc.NEInt(drs[0]["ColPosition"], 0);
                    }
                }
                if (vc == 1)
                {
                    cbo.DisplayLayout.Bands[0].ColHeadersVisible = false;

                    if (cbo.DisplayLayout.Bands[0].Columns[vcName].Width < cbo.Width)//Added by May. To make the width of column same as combo box if there is only one column and the column width is less than combo width
                        cbo.DisplayLayout.Bands[0].Columns[vcName].Width = cbo.Width;
                }
                cbo.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToFill;
                cbo.DisplayLayout.ScrollStyle = ScrollStyle.Immediate;

            }
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void BindComboValue(TAUtil.TAComboBox cboSource, string ListSettingID)
        {
            try
            {
                if (cboSource != null)
                {
                    string[] colHeaders = null;
                    string[] colWidths = null;
                    SYSFormSettingID objSysListSetting = null;
                    SYSFormSettingIDDet objFormSettingDet = SYSFormSettingIDDet.Get(ListSettingID);
                    DataTable dtdet = (DataTable)objFormSettingDet;

                    if (ListSettingID == null)
                        return;
                    string[] ListParameters = ListSettingID.Split('%');

                    if (ListParameters.Length > 0)
                    {
                        //Use index 0 to List Setting.
                        objSysListSetting = SYSFormSettingID.Get(ListParameters[0]);
                        objFormSettingDet = SYSFormSettingIDDet.Get(ListParameters[0]);
                        dtdet = (DataTable)objFormSettingDet;
                    }

                    if (!GFunc.IsNE(objSysListSetting))
                    {
                        if (ListSettingID.Equals("mstaccall_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstaccforeign_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstacchome_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstaccpayment_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconall_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconbothcash_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconbothcredit_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconpurchase_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("MSTConALLPurchase_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals(GVar.ListSettingID.MSTConPurchasePY_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsales_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsales_idstate", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsalesall_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconjobsall_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsalescash_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsalescredit_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsalespycredit_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsalespycash_id", StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.ConAccessLevel.ToString() };

                        //if(ListSettingID.Equals("MSTJobAll_id", StringComparison.InvariantCultureIgnoreCase))
                        else if (ListSettingID.Equals(GVar.ListSettingID.MSTJobAll_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };

                        else if (ListSettingID.Equals("mstitmall_id", StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstitmfscanvg_id", StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.ItemAccessLevel.ToString() };

                        else if (ListSettingID.Equals("mstpricelistbycurrkey", StringComparison.InvariantCultureIgnoreCase))
                        {    //when form loading, the param Currecy is default 1, if not, will get the current currency Value
                            int DocCurrKey = 1;
                            string seachFdNm = "CCurrkey";
                            if (cboSource.Name.ToLower().Equals("VPriceType"))
                                seachFdNm = "VCurrkey";

                            Control[] ctrls = GetActiveControlAtActiveForm().FindForm().Controls.Find(seachFdNm, true);
                            if (ctrls != null && ctrls.Count() > 0)
                                DocCurrKey = GFunc.NEInt(((TAUtil.TAComboBox)ctrls[0]).Value, 0);
                            ListParameters = new string[] { ListSettingID, DocCurrKey.ToString() };
                        }
                        else if (ListSettingID.Equals("mstjoball_id", StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };
                        else if (ListSettingID.Equals("mstjobsales_id", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int DocConKey = 0;
                            Control[] ctrlsjob = GetActiveControlAtActiveForm().FindForm().Controls.Find("DocConKey", true);
                            if (ctrlsjob != null && ctrlsjob.Count() > 0)
                                DocConKey = GFunc.NEInt(((TAUtil.TAComboBox)ctrlsjob[0]).Value, 0);
                            ListParameters = new string[] { ListSettingID, "", DocConKey.ToString(), AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };
                        }
                        else if (ListSettingID.ToLower().StartsWith("mstjobsalesbyconkey"))
                            ListParameters = new string[] { ListSettingID, ListParameters[1].ToString(), AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };
                        else if (ListSettingID.Equals("mstshipnamebyconkey", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int DocConKey = 0;
                            Control[] ctrlsjob = GetActiveControlAtActiveForm().FindForm().Controls.Find("DocConKey", true);
                            if (ctrlsjob != null && ctrlsjob.Count() > 0)
                            {
                                DocConKey = GFunc.NEInt(((TAUtil.TAComboBox)ctrlsjob[0]).Value, 0);
                                ListParameters = new string[] { ListSettingID, DocConKey.ToString() };
                            }
                        }
                        for (int i = 1; i < ListParameters.Length; i++)
                        {
                            string oldVal = "{" + (i - 1) + "}";
                            objSysListSetting.ListSQL = objSysListSetting.ListSQL.Replace(oldVal, ListParameters[i]);
                        }

                        if (objSysListSetting.ListSQL != "")
                        {
                            cboSource.ValueMember = "";
                            cboSource.DisplayMember = "";
                            cboSource.DataSource = null;
                            cboSource.DataSource = GFunc.ExecuteQuery(objSysListSetting.ListSQL);
                        }

                        //Format
                        colHeaders = objSysListSetting.ColHeader.Split(',');
                        colWidths = objSysListSetting.ColWidth.Split(',');
                        // Combo_Format(cboSource, colHeaders, colWidths);
                        Combo_Format(cboSource, dtdet);

                        cboSource.ValueMember = objSysListSetting.ValueColName;
                        cboSource.DisplayMember = objSysListSetting.DisplayColName;
                        cboSource.AllowNull = DefaultableBoolean.True;
                        GlobalUI.AddComboEmptyValue(cboSource, true);
                    }

                    //cboSource.LimitToList = true;
                    cboSource.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData, false);
                }
                else
                {
                    throw new TAException("There is no TACombo to set data");
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, true, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, false);
            }
        }

        public static void BindComboValueNew(TAUtil.TAComboBox cboSource, DataRowView drCombo, DataSet dsAllSettings,int codeKey)
        {
            try
            {
                if (cboSource != null)
                {
                    string listSettingID = GFunc.NEStr(drCombo["ListSettingID"], "");

                    if (!dsAllSettings.Tables.Contains(listSettingID))
                    {
                        try
                        {
                            string src = File.ReadAllText(Application.StartupPath+@"\FormSettings\" + SysOptionUtility.DatabaseBranchCode + @"\Ref\" + listSettingID + ".txt");
                            if (src != "")
                            {
                                DataTable dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(src);
                                cboSource.DataSource = dt;
                            }   
                        }
                        catch(Exception ex)
                        {

                        }
                        if(cboSource.DataSource==null)
                            BindComboValue(cboSource, listSettingID + "%" + codeKey);
                    }
                    else
                        cboSource.DataSource = dsAllSettings.Tables[listSettingID];

                    cboSource.DisplayMember = GFunc.NEStr(drCombo["ControlMemberDisplay"], "");
                    cboSource.ValueMember = GFunc.NEStr(drCombo["ControlMemberValue"], "");
                    cboSource.AllowNull = DefaultableBoolean.True;
                    GlobalUI.AddComboEmptyValue(cboSource, true);
                    cboSource.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData, false);

                    dsAllSettings.Tables["frmSettingCombos"].DefaultView.RowFilter = "ListID='" + listSettingID + "'";
                    Combo_Format(cboSource, dsAllSettings.Tables["frmSettingCombos"].DefaultView);
                    //using (DataTable dt = dsAllSettings.Tables["frmSettingCombos"].DefaultView.ToTable())
                    //{
                    //    Combo_Format(cboSource, dt);
                    //}
                    cboSource.LimitToList = GFunc.NEBool(drCombo["ControlLimitToList"], false);
                    cboSource.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                    cboSource.TextRenderingMode = TextRenderingMode.GDI;
                    cboSource.Font = new Font("Calibri", 11);
                    cboSource.Appearance.BackColor = Color.White;
                }
                else
                {
                    throw new TAException("There is no TACombo to set data");
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, true, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, false);
            }
        }

        public static void BindComboValueNew(TAUtil.TAComboBox cboSource, DataRow drCombo,DataSet dsAllSettings,int codeKey)
        {
            try
            {
                if (cboSource != null)
                {                    
                    string listSettingID = GFunc.NEStr(drCombo["ListSettingID"], "");

                    if(!dsAllSettings.Tables.Contains(listSettingID))
                    {
                        try
                        {
                            string src = File.ReadAllText(Application.StartupPath+@"\FormSettings\" + SysOptionUtility.DatabaseBranchCode + @"\Ref\" + listSettingID + ".txt");
                            if (src != "")
                            {
                                DataTable dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(src);
                                cboSource.DataSource = dt;
                            }
                        }
                        catch (Exception ex)
                        {
                            //file not found
                        }
                        if (cboSource.DataSource == null)
                            BindComboValue(cboSource, listSettingID+"%"+codeKey);
                    }
                    else
                        cboSource.DataSource = dsAllSettings.Tables[listSettingID];

                    cboSource.DisplayMember = GFunc.NEStr(drCombo["ControlMemberDisplay"], "");
                    cboSource.ValueMember = GFunc.NEStr(drCombo["ControlMemberValue"], "");
                    cboSource.AllowNull = DefaultableBoolean.True;
                    GlobalUI.AddComboEmptyValue(cboSource, true);
                    cboSource.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData, false);

                    dsAllSettings.Tables["frmSettingCombos"].DefaultView.RowFilter = "ListID='" + listSettingID + "'";
                    Combo_Format(cboSource, dsAllSettings.Tables["frmSettingCombos"].DefaultView);
                    //using (DataTable dt = dsAllSettings.Tables["frmSettingCombos"].DefaultView.ToTable())
                    //{
                    //    Combo_Format(cboSource, dt);
                    //}
                    cboSource.LimitToList = GFunc.NEBool(drCombo["ControlLimitToList"], false);
                    cboSource.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                    cboSource.TextRenderingMode = TextRenderingMode.GDI;
                    cboSource.Font = new Font("Calibri", 11);
                    cboSource.Appearance.BackColor = Color.White;
                }
                else
                {
                    throw new TAException("There is no TACombo to set data");
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, true, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, false);
            }
        }
        public static void BindComboValue(TAUtil.TAComboBox cboSource, string ListSettingID, string DisplayMember, string ValueMember, int codeKey)
        {
            try
            {
                if (cboSource != null)
                {
                    string[] colHeaders = null;
                    string[] colWidths = null;
                    SYSFormSettingID objSysFormSetting = null;
                    SYSFormSettingIDDet objFormSettingDet = null;
                    DataTable dtdet = null;

                    if (ListSettingID == null)
                        return;
                    string[] ListParameters = ListSettingID.Split('%');

                    if (ListParameters.Length > 0)
                    {
                        //Use index 0 to List Setting.
                        objSysFormSetting = SYSFormSettingID.Get(ListParameters[0]);
                        objFormSettingDet = SYSFormSettingIDDet.Get(ListParameters[0]);
                        dtdet = (DataTable)objFormSettingDet;
                    }

                    if (!GFunc.IsNE(objSysFormSetting))
                    {
                        if (ListParameters[0].Equals(GVar.ListSettingID.MSTAccReport_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConPurchaseReport_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConReport_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConSalesReport_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmReport_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTJobReport_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "" };
                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTAccAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTAccForeign_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTAccHome_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTAccPayment_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTAccIncome_des, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConBothCash_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConBothCredit_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConPurchase_id, StringComparison.InvariantCultureIgnoreCase) ||
                            //ttm
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConALLPurchase_id, StringComparison.InvariantCultureIgnoreCase) ||
                            //ttm
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConPurchasePY_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConSales_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals("mstconsales_idstate", StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConSalesAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConJobsAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConSalesCash_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConSalesCredit_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConSalesPYCash_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTConSalesPYCredit_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.ConAccessLevel.ToString() };
                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTItmAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmFSCANVG_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmM_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmH_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmR_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.AssemblyItmList, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmN_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmF_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmSales_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmPurchase_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListParameters[0].Equals(GVar.ListSettingID.MSTItmJob_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.ItemAccessLevel.ToString() };

                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTJobAll_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };

                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTJobSales_id, StringComparison.InvariantCultureIgnoreCase))
                        {
                            //when form loading, the param ConKey is default 0, else if not, will get the current currency Value
                            int DocConKey = 0;

                            Control[] ctrls = null;
                            Control frms = GetActiveControlAtActiveForm();
                            if (frms != null)
                                ctrls = frms.FindForm().Controls.Find("DocConKey", true);
                            if (ctrls != null && ctrls.Count() > 0)
                                DocConKey = GFunc.NEInt(((TAUtil.TAComboBox)ctrls[0]).Value, 0);
                            ListParameters = new string[] { ListSettingID, "", DocConKey.ToString(), AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };
                        }
                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTJobSalesByConKey, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };

                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTItmFS_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.ItemAccessLevel.ToString() };

                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTAccLiability_id, StringComparison.InvariantCultureIgnoreCase) ||
                        ListParameters[0].Equals(GVar.ListSettingID.MSTAccLiability_des, StringComparison.InvariantCultureIgnoreCase)||
                        ListParameters[0].Equals("MSTAccRetainEarning_id", StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "",AppInfor.ConAccessLevel.ToString(),AppInfor.CurrentUserKey.ToString() };

                        else if (ListParameters[0].Equals(GVar.ListSettingID.MSTPriceListByCurrKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            //when form loading, the param Currecy is default 1, else if not, will get the current currency Value
                            int DocCurrKey = 1;
                            Control[] ctrl = null;                            

                            string seachFdNm = "CCurrkey";
                            if (cboSource.Name.ToLower().Equals("vpricetype"))
                                seachFdNm = "VCurrkey";
                            
                            ctrl = cboSource.FindForm().Controls.Find(seachFdNm, true);
                            if (ctrl != null && ctrl.Count() > 0)
                                DocCurrKey = GFunc.NEInt(((TAUtil.TAComboBox)ctrl[0]).Value, 0);
                            ListParameters = new string[] { ListSettingID, DocCurrKey.ToString() };
                        }
                        else if (ListParameters[0].Equals(GVar.ListSettingID.SYSRepRptDefaultRepKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (codeKey == (int)GEnum.SystemCode.ARAP_Revaluation)
                                ListParameters = new string[] { ListSettingID, "1155" };
                            else if (codeKey == (int)GEnum.SystemCode.Financial_Charge)
                                ListParameters = new string[] { ListSettingID, "1160" };
                        }
                        else if (ListParameters[0].Equals(GVar.ListSettingID.SYSDocTypeNmByDC, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (codeKey == (int)GEnum.SystemCode.DO_to_IV_Transfer)
                                ListParameters = new string[] { ListSettingID, ((int)GEnum.SystemCode.Delivery_Order).ToString() };
                        }
                        else if (ListParameters[0].Equals(GVar.ListSettingID.SYSRepRpt, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, codeKey.ToString() }; //RepKey and CodeKey are the same.

                        else if (ListParameters[0].Equals(GVar.ListSettingID.SECGroupsByUserBrowseCombo, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, AppInfor.CurrentUserKey.ToString() };

                        else if (ListParameters[0].Equals("frmInsertPODocID", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (ListParameters.Count() == 1)
                            {
                                ListParameters = new string[] { ListSettingID, codeKey.ToString(), DateTime.Today.Date.ToString("dd/MMM/yyyy"), DateTime.Today.Date.ToString("dd/MMM/yyyy"), "0" }; //RepKey and CodeKey are the same.
                            }
                        }
                        else if (ListParameters[0].Equals("frmInsertCPSDocID", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (ListParameters.Count() == 1)
                            {
                                ListParameters = new string[] { ListSettingID, DateTime.Today.Date.ToString("dd/MMM/yyyy") };
                            }
                        }
                        else if (ListParameters[0].Equals("frmInsertPDDocID", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (ListParameters.Count() == 1)
                            {
                                ListParameters = new string[] { ListSettingID, DateTime.Today.Date.ToString("dd/MMM/yyyy"), DateTime.Today.Date.ToString("dd/MMM/yyyy"), "0" }; //RepKey and CodeKey are the same.
                            }
                        }
                        else if (ListParameters[0].Equals("SYSMsgDCInsertData", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (ListParameters.Count() == 1)
                            {
                                ListParameters = new string[] { ListSettingID, codeKey.ToString() }; //RepKey and CodeKey are the same.
                            }
                        }
                        else if (ListParameters[0].Equals("frmInsertDataDocID", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (ListParameters.Count() == 1)
                            {
                                ListParameters = new string[] { ListSettingID, codeKey.ToString(), DateTime.Today.Date.ToString("dd/MMM/yyyy"), DateTime.Today.Date.ToString("dd/MMM/yyyy") }; //RepKey and CodeKey are the same.
                            }
                        }
                        else if (ListParameters[0].Equals("MSTShipNameByConKey", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (ListParameters.Count() == 1)
                            {
                                ListParameters = new string[] { ListSettingID, "0" };
                            }
                        }
                        else if (ListParameters[0].Equals("ToDODocID", StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "0" };

                        else if (ListParameters[0].Equals("ItemInternalRef", StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, codeKey.ToString() };

                        for (int i = 1; i < ListParameters.Length; i++)
                        {
                            string oldVal = "{" + (i - 1) + "}";
                            objSysFormSetting.ListSQL = objSysFormSetting.ListSQL.Replace(oldVal, ListParameters[i]);
                        }

                        if (objSysFormSetting.ListSQL != "")
                        {
                            cboSource.ValueMember = "";
                            cboSource.DisplayMember = "";
                            cboSource.DataSource = null;
                            cboSource.DataSource = GFunc.ExecuteQuery(objSysFormSetting.ListSQL);
                        }

                        //Format
                        //colHeaders = objSysFormSetting.ColHeader.Split(',');
                        //colWidths = objSysFormSetting.ColWidth.Split(',');
                        // Combo_Format(cboSource, colHeaders, colWidths);
                        Combo_Format(cboSource, dtdet);

                        if (DisplayMember == string.Empty || ValueMember == string.Empty)
                        {
                            cboSource.ValueMember = objSysFormSetting.ValueColName;
                            cboSource.DisplayMember = objSysFormSetting.DisplayColName;
                        }
                        else
                        {
                            cboSource.ValueMember = ValueMember;
                            cboSource.DisplayMember = DisplayMember;
                        }
                        cboSource.AllowNull = DefaultableBoolean.True;
                        GlobalUI.AddComboEmptyValue(cboSource, true);
                    }

                    //cboSource.LimitToList = true;
                    cboSource.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData, false);
                }
                else
                {
                    throw new TAException("There is no TACombo to set data");
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, true, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, false);
            }
        }
        public static void BindComboValue(SqlConnection cn, TAUtil.TAComboBox cboSource, string ListSettingID, string DisplayMember, string ValueMember)
        {
            try
            {
                if (cboSource != null)
                {
                    string[] colHeaders = null;
                    string[] colWidths = null;
                    SYSFormSettingID objSysFormSetting = null;
                    SYSFormSettingIDDet objFormSettingDet = null;
                    DataTable dtdet = null;
                    if (ListSettingID == null)
                        return;

                    string[] ListParameters = ListSettingID.Split('%');

                    if (ListParameters.Length > 0)
                    {
                        //Use index 0 to List Setting.
                        objSysFormSetting = SYSFormSettingID.GetFormList(cn, ListParameters[0]);
                        objFormSettingDet = SYSFormSettingIDDet.GetFormList(cn, ListParameters[0]);
                        dtdet = (DataTable)objFormSettingDet;
                    }

                    if (!GFunc.IsNE(objSysFormSetting))
                    {
                        if (ListSettingID.Equals(GVar.ListSettingID.MSTAccAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTAccForeign_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTAccHome_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTAccPayment_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConBothCash_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConBothCredit_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConPurchase_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConPurchasePY_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConSales_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals("mstconsales_idstate", StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConSalesAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConSalesCash_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConSalesCredit_id, StringComparison.InvariantCultureIgnoreCase) ||
                           ListSettingID.Equals(GVar.ListSettingID.MSTConSalesPYCash_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals(GVar.ListSettingID.MSTConSalesPYCredit_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.ConAccessLevel.ToString() };

                        else if (ListSettingID.Equals(GVar.ListSettingID.MSTJobAll_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };

                        else if (ListSettingID.Equals(GVar.ListSettingID.MSTJobSales_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", "0", AppInfor.CurrentUserKey.ToString(), AppInfor.JobAccessLevel.ToString() };

                        else if (ListSettingID.Equals(GVar.ListSettingID.MSTItmAll_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals(GVar.ListSettingID.MSTItmFSCANVG_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals(GVar.ListSettingID.MSTItmM_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals(GVar.ListSettingID.MSTItmFSNVG_id, StringComparison.InvariantCultureIgnoreCase) ||
                            ListSettingID.Equals(GVar.ListSettingID.MSTItmFS_id, StringComparison.InvariantCultureIgnoreCase))
                            ListParameters = new string[] { ListSettingID, "", AppInfor.CurrentUserKey.ToString(), AppInfor.ItemAccessLevel.ToString() };

                        for (int i = 1; i < ListParameters.Length; i++)
                        {
                            string oldVal = "{" + (i - 1) + "}";
                            objSysFormSetting.ListSQL = objSysFormSetting.ListSQL.Replace(oldVal, ListParameters[i]);
                        }

                        if (objSysFormSetting.ListSQL != "")
                        {
                            cboSource.ValueMember = "";
                            cboSource.DisplayMember = "";
                            cboSource.DataSource = null;
                            cboSource.DataSource = GFunc.ExecuteQuery(cn, objSysFormSetting.ListSQL);
                        }

                        //Format
                        colHeaders = objSysFormSetting.ColHeader.Split(',');
                        colWidths = objSysFormSetting.ColWidth.Split(',');
                        // Combo_Format(cboSource, colHeaders, colWidths);
                        Combo_Format(cboSource, dtdet);

                        if (DisplayMember == string.Empty || ValueMember == string.Empty)
                        {
                            cboSource.ValueMember = objSysFormSetting.ValueColName;
                            cboSource.DisplayMember = objSysFormSetting.DisplayColName;
                        }
                        else
                        {
                            cboSource.ValueMember = ValueMember;
                            cboSource.DisplayMember = DisplayMember;
                        }
                        cboSource.AllowNull = DefaultableBoolean.True;
                        GlobalUI.AddComboEmptyValue(cboSource, true);
                    }

                    //cboSource.LimitToList = true;
                    cboSource.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData, false);
                }
                else
                {
                    throw new TAException("There is no TACombo to set data");
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, true, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, true, false);
            }
        }

        //public static void BindGridCombo(TAUtil.TAComboBox taCbo, DataTable dt, string valueMember, string displayMember)
        //{
        //    try
        //    {
        //        taCbo.DataSource = dt;
        //        taCbo.ValueMember = valueMember;
        //        taCbo.DisplayMember = displayMember;
        //        //GlobalUI.AddComboEmptyValue(taCbo, true);

        //        foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn col in taCbo.DisplayLayout.Bands[0].Columns)
        //        {
        //            if (col.Key != valueMember && col.Key != displayMember)
        //                col.Hidden = true;
        //        }
        //        taCbo.DisplayLayout.Bands[0].Columns[displayMember].Width = 100;
        //        taCbo.DisplayLayout.Bands[0].Columns[displayMember].Width = 245;

        //        taCbo.AllowNull = DefaultableBoolean.True;
        //        taCbo.Rows.Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.ReloadData, false);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
        //    }
        //}
        public static void ItemNotInList(TAUtil.TAComboBox cbo, ValidationErrorEventArgs e, bool quickAdd, int? quickAddOption)
        {
            try
            {
                if (cbo.LimitToList==false)//will not check if limit to list is false
                    return;
                if (cbo.Text.Trim() != string.Empty)
                {
                    #region Display NotinList Message
                    GEnum.MsgBoxButton btnSelect = GEnum.MsgBoxButton.Try_Again;

                    if (quickAdd)
                        btnSelect = MsgBox.Show(MsgID.Common.AddNewRecord + "%" + cbo.Text, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Add_New_Record, GEnum.MsgBoxButton.Try_Again, GEnum.MsgBoxButton.Cancel);
                    else
                        btnSelect = MsgBox.Show(MsgID.Common.ItemNotInList + "%" + cbo.Text, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Try_Again, GEnum.MsgBoxButton.Cancel);
                    #endregion

                    if (btnSelect == GEnum.MsgBoxButton.Add_New_Record)
                    {
                        #region Open Form for adding new record
                        string currValue = cbo.Text;

                        string listSettingID = GlobalUI.FormSettingID_Get(cbo, GEnum.FormSettingGetType.HeaderListSettingID);
                        string formName = GlobalUI.FormSettingID_Get(cbo, GEnum.FormSettingGetType.HeaderPopupForm);


                        //to split Customer or Vendor ( C or V) in Popupform value 
                        if (formName.IndexOf("%") != -1)
                        {
                            formName = formName.Remove(formName.IndexOf("%"));
                        }

                        OpenFormAsDialog(cbo, GEnum.FormOpenMode.Add, quickAddOption, formName, listSettingID);

                        if (!cbo.IsItemInList(currValue))
                        {
                            MsgBox.Show(MsgID.Common.ItemNotInList + "%" + currValue, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                            e.RetainFocus = true;
                            cbo.PerformAction(UltraComboAction.Dropdown);
                        }
                        else
                        {
                            //cbo.Text = currValue;//this cannot be done in new version, the following two lines of code is needed instead
                            DataTable dt = cbo.DataSource as DataTable;
                            cbo.Value = dt.Select(cbo.DisplayMember + "='" + currValue + "'")[0][cbo.ValueMember];

                            e.RetainFocus = false;
                        }
                        #endregion
                    }
                    else if (btnSelect == GEnum.MsgBoxButton.Try_Again)
                    {
                        e.RetainFocus = true;
                    }
                    else
                    {
                        if (!GFunc.IsNE(e.LastValidValue))
                            cbo.Value = e.LastValidValue;
                        else if (cbo.DisplayLayout.Bands[0].Columns[cbo.ValueMember].DataType == typeof(string))
                            cbo.Value = string.Empty;
                        else
                        {
                            cbo.Value = 0;
                        }
                        e.RetainFocus = false;
                    }
                }
                else
                    e.RetainFocus = false;
            }
            catch (TAException tex)
            {
                throw SysAuditLogUtility.ModifyTAException(tex, true, new object[] { });
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }
        public static void ItemNotInList(UltraGridCell curCell, CellDataErrorEventArgs e, int? quickAddOption)
        {
            bool stayInEditMode = false;
            TAUtil.TAComboBox cbo = null;
            string TextCopy = string.Empty;

            try
            {
                TextCopy = curCell.Text;

                #region Checking for EditorControl of type TAComboBox
                if (curCell.EditorComponent != null)
                {
                    if (curCell.EditorComponent.GetType() == typeof(TAComboBox))
                        cbo = curCell.EditorComponent as TAUtil.TAComboBox;
                }
                else if (curCell.Column.EditorComponent != null)
                {
                    if (curCell.Column.EditorComponent.GetType() == typeof(TAComboBox))
                        cbo = curCell.Column.EditorComponent as TAUtil.TAComboBox;
                }
                else
                {
                    curCell.Value = 0;
                    stayInEditMode = false;
                }
                #endregion

                if (quickAddOption == 0)
                {
                    if (MsgBox.Show(MsgID.Common.ItemNotInList + "%" + TextCopy, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.Try_Again, GEnum.MsgBoxButton.Cancel) == GEnum.MsgBoxButton.Try_Again)
                    {
                        //Perform a drop to tell grid to retain focus and display error value
                        curCell.EditorResolved.DropDown();
                    }
                    else
                    {
                        //Currently the argument(e) is useless so we will let the grid perform the cancelchanges
                        return;
                    }
                }
                else
                {
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.AddNewRecord + "%" + TextCopy, GEnum.MsgBoxIcon.Question, GEnum.MsgBoxButton.Add_New_Record, GEnum.MsgBoxButton.Try_Again, GEnum.MsgBoxButton.Cancel);

                    if (btnSelect == GEnum.MsgBoxButton.Add_New_Record)
                    {

                        TAComboBox tmpcbo = null;
                        tmpcbo = (TAComboBox)curCell.Column.EditorComponent;
                        tmpcbo.Name = curCell.Column.Key;
                        tmpcbo.Text = curCell.Text;
                        string contextMenuSetting = GFunc.NEStr(ActiveFormObject_Get("contextMenuSetting"), "");
                        string listSettingID = GlobalUI.ListSettingID_Get(contextMenuSetting, curCell.Column.Key, curCell.Band.Layout.Grid.Name);
                        string formName = GlobalUI.FormSettingID_PopupFormName_Get(contextMenuSetting, curCell.Column.Key, curCell.Band.Layout.Grid.Name);

                        OpenFormAsDialog(cbo, GEnum.FormOpenMode.Add, quickAddOption, formName, listSettingID);

                        if (cbo.IsItemInList(curCell.Text))
                        {
                            DataTable dt = cbo.DataSource as DataTable;
                            cbo.Value = dt.Select(cbo.DisplayMember + "='" + curCell.Text + "'")[0][cbo.ValueMember];
                            curCell.Value = cbo.Value;
                        }
                        else
                        {
                            MsgBox.Show(MsgID.Common.ItemNotInList + "%" + curCell.Text, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                            curCell.EditorResolved.DropDown();
                        }

                        //commented by jane
                        #region "old code"
                        //Open form for new item entry
                        //OpenFormAsDialog(cbo,curCell, GEnum.FormOpenMode.Add, TextCopy, quickAddOption);

                        #endregion
                        //Return to grid (for it to test if the user have added a new item tht matches the list
                        return;
                    }
                    else if (btnSelect == GEnum.MsgBoxButton.Try_Again)
                    {
                        cbo.SelectedRow = null;
                        //Perform a drop to tell grid to retain focus and display error value
                        curCell.EditorResolved.DropDown();
                    }
                    else
                    {
                        //Currently the argument(e) is useless so we will let the grid perform the cancelchanges
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }

        public static void OpenFormAsDialog(TAUtil.TAComboBox cbo, GEnum.FormOpenMode OpenMode, object curValue, int? option)
        {
            try
            {
                switch (cbo.ValueMember.ToLower())
                {
                    #region CatKey
                    case "catkey":
                        frmREFCat fCat = null;
                        int CatNumber = 0;
                        switch (cbo.Name.ToLower())
                        {
                            case "catkey1":
                            case "cat1":
                                CatNumber = 1;
                                break;
                            case "catkey2":
                            case "cat2":
                                CatNumber = 2;
                                break;
                            case "catkey3":
                            case "cat3":
                                CatNumber = 3;
                                break;
                            case "catkey4":
                            case "cat4":
                                CatNumber = 4;
                                break;
                            case "catkey5":
                            case "cat5":
                                CatNumber = 5;
                                break;
                            default:
                                break;
                        }
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fCat = new frmREFCat((string)curValue, CatNumber);
                        else
                            fCat = new frmREFCat((int)curValue, CatNumber);
                        fCat.ShowDialog();
                        BindComboValue(cbo, "REFCat" + CatNumber + "SortID");
                        break;
                    #endregion

                    case "lockey":
                        frmREFLoc fLoc = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fLoc = new frmREFLoc((string)curValue);
                        else
                            fLoc = new frmREFLoc((int)curValue);
                        fLoc.ShowDialog();
                        // BindComboValue(cbo, GVar.ListSettingID.REFLocSortID);
                        break;

                    case "overheadkey":
                        frmREFOverHead fOverHead = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fOverHead = new frmREFOverHead((string)curValue);
                        else
                            fOverHead = new frmREFOverHead((int)curValue);
                        fOverHead.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.REFOverHeadSortID);
                        break;

                    case "branchkey":
                        frmMstAccBranch fMstAccBranch = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fMstAccBranch = new frmMstAccBranch((string)curValue);
                        else
                            fMstAccBranch = new frmMstAccBranch((int)curValue);
                        fMstAccBranch.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.MSTAccBranchSortID);
                        break;

                    case "idformatkey":
                        frmREFIDFormat fIDFormat = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fIDFormat = new frmREFIDFormat((string)curValue);
                        else
                            fIDFormat = new frmREFIDFormat((int)curValue);
                        fIDFormat.ShowDialog();
                        BindComboValue(cbo, GVar.ListSettingID.ReportSearchFormatCombo);
                        break;

                    case "taxgrpkey":
                        frmREFTaxGrp fREFTaxGrp = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFTaxGrp = new frmREFTaxGrp((string)curValue);
                        else
                            fREFTaxGrp = new frmREFTaxGrp((int)curValue);
                        fREFTaxGrp.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.REFTaxGrpSortID);
                        break;
                    
                    case "currkey":
                    case "doccurrkey":
                        frmREFCurr fREFCurr = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFCurr = new frmREFCurr((string)curValue);
                        else
                            fREFCurr = new frmREFCurr((int)curValue);
                        fREFCurr.ShowDialog();
                        //    BindComboValue(cbo, GVar.ListSettingID.REFCurrencySortID);
                        break;

                    case "paymodekey":
                        frmREFPayMode fREFPayMode = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFPayMode = new frmREFPayMode((string)curValue);
                        else
                            fREFPayMode = new frmREFPayMode((int)curValue);
                        fREFPayMode.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.REFPayModeSortID);
                        break;

                    case "bankkey":
                        frmREFBank fREFBank = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFBank = new frmREFBank((string)curValue);
                        else
                            fREFBank = new frmREFBank((int)curValue);
                        fREFBank.ShowDialog();
                        //   BindComboValue(cbo, GVar.ListSettingID.REFPayModeSortID);
                        break;

                    case "docgrpkey":
                        frmREFDocGrp fREFDocGrp = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFDocGrp = new frmREFDocGrp((string)curValue);
                        else
                            fREFDocGrp = new frmREFDocGrp((int)curValue);
                        fREFDocGrp.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.REFDocGrpSortID);
                        break;

                    case "emkey":
                        frmMstSalesRep fMstSalesRep = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fMstSalesRep = new frmMstSalesRep((string)curValue);
                        else
                            fMstSalesRep = new frmMstSalesRep((int)curValue);
                        fMstSalesRep.ShowDialog();
                        //   BindComboValue(cbo, GVar.ListSettingID.MSTSaleRepSortID);
                        break;

                    case "deptkey":
                        frmMstAccDept fMstAccDept = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fMstAccDept = new frmMstAccDept((string)curValue);
                        else
                            fMstAccDept = new frmMstAccDept((int)curValue);
                        fMstAccDept.ShowDialog();
                        //   BindComboValue(cbo, GVar.ListSettingID.MSTDeptSortID);
                        break;

                    case "trangrpkey":
                        frmMstAccTranGrp fMstAccTranGrp = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fMstAccTranGrp = new frmMstAccTranGrp((string)curValue);
                        else
                            fMstAccTranGrp = new frmMstAccTranGrp((int)curValue);
                        fMstAccTranGrp.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.MSTAccTranGrpSortID);
                        break;

                    case "jobkey":
                        frmMSTJob fMSTJob = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fMSTJob = new frmMSTJob((string)curValue);
                        else
                            fMSTJob = new frmMSTJob((int)curValue);
                        fMSTJob.ShowDialog();

                        string msgID = string.Empty; //May added  when removing msgID parameter from this function
                        DataTable dt = MASList.GetMSTJobs(out msgID, (int)option, 0, 0, string.Empty, 0);
                        cbo.DataSource = dt;
                        break;

                    case "jobphasekey":
                        frmREFJobPhase fREFJobPhase = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFJobPhase = new frmREFJobPhase((string)curValue);
                        else
                            fREFJobPhase = new frmREFJobPhase((int)curValue);
                        fREFJobPhase.ShowDialog();
                        // BindComboValue(cbo, GVar.ListSettingID.REFJobPhaseSortID);
                        break;

                    case "jobtaskkey":
                        frmREFJobTask fREFJobTask = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFJobTask = new frmREFJobTask((string)curValue);
                        else
                            fREFJobTask = new frmREFJobTask((int)curValue);
                        fREFJobTask.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.REFJobTaskSortID);
                        break;

                    case "jobcosttypekey":
                        frmREFJobCostType fREFJobCostType = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFJobCostType = new frmREFJobCostType((string)curValue);
                        else
                            fREFJobCostType = new frmREFJobCostType((int)curValue);
                        fREFJobCostType.ShowDialog();
                        //   BindComboValue(cbo, GVar.ListSettingID.REFJobCostTypeSortID);
                        break;
                    case "shipname":
                        frmMSTShipName ffrmMSTShipName = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                           ffrmMSTShipName = new frmMSTShipName((string)curValue);                        
                        else
                            ffrmMSTShipName = new frmMSTShipName((int)curValue);
                        ffrmMSTShipName.ShowDialog();
                        //  BindComboValue(cbo, GVar.ListSettingID.REFJobCostTypeSortID);
                        break;
                    case "uomkey":
                        frmREFUOM fREFUOM = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFUOM = new frmREFUOM((string)curValue);
                        else
                            fREFUOM = new frmREFUOM((int)curValue);
                        fREFUOM.ShowDialog();
                        switch (cbo.Name.ToLower())
                        {
                            case "assuomkey":
                            case "bombuomkey":
                            case "buomkey":
                            case "detitmuomkey":
                            case "docmeasuomkey":
                            case "doctlmeasuomkey":
                            case "estuomkey":
                            case "fgbuomkey":
                            case "itmuomkey":
                            case "othuomkey":
                            case "uomconkey":
                            case "uomkey":
                                // BindComboValue(cbo, GVar.ListSettingID.RERUOMSortID);
                                break;
                            case "docweightuomkey":
                            case "doctlweightuomkey":
                            case "detitmweightuomkey":
                            case "fgweightuomkey":
                            case "bomweightuomkey":
                            case "weightuomkey":
                                // BindComboValue(cbo, GVar.ListSettingID.REFUOMWeightSortID);
                                break;
                            default:
                                break;
                        }
                        break;

                    case "itmkey":
                        frmMSTItm fItem = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fItem = new frmMSTItm((string)curValue, false);
                        else
                            fItem = new frmMSTItm((int)curValue);
                        fItem.ShowDialog();
                        switch (cbo.Name.ToLower())
                        {
                            case "assitmkey":
                                //  GlobalUI.BindComboValue(cbo, GVar.ListSettingID.MSTItmAssemblyList);
                                break;
                            case "batchitmkey":
                            case "bomitmkey":
                            case "detitmkey":
                            case "detitmkeyselect":
                            case "eqptitmkey":
                            case "eqptitmkeyselect":
                            case "eqptsubdetitmkey":
                            case "eqptsubdetitmkeyselect":
                            case "eqptsubitmkey":
                            case "eqptsubitmkeyselect":
                            case "estitmkey":
                            case "estitmkeyselect":
                            case "itmfgkey":
                            case "itmfgkeyselect":
                            case "itmkeyselect":
                            case "joblabouritmkey":
                            case "othitmkey":
                            case "othitmkeyselect":
                            case "substituteitmkey":
                            case "itmkey":
                                //     GlobalUI.BindComboValue(cbo, GVar.ListSettingID.MSTItmCombo);
                                break;
                            case "masteritmkey":
                            case "masterkey":
                                //   GlobalUI.BindComboValue(cbo, GVar.ListSettingID.MSTItmSortMasterItmKey);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "docvendorkey":
                    case "conkey":
                        frmMSTCon fCon = new frmMSTCon(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fCon = new frmMSTCon((string)curValue);
                        else
                            fCon = new frmMSTCon((int)curValue);
                        fCon.ShowDialog();
                        break;
                    case "territorykey":
                        frmREFTerritory fTerritory = new frmREFTerritory(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fTerritory = new frmREFTerritory((string)curValue);
                        else
                            fTerritory = new frmREFTerritory((int)curValue);
                        fTerritory.ShowDialog();
                        break;
                    case "industrykey":
                        frmREFIndustry ffrmREFIndustry = new frmREFIndustry(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            ffrmREFIndustry = new frmREFIndustry((string)curValue);
                        else
                            ffrmREFIndustry = new frmREFIndustry((int)curValue);
                        ffrmREFIndustry.ShowDialog();
                        break;
                    case "termkey":
                        frmREFTerm ffrmREFTerm = new frmREFTerm(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            ffrmREFTerm = new frmREFTerm((string)curValue);
                        else
                            ffrmREFTerm = new frmREFTerm((int)curValue);
                        ffrmREFTerm.ShowDialog();
                        break;
                    case "pricekey":
                        frmMSTPriceInfo ffrmMSTPriceInfo = new frmMSTPriceInfo(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            ffrmMSTPriceInfo = new frmMSTPriceInfo((string)curValue);
                        else
                            ffrmMSTPriceInfo = new frmMSTPriceInfo((int)curValue);
                        ffrmMSTPriceInfo.ShowDialog();
                        break;
                    case "brandkey":
                        frmREFBrand ffrmREFBrand = new frmREFBrand(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            ffrmREFBrand = new frmREFBrand((string)curValue);
                        else
                            ffrmREFBrand = new frmREFBrand((int)curValue);
                        ffrmREFBrand.ShowDialog();
                        break;
                    case "scalekey":
                        frmREFScale ffrmREFScale = new frmREFScale(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            ffrmREFScale = new frmREFScale((string)curValue);
                        else
                            ffrmREFScale = new frmREFScale((int)curValue);
                        ffrmREFScale.ShowDialog();
                        GlobalUI.BindComboValue(cbo, GVar.ListSettingID.REFScale);
                        break;                    
                    case "taxkey":
                        frmREFTaxA ffrmREFTaxA = new frmREFTaxA(curValue.ToString());
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            ffrmREFTaxA = new frmREFTaxA((string)curValue);
                        else
                            ffrmREFTaxA = new frmREFTaxA((int)curValue);
                        ffrmREFTaxA.ShowDialog();
                        break;
                    case "key":
                        if (GFunc.CompareString(cbo.Name, "AccKey"))
                        {
                            frmMstAcc frmMstAcc = null;
                            if (OpenMode == GEnum.FormOpenMode.Add)
                                frmMstAcc = new frmMstAcc((string)curValue);
                            else
                                frmMstAcc = new frmMstAcc((int)curValue);
                            frmMstAcc.ShowDialog();
                        }
                        break;
                    case "accgrpkey":
                        frmREFAccGrp frmREFAccGrp = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            frmREFAccGrp = new frmREFAccGrp((string)curValue);
                        else
                            frmREFAccGrp = new frmREFAccGrp((int)curValue);
                        frmREFAccGrp.ShowDialog();
                        break;
                }

                BindComboValue(cbo, GFunc.NEStr(cmnuGlobal.Tag, ""));
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }
        public static void OpenFormAsDialog(TAUtil.TATextBoxEditor txt, GEnum.FormOpenMode OpenMode, object curValue, int? option)
        {
            try
            {
                switch (txt.Tag.ToString().ToLower())
                {
                    case "customervendor":

                        frmMSTCon fMstCon = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fMstCon = new frmMSTCon((string)curValue);
                        else
                            fMstCon = new frmMSTCon((int)curValue);
                        fMstCon.ShowDialog();
                        break;
                    case "inventory":
                        frmMSTItm fMstItm = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fMstItm = new frmMSTItm((string)curValue, false);
                        else
                            fMstItm = new frmMSTItm((int)curValue);
                        fMstItm.ShowDialog();
                        break;
                    case "job":
                        frmREFTaxGrp fREFTaxGrp = null;
                        if (OpenMode == GEnum.FormOpenMode.Add)
                            fREFTaxGrp = new frmREFTaxGrp((string)curValue);
                        else
                            fREFTaxGrp = new frmREFTaxGrp((int)curValue);
                        fREFTaxGrp.ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }

        //----------New open form Dialog for combo NotinListAdd--------//
        public static void OpenFormAsDialog(TAUtil.TAComboBox cbo, GEnum.FormOpenMode OpenMode, int? option, string formName, string listSettingID)
        {
            try
            {

                TAUtil.TAComboBox cboControl = cbo;
                TAUtil.TATextBoxEditor txtControl = null;
                Control activeControl = cbo;
                string activeCrlText = OpenMode == GEnum.FormOpenMode.Add ? cbo.Text : cbo.Value.ToString();//For string accepted constructor
                AssemblyName oAName = Assembly.GetExecutingAssembly().GetName();


                GEnum.SystemCode _conType = GEnum.SystemCode.Customer;
                string ConType = "C";

                //to split Customer or Vendor ( C or V) in Popupform value that set in cmnuGlobal.AccessibleDescription
                if (formName.IndexOf("%") != -1)
                {
                    ConType = formName.Substring(formName.IndexOf("%") + 1, 1);
                    formName = formName.Remove(formName.IndexOf("%"));
                }

                #region Get Code Key
                int codeKey = ActiveFormDocCodeAll_Get();

                #region Old code
                /*
                GEnum.SystemCode conType = GEnum.SystemCode.Customer;
                switch (codeKey)
                {
                    case (int)GEnum.SystemCode.Purchase_Plan:
                    case (int)GEnum.SystemCode.Purchase_Order:
                    case (int)GEnum.SystemCode.Purchase_Order_Adjustment:
                    case (int)GEnum.SystemCode.Purchase_Delivery:
                    case (int)GEnum.SystemCode.Purchase_Invoice:
                    case (int)GEnum.SystemCode.Purchase_Debit_Note:
                    case (int)GEnum.SystemCode.Purchase_Credit_Note:
                    case (int)GEnum.SystemCode.Purchase_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Issue:
                    case (int)GEnum.SystemCode.AP_Opening_Balance:
                    case (int)GEnum.SystemCode.Order_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Order_Adjustment:
                    case (int)GEnum.SystemCode.Received_Consignment:
                    case (int)GEnum.SystemCode.Consignment_Settlement:
                    case (int)GEnum.SystemCode.Inventory:
                        conType = GEnum.SystemCode.Vendor;
                        break;
                    case (int)GEnum.SystemCode.Quotation:
                    case (int)GEnum.SystemCode.Sales_Order:
                    case (int)GEnum.SystemCode.Sales_Order_Adjustment:
                    case (int)GEnum.SystemCode.Delivery_Order:
                    case (int)GEnum.SystemCode.Packing_List:
                    case (int)GEnum.SystemCode.Sales_Invoice:
                    case (int)GEnum.SystemCode.Sales_Debit_Note:
                    case (int)GEnum.SystemCode.Sales_Credit_Note:
                    case (int)GEnum.SystemCode.Sales_Adjustment:
                    case (int)GEnum.SystemCode.Payment_Received:
                    case (int)GEnum.SystemCode.AR_Opening_Balance:
                    case (int)GEnum.SystemCode.Contra:
                    case (int)GEnum.SystemCode.Cash_Sale:
                    case (int)GEnum.SystemCode.Cash_Debit_Note:
                    case (int)GEnum.SystemCode.Cash_Credit_Note:
                    case (int)GEnum.SystemCode.Cash_Adjustment:
                    case (int)GEnum.SystemCode.Cash_Payment_Received:
                    case (int)GEnum.SystemCode.AR_Cash_Opening_Balance:
                    case (int)GEnum.SystemCode.Cash_Contra:
                    case (int)GEnum.SystemCode.Issue_Consignment:
                    case (int)GEnum.SystemCode.Return_Consignment:
                    case (int)GEnum.SystemCode.CounterGrp://All , Default Cust
                    case (int)GEnum.SystemCode.Currency://All , Default Cust
                    case (int)GEnum.SystemCode.Ship_Name:
                    case (int)GEnum.SystemCode.Job:
                    case (int)GEnum.SystemCode.Machine_List://All , Default Cust
                    case (int)GEnum.SystemCode.Financial_Charge:
                        conType = GEnum.SystemCode.Customer;
                        break;
                }
                
                */

                #endregion

                #endregion


                if (GFunc.CompareString(ConType, "C"))
                {
                    _conType = GEnum.SystemCode.Customer;
                }
                else
                {
                    _conType = GEnum.SystemCode.Vendor;
                }

                Form objectForm = null;
                if (GFunc.CompareString(formName, "frmREFCat")) //Limitation: This Code needs the control name be followed by 1,2,3,4,5 bcos it use the last char of control to pass them to Category Form as constructor param
                    objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText, Convert.ToInt16(cboControl.Name.Substring(cboControl.Name.Length - 1)) }, null, null);
                else if (GFunc.CompareString(formName.ToUpper(), "frmMstCon".ToUpper()))
                    objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText, _conType }, null, null);
                else
                    objectForm = (Form)Assembly.Load(oAName.Name).CreateInstance("WinUI." + formName, true, new BindingFlags(), null, new object[] { activeCrlText }, null, null);

                //Show The Dialog
                objectForm.ShowDialog();

                //Update the DataSource Again
                if (cboControl != null)
                {
                    if (dependenFillEvent == null)
                        BindComboValue(cboControl, listSettingID);
                    else
                        dependenFillEvent.Invoke(cboControl.Name);//If the form needs to fill Dependent Combo                 
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// To Select Or Focus The Next Comtrol That We make Key 'Enter/Tab/UpArrow/DownArrow/Shift+Tab
        /// </summary>
        /// <param name="frm">Active Form</param>
        /// <param name="e">KeyEventArgs is mean KeyUp(Michal Recommend) Or KeyDown</param>
        public static void SelectNextControl(System.Windows.Forms.Form frm, KeyEventArgs e)
        {
            try
            {
                ConstructorInfo EventCInfo = e.GetType().GetConstructor(Type.EmptyTypes);

                #region declaration
                int SwtC = 0;
                bool SuppressKey = false;
                bool SelectAll = true;
                //We Can't Use frm.ActiveControl, The frm.ActiveControl is wrong for the control is place inside at 'splitContainer/groupbox/Panel'
                Control FocusControl = GetActiveControlAtActiveForm();

                if (FocusControl != null)
                {
                    if (FocusControl.GetType() == typeof(Button) || FocusControl.GetType() == typeof(Infragistics.Win.Misc.UltraButton))
                        return;
                }
                if (FocusControl != null)
                {
                    TAUtil.TATextBoxEditor TmpText = null;
                    TAUtil.TAComboBox TmpCombo = null;
                    TAUtil.TANumericEditor TmpNumber = null;
                    TAUtil.TADateEditor TmpDate = null;
                    TAUtil.TAGridEditor TmpGrid = null;
                    TAUtil.TACheckBoxEditor TmpCheck = null;
                    System.Windows.Forms.DateTimePicker TmpTime = null;
                    Infragistics.Win.UltraWinTabControl.UltraTabControl TmpTap = null;
                    System.Windows.Forms.RadioButton TmpRadio = null;
                #endregion

                    #region get control type
                    //Number Editor
                    if (FocusControl.GetType() == typeof(TAUtil.TANumericEditor))
                    {
                        TmpNumber = (TAUtil.TANumericEditor)FocusControl;
                        SwtC = 1;
                    }
                    else if(FocusControl.Parent!=null)
                       if(FocusControl.Parent.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            TmpNumber = (TAUtil.TANumericEditor)FocusControl.Parent;
                            SwtC = 1;
                        }

                    //Textbox
                    if (FocusControl.GetType() == typeof(TAUtil.TATextBoxEditor))
                    {
                        TmpText = (TAUtil.TATextBoxEditor)FocusControl;
                        SwtC = 2;
                    }
                    else if (FocusControl.Parent != null)
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            TmpText = (TAUtil.TATextBoxEditor)FocusControl.Parent;
                            SwtC = 2;
                        }

                    //Combobox
                    if (FocusControl.GetType() == typeof(TAUtil.TAComboBox))
                    {
                        TmpCombo = (TAUtil.TAComboBox)FocusControl;
                        SwtC = 3;
                    }
                    else if (FocusControl.Parent != null)
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                    {
                        TmpCombo = (TAUtil.TAComboBox)FocusControl.Parent;
                        SwtC = 3;
                    }

                    //Date Editor
                    if (FocusControl.GetType() == typeof(TAUtil.TADateEditor))
                    {
                        TmpDate = (TAUtil.TADateEditor)FocusControl;
                        SwtC = 4;
                    }
                    else if (FocusControl.Parent != null)
                    {
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            TmpDate = (TAUtil.TADateEditor)FocusControl.Parent;
                            SwtC = 4;
                        }
                    }
                    
                    if (FocusControl.GetType() == typeof(System.Windows.Forms.DateTimePicker))
                    {
                        TmpTime = (System.Windows.Forms.DateTimePicker)FocusControl;
                        SwtC = 9;
                    }
                    else  if (FocusControl.Parent != null)
                    {
                        if (FocusControl.Parent.GetType() == typeof(System.Windows.Forms.DateTimePicker))
                        {
                            TmpTime = (System.Windows.Forms.DateTimePicker)FocusControl.Parent;
                            SwtC = 9;
                        }
                    }

                    //Grid
                    if (FocusControl.Parent != null)
                    {
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TAGridEditor))
                        {
                            TmpGrid = (TAUtil.TAGridEditor)FocusControl.Parent;
                            SwtC = 5;
                        }
                    }
                    else if (FocusControl.GetType() == typeof(TAUtil.TAGridEditor))
                    {
                        TmpGrid = (TAUtil.TAGridEditor)FocusControl;
                        SwtC = 5;
                    }

                    //Check Box
                    if (FocusControl.GetType() == typeof(TAUtil.TACheckBoxEditor))
                    {
                        TmpCheck = (TAUtil.TACheckBoxEditor)FocusControl;
                        SwtC = 6;
                    }

                    //Tab Contril
                    if (FocusControl.GetType() == typeof(UltraTabControl))
                    {
                        TmpTap = (UltraTabControl)FocusControl;
                        SwtC = 7;
                    }

                    //Radio Button
                    if (FocusControl.GetType() == typeof(RadioButton))
                    {
                        TmpRadio = (RadioButton)FocusControl;
                        SwtC = 8;
                    }
                    else if (FocusControl.Parent != null)
                    {
                        if (FocusControl.Parent.GetType() == typeof(RadioButton))
                        {
                            TmpRadio = (RadioButton)FocusControl.Parent;
                            SwtC = 8;
                        }
                    }
                    #endregion

                    #region (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)

                    if ((e.KeyCode == Keys.Down && !(e.Control || e.Shift || e.Alt)) || ((e.KeyCode == Keys.Enter) && !(e.Shift || e.Alt)))
                    {
                        switch (SwtC)
                        {
                            case 1://TANumericEditor                                
                                frm.SelectNextControl(TmpNumber, true, true, true, true);
                                SuppressKey = true;
                                break;

                            case 2://TATextBoxEditor
                                if ((e.KeyCode == Keys.Enter && e.Control == true))
                                {
                                    SuppressKey = false;
                                    SelectAll = false;
                                }
                                else
                                {
                                    if (TmpText.Multiline)
                                    {
                                        int SelStart = TmpText.SelectionStart;
                                        string sText = string.Empty;
                                        sText = TmpText.Text.Substring(SelStart, TmpText.Text.Length - SelStart);

                                        if (e.KeyCode == Keys.Down && TmpText.IsInEditMode && TmpText.SelectionLength < TmpText.TextLength && TmpText.TextLength > 0 && sText.Contains("\r\n") == true)
                                        {
                                            SuppressKey = false;
                                            SelectAll = false;

                                        }
                                        else
                                        {
                                            frm.SelectNextControl(TmpText, true, true, true, true);
                                            SuppressKey = true;
                                        }
                                    }
                                    else
                                    {
                                        frm.SelectNextControl(TmpText, true, true, true, true);
                                        SuppressKey = true;
                                    }
                                }
                                break;
                            case 4://TADateEditor
                                frm.SelectNextControl(TmpDate, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 6://TACheckBox
                                frm.SelectNextControl(TmpCheck, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 3://TAComboBox
                                if (e.KeyCode == Keys.Enter)
                                {
                                    frm.SelectNextControl(TmpCombo, true, true, true, true);
                                    SuppressKey = true;
                                }
                                else
                                {
                                    if (TmpCombo.IsDroppedDown == false)
                                    {
                                        frm.SelectNextControl(TmpCombo, true, true, true, true);
                                        SuppressKey = true;
                                    }
                                }
                                break;
                            case 5://TAGridEditor
                                //DO NOT WRITE any code for Grid, it is implemented in the custom Grid control------Mic
                                //if (e.KeyCode == Keys.Enter)
                                //{
                                //    TmpGrid.PerformAction(UltraGridAction.ActivateCell);
                                //    if (TmpGrid.ActiveCell != null)
                                //    {
                                //        if (TmpGrid.ActiveCell.IsInEditMode)
                                //        {
                                //            TmpGrid.ActiveCell.SelectAll();
                                //            if (TmpGrid.ActiveCell.Column.DataType == typeof(bool))
                                //            {
                                //                TmpGrid.PerformAction(UltraGridAction.ExitEditMode);
                                //                TmpGrid.PerformAction(UltraGridAction.NextCellByTab);
                                //                SuppressKey = true;
                                //            }
                                //            else if (TmpGrid.ActiveCell.SelText == TmpGrid.ActiveCell.Text || TmpGrid.ActiveCell.SelStart == TmpGrid.ActiveCell.Text.Length)
                                //            {
                                //                TmpGrid.PerformAction(UltraGridAction.ExitEditMode);
                                //                TmpGrid.PerformAction(UltraGridAction.NextCellByTab);
                                //                SuppressKey = true;
                                //            }
                                //        }
                                //    }
                                //}
                                break;
                            case 7://UltraTabControl
                                frm.SelectNextControl(TmpTap, true, true, true, true);
                                SuppressKey = false;
                                break;
                            case 8://Radio
                                frm.SelectNextControl(TmpRadio, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 9:
                                frm.SelectNextControl(TmpTime, true, true, true, true);
                                SuppressKey = false;
                                break;
                            default:
                                break;
                        }

                        #region selecting text in next control
                        Control NextCtrl = GetActiveControlAtActiveForm();

                        //Number Editor
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            ((TAUtil.TANumericEditor)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            ((TAUtil.TANumericEditor)NextCtrl).SelectAll();
                        }
                        //Textbox
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            if (((TAUtil.TATextBoxEditor)NextCtrl.Parent).Multiline == false)
                            {
                                ((TAUtil.TATextBoxEditor)NextCtrl.Parent).SelectAll();
                            }
                            else
                            {
                                if (SelectAll == true)
                                    ((TAUtil.TATextBoxEditor)NextCtrl.Parent).SelectAll();
                            }

                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            if (((TAUtil.TATextBoxEditor)NextCtrl).Multiline == false)
                            {
                                ((TAUtil.TATextBoxEditor)NextCtrl).SelectAll();
                            }
                            else
                            {
                                if (SelectAll == true)
                                    ((TAUtil.TATextBoxEditor)NextCtrl).SelectAll();
                            }

                        }

                        //Combobox
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            ((TAUtil.TAComboBox)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            ((TAUtil.TAComboBox)NextCtrl).SelectAll();

                        }

                        //Date Editor
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            ((TAUtil.TADateEditor)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            ((TAUtil.TADateEditor)NextCtrl).SelectAll();
                        }

                        #endregion


                    }
                    #endregion

                    #region (e.KeyCode == Keys.Up)

                    else if ((e.KeyCode == Keys.Up) && !(e.Shift || e.Control || e.Alt))
                    {
                        switch (SwtC)
                        {
                            case 1://TANumericEditor
                                frm.SelectNextControl(TmpNumber, false, true, true, true);
                                SuppressKey = true;
                                break;
                            case 2://TATextBoxEditor
                                if (TmpText.Multiline)
                                {
                                    if (e.KeyCode == Keys.Up && TmpText.IsInEditMode && TmpText.SelectionLength < TmpText.TextLength && TmpText.TextLength > 0)
                                    {
                                        int SelStart = TmpText.SelectionStart;
                                        string sText = string.Empty;
                                        sText = TmpText.Text.Substring(0, SelStart);
                                        if (sText.Contains("\r\n") != true)
                                        {
                                            frm.SelectNextControl(TmpText, false, true, true, true);
                                            SuppressKey = true;
                                        }
                                        else
                                            SelectAll = false;
                                    }
                                    else
                                    {
                                        frm.SelectNextControl(TmpText, false, true, true, true);
                                        SuppressKey = true;
                                    }
                                }
                                else
                                {
                                    frm.SelectNextControl(TmpText, false, true, true, true);
                                    SuppressKey = true;
                                }
                                break;
                            case 4://TADateEditor
                                frm.SelectNextControl(TmpDate, false, true, true, true);
                                SuppressKey = true;
                                break;
                            case 6://TACheckBox
                                frm.SelectNextControl(TmpCheck, false, true, true, true);
                                SuppressKey = true;
                                break;
                            case 3://TAComboBox
                                if (TmpCombo.IsDroppedDown == false)
                                {
                                    frm.SelectNextControl(TmpCombo, false, true, true, true);
                                    SuppressKey = true;
                                }
                                break;
                            case 7://UltraTabControl
                                frm.SelectNextControl(TmpTap, false, true, true, true);
                                SuppressKey = true;
                                break;
                            case 8://Radio
                                frm.SelectNextControl(TmpRadio, false, true, true, true);
                                SuppressKey = true;
                                break;
                            case 9:
                                frm.SelectNextControl(TmpTime, false, true, true, true);
                                SuppressKey = false;
                                break;
                            default:
                                break;
                        }

                        #region selecting text in next control
                        Control NextCtrl = GetActiveControlAtActiveForm();
                        //Number Editor
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            ((TAUtil.TANumericEditor)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            ((TAUtil.TANumericEditor)NextCtrl).SelectAll();
                        }
                        //Textbox
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            if (((TAUtil.TATextBoxEditor)NextCtrl.Parent).Multiline == false)
                            {
                                ((TAUtil.TATextBoxEditor)NextCtrl.Parent).SelectAll();
                            }
                            else
                            {
                                if (SelectAll == true)
                                    ((TAUtil.TATextBoxEditor)NextCtrl.Parent).SelectAll();
                            }
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            if (((TAUtil.TATextBoxEditor)NextCtrl).Multiline == false)
                            {
                                ((TAUtil.TATextBoxEditor)NextCtrl).SelectAll();
                            }
                            else
                            {
                                if (SelectAll == true)
                                    ((TAUtil.TATextBoxEditor)NextCtrl).SelectAll();
                            }
                        }

                        //Combobox
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            ((TAUtil.TAComboBox)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            ((TAUtil.TAComboBox)NextCtrl).SelectAll();

                        }

                        //Date Editor
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            ((TAUtil.TADateEditor)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            ((TAUtil.TADateEditor)NextCtrl).SelectAll();
                        }


                        #endregion
                    }
                    #endregion

                    #region (e.KeyCode == Keys.Right)

                    else if ((e.KeyCode == Keys.Right) && !(e.Shift || e.Control || e.Alt))
                    {
                        switch (SwtC)
                        {
                            case 1://TANumericEditor
                            case 2://TATextBoxEditor
                            case 4://TADateEditor
                            case 6://TACheckBox
                            case 3://TAComboBox
                            case 7://UltraTabControl
                                break;
                            case 5://TAGridEditor
                                //TmpGrid.PerformAction(UltraGridAction.ActivateCell);
                                //if (TmpGrid.ActiveCell != null)
                                //{
                                // if (TmpGrid.ActiveCell.IsInEditMode)
                                // {
                                // if (TmpGrid.ActiveCell.Column.DataType == typeof(bool))
                                // {
                                // TmpGrid.PerformAction(UltraGridAction.ExitEditMode);
                                // TmpGrid.PerformAction(UltraGridAction.NextCellByTab);
                                // SuppressKey = true;
                                // }
                                // else if (TmpGrid.ActiveCell.SelText == TmpGrid.ActiveCell.Text || TmpGrid.ActiveCell.SelStart == TmpGrid.ActiveCell.Text.Length)
                                // {
                                // TmpGrid.PerformAction(UltraGridAction.ExitEditMode);
                                // TmpGrid.PerformAction(UltraGridAction.NextCellByTab);
                                // SuppressKey = true;
                                // }
                                // }
                                //}
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region (e.KeyCode == Keys.Left)

                    else if ((e.KeyCode == Keys.Left) && !(e.Shift || e.Control || e.Alt))
                    {
                        switch (SwtC)
                        {
                            case 1://TANumericEditor
                            case 2://TATextBoxEditor
                            case 4://TADateEditor
                            case 6://TACheckBox
                            case 3://TAComboBox
                            case 7://UltraTabControl
                                break;
                            case 5://TAGridEditor
                                //TmpGrid.PerformAction(UltraGridAction.ActivateCell);
                                //if (TmpGrid.ActiveCell != null)
                                //{
                                // if (TmpGrid.ActiveCell.IsInEditMode)
                                // {
                                // if (TmpGrid.ActiveCell.Column.DataType == typeof(bool))
                                // {
                                // TmpGrid.PerformAction(UltraGridAction.ExitEditMode);
                                // TmpGrid.PerformAction(UltraGridAction.PrevCellByTab);
                                // SuppressKey = true;
                                // }
                                // else if (TmpGrid.ActiveCell.SelText == TmpGrid.ActiveCell.Text || TmpGrid.ActiveCell.SelStart == 0)
                                // {
                                // TmpGrid.PerformAction(UltraGridAction.ExitEditMode);
                                // TmpGrid.PerformAction(UltraGridAction.PrevCellByTab);
                                // SuppressKey = true;
                                // }
                                // }
                                //}
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region (e.KeyCode == Keys.F2)

                    else if ((e.KeyCode == Keys.F2) && !(e.Shift || e.Control || e.Alt))
                    {
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            if (TmpText.Text != "")
                            {
                                if (TmpText.SelectionLength != TmpText.TextLength)
                                    TmpText.SelectAll();
                                else
                                    TmpText.SelectionStart = TmpText.TextLength;
                            }
                        }
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            if (TmpNumber.Text != "")
                            {
                                if (TmpNumber.SelectionLength != TmpNumber.TextLength)
                                    TmpNumber.SelectAll();
                                else
                                    TmpNumber.SelectionStart = TmpNumber.TextLength;
                            }
                        }
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            if (TmpCombo.Text != "")
                            {
                                if (TmpCombo.Textbox.SelectionLength != TmpCombo.Textbox.TextLength)
                                    TmpCombo.Textbox.SelectAll();
                                else
                                    TmpCombo.Textbox.SelectionStart = TmpCombo.Textbox.TextLength;
                            }
                        }
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            if (TmpDate.Text != "")
                            {
                                if (TmpDate.SelectionLength != TmpDate.TextLength)
                                    TmpDate.SelectAll();
                                else
                                    TmpDate.SelectionStart = TmpDate.TextLength;
                            }
                        }
                        if (FocusControl.Parent.GetType() == typeof(TAUtil.TAGridEditor) || FocusControl.GetType() == typeof(TAUtil.TAGridEditor))
                        {
                            TmpGrid.PerformAction(UltraGridAction.ActivateCell);
                        }
                    }
                    #endregion

                    #region (e.KeyCode == Keys.F4)

                    else if (e.KeyCode == Keys.F4)
                    {
                        if (e.Shift || e.Control || e.Alt)
                        {
                            SuppressKey = true;
                        }
                        //if (TmpGrid != null)
                        //{
                        // if (TmpGrid.ActiveCell != null)
                        // {
                        // if (TmpGrid.ActiveCell.Column.EditorComponent != null)
                        // {
                        // if (TmpGrid.ActiveCell.Column.EditorComponent.GetType() == typeof(TATextBoxEditor))
                        // {
                        // TmpText = (TATextBoxEditor)TmpGrid.ActiveCell.Column.EditorComponent;
                        // }
                        // }
                        // }
                        //}

                    }
                    #endregion

                    #region (e.KeyCode == Keys.F6)

                    else if ((e.KeyCode == Keys.F6) && !(e.Shift || e.Control || e.Alt))
                    {
                        string EventForGridCell = "";
                        //if (TmpGrid != null)
                        //{
                        // if (TmpGrid.ActiveCell != null)
                        // {
                        // if (TmpGrid.ActiveCell.Column.EditorComponent != null)
                        // {
                        // if (TmpGrid.ActiveCell.Column.EditorComponent.GetType() == typeof(TAComboBox))
                        // {
                        // TmpCombo = (TAComboBox)TmpGrid.ActiveCell.Column.EditorComponent;

                        // //Check Run Event For Grid
                        // if (TmpCombo.ButtonsRight.Count > 0)
                        // { EventForGridCell = TmpCombo.ButtonsRight[0].Key; }
                        // }
                        // }
                        // }
                        //}

                        if (TmpText != null)
                        {
                            if (TmpText.ButtonsRight.Count > 0)
                            {
                                if (TmpText.ButtonsRight[0].Enabled)
                                {
                                    //Fire The Active Form's Active TATextEditorControl Editor button Click.
                                    //Find And Get Methods Or Event That We Want to Invoke.
                                    MethodInfo[] Methods = frm.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                                    //Search Function : Find For 'Event' By Control Name And Event Name. If We Want 'Method' Use Methods Name To Search.
                                    MethodInfo method = Methods.Single(p => p.Name.Contains(TmpText.Name) & p.Name.Contains("EditorButtonClick"));

                                    //EventInfo Need Only For 'Event' Which We Want To Invoke. No Need To Inplement For 'Method' Invoke.
                                    EventInfo eventInfo = TmpText.GetType().GetEvent("EditorButtonClick");
                                    //Get The Event Type (eg. EditorButtonClick, ButtonClike,FormLoad,FormClosed)
                                    Type type = eventInfo.EventHandlerType;

                                    //Create Dynamic Delegate To Fire The Event.
                                    Delegate handler = Delegate.CreateDelegate(type, frm, method);
                                    //Create New Event For The Event Which Type Is Same With Invoke Event Type.
                                    EditorButtonEventArgs edit_e = new EditorButtonEventArgs(TmpText.ButtonsRight[0], TmpText);

                                    // Raise the event. Then Invoke Event.
                                    frm.Invoke(handler, new object[] { TmpText, edit_e });
                                }

                            }
                        }
                        if (TmpCombo != null)
                        {
                            if (GFunc.CompareString(EventForGridCell, "F6") == false)
                            {
                                if (TmpCombo.ButtonsRight.Count > 0)
                                {
                                    if (TmpCombo.ButtonsRight[0].Enabled)
                                    {
                                        //Fire The Active Form's Active TATextEditorControl Editor button Click.
                                        //Find And Get Methods Or Event That We Want to Invoke.
                                        MethodInfo[] Methods = frm.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                                        //Search Function : Find For 'Event' By Control Name And Event Name. If We Want 'Method' Use Methods Name To Search.

                                        if (Methods.Count(p => p.Name.Contains(TmpCombo.Name) & p.Name.Contains("EditorButtonClick")) > 0)
                                        {
                                            MethodInfo method = Methods.Single(p => p.Name.Contains(TmpCombo.Name) & p.Name.Contains("EditorButtonClick"));

                                            //EventInfo Need Only For 'Event' Which We Want To Invoke. No Need To Inplement For 'Method' Invoke.
                                            EventInfo eventInfo = TmpCombo.GetType().GetEvent("EditorButtonClick");
                                            //Get The Event Type (eg. EditorButtonClick, ButtonClike,FormLoad,FormClosed)
                                            Type type = eventInfo.EventHandlerType;

                                            //Create Dynamic Delegate To Fire The Event.
                                            Delegate handler = Delegate.CreateDelegate(type, frm, method);
                                            //Create New Event For The Event Which Type Is Same With Invoke Event Type.
                                            EditorButtonEventArgs edit_e = new EditorButtonEventArgs(TmpCombo.ButtonsRight[0], TmpCombo);

                                            // Raise the event. Then Invoke Event.
                                            frm.Invoke(handler, new object[] { TmpCombo, edit_e });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MethodInfo[] Methods = frm.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                                MethodInfo method = Methods.Single(p => p.Name.Contains(TmpGrid.Name) & p.Name.Contains("ClickCellButton"));

                                EventInfo eventInfo = TmpGrid.GetType().GetEvent("ClickCellButton");
                                Type type = eventInfo.EventHandlerType;
                                Delegate handler = Delegate.CreateDelegate(type, frm, method);
                                Infragistics.Win.UltraWinGrid.CellEventArgs edit_e = new CellEventArgs(TmpGrid.ActiveCell);

                                // Raise the event. Then Invoke Event.
                                frm.Invoke(handler, new object[] { TmpCombo, edit_e });
                            }
                        }
                    }
                    #endregion

                    #region (e.KeyCode == Keys.Escape)

                    else if ((e.KeyCode == Keys.Escape) && !(e.Shift || e.Control || e.Alt))
                    {
                        //This feature can only be implemented on form that has no data entry
                        //if (frm.Modal == true)
                        //    frm.DialogResult = DialogResult.Cancel;

                        if (TmpText != null) { TmpText.Text = TmpText.OldText; }
                        if (TmpCombo != null) { TmpCombo.Value = TmpCombo.OldValue; }

                        if (TmpCombo != null)
                        {
                            TmpCombo.Value = TmpCombo.OldValue;
                            if (TmpCombo.OldValue == null)
                                TmpCombo.Text = string.Empty;
                        }

                        if (TmpNumber != null) { TmpNumber.Text = TmpNumber.OldText; }
                        if (TmpDate != null) { TmpDate.DateValue = TmpDate.OldValue; }
                        //if (TmpGrid != null)
                        //{
                        // if (TmpGrid.ActiveCell != null)
                        // {
                        // TmpGrid.PerformAction(UltraGridAction.ToggleEditMode);
                        // }
                        //}
                    }
                    #endregion

                    #region (e.KeyCode == Keys.Tab)

                    else if ((e.KeyCode == Keys.Tab) && !(e.Shift || e.Control || e.Alt))
                    {
                        #region selecting text in next control
                        switch (SwtC)
                        {
                            case 1://TANumericEditor
                                frm.SelectNextControl(TmpNumber, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 2://TATextBoxEditor                                
                                frm.SelectNextControl(TmpText, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 4://TADateEditor
                                frm.SelectNextControl(TmpDate, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 6://TACheckBox
                                frm.SelectNextControl(TmpCheck, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 3://TAComboBox                               
                                frm.SelectNextControl(TmpCombo, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 7://UltraTabControl
                                frm.SelectNextControl(TmpTap, true, true, true, true);
                                SuppressKey = false;
                                break;
                            case 8://Radio
                                frm.SelectNextControl(TmpRadio, true, true, true, true);
                                SuppressKey = true;
                                break;
                            case 9:
                                frm.SelectNextControl(TmpTime, true, true, true, true);
                                SuppressKey = false;
                                break;
                            default:
                                break;
                        }

                        Control NextCtrl = GetActiveControlAtActiveForm();
                        //Number Editor
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            ((TAUtil.TANumericEditor)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TANumericEditor))
                        {
                            ((TAUtil.TANumericEditor)NextCtrl).SelectAll();
                        }
                        //Textbox
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            ((TAUtil.TATextBoxEditor)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TATextBoxEditor))
                        {
                            ((TAUtil.TATextBoxEditor)NextCtrl).SelectAll();
                        }

                        //Combobox
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            ((TAUtil.TAComboBox)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TAComboBox))
                        {
                            ((TAUtil.TAComboBox)NextCtrl).SelectAll();

                        }

                        //Date Editor
                        if (NextCtrl.Parent.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            ((TAUtil.TADateEditor)NextCtrl.Parent).SelectAll();
                        }
                        else if (NextCtrl.GetType() == typeof(TAUtil.TADateEditor))
                        {
                            ((TAUtil.TADateEditor)NextCtrl).SelectAll();
                        }


                        #endregion-+
                    }
                    #endregion

                    if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.F2 || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.F4 || e.KeyCode == Keys.F6)
                    {
                        if (SwtC > 0)
                        {
                            if (TmpGrid != null)
                            {
                                if (e.KeyCode == Keys.F4 || e.KeyCode == Keys.F6)
                                    e.SuppressKeyPress = SuppressKey;
                                else if (e.KeyCode == Keys.Escape)
                                    TmpGrid.PerformAction(UltraGridAction.Undo);
                            }
                            else
                            {
                                e.SuppressKeyPress = SuppressKey;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void PrintGrid(TAGridEditor grd)
        {
            foreach (UltraGridRow row in grd.Rows)
            {
                for (int i = 0; i < grd.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    if (GFunc.IsNE(row.Cells[i].Value))
                    {
                        Debug.Print(grd.DisplayLayout.Bands[0].Columns[i].Key.ToString() + "\t null");
                    }
                    else
                    {
                        Debug.Print(grd.DisplayLayout.Bands[0].Columns[i].Key.ToString() + "\t" + row.Cells[i].Value.ToString());
                    }


                }
            }
        }
        public static void PrintGrid(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Debug.Print(dt.Columns[i].ColumnName.ToString() + "\t" + row[i].ToString());
                }
                Debug.Print("\n");
            }
        }
        public static void PrintDocumentProfitAndLost(int DocCodeKey, int DocKey)
        {

            string _recordSource = string.Empty;
            ReportLoader ReportLoader = new ReportLoader();
            int RepKey = 2340;
            List<SqlParameter> parmList = new List<SqlParameter>();


            ReportLoader._reportFileName = "AR_Doc_GP.rpt";

            //Common Process before Print/Fax/Email/Preview
            parmList.Clear();
            parmList.Add(new SqlParameter("@DocKey", DocKey));
            parmList.Add(new SqlParameter("@DocCodeKey", DocCodeKey));//Request for Quotation RepKey                        
            ReportLoader.ReportSqlParameter = parmList;

            //Get Report Data With repKey
            SYSRep rep = SYSRep.Get(RepKey);
            _recordSource = rep.RPTRecordSource1;

            //Get report Datasource with store procedure name and parameter from Sys_repCriter               
            DataTable dtSource = GFunc.ExecuteProc(_recordSource, ReportLoader.ReportSqlParameter);

            //report parameters
            List<ReportParameter> l_Reval = new List<ReportParameter>();
            ReportLoader.ReportParameter = l_Reval;
            string opCmpValue = SysOptionUtility.GetStr("CompanyName");
            ReportParameter CmpName = new ReportParameter("pCmpName", opCmpValue);
            ReportLoader.ReportParameter.Add(CmpName);


            ReportLoader.rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            ReportLoader.rptDoc.Load(Application.StartupPath + @"\Reports\" + ReportLoader._reportFileName);
            ReportLoader.rptDoc.SetDataSource(dtSource);

            foreach (ReportParameter p in ReportLoader.ReportParameter)
            {
                ReportLoader.rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
            }

            //Perform Preview 
            ReportLoader.PrintPreview();
           
 


        }//Completed


        public static string FormSettingID_Get(UltraGrid grid)
        {
            try
            {
                Form f = (Form)grid.TopLevelControl;
                if (f == frmMain.gfrmMain)
                {
                    f = frmMain.gfrmMain.ActiveMdiChild;
                }
                FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                //declaring one should be 'declaredobjectName'
                object obj = myFieldInfo.First(o => o.Name.ToLower().Contains("contextmenusetting")).GetValue(f);
                string controlSettingList = obj.ToString();//eg;=>"|ApproveDate#NA#0##|ApproveUserKey#NA#0##SECUserSortID|";
                char[] charArray = controlSettingList.ToCharArray();
                Array.Reverse(charArray);
                string controlSettingListReverse = new string(charArray);
                int foundIndex = controlSettingList.IndexOf("|0GF#" + grid.Name);

                if (foundIndex < 0)
                {
                    MsgBox.Show("ERROR: Menu has not been set to control=" + grid.Name);
                    throw new TAException("ERROR : FormSettingID doesn't exist for the grid");
                }

                int endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('|'+bindingFieldNmorControlNm) ;Remark=> extra separator='|'
                foundIndex = controlSettingList.Length - foundIndex;
                int startIndex = controlSettingList.Length - controlSettingListReverse.IndexOf('|', foundIndex);
                string st = controlSettingList.Substring(startIndex, endIndex - startIndex);
                string listSettingID = st.Split('#')[5];

                if (listSettingID != string.Empty)
                    return listSettingID;
                else
                    throw new TAException("ERROR: FormSettingID doesn't exist for the grid");

            }
            catch (TAException taex)
            {
                throw SysAuditLogUtility.ModifyTAException(taex, true, new object[] { });
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }

        }
        public static string FormSettingID_Get(Control control, GEnum.FormSettingGetType settingType)
        {
            try
            {
                Form f = (Form)control.TopLevelControl;
                if (f == frmMain.gfrmMain || GFunc.IsNE(f))
                {
                    f = frmMain.gfrmMain.ActiveMdiChild;
                }
                FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                //declaring one should be 'declaredobjectName'
                object obj = myFieldInfo.First(o => o.Name.ToLower().Contains("contextmenusetting")).GetValue(f);
                string controlSettingList = obj.ToString();//eg;=>"|ApproveDate#NA#0##|ApproveUserKey#NA#0##SECUserSortID|";
                char[] charArray = controlSettingList.ToCharArray();
                Array.Reverse(charArray);
                string controlSettingListReverse = new string(charArray);

                string strToSearch = "";

                if (control.GetType() != typeof(TAUtil.TAGridEditor))
                    strToSearch = "|" + control.Name + "#NA";
                else
                    strToSearch = "#" + control.Name;

                int foundIndex = controlSettingList.IndexOf(strToSearch);
                if (foundIndex < 0)
                {
                    MsgBox.Show("ERROR: Menu has not yet set to control=" + control.Name);
                    throw new TAException("ERROR: FormSettingID doesn't exist for " + control.Name);
                }
                int endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('|'+bindingFieldNmorControlNm) ;Remark=> extra separator='|'

                string st = controlSettingList.Substring(foundIndex, endIndex - foundIndex);
                string listSettingID = st.Split('#')[(int)settingType];

                //Added the following block on 21-Dec-2010, to search again if a control contained in both grid columns and header
                if (listSettingID == string.Empty)
                {
                    foundIndex = controlSettingList.IndexOf("#" + control.Name, ++endIndex);
                    if (foundIndex >= 0)
                    {
                        endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('|'+bindingFieldNmorControlNm) ;Remark=> extra separator='|'

                        st = controlSettingList.Substring(foundIndex, endIndex - foundIndex);
                        listSettingID = st.Split('#')[(int)settingType];

                        if (listSettingID != string.Empty)
                            return listSettingID;
                        else
                            throw new TAException("ERROR: FormSettingID doesn't exist for" + control.Name);
                    }
                }
                else if (listSettingID == string.Empty)
                    throw new TAException("ERROR: FormSettingID doesn't exist for" + control.Name);

                return listSettingID;
            }
            catch (TAException taex)
            {
                throw SysAuditLogUtility.ModifyTAException(taex, true, new object[] { });
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }

        public static void GridControl_SetNew(TAGridEditor grd, DataView dvGrdColList, DataSet dsAllSetting, int codeKey)
        {
            int option = 0;

            try
            {
                
                foreach (DataRowView dr in dvGrdColList)
                {
                    string colNm = GFunc.NEStr(dr["ColFldNm"],"");
                    if (grd.DisplayLayout.Bands[0].Columns.Exists(colNm))
                    {
                        UltraGridColumn c = grd.DisplayLayout.Bands[0].Columns[colNm];
                        if (c.Hidden)
                            continue;
                        if (dr["ColControlType"].ToString() != "")
                        {
                            switch (dr["ColControlType"].ToString().ToLower())
                            {
                                case "tautil.tadateeditor":

                                    TAUtil.TADateEditor TADateControl = new TADateEditor(true);
                                    TADateControl.DataFilter = new DateFormattingDataFilter(GFunc.NEStr(dr["ColDataFormat"], string.Empty));
                                    TADateControl.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    TADateControl.ButtonsRight[0].Appearance.Image = WinUI.Properties.Resources.calendar3;
                                    TADateControl.TextRenderingMode = TextRenderingMode.GDI;
                                    TADateControl.Visible = false;
                                    c.EditorComponent = TADateControl;
                                    ((TADateEditor)c.EditorComponent).calendarContainer = grd;

                                    break;

                                case "tautil.tacombobox":
                                    TAComboBox tacbox = new TAComboBox();
                                    EditorButton editorButton = new EditorButton();
                                    tacbox.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                                    tacbox.ButtonsRight.Add(editorButton);
                                    tacbox.ButtonsRight[0].Visible = false;
                                    tacbox.LimitToList = GFunc.NEBool(dr["ControlLimitToList"], false);

                                    BindComboValueNew(tacbox, dr, dsAllSetting,codeKey);

                                    tacbox.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    tacbox.TextRenderingMode = TextRenderingMode.GDI;
                                    c.EditorComponent = tacbox;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    c.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                                    break;

                                case "dropdownwithbutton":
                                    c.EditorComponent = new TAComboBox();
                                    ((TAComboBox)c.EditorComponent).LimitToList = false;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.Browse;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;
                                case "tautil.tatextboxeditor":
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.open3;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;

                                case "tautil.tacheckboxeditor":
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                                    break;
                                case "tautil.tanumericeditor":
                                    TAUtil.TANumericEditor TANumericControl = new TANumericEditor();
                                    TANumericControl.DataFilter = new NumericFormattingDataFilter(GFunc.NEStr(dr["ColDataFormat"], string.Empty).Replace("?", "")); //We need to replace the "?" with "" in the formsetting format

                                    TAUtil.NumericType pNumericType = TAUtil.NumericType.Decimal;
                                    if (c.DataType == typeof(short) || c.DataType == typeof(short?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer16Bit;
                                    }
                                    else if (c.DataType == typeof(int) || c.DataType == typeof(int?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer32Bit;
                                    }
                                    else if (c.DataType == typeof(long) || c.DataType == typeof(long?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer64Bit;
                                    }
                                    else if (c.DataType == typeof(System.Decimal) || c.DataType == typeof(System.Decimal?))
                                    {

                                        pNumericType = TAUtil.NumericType.Decimal;
                                    }
                                    else if (c.DataType == typeof(System.Double) || c.DataType == typeof(System.Double?))
                                    {

                                        pNumericType = TAUtil.NumericType.Double;
                                    }

                                    TANumericControl.NumberType = pNumericType;
                                    TANumericControl.Format = GFunc.NEStr(dr["ColDataFormat"], string.Empty);
                                    c.EditorComponent = TANumericControl;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    break;
                            }
                        }

                        if (dr["ColAlignment"].ToString() != "")
                        {
                            switch (dr["ColAlignment"].ToString().ToLower())
                            {
                                case "l":
                                    c.CellAppearance.TextHAlign = HAlign.Left;
                                    break;
                                case "r":
                                    c.CellAppearance.TextHAlign = HAlign.Right;
                                    break;
                                case "c":
                                    c.CellAppearance.TextHAlign = HAlign.Center;
                                    break;
                            }
                            c.Header.Appearance = c.CellAppearance;
                        }
                        if (c.EditorComponent == null || GFunc.CompareString(dr["ColControlType"].ToString().ToLower(), "tautil.tatextboxeditor"))
                        {
                            if (GFunc.NEBool(dr["ColMultiline"], false) == true)
                                c.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                            else
                                c.CellMultiLine = Infragistics.Win.DefaultableBoolean.False;
                        }

                    }
                }//End ForEach           
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }

        //New Function to addVCE File Reset Pswd
        public static void GridControl_SetNew(TAGridEditor grd,DataTable dtGrdColList, DataSet dsAllSetting, int codeKey)
        {
            int option = 0;

            try
            {               
                foreach (UltraGridColumn c in grd.DisplayLayout.Bands[0].Columns)
                {

                    DataRow[] drs = dtGrdColList.Select("ColFldNm='" + c.Key + "'");

                    if (drs.Length > 0)
                    {
                        DataRow dr = drs[0];
                        if (dr["ColControlType"].ToString() != "")
                        {
                            switch (dr["ColControlType"].ToString().ToLower())
                            {
                                case "tautil.tadateeditor":

                                    TAUtil.TADateEditor TADateControl = new TADateEditor(true);
                                    TADateControl.DataFilter = new DateFormattingDataFilter(GFunc.NEStr(dr["ColDataFormat"], string.Empty));
                                    TADateControl.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    TADateControl.ButtonsRight[0].Appearance.Image = WinUI.Properties.Resources.calendar3;
                                    TADateControl.TextRenderingMode = TextRenderingMode.GDI;
                                    TADateControl.Visible = false;
                                    c.EditorComponent = TADateControl;
                                    ((TADateEditor)c.EditorComponent).calendarContainer = grd;

                                    break;

                                case "tautil.tacombobox":
                                    TAComboBox tacbox = new TAComboBox();
                                    EditorButton editorButton = new EditorButton();
                                    tacbox.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                                    tacbox.ButtonsRight.Add(editorButton);
                                    tacbox.ButtonsRight[0].Visible = false;
                                    tacbox.LimitToList = GFunc.NEBool(dr["ControlLimitToList"], false);

                                    BindComboValueNew(tacbox, dr, dsAllSetting,codeKey);

                                    tacbox.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    tacbox.TextRenderingMode = TextRenderingMode.GDI;
                                    c.EditorComponent = tacbox;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    c.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                                    break;

                                case "dropdownwithbutton":
                                    c.EditorComponent = new TAComboBox();
                                    ((TAComboBox)c.EditorComponent).LimitToList = false;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.Browse;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;
                                case "tautil.tatextboxeditor":
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.open3;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;

                                case "tautil.tacheckboxeditor":
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                                    break;
                                case "tautil.tanumericeditor":
                                    TAUtil.TANumericEditor TANumericControl = new TANumericEditor();
                                    TANumericControl.DataFilter = new NumericFormattingDataFilter(GFunc.NEStr(dr["ColDataFormat"], string.Empty).Replace("?", "")); //We need to replace the "?" with "" in the formsetting format

                                    TAUtil.NumericType pNumericType = TAUtil.NumericType.Decimal;
                                    if (c.DataType == typeof(short) || c.DataType == typeof(short?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer16Bit;
                                    }
                                    else if (c.DataType == typeof(int) || c.DataType == typeof(int?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer32Bit;
                                    }
                                    else if (c.DataType == typeof(long) || c.DataType == typeof(long?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer64Bit;
                                    }
                                    else if (c.DataType == typeof(System.Decimal) || c.DataType == typeof(System.Decimal?))
                                    {

                                        pNumericType = TAUtil.NumericType.Decimal;
                                    }
                                    else if (c.DataType == typeof(System.Double) || c.DataType == typeof(System.Double?))
                                    {

                                        pNumericType = TAUtil.NumericType.Double;
                                    }

                                    TANumericControl.NumberType = pNumericType;
                                    TANumericControl.Format = GFunc.NEStr(dr["ColDataFormat"], string.Empty);
                                    c.EditorComponent = TANumericControl;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    break;
                            }
                        }

                        if (dr["ColAlignment"].ToString() != "")
                        {
                            switch (dr["ColAlignment"].ToString().ToLower())
                            {
                                case "l":
                                    c.CellAppearance.TextHAlign = HAlign.Left;
                                    break;
                                case "r":
                                    c.CellAppearance.TextHAlign = HAlign.Right;
                                    break;
                                case "c":
                                    c.CellAppearance.TextHAlign = HAlign.Center;
                                    break;
                            }
                            c.Header.Appearance = c.CellAppearance;
                        }
                        if (c.EditorComponent == null || GFunc.CompareString(dr["ColControlType"].ToString().ToLower(), "tautil.tatextboxeditor"))
                        {
                            if (GFunc.NEBool(dr["ColMultiline"], false) == true)
                                c.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                            else
                                c.CellMultiLine = Infragistics.Win.DefaultableBoolean.False;
                        }

                    }
                }//End ForEach           
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
        }
        //New Function to addVCE File Reset Pswd
        public static void GridControl_Set(TAGridEditor grd, string ListID, int codeKey,Form f=null)
        {
            //ReportDirectory and SysOption can't used.


            int option = 0;

            try
            {

                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Option", option));
                list.Add(new SqlParameter("@ListID", ListID));
                DataTable dtGrdColList = GFunc.ExecuteProc("SYSFormSettingIDDet_GridControl_Get", list);

                foreach (UltraGridColumn c in grd.DisplayLayout.Bands[0].Columns)
                {
                    if(c.Key =="ItmJobKey")
                    {

                    }
                    DataRow[] drs = dtGrdColList.Select("ColFldNm='" + c.Key + "'");

                    if (drs.Length > 0)
                    {
                        DataRow dr = drs[0];
                        if (dr["ColControlType"].ToString() != "")
                        {
                            switch (dr["ColControlType"].ToString().ToLower())
                            {
                                case "tautil.tadateeditor":

                                    TAUtil.TADateEditor TADateControl = new TADateEditor(true);
                                    TADateControl.DataFilter = new DateFormattingDataFilter(GFunc.NEStr(dr["ColDataFormat"], string.Empty));
                                    TADateControl.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    TADateControl.ButtonsRight[0].Appearance.Image = WinUI.Properties.Resources.calendar3;
                                    TADateControl.TextRenderingMode = TextRenderingMode.GDI;
                                    TADateControl.Visible = false;
                                    c.EditorComponent = TADateControl;
                                    ((TADateEditor)c.EditorComponent).calendarContainer = grd;

                                    break;

                                case "tautil.tacombobox":
                                    TAComboBox tacbox = new TAComboBox();
                                    EditorButton editorButton = new EditorButton();
                                    tacbox.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                                    tacbox.ButtonsRight.Add(editorButton);
                                    tacbox.ButtonsRight[0].Visible = false;
                                    tacbox.LimitToList = GFunc.NEBool(dr["ColControlLimitToList"], false);
                                    //commented by Jane. We can not format the ComboBox in GlobalUI. Because Some ComboBox are in different style. Eg. Vendor Combo from APPD
                                    tacbox.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    tacbox.TextRenderingMode = TextRenderingMode.GDI;
                                    c.EditorComponent = tacbox;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    c.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                                    break;
                             
                                case "dropdownwithbutton":
                                    c.EditorComponent = new TAComboBox();
                                    ((TAComboBox)c.EditorComponent).LimitToList = false;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.Browse;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;
                                case "tautil.tatextboxeditor":
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.open3;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;

                                case "tautil.tacheckboxeditor":
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                                    break;
                                case "tautil.tanumericeditor":
                                    TAUtil.TANumericEditor TANumericControl = new TANumericEditor();
                                    TANumericControl.DataFilter = new NumericFormattingDataFilter(GFunc.NEStr(dr["ColDataFormat"], string.Empty).Replace("?", "")); //We need to replace the "?" with "" in the formsetting format

                                    TAUtil.NumericType pNumericType = TAUtil.NumericType.Decimal;
                                    if (c.DataType == typeof(short) || c.DataType == typeof(short?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer16Bit;
                                    }
                                    else if (c.DataType == typeof(int) || c.DataType == typeof(int?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer32Bit;
                                    }
                                    else if (c.DataType == typeof(long) || c.DataType == typeof(long?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer64Bit;
                                    }
                                    else if (c.DataType == typeof(System.Decimal) || c.DataType == typeof(System.Decimal?))
                                    {

                                        pNumericType = TAUtil.NumericType.Decimal;
                                    }
                                    else if (c.DataType == typeof(System.Double) || c.DataType == typeof(System.Double?))
                                    {

                                        pNumericType = TAUtil.NumericType.Double;
                                    }

                                    TANumericControl.NumberType = pNumericType;
                                    TANumericControl.Format = GFunc.NEStr(dr["ColDataFormat"], string.Empty);
                                    c.EditorComponent = TANumericControl;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    break;
                            }
                        }

                        if (dr["ColAlignment"].ToString() != "")
                        {
                            switch (dr["ColAlignment"].ToString().ToLower())
                            {
                                case "l":
                                    c.CellAppearance.TextHAlign = HAlign.Left;
                                    break;
                                case "r":
                                    c.CellAppearance.TextHAlign = HAlign.Right;
                                    break;
                                case "c":
                                    c.CellAppearance.TextHAlign = HAlign.Center;
                                    break;
                            }
                            c.Header.Appearance = c.CellAppearance;
                        }
                        if (c.EditorComponent == null || GFunc.CompareString(dr["ColControlType"].ToString().ToLower(), "tautil.tatextboxeditor"))
                        {
                            if (GFunc.NEBool(dr["ColMultiline"], false) == true)
                                c.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
                            else
                                c.CellMultiLine = Infragistics.Win.DefaultableBoolean.False;
                        }

                    }
                }//End ForEach           

                foreach (DataRow dr in dtGrdColList.Rows)
                {
                    Control grdCtrl = null;

                    if (GFunc.CompareString(dr["ColFldNm"].ToString(), "NA") || GFunc.CompareString(dr["ColFldNm"].ToString(), "GF")) continue;

                    if (grd.DisplayLayout.Bands[0].Columns.Exists(dr["ColFldNm"].ToString()))
                        grdCtrl = (Control)grd.DisplayLayout.Bands[0].Columns[dr["ColFldNm"].ToString()].EditorComponent;

                    if (grdCtrl == null) continue;

                    if (grdCtrl.GetType() == typeof(TAComboBox))
                    {

                        TAUtil.TAComboBox cbo = (TAUtil.TAComboBox)grdCtrl;
                        if (cbo != null)
                        {

                            string listSettingID = dr["ColListID"].ToString();

                            string displayMember = dr["ColControlMemberDisplay"].ToString();
                            string valueMember = dr["ColControlMemberValue"].ToString();

                            if (listSettingID == string.Empty) continue;

                            if (GFunc.CompareString(ListID, "frmMSTBudgetGridCurrent"))
                            {
                                if (GFunc.CompareString(listSettingID, "SYSCounterGrpByDC"))
                                {
                                    listSettingID = listSettingID + "%" + (int)GEnum.SystemCode.Budget;
                                }
                            }
                            else if (GFunc.CompareString("KPITgtMth", listSettingID))
                            {
                                if (f != null)
                                {
                                    if (f.Name == "frmRepKPICmpTgt")
                                    {
                                        FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                        object obj = myFieldInfo.First(o => o.Name.ToLower().Contains("tacboyear")).GetValue(f);
                                        if (obj.GetType() == typeof(TAUtil.TAComboBox))
                                            listSettingID += "%" + GFunc.NEInt(((TAUtil.TAComboBox)obj).Value, DateTime.Today.Year);                                        
                                    }
                                    else
                                        listSettingID += "%" + DateTime.Today.Year;
                                }
                            }
                            //if (listSettingID == "MSTJobSales_id")
                            //{
                            //    TAComboBox DocConKey = ((TAComboBox)grd.FindForm().Controls.Find("DocConKey", true)[0]);
                            //    listSettingID = listSettingID + "%" + GFunc.NEInt(DocConKey.Value,0);
                            //}
                            GlobalUI.BindComboValue((TAUtil.TAComboBox)cbo, listSettingID, displayMember, valueMember, codeKey);

                        }
                    }
                }//End Loop
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }

        }
        public static void GridControl_Set(SqlConnection cn, TAGridEditor grd, string ListID, int codeKey)
        {
            //ReportDirectory and SysOption can't used.


            int option = 0;

            try
            {

                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Option", option));
                list.Add(new SqlParameter("@ListID", ListID));
                DataTable dtGrdColList = GFunc.ExecuteProc(cn, "SYSFormSettingIDDet_GridControl_Get", list);



                foreach (UltraGridColumn c in grd.DisplayLayout.Bands[0].Columns)
                {

                    DataRow[] drs = dtGrdColList.Select("ColFldNm='" + c.Key + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["ColControlType"].ToString() != "")
                        {
                            switch (drs[0]["ColControlType"].ToString().ToLower())
                            {
                                case "tautil.tadateeditor":
                                    TAUtil.TADateEditor TADateControl = new TADateEditor(true);
                                    TADateControl.DataFilter = new DateFormattingDataFilter(GFunc.NEStr(drs[0]["ColDataFormat"], string.Empty));
                                    TADateControl.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    TADateControl.ButtonsRight[0].Appearance.Image = WinUI.Properties.Resources.calendar3;
                                    TADateControl.TextRenderingMode = TextRenderingMode.GDI;
                                    TADateControl.Visible = false;
                                    c.EditorComponent = TADateControl;
                                    ((TADateEditor)c.EditorComponent).calendarContainer = grd;
                                    break;

                                case "tautil.tacombobox":
                                    TAComboBox tacbox = new TAComboBox();
                                    EditorButton editorButton = new EditorButton();
                                    tacbox.ButtonsRight.Add(editorButton);
                                    tacbox.ButtonsRight[0].Visible = false;
                                    tacbox.LimitToList = false;
                                    tacbox.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    tacbox.TextRenderingMode = TextRenderingMode.GDI;
                                    c.EditorComponent = tacbox;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    break;

                                case "dropdownwithbutton":
                                    c.EditorComponent = new TAComboBox();
                                    ((TAComboBox)c.EditorComponent).LimitToList = false;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
                                    ((Control)c.EditorComponent).Visible = false;
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.Browse;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;

                                case "tautil.tatextboxeditor":
                                    c.CellButtonAppearance.Image = WinUI.Properties.Resources.open3;
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                    break;

                                case "tautil.tacheckboxeditor":
                                    c.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                                    break;
                                case "tautil.tanumericeditor":
                                    c.EditorComponent = new TANumericEditor();
                                    TAUtil.NumericType pNumericType = TAUtil.NumericType.Decimal;
                                    if (c.DataType == typeof(short) || c.DataType == typeof(short?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer16Bit;
                                    }
                                    else if (c.DataType == typeof(int) || c.DataType == typeof(int?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer32Bit;
                                    }
                                    else if (c.DataType == typeof(long) || c.DataType == typeof(long?))
                                    {
                                        pNumericType = TAUtil.NumericType.Integer64Bit;
                                    }
                                    else if (c.DataType == typeof(System.Decimal) || c.DataType == typeof(System.Decimal?))
                                    {

                                        pNumericType = TAUtil.NumericType.Decimal;
                                    }
                                    else if (c.DataType == typeof(System.Double) || c.DataType == typeof(System.Double?))
                                    {

                                        pNumericType = TAUtil.NumericType.Double;
                                    }
                                    ((TANumericEditor)c.EditorComponent).NumberType = pNumericType;
                                    c.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
                                    ((Control)c.EditorComponent).Visible = true;
                                    break;
                            }
                        }

                        if (drs[0]["ColAlignment"].ToString() != "")
                        {
                            switch (drs[0]["ColAlignment"].ToString().ToLower())
                            {
                                case "l":
                                    c.CellAppearance.TextHAlign = HAlign.Left;
                                    break;
                                case "r":
                                    c.CellAppearance.TextHAlign = HAlign.Right;
                                    break;
                                case "c":
                                    c.CellAppearance.TextHAlign = HAlign.Center;
                                    break;
                            }
                            c.Header.Appearance = c.CellAppearance;
                        }
                    }
                }//End ForEach           

                foreach (DataRow dr in dtGrdColList.Rows)
                {

                    Control grdCtrl = null;

                    if (GFunc.CompareString(dr["ColFldNm"].ToString(), "NA") || GFunc.CompareString(dr["ColFldNm"].ToString(), "GF")) continue;

                    if (grd.DisplayLayout.Bands[0].Columns.Exists(dr["ColFldNm"].ToString()))
                        grdCtrl = (Control)grd.DisplayLayout.Bands[0].Columns[dr["ColFldNm"].ToString()].EditorComponent;

                    if (grdCtrl == null) continue;

                    if (grdCtrl.GetType() == typeof(TAComboBox))
                    {

                        TAUtil.TAComboBox cbo = (TAUtil.TAComboBox)grdCtrl;
                        if (cbo != null)
                        {
                            string listSettingID = dr["ColListID"].ToString();
                            string displayMember = dr["ColControlMemberDisplay"].ToString();
                            string valueMember = dr["ColControlMemberValue"].ToString();

                            if (listSettingID == string.Empty) continue;
                            GlobalUI.BindComboValue(cn, (TAUtil.TAComboBox)cbo, listSettingID, displayMember, valueMember);

                        }
                    }
                }//End Loop
            }
            catch (Exception ex)
            {
                throw SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }

        }
        public static void Combos_Fill(Control ctrls, int codeKey)
        {
            int option = 0;
            //Option 0 for CodeKey Criteria, 1 for FormNm Criteria
            try
            {
                string frmName = ctrls.Name;
                //if (codeKey == 0)
                //    option = 2; commented by jane. option 2 is filetered by only formName. Some form cann't be filtered only with formName. coz in sys_listsetting table, some form may have more than one codekey (eg.frmprintselection)
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@Option", option));
                list.Add(new SqlParameter("@CodeKey", codeKey));
                list.Add(new SqlParameter("@FormName", frmName));
                //DataTable dtUIList = GFunc.ExecuteProc("ROSYSListSettingUI_Get", list);
                DataTable dtUIList = GFunc.ExecuteProc("SYSFormSetting_Get", list);
                CombosChild_Fill(ctrls, codeKey, dtUIList);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }
        public static void CombosChild_Fill(Control ctrls, int codeKey, DataTable dtUIList)
        {

            try
            {
                foreach (Control ctrl in ctrls.Controls)
                {

                    if (ctrl.HasChildren && ctrl.GetType() != typeof(TAUtil.TAComboBox))
                    {
                        CombosChild_Fill(ctrl, codeKey, dtUIList);
                    }
                    else
                    {
                        if (codeKey > 0)
                        {
                            #region Child Controls

                            string listSettingID = null;
                            string bindingFieldNm = null;
                            string DisplayMember = string.Empty;
                            string ValueMember = string.Empty;

                            #region Combo
                            if (ctrl.GetType() == typeof(TAComboBox))
                            {
                                if (GFunc.CompareString(ctrl.Name, "VDefaultAPPYDocType"))
                                {//Test
                                }
                                if (ctrl.DataBindings.Count > 0)
                                {
                                    //Get the Binding Field Name of Control
                                    bindingFieldNm = ctrl.DataBindings[0].BindingMemberInfo.BindingField;

                                    //Find Control's binding field name in DataTable
                                    DataRow[] drList = dtUIList.Select("BindingFieldNm='" + bindingFieldNm + "'");

                                    if (drList.Length > 0)
                                    {
                                        listSettingID = drList[0]["ListSettingID"].ToString();
                                        if (GFunc.CompareString(bindingFieldNm, "DocTypeNm") || GFunc.CompareString(bindingFieldNm, "DocType"))
                                        {
                                            listSettingID += "%" + codeKey;
                                        }

                                        if (listSettingID == GVar.ListSettingID.REFAddrByCon) continue;

                                        ValueMember = GFunc.NEStr(drList[0]["ControlMemberValue"], "");
                                        DisplayMember = GFunc.NEStr(drList[0]["ControlMemberDisplay"], "");

                                        if (listSettingID == "" && drList.Length > 1)
                                        {
                                            listSettingID = drList[1]["ListSettingID"].ToString();
                                            ValueMember = GFunc.NEStr(drList[1]["ControlMemberValue"], "");
                                            DisplayMember = GFunc.NEStr(drList[1]["ControlMemberDisplay"], "");
                                        }

                                        GlobalUI.BindComboValue((TAUtil.TAComboBox)ctrl, listSettingID, DisplayMember, ValueMember, codeKey);

                                        ((TAUtil.TAComboBox)ctrl).LimitToList = GFunc.NEBool(drList[0]["ControlLimitToList"], false);

                                        ((TAUtil.TAComboBox)ctrl).AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;

                                        ((TAUtil.TAComboBox)ctrl).TextRenderingMode = TextRenderingMode.GDI;
                                        ((TAUtil.TAComboBox)ctrl).Font = new Font("Calibri", 11);
                                        ((TAUtil.TAComboBox)ctrl).Appearance.BackColor = Color.White;
                                    }

                                }
                                else
                                {
                                    DataRow[] drList = dtUIList.Select("ControlNm='" + ctrl.Name + "'");

                                    if (drList.Length > 0)
                                    {
                                        listSettingID = drList[0]["ListSettingID"].ToString();

                                        if (GFunc.CompareString(GVar.ListSettingID.MSTJobSalesByConKey, listSettingID))
                                        {
                                            listSettingID += "%%" + AppInfor.JobAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.MSTItmFS_id, listSettingID))
                                        {
                                            listSettingID += "%%" + AppInfor.ItemAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_id, listSettingID) ||
                                            //ttm
                                        GFunc.CompareString(GVar.ListSettingID.MSTConALLPurchase_id, listSettingID) ||
                                        //ttm
                                        GFunc.CompareString(GVar.ListSettingID.MSTConPurchasePY_id, listSettingID) ||
                                        GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_des, listSettingID) ||
                                        GFunc.CompareString(GVar.ListSettingID.MSTConPurchasePY_des, listSettingID))
                                        {
                                            listSettingID += "%%" + AppInfor.ConAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.MSTAccLiability_id, listSettingID) ||
                                        GFunc.CompareString(GVar.ListSettingID.MSTAccLiability_des, listSettingID))
                                        {
                                            listSettingID += "%";
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.SYSRepRptDefaultRepKey, listSettingID))
                                        {
                                            if (codeKey == (int)GEnum.SystemCode.ARAP_Revaluation)
                                                listSettingID += "%1155";
                                            else if (codeKey == (int)GEnum.SystemCode.Financial_Charge)
                                                listSettingID += "%1160";
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.SYSDocTypeNmByDC, listSettingID))
                                        {
                                            if (codeKey == (int)GEnum.SystemCode.DO_to_IV_Transfer)
                                                listSettingID += "%" + (int)GEnum.SystemCode.Delivery_Order;
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.SYSRepRpt, listSettingID))//Used in frmPrintSelection                                               
                                        {
                                            listSettingID += "%" + codeKey;
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.REFContactInforConEmail, listSettingID))
                                        {
                                            Form f = (Form)ctrl.TopLevelControl;
                                            if (f == frmMain.gfrmMain || GFunc.IsNE(f))
                                                f = frmMain.gfrmMain.ActiveMdiChild;

                                            FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                            object obj = myFieldInfo.First(o => o.Name.ToLower().Contains("_conkey")).GetValue(f);
                                            listSettingID += "%" + obj.ToString();

                                        }
                                        else if (GFunc.CompareString("KPITgtMth", listSettingID))
                                        {
                                            Form f = (Form)ctrl.TopLevelControl;
                                            if (f == frmMain.gfrmMain || GFunc.IsNE(f))
                                                f = frmMain.gfrmMain.ActiveMdiChild;

                                            FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                            object obj = myFieldInfo.First(o => o.Name.ToLower().Contains("tacboyear")).GetValue(f);
                                            listSettingID += "%" +GFunc.NEInt(obj,0);

                                        }
                                        //ATL
                                        if (GFunc.CompareString(ctrl.Name, "DocTypeNm") || GFunc.CompareString(ctrl.Name, "pDocType") || GFunc.CompareString(ctrl.Name, "DocType") || GFunc.CompareString(ctrl.Name, "DocCode"))
                                        {
                                            listSettingID += "%" + codeKey;
                                        }

                                        if (listSettingID == GVar.ListSettingID.REFAddrByCon) continue;

                                        ValueMember = GFunc.NEStr(drList[0]["ControlMemberValue"], "");
                                        DisplayMember = GFunc.NEStr(drList[0]["ControlMemberDisplay"], "");
                                        GlobalUI.BindComboValue((TAUtil.TAComboBox)ctrl, listSettingID, DisplayMember, ValueMember, codeKey);
                                        ((TAUtil.TAComboBox)ctrl).LimitToList = GFunc.NEBool(drList[0]["ControlLimitToList"], false);
                                        ((TAUtil.TAComboBox)ctrl).AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;

                                        ((TAUtil.TAComboBox)ctrl).TextRenderingMode = TextRenderingMode.GDI;
                                        ((TAUtil.TAComboBox)ctrl).Font = new Font("Calibri", 11);
                                        ((TAUtil.TAComboBox)ctrl).Appearance.BackColor = Color.White;
                                    }
                                }
                            }//End IF
                            #endregion Combo
                            #region Grid Combo

                            #endregion Grid Combo
                            #endregion

                        }//End If CodeKey Check
                        else
                        {
                            #region Child Controls

                            string listSettingID = null;
                            string bindingFieldNm = null;
                            string DisplayMember = string.Empty;
                            string ValueMember = string.Empty;

                            #region Combo
                            if (ctrl.GetType() == typeof(TAComboBox))
                            {
                                if (GFunc.CompareString(ctrl.Name, "AddrShipViaKey"))
                                {//Test
                                }
                                if (ctrl.DataBindings.Count > 0)
                                {
                                    //Get the Binding Field Name of Control
                                    bindingFieldNm = ctrl.DataBindings[0].BindingMemberInfo.BindingField;


                                    //Find Control's binding field name in DataTable
                                    DataRow[] drList = dtUIList.Select("BindingFieldNm='" + bindingFieldNm + "'");

                                    if (drList.Length > 0)
                                    {
                                        listSettingID = drList[0]["ListSettingID"].ToString();

                                        if (GFunc.CompareString(GVar.ListSettingID.MSTJobSalesByConKey, listSettingID))
                                        {
                                            listSettingID += "%0%" + AppInfor.JobAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.MSTItmFS_id, listSettingID))
                                        {
                                            listSettingID += "%0%" + AppInfor.ItemAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_id, listSettingID) ||
                                           GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_des, listSettingID) ||
                                           GFunc.CompareString(GVar.ListSettingID.MSTConPurchasePY_id, listSettingID) ||
                                           GFunc.CompareString(GVar.ListSettingID.MSTConPurchasePY_des, listSettingID))
                                        {
                                            listSettingID += "%0%" + AppInfor.ConAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.SYSRepRptDefaultRepKey, listSettingID))
                                        {
                                            if (codeKey == (int)GEnum.SystemCode.ARAP_Revaluation)
                                                listSettingID += "%1155";
                                            else if (codeKey == (int)GEnum.SystemCode.Financial_Charge)
                                                listSettingID += "%1160";
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.SYSDocTypeNmByDC, listSettingID))
                                        {
                                            if (codeKey == (int)GEnum.SystemCode.DO_to_IV_Transfer)
                                                listSettingID += "%" + (int)GEnum.SystemCode.Delivery_Order;
                                        }

                                        if (GFunc.CompareString(bindingFieldNm, "DocTypeNm") || GFunc.CompareString(bindingFieldNm, "DocType"))
                                        {
                                            listSettingID += "%" + codeKey;
                                        }

                                        ValueMember = GFunc.NEStr(drList[0]["ControlMemberValue"], "");
                                        DisplayMember = GFunc.NEStr(drList[0]["ControlMemberDisplay"], "");
                                        GlobalUI.BindComboValue((TAUtil.TAComboBox)ctrl, listSettingID, DisplayMember, ValueMember, codeKey);
                                        ((TAUtil.TAComboBox)ctrl).LimitToList = GFunc.NEBool(drList[0]["ControlLimitToList"], false);

                                        ((TAUtil.TAComboBox)ctrl).TextRenderingMode = TextRenderingMode.GDI;
                                        ((TAUtil.TAComboBox)ctrl).Font = new Font("Calibri", 11);
                                        ((TAUtil.TAComboBox)ctrl).Appearance.BackColor = Color.White;
                                    }

                                }
                                else
                                {
                                    DataRow[] drList = dtUIList.Select("ControlNm='" + ctrl.Name + "'");

                                    if (drList.Length > 0)
                                    {
                                        listSettingID = drList[0]["ListSettingID"].ToString();

                                        if (GFunc.CompareString(GVar.ListSettingID.MSTJobSalesByConKey, listSettingID))
                                        {
                                            listSettingID += "%0%" + AppInfor.JobAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.MSTItmFS_id, listSettingID))
                                        {
                                            listSettingID += "%0%" + AppInfor.ItemAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_id, listSettingID) ||
                                            GFunc.CompareString(GVar.ListSettingID.MSTConPurchase_des, listSettingID) ||
                                            GFunc.CompareString(GVar.ListSettingID.MSTConPurchasePY_id, listSettingID) ||
                                            GFunc.CompareString(GVar.ListSettingID.MSTConPurchasePY_des, listSettingID))
                                        {
                                            listSettingID += "%0%" + AppInfor.ConAccessLevel.ToString() + "%" + AppInfor.CurrentUserKey.ToString();
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.SYSRepRptDefaultRepKey, listSettingID))
                                        {
                                            if (codeKey == (int)GEnum.SystemCode.ARAP_Revaluation)
                                                listSettingID += "%1155";
                                            else if (codeKey == (int)GEnum.SystemCode.Financial_Charge)
                                                listSettingID += "%1160";
                                        }
                                        else if (GFunc.CompareString(GVar.ListSettingID.SYSDocTypeNmByDC, listSettingID))
                                        {
                                            if (codeKey == (int)GEnum.SystemCode.DO_to_IV_Transfer)
                                                listSettingID += "%" + (int)GEnum.SystemCode.Delivery_Order;
                                        }
                                        else if (GFunc.CompareString("KPITgtMth", listSettingID))
                                        {
                                            Form f = GetActiveControlAtActiveForm().FindForm();                                            

                                            if (f != null)
                                            {
                                                if (f.Name == "frmRepKPICmpTgt")
                                                {
                                                    FieldInfo[] myFieldInfo = f.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                                    object obj = myFieldInfo.First(o => o.Name.ToLower().Contains("tacboyear")).GetValue(f);
                                                    if (obj.GetType() == typeof(TAUtil.TAComboBox))
                                                        listSettingID += "%" + GFunc.NEInt(((TAUtil.TAComboBox)obj).Value, DateTime.Today.Year);
                                                }
                                                else
                                                    listSettingID += "%" + DateTime.Today.Year;
                                            }
                                        }

                                        //ATL
                                        if (GFunc.CompareString(ctrl.Name, "DocTypeNm") || GFunc.CompareString(ctrl.Name, "pDocType") || GFunc.CompareString(ctrl.Name, "DocType"))
                                        {
                                            listSettingID += "%" + codeKey;
                                        }

                                        ValueMember = GFunc.NEStr(drList[0]["ControlMemberValue"], "");
                                        DisplayMember = GFunc.NEStr(drList[0]["ControlMemberDisplay"], "");

                                        GlobalUI.BindComboValue((TAUtil.TAComboBox)ctrl, listSettingID, DisplayMember, ValueMember, codeKey);

                                        ((TAUtil.TAComboBox)ctrl).LimitToList = GFunc.NEBool(drList[0]["ControlLimitToList"], false);
                                        ((TAUtil.TAComboBox)ctrl).TextRenderingMode = TextRenderingMode.GDI;
                                        ((TAUtil.TAComboBox)ctrl).Font = new Font("Calibri", 11);
                                        ((TAUtil.TAComboBox)ctrl).Appearance.BackColor = Color.White;
                                    }
                                }
                            }//End IF
                            #endregion Combo

                            #endregion

                        }//End Else CodeKey Check

                    }//End Else HasChilden Check

                }//End Loop


            }//End Try
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }



        public static string ListSettingID_Get(string controlSettingList, string controlNm)
        {
            try
            {

                string listSettingID = string.Empty;

                char[] charArray = controlSettingList.ToCharArray();
                Array.Reverse(charArray);
                string controlSettingListReverse = new string(charArray);

                int foundIndex = controlSettingList.IndexOf("|" + controlNm + "#NA#" + controlNm);
                if (foundIndex == -1)
                {//find for no BindingFieldNm 
                    foundIndex = controlSettingList.IndexOf("|0BF#NA#" + controlNm);
                }
                if (foundIndex == -1)
                {//find for no BindingFieldNm 
                    foundIndex = controlSettingList.IndexOf("|0GF#" + controlNm + "#NA#");
                }
                int endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('|'+bindingFieldNmorControlNm) ;Remark=> extra separator='|'

                foundIndex = controlSettingList.Length - foundIndex;
                int startIndex = controlSettingList.Length - controlSettingListReverse.IndexOf('|', foundIndex);
                string st = controlSettingList.Substring(startIndex, endIndex - startIndex);
                listSettingID = st.Split('#')[5];

                if (listSettingID.ToLower().Contains("func"))
                    listSettingID = listSettingID.Replace("Func", "").Replace("func", "");
                return listSettingID;

            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }

        public static string ListSettingID_Get(string controlSettingList, string controlNm, string gridNm)
        {
            try
            {
                string listSettingID = string.Empty;
                char[] charArray = controlSettingList.ToCharArray();
                Array.Reverse(charArray);
                string controlSettingListReverse = new string(charArray);

                int foundIndex = controlSettingList.IndexOf("|" + controlNm + "#" + gridNm + "#" + (controlNm.ToLower().Equals("0gf") ? "NA" : controlNm));
                int endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('|'+bindingFieldNmorControlNm) ;Remark=> extra separator='|'

                foundIndex = controlSettingList.Length - foundIndex;
                int startIndex = controlSettingList.Length - controlSettingListReverse.IndexOf('|', foundIndex);
                string st = controlSettingList.Substring(startIndex, endIndex - startIndex);

                listSettingID = st.Split('#')[5];
                if (listSettingID.ToLower().Contains("func"))
                    listSettingID = listSettingID.Replace("Func", "").Replace("func", "");
                return listSettingID;
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static string FormSettingID_PopupFormName_Get(string controlSettingList, string controlNm, string gridNm)
        {
            try
            {
                string listSettingID = string.Empty;
                char[] charArray = controlSettingList.ToCharArray();
                Array.Reverse(charArray);
                string controlSettingListReverse = new string(charArray);

                int foundIndex = controlSettingList.IndexOf("|" + controlNm + "#" + gridNm + "#" + (controlNm.ToLower().Equals("0gf") ? "NA" : controlNm));
                int endIndex = controlSettingList.IndexOf('|', ++foundIndex);//increase the index bcos of extra separator used to search for string ('|'+bindingFieldNmorControlNm) ;Remark=> extra separator='|'

                foundIndex = controlSettingList.Length - foundIndex;
                int startIndex = controlSettingList.Length - controlSettingListReverse.IndexOf('|', foundIndex);
                string st = controlSettingList.Substring(startIndex, endIndex - startIndex);

                listSettingID = st.Split('#')[4];
                return listSettingID;
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static TAUtil.TATextBoxEditor GetBrowseButton()
        {
            TAUtil.TATextBoxEditor txt = new TAUtil.TATextBoxEditor();
            Infragistics.Win.UltraWinEditors.EditorButton btn1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance btnAppearance = new Infragistics.Win.Appearance();
            btnAppearance.Image = WinUI.Properties.Resources.Browse;
            btn1.Appearance = btnAppearance;
            btn1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            txt.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            txt.ButtonsRight.Add(btn1);
            return txt;
        }
        public static Boolean RefreshGridDependentText(string DetKeyNm, string DetKeySubNm, string KeyName, string TextName, UltraGrid grd)
        {
            //Notes:
            //DetKeyNm      -- Detail PrimaryKey field Name e.g "DocItmKey" if there are no primarykey it will always be string.empty
            //DetKeySubNm   -- Detail 2nd PrimaryKey field Name if there are no 2nd field for the primarykey it will always be string.empty
            //KeyName       -- Detail columnkey for use in join in SP e.g "ItmAccKey"
            //TextName      -- Detail columnkey for value to match in SP e.g "ItmAccDes"
            //grdDetails    -- Grid from the caller

            try
            {
                string TableName = string.Empty;    //TableName Ref/Master use in the SP e.g MST_Acc
                string MstKeyName = string.Empty;   //FieldName to Join in the SP e.g AccKey
                string MstTextName = string.Empty;  //FieldName to match in the SP e.g AccDes

                DataTable dt = new DataTable();
                DataTable dtDetail = (DataTable)grd.DataSource;
                dt.Columns.Add("DetKey", typeof(int)); //Detail Primary Key Value (e.g DocItmKey -- 7)
                dt.Columns.Add("DetKeySub", typeof(int));//Use only if the Detail has 2 key for primarykey if not will always be 0
                dt.Columns.Add("KeyValue", typeof(int)); //The fieldValue use for join (e.g ItmAccKey -- 13)
                dt.Columns.Add("TextValue", typeof(string)); //current value (e.g ItmAccDes -- Account Receivable)
                dt.Columns.Add("TextNewValue", typeof(string));//the new value from the result of the update SP 

                //Validate the input parameters
                if (dtDetail.Rows.Count <= 0)
                    return false;

                //prepare xml data
                foreach (DataRow rowLocal in dtDetail.Rows)
                {
                    DataRow dr = dt.NewRow();
                    if (GFunc.NEStr(DetKeyNm, string.Empty) != string.Empty)
                        dr["DetKey"] = rowLocal[DetKeyNm];
                    else dr["DetKey"] = 0;

                    dr["KeyValue"] = rowLocal[KeyName];
                    dr["TextValue"] = rowLocal[TextName];
                    dr["DetKeySub"] = 0;
                    dt.Rows.Add(dr);

                }

                dt.TableName = "dt";
                string xmlData = GFunc.ConvertDataTableToXML(dt);

                //prepare parameters for storedprocedure
                string LinkKeys_Itm = "altitmkey,assitmkey,batchitmkey,bomitmkey,detitmkey,detitmkeyselect,eqptitmkey,eqptitmkeyselect,eqptsubdetitmkey,eqptsubdetitmkeyselect,eqptsubitmkey,eqptsubitmkeyselect,estitmkey,estitmkeyselect,itmfgkey,itmfgkeyselect,itmkey,itmkeyselect,joblabouritmkey,masteritmkey,masterkey,othitmkey,othitmkeyselect,substituteitmkey";
                string LinkKeys_Acc = "accickey,accinkey,acckey,acckeyadj,acckeycos,acckeyin,acckeymfnoh,acckeyrnd,accphkey,cacckey,docaccapkey,docaccarkey,docaccbkkey,docaccchargeskey,docaccgainkey,docaccglkey,docacckey,docacclabourkey,docacclosskey,docaccohkey,docaccrndkey,docaddcostacckey,docapplygainacckey,docoveralldisacc,docpaidacckey,expacckey,itmaccinkey,itmacckey,itmapplydisacckey,itmapplygainacckey,itmdocacckey,itmfromacckey,itmtoacckey,linkdocacckey,logacckey,vacckey";
                string LinkKeys_Con = "conkey,csgvendorkey,docconkey,doccvkey,docvendorkey,eqptconkey,intdocconkey,itmvendorkey,jobconkey,vendorkey";
                string LinkKeys_Job = "docjobkey,expjobkey,itmjobkey,itmjobkey,jobkey";

                if (LinkKeys_Itm.Split(",".ToCharArray()).Contains(KeyName.ToLower()))
                {
                    TableName = "Mst_Itm";
                    MstKeyName = "ItmKey";
                    MstTextName = "ItmID";
                }
                else if (LinkKeys_Acc.Split(",".ToCharArray()).Contains(KeyName.ToLower()))
                {
                    TableName = "Mst_Acc";
                    MstKeyName = "AccKey";
                    MstTextName = "AccID";
                }
                else if (LinkKeys_Con.Split(",".ToCharArray()).Contains(KeyName.ToLower()))
                {
                    TableName = "Mst_Con";
                    MstKeyName = "ConKey";
                    MstTextName = "ConNm";
                }
                else if (LinkKeys_Job.Split(",".ToCharArray()).Contains(KeyName.ToLower()))
                {
                    TableName = "Mst_Job";
                    MstKeyName = "JobKey";
                    MstTextName = "JobID";

                }

                //call storedprocedure
                List<SqlParameter> paraList = new List<SqlParameter>();
                paraList.Add(new SqlParameter("@TableName", TableName));
                paraList.Add(new SqlParameter("@KeyName", MstKeyName));
                paraList.Add(new SqlParameter("@TextName", MstTextName));
                paraList.Add(new SqlParameter("@XmlData", xmlData));
                SqlParameter RetValue = new SqlParameter();
                RetValue.ParameterName = "@RetValue";
                RetValue.Value = 0;
                RetValue.Direction = ParameterDirection.InputOutput;
                paraList.Add(RetValue);

                DataSet ds = GFunc.ExecuteProcDataSet("RefreshData_Get", paraList);

                //if retValue = 0 throw error.
                //if retValue = 1 perform Update and return true
                //if retValue = 2 no update require and return false

                if (GFunc.NEInt(RetValue.Value, 0) == 0)
                {
                    throw new Exception();
                }
                else if (GFunc.NEInt(RetValue.Value, 0) == 2)
                    return false;


                grd.FindForm().Cursor = Cursors.WaitCursor;

                grd.PerformAction(UltraGridAction.ExitEditMode);

                //update new values in grid
                foreach (DataRow rowSvr in ds.Tables[0].Rows)
                {
                    if (GFunc.NEStr(DetKeyNm, string.Empty) != string.Empty)
                    {
                        DataRow[] drs = dtDetail.Select("" + DetKeyNm + " = " + rowSvr["DetKey"] + " AND " + KeyName + " = " + rowSvr["KeyValue"] + "");
                        if (drs.Length > 0)
                        {
                            drs[0][TextName] = GFunc.NEStr(rowSvr["TextNewValue"], string.Empty);
                        }
                    }

                }
                dtDetail.AcceptChanges();
                grd.FindForm().Cursor = Cursors.Default;
                return true;
            }
            catch (TAException taex)
            {
                throw Error(taex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }

        }

        public static bool UpdateBatchChildItem(Document objDoc, UltraGrid itmDetailGrid)
        {
            try
            {
                DataTable dtGrid = (itmDetailGrid.DataSource as DataTable).Copy();

                //Select only StkB, FGB, StkB serial and FGB Serial
                DataRow[] drs = dtGrid.Select("LineLinkKey = 0 And (ItmType = 110 Or ItmType = 210 Or ItmType = 310 Or ItmType = 410)");
                if (drs.Length == 0)
                    return true;

                string headerQtyColumnName = string.Empty;

                foreach (DataRow item in drs)
                {
                    switch ((int)item["LineType"])
                    {
                        case 3000:
                        case 3010:
                        case 3020:
                        case 3030:
                            headerQtyColumnName = "FGProduceQty";
                            break;
                        case 3100:
                        case 3110:
                        case 3120:
                        case 3130:
                        case 3200:
                        case 3210:
                        case 3220:
                        case 3230:
                            headerQtyColumnName = "BOMUsed";
                            break;
                        default:
                            headerQtyColumnName = "ItmQty";//Default Qty column for most of the document
                            break;
                    }
                    decimal vChildQtyTotal = GFunc.NEDec(dtGrid.Compute("Sum(ItmBatchQty)", "LineLinkKey =" + item["DocItmKey"].ToString()), 0.00M);
                    if (Math.Abs(GFunc.NEDec(item[headerQtyColumnName], 0.00M)) != Math.Abs(vChildQtyTotal))
                    {
                        #region open popupform Batch
                        if (GlobalUI.bRuningImport == false)
                        {
                            switch (objDoc.DocCodeKey)
                            {
                                case (int)GEnum.SystemCode.Delivery_Order:
                                case (int)GEnum.SystemCode.Sales_Invoice:
                                case (int)GEnum.SystemCode.Sales_Debit_Note:
                                case (int)GEnum.SystemCode.Sales_Credit_Note:
                                case (int)GEnum.SystemCode.Cash_Sale:
                                case (int)GEnum.SystemCode.Cash_Debit_Note:
                                case (int)GEnum.SystemCode.Cash_Credit_Note:
                                case (int)GEnum.SystemCode.Purchase_Delivery:
                                case (int)GEnum.SystemCode.Purchase_Invoice:
                                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                case (int)GEnum.SystemCode.Inventory_Adjustment:
                                case (int)GEnum.SystemCode.Inventory_Transfer:
                                case (int)GEnum.SystemCode.Issue_Consignment:
                                case (int)GEnum.SystemCode.Return_Consignment:
                                    frmBatchEntry batchEntry = new frmBatchEntry(objDoc, itmDetailGrid, false);
                                    batchEntry.ShowDialog();
                                    return false;

                                case (int)GEnum.SystemCode.Inventory_Production:

                                    frmBatchSelection batchSelection = new frmBatchSelection(objDoc, itmDetailGrid as TAUtil.TAGridEditor, false);
                                    batchSelection.ShowDialog();
                                    return false;
                            }
                        }
                        #endregion

                        break; //exit loop
                    }
                }

                Hashtable docDet = new Hashtable();
                docDet.Add(GEnum.Details.Doc_Itm, dtGrid);
                if (DocComUtility.CalForm(objDoc, docDet, true, false) == false)
                    return false;

                return true;

            }//End Try
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static bool UpdateBatchChildItem(Document objDoc, UltraGrid itmDetailGrid, UltraGrid expDetailGrid)
        {
            try
            {
                DataTable dtGrid = (itmDetailGrid.DataSource as DataTable).Copy();

                //Select only StkB, FGB, StkB serial and FGB Serial
                DataRow[] drs = dtGrid.Select("LineLinkKey = 0 And (ItmType = 110 Or ItmType = 210 Or ItmType = 310 Or ItmType = 410)");
                if (drs.Length == 0)
                    return true;

                string headerQtyColumnName = string.Empty;

                foreach (DataRow item in drs)
                {

                    switch ((int)item["LineType"])
                    {
                        case 3000:
                        case 3010:
                        case 3020:
                        case 3030:
                            headerQtyColumnName = "FGProduceQty";
                            break;
                        case 3100:
                        case 3110:
                        case 3120:
                        case 3130:
                        case 3200:
                        case 3210:
                        case 3220:
                        case 3230:
                            headerQtyColumnName = "BOMUsed";
                            break;
                        default:
                            headerQtyColumnName = "ItmQty";//Default Qty column for most of the document
                            break;
                    }
                    decimal vChildQtyTotal = GFunc.NEDec(dtGrid.Compute("Sum(ItmBatchQty)", "LineLinkKey =" + item["DocItmKey"].ToString()), 0.00M);
                    if (Math.Abs(GFunc.NEDec(item[headerQtyColumnName], 0.00M)) != Math.Abs(vChildQtyTotal))
                    {
                        #region open popupform Batch
                        if (GlobalUI.bRuningImport == false)
                        {
                            switch (objDoc.DocCodeKey)
                            {
                                case (int)GEnum.SystemCode.Delivery_Order:
                                case (int)GEnum.SystemCode.Sales_Invoice:
                                case (int)GEnum.SystemCode.Sales_Debit_Note:
                                case (int)GEnum.SystemCode.Sales_Credit_Note:
                                case (int)GEnum.SystemCode.Cash_Sale:
                                case (int)GEnum.SystemCode.Cash_Debit_Note:
                                case (int)GEnum.SystemCode.Cash_Credit_Note:
                                case (int)GEnum.SystemCode.Purchase_Delivery:
                                case (int)GEnum.SystemCode.Purchase_Invoice:
                                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                case (int)GEnum.SystemCode.Inventory_Adjustment:
                                case (int)GEnum.SystemCode.Inventory_Transfer:
                                case (int)GEnum.SystemCode.Issue_Consignment:
                                case (int)GEnum.SystemCode.Return_Consignment:
                                case (int)GEnum.SystemCode.Inventory_Production:

                                    if (objDoc.DocCodeKey == (int)GEnum.SystemCode.Inventory_Production)
                                    {
                                        frmBatchSelection batchPopup = new frmBatchSelection(objDoc, itmDetailGrid as TAUtil.TAGridEditor, false);
                                        batchPopup.ShowDialog();
                                    }
                                    else
                                    {
                                        frmBatchEntry batchPopup = new frmBatchEntry(objDoc, itmDetailGrid, false);
                                        batchPopup.ShowDialog();
                                    }
                                    return false;
                                    break;
                            }
                        }
                        #endregion
                        break;
                    }
                }

                Hashtable docDet = new Hashtable();
                docDet.Add(GEnum.Details.Doc_Itm, dtGrid);
                docDet.Add(GEnum.Details.Doc_Exp, expDetailGrid.DataSource as DataTable);

                if (DocComUtility.CalForm(objDoc, docDet, true, false) == false)
                    return false;

                return true;
            }//End Try
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static bool UpdateAssemblyChildItem(Document objDoc, UltraGrid itmDetailGrid)
        {
            try
            {
                DataTable dtGrid = itmDetailGrid.DataSource as DataTable;

                //Select only Assembly itmtype
                DataRow[] drs = dtGrid.Select("LineLinkKey = 0 And ISNULL(ItmType,0) = 250");

                if (drs.Length == 0)
                    return true;

                foreach (DataRow dr in drs)
                {
                    int vChildCount = GFunc.NEInt(dtGrid.Compute("Count(DocItmKey)", "LineLinkKey =" + dr["DocItmKey"].ToString()), 0);
                    if (vChildCount == 0)
                    {
                        #region open popupform Assembly
                        if (GlobalUI.bRuningImport == false)
                        {
                            switch (objDoc.DocCodeKey)
                            {
                                case (int)GEnum.SystemCode.Delivery_Order:
                                case (int)GEnum.SystemCode.Sales_Invoice:
                                case (int)GEnum.SystemCode.Sales_Debit_Note:
                                case (int)GEnum.SystemCode.Sales_Credit_Note:
                                case (int)GEnum.SystemCode.Cash_Sale:
                                case (int)GEnum.SystemCode.Cash_Debit_Note:
                                case (int)GEnum.SystemCode.Cash_Credit_Note:
                                case (int)GEnum.SystemCode.Purchase_Delivery:
                                case (int)GEnum.SystemCode.Purchase_Invoice:
                                case (int)GEnum.SystemCode.Purchase_Debit_Note:
                                case (int)GEnum.SystemCode.Purchase_Credit_Note:
                                case (int)GEnum.SystemCode.Inventory_Adjustment:
                                case (int)GEnum.SystemCode.Inventory_Transfer:
                                case (int)GEnum.SystemCode.Issue_Consignment:
                                case (int)GEnum.SystemCode.Return_Consignment:
                                case (int)GEnum.SystemCode.Inventory_Production:
                                    frmAssemblyEntry assemblyPopup = new frmAssemblyEntry(objDoc, itmDetailGrid, GFunc.NEInt(dr["DocItmKey"], 0), true);
                                    assemblyPopup.ShowDialog();
                                    return false;
                                    break;
                            }
                        }
                        #endregion
                    }
                }

                Hashtable docDet = new Hashtable();
                docDet.Add(GEnum.Details.Doc_Itm, dtGrid);
                if (DocComUtility.CalForm(objDoc, docDet, true, false) == false)
                    return false;
                return true;
            }//End Try
            catch (TAException tex)
            {
                throw Error(tex, false, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, false);
            }
        }
        public static void TabKeyDownForGrid(UltraGrid grid)
        {
            try
            {
                if (grid != null)
                {
                    grid.Focus();

                    UltraGridColumn FirstVisCol = grid.DisplayLayout.Bands[0].Columns[0].GetRelatedVisibleColumn(VisibleRelation.First);
                    if (FirstVisCol != null)
                    {
                        grid.ActiveCell = grid.Rows.TemplateAddRow.Cells[FirstVisCol.Key];
                        grid.PerformAction(UltraGridAction.EnterEditMode, false, false);
                    }
                }
            }
            catch (TAException tex)
            {
                Error(tex, true, false);
            }
            catch (Exception ex)
            {
                Error(ex, true, false);
            }
        }

        public static void CustomDataError(object sender, TAUtil.TAErrorEventArgs e, Form f)
        {
            string msgID = string.Empty;
            try
            {
                if (e.ErrorCode == TAUtil.TAErrorCode.NOT_IN_LIST)
                {
                    if (sender.GetType() == typeof(TAUtil.TAGridEditor))
                    {
                        TAUtil.TAGridEditor grd = sender as TAUtil.TAGridEditor;
                        if (grd.ActiveCell.Column.EditorComponent != null)
                        {
                            grd.PerformAction(UltraGridAction.EnterEditMode);
                            GlobalUI.ItemNotInList(grd.ActiveCell, null, 0);
                        }
                    }
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_NUMERIC || e.ErrorCode == TAUtil.TAErrorCode.INTEGER_EXCEED_LIMIT)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeNumeric + "% Data Type");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.FORMULA_NOT_RECOGNIZE)
                {
                    throw new TAException("FORMULA NOT RECOGNIZE");
                }
                else if (e.ErrorCode == TAUtil.TAErrorCode.INVALID_DATE)
                {
                    throw new TAException(MsgID.Common.InvalidCellDataTypeDate + "% Date");
                }
                else
                {
                    throw new TAException("Invalid Value");
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false, f);
            }
            catch (Exception ex)
            {
                throw Error(ex, false, f);
            }
        }

        /// <summary>
        /// Handles the UltraGridFilterUIProvider's AfterMenuPopulate event which is used like Excel Data Filter.
        /// </summary>
        public static void filterUIProvider_AfterMenuPopulate(object sender, AfterMenuPopulateEventArgs e)
        {
            if (e.ColumnFilter == null)
                return;

            UltraGridBand band = e.ColumnFilter.Column.Band;

            foreach (UltraGridColumn col in band.Columns)
            {
                if (col.EditorComponent != null)
                {
                    //  If the Combobox field Data Type is numeric, by default it will display
                    //  a 'Numeric Filters' menu item. //  however, let's not present this to the end user, since 
                    //the combo box includes strings from their perspective

                    if (col.EditorComponent.GetType() == typeof(TAUtil.TAComboBox))
                    {
                        foreach (FilterTool tool in e.MenuItems)
                        {
                            if (string.Equals(tool.Id, "Number Filters"))
                            {
                                e.MenuItems.Remove(tool);
                                break;
                            }
                        }
                    }
                }
            }
        }



        //Set Error Methods
        private static Exception Error(Exception ex, bool ShowMessage, bool AddLog)
        {
            try
            {
                ex = SysAuditLogUtility.AppendException(ex, ShowMessage);
                if (AddLog)
                    SysAuditLogUtility.AddErrorLog_New(ex);

            }
            catch (Exception nex)
            {
                throw Error(nex, false, false);
            }
            return ex;
        }//CodeCompleted
        private static TAException Error(TAException ex, bool ShowMessage, bool AddLog)
        {
            try
            {
                ex = SysAuditLogUtility.AppendTAException(ex, ShowMessage);
                if (AddLog)
                    SysAuditLogUtility.AddErrorLog_New(ex);
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }//CodeCompleted


        #region Error Logging Methods
        public static Exception Error(Exception ex, bool ShowMessage, Form f)
        {
            Exception l_tmpex = ex;
            try
            {
                if (f.ActiveControl != null)
                {
                    if (f.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { f.Name, f.ActiveControl });
                    }
                    else
                    {
                        l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { f.Name, f.ActiveControl.Name, ((TAUtil.TAGridEditor)f.ActiveControl).ActiveCell.Column.Key });
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
        public static TAException Error(TAException ex, bool ShowMessage, Form f)
        {
            try
            {
                TAException l_tmpex = ex;
                if (f.ActiveControl != null)
                {
                    if (f.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                    {
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { f.Name, f.ActiveControl });
                    }
                    else
                    {
                        string ActiveColKey = "";
                        if (((TAUtil.TAGridEditor)f.ActiveControl).ActiveCell != null)
                        {
                            ActiveColKey = ((TAUtil.TAGridEditor)f.ActiveControl).ActiveCell.Column.Key;
                        }
                        l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { f.Name, f.ActiveControl.Name, ActiveColKey });
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
        #endregion

       

         //May added for Fin Report popup on 25 Nov 2014 (added function)
        public static void OpenQOForm(string docID)
        {
            int DocKey = GFunc.DocKey_Get((int)GEnum.SystemCode.Quotation, docID);
            if (DocKey == 0)
            {
                MsgBox.Show("Document ID doesn't exist");
                return;
            }
            frmARQO ARQO = new frmARQO(GEnum.SystemCode.Quotation);
            ARQO.Show();
            ARQO.MdiParent = frmMain.gfrmMain;
            ARQO.OnDocList_OpenRecord(DocKey);
            ARQO.Activate();           
        }
    
    }

    //Grid Printing Related
    public class Grid_Print
    {
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument ultraGridPrintDocument1;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog ultraPrintPreviewDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        public Grid_Print(Infragistics.Win.UltraWinGrid.UltraGrid grd)
        {
            ultraGridPrintDocument1 = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument();
            ultraPrintPreviewDialog1 = new Infragistics.Win.Printing.UltraPrintPreviewDialog();
            printDialog1 = new PrintDialog();

            ultraPrintPreviewDialog1.Printing += new Infragistics.Win.Printing.PrintingEventHandler(ultraPrintPreviewDialog1_Printing);
            ultraPrintPreviewDialog1.Printed += new EventHandler(ultraPrintPreviewDialog1_Printed);
            ultraPrintPreviewDialog1.FormClosing += new FormClosingEventHandler(ultraPrintPreviewDialog1_FormClosing);

            ultraPrintPreviewDialog1.Document = ultraGridPrintDocument1;
            this.ultraGridPrintDocument1.Grid = grd;
        }

        void ultraPrintPreviewDialog1_Printed(object sender, EventArgs e)
        {
            ultraPrintPreviewDialog1.Document.PrinterSettings = printDialog1.PrinterSettings;
            ultraPrintPreviewDialog1.Document.PrinterSettings = null;
        }
        void ultraPrintPreviewDialog1_Printing(object sender, Infragistics.Win.Printing.PrintingEventArgs e)
        {
            // cancle ultra Printing , Show Print Dialog and replace PrinterSettings.
            e.Cancel = true;
            ultraPrintPreviewDialog1.DisplayPrintStatus = false;
            ultraPrintPreviewDialog1.DisplayPreviewStatus = false;

            printDialog1.AllowCurrentPage = true;
            printDialog1.AllowSelection = true;
            printDialog1.AllowSomePages = true;
            printDialog1.PrinterSettings = ultraPrintPreviewDialog1.Document.PrinterSettings;

            DialogResult res = printDialog1.ShowDialog(ultraPrintPreviewDialog1);

            if (res == DialogResult.OK)
            {
                ultraPrintPreviewDialog1.Document.PrinterSettings = printDialog1.PrinterSettings;
                ultraPrintPreviewDialog1.Document.Print();
            }
        }
        void ultraPrintPreviewDialog1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ultraPrintPreviewDialog1.Document.PrinterSettings = null;
        }
        public void Preview()
        {
            ultraPrintPreviewDialog1.DisplayPreviewStatus = false;
            ultraPrintPreviewDialog1.ShowDialog();
        }
        public void Dispose()
        {
            ultraPrintPreviewDialog1.Dispose();
            ultraGridPrintDocument1.Dispose();
            ultraGridPrintDocument1 = null;
            ultraPrintPreviewDialog1 = null;
        }
    }

    //Extension Methods  -- Add More methods to DataTable & other types
    static class Extensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> enumberable)
        {
            //DataTable table = new DataTable("Generated");
            try
            {

                DataTable table = new DataTable("Table1");

                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


                foreach (PropertyInfo pi in properties)
                    table.Columns.Add(pi.Name, BaseType(pi.PropertyType));

                foreach (T t in enumberable)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyInfo pi in properties)
                        row[pi.Name] = t.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, t, null) == null ? DBNull.Value : t.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, t, null);
                    table.Rows.Add(row);
                }


                return table;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static Type BaseType(Type oType)
        {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(System.Nullable<>)
            )
            {
                return System.Nullable.GetUnderlyingType(oType);
            }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else
            {
                return oType;
            }
        }

    }
    public static class DBNullableExtensions
    {
        public static object ToDBValue<T>(this Nullable<T> value) where T : struct
        {
            return value.HasValue ? (object)value.Value : DBNull.Value;
        }
    }


    #region DTC Utility

    /// <summary>
    /// Used to get the DTC settings on a machine
    /// </summary>
    internal static class DTCUtility
    {
        /// <summary>
        /// Reads the DTC settings.
        /// </summary>
        /// <returns></returns>
        public static DTCSettings ReadDTCSettings()
        {
            //Lets grab the DTC settings from the registry and then populate our entity
            try
            {
                //we work from the local machine key
                var localReg = Registry.LocalMachine;
                //set the root dtc key
                string rootKey = @"Software\Microsoft\MSDTC";
                string securityKey = rootKey + @"\Security";
                var dtcReg = localReg.OpenSubKey(rootKey, true);
                var securityReg = localReg.OpenSubKey(securityKey, true);

                DTCSettings settings = new DTCSettings();
                try
                {
                    //is network dtc access even turned on at all
                    settings.NetworkDTCAccess = (int)securityReg.GetValue("NetworkDtcAccess", 0) == 1;

                    //default to false, then prove it is on.
                    settings.AllowInbound = false;
                    settings.AllowOutbound = false;
                    //check to see if network access is on at all and transactions allowed
                    if (settings.NetworkDTCAccess &&
                        (((int)securityReg.GetValue("NetworkDtcAccessTransactions", 0)) == 1))
                    {
                        //so we can talk to network, can we allow outbound?
                        if (settings.NetworkDTCAccess &&
                        (((int)securityReg.GetValue("NetworkDtcAccessOutbound", 0)) == 1))
                        {
                            settings.AllowOutbound = true;
                        }
                        //same as above, but inbound connections allowed
                        if (settings.NetworkDTCAccess &&
                        (((int)securityReg.GetValue("NetworkDtcAccessInbound", 0)) == 1))
                        {
                            settings.AllowInbound = true;
                        }
                    }
                    //grab authentication values
                    bool allowOnlySecureRPCCalls = (int)dtcReg.GetValue(@"AllowOnlySecureRpcCalls", 0) == 1;
                    bool fallbackToUnsecuredRPC = (int)dtcReg.GetValue(@"FallbackToUnsecureRPCIfNecessary", 0) == 1;
                    bool turnOffRpcSecurity = (int)dtcReg.GetValue(@"TurnOffRpcSecurity", 0) == 1;
                    //process the logic to determine which mode is active.
                    settings.MutualAuthenticationRequired = (allowOnlySecureRPCCalls && !fallbackToUnsecuredRPC && !turnOffRpcSecurity);
                    settings.IncomingCallerAuthenticationRequired = (!allowOnlySecureRPCCalls && fallbackToUnsecuredRPC && !turnOffRpcSecurity);
                    settings.NoAuthenticationRequired = (!allowOnlySecureRPCCalls && !fallbackToUnsecuredRPC && turnOffRpcSecurity);

                }
                finally
                {
                    try
                    {
                        //close but not a failure if there is an error
                        dtcReg.Close();
                        securityReg.Close();
                    }
                    catch { }
                }
                return settings;
            }
            catch
            {
                return null;
            }
        }

    }

    /// <summary>
    /// DTC Settings
    /// </summary>
    internal class DTCSettings
    {
        /// <summary>
        /// Determines whether DTC on the local computer is allowed to access the network.
        /// This setting must be enabled in combination with one of the other settings to
        /// enable network DTC transactions.
        /// </summary>
        /// <value><c>true</c> if [network DTC access]; otherwise, <c>false</c>.</value>
        public bool NetworkDTCAccess { get; set; }
        /// <summary>
        /// Allows a distributed transaction that originates from a remote computer to run on this computer.
        /// Default setting: Off
        /// </summary>
        /// <value><c>true</c> if [allow inbound]; otherwise, <c>false</c>.</value>
        public bool AllowInbound { get; set; }
        /// <summary>
        /// Allows the local computer to initiate a transaction and run it on a remote computer.
        /// </summary>
        /// <value><c>true</c> if [allow outbound]; otherwise, <c>false</c>.</value>
        public bool AllowOutbound { get; set; }
        /// <summary>
        /// Adds support for mutual authentication in future versions and is the highest secured
        /// communication mode. In the current versions of Windows and Windows Server, it is
        /// functionally equivalent to the Incoming Caller Authentication Required setting.
        /// This is the recommended transaction mode for clients running Windows XP SP2 and
        /// servers running a member of the Windows Server 2003 family.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [mutual authentication required]; otherwise, <c>false</c>.
        /// </value>
        public bool MutualAuthenticationRequired { get; set; }
        /// <summary>
        /// Requires the local DTC to communicate with a remote DTC using only encrypted messages
        /// and mutual authentication. This setting is recommended for servers running Windows Server
        /// 2003 that are operating in a cluster.
        /// Only Windows Server 2003 and Windows XP SP2 support this feature, so you should only use
        /// this if you know that the DTC on the remote computer runs either the Windows Server 2003
        /// or Windows XP SP2 operating system.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [incoming caller authentication required]; otherwise, <c>false</c>.
        /// </value>
        public bool IncomingCallerAuthenticationRequired { get; set; }
        /// <summary>
        /// Provides system compatibility between previous versions of the Windows operating system.
        /// When enabled, communication on the network between DTCs can fall back to a non-authentication
        /// or non-encrypted communication if a secure communication channel cannot be established.
        /// This setting should be used if the DTC on the remote computer runs a Windows 2000 operating system
        /// or a Windows XP operating system earlier than SP2. This setting is also useful when the DTCs
        /// that are involved are located on computers that are in domains that do not have an established
        /// trust relationship or if the computers are part of a Windows workgroup.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [no authentication required]; otherwise, <c>false</c>.
        /// </value>
        public bool NoAuthenticationRequired { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder state = new StringBuilder();
            state.AppendFormat("NetworkDTCAccess: {0}\r\n", NetworkDTCAccess);
            state.AppendFormat("AllowInbound: {0}\r\n", AllowInbound);
            state.AppendFormat("AllowOutbound: {0}\r\n", AllowOutbound);
            state.AppendFormat("MutualAuthenticationRequired: {0}\r\n", MutualAuthenticationRequired);
            state.AppendFormat("IncomingCallerAuthenticationRequired: {0}\r\n", IncomingCallerAuthenticationRequired);
            state.AppendFormat("NoAuthenticationRequired: {0}\r\n", NoAuthenticationRequired);
            return state.ToString();
        }

    }

    #endregion

    

}

public interface DocInterface
{
    void GetDocInfor(out Document objDoc, out Hashtable details);
}
