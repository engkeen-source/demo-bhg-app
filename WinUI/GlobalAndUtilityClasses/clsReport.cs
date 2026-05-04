using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Collections;
using BOLib;
using System.Windows.Forms;
using DataDynamics.ActiveReports;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO.Pipes;
using TAUtil;
using System.Drawing.Printing;
using System.Xml;
using Microsoft.Office.Interop.Excel;



namespace WinUI
{
    public class ReportLoader
    {
        #region Variable

        private frmRpxViewer reportViewerForm = null;
        private SqlConnection _cn = null;
        private int _repKey = 0;
        private string _recordSource = string.Empty;
        public string _reportFileName = "";
        public DataDynamics.ActiveReports.ActiveReport rpxDoc;
        public CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc;
        string pMth1, pMth2, pMth3, pMth4, pMth5, pMth6, pMth7, pMth8, pMth9, pMth10, pMth11, pMth12;
        private List<SqlParameter> _reportSqlParameter;
        #endregion

        #region Properties

        public List<SqlParameter> ReportSqlParameter { get { return _reportSqlParameter; } set { _reportSqlParameter = value; } }
        public List<ReportParameter> ReportParameter { get; set; }
        public string ReportName { get; set; }
        public int RepKey { get; set; }

        public string RptPrintCondition { get; set; }
        public System.Data.DataTable ReportDataSource { get; set; }

        //mic check, added this property to be able to set event for  updating DocPrinted flag
        public frmRpxViewer RpxViewerForm
        {
            get
            {
                return reportViewerForm;
            }
        }
        #endregion

        private void AfterRecordUpdated()
        {
            System.Data.DataTable dt = null;
            if (_cn == null)
            {
                dt = GFunc.ExecuteProc(_recordSource, GetSqlParameter(_repKey));
            }
            else
            {
                dt = GFunc.ExecuteProc(_cn, _recordSource, GetSqlParameter(_repKey));
            }

            reportViewerForm.rptDoc.DataSource = dt;
        }
        public bool MtnPara(DateTime AgeDate, string rptName)
        {

            if (AgeDate.Year == 1)//AgeDate is hasn't set
            {
                AgeDate = DateTime.Today;
            }
            int days = 0;
            DateTime firstDate;


            days = (AgeDate.Day) - 1;
            firstDate = AgeDate.AddDays(-days);
            if (RepKey == 1280)
            {
                pMth1 = (firstDate.AddMonths(0).ToString("dd MMM yyyy"));
                pMth2 = (firstDate.AddMonths(-1).ToString("dd MMM yyyy"));
                pMth3 = (firstDate.AddMonths(-2).ToString("dd MMM yyyy"));
                pMth4 = (firstDate.AddMonths(-3).ToString("dd MMM yyyy"));
                pMth5 = (firstDate.AddMonths(-4).ToString("dd MMM yyyy"));
                pMth6 = (firstDate.AddMonths(-5).ToString("dd MMM yyyy"));
                pMth7 = (firstDate.AddMonths(-6).ToString("dd MMM yyyy"));
                pMth8 = (firstDate.AddMonths(-7).ToString("dd MMM yyyy"));
                pMth9 = (firstDate.AddMonths(-8).ToString("dd MMM yyyy"));
                pMth10 = (firstDate.AddMonths(-9).ToString("dd MMM yyyy"));
                pMth11 = (firstDate.AddMonths(-10).ToString("dd MMM yyyy"));
                pMth12 = (firstDate.AddMonths(-11).ToString("dd MMM yyyy"));
            }
            else if (RepKey == 1875 || RepKey == 1880 || RepKey == 1890 || RepKey == 1510 || RepKey == 1525)
            {
                pMth1 = (firstDate.AddMonths(0).ToString("dd MMM yyyy"));
                pMth2 = (firstDate.AddMonths(-1).ToString("dd MMM yyyy"));
                pMth3 = (firstDate.AddMonths(-2).ToString("dd MMM yyyy"));
                pMth4 = (firstDate.AddMonths(-3).ToString("dd MMM yyyy"));
                pMth5 = (firstDate.AddMonths(-4).ToString("dd MMM yyyy"));
                pMth6 = (firstDate.AddMonths(-5).ToString("dd MMM yyyy"));
                pMth7 = (firstDate.AddMonths(-6).ToString("dd MMM yyyy"));
                pMth8 = (firstDate.AddMonths(-7).ToString("dd MMM yyyy"));
                pMth9 = (firstDate.AddMonths(-8).ToString("dd MMM yyyy"));
                pMth10 = (firstDate.AddMonths(-9).ToString("dd MMM yyyy"));
                pMth11 = (firstDate.AddMonths(-10).ToString("dd MMM yyyy"));
                pMth12 = (firstDate.AddMonths(-11).ToString("dd MMM yyyy"));
            }
            else if (RepKey == 1305 || RepKey == 1550)
            {
                if (GFunc.CompareString(rptName, "Cust_Bal3YearF.rpx") || GFunc.CompareString(rptName, "Cust_Bal3YearH.rpx"))
                {
                    pMth1 = (firstDate.AddYears(0).ToString("yyyy"));
                    pMth2 = (firstDate.AddYears(-1).ToString("yyyy"));
                    pMth3 = (firstDate.AddYears(-2).ToString("yyyy"));
                    pMth4 = (firstDate.AddYears(-3).ToString("yyyy"));
                    pMth5 = (firstDate.AddYears(-4).ToString("yyyy"));
                    pMth6 = (firstDate.AddYears(-5).ToString("yyyy"));
                    pMth7 = (firstDate.AddYears(-6).ToString("yyyy"));
                    pMth8 = (firstDate.AddYears(-7).ToString("yyyy"));
                    pMth9 = (firstDate.AddYears(-8).ToString("yyyy"));
                    pMth10 = (firstDate.AddYears(-9).ToString("yyyy"));
                    pMth11 = (firstDate.AddYears(-10).ToString("yyyy"));
                    pMth12 = (firstDate.AddYears(-11).ToString("yyyy"));
                }
                else
                {
                    pMth1 = (firstDate.AddMonths(0).ToString("MMM yyyy"));
                    pMth2 = (firstDate.AddMonths(-1).ToString("MMM yyyy"));
                    pMth3 = (firstDate.AddMonths(-2).ToString("MMM yyyy"));
                    pMth4 = (firstDate.AddMonths(-3).ToString("MMM yyyy"));
                    pMth5 = (firstDate.AddMonths(-4).ToString("MMM yyyy"));
                    pMth6 = (firstDate.AddMonths(-5).ToString("MMM yyyy"));
                    pMth7 = (firstDate.AddMonths(-6).ToString("MMM yyyy"));
                    pMth8 = (firstDate.AddMonths(-7).ToString("MMM yyyy"));
                    pMth9 = (firstDate.AddMonths(-8).ToString("MMM yyyy"));
                    pMth10 = (firstDate.AddMonths(-9).ToString("MMM yyyy"));
                    pMth11 = (firstDate.AddMonths(-10).ToString("MMM yyyy"));
                    pMth12 = (firstDate.AddMonths(-11).ToString("MMM yyyy"));
                }
            }
            else
            {
                pMth1 = (firstDate.AddMonths(0).ToString("dd MMM yyyy"));
                pMth2 = (firstDate.AddMonths(-1).ToString("dd MMM yyyy"));
                pMth3 = (firstDate.AddMonths(-2).ToString("dd MMM yyyy"));
                pMth4 = (firstDate.AddMonths(-3).ToString("dd MMM yyyy"));
                pMth5 = (firstDate.AddMonths(-4).ToString("dd MMM yyyy"));
                pMth6 = (firstDate.AddMonths(-5).ToString("dd MMM yyyy"));
                pMth7 = (firstDate.AddMonths(-6).ToString("dd MMM yyyy"));
                pMth8 = (firstDate.AddMonths(-7).ToString("dd MMM yyyy"));
                pMth9 = (firstDate.AddMonths(-8).ToString("dd MMM yyyy"));
                pMth10 = (firstDate.AddMonths(-9).ToString("dd MMM yyyy"));
                pMth11 = (firstDate.AddMonths(-10).ToString("dd MMM yyyy"));
                pMth12 = (firstDate.AddMonths(-11).ToString("dd MMM yyyy"));
            }
            return true;
        }
        private void GetReportParameters(int repKey)
        {
            SYSRepParas objParas = null;
            string companyName = string.Empty;
            if (_cn == null)
            {
                objParas = SYSRepParas.Get(repKey);
                companyName = SysOptionUtility.GetStr("CompanyName");
            }
            else
            {
                objParas = SYSRepParas.Get(_cn, repKey);
                companyName = SysOptionUtility.GetStr("CompanyName", _cn);
            }


            foreach (DataRow obj in objParas.Rows)
            {
                if (rpxDoc.Parameters[obj["ParName"].ToString()]!=null)
                {
                    rpxDoc.Parameters[obj["ParName"].ToString()].PromptUser = false;

                    switch (obj["ParName"].ToString().ToLower())
                    {
                        case "pcmpname":
                            rpxDoc.Parameters[obj["ParName"].ToString()].DefaultValue = companyName;
                            break;
                        case "psubreppath":
                            rpxDoc.Parameters[obj["ParName"].ToString()].DefaultValue =System.Windows.Forms.Application.StartupPath + @"\Reports\" + obj["ParDefaultValue"].ToString();
                            break;
                        //Add more default Parameter
                        //
                        //
                        default:
                            ReportParameter para = new ReportParameter();
                            if (ReportParameter != null)
                            {
                                para = ReportParameter.Find(p => p.ParameterName == obj["ParName"].ToString());
                                if (!GFunc.IsNE(para))
                                {
                                    rpxDoc.Parameters[obj["ParName"].ToString()].DefaultValue = para.ParameterValue.ToString();
                                }
                            }
                            else
                            {
                                rpxDoc.Parameters[obj["ParName"].ToString()].DefaultValue = obj["ParDefaultValue"].ToString();
                            }
                            break;
                    }
                }
            }

        }
        public void UpdateReportParameters()
        {
            try
            {
                foreach (ReportParameter obj in this.ReportParameter)
                {

                    switch (obj.ParameterName.ToLower())
                    {
                        case "pm1":
                            obj.ParameterValue = pMth1;
                            break;
                        case "pm2":
                            obj.ParameterValue = pMth2;
                            break;
                        case "pm3":
                            obj.ParameterValue = pMth3;
                            break;
                        case "pm4":
                            obj.ParameterValue = pMth4;
                            break;
                        case "pm5":
                            obj.ParameterValue = pMth5;
                            break;
                        case "pm6":
                            obj.ParameterValue = pMth6;
                            break;
                        case "pm7":
                            obj.ParameterValue = pMth7;
                            break;
                        case "pm8":
                            obj.ParameterValue = pMth8;
                            break;
                        case "pm9":
                            obj.ParameterValue = pMth9;
                            break;
                        case "pm10":
                            obj.ParameterValue = pMth10;
                            break;
                        case "pm11":
                            obj.ParameterValue = pMth11;
                            break;
                        case "pm12":
                            obj.ParameterValue = pMth12;
                            break;
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
        }
        private List<SqlParameter> GetSqlParameter(int repKey)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();

            SYSRepCriterias objCriterias = null;


            if (_cn == null)
            {
                objCriterias = SYSRepCriterias.Get(repKey);
            }
            else
            {
                if (_cn.ConnectionString == "")
                    _cn.ConnectionString = AppInfor.CurrentDBConnectionStr;
                objCriterias = SYSRepCriterias.Get(_cn, repKey);
            }
            foreach (DataRow obj in objCriterias.Rows)
            {
                SqlParameter para = new SqlParameter();
                switch (obj["CriteriaNm"].ToString())
                {
                    case "":
                        break;
                    default:
                        if (ReportSqlParameter != null)
                        {
                            para = ReportSqlParameter.Find(p => GFunc.CompareString(p.ParameterName, "@" + obj["CriteriaNm"].ToString()));
                            if (GFunc.IsNE(para))
                            {
                                para = new SqlParameter("@" + obj["CriteriaNm"].ToString(), obj["CriteriaDefaultValue"]);
                            }

                        }
                        else
                        {
                            para = new SqlParameter("@" + obj["CriteriaNm"], obj["CriteriaDefaultValue"]);
                        }
                        break;
                }
                paraList.Add(para);
            }

            return paraList;
        }

        public string CreatePdfFile(string ExportLocation, string fileNamewoExt)
        {
            int i = 0;
            bool fail = true;
            string tmpFileName = ExportLocation;
            string fileName = "";
        
            fileName = ReportName;
            tmpFileName = ExportLocation + fileNamewoExt + ".pdf";
          

            if (fileName.ToLower().EndsWith(".rpx"))
            {
                DataDynamics.ActiveReports.Export.Pdf.PdfExport pdfExport = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
                rpxDoc.Run(false);
                while (fail && i < 10)
                {
                    try
                    {
                        pdfExport.Export(rpxDoc.Document, tmpFileName);
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".pdf";
                    }
                }
            }
            else if (fileName.ToLower().EndsWith(".rpt"))
            {
                CrystalDecisions.Shared.DiskFileDestinationOptions dfdOptions = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                CrystalDecisions.Shared.ExportOptions exOptions = this.rptDoc.ExportOptions;

                exOptions = this.rptDoc.ExportOptions;
                exOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                exOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;

                exOptions.DestinationOptions = dfdOptions;

                while (fail && i < 10)
                {
                    try
                    {
                        dfdOptions.DiskFileName = tmpFileName;
                        this.rptDoc.Export();
                        this.rptDoc.Close();
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".pdf";
                    }
                }

            }
            return tmpFileName;
        }

        private string CreatePdfFile(string ExportLocation)
        {
            int i = 0;
            bool fail = true;
            string tmpFileName = ExportLocation;
            string fileName = "";
            if (_reportFileName != "")
            {
                fileName = _reportFileName;
                tmpFileName = ExportLocation + _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + ".pdf";
            }
            else if (ReportName != "" && System.IO.Path.GetFileName(ExportLocation).Length == 0)
            {
                fileName = ReportName;
                tmpFileName = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + ".pdf";
            }
            else if (ReportName != "")
                fileName = ReportName;

            if (fileName.ToLower().EndsWith(".rpx"))
            {
                DataDynamics.ActiveReports.Export.Pdf.PdfExport pdfExport = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
                while (fail && i < 10)
                {
                    try
                    {
                        pdfExport.Export(rpxDoc.Document, tmpFileName);
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".pdf";
                    }
                }
            }
            else if (fileName.ToLower().EndsWith(".rpt"))
            {
                CrystalDecisions.Shared.DiskFileDestinationOptions dfdOptions = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                CrystalDecisions.Shared.ExportOptions exOptions = this.rptDoc.ExportOptions;

                exOptions = this.rptDoc.ExportOptions;
                exOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                exOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
               
                exOptions.DestinationOptions = dfdOptions;

                while (fail && i < 10)
                {
                    try
                    {
                        dfdOptions.DiskFileName = tmpFileName;
                        this.rptDoc.Export();
                        this.rptDoc.Close();
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".pdf";
                    }
                }              

            }
            return tmpFileName;
        }

        private string CreatePdfNExcelFile(string ExportLocation)
        {
            int i = 0;
            bool fail = true;
            string tmpFileNamepdf = ExportLocation;
            string tmpFileNamexls = ExportLocation;
            string fileName = "";
            if (_reportFileName != "")
            {
                fileName = _reportFileName;
                tmpFileNamepdf = ExportLocation + _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + ".pdf";
                tmpFileNamexls = ExportLocation + _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + "_NoFormat.xls";
            }
            else if (ReportName != "" && System.IO.Path.GetFileName(ExportLocation).Length == 0)
            {
                fileName = ReportName;
                tmpFileNamepdf = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + ".pdf";
                tmpFileNamexls = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + "_NoFormat.xls";
            }
            else if (ReportName != "")
                fileName = ReportName;

            if (fileName.ToLower().EndsWith(".rpx"))
            {
                DataDynamics.ActiveReports.Export.Pdf.PdfExport pdfExport = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
                while (fail && i < 10)
                {
                    try
                    {
                        pdfExport.Export(rpxDoc.Document, tmpFileNamepdf);
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileNamepdf = tmpFileNamepdf.Remove(tmpFileNamepdf.LastIndexOf(".")) + i.ToString() + ".pdf";
                    }
                }
            }
            else if (fileName.ToLower().EndsWith(".rpt"))
            {
                CrystalDecisions.Shared.DiskFileDestinationOptions dfdOptions = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                CrystalDecisions.Shared.ExportOptions exOptions = this.rptDoc.ExportOptions;

                exOptions = this.rptDoc.ExportOptions;
                exOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                //exOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;

                //exOptions.DestinationOptions = dfdOptions;

                //while (fail && i < 10)
                //{
                //    try
                //    {
                //        dfdOptions.DiskFileName = tmpFileNamepdf;
                //        this.rptDoc.Export();
                       
                //        fail = false;
                //    }
                //    catch
                //    {
                //        i++;
                //        tmpFileNamepdf = tmpFileNamepdf.Remove(tmpFileNamepdf.LastIndexOf(".")) + i.ToString() + ".pdf";
                //    }
                //}

                //exOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;

                //exOptions.DestinationOptions = dfdOptions;

                //fail = true;
                //while (fail && i < 10)
                //{
                //    try
                //    {
                //        dfdOptions.DiskFileName = tmpFileNamexls;
                //        this.rptDoc.Export();

                //        fail = false;
                //    }
                //    catch
                //    {
                //        i++;
                //        tmpFileNamexls = tmpFileNamepdf.Remove(tmpFileNamepdf.LastIndexOf(".")) + i.ToString() + ".xls";
                //    }
                //}

                exOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.ExcelRecord;

                exOptions.DestinationOptions = dfdOptions;
                fail = true;               
                while (fail && i < 10)
                {
                    try
                    {
                        dfdOptions.DiskFileName = tmpFileNamexls;
                        this.rptDoc.Export();

                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileNamexls = tmpFileNamexls.Remove(tmpFileNamexls.LastIndexOf(".")) + i.ToString() + ".xls";
                    }
                }

                this.rptDoc.Close();

            }
            return tmpFileNamexls;
        }

        private string CreateExcelFile(string ExportLocation)
        {
            int i = 0;
            bool fail = true;
            string tmpFileName = ExportLocation;
            string fileName = "";
            if (_reportFileName != "")
            {
                fileName = _reportFileName;
                tmpFileName = ExportLocation + _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + ".xls";
            }
            else if (ReportName != "" && System.IO.Path.GetFileName(ExportLocation).Length == 0)
            {
                fileName = ReportName;
                tmpFileName = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + ".xls";
            }
            else if (ReportName != "")
                fileName = ReportName;

            if (fileName.ToLower().EndsWith(".rpx"))
            {
                DataDynamics.ActiveReports.Export.Pdf.PdfExport pdfExport = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
                while (fail && i < 10)
                {
                    try
                    {
                        pdfExport.Export(rpxDoc.Document, tmpFileName);
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".pdf";
                    }
                }
            }
            else if (fileName.ToLower().EndsWith(".rpt"))
            {
                CrystalDecisions.Shared.DiskFileDestinationOptions dfdOptions = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                CrystalDecisions.Shared.ExportOptions exOptions = this.rptDoc.ExportOptions;

                exOptions = this.rptDoc.ExportOptions;
                exOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                exOptions.FormatOptions = null;
                exOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;

                exOptions.DestinationOptions = dfdOptions;

                while (fail && i < 10)
                {
                    try
                    {
                        dfdOptions.DiskFileName = tmpFileName;
                        this.rptDoc.Export();
                        this.rptDoc.Close();
                        fail = false;
                    }
                    catch
                    {
                        i++;
                        tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".xls";
                    }
                }

            }
            return tmpFileName;
        }

        private string CreateHtmlFile(string ExportLocation)
        {
            int i = 0;
            bool fail = true;
            string tmpFileName = ExportLocation;
            if (_reportFileName != "")
                tmpFileName = ExportLocation + _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + ".htm";
            else if (ReportName != "" && System.IO.Path.GetFileName(ExportLocation).Length == 0)
                tmpFileName = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + ".htm";

            DataDynamics.ActiveReports.Export.Html.HtmlExport htmlExport = new DataDynamics.ActiveReports.Export.Html.HtmlExport();

            while (fail && i < 10)
            {
                try
                {
                    htmlExport.Export(rpxDoc.Document, tmpFileName);
                    fail = false;
                }
                catch
                {
                    i++;
                    tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".htm";
                }
            }

            return tmpFileName;
        }
        private string CreateExcelDataOnlyFile(string ExportLocation)
        {
            //Report Header Section Name is necessary for adding Column Titles
            string RptHeaderSectionNm = "";
            //Detail Header Section Name is necessary for adding Data Controls
            string DetailSectionNm = "";

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("GroupNum", typeof(int));
            dt.Columns.Add("XPos", typeof(decimal));
            dt.Columns.Add("YPos", typeof(decimal));
            dt.Columns.Add("Control", typeof(object));
            //Control Text contains column Title
            dt.Columns.Add("ControlText", typeof(string));
            dt.Columns.Add("NewXPos", typeof(decimal));

            //create a new instance of rpx which will be used to export to excel
            DataDynamics.ActiveReports.ActiveReport rpx = new DataDynamics.ActiveReports.ActiveReport();

            //Load the original layout
            rpx.LoadLayout(this.ReportName);

            //Copy Parameters from the previewed report to the new rpx
            foreach (DataDynamics.ActiveReports.Parameter param in rpxDoc.Parameters)
            {
                rpx.Parameters[param.Key].Value = param.Value;
                rpx.Parameters[param.Key].PromptUser = false;
            }
            //loop all sections
            int groupNum = 0;
            foreach (DataDynamics.ActiveReports.Section vSection in rpx.Sections)
            {
                switch (vSection.Type)
                {
                    case SectionType.ReportHeader:

                        //Remove all controls from Report Header. Later, Column Titles will be added.
                        while (vSection.Controls.Count > 0)
                            vSection.Controls.RemoveAt(0);

                        vSection.Visible = true;
                        RptHeaderSectionNm = vSection.Name;
                        break;

                    default:
                        vSection.Visible = false;//Only Detail Section will be Visible true later
                        if ((vSection.Type == SectionType.GroupHeader && vSection.Name.Contains("TAHeader"))
                            || vSection.Type == SectionType.PageHeader || vSection.Type == SectionType.PageFooter)//Hide TAHeader1,2, Page Header/Footer
                        {
                            //Do Nothing
                        }
                        else
                        {
                            int Count = vSection.Controls.Count;

                            if (vSection.Type == SectionType.Detail)
                                DetailSectionNm = vSection.Name;

                            groupNum++;

                            //Loop all controls in Group Headers,Detail, Group Footers,Report Footer sections     
                            //Add to DataTalbe if control Text is not empty. 
                            for (int i = 0; i < Count; i++)
                            {
                                if ((GFunc.GetStringPropertyValueOrEmpty("Text", vSection.Controls[i]) != string.Empty
                                    && vSection.Controls[i].GetType() == typeof(DataDynamics.ActiveReports.TextBox))
                                    || rpx.Script.Contains(vSection.Controls[i].Name))
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["GroupNum"] = groupNum;
                                    dr["XPos"] = vSection.Controls[i].Location.X;
                                    dr["YPos"] = vSection.Controls[i].Location.Y;
                                    dr["Control"] = vSection.Controls[i];
                                    dr["ControlText"] = GFunc.GetStringPropertyValueOrEmpty("Text", vSection.Controls[i]);
                                    dt.Rows.Add(dr);
                                }
                            }
                            //Remove control from the section.
                            //Later, all controls will be added to detail section
                            while (vSection.Controls.Count > 0)
                                vSection.Controls.RemoveAt(0);
                        }
                        break;
                }
            }
            dt.DefaultView.RowFilter = "ControlText<>''";
            dt.DefaultView.Sort = "GroupNum,YPos,XPos";


            float paperWidth = 0;
            float newXPos = 0;

            rpx.Sections[RptHeaderSectionNm].CanGrow = true;
            rpx.Sections[DetailSectionNm].CanGrow = true;
            rpx.Sections[DetailSectionNm].Visible = true;

            //Adding Data Controls to Detail Section
            foreach (DataRowView drv in dt.DefaultView)
            {
                if (drv["Control"].GetType() != typeof(DataDynamics.ActiveReports.TextBox))
                    continue;
                //Get the textbox control
                DataDynamics.ActiveReports.TextBox ctrl = (DataDynamics.ActiveReports.TextBox)drv["Control"];

                ctrl.MultiLine = true;
                ctrl.CanGrow = true;
                ctrl.CanShrink = false;
                ctrl.WordWrap = true;
                ctrl.Border.TopStyle = BorderLineStyle.None;
                ctrl.Border.BottomStyle = BorderLineStyle.None;
                ctrl.Border.LeftStyle = BorderLineStyle.None;
                ctrl.Border.RightStyle = BorderLineStyle.None;

                //Add to the detail section
                rpx.Sections[DetailSectionNm].Controls.Add(ctrl);
                ctrl.Location = new System.Drawing.PointF(newXPos, 0);

                DataDynamics.ActiveReports.TextBox ctrlTitle = new DataDynamics.ActiveReports.TextBox();
                ctrlTitle.Text = GFunc.NEStr(drv["ControlText"], "");

                ctrlTitle.Width = ctrl.Width;
                ctrlTitle.Font = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold);
                ctrlTitle.CanGrow = true;
                ctrlTitle.MultiLine = false;
                ctrlTitle.WordWrap = false;
                ctrlTitle.CanShrink = false;
                ctrlTitle.Width = ctrl.Width;

                //Adding Column Title to Report Header Section
                rpx.Sections[RptHeaderSectionNm].Controls.Add(ctrlTitle);
                ctrlTitle.Location = new System.Drawing.PointF(newXPos, 0);

                paperWidth += ctrl.Width;
                newXPos += ctrl.Width;
            }
            dt.DefaultView.RowFilter = "ControlText=''";
            foreach (DataRowView drv in dt.DefaultView)
            {
                DataDynamics.ActiveReports.ARControl ctrl = (DataDynamics.ActiveReports.ARControl)drv["Control"];
                rpx.Sections[DetailSectionNm].Controls.Add(ctrl);
                ctrl.Location = new System.Drawing.PointF(0, 0);
                ctrl.Visible = false;
            }

            rpx.PageSettings.PaperWidth = paperWidth + 4;
            rpx.PrintWidth = paperWidth + 4;
            rpx.DataSource = (System.Data.DataTable)rpxDoc.DataSource;
            rpx.AddScriptReference(System.Windows.Forms.Application.StartupPath + @"\TAReport.DLL");

            rpx.Run();

            DataDynamics.ActiveReports.Export.Xls.XlsExport xlsExport = new DataDynamics.ActiveReports.Export.Xls.XlsExport();

            string tmpFileName = ExportLocation;
            if (_reportFileName != string.Empty)
                tmpFileName += _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + ".xls";
            else if (ReportName != "" && System.IO.Path.GetFileName(ExportLocation).Length == 0)
                tmpFileName = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + ".xls";

            xlsExport.RemoveVerticalSpace = true;
            xlsExport.AutoRowHeight = true;
            xlsExport.UseCellMerging = true;

            int c = 0;
            bool fail = true;

            while (fail && c < 10)
            {
                try
                {
                    xlsExport.Export(rpx.Document, tmpFileName);
                    fail = false;
                }
                catch
                {
                    c++;
                    tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + c.ToString() + ".xls";
                }
            }

            rpx.Document.Dispose();
            rpx.Dispose();
            rpx = null;
            GC.Collect();
            return tmpFileName;
        }
        private string CreateRichTextFile(string ExportLocation)
        {
            int i = 0;
            bool fail = true;
            string tmpFileName = ExportLocation;
            DataDynamics.ActiveReports.Export.Rtf.RtfExport rtfExport = new DataDynamics.ActiveReports.Export.Rtf.RtfExport();

            if (_reportFileName != "")
                tmpFileName = ExportLocation + _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + ".rts";
            else if (ReportName != "" && System.IO.Path.GetFileName(ExportLocation).Length == 0)
                tmpFileName = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + ".rts";

            while (fail && i < 10)
            {
                try
                {
                    rtfExport.Export(rpxDoc.Document, tmpFileName);
                    fail = false;
                }
                catch
                {
                    i++;
                    tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".rts";
                }
            }

            return tmpFileName;
        }
        private string CreateCSVFile(string ExportLocation)
        {
            int i = 0;
            bool fail = true;
            string tmpFileName = ExportLocation;
            if (_reportFileName != "")
                tmpFileName = ExportLocation + _reportFileName.Remove(_reportFileName.LastIndexOf(".")) + ".csv";
            else if (ReportName != "" && System.IO.Path.GetFileName(ExportLocation).Length == 0)
                tmpFileName = ExportLocation + System.IO.Path.GetFileNameWithoutExtension(ReportName) + ".csv";

            DataDynamics.ActiveReports.Export.Text.TextExport csvExport = new DataDynamics.ActiveReports.Export.Text.TextExport();
            while (fail && i < 10)
            {
                try
                {
                    csvExport.Export(rpxDoc.Document, tmpFileName);
                    fail = false;
                }
                catch
                {
                    i++;
                    tmpFileName = tmpFileName.Remove(tmpFileName.LastIndexOf(".")) + i.ToString() + ".csv";
                }
            }

            return tmpFileName;
        }

        public string CreateReportFile(string reportFileType)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                return CreateExportFile(reportFileType, Path.GetTempPath());
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

        public string CreateReportFile(string reportFileType, string ExportLocation)
        {
            try
            {
                return CreateExportFile(reportFileType, ExportLocation);
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

        private string CreateExportFile(string reportFileType, string ExportLocation)
        {
            try
            {
                string fileName = "";


                if (GFunc.CompareString(ReportFileType.AcrobatPDFFile, reportFileType))
                {
                    fileName = CreatePdfFile(ExportLocation);
                }
                else if (GFunc.CompareString(ReportFileType.HTMLFile, reportFileType))
                {
                    fileName = CreateHtmlFile(ExportLocation);
                }
                else if (GFunc.CompareString(ReportFileType.ExcelFile, reportFileType))
                {
                    fileName = CreateExcelFile(ExportLocation);

                }
                else if (GFunc.CompareString(ReportFileType.ExcelFileDataOnly, reportFileType))
                {
                    fileName = CreateExcelDataOnlyFile(ExportLocation);
                }
                else if (GFunc.CompareString(ReportFileType.RichTextFile, reportFileType))
                {
                    fileName = CreateRichTextFile(ExportLocation);
                }
                else if (GFunc.CompareString(ReportFileType.CSVFile, reportFileType))
                {
                    fileName = CreateCSVFile(ExportLocation);
                }

                return fileName;
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool CheckPrintCondition(SqlConnection cn, int RepKey, ref System.Data.DataTable dt)
        {
            bool retValue=true;
            try
            {
               
                    if (GFunc.CompareString(GVar.RptPrintCondition.BlockBelowMinMarkup, this.RptPrintCondition))
                    {                        
                        int[] StkNsChgItmTypes ={   (int)GEnum.ItemType.Stock,
                                                    (int)GEnum.ItemType.StockB,
                                                    (int)GEnum.ItemType.Finished_GD,
                                                    (int)GEnum.ItemType.Finished_GDB,
                                                    (int)GEnum.ItemType.Serial_StockB,
                                                    (int)GEnum.ItemType.Serial_Finished_GDB,
                                                    (int)GEnum.ItemType.Assembly,
                                                    (int)GEnum.ItemType.Non_Stock,
                                                    (int)GEnum.ItemType.Service,
                                                    (int)GEnum.INTypeGrp.Charges
                                                };
                        bool hasPermission=SECPermUtility.Perform(GVar.PermissionID.Able_to_print_document_with_details_below_minimum_markup, false);
                        decimal? docMinMarkup = SysOptionUtility.GetDec(GVar.SystemOption.Posting_Option.DocumentMinMarkup);
                        bool multiPrint=true;

                        //Retrieve all the Docs with details which have below minmimum markup
                        System.Data.DataTable dtBelowMinMarkUpDocs =
                            (from row in dt.AsEnumerable()
                             where (StkNsChgItmTypes.Contains(row.Field<int>("DetINType")) && (row.Field<decimal>("DetVendorPriceRatio") < docMinMarkup))
                             group row by new
                             {
                                 DocKey = row.Field<int>("DocKey"),
                                 DocID = row.Field<string>("DocID"),
                                 DocDate = row.Field<DateTime>("DocDate")
                             }
                             into grp
                             select new
                             {
                                 DocKey = grp.Key.DocKey,
                                 DocID = grp.Key.DocID,
                                 DocDate = grp.Key.DocDate
                             }).AsDataTable();


                        if (dtBelowMinMarkUpDocs.Rows.Count > 0)
                        {
                            //Remove Docs which has below min Markup. The Printout report will not show those Docs 
                            var dtResult =
                                from drE in dt.AsEnumerable()
                                where !(from drMinMU in dtBelowMinMarkUpDocs.AsEnumerable()
                                        select drMinMU.Field<int>("DocKey"))
                                   .Contains(drE.Field<int>("DocKey"))
                                select drE;     

                            if (dtBelowMinMarkUpDocs.Rows.Count == 1 && dtResult.Count() == 0)
                                multiPrint = false;

                            if (hasPermission)
                            {
                                //show warning message for one Document printout, Multi print will show the popup form instead.
                                if (!multiPrint)
                                    if (MsgBox.Show("One or more Items in this Document are below minimum markup. Continue Printing?", GEnum.MsgBoxIcon.Question,
                                        GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No) == GEnum.MsgBoxButton.No)
                                        return false;
                            }
                            else   
                            {
                                //Replace the Data to show in Report in which docs which items do not fall below min Markup 
                                if (dtResult.Count() > 0)
                                    dt = dtResult.CopyToDataTable();
                                else
                                {
                                    System.Data.DataTable dtBelowMinMarkup;

                                    if(multiPrint)
                                        dtBelowMinMarkup =
                                           (from row in dt.AsEnumerable()
                                           where (StkNsChgItmTypes.Contains(row.Field<int>("DetINType")) && (row.Field<decimal>("DetVendorPriceRatio") < docMinMarkup))
                                           select new
                                             {
                                                 DocNum = row.Field<string>("DocID"),
                                                 SN = row.Field<decimal?>("DetSN"),
                                                 ItemID = row.Field<string>("DetKeySelectID"),
                                                 Description = row.Field<string>("DetDes"),
                                                 Price = row.Field<decimal?>("DetPriceUser"),
                                                 Markup = row.Field<decimal?>("DetVendorPriceRatio")
                                             }).AsDataTable();
                                    else
                                        dtBelowMinMarkup =
                                           (from row in dt.AsEnumerable()
                                            where (StkNsChgItmTypes.Contains(row.Field<int>("DetINType")) && (row.Field<decimal>("DetVendorPriceRatio") < docMinMarkup))
                                            select new
                                            {                                               
                                                SN = row.Field<decimal?>("DetSN"),
                                                ItemID = row.Field<string>("DetKeySelectID"),
                                                Description = row.Field<string>("DetDes"),
                                                Price = row.Field<decimal?>("DetPriceUser"),
                                                Markup = row.Field<decimal?>("DetVendorPriceRatio")
                                            }).AsDataTable();

                                    //Cannot proceed to Report preview as all docs are below min Markup.
                                    if (multiPrint)
                                        MsgBoxGrid.Show("Not allow to print the report. All Documents have items below minimum markup.", dtBelowMinMarkup,GEnum.MsgBoxIcon.ErrorInfo,GEnum.MsgBoxButton.OK);
                                    else
                                        MsgBoxGrid.Show("Not allow to print the report. One or more items are below minimum markup.", dtBelowMinMarkup, GEnum.MsgBoxIcon.ErrorInfo, GEnum.MsgBoxButton.OK);

                                    return false;
                                }
                            }
                            
                            if (multiPrint)
                            {
                                //to show the list of Docs which has below MinMarkup in the Popup form
                                frmPrintCond fCond = null;
                                if (cn == null)
                                    fCond = new frmPrintCond(RepKey, dtBelowMinMarkUpDocs, "Documents Below Minimum Markup", hasPermission);
                                else
                                    fCond = new frmPrintCond(cn, RepKey, dtBelowMinMarkUpDocs, "Documents Below Minimum Markup", hasPermission);

                                fCond.ShowDialog();
                            }
                        }                                                   
                 
                }
            }
            catch (Exception ex)
            {
                throw (ex);                
            }
            return retValue;
        }

        public void LoadReport(int repKey)
        {
            try
            {

                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    this.LoadReport(cn, repKey);
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
      
        public void LoadReport(int repKey, int uID)
        {
            try
            {

                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    this.LoadReport(cn, repKey, uID);
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

        }

        public void LoadReport(SqlConnection cn, int repKey, int uID)
        {
            try
            {
                this._cn = cn;
                rpxDoc = new DataDynamics.ActiveReports.ActiveReport();

                _repKey = repKey;

                //Get Report Data With repKey
                SYSRepListFactory objRepListFactory = new SYSRepListFactory(GEnum.InstanceMode.Normal);
                objRepListFactory.GetRptFile(uID);
                this.ReportName = objRepListFactory.SYSRepRpt.RptNm;
                SYSRep rep = SYSRep.Get(_cn, (int?)repKey);
                _recordSource = rep.RPTRecordSource1;


                _reportFileName = (GFunc.IsNE(this.ReportName) ? rep.RPTname1 : this.ReportName) + ".rpx";

                //Get report Datasource with store procedure name and parameter from Sys_repCriter
                System.Data.DataTable dtSource = GFunc.ExecuteProc(_cn, _recordSource, GetSqlParameter(_repKey));

                //Set report layout with report file name
                rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);

                //set report datasource
                if (dtSource.Rows.Count > 0)
                {
                    rpxDoc.DataSource = dtSource;
                }
                else
                {
                    throw new TAException(MsgID.Report.NoDataExistForThisReport);
                }

                rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + @"\TAReport.dll");

                //get report parameter and default value from Sys_RepPara
                GetReportParameters(repKey);
                rpxDoc.Run();

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

        public void LoadReport(SqlConnection cn, int repKey)
        {
            try
            {

                this._cn = cn;
                rpxDoc = new DataDynamics.ActiveReports.ActiveReport();

                _repKey = repKey;

                //Get Report Data With repKey
                SYSRep rep = SYSRep.Get(_cn, (int?)repKey);
                _recordSource = rep.RPTRecordSource1;


                _reportFileName = (GFunc.IsNE(this.ReportName) ? rep.RPTname1 : this.ReportName);

                //Get report Datasource with store procedure name and parameter from Sys_repCriter               
                System.Data.DataTable dtSource = GFunc.ExecuteProc(_cn, _recordSource, ReportSqlParameter == null ? GetSqlParameter(_repKey) : ReportSqlParameter);

                //Set report layout with report file name
                rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);

                //set report datasource
                if (dtSource.Rows.Count > 0)
                {
                    rpxDoc.DataSource = dtSource;
                }
                else
                {
                    throw new TAException(MsgID.Report.NoDataExistForThisReport);
                }
                //Testing, to remove later             

                rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + @"\TAReport.dll");

                //get report parameter and default value from Sys_RepPara

                GetReportParameters(repKey);
                rpxDoc.Run();

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        //public void LoadReport(int repKey, string reportFileName)
        //{
        //    try
        //    {
        //        SqlConnection cn = new SqlConnection(Database.BossDemoConnection);
        //        cn.Open();
        //        this._cn = cn;             

        //        _repKey = repKey;

        //        //Get Report Data With repKey
        //        SYSRep rep = SYSRep.Get(_cn, (int?)repKey);
        //        _recordSource = rep.RPTRecordSource1;

        //        _reportFileName = (GFunc.IsNE(this.ReportName) ? rep.RPTname1 : this.ReportName);

        //        //Get report Datasource with store procedure name and parameter from Sys_repCriter               
        //        System.Data.DataTable dtSource = GFunc.ExecuteProc(_cn, _recordSource, ReportSqlParameter == null ? GetSqlParameter(_repKey) : ReportSqlParameter);

        //        if (_reportFileName.ToUpper().EndsWith(".RPX"))
        //        {
        //            rpxDoc = new DataDynamics.ActiveReports.ActiveReport();
        //            //Set report layout with report file name
        //            rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\Reports\\" + _reportFileName);//Orignal

        //            //MovingControlsInRpx();
        //            if (dtSource != null && dtSource.Rows.Count > 0)
        //            {
        //                rpxDoc.DataSource = _recordSource;
        //            }
        //            else
        //            {
        //                throw new TAException(MsgID.Report.NoDataExistForThisReport);
        //            }

        //            rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + "\\TAReport.dll");

        //            //if (GFunc.NEInt(RepKey, 0) > 0)
        //            //{
        //            //    GetReportParameters(RepKey);
        //            //}

        //            //if (GFunc.IsNE(ReportParameter) == false)
        //            //{
        //            //    foreach (ReportParameter item in ReportParameter)
        //            //    {
        //            //        try
        //            //        {
        //            //            rpxDoc.Parameters[item.ParameterName].PromptUser = false;
        //            //            rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
        //            //            rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
        //            //        }
        //            //        catch (Exception ex)
        //            //        {
        //            //            break;
        //            //        }
        //            //    }
        //            //}
        //            rpxDoc.Run();
        //        }
        //        else if (_reportFileName.ToUpper().EndsWith(".RPT"))
        //        {
        //            rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        //            rptDoc.Load(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);
        //            rptDoc.SetDataSource(dtSource);

        //            //foreach (ReportParameter p in ReportParameter)
        //            //{
        //            //    rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
        //            //}
                    
        //        }

        //    }
        //    catch (TAException tex)
        //    {
        //        throw tex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Error(ex, false);
        //    }
        //}

        public void LoadReportWCondition(int repKey, bool printMulti)
        {
            try
            {

                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    cn.Open();
                    this.LoadReportWCondition(cn, repKey, printMulti);
                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }
        public void LoadReportWCondition(SqlConnection cn, int repKey, bool printMulti)
        {
            try
            {

                this._cn = cn;
                rpxDoc = new DataDynamics.ActiveReports.ActiveReport();

                _repKey = repKey;               

                //Get Report Data With repKey
                SYSRep rep = SYSRep.Get(_cn, (int?)repKey);
                _reportFileName = (GFunc.IsNE(this.ReportName) ? rep.RPTname1 : this.ReportName);

                SYSRepRpt rpt = SYSRepRpt.Get(RepKey, _reportFileName, 3);

                _recordSource = GFunc.IsNE(rpt.RptAltRecordSource) ? rep.RPTRecordSource1 : rpt.RptAltRecordSource;
             

                //Get report Datasource with store procedure name and parameter from Sys_repCriter               
                System.Data.DataTable dtSource = GFunc.ExecuteProc(_cn, _recordSource, ReportSqlParameter == null ? GetSqlParameter(_repKey) : ReportSqlParameter);

                //Add QRCode column and data into the datasource //commented by May. QR Code is not used in rpx
                //if (SysOptionUtility.HasDMASLink && !GFunc.IsNE(custom1))                
                //    dtSource = GetSourceWithQRCode(dtSource, Path.GetExtension(_reportFileName));

                if (CheckPrintCondition(cn, repKey, ref dtSource) == false)
                    throw new TAUtil.TAException("There is a condition which does not allow to print this report.", "PrintCondition");

                //Set report layout with report file name
                rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);

                //set report datasource
                if (dtSource.Rows.Count > 0)
                {
                    rpxDoc.DataSource = dtSource;
                }
                else
                {
                    throw new TAException(MsgID.Report.NoDataExistForThisReport);
                }
                //Testing, to remove later
                rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + @"\TAReport.dll");

                //get report parameter and default value from Sys_RepPara
                GetReportParameters(repKey);
               // rpxDoc.Run(); commented by Jane on 31-Mar2014. RpxViewer aldy called Run() function. 

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }

        public void LoadReport(System.Data.DataTable dtSource, string reportFileName,DataDynamics.ActiveReports.Parameter[] additionalParams)
        {
            try
            {

                rpxDoc = new DataDynamics.ActiveReports.ActiveReport();

                _reportFileName = reportFileName;
                //Set report layout with report file name
                rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);//Orignal

                //MovingControlsInRpx();
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    rpxDoc.DataSource = dtSource;
                }
                else
                {
                    throw new TAException(MsgID.Report.NoDataExistForThisReport);
                }

                rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + @"\TAReport.dll");

                if (GFunc.NEInt(RepKey, 0) > 0)
                {
                    GetReportParameters(RepKey);
                }
                if (additionalParams != null)
                {
                    foreach (DataDynamics.ActiveReports.Parameter p in additionalParams)
                        rpxDoc.Parameters[p.Key].Value = p.Value;
                }
                if (GFunc.IsNE(ReportParameter) == false)
                {
                    foreach (ReportParameter item in ReportParameter)
                    {
                        try
                        {
                            rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                            rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                            rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
                rpxDoc.Run();
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

        public void LoadReport(System.Data.DataTable dtSource, string reportFileName)
        {
            try
            {
                _repKey = RepKey;
                _reportFileName = reportFileName;

                if (_reportFileName.ToUpper().EndsWith(".RPX"))
                {
                    rpxDoc = new DataDynamics.ActiveReports.ActiveReport();
                    //Set report layout with report file name
                    rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\Reports\\" + _reportFileName);//Orignal

                    //MovingControlsInRpx();
                    if (dtSource != null && dtSource.Rows.Count > 0)
                    {
                        rpxDoc.DataSource = dtSource;
                    }
                    else
                    {
                        throw new TAException(MsgID.Report.NoDataExistForThisReport);
                    }

                    rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + "\\TAReport.dll");

                    if (GFunc.NEInt(RepKey, 0) > 0)
                    {
                        GetReportParameters(RepKey);
                    }

                    if (GFunc.IsNE(ReportParameter) == false)
                    {
                        foreach (ReportParameter item in ReportParameter)
                        {
                            try
                            {
                                rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                                rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                                rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }
                    }
                    //rpxDoc.Run(); temporarily commented by Jane on 27-Jan-2014. GST Form5 Report show always 0. CHECK!!!
                }
                else if (_reportFileName.ToUpper().EndsWith(".RPT"))
                {
                    rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rptDoc.Load(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);
                    rptDoc.SetDataSource(dtSource);

                    foreach (ReportParameter p in ReportParameter)
                    {
                        rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                    }
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
        }

        public void LoadReport(System.Data.DataTable dtSource, string reportFileName, bool requireParam)
        {
            try
            {

                rpxDoc = new DataDynamics.ActiveReports.ActiveReport();
                _repKey = RepKey;
                _reportFileName = reportFileName;
                //Set report layout with report file name
                rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);//Orignal

                //MovingControlsInRpx();
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    rpxDoc.DataSource = dtSource;
                }
                else
                {
                    throw new TAException(MsgID.Report.NoDataExistForThisReport);
                }

                rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + @"\TAReport.dll");

                if (requireParam && GFunc.NEInt(RepKey, 0) > 0)
                {
                    GetReportParameters(RepKey);
                }

                if (GFunc.IsNE(ReportParameter) == false)
                {
                    foreach (ReportParameter item in ReportParameter)
                    {
                        try
                        {
                            rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                            rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                            rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
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
        }

        public void LoadReport(DataSet dtSource, string reportFileName) //for Sub Report
        {
            try
            {

                rpxDoc = new DataDynamics.ActiveReports.ActiveReport();

                _reportFileName = reportFileName;
                //Set report layout with report file name
                rpxDoc.LoadLayout(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName);//Orignal

                if (dtSource != null && dtSource.Tables[0].Rows.Count > 0)
                {
                    rpxDoc.DataSource = dtSource;
                    rpxDoc.DataMember = "Header";
                }
                else
                {
                    throw new TAException(MsgID.Report.NoDataExistForThisReport);
                }

                rpxDoc.AddScriptReference(System.Windows.Forms.Application.StartupPath + @"\TAReport.dll");

                if (GFunc.NEInt(RepKey, 0) > 0)
                {
                    GetReportParameters(RepKey);
                }

                if (GFunc.IsNE(ReportParameter) == false)
                {
                    foreach (ReportParameter item in ReportParameter)
                    {
                        try
                        {
                            rpxDoc.Parameters[item.ParameterName].PromptUser = false;
                            rpxDoc.Parameters[item.ParameterName].Value = item.ParameterValue.ToString();
                            rpxDoc.Parameters[item.ParameterName].DefaultValue = item.ParameterValue.ToString();
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
                rpxDoc.Run();
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

        public void LoadCrystalReport(System.Data.DataTable dtSource, string reportFileName)
        {
            try
            {
                _reportFileName = reportFileName;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rpxDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rpxDoc.Load(System.Windows.Forms.Application.StartupPath + @"\Reports\" + reportFileName);
                rpxDoc.SetDataSource(dtSource);

                List<ReportParameter> rptParams = this.ReportParameter;
                foreach (ReportParameter p in rptParams)
                {
                    rpxDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                }

                frmReportViewer fRptViewer = new frmReportViewer();
                fRptViewer.RepKey = RepKey;
                fRptViewer.RptName = reportFileName;
                fRptViewer.RptDocument = rpxDoc;
                fRptViewer.MdiParent = frmMain.gfrmMain;
                fRptViewer.Show();
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
        public void LoadCrystalReport(DataSet dsSource, string reportFileName, string subReportName)
        {
            try
            {
                _reportFileName = reportFileName;
                this.rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDoc.Load(System.Windows.Forms.Application.StartupPath + @"\Reports\" + reportFileName);
                rptDoc.SetDataSource(dsSource.Tables[0]);

                //Set Sup report datasource
                if (dsSource.Tables.Count > 1)
                {
                    rptDoc.OpenSubreport(subReportName).SetDataSource(dsSource.Tables[1]);
                }

                List<ReportParameter> rptParams = this.ReportParameter;

                foreach (ReportParameter p in rptParams)
                {
                    if (p.ParameterName != "pSubRepPath")
                    {
                        rptDoc.SetParameterValue(p.ParameterName, p.ParameterValue);
                    }
                }
                //frmReportViewer fRptViewer = new frmReportViewer();
                //fRptViewer.RptName = reportFileName;
                //fRptViewer.RptDocument = rptDoc;
                
                //fRptViewer.MdiParent = frmMain.gfrmMain;
                //fRptViewer.Show();
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

        //For Agent
        public void MakeReport(TASToDoFactory factoryToDo, string userName, string toEmails)
        {
            try
            {

                #region Prepare SQL & Report Parameter based on the ToDoCriteria Table
                ReportSqlParameter = new List<SqlParameter>();
                foreach (DataRow row in factoryToDo.ObjTASToDoDetCriterias.Rows)
                {
                    SqlParameter param = null;
                    if (GFunc.CompareString(row["CriteriaDataType"].ToString().ToUpper(), "String".ToUpper())) //Boolean DataType? where should
                    {
                        if (row["CriteriaValueChar"] != DBNull.Value)
                            param = new SqlParameter("@" + row["CriteriaName"].ToString(), row["CriteriaValueChar"]);
                    }
                    else if (GFunc.CompareString(row["CriteriaDataType"].ToString().ToUpper(), "Integer".ToUpper()) || GFunc.CompareString(row["CriteriaDataType"].ToString().ToUpper(), "IntegerPeriod".ToUpper()))
                    {
                        if (row["CriteriaValueInt"] != DBNull.Value)
                            param = new SqlParameter("@" + row["CriteriaName"].ToString(), row["CriteriaValueInt"]);
                    }
                    else if (GFunc.CompareString(row["CriteriaDataType"].ToString().ToUpper(), "Date".ToUpper()))
                    {
                        if (row["CriteriaValueDate"] != DBNull.Value)
                            param = new SqlParameter("@" + row["CriteriaName"].ToString(), row["CriteriaValueDate"]);
                    }
                    else if (GFunc.CompareString(row["CriteriaDataType"].ToString().ToUpper(), "Money".ToUpper()))
                    {
                        if (row["CriteriaValueMoney"] != DBNull.Value)
                            param = new SqlParameter("@" + row["CriteriaName"].ToString(), row["CriteriaValueMoney"]);
                    }
                    if (param != null)
                        ReportSqlParameter.Add(param);

                }
                ReportFactory.AddAdditionalParameters(ReportSqlParameter, factoryToDo.ObjTASToDo.RepKey.Value, null);
                #endregion

                #region Prepare Range Param
                string RepRange = string.Empty;
                int rowCount = factoryToDo.ObjTASToDoDetCriterias.Rows.Count;
                int Period = 0;
                for (int i = 0; i < rowCount; i++)
                {
                    //For From,To, SearchType
                    if (factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaName"].ToString().Contains("From") && (i + 2) < rowCount) //Third Row exist but need to check whether it is Search Type
                    {
                        if (factoryToDo.ObjTASToDoDetCriterias.Rows[i + 2]["CriteriaName"].ToString().Contains("SearchType"))
                        {
                            //if SearchType is 10=>Range 
                            if (GFunc.NEInt(factoryToDo.ObjTASToDoDetCriterias.Rows[i + 2]["CriteriaValueChar"], 0) == (int)GEnum.ReportSearchOption.Range)
                            {
                                //This is (from - to)
                                if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "String".ToUpper()))
                                {
                                    if (factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"].ToString() != string.Empty && factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueChar"].ToString() != string.Empty)
                                    {
                                        RepRange += factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "BETWEEN \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"] + "\" AND \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueChar"] + "\",";
                                    }
                                    else
                                    {
                                        RepRange += "FOR ALL " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "");
                                    }
                                }
                                else if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "Integer".ToUpper()))
                                {
                                    if (factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueInt"].ToString() != "0" && factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueInt"].ToString() != "0")
                                    {
                                        RepRange += factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "BETWEEN \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"] + "\" AND \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueChar"] + "\",";
                                    }
                                    else
                                    {
                                        RepRange += "FOR ALL " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "");
                                    }
                                }
                            }
                            //if SearchType is 20 =>WildCard
                            else
                            {
                                //This is (Something=from) ;eg=>CustomerFrom='%5K%'
                                if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "String".ToUpper()))
                                {
                                    if (!GFunc.IsNE(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"]))
                                        RepRange += " : " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "=" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"] + ",";
                                }
                                else if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "Integer".ToUpper()))
                                {
                                    RepRange += " : " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueInt"] + ",";
                                }
                            }
                            i = i + 2;//Increase Index, bcos this action takes the value of from,To & SearchType too
                        }
                        else
                        {
                            //This is (from - to)
                            if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "String".ToUpper()))
                            {
                                if (factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"].ToString() != string.Empty && factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueChar"].ToString() != string.Empty)
                                {
                                    RepRange += factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "BETWEEN \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"] + "\" AND \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueChar"] + "\",";
                                }
                                else
                                {
                                    RepRange += "FOR ALL " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "; ";
                                }
                            }
                            else if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "Integer".ToUpper()))
                            {
                                if (factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueInt"].ToString() != "0" && factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueInt"].ToString() != "0")
                                {
                                    RepRange += factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "BETWEEN \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"] + "\" AND \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueChar"] + "\",";
                                }
                                else
                                {
                                    RepRange += "FOR ALL " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "; ";
                                }
                            }
                            else if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "Date".ToUpper()))
                            {
                                DateTime fromDate = new DateTime();
                                DateTime toDate = new DateTime();
                                DateTime.TryParse(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueDate"].ToString(),out fromDate);
                                DateTime.TryParse(factoryToDo.ObjTASToDoDetCriterias.Rows[i + 1]["CriteriaValueDate"].ToString(), out toDate);
                                RepRange += factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString().Replace(" From", "") + "\"" + fromDate.ToString("dd MMM yyyy") + "\" AND \"" + toDate.ToString("dd MMM yyyy") + "\",";
                            }
                            i = i + 1;//Increase Index, bcos this action takes the value of from & To too
                        }
                    }
                    // For Only One Option
                    else if (factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaName"].ToString().ToLower()!="additionalfilter")
                    {
                        if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "String".ToUpper()))
                        {
                            if (factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"].ToString() != string.Empty)
                            {
                                RepRange += factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString() + "= \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"] + "\",";
                            }
                            else
                            {
                                RepRange += "FOR ALL " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString() + "; ";
                            }
                        }
                        else if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), "Integer".ToUpper()))
                        {
                            if (GFunc.NEStr(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueInt"], "") != string.Empty)
                            {
                                RepRange += factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString();
                                if (GFunc.IsNE(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"]))
                                    RepRange += "= \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueInt"] + "\",";
                                else
                                    RepRange += "= \"" + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueChar"] + "\",";
                            }
                            else
                            {
                                RepRange += "FOR ALL " + factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaLabel"].ToString() + "; ";
                            }
                        }
                        else if (GFunc.CompareString(factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaDataType"].ToString().ToUpper(), GVar.ReportCriteriaDataType.IntegerPeriod.ToString().ToUpper()))
                        {
                            Period = (int)factoryToDo.ObjTASToDoDetCriterias.Rows[i]["CriteriaValueInt"];
                        }
                    }
                }

                #endregion

                ReportParameter = new List<ReportParameter>();
                string opCmpValue = SysOptionUtility.GetStr("CompanyName");
                SYSRepParas ReportParameters = SYSRepParas.Get((int)factoryToDo.ObjTASToDo.RepKey);
                foreach (DataRow obj in ReportParameters.Rows)
                {
                    ReportParameter repParam = new ReportParameter(obj["ParName"].ToString(), false);
                    switch (obj["ParName"].ToString().ToLower())
                    {
                        case "pcmpname":
                            repParam.ParameterValue = opCmpValue;
                            break;
                        case "preprange":
                            repParam.ParameterValue = RepRange;
                            break;
                        case "preportdate":
                            int Year = GFunc.NEInt(Period.ToString().Substring(0, 4), 0);
                            int Month = GFunc.NEInt(Period.ToString().Substring(4, 2), 0);
                            int TotalDaysInMonth = DateTime.DaysInMonth(Year, Month);
                            repParam.ParameterValue = String.Format("{0:MM-dd-yyyy}", TotalDaysInMonth + "/" + Month + "/" + Year); ;
                            break;
                        default:
                            //fViewr.rpxDoc.Parameters[obj.ParName].PromptUser = true;
                            repParam.ParameterValue = obj["Custom1"].ToString();
                            break;
                    }
                    ReportParameter.Add(repParam);
                }

                //Get Report DatatTable
                SYSRep rep = SYSRep.Get((int)factoryToDo.ObjTASToDo.RepKey);

                System.Data.DataTable dt = GFunc.ExecuteProc(rep.RPTRecordSource1, ReportSqlParameter);

                //Make Report
                LoadReport(dt, factoryToDo.ObjTASToDo.RepFileNm);

                string emailBody = "<html><body><div>" +
                           "<p style='padding-left:20px;align:justify'>Dear " + userName + ",</p> " +
                           "<p style='padding-top:5px;padding-left:10px;color:#0349A0;width:100%;text-decoration:underline'>" +
                           "<p style='padding-left:20px;align:justify'>The attached is \"" + factoryToDo.ObjTASToDo.ToDoDes + "\" report automatically generated from BOSS System" +                       
                           ".</p></div></body></html>" +
                           "<p style='padding-top:10px;padding-left:20px;align:justify'> Thanks & Regards,</p> " +
                           "<p style='padding-left:20px;align:justify'>BHG Support Team</p> ";

                string attachTempFile = CreatePdfNExcelFile(@"C:\Temp\");
                System.Windows.Forms.Application.DoEvents();

                UpdateExcel(attachTempFile, RepRange, (int)factoryToDo.ObjTASToDo.RepKey);
               // attachTempFile = Path.GetTempPath() + attachTempFile;
                System.Net.Mail.Attachment[] Attach = { new System.Net.Mail.Attachment(attachTempFile) };
                GEmail.SendEmailwithAttachments(toEmails, factoryToDo.ObjTASToDo.ToDoDes, emailBody, Attach);

                //SendEmail(toEmails, factoryToDo.ObjTASToDo.ToDoDes + " Report", emailBody, ReportFileType.AcrobatPDFFile);
            }
            catch (TAException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void UpdateExcel(string workbookname, string pRepRange,int RepKey)
        {
            Microsoft.Office.Interop.Excel.Application oXL = null;
            Microsoft.Office.Interop.Excel._Workbook oWB = null;
            Microsoft.Office.Interop.Excel._Worksheet oSheet = null;

            string title="";
            if(RepKey==14601)
                title = "QUOTATION TRACKING";
            else if (RepKey == 14602)
                title = "KEY CUSTOMER SALES";
            try
            {
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oWB = oXL.Workbooks.Open(workbookname);
               
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Worksheets[1];

                oSheet.Rows[1].Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                oSheet.Rows[1].Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                oSheet.Rows[1].Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                oSheet.Cells[1, 1] =title;
                oSheet.Cells[2, 1] = pRepRange;

                oWB.Save();
            }
            catch (Exception ex)
            {
                FileStream fs = new FileStream(@"C:\Temp\TAAgentServiceErrorLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_streamWriter.WriteLine("Error:" + ex.Message + " - " + ex.StackTrace);
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            finally
            {
                if (oWB != null)
                    oWB.Close();
            }
        }

        public bool SendEmail(string emails, string subject, string messageBody, string filetype)
        {
            try
            {
                string attachTempFile = CreatePdfNExcelFile(Path.GetTempPath());
              
                attachTempFile = Path.GetTempPath() + attachTempFile;
                System.Net.Mail.Attachment[] Attach = { new System.Net.Mail.Attachment(attachTempFile + ".pdf"), 
                                                          new System.Net.Mail.Attachment(attachTempFile + ".xls")
                                                      ,new System.Net.Mail.Attachment(attachTempFile + "_NoFormat.xls") };
                GEmail.SendEmailwithAttachments(emails, subject, messageBody, Attach);
               

                return true;
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
        public bool EmailDisplayinOutlook(string emails, string subject, string messageBody, string fileName)
        {
            try
            {
                string attachTempFile = CreatePdfFile(Path.GetTempPath(), GFunc.ValidateFileName(fileName)); /* modified by YST on 2023/07/25 to settle email error according to feedback by Joan from procurement team  */

                frmMain.gfrmMain.SetNotifyStatus("Sending email ...............");
                OutlookEmail.SendEmail(emails, subject, messageBody, attachTempFile, "", true, true);
                frmMain.gfrmMain.SetNormalStaus("Ready");

                return true;
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

        public bool PrintPreview()
        {
            try
            {
                if (rpxDoc != null)
                {
                    reportViewerForm = new frmRpxViewer();
                    reportViewerForm.rptDoc = rpxDoc;
                    reportViewerForm.reportLoder = this;
                    reportViewerForm.arvMain.Document = reportViewerForm.rptDoc.Document;
                    reportViewerForm.arvMain.Tag = _repKey;
                    reportViewerForm.MdiParent = frmMain.gfrmMain;
                    reportViewerForm.ListUpdateEvent += new GVar.ListUpdateEvent(AfterRecordUpdated);
                    reportViewerForm.ReportSourceFilePath = System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName;
                    reportViewerForm.Show();
                    return true;
                }
                else if(rptDoc!=null)
                {
                    frmReportViewer f = new frmReportViewer();
                    f.RepKey = RepKey;
                    f.RptDocument = rptDoc;
                    f.reportLoader = this;
                    f.MdiParent = frmMain.gfrmMain;
                    f.RptName = _reportFileName;
                    f.Show();
                }
                return false;
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

        public bool SendFax(string faxNo, string docName)
        {

            try
            {
                //FAXCOMLib.FaxServerClass fsc = new FAXCOMLib.FaxServerClass();
                //fsc.Connect(Environment.MachineName);

                //object obj = fsc.CreateDocument(CreatePdfFile(Path.GetTempPath()));
                //FAXCOMLib.FaxDoc fd = (FAXCOMLib.FaxDoc)obj;
                //fd.FaxNumber = faxNo;
                ////fd.RecipientName = receipient;
                //int i = fd.Send();

                if (_reportFileName.ToLower().EndsWith("rpx"))
                {
                    rpxDoc = LoadRpxPageAndPrinterSetting(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName, rpxDoc);
                    rpxDoc.Document.Printer.PrinterSettings.Copies = 1;
                    rpxDoc.Document.Printer.PrinterName = "ActiveFax";
                    rpxDoc.Document.Name = docName + "@F211 " + faxNo + "@"; //to set fax number for ActiveFax server
                    rpxDoc.Document.Print(false, false);
                }
                else
                {
                    string fileName = System.IO.Path.GetTempPath() + docName.Replace("/", "")+ "@F211 " +faxNo + "@.rpt";
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    rptDoc.SaveAs(fileName);
                    rptDoc.PrintToPrinter(1, false, 0, 0);
                }
                return true;                
            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DocPrintStatus_Set(int DocCodeKey, string DocKeyXML)
        {
            try
            {
                string l_TableXml = DocKeyXML;

                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        using (SqlCommand cm = cn.CreateCommand())
                        {
                            cm.CommandType = CommandType.StoredProcedure;
                            cm.CommandText = "Doc_ChangePrintStatus";
                            cm.Parameters.AddWithValue("@Option", 0);
                            cm.Parameters.AddWithValue("@DocCodeKey", DocCodeKey);
                            cm.Parameters.AddWithValue("@DocState", (int)GEnum.DocState.Posted);
                            cm.Parameters.AddWithValue("@xmlDocInformation_xml", l_TableXml);
                            cm.Parameters.AddWithValue("@RetValue", 0);
                            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                            cm.ExecuteNonQuery();

                            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                            {
                                if (System.Transactions.Transaction.Current.TransactionInformation.Status != System.Transactions.TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }// Already close and dispose sql connection.
                    }
                }
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
        public bool DocPrintStatus_Set(int DocCodeKey, int DocKey)
        {
            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        using (SqlCommand cm = cn.CreateCommand())
                        {
                            cm.CommandType = CommandType.StoredProcedure;
                            cm.CommandText = "Doc_ChangePrintStatus";

                            cm.Parameters.AddWithValue("@Option", 1);
                            cm.Parameters.AddWithValue("@DocState", (int)GEnum.DocState.Posted);
                            cm.Parameters.AddWithValue("@DocCodeKey", DocCodeKey);
                            cm.Parameters.AddWithValue("@DocKey", DocKey);
                            cm.Parameters.AddWithValue("@RetValue", 0);
                            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                            cm.ExecuteNonQuery();

                            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                            {
                                if (System.Transactions.Transaction.Current.TransactionInformation.Status != System.Transactions.TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }// Already close and dispose sql connection.
                    }
                }
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
        public bool PrintFile(string reportFileType)
        {
            if (rpxDoc != null)
            {
                System.Diagnostics.Process.Start(CreateReportFile(reportFileType));
                return true;
            }
            return false;
        }

        public bool Print()
        {
            try
            {
                if (rpxDoc != null)
                {
                    rpxDoc = LoadRpxPageAndPrinterSetting(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName, rpxDoc);
                    rpxDoc.Document.Printer.PrinterSettings.Copies = 1;

                    rpxDoc.Document.Name = "xxxxxxxDOC@F211 65070790@";
                    rpxDoc.Document.Print();
                   
                }
                else if (rptDoc != null)
                {
                    PrinterSettings printerSettings = new PrinterSettings();
                    PrintDialog printDialog = new PrintDialog();
                    printDialog.PrinterSettings = printerSettings;
                    printDialog.AllowPrintToFile = false;
                    printDialog.AllowSomePages = true;
                    printDialog.UseEXDialog = true;

                    DialogResult result = printDialog.ShowDialog();

                    if (result == DialogResult.Cancel)
                    {
                        return false;
                    }

                    rptDoc.PrintOptions.PrinterName = printerSettings.PrinterName;
                    rptDoc.PrintToPrinter(printerSettings.Copies, false, 0, 0);
                   
                }


                return true;
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

        public bool Print(short NumberOfCopy)
        {
            try
            {
                rpxDoc = LoadRpxPageAndPrinterSetting(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName, rpxDoc);
                rpxDoc.Document.Printer.PrinterSettings.Copies = NumberOfCopy;
                rpxDoc.Document.Print();

                return true;
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

        public bool Print(bool ShowPrintDialog)
        {
            try
            {                
                rpxDoc = LoadRpxPageAndPrinterSetting(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName, rpxDoc);
                rpxDoc.Document.Printer.PrinterSettings.Copies = 1;
                rpxDoc.Document.Print(ShowPrintDialog, false, false);                
                return true;
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

        public bool Print(bool ShowPrintDialog, bool ShowPrintProgress)
        {
            try
            {
                if (rpxDoc != null)
                {
                    rpxDoc = LoadRpxPageAndPrinterSetting(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName, rpxDoc);
                    rpxDoc.Document.Printer.PrinterSettings.Copies = 1;
                    rpxDoc.Document.Print(ShowPrintDialog, ShowPrintProgress, false);
                }
                else if (rptDoc != null)
                {
                    if (ShowPrintDialog)
                    {
                        PrinterSettings printerSettings = new PrinterSettings();
                        PrintDialog printDialog = new PrintDialog();
                        printDialog.PrinterSettings = printerSettings;
                        printDialog.AllowPrintToFile = false;
                        printDialog.AllowSomePages = true;
                        printDialog.UseEXDialog = true;

                        DialogResult result = printDialog.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            return false;
                        }
                        rptDoc.PrintOptions.PrinterName = printerSettings.PrinterName;
                       
                        rptDoc.PrintToPrinter(1, false, 0, 0);
                    }
                }
                return true;
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

        public bool Print(short NumberOfCopy, bool ShowPrintDialog)
        {
            try
            {
                if (rpxDoc != null)
                {
                    rpxDoc = LoadRpxPageAndPrinterSetting(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName, rpxDoc);
                    rpxDoc.Document.Printer.PrinterSettings.Copies = NumberOfCopy;
                    rpxDoc.Document.Print(ShowPrintDialog, false, false);
                }
                else if (rptDoc != null)
                {
                    if (ShowPrintDialog)
                    {
                        PrinterSettings printerSettings = new PrinterSettings();
                        PrintDialog printDialog = new PrintDialog();
                        printDialog.PrinterSettings = printerSettings;
                        printDialog.AllowPrintToFile = false;
                        printDialog.AllowSomePages = true;
                        printDialog.UseEXDialog = true;

                        DialogResult result = printDialog.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            return false;
                        }
                        rptDoc.PrintOptions.PrinterName = printerSettings.PrinterName;
                    }
                    rptDoc.PrintToPrinter(NumberOfCopy, false, 0, 0);
                }
                return true;
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

        public bool Print(short NumberOfCopy, bool ShowPrintDialog, bool ShowPrintProgress)
        {
            try
            {                
                if (rpxDoc != null)
                {
                    rpxDoc.Run();
                    rpxDoc = LoadRpxPageAndPrinterSetting(System.Windows.Forms.Application.StartupPath + @"\Reports\" + _reportFileName, rpxDoc);
                    rpxDoc.Document.Printer.PrinterSettings.Copies = NumberOfCopy;
                    rpxDoc.Document.Print(ShowPrintDialog, ShowPrintProgress, false);
                }
                else if (rptDoc != null)
                {
                  rptDoc.PrintToPrinter(NumberOfCopy,true, 0, 0);                
                }
               
                return true;
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

        public bool PrintMulti(string MultiTitle, CrystalDecisions.CrystalReports.Engine.ReportDocument PrptDoc, short NoCopies, string MainReportTitle) //Customized for BengHui -- For Direct Print
        {

            string Title = string.Empty;
            string newTitle = string.Empty;
            try
            {
                object[] TitleArray = MultiTitle.Split(",".ToCharArray());
                rptDoc = PrptDoc;


                for (int i = 0; i < TitleArray.Count(); i++) // Loop Report Title
                {
                    //if there is nothing in between 2 commas like ("Title1","Title2",,"Title3") then use "Title2" for blank comma.
                    //If three is white space in between 2 commas like ("Title1","Title2", ,"Title3") then use as it is.
                    if (TitleArray[i] != "")
                    {
                        Title = GFunc.NEStr(TitleArray[i].ToString(), "");
                    }
                    foreach (ReportParameter p in ReportParameter)
                    {
                        if (p.ParameterName == "pRepTitle")
                        {
                            newTitle = MainReportTitle + "-" + Title;
                            rptDoc.SetParameterValue(p.ParameterName, newTitle);
                        }
                    }
                    //Print
                    rptDoc.PrintToPrinter(NoCopies, true, 0, 0);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        public bool PrintMulti(string MultiTitle, CrystalDecisions.CrystalReports.Engine.ReportDocument PrptDoc, short NoCopies,
            bool Collate, int FromPage, int ToPage, string MainReportTitle, string docKeyXMl) //Customized for BengHui -- For Print from Perview
        {
            string Title = string.Empty;           
            string newTitle = string.Empty;
            try
            {
                object[] TitleArray = MultiTitle.Split(",".ToCharArray());
                rptDoc = PrptDoc;

                
                for (int i = 0; i < TitleArray.Count(); i++) // Loop Report Title
                {                    
                    //if there is nothing in between 2 commas like ("Title1","Title2",,"Title3") then use "Title2" for blank comma.
                    //If three is white space in between 2 commas like ("Title1","Title2", ,"Title3") then use as it is.
                    if (TitleArray[i] != "")
                    {
                        Title = GFunc.NEStr(TitleArray[i].ToString(), "");
                    }                    
                    foreach (ReportParameter p in ReportParameter)
                    {
                        if (p.ParameterName == "pRepTitle")
                        {
                            newTitle = MainReportTitle + "-" + Title;
                            rptDoc.SetParameterValue(p.ParameterName,newTitle);
                        }                       
                    }
                   
                    //Print
                    rptDoc.PrintToPrinter(NoCopies, Collate, FromPage, ToPage);

                }
               
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        
        
        public static DataDynamics.ActiveReports.ActiveReport LoadRpxPageAndPrinterSetting(string RPXReportFilePath, DataDynamics.ActiveReports.ActiveReport RptDoc)
        {
            try
            {
                DataDynamics.ActiveReports.Design.Designer Desin = new DataDynamics.ActiveReports.Design.Designer();
                //Desin.DesignUnits = DataDynamics.ActiveReports.Design.MeasurementUnits.US;
                Desin.LoadReport(RPXReportFilePath);

                DataDynamics.ActiveReports.ActiveReport l_rpxDoc = RptDoc;
                l_rpxDoc.LoadLayout(RPXReportFilePath);

                IEnumerator ie = l_rpxDoc.Parameters.GetEnumerator();
                PrintDialog PG = new PrintDialog();
                string PrinterName = "";

                while (ie.MoveNext())
                {
                    if (GFunc.CompareString(ie.Current.ToString(), "PrinterName"))
                    {
                        PrinterName = ((DataDynamics.ActiveReports.Parameter)ie.Current).DefaultValue.ToString();
                        PG.PrinterSettings.PrinterName = PrinterName;
                        l_rpxDoc.Document.Printer.PrinterName = PrinterName;
                    }

                    if (GFunc.CompareString(ie.Current.ToString(), "PaperSize"))
                    { 
                        string[] strs = l_rpxDoc.Parameters["PaperSize"].DefaultValue.ToString().Split(',');

                        if (strs.Length == 3)
                        {
                            foreach (System.Drawing.Printing.PaperSize item in l_rpxDoc.Document.Printer.PrinterSettings.PaperSizes)
                            {
                                if ((int)item.Kind == Convert.ToInt32(strs[0]))
                                {
                                    if (item.PaperName == strs[2])
                                    {
                                        l_rpxDoc.Document.Printer.DefaultPageSettings.PaperSize = item;
                                        l_rpxDoc.PageSettings.PaperKind = item.Kind;
                                        l_rpxDoc.PageSettings.PaperName = item.PaperName;
                                        break;
                                    }
                                }
                            }
                        }

                        // PG.PrinterSettings.PrinterName = PrinterName;
                        //IEnumerator iPSize = PG.PrinterSettings.PaperSizes.GetEnumerator();
                        //while (iPSize.MoveNext())
                        //{
                        //    if (((System.Drawing.Printing.PaperSize)iPSize.Current).PaperName == Desin.Report.Parameters["PaperSize"].DefaultValue)
                        //    {
                        //        l_rpxDoc.Document.Printer.DefaultPageSettings.PaperSize = (System.Drawing.Printing.PaperSize)iPSize.Current;
                        //        break;
                        //    }
                        //}
                    }

                    if (GFunc.CompareString(ie.Current.ToString(), "PaperSource"))
                    {

                        string[] strs = l_rpxDoc.Parameters["PaperSource"].DefaultValue.ToString().Split(',');

                        if (strs.Length == 3)
                        {
                            foreach (System.Drawing.Printing.PaperSource item in l_rpxDoc.Document.Printer.PrinterSettings.PaperSources)
                            {
                                if ((int)item.Kind == Convert.ToInt32(strs[0]))
                                {
                                    if (item.SourceName == strs[2])
                                    {
                                        l_rpxDoc.Document.Printer.DefaultPageSettings.PaperSource = item;
                                        l_rpxDoc.PageSettings.PaperSource = item.Kind;
                                        l_rpxDoc.PageSettings.PaperSourceName = item.SourceName;
                                        break;
                                    }
                                }
                            }
                        }                       
                        //PG.PrinterSettings.PrinterName = PrinterName;
                        //IEnumerator iPSize = PG.PrinterSettings.PaperSources.GetEnumerator();
                        //while (iPSize.MoveNext())
                        //{
                        //    if (((System.Drawing.Printing.PaperSource)iPSize.Current).SourceName == Desin.Report.Parameters["PaperSource"].DefaultValue)
                        //    {
                        //        l_rpxDoc.Document.Printer.DefaultPageSettings.PaperSource = (System.Drawing.Printing.PaperSource)iPSize.Current;
                        //        break;
                        //    }
                        //}
                    }

                    if (GFunc.CompareString(ie.Current.ToString(), "Landscape"))
                    {
                        l_rpxDoc.PageSettings.Orientation = (Convert.ToBoolean(Desin.Report.Parameters["Landscape"].DefaultValue)) ? DataDynamics.ActiveReports.Document.PageOrientation.Landscape : DataDynamics.ActiveReports.Document.PageOrientation.Portrait;
                        l_rpxDoc.Document.Printer.DefaultPageSettings.Landscape = Convert.ToBoolean(Desin.Report.Parameters["Landscape"].DefaultValue);
                    }
                }

                return l_rpxDoc;
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

        public static void SaveRpxPageAndPrinterSetting(string RPXReportFilePath, DataDynamics.ActiveReports.ActiveReport RptDoc)
        {
            DataDynamics.ActiveReports.Design.Designer Desin = new DataDynamics.ActiveReports.Design.Designer();
            try
            {
                //Desin.DesignUnits = DataDynamics.ActiveReports.Design.MeasurementUnits.US;
                Desin.Report.LoadLayout(RPXReportFilePath);

                Desin.Report.Document.Printer.PrinterName = RptDoc.Document.Printer.PrinterName;
                Desin.Report.Document.Printer.PrinterSettings = RptDoc.Document.Printer.PrinterSettings;

                //DataDynamics.ActiveReports.ActiveReport l_rpxDoc = RptDoc;

                bool PrinterName = false;
                bool PaperName = false;
                bool PaperSource = false;
                bool Landscape = false;
                bool Margins = false;

                IEnumerator ie = Desin.Report.Parameters.GetEnumerator();

                while (ie.MoveNext())
                {
                    if (GFunc.CompareString(ie.Current.ToString(), "PrinterName"))
                    {
                        Desin.Report.Parameters["PrinterName"].DefaultValue = Desin.Report.Document.Printer.PrinterName;
                        PrinterName = true;
                    }
                    else if (GFunc.CompareString(ie.Current.ToString(), "PaperSize"))
                    {
                        Desin.Report.Parameters["PaperSize"].DefaultValue = ((int)RptDoc.Document.Printer.DefaultPageSettings.PaperSize.Kind + "," + (int)RptDoc.Document.Printer.DefaultPageSettings.PaperSize.RawKind + "," + RptDoc.PageSettings.PaperName).ToString();
                        PaperName = true;
                    }
                    else if (GFunc.CompareString(ie.Current.ToString(), "PaperSource"))
                    {
                        Desin.Report.Parameters["PaperSource"].DefaultValue = ((int)RptDoc.Document.Printer.DefaultPageSettings.PaperSource.Kind + "," + (int)RptDoc.Document.Printer.DefaultPageSettings.PaperSource.RawKind + "," + RptDoc.PageSettings.PaperSourceName).ToString();
                        PaperSource = true;
                    }
                    else if (GFunc.CompareString(ie.Current.ToString(), "Landscape"))
                    {
                        Desin.Report.Parameters["Landscape"].DefaultValue = Desin.Report.Document.Printer.DefaultPageSettings.Landscape.ToString();
                        Landscape = true;
                    }
                    else if (GFunc.CompareString(ie.Current.ToString(), "Margins"))
                    {
                        Desin.Report.Parameters["Margins"].DefaultValue = RptDoc.PageSettings.Margins.Left + "," + RptDoc.PageSettings.Margins.Top
                            + "," + RptDoc.PageSettings.Margins.Right + "," + RptDoc.PageSettings.Margins.Bottom ;
                        Margins = true;
                    }
                }

                //If Not Insert Value
                if (!PrinterName)
                {
                    DataDynamics.ActiveReports.Parameter Pmeter = new DataDynamics.ActiveReports.Parameter();
                    Pmeter.Key = "PrinterName";
                    Pmeter.PromptUser = false;
                    Pmeter.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
                    Pmeter.DefaultValue = Desin.Report.Document.Printer.PrinterName;

                    Desin.Report.Parameters.Add(Pmeter);
                }

                if (!PaperName)
                {
                    DataDynamics.ActiveReports.Parameter Pmeter = new DataDynamics.ActiveReports.Parameter();
                    Pmeter.Key = "PaperSize";
                    Pmeter.PromptUser = false;
                    Pmeter.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
                    Pmeter.DefaultValue = (((int)RptDoc.Document.Printer.DefaultPageSettings.PaperSize.Kind + "," + (int)RptDoc.Document.Printer.DefaultPageSettings.PaperSize.RawKind + "," + RptDoc.PageSettings.PaperName).ToString()).ToString();
                    Desin.Report.Parameters.Add(Pmeter);
                }

                if (!PaperSource)
                {
                    DataDynamics.ActiveReports.Parameter Pmeter = new DataDynamics.ActiveReports.Parameter();
                    Pmeter.Key = "PaperSource";
                    Pmeter.PromptUser = false;
                    Pmeter.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
                    Pmeter.DefaultValue = ((int)RptDoc.Document.Printer.DefaultPageSettings.PaperSource.Kind + "," + (int)RptDoc.Document.Printer.DefaultPageSettings.PaperSource.RawKind + "," + RptDoc.PageSettings.PaperSourceName).ToString();
                    Desin.Report.Parameters.Add(Pmeter);
                }

                if (!Landscape)
                {
                    DataDynamics.ActiveReports.Parameter Pmeter = new DataDynamics.ActiveReports.Parameter();
                    Pmeter.Key = "Landscape";
                    Pmeter.PromptUser = false;
                    Pmeter.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
                    Pmeter.DefaultValue = Desin.Report.Document.Printer.DefaultPageSettings.Landscape.ToString();

                    Desin.Report.Parameters.Add(Pmeter);
                }

                if (!Margins)
                {
                    DataDynamics.ActiveReports.Parameter Pmeter = new DataDynamics.ActiveReports.Parameter();
                    Pmeter.Key = "Margins";
                    Pmeter.PromptUser = false;
                    Pmeter.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
                    Pmeter.DefaultValue = RptDoc.PageSettings.Margins.Left + "," + RptDoc.PageSettings.Margins.Top + ","
                            + RptDoc.PageSettings.Margins.Right + "," + RptDoc.PageSettings.Margins.Bottom;

                    Desin.Report.Parameters.Add(Pmeter);
                }

                Desin.SaveReport(RPXReportFilePath);
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

        #region Set Error Methods
        private static Exception Error(Exception ex, bool ShowMessage)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false);

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(ex);
                }
            }
            catch (Exception nex)
            {
            }
            return ex;

        }

        private static TAException Error(TAException ex, bool ShowMessage)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false);

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(ex);
                }
            }
            catch (Exception nex)
            {
            }
            return ex;
        }
        #endregion

        public static void TestIntoBoss()
        {

        }

        //QR Code testing by May        
        public System.Data.DataTable GetSourceWithQRCode(System.Data.DataTable dt,string filetype)
        {
            System.Data.DataTable dtHdr = (from row in dt.AsEnumerable()
                               group row by new
                               {
                                   DocKey = row.Field<int>("DocKey"),
                                   DocCodeKey = row.Field<int>("DocCodeKey"),
                                   //DocID = row.Field<string>("DocID"),
                                   //DocDate = row.Field<DateTime>("DocDate"),
                                   //DocType = row.Field<int>("DocType"),
                                   //CustID = row.Field<string>("DocConID"),
                               } into grp
                               select new
                               {
                                   DocKey = grp.Key.DocKey,
                                   DocCodeKey = grp.Key.DocCodeKey,
                                   //DocID = grp.Key.DocID,
                                   //DocDate = grp.Key.DocDate,
                                   //DocType = grp.Key.DocType,
                                   //CustID = grp.Key.CustID
                               }).AsDataTable();

            if (dtHdr != null)
            {
                if (dtHdr.Rows.Count > 0)
                {
                    if (filetype.ToLower() == ".rpt")
                    {
                        dtHdr.Columns.Add(new DataColumn("QRCodeInfo", typeof(byte[])));
                        dt.Columns.Add(new DataColumn("QRCodeInfo", typeof(byte[])));
                    }
                    else if (filetype.ToLower() == ".rpx")
                    {
                        dtHdr.Columns.Add(new DataColumn("QRCodeInfo", typeof(string)));
                        dt.Columns.Add(new DataColumn("QRCodeInfo", typeof(string)));
                    }

                    foreach (DataRow dr in dtHdr.Rows)
                    {
                        //Crystal report require image, byte[], Active report only require xml data
                        if (filetype.ToLower() == ".rpt")
                        {
                            //BusinessRefinery.Barcode.CrystalReport.QRCode QRCodeConverter = new BusinessRefinery.Barcode.CrystalReport.QRCode();
                            //QRCodeConverter.Code = GetQRCodeXML(dr);
                            //byte[] imageData = QRCodeConverter.drawBarcode2ByteArray();
                            //dr["QRCodeInfo"] = imageData;

                            string Data ="0000"+ GFunc.NEStr(dr["DocCodeKey"],"")+ GFunc.NEStr(dr["DocKey"], "");//GetQRCodeXML(dr);

                            GdPicture10.LicenseManager oLicenseManager = new GdPicture10.LicenseManager();
                            oLicenseManager.RegisterKEY(SysOptionUtility.GetStr("GDPictureLicenseKey"));

                            GdPicture10.GdPictureImaging m_GdPictureImaging = new GdPicture10.GdPictureImaging();
                            int version = 0;

                            int size = m_GdPictureImaging.BarcodeQRGetSize(Data, GdPicture10.BarcodeQREncodingMode.BarcodeQREncodingModeUndefined, GdPicture10.BarcodeQRErrorCorrectionLevel.BarcodeQRErrorCorrectionLevelH, ref version, 4, 4);
                            byte[] QRCodeArr = new byte[size];

                            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(size, size);
                            int Image_ID = m_GdPictureImaging.CreateGdPictureImageFromBitmap(bitmap);

                            GdPicture10.GdPictureStatus status = m_GdPictureImaging.BarcodeQRWrite(Image_ID, Data, GdPicture10.BarcodeQREncodingMode.BarcodeQREncodingModeUndefined,
                                GdPicture10.BarcodeQRErrorCorrectionLevel.BarcodeQRErrorCorrectionLevelH, 0, 4, 4, 0, 0, 0, System.Drawing.Color.Black, System.Drawing.Color.White);
                            m_GdPictureImaging.SaveAsByteArray(Image_ID, ref QRCodeArr, ref size, GdPicture10.DocumentFormat.DocumentFormatJPEG, 0);


                            dr["QRCodeInfo"] = QRCodeArr;

                            m_GdPictureImaging.ReleaseGdPictureImage(Image_ID);
                            m_GdPictureImaging.Dispose();
                        }
                        else if (filetype.ToLower() == ".rpx")
                            dr["QRCodeInfo"] = GetQRCodeXML(dr);
                    }

                    foreach (DataRow dr in dtHdr.Rows)
                    {
                        DataRow[] d = dt.Select("DocKey=" + dr["DocKey"].ToString());

                        foreach (DataRow dtr in d)
                        {
                            dtr["QRCodeInfo"] = dr["QRCodeInfo"];
                        }
                    }
                }
            }            
           
            return dt;
        }
       

        private string GetQRCodeXML(DataRow drDoc)
        {
           // int DocCodeKey = GFunc.NEInt(drDoc["DocCodeKey"], 0);
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("DO"));
            el.AppendChild(doc.CreateElement("DocKey")).InnerText = drDoc["DocKey"].ToString();
         //   el.AppendChild(doc.CreateElement("DocCodeKey")).InnerText = drDoc["DocCodeKey"].ToString();
            el.AppendChild(doc.CreateElement("DocID")).InnerText = drDoc["DocID"].ToString();
            el.AppendChild(doc.CreateElement("DocType")).InnerText = drDoc["DocType"].ToString();
            el.AppendChild(doc.CreateElement("CustID")).InnerText = drDoc["CustID"].ToString();

            //Console.WriteLine(doc.OuterXml);
          //  return doc.OuterXml.Replace("<", "").Replace(">", "");
            return doc.OuterXml;
        }

        public string GetQRCodeXML(Document objDoc)
        {          
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("DO"));
            el.AppendChild(doc.CreateElement("DocKey")).InnerText = objDoc.DocKey.ToString();
            //el.AppendChild(doc.CreateElement("DocCodeKey")).InnerText = objDoc.DocCodeKey.ToString();
            el.AppendChild(doc.CreateElement("DocID")).InnerText = objDoc.DocID.ToString();
            el.AppendChild(doc.CreateElement("DocType")).InnerText = objDoc.DocType.ToString();

            string docConKey= GFunc.GetStringPropertyValueOrEmpty("DocConKey", objDoc);
            if (docConKey != string.Empty)
                el.AppendChild(doc.CreateElement("CustID")).InnerText = MSTCon.Get(Convert.ToInt32(docConKey)).ConID;
            else
                el.AppendChild(doc.CreateElement("CustID")).InnerText = "";            
            return doc.OuterXml;
        }
    }

    public class ReportParameter
    {
        public ReportParameter()
        {
        }
        public ReportParameter(string parameterName, object parameterValue)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
        }
        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }

    }

    public class Win32
    {
        public const int WM_CLOSE = 16;
        public const int BN_CLICKED = 245;
        public const int WM_COPYDATA = 0x004A;

        public struct CopyDataStruct : IDisposable
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;

            public void Dispose()
            {
                if (this.lpData != IntPtr.Zero)
                {
                    LocalFree(this.lpData);
                    this.lpData = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Contains message information from a thread's message queue.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        /// <summary>
        /// The WM_COMMAND message is sent when the user selects a command 
        /// item from a menu, when a control sends a notification message to 
        /// its parent window, or when an accelerator keystroke is translated.
        /// </summary>
        public const int WM_COMMAND = 0x111;

        /// <summary>
        /// The FindWindow function retrieves a handle to the top-level 
        /// window whose class name and window name match the specified strings.
        /// This function does not search child windows. This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="strClassName">the class name for the window to search for</param>
        /// <param name="strWindowName">the name of the window to search for</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int FindWindow(string strClassName, string strWindowName);

        /// <summary>
        /// The FindWindowEx function retrieves a handle to a window whose class name
        /// and window name match the specified strings.
        /// The function searches child windows, beginning with the one following the specified child window.
        /// This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="hwndParent">a handle to the parent window </param>
        /// <param name="hwndChildAfter">a handle to the child window to start search after</param>
        /// <param name="strClassName">the class name for the window to search for</param>
        /// <param name="strWindowName">the name of the window to search for</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int FindWindowEx(int hwndParent, int hwndChildAfter, string strClassName, string strWindowName);

        /// <summary>
        /// The FindWindowEx API
        /// </summary>
        /// <param name="parentHandle">a handle to the parent window </param>
        /// <param name="childAfter">a handle to the child window to start search after</param>
        /// <param name="className">the class name for the window to search for</param>
        /// <param name="windowTitle">the name of the window to search for</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);


        /// <summary>
        /// The SendMessage function sends the specified message to a
        /// window or windows. It calls the window procedure for the specified
        /// window and does not return until the window procedure
        /// has processed the message.
        /// </summary>
        /// <param name="hWnd">handle to destination window</param>
        /// <param name="Msg">message</param>
        /// <param name="wParam">first message parameter</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(int hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="hWnd">handle to destination window</param>
        /// <param name="Msg">message</param>
        /// <param name="wParam">first message parameter</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// The SendMessage API
        /// </summary>
        /// <param name="hWnd">handle to the required window</param>
        /// <param name="msg">the system/Custom message to send</param>
        /// <param name="wParam">first message parameter</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

        /// <summary>
        /// The SendMessage API
        /// </summary>
        /// <param name="hWnd">handle to the required window</param>
        /// <param name="Msg">the system/Custom message to send</param>
        /// <param name="wParam">first message parameter</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref CopyDataStruct lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalAlloc(int flag, int size);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr p);

        /// <summary>
        /// The PeekMessage function dispatches incoming sent messages, 
        /// checks the thread message queue for a posted message, 
        /// and retrieves the message (if any exist).
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

        /// <summary>
        /// Constructor
        /// </summary>
        public Win32()
        {
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~Win32()
        {
        }
    }
}
