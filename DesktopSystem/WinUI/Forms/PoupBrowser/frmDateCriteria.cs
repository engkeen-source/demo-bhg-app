using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;

namespace WinUI
{
    public partial class frmDateCriteria : Form
    {
        //Variable
        internal DataTable dtCriteria;
        internal DateTime CriteriaDate;
        internal string criteriaNm;
        internal string criteriaLabel;       
        string ContextMenuSetting = string.Empty;

        //Initialization
        public frmDateCriteria()
        {
            InitializeComponent();
        }
        public frmDateCriteria(DataTable dt,string vCriteriaNm,string vCriteriaLabel)
        {
            InitializeComponent();
            dtCriteria = dt;
            criteriaNm = vCriteriaNm;
            criteriaLabel = vCriteriaLabel;
        }//Completed

        //Form Event
        private void frmDateCriteria_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                GlobalUI.FormGrids_Set(this, 0, out ContextMenuSetting);
                //GlobalUI.cmnuGlobal_Set(this);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew(0);
                GlobalUI.Combos_Fill(this, 0);

                SelectedDate.SetValueTrigger(DateTime.Today, false);
                DateType.SetValueTrigger(GEnum.DateType.Use_Selected_Date, false);
                DateType_CustomUpdate(null, null);
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
        }//Completed
        private void frmDateCriteria_KeyDown(object sender, KeyEventArgs e)
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
        }//Completed

        //Control Event
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
        private void DateType_CustomUpdate(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                #region Variable Declearation
                bool SelectedDate_Enable = false;
                bool WeeKDay_Enable = false;
                bool Day0_Checked = false;
                bool Text0_Checked = false;
                bool Day1_Checked = false;
                bool Text1_Checked = false;

                bool MthDayNum_Enabled = false;
                bool MthWeek_Enabled = false;
                bool MthDay_Enabled = false;

                bool YearMthDayNum_Enabled = false;
                bool YearMthNum_Enabled = false;
                bool YearMthWeek_Enabled = false;
                bool YearMthDay_Enabled = false;

                int WeekDay_Default = 0;

                int MthDayNum_Default = 0;
                int MthWeek_Default = 0;
                int MthDay_Default = 0;

                int YearMthDayNum_Default = 0;
                int YearMthNum_Default = 0;
                int YearMthWeek_Default = 0;
                int YearMthDay_Default = 0;
                #endregion

                #region Define Default Value And Control Enable True/False
                if (GFunc.NEInt(DateType.Value, 0) == (int)GEnum.DateType.Use_Selected_Date)//Use Selected Date
                    SelectedDate_Enable = true;
                else if (GFunc.NEInt(DateType.Value, 0) == (int)GEnum.DateType.Current_Day)//Use Current Day
                    SelectedDate.DateValue = DateTime.Today;
                else if (GFunc.NEInt(DateType.Value, 0) == (int)GEnum.DateType.Current_Week)//Use Current Week
                    WeeKDay_Enable = true;
                else if (GFunc.NEInt(DateType.Value, 0) == (int)GEnum.DateType.Current_Month)//Use Current Month
                {
                    Day0_Checked = true;
                    MthDayNum_Enabled = true;
                    MthDayNum_Default = 1;
                }
                else if (GFunc.NEInt(DateType.Value, 0) == (int)GEnum.DateType.Current_Year)//Use Current Year
                {
                    Day1_Checked = true;
                    YearMthDayNum_Enabled = true;
                    YearMthNum_Enabled = true;
                    YearMthDayNum_Default = 1;
                    YearMthNum_Default = 1;
                }
                #endregion

                #region Set Controls Enable
                SelectedDate.Enabled = SelectedDate_Enable;
                WeekDay.Enabled = WeeKDay_Enable;
                Day0.Checked = Day0_Checked;
                Text0.Checked = Text0_Checked;
                Day0.Enabled = Text0.Enabled = Day0_Checked;
                Day1.Checked = Day1_Checked;
                Text1.Checked = Text1_Checked;
                Day1.Enabled = Text1.Enabled = Day1_Checked;

                MthDayNum.Enabled = MthDayNum_Enabled;
                MthWeek.Enabled = MthWeek_Enabled;
                MthDay.Enabled = MthDay_Enabled;
                YearMthDayNum.Enabled = YearMthDayNum_Enabled;
                YearMthNum.Enabled = YearMthNum_Enabled;
                YearMthWeek.Enabled = YearMthWeek_Enabled;
                YearMthDay.Enabled = YearMthDay_Enabled;
                #endregion

                #region Set Control Default Value
                WeekDay.SetValueTrigger(WeekDay_Default.ToString(), false);
                MthDayNum.SetValueTrigger(MthDayNum_Default.ToString(), false);
                MthWeek.SetValueTrigger(MthWeek_Default, false);
                MthDay.SetValueTrigger(MthDay_Default, false);

                YearMthDayNum.SetValueTrigger(YearMthDayNum_Default.ToString(), false);
                YearMthNum.SetValueTrigger(YearMthNum_Default.ToString(), false);
                YearMthWeek.SetValueTrigger(YearMthWeek_Default, false);
                YearMthDay.SetValueTrigger(YearMthDay_Default, false);
                #endregion
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
        }//Completed
        private void Day0_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                MthDayNum.Enabled = true;
                MthDayNum.SetValueTrigger(1,false);

                MthWeek.Enabled = false;
                MthDay.Enabled = false;
                MthWeek.SetValueTrigger(0,false);
                MthDay.SetValueTrigger(0,false);
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
        }//Completed
        private void Text0_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                MthWeek.Enabled = true;
                MthDay.Enabled = true;
                MthWeek.SetValueTrigger(1,false);
                MthDay.SetValueTrigger(1,false);

                MthDayNum.Enabled = false;
                MthDayNum.SetValueTrigger(0,false);
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
        }//Completed
        private void Day1_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                YearMthDayNum.Enabled = true;
                YearMthDayNum.SetValueTrigger(1,false);

                YearMthNum.Enabled = true;
                YearMthNum.SetValueTrigger(1,false);            

                YearMthWeek.Enabled = false;
                YearMthWeek.SetValueTrigger(0,false);
                YearMthDay.Enabled = false;
                YearMthDay.SetValueTrigger(0,false);
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
        }//Completed
        private void Text1_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                YearMthWeek.Enabled = true;
                YearMthWeek.SetValueTrigger(1,false);
                YearMthDay.Enabled = true;
                YearMthDay.SetValueTrigger(1,false);

                YearMthNum.Enabled = true;
                YearMthNum.SetValueTrigger(1,false);

                YearMthDayNum.Enabled = false;
                YearMthDayNum.SetValueTrigger(0,false);
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
        }//Completed
        private void MthDayNum_Validated(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.NEInt(MthDayNum.Value, 0) < 1)
                    MthDayNum.SetValueTrigger(1, false);
                else if (GFunc.NEInt(MthDayNum.Value, 0) > DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))
                {
                    MthDayNum.SetValueTrigger(DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month).ToString(), false);
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
                Cursor = Cursors.Default;
            }
        }//Completed
        private void YearMthDayNum_Validated(object sender, EventArgs e)
        {
            try
            {
                if (GFunc.NEInt(MthDayNum.Value, 0) < 1)
                    MthDayNum.SetValueTrigger(1, false);
                else if (GFunc.NEInt(MthDayNum.Value, 0) > DateTime.DaysInMonth(DateTime.Today.Year, (int)YearMthNum.Value))
                {
                    MthDayNum.SetValueTrigger(DateTime.DaysInMonth(DateTime.Today.Year, (int)YearMthNum.Value).ToString(), false);
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
                Cursor = Cursors.Default;
            }
        }//Completed

        //Button Event
        private void btnOk_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                DateTime crDate=DateTime.Today;
                if (DateType.Value == null)
                    return;

                #region Get Criteria Date
                switch (GFunc.NEInt(DateType.Value, 0))
                {
                    case (int)GEnum.DateType.Use_Selected_Date:
                        crDate = (DateTime)SelectedDate.DateValue;
                        break;
                    case (int)GEnum.DateType.Current_Day:
                        crDate = DateTime.Today;
                        break;
                    case (int)GEnum.DateType.Current_Week:
                        int dayToBeAdd = (int)DateTime.Today.DayOfWeek;
                        crDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, (DateTime.Today.Day - dayToBeAdd)).AddDays(getDayofGivenDay(WeekDay.Text));
                        break;
                    case (int)GEnum.DateType.Current_Month:
                        if (DayMonthAndYearValidation(false) == false)
                            return;
                        if (Day0.Checked)
                            crDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, Convert.ToInt16(MthDayNum.Value));
                        else
                        {
                            int week;
                            if ((DateTime.IsLeapYear(DateTime.Today.Year) == false && (int)MthWeek.Value == 5 && DateTime.Today.Month == 2))
                                week = 4;
                            else
                                week = (int)MthWeek.Value;
                            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                            int DayOfWeek = (int)new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).DayOfWeek;
                            int Day = getDayofGivenDay(MthDay.Text);
                            if (DayOfWeek > Day) //if the required week day is not exist in current week , skip to next week
                            {
                                crDate = firstDayOfMonth.AddDays((week * 7 - DayOfWeek) + Day);
                            }
                            else if (DayOfWeek < Day) //Found in this week
                            {
                                crDate = firstDayOfMonth.AddDays(((week - 1) * 7) + (Day - DayOfWeek));
                            }
                            else
                            {
                                crDate = firstDayOfMonth.AddDays(((week - 1) * 7));
                            }
                        }
                        break;
                    case (int)GEnum.DateType.Current_Year:
                        if (DayMonthAndYearValidation(false) == false)
                            return;

                        if (Day1.Checked)
                            crDate = new DateTime(DateTime.Today.Year, (int)YearMthNum.Value, (int)YearMthDayNum.Value);    
                        else
                        {
                            int week;
                            if ((DateTime.IsLeapYear(DateTime.Today.Year) == false && (int)YearMthWeek.Value == 5 && (int)YearMthNum.Value == 2))
                                week = 4;
                            else
                                week = (int)YearMthWeek.Value;
                            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, (int)YearMthNum.Value, 1);
                            int DayOfWeek = (int)new DateTime(DateTime.Today.Year, (int)YearMthNum.Value, 1).DayOfWeek;
                            int Day = getDayofGivenDay(YearMthDay.Text);
                            if (DayOfWeek > Day) //if the required week day is not exist in current week , skip to next week
                            {
                                crDate = firstDayOfMonth.AddDays((week * 7 - DayOfWeek) + Day);
                            }
                            else if (DayOfWeek < Day) //Found in this week
                            {
                                crDate = firstDayOfMonth.AddDays(((week - 1) * 7) + (Day - DayOfWeek));
                            }
                            else
                            {
                                crDate = firstDayOfMonth.AddDays(((week - 1) * 7));
                            }
                        }
                        break;
                }
                #endregion

                #region Criteria Row Add

                IEnumerable<DataRow> dtCriteriaFilter = dtCriteria.AsEnumerable().Where(r => r.Field<string>("CriteriaNm").Equals(criteriaNm));
                if (dtCriteriaFilter.Count() < 1)
                {
                    dtCriteria.Rows.Add(new object[] { criteriaNm, criteriaLabel, crDate, DBNull.Value, GFunc.NEInt(DateType.Value, 0), GFunc.NEInt(DateDifference.Value, 0), GFunc.NEInt(WeekDay.Value, 0), GFunc.NEInt(MthDayNum.Value, 0), GFunc.NEInt(MthWeek.Value, 0), GFunc.NEInt(MthDay.Value, 0), GFunc.NEInt(YearMthNum.Value, 0), GFunc.NEInt(YearMthDayNum.Value, 0), GFunc.NEInt(YearMthWeek.Value, 0), GFunc.NEInt(YearMthDay.Value, 0) });
                }
                else
                {
                    dtCriteria.Rows.Remove(dtCriteriaFilter.ElementAt(0));
                    dtCriteria.Rows.Add(new object[] { criteriaNm, criteriaLabel, crDate, DBNull.Value, GFunc.NEInt(DateType.Value, 0), GFunc.NEInt(DateDifference.Value, 0), GFunc.NEInt(WeekDay.Value, 0), GFunc.NEInt(MthDayNum.Value, 0), GFunc.NEInt(MthWeek.Value, 0), GFunc.NEInt(MthDay.Value, 0), GFunc.NEInt(YearMthNum.Value, 0), GFunc.NEInt(YearMthDayNum.Value, 0), GFunc.NEInt(YearMthWeek.Value, 0), GFunc.NEInt(YearMthDay.Value, 0) });
                }
                #endregion
                
                this.CriteriaDate = crDate;
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
                Cursor = Cursors.Default;
            }
        }//Completed
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }//Completed

        //Form Function
        private bool DayMonthAndYearValidation(bool CheckForMonth)
        {
            try
            {
                if (CheckForMonth)
                {
                    if (Day0.Checked)
                    {
                        if (MthDayNum.Value == null)
                        {
                            MsgBox.Show("Please choose day for current month");
                            MthDayNum.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        if (MthWeek.Value == null)
                        {
                            MsgBox.Show("Please Choose Week for current month");
                            MthWeek.Focus();
                            return false;
                        }
                        else if (MthDay.Value == null)
                        {
                            MsgBox.Show("Please Choose Day for current month");
                            MthDay.Focus();
                            return false;
                        }
                    }
                }
                else
                {
                    if (Day1.Checked)
                    {
                        if (YearMthDayNum.Value == null)
                        {
                            MsgBox.Show("Please choose day for current year");
                            YearMthDayNum.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        if (YearMthWeek.Value == null)
                        {
                            MsgBox.Show("Please Choose Week for current year");
                            YearMthWeek.Focus();
                            return false;
                        }
                        else if (YearMthDay.Value == null)
                        {
                            MsgBox.Show("Please Choose Day for current year");
                            YearMthDay.Focus();
                            return false;
                        }
                    }

                    if (YearMthNum.Value == null)
                    {
                        MsgBox.Show("Please Choose Month for current year");
                        YearMthNum.Focus();
                        return false;
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
        }//Completed
        private int getDayofGivenDay(string day)
        {
            try
            {
                switch(day.Substring(0,3).ToUpper())
                {
                    case "MON":
                        return 1;
                    case "TUE":
                        return 2;
                    case "WED":
                        return 3;
                    case "THU":
                        return 4;
                    case "FRI":
                        return 5;
                    case "SAT":
                        return 6;
                    case "SUN":
                        return 7;
                    default :
                        throw new IndexOutOfRangeException("The string is out of range");
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
                Cursor = Cursors.Default;
            }
        }//Completed

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
