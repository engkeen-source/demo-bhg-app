using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;

namespace BOLib
{
    [Serializable()]
    public class MSTFinDetail : Csla.BusinessBase<MSTFinDetail>
    {
        #region Business Properties and Methods

        //declare members
        internal int _repKey = 0;
        internal int _repDetKey = 0;
        internal int _detType = 0;
        internal int _detSeq = 0;
        internal decimal _detHeight = 0.0M;
        internal bool _firstColumn = false;
        internal string _colFormat = string.Empty;
        internal string _bodyTextValue = string.Empty;
        internal string _bodyTextFormat = string.Empty;
        internal int? _rowNo = 0;
        internal string _rowSummaryText = string.Empty;
        internal bool _rowRevValueForBal = false;
        internal bool _rowRevValueForFormula = false;
        internal string _rowHide = string.Empty;
        internal string _totalText = string.Empty;
        internal string _totalFormat = string.Empty;
        internal string _totalHide = string.Empty;
        internal bool _pageBreak = false;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = 0;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = 0;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int RepKey
        {
            get
            {
                return _repKey;
            }
            set
            {
                _repKey = value;
                PropertyHasChanged("RepKey");
            }
        }

        public int RepDetKey
        {
            get
            {
                return _repDetKey;
            }
            set
            {
                _repDetKey = value;
                PropertyHasChanged("RepDetKey");
            }
        }

        public int DetType
        {
            get
            {
                return _detType;
            }
            set
            {
                _detType = value;
                PropertyHasChanged("DetType");
            }
        }

        public int DetSeq
        {
            get
            {
                return _detSeq;
            }
            set
            {
                _detSeq = value;
                PropertyHasChanged("DetSeq");
            }
        }

        public decimal DetHeight
        {
            get
            {
                return _detHeight;
            }
            set
            {
                _detHeight = value;
                PropertyHasChanged("DetHeight");
            }
        }

        public bool FirstColumn
        {
            get
            {
                return _firstColumn;
            }
            set
            {
                _firstColumn = value;
                PropertyHasChanged("FirstColumn");
            }
        }

        public string ColFormat
        {
            get
            {
                return _colFormat;
            }
            set
            {
                _colFormat = value;
                PropertyHasChanged("ColFormat");
            }
        }

        public string BodyTextValue
        {
            get
            {
                return _bodyTextValue;
            }
            set
            {
                _bodyTextValue = value;
                PropertyHasChanged("BodyTextValue");
            }
        }

        public string BodyTextFormat
        {
            get
            {
                return _bodyTextFormat;
            }
            set
            {
                _bodyTextFormat = value;
                PropertyHasChanged("BodyTextFormat");
            }
        }

        public int? RowNo
        {
            get
            {
                return _rowNo;
            }
            set
            {
                _rowNo = value;
                PropertyHasChanged("RowNo");
            }
        }

        public string RowSummaryText
        {
            get
            {
                return _rowSummaryText;
            }
            set
            {
                _rowSummaryText = value;
                PropertyHasChanged("RowSummaryText");
            }
        }

        public bool RowRevValueForBal
        {
            get
            {
                return _rowRevValueForBal;
            }
            set
            {
                _rowRevValueForBal = value;
                PropertyHasChanged("RowRevValueForBal");
            }
        }

        public bool RowRevValueForFormula
        {
            get
            {
                return _rowRevValueForFormula;
            }
            set
            {
                _rowRevValueForFormula = value;
                PropertyHasChanged("RowRevValueForFormula");
            }
        }

        public string RowHide
        {
            get
            {
                return _rowHide;
            }
            set
            {
                _rowHide = value;
                PropertyHasChanged("RowHide");
            }
        }

        public string TotalExp
        {
            get
            {
                return _totalText;
            }
            set
            {
                _totalText = value;
                PropertyHasChanged("TotalExp");
            }
        }

        public string TotalFormat
        {
            get
            {
                return _totalFormat;
            }
            set
            {
                _totalFormat = value;
                PropertyHasChanged("TotalFormat");
            }
        }

        public string TotalHide
        {
            get
            {
                return _totalHide;
            }
            set
            {
                _totalHide = value;
                PropertyHasChanged("TotalHide");
            }
        }

        public bool PageBreak
        {
            get
            {
                return _pageBreak;
            }
            set
            {
                _pageBreak = value;
                PropertyHasChanged("PageBreak");
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;
                PropertyHasChanged("CreateDate");
            }
        }

        public int? CreateUserKey
        {
            get
            {
                return _createUserKey;
            }
            set
            {
                _createUserKey = value;
                PropertyHasChanged("CreateUserKey");
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return _lastModifiedDate;
            }
            set
            {
                _lastModifiedDate = value;
                PropertyHasChanged("LastModifiedDate");
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                return _lastModifiedUserKey;
            }
            set
            {
                _lastModifiedUserKey = value;
                PropertyHasChanged("LastModifiedUserKey");
            }
        }

        public string Custom1
        {
            get
            {
                return _custom1;
            }
            set
            {
                _custom1 = value;
                PropertyHasChanged("Custom1");
            }
        }

        public string Custom2
        {
            get
            {
                return _custom2;
            }
            set
            {
                _custom2 = value;
                PropertyHasChanged("Custom2");
            }
        }

        public string Custom3
        {
            get
            {
                return _custom3;
            }
            set
            {
                _custom3 = value;
                PropertyHasChanged("Custom3");
            }
        }
        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            ////
            //// MSTFinDetail
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "MSTFinDetail");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MST_FinDetailID", 50));
            ////
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal static MSTFinDetail New()
        {
            MSTFinDetail child = new MSTFinDetail();
            return child;
        }

        internal static MSTFinDetail NewChild()
        {
            MSTFinDetail child = new MSTFinDetail();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTFinDetail Get(SafeDataReader dr)
        {
            MSTFinDetail child = new MSTFinDetail();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTFinDetail Get(int repKey, int repDetKey)
        {
            MSTFinDetail child = new MSTFinDetail();
            child.Fetch(new Criteria(repKey, repDetKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int _repKey = 0;
            public int _repDetKey = 0;
            public int _detType = 0;
            public int _detSeq = 0;
            public decimal _detHeight = 0.0M;
            public bool _firstColumn = false;
            public string _colFormat = string.Empty;
            public string _bodyTextValue = string.Empty;
            public string _bodyTextFormat = string.Empty;
            public int? _rowNo = 0;
            public string _rowSummaryText = string.Empty;
            public bool _rowRevValueForBal = false;
            public bool _rowRevValueForFormula = false;
            public string _rowHide = string.Empty;
            public string _totalText = string.Empty;
            public string _totalFormat = string.Empty;
            public string _totalHide = string.Empty;
            public bool _pageBreak = false;
            public string _custom1 = string.Empty;
            public string _custom2 = string.Empty;
            public string _custom3 = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int repKey, int repDetKey)
            {
                _repKey = repKey;
                _repDetKey = repDetKey;
            }

            internal Criteria(int repKey, int repDetKey, int? Option)
            {
                _repKey = repKey;
                _repDetKey = repDetKey;
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
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_FinDetail_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@RepKey", criteria._repKey);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._repDetKey);


                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        this.Fetch(dr);
                    }

                }// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.                       

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _repKey = dr.GetInt32("RepKey");
            _repDetKey = dr.GetInt32("RepDetKey");
            _detType = dr.GetInt32("DetType");
            _detSeq = dr.GetInt32("DetSeq");
            _detHeight = dr.GetDecimal("DetHeight");
            _firstColumn = dr.GetBoolean("FirstColumn");
            _colFormat = dr.GetString("ColFormat");
            _bodyTextValue = dr.GetString("BodyTextValue");
            _bodyTextFormat = dr.GetString("BodyTextFormat");
            _rowNo = dr.GetInt32("RowNo");
            _rowSummaryText = dr.GetString("RowSummaryText");
            _rowRevValueForBal = dr.GetBoolean("RowRevValueForBal");
            _rowRevValueForFormula = dr.GetBoolean("RowRevValueForFormula");
            _rowHide = dr.GetString("RowHide");
            _totalText = dr.GetString("TotalExp");
            _totalFormat = dr.GetString("TotalFormat");
            _totalHide = dr.GetString("TotalHide");
            _pageBreak = dr.GetBoolean("PageBreak");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(int repKey, int repDetKey)
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, repKey, repDetKey);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               

            return retValue;
        }

        internal bool Insert(SqlConnection cn, int repKey, int repDetKey)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_FinDetail_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewRepKey", repKey);
                cm.Parameters.AddWithValue("@NewRepDetKey", repDetKey);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repDetKey == null)
                    cm.Parameters.AddWithValue("@RepDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepDetKey", _repDetKey);

                if (_detType == null)
                    cm.Parameters.AddWithValue("@DetType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DetType", _detType);

                if (_detSeq == null)
                    cm.Parameters.AddWithValue("@DetSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DetSeq", _detSeq);

                if (_detHeight == null)
                    cm.Parameters.AddWithValue("@DetHeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DetHeight", _detHeight);

                if (_firstColumn == null)
                    cm.Parameters.AddWithValue("@FirstColumn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FirstColumn", _firstColumn);

                if (_colFormat == null)
                    cm.Parameters.AddWithValue("@ColFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColFormat", _colFormat);

                if (_bodyTextValue == null)
                    cm.Parameters.AddWithValue("@BodyTextValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BodyTextValue", _bodyTextValue);

                if (_bodyTextFormat == null)
                    cm.Parameters.AddWithValue("@BodyTextFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BodyTextFormat", _bodyTextFormat);

                if (_rowNo == null)
                    cm.Parameters.AddWithValue("@RowNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowNo", _rowNo);

                if (_rowSummaryText == null)
                    cm.Parameters.AddWithValue("@RowSummaryText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowSummaryText", _rowSummaryText);

                if (_rowRevValueForBal == null)
                    cm.Parameters.AddWithValue("@RowRevValueForBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowRevValueForBal", _rowRevValueForBal);

                if (_rowRevValueForFormula == null)
                    cm.Parameters.AddWithValue("@RowRevValueForFormula", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowRevValueForFormula", _rowRevValueForFormula);

                if (_rowHide == null)
                    cm.Parameters.AddWithValue("@RowHide", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowHide", _rowHide);

                if (_totalText == null)
                    cm.Parameters.AddWithValue("@TotalExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalExp", _totalText);

                if (_totalFormat == null)
                    cm.Parameters.AddWithValue("@TotalFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalFormat", _totalFormat);

                if (_totalHide == null)
                    cm.Parameters.AddWithValue("@TotalHide", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalHide", _totalHide);

                if (_pageBreak == null)
                    cm.Parameters.AddWithValue("@PageBreak", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PageBreak", _pageBreak);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate);

                if (_lastModifiedUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

                if (_custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _custom1);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _custom2);

                if (_custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _custom3);

                cm.Parameters["@NewRepKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewRepDetKey"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                repKey = (int)cm.Parameters["@NewRepKey"].Value;
                repDetKey = (int)cm.Parameters["@NewRepDetKey"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.            

            return retValue;
        }

        #endregion //Data Access - Insert

        #region Data Access - Update

        internal bool Update()
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue = this.Update(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {


            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_FinDetail_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewRepKey", 0);
                cm.Parameters.AddWithValue("@NewRepDetKey", 0);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repDetKey == null)
                    cm.Parameters.AddWithValue("@RepDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepDetKey", _repDetKey);

                if (_detType == null)
                    cm.Parameters.AddWithValue("@DetType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DetType", _detType);

                if (_detSeq == null)
                    cm.Parameters.AddWithValue("@DetSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DetSeq", _detSeq);

                if (_detHeight == null)
                    cm.Parameters.AddWithValue("@DetHeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DetHeight", _detHeight);

                if (_firstColumn == null)
                    cm.Parameters.AddWithValue("@FirstColumn", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@FirstColumn", _firstColumn);

                if (_colFormat == null)
                    cm.Parameters.AddWithValue("@ColFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColFormat", _colFormat);

                if (_bodyTextValue == null)
                    cm.Parameters.AddWithValue("@BodyTextValue", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BodyTextValue", _bodyTextValue);

                if (_bodyTextFormat == null)
                    cm.Parameters.AddWithValue("@BodyTextFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@BodyTextFormat", _bodyTextFormat);

                if (_rowNo == null)
                    cm.Parameters.AddWithValue("@RowNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowNo", _rowNo);

                if (_rowSummaryText == null)
                    cm.Parameters.AddWithValue("@RowSummaryText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowSummaryText", _rowSummaryText);

                if (_rowRevValueForBal == null)
                    cm.Parameters.AddWithValue("@RowRevValueForBal", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowRevValueForBal", _rowRevValueForBal);

                if (_rowRevValueForFormula == null)
                    cm.Parameters.AddWithValue("@RowRevValueForFormula", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowRevValueForFormula", _rowRevValueForFormula);

                if (_rowHide == null)
                    cm.Parameters.AddWithValue("@RowHide", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowHide", _rowHide);

                if (_totalText == null)
                    cm.Parameters.AddWithValue("@TotalExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalExp", _totalText);

                if (_totalFormat == null)
                    cm.Parameters.AddWithValue("@TotalFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalFormat", _totalFormat);

                if (_totalHide == null)
                    cm.Parameters.AddWithValue("@TotalHide", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalHide", _totalHide);

                if (_pageBreak == null)
                    cm.Parameters.AddWithValue("@PageBreak", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@PageBreak", _pageBreak);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate);

                if (_lastModifiedUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

                if (_custom1 == null)
                    cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom1", _custom1);

                if (_custom2 == null)
                    cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom2", _custom2);

                if (_custom3 == null)
                    cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Custom3", _custom3);

                cm.Parameters["@NewRepKey"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewRepDetKey"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

        }
        #endregion //Data Access - Update

        #region Data Access - Delete

        internal bool Delete(Criteria criteria)
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call delete method.
                    retValue = this.Delete(cn, criteria);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_FinDetail_Delete";

                cm.Parameters.AddWithValue("@RepKey", 0);
                cm.Parameters.AddWithValue("@RepDetKey", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();



                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

        }

        #endregion //Data Access - Delete

        #region Data Access - Validation

        internal bool Validation(Criteria criteria, bool isNew)
        {
            bool retValue = false;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call validation method.
                    retValue = this.Validation(cn, criteria, isNew);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope             

            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_FinDetail_Validation";


                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@RepKey", 0);
                cm.Parameters.AddWithValue("@RepDetKey", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }

        }
        #endregion //Data Access - Validation
    }
}
