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
    public partial class frmPeriodChooser : Form
    {
        //Variable
        internal DataTable dtCriteria;
        internal string criteriaNm;
        internal string criteriaLabel;
        internal int criteriaValueInt;
        string ContextMenuSetting = string.Empty;

        //Initialization
        public frmPeriodChooser()
        {
            InitializeComponent();
        }
        public frmPeriodChooser(DataTable dt, string CriteriaNm, string CriteriaLabel)
        {
            //Created by Jack....
            //Call From frmReportDirectory ... **When form is not have MDI parent**.
            InitializeComponent();
            dtCriteria = dt;
            criteriaNm = CriteriaNm;
            criteriaLabel = CriteriaLabel;
        }

        //Form Event
        private void frmPeriodChooser_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                //This Form process is generate the To period base on Fromperiod, DifferencePeriod,PeriodType and Period..
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);

                this.PeriodType.SetValueTrigger(10, false);
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
                Cursor = Cursors.Default;
            }
        }
        private void frmPeriodChooser_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
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
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        //Button Event
        private void btnOk_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            IEnumerable<DataRow> dtFilter = null;

            try
            {
                switch (GFunc.NEInt(PeriodType.Value, 0))
                {
                    case (int)GEnum.PeriodType.Use_Selected_Period:
                        if (GFunc.IsNE(Period.Value))
                        {
                            MsgBox.Show("Period cannot be empty");
                            return;
                        }
                        criteriaValueInt = Convert.ToInt32(Period.Value);
                        break;
                    case (int)GEnum.PeriodType.Current_Period:
                        criteriaValueInt = Convert.ToInt32(DateTime.Today.Year.ToString() + (DateTime.Today.Month.ToString().Length == 1 ? "0" : "") + DateTime.Today.Month.ToString());
                        break;
                    case (int)GEnum.PeriodType.Current_Year_Jan:
                        criteriaValueInt = Convert.ToInt32(DateTime.Today.Year.ToString() + "01");
                        break;
                    case (int)GEnum.PeriodType.Current_Year_Dec:
                        criteriaValueInt = Convert.ToInt32(DateTime.Today.Year.ToString() + "12");
                        break;
                    case (int)GEnum.PeriodType.Current_Fiscal_Year_Start:
                        int curPeriod = Convert.ToInt32(DateTime.Today.Year.ToString() + (DateTime.Today.Month.ToString().Length == 1 ? "0" : "") + DateTime.Today.Month.ToString());
                        DataTable dt = Period.DataSource as DataTable;
                        //dt.DefaultView.RowFilter = "Period=" + curPeriod;                        
                        dtFilter = dt.AsEnumerable().Where(r => r.Field<int>("Period") == curPeriod);                      
                        int seqToFilter =dtFilter.Count() >0 ? Convert.ToInt32(dtFilter.ElementAt(0)["PeriodSeq"]) :0;
                        //dt.DefaultView.RowFilter = "";

                        //dt.DefaultView.RowFilter = "PeriodSeq=" + seqToFilter;
                        //dt.DefaultView.Sort = "Period Asc";
                         dtFilter = dt.AsEnumerable().Where(r => r.Field<int>("PeriodSeq") == seqToFilter).OrderBy(r=>r.Field<int>("Period"));
                        criteriaValueInt =dtFilter.Count() >0 ? Convert.ToInt32(dtFilter.ElementAt(0)["Period"]) :0;
                        //dt.DefaultView.RowFilter = "";
                        break;
                    case (int)GEnum.PeriodType.Current_Fiscal_Year_End:
                        curPeriod = Convert.ToInt32(DateTime.Today.Year.ToString() + (DateTime.Today.Month.ToString().Length == 1 ? "0" : "") + DateTime.Today.Month.ToString());
                        dt = Period.DataSource as DataTable;

                        //dt.DefaultView.RowFilter = "Period=" + curPeriod;
                        //seqToFilter = Convert.ToInt32(dt.DefaultView[0]["PeriodSeq"]);
                        //dt.DefaultView.RowFilter = "";

                        //dt.DefaultView.RowFilter = "PeriodSeq=" + seqToFilter;
                        //dt.DefaultView.Sort = "Period Desc";
                        //criteriaValueInt = Convert.ToInt32(dt.DefaultView[0]["Period"]);
                        //dt.DefaultView.RowFilter = "";

                         dtFilter = dt.AsEnumerable().Where(r => r.Field<int>("Period") == curPeriod);       
                         seqToFilter = dtFilter.Count() > 0 ? Convert.ToInt32(dtFilter.ElementAt(0)["PeriodSeq"]) : 0;
                        //dt.DefaultView.RowFilter = "";

                        //dt.DefaultView.RowFilter = "PeriodSeq=" + seqToFilter;
                        //dt.DefaultView.Sort = "Period Asc";
                        dtFilter = dt.AsEnumerable().Where(r => r.Field<int>("PeriodSeq") == seqToFilter).OrderByDescending(r => r.Field<int>("Period"));
                        criteriaValueInt = dtFilter.Count() > 0 ? Convert.ToInt32(dtFilter.ElementAt(0)["Period"]) : 0;
                        break;
                }

                //Add to DataTable
                //dtCriteria.DefaultView.RowFilter = "CriteriaNm='" + criteriaNm + "'";
                dtFilter = dtCriteria.AsEnumerable().Where(r => r.Field<string>("CriteriaNm").Equals(criteriaNm));
                if (dtFilter.Count() < 1)
                {
                    DataRow dr = dtCriteria.NewRow();
                    dr["CriteriaNm"] = criteriaNm;
                    dr["CriteriaLabel"] = criteriaLabel;
                    dr["PeriodType"] = (int)PeriodType.Value;
                    dr["PeriodDifference"] = Convert.ToInt32(PeriodDifference.Value);
                    dr["CriteriaValueInt"] = criteriaValueInt;
                    dtCriteria.Rows.Add(dr);
                }
                else
                {
                    dtCriteria.Rows.Remove(dtFilter.ElementAt(0));
                    DataRow dr = dtCriteria.NewRow();
                    dr["CriteriaNm"] = criteriaNm;
                    dr["CriteriaLabel"] = criteriaLabel;
                    dr["PeriodType"] = (int)PeriodType.Value;
                    dr["PeriodDifference"] = Convert.ToInt32(PeriodDifference.Value);
                    dr["CriteriaValueInt"] = criteriaValueInt;
                    dtCriteria.Rows.Add(dr);
                }
               //dtCriteria.DefaultView.RowFilter = "";
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
                Cursor = Cursors.Arrow;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        //Controls Event
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
        private void PeriodType_CustomUpdate(object sender, CancelEventArgs e)  
        {
            try
            {
                Period.Enabled = false;
                PeriodDifference.Enabled = false;

                if (GFunc.NEInt(PeriodType.Value, 0) == (int)GEnum.PeriodType.Use_Selected_Period)
                {
                    Period.Enabled = true;
                    PeriodDifference.Enabled = true;
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
