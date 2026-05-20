


using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for TASToDoDetCriterias.
    /// </summary>
    [Serializable]
    public class TASToDoDetCriteria 
    {
        #region +++  Local variables declaration for the class +++
        protected int _ToDoKey;
        protected int _CriteriaKey;
        protected int _CriteriaSeq;
        protected string _CriteriaName;
        protected string _CriteriaSearchFormat;
        protected string _CriteriaLabel;
        protected string _CriteriaDataType;
        protected string _CriteriaValueChar;
        protected int? _CriteriaValueInt;
        protected decimal? _CriteriaValueMoney;
        protected DateTime? _CriteriaValueDate;
        protected int _DateType;
        protected Int16 _DateDifference;
        protected int _WeekDay;
        protected int _MthDayNum;
        protected int _MthWeek;
        protected int _MthDay;
        protected int _YearMthNum;
        protected int _YearMthDayNum;
        protected int _YearMthWeek;
        protected int _YearMthDay;
        protected int _PeriodType;
        protected Int16 _PeriodDifference;
        protected Int16 _PeriodMth;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        
        #region +++  Properties  +++

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        
        public int ToDoKey
        {

            get
            {
                return this._ToDoKey;
            }
            set
            {
                this._ToDoKey = value;
                NotifyPropertyChanged("ToDoKey");
            }
        }
        public int CriteriaKey
        {

            get
            {
                return this._CriteriaKey;
            }
            set
            {
                this._CriteriaKey = value;
                NotifyPropertyChanged("CriteriaKey");
            }
        }
        public int CriteriaSeq
        {

            get
            {
                return this._CriteriaSeq;
            }
            set
            {
                this._CriteriaSeq = value;
                NotifyPropertyChanged("CriteriaSeq");
            }
        }
        public string CriteriaName
        {

            get
            {
                return this._CriteriaName;
            }
            set
            {
                this._CriteriaName = value;
                NotifyPropertyChanged("CriteriaName");
            }
        }
        public string CriteriaSearchFormat
        {

            get
            {
                return this._CriteriaSearchFormat;
            }
            set
            {
                this._CriteriaSearchFormat = value;
                NotifyPropertyChanged("CriteriaSearchFormat");
            }
        }
        public string CriteriaLabel
        {

            get
            {
                return this._CriteriaLabel;
            }
            set
            {
                this._CriteriaLabel = value;
                NotifyPropertyChanged("CriteriaLabel");
            }
        }
        public string CriteriaDataType
        {

            get
            {
                return this._CriteriaDataType;
            }
            set
            {
                this._CriteriaDataType = value;
                NotifyPropertyChanged("CriteriaDataType");
            }
        }
        public string CriteriaValueChar
        {

            get
            {
                return this._CriteriaValueChar;
            }
            set
            {
                this._CriteriaValueChar = value;
                NotifyPropertyChanged("CriteriaValueChar");
            }
        }
        public int? CriteriaValueInt
        {

            get
            {
                return this._CriteriaValueInt;
            }
            set
            {
                this._CriteriaValueInt = value;
                NotifyPropertyChanged("CriteriaValueInt");
            }
        }
        public decimal? CriteriaValueMoney
        {

            get
            {
                return this._CriteriaValueMoney;
            }
            set
            {
                this._CriteriaValueMoney = value;
                NotifyPropertyChanged("CriteriaValueMoney");
            }
        }
        public DateTime? CriteriaValueDate
        {

            get
            {
                return this._CriteriaValueDate;
            }
            set
            {
                this._CriteriaValueDate = value;
                NotifyPropertyChanged("CriteriaValueDate");
            }
        }
        public int DateType
        {

            get
            {
                return this._DateType;
            }
            set
            {
                this._DateType = value;
                NotifyPropertyChanged("DateType");
            }
        }
        public Int16 DateDifference
        {

            get
            {
                return this._DateDifference;
            }
            set
            {
                this._DateDifference = value;
                NotifyPropertyChanged("DateDifference");
            }
        }
        public int WeekDay
        {

            get
            {
                return this._WeekDay;
            }
            set
            {
                this._WeekDay = value;
                NotifyPropertyChanged("WeekDay");
            }
        }
        public int MthDayNum
        {

            get
            {
                return this._MthDayNum;
            }
            set
            {
                this._MthDayNum = value;
                NotifyPropertyChanged("MthDayNum");
            }
        }
        public int MthWeek
        {

            get
            {
                return this._MthWeek;
            }
            set
            {
                this._MthWeek = value;
                NotifyPropertyChanged("MthWeek");
            }
        }
        public int MthDay
        {

            get
            {
                return this._MthDay;
            }
            set
            {
                this._MthDay = value;
                NotifyPropertyChanged("MthDay");
            }
        }
        public int YearMthNum
        {

            get
            {
                return this._YearMthNum;
            }
            set
            {
                this._YearMthNum = value;
                NotifyPropertyChanged("YearMthNum");
            }
        }
        public int YearMthDayNum
        {

            get
            {
                return this._YearMthDayNum;
            }
            set
            {
                this._YearMthDayNum = value;
                NotifyPropertyChanged("YearMthDayNum");
            }
        }
        public int YearMthWeek
        {

            get
            {
                return this._YearMthWeek;
            }
            set
            {
                this._YearMthWeek = value;
                NotifyPropertyChanged("YearMthWeek");
            }
        }
        public int YearMthDay
        {

            get
            {
                return this._YearMthDay;
            }
            set
            {
                this._YearMthDay = value;
                NotifyPropertyChanged("YearMthDay");
            }
        }
        public int PeriodType
        {

            get
            {
                return this._PeriodType;
            }
            set
            {
                this._PeriodType = value;
                NotifyPropertyChanged("PeriodType");
            }
        }
        public Int16 PeriodDifference
        {

            get
            {
                return this._PeriodDifference;
            }
            set
            {
                this._PeriodDifference = value;
                NotifyPropertyChanged("PeriodDifference");
            }
        }
        public Int16 PeriodMth
        {

            get
            {
                return this._PeriodMth;
            }
            set
            {
                this._PeriodMth = value;
                NotifyPropertyChanged("PeriodMth");
            }
        }

        #endregion
    }
}