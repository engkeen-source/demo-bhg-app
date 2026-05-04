using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
namespace WinUI
{
    public partial class frmReportExport : Form
    {
        #region Variables
        private string ExportFileType = "";
        private string FileDestinationType = "";
        string ContextMenuSetting = string.Empty;
        private bool _isFinRep = false;     //Is the form called from Financial Statement
        #endregion

        #region Properties
        public string FileType
        {
            get { return ExportFileType; }
            set { ExportFileType = value; }
        }

        public string DestinationType
        {
            get { return FileDestinationType; }
            set { FileDestinationType = value; }
        }

        #endregion

        #region Constructure
        public frmReportExport(bool isFinRep)
        {
            _isFinRep = isFinRep;
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void DOFromLoad()
        {
            try
            {
                ////Add Export File Types //Cbo Editor Style
                //FormattaComboBoxEditor.Items.Add(ReportFileType.AcrobatPDFFile);
                //FormattaComboBoxEditor.Items.Add(ReportFileType.HTMLFile);
                //FormattaComboBoxEditor.Items.Add(ReportFileType.ExcelFile);
                //FormattaComboBoxEditor.Items.Add(ReportFileType.ExcelFileDataOnly);
                //FormattaComboBoxEditor.Items.Add(ReportFileType.CSVFile);
                //FormattaComboBoxEditor.Items.Add(ReportFileType.RichTextFile);
                
                DataTable dtFormat = new DataTable();
                dtFormat.Columns.Add("ValueCol");
                dtFormat.Columns.Add("Format");
                //Add Export File Types
                dtFormat.Rows.Add(0,ReportFileType.AcrobatPDFFile);
                dtFormat.Rows.Add(1,ReportFileType.HTMLFile);
                dtFormat.Rows.Add(2,ReportFileType.ExcelFile);
                dtFormat.Rows.Add(3,ReportFileType.ExcelFileDataOnly);
                dtFormat.Rows.Add(4,ReportFileType.CSVFile);
                dtFormat.Rows.Add(5,ReportFileType.RichTextFile);
                
                Format.DataSource = dtFormat;
                Format.ValueMember = "ValueCol";
                Format.DisplayMember = "Format";
                Format.SetValueTrigger(0,false);

                //Add Destination Type
                DataTable dtDestination = new DataTable();
                dtDestination.Columns.Add("ValueCol");
                dtDestination.Columns.Add("DestinationType");

                dtDestination.Rows.Add(0,ReportFileDestinationType.Application);
                dtDestination.Rows.Add(1,ReportFileDestinationType.DiskFile);

                Destination.DataSource = dtDestination;
                Destination.ValueMember = "ValueCol";
                Destination.DisplayMember = "DestinationType";
                Destination.SetValueTrigger(0,false);

            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }

        #endregion

        #region Events
        private void frmReportExport_Load(object sender, EventArgs e)
        {
            try 
	        {
                DOFromLoad();
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);
                if (!_isFinRep)
                {
                    DataTable dt = Format.DataSource as DataTable;
                   dt.DefaultView.RowFilter="MsgValue<>30";
                   Format.DataSource = dt.DefaultView;
                }
                
	        }
	        catch (Exception ex)
	        {
		        throw ex;
	        }
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ExportFileType = Format.Text;
            DestinationType = Destination.Text;

            this.DialogResult = DialogResult.OK;
        }


        #region Set Error Methods
        private Exception Error(Exception ex, bool ShowMessage)
        {
            Exception l_tmpex = ex;
            try
            {

                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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
                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, ShowMessage, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
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

        private void frmReportExport_KeyDown(object sender, KeyEventArgs e)
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
        }//CodeCompleted
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
        }//CodeCompleted
    }
}
