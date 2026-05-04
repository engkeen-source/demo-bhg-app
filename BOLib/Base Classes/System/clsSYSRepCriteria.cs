using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
namespace BOLib
{
    /// <summary>
    /// Summary description for SYSRepCriteria.
    /// </summary>
    [Serializable]
    public class SYSRepCriteria : IDataErrorInfo, INotifyPropertyChanged
    {
        #region +++  Local variables declaration for the class +++

        private int? _RepKey;
        private int? _CriteriaSeq;
        private string _CriteriaType;
        private string _CriteriaDataType;
        private string _CriteriaNm;
        private string _CriteriaLabel;
        private string _CriteriaRowSource;
        private bool _CriteriaSetDateButton;
        private bool _CriteriaHaveFormatSearch;
        private string _CriteriaSearchQryRange;
        private string _CriteriaSearchQryFormat;
        private bool _CriteriaIgnoreRangeOperator;
        private bool _CriteriaIgnoreRange;
        private int? _CriteriaRangeColValue;
        private string _CriteriaSpecialTag;
        private bool _CriteriaHidden;
        private bool _CriteriaLimitToList;
        private bool _CriteriaRequired;
        private string _CriteriaDefaultValue;
        private bool _isDirty;
        private string _error = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +++  Constructor and destructor codes  +++

        /// <summary>
        /// Default constructor that will initialize all properties with default values.
        /// </summary>

        public SYSRepCriteria()
        {
            this._RepKey = null;
            this._CriteriaSeq = null;
            this._CriteriaType = string.Empty;
            this._CriteriaNm = string.Empty;
            this._CriteriaLabel = string.Empty;
            this._CriteriaRowSource = string.Empty;
            this._CriteriaSetDateButton = false;
            this._CriteriaHaveFormatSearch = false;
            this._CriteriaSearchQryRange = string.Empty;
            this._CriteriaSearchQryFormat = string.Empty;
            this._CriteriaIgnoreRangeOperator = false;
            this._CriteriaIgnoreRange = false;
            this._CriteriaRangeColValue = null;
            this._CriteriaSpecialTag = string.Empty;
            this._CriteriaHidden = false;
            this._CriteriaLimitToList = false;
            this._CriteriaRequired = false;
            this._isDirty = false;
        }


        public SYSRepCriteria Clone()
        {

            SYSRepCriteria objCopy = (SYSRepCriteria)this.MemberwiseClone();
            objCopy._isDirty = false;
            return objCopy;
        }


        //Need for implementing IDataErrorInfo interface
        public string this[string name]
        {
            get
            {
                string result = string.Empty;
                return result;
            }
        }
        /// <summary>
        /// Disposing objects
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        #region +++  Properties  +++

        private void NotifyPropertyChanged(String info)
        {
            _isDirty = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        public string Error
        {
            get
            {
                return this._error;
            }
            set
            {
                this._error = value;
            }
        }

        public int? RepKey
        {
            get
            {
                return this._RepKey;
            }
            set
            {
                this._RepKey = value;
                NotifyPropertyChanged("RepKey");
            }
        }

        public int? CriteriaSeq
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

        public string CriteriaType
        {
            get
            {
                return this._CriteriaType;
            }
            set
            {
                this._CriteriaType = value;
                NotifyPropertyChanged("CriteriaType");
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
        public string CriteriaNm
        {
            get
            {
                return this._CriteriaNm;
            }
            set
            {
                this._CriteriaNm = value;
                NotifyPropertyChanged("CriteriaNm");
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

        public string CriteriaRowSource
        {
            get
            {
                return this._CriteriaRowSource;
            }
            set
            {
                this._CriteriaRowSource = value;
                NotifyPropertyChanged("CriteriaRowSource");
            }
        }

        public bool CriteriaSetDateButton
        {
            get
            {
                return this._CriteriaSetDateButton;
            }
            set
            {
                this._CriteriaSetDateButton = value;
                NotifyPropertyChanged("CriteriaSetDateButton");
            }
        }

        public bool CriteriaHaveFormatSearch
        {
            get
            {
                return this._CriteriaHaveFormatSearch;
            }
            set
            {
                this._CriteriaHaveFormatSearch = value;
                NotifyPropertyChanged("CriteriaHaveFormatSearch");
            }
        }

        public string CriteriaSearchQryRange
        {
            get
            {
                return this._CriteriaSearchQryRange;
            }
            set
            {
                this._CriteriaSearchQryRange = value;
                NotifyPropertyChanged("CriteriaSearchQryRange");
            }
        }

        public string CriteriaSearchQryFormat
        {
            get
            {
                return this._CriteriaSearchQryFormat;
            }
            set
            {
                this._CriteriaSearchQryFormat = value;
                NotifyPropertyChanged("CriteriaSearchQryFormat");
            }
        }

        public bool CriteriaIgnoreRangeOperator
        {
            get
            {
                return this._CriteriaIgnoreRangeOperator;
            }
            set
            {
                this._CriteriaIgnoreRangeOperator = value;
                NotifyPropertyChanged("CriteriaIgnoreRangeOperator");
            }
        }

        public bool CriteriaIgnoreRange
        {
            get
            {
                return this._CriteriaIgnoreRange;
            }
            set
            {
                this._CriteriaIgnoreRange = value;
                NotifyPropertyChanged("CriteriaIgnoreRange");
            }
        }

        public int? CriteriaRangeColValue
        {
            get
            {
                return this._CriteriaRangeColValue;
            }
            set
            {
                this._CriteriaRangeColValue = value;
                NotifyPropertyChanged("CriteriaRangeColValue");
            }
        }

        public string CriteriaSpecialTag
        {
            get
            {
                return this._CriteriaSpecialTag;
            }
            set
            {
                this._CriteriaSpecialTag = value;
                NotifyPropertyChanged("CriteriaSpecialTag");
            }
        }

        public bool CriteriaHidden
        {
            get
            {
                return this._CriteriaHidden;
            }
            set
            {
                this._CriteriaHidden = value;
                NotifyPropertyChanged("CriteriaHidden");
            }
        }

        public bool CriteriaLimitToList
        {
            get
            {
                return this._CriteriaLimitToList;
            }
            set
            {
                this._CriteriaLimitToList = value;
                NotifyPropertyChanged("CriteriaLimitToList");
            }
        }

        public bool CriteriaRequired
        {
            get
            {
                return this._CriteriaRequired;
            }
            set
            {
                this._CriteriaRequired = value;
                NotifyPropertyChanged("CriteriaRequired");
            }
        }

        public string CriteriaDefaultValue
        {
            get
            {
                return this._CriteriaDefaultValue;
            }
            set
            {
                this._CriteriaDefaultValue = value;
                NotifyPropertyChanged("CriteriaDefaultValue");
            }
        }

        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
        }


        #endregion

        #region Criteria
        [Serializable()]
        internal class Criteria
        {
            public int? _RepKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }
            internal Criteria(int? RepKey)
            {
                _RepKey = RepKey;
            }
            internal Criteria(int? RepKey, int? Option)
            {
                _RepKey = RepKey;
                _option = Option;
            }
        }
        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
 
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria);
            }
            
            return retValue;
        }
        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRepCriteria_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                }// Already close and dispose data reader.
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }// Already close and dispose sql connection.
            
            return retValue;
        }

        internal static SYSRepCriteria Get(IDataReader dr)
        {
            
            SYSRepCriteria child = new SYSRepCriteria();
            child.Fetch(dr);
            return child;
        }

        internal bool Fetch(IDataReader dataReader)
        {
            
            // Fill data to entity object
            _RepKey =dataReader["RepKey"] == DBNull.Value ? null : (int?)dataReader["RepKey"];
            _CriteriaSeq =dataReader["CriteriaSeq"] == DBNull.Value ? null : (int?)dataReader["CriteriaSeq"];
            _CriteriaType =dataReader["CriteriaType"] == DBNull.Value ? string.Empty:dataReader["CriteriaType"].ToString();
            _CriteriaDataType = dataReader["CriteriaDataType"] == DBNull.Value ? string.Empty : dataReader["CriteriaDataType"].ToString();
            _CriteriaNm =dataReader["CriteriaNm"] == DBNull.Value ? string.Empty:dataReader["CriteriaNm"].ToString();
            _CriteriaLabel =dataReader["CriteriaLabel"] == DBNull.Value ? string.Empty:dataReader["CriteriaLabel"].ToString();
            _CriteriaRowSource =dataReader["CriteriaRowSource"] == DBNull.Value ? string.Empty:dataReader["CriteriaRowSource"].ToString();
            _CriteriaSetDateButton =dataReader["CriteriaSetDateButton"] == DBNull.Value ? false : (bool)dataReader["CriteriaSetDateButton"];
            _CriteriaHaveFormatSearch =dataReader["CriteriaHaveFormatSearch"] == DBNull.Value ? false : (bool)dataReader["CriteriaHaveFormatSearch"];
            _CriteriaSearchQryRange =dataReader["CriteriaSearchQryRange"] == DBNull.Value ? string.Empty:dataReader["CriteriaSearchQryRange"].ToString();
            _CriteriaSearchQryFormat =dataReader["CriteriaSearchQryFormat"] == DBNull.Value ? string.Empty:dataReader["CriteriaSearchQryFormat"].ToString();
            _CriteriaIgnoreRangeOperator =dataReader["CriteriaIgnoreRangeOperator"] == DBNull.Value ? false : (bool)dataReader["CriteriaIgnoreRangeOperator"];
            _CriteriaIgnoreRange =dataReader["CriteriaIgnoreRange"] == DBNull.Value ? false : (bool)dataReader["CriteriaIgnoreRange"];
            _CriteriaRangeColValue =dataReader["CriteriaRangeColValue"] == DBNull.Value ? null : (int?)dataReader["CriteriaRangeColValue"];
            _CriteriaSpecialTag =dataReader["CriteriaSpecialTag"] == DBNull.Value ? string.Empty:dataReader["CriteriaSpecialTag"].ToString();
            _CriteriaHidden =dataReader["CriteriaHidden"] == DBNull.Value ? false : (bool)dataReader["CriteriaHidden"];
            _CriteriaLimitToList =dataReader["CriteriaLimitToList"]== DBNull.Value ? false : (bool)dataReader["CriteriaLimitToList"];
            _CriteriaDefaultValue =dataReader["CriteriaDefaultValue"] == DBNull.Value ? string.Empty:dataReader["CriteriaDefaultValue"].ToString();
            _CriteriaRequired = dataReader["CriteriaRequired"]== DBNull.Value ? false : (bool)dataReader["CriteriaRequired"];

            
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
           
            
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Insert(cn);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }
        internal bool Insert(SqlConnection cn)
        {
           
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRepCriteria_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                
                cm.Parameters.AddWithValue("@NewRepKey", 0);
                if (_RepKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _RepKey);
                if (_CriteriaSeq == null)
                    cm.Parameters.AddWithValue("@CriteriaSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSeq", _CriteriaSeq);
                if (_CriteriaType == null)
                    cm.Parameters.AddWithValue("@CriteriaType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaType", _CriteriaType);
                if (_CriteriaNm == null)
                    cm.Parameters.AddWithValue("@CriteriaNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaNm", _CriteriaNm);
                if (_CriteriaLabel == null)
                    cm.Parameters.AddWithValue("@CriteriaLabel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaLabel", _CriteriaLabel);
                if (_CriteriaRowSource == null)
                    cm.Parameters.AddWithValue("@CriteriaRowSource", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaRowSource", _CriteriaRowSource);
                if (_CriteriaSetDateButton == null)
                    cm.Parameters.AddWithValue("@CriteriaSetDateButton", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSetDateButton", _CriteriaSetDateButton);
                if (_CriteriaHaveFormatSearch == null)
                    cm.Parameters.AddWithValue("@CriteriaHaveFormatSearch", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaHaveFormatSearch", _CriteriaHaveFormatSearch);
                if (_CriteriaSearchQryRange == null)
                    cm.Parameters.AddWithValue("@CriteriaSearchQryRange", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSearchQryRange", _CriteriaSearchQryRange);
                if (_CriteriaSearchQryFormat == null)
                    cm.Parameters.AddWithValue("@CriteriaSearchQryFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSearchQryFormat", _CriteriaSearchQryFormat);
                if (_CriteriaIgnoreRangeOperator == null)
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRangeOperator", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRangeOperator", _CriteriaIgnoreRangeOperator);
                if (_CriteriaIgnoreRange == null)
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRange", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRange", _CriteriaIgnoreRange);
                if (_CriteriaRangeColValue == null)
                    cm.Parameters.AddWithValue("@CriteriaRangeColValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaRangeColValue", _CriteriaRangeColValue);
                if (_CriteriaSpecialTag == null)
                    cm.Parameters.AddWithValue("@CriteriaSpecialTag", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSpecialTag", _CriteriaSpecialTag);
                if (_CriteriaHidden == null)
                    cm.Parameters.AddWithValue("@CriteriaHidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaHidden", _CriteriaHidden);
                if (_CriteriaLimitToList == null)
                    cm.Parameters.AddWithValue("@CriteriaLimitToList", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaLimitToList", _CriteriaLimitToList);
                if (_CriteriaRequired == null)
                    cm.Parameters.AddWithValue("@CriteriaRequired", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaRequired", _CriteriaRequired);
                if (_CriteriaDefaultValue == null)
                    cm.Parameters.AddWithValue("@CriteriaDefaultValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaDefaultValue", _CriteriaDefaultValue);

                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewRepKey"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.            
        }
        #endregion Insert
        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Update(cn);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }
        internal bool Update(SqlConnection cn)
        {
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRepCriteria_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                
                cm.Parameters.AddWithValue("@NewRepKey", 0);
                if (_RepKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _RepKey);
                if (_CriteriaSeq == null)
                    cm.Parameters.AddWithValue("@CriteriaSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSeq", _CriteriaSeq);
                if (_CriteriaType == null)
                    cm.Parameters.AddWithValue("@CriteriaType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaType", _CriteriaType);
                if (_CriteriaNm == null)
                    cm.Parameters.AddWithValue("@CriteriaNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaNm", _CriteriaNm);
                if (_CriteriaLabel == null)
                    cm.Parameters.AddWithValue("@CriteriaLabel", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaLabel", _CriteriaLabel);
                if (_CriteriaRowSource == null)
                    cm.Parameters.AddWithValue("@CriteriaRowSource", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaRowSource", _CriteriaRowSource);
                if (_CriteriaSetDateButton == null)
                    cm.Parameters.AddWithValue("@CriteriaSetDateButton", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSetDateButton", _CriteriaSetDateButton);
                if (_CriteriaHaveFormatSearch == null)
                    cm.Parameters.AddWithValue("@CriteriaHaveFormatSearch", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaHaveFormatSearch", _CriteriaHaveFormatSearch);
                if (_CriteriaSearchQryRange == null)
                    cm.Parameters.AddWithValue("@CriteriaSearchQryRange", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSearchQryRange", _CriteriaSearchQryRange);
                if (_CriteriaSearchQryFormat == null)
                    cm.Parameters.AddWithValue("@CriteriaSearchQryFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSearchQryFormat", _CriteriaSearchQryFormat);
                if (_CriteriaIgnoreRangeOperator == null)
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRangeOperator", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRangeOperator", _CriteriaIgnoreRangeOperator);
                if (_CriteriaIgnoreRange == null)
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRange", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaIgnoreRange", _CriteriaIgnoreRange);
                if (_CriteriaRangeColValue == null)
                    cm.Parameters.AddWithValue("@CriteriaRangeColValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaRangeColValue", _CriteriaRangeColValue);
                if (_CriteriaSpecialTag == null)
                    cm.Parameters.AddWithValue("@CriteriaSpecialTag", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaSpecialTag", _CriteriaSpecialTag);
                if (_CriteriaHidden == null)
                    cm.Parameters.AddWithValue("@CriteriaHidden", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaHidden", _CriteriaHidden);
                if (_CriteriaLimitToList == null)
                    cm.Parameters.AddWithValue("@CriteriaLimitToList", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaLimitToList", _CriteriaLimitToList);
                if (_CriteriaRequired == null)
                    cm.Parameters.AddWithValue("@CriteriaRequired", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CriteriaRequired", _CriteriaRequired);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewRepKey"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.            
        }
        #endregion Update
        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;
            
            using (TransactionScope scope = new TransactionScope())
            {
                //Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    retValue = this.Delete(cn, criteria);
                }
                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// Already close and dispose sql connection.
            
            return retValue;
        }
        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSRepCriteria_Delete";

                
                cm.Parameters.AddWithValue("@RepKey", criteria._RepKey);
                
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql command.            
        }
        #endregion Delete
    }
}