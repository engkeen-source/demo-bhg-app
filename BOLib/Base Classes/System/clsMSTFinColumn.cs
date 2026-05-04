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
    public class MSTFinColumn : Csla.BusinessBase<MSTFinColumn>
    {
        #region Business Properties and Methods

        //declare members
        internal int _repKey = 0;
        internal int _repDetKey = 0;
        internal short _colNo = 0;
        internal int _colType = 0;
        internal string _colText = string.Empty;
        internal bool _colDisplay = false;
        internal decimal _colWidth = 0.0M;
        internal string _colDetailFormat = string.Empty;
        internal string _colBalanceExp = string.Empty;
        internal string _colFormulaExp = string.Empty;
        internal bool _colIgnoreRowReverse = false;
        internal string _totalExp = string.Empty;
        internal int _colBranchKey = -1;
        internal int _colDeptKey = -1;
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

        public short ColNo
        {
            get
            {
                return _colNo;
            }
            set
            {
                _colNo = value;
                PropertyHasChanged("ColNo");
            }
        }

        public int ColType
        {
            get
            {
                return _colType;
            }
            set
            {
                _colType = value;
                PropertyHasChanged("ColType");
            }
        }

        public string ColText
        {
            get
            {
                return _colText;
            }
            set
            {
                _colText = value;
                PropertyHasChanged("ColText");
            }
        }

        public bool ColDisplay
        {
            get
            {
                return _colDisplay;
            }
            set
            {
                _colDisplay = value;
                PropertyHasChanged("ColDisplay");
            }
        }

        public decimal ColWidth
        {
            get
            {
                return _colWidth;
            }
            set
            {
                _colWidth = value;
                PropertyHasChanged("ColWidth");
            }
        }

        public string ColDetailFormat
        {
            get
            {
                return _colDetailFormat;
            }
            set
            {
                _colDetailFormat = value;
                PropertyHasChanged("ColDetailFormat");
            }
        }

        public string ColBalanceExp
        {
            get
            {
                return _colBalanceExp;
            }
            set
            {
                _colBalanceExp = value;
                PropertyHasChanged("ColBalanceExp");
            }
        }

        public string ColFormulaExp
        {
            get
            {
                return _colFormulaExp;
            }
            set
            {
                _colFormulaExp = value;
                PropertyHasChanged("ColFormulaExp");
            }
        }

        public bool ColIgnoreRowReverse
        {
            get
            {
                return _colIgnoreRowReverse;
            }
            set
            {
                _colIgnoreRowReverse = value;
                PropertyHasChanged("ColIgnoreRowReverse");
            }
        }

        public string TotalExp
        {
            get
            {
                return _totalExp;
            }
            set
            {
                _totalExp = value;
                PropertyHasChanged("TotalExp");
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

        public int ColBranchKey
        {
            get
            {
                return _colBranchKey;
            }
            set
            {
                _colBranchKey = value;
                PropertyHasChanged("ColBranchKey");
            }
        }

        public int ColDeptKey
        {
            get
            {
                return _colDeptKey;
            }
            set
            {
                _colDeptKey = value;
                PropertyHasChanged("ColDeptKey");
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
            //// MSTFinColumn
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "MSTFinColumn");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MST_FinColumnID", 50));
            ////
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTFinColumn()
        { /* require use of factory method */ }

        internal static MSTFinColumn New()
        {
            MSTFinColumn child = new MSTFinColumn();
            return child;
        }

        internal static MSTFinColumn NewChild()
        {
            MSTFinColumn child = new MSTFinColumn();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTFinColumn Get(SafeDataReader dr)
        {
            MSTFinColumn child = new MSTFinColumn();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTFinColumn Get(int repKey, int repDetKey, short colNo)
        {
            MSTFinColumn child = new MSTFinColumn();
            child.Fetch(new Criteria(repKey, repDetKey, colNo, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int _repKey = 0;
            public int _repDetKey = 0;
            public short _colNo = 0;
            public int _colType = 0;
            public string _colText = string.Empty;
            public bool _colDisplay = false;
            public decimal _colWidth = 0.0M;
            public string _colDetailFormat = string.Empty;
            public string _colBalanceExp = string.Empty;
            public string _colFormulaExp = string.Empty;
            public bool _colIgnoreRowReverse = false;
            public string _totalExp = string.Empty;
            public string _custom1 = string.Empty;
            public string _custom2 = string.Empty;
            public string _custom3 = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int repKey, int repDetKey, short colNo)
            {
                _repKey = repKey;
                _repDetKey = repDetKey;
                _colNo = colNo;
            }

            internal Criteria(int repKey, int repDetKey, short colNo, int? Option)
            {
                _repKey = repKey;
                _repDetKey = repDetKey;
                _colNo = colNo;
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
                cm.CommandText = "MST_FinColumn_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RepKey", criteria._repKey);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._repDetKey);
                cm.Parameters.AddWithValue("@ColNo", criteria._colNo);


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
            _colNo = dr.GetInt16("ColNo");
            _colType = dr.GetInt32("ColType");
            _colText = dr.GetString("ColText");
            _colDisplay = dr.GetBoolean("ColDisplay");
            _colWidth = dr.GetDecimal("ColWidth");
            _colDetailFormat = dr.GetString("ColDetailFormat");
            _colBalanceExp = dr.GetString("ColBalanceExp");
            _colFormulaExp = dr.GetString("ColFormulaExp");
            _colIgnoreRowReverse = dr.GetBoolean("ColIgnoreRowReverse");
            _totalExp = dr.GetString("TotalExp");
            _colBranchKey = dr.GetInt32("ColBranchKey");
            _colDeptKey = dr.GetInt32("ColDeptKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(int repKey, int repDetKey, short colNo)
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
                    retValue = this.Insert(cn, repKey, repDetKey, colNo);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               

            return retValue;
        }

        internal bool Insert(SqlConnection cn, int repKey, int repDetKey, int colNo)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MST_FinColumn_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewRepKey", repKey);
                cm.Parameters.AddWithValue("@NewRepDetKey", repDetKey);
                cm.Parameters.AddWithValue("@NewColNo", colNo);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repDetKey == null)
                    cm.Parameters.AddWithValue("@RepDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepDetKey", _repDetKey);

                if (_colNo == null)
                    cm.Parameters.AddWithValue("@ColNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColNo", _colNo);

                if (_colType == null)
                    cm.Parameters.AddWithValue("@ColType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColType", _colType);

                if (_colText == null)
                    cm.Parameters.AddWithValue("@ColText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColText", _colText);

                if (_colDisplay == null)
                    cm.Parameters.AddWithValue("@ColDisplay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColDisplay", _colDisplay);

                if (_colWidth == null)
                    cm.Parameters.AddWithValue("@ColWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColWidth", _colWidth);

                if (_colDetailFormat == null)
                    cm.Parameters.AddWithValue("@ColDetailFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColDetailFormat", _colDetailFormat);

                if (_colBalanceExp == null)
                    cm.Parameters.AddWithValue("@ColBalanceExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColBalanceExp", _colBalanceExp);

                if (_colFormulaExp == null)
                    cm.Parameters.AddWithValue("@ColFormulaExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColFormulaExp", _colFormulaExp);

                if (_colIgnoreRowReverse == null)
                    cm.Parameters.AddWithValue("@ColIgnoreRowReverse", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColIgnoreRowReverse", _colIgnoreRowReverse);

                if (_totalExp == null)
                    cm.Parameters.AddWithValue("@TotalExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalExp", _totalExp);

                if (_colBranchKey == null)
                    cm.Parameters.AddWithValue("@ColBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColBranchKey", _colBranchKey);

                if (_colDeptKey == null)
                    cm.Parameters.AddWithValue("@ColDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColDeptKey", _colDeptKey);

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
                cm.Parameters["@NewColNo"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                repKey = (int)cm.Parameters["@NewRepKey"].Value;
                repDetKey = (int)cm.Parameters["@NewRepDetKey"].Value;
                colNo = (int)cm.Parameters["@NewColNo"].Value;

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
                cm.CommandText = "MST_FinColumn_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewRepKey", 0);
                cm.Parameters.AddWithValue("@NewRepDetKey", 0);
                cm.Parameters.AddWithValue("@NewColNo", 0);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repDetKey == null)
                    cm.Parameters.AddWithValue("@RepDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepDetKey", _repDetKey);

                if (_colNo == null)
                    cm.Parameters.AddWithValue("@ColNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColNo", _colNo);

                if (_colType == null)
                    cm.Parameters.AddWithValue("@ColType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColType", _colType);

                if (_colText == null)
                    cm.Parameters.AddWithValue("@ColText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColText", _colText);

                if (_colDisplay == null)
                    cm.Parameters.AddWithValue("@ColDisplay", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColDisplay", _colDisplay);

                if (_colWidth == null)
                    cm.Parameters.AddWithValue("@ColWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColWidth", _colWidth);

                if (_colDetailFormat == null)
                    cm.Parameters.AddWithValue("@ColDetailFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColDetailFormat", _colDetailFormat);

                if (_colBalanceExp == null)
                    cm.Parameters.AddWithValue("@ColBalanceExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColBalanceExp", _colBalanceExp);

                if (_colFormulaExp == null)
                    cm.Parameters.AddWithValue("@ColFormulaExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColFormulaExp", _colFormulaExp);

                if (_colIgnoreRowReverse == null)
                    cm.Parameters.AddWithValue("@ColIgnoreRowReverse", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColIgnoreRowReverse", _colIgnoreRowReverse);

                if (_totalExp == null)
                    cm.Parameters.AddWithValue("@TotalExp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TotalExp", _totalExp);

                if (_colBranchKey == null)
                    cm.Parameters.AddWithValue("@ColBranchKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColBranchKey", _colBranchKey);

                if (_colDeptKey == null)
                    cm.Parameters.AddWithValue("@ColDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColDeptKey", _colDeptKey);

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
                cm.Parameters["@NewColNo"].Direction = ParameterDirection.InputOutput;

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
                cm.CommandText = "MST_FinColumn_Delete";

                cm.Parameters.AddWithValue("@RepKey", 0);
                cm.Parameters.AddWithValue("@RepDetKey", 0);
                cm.Parameters.AddWithValue("@ColNo", 0);

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
                cm.CommandText = "MST_FinColumn_Validation";


                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@RepKey", 0);
                cm.Parameters.AddWithValue("@RepDetKey", 0);
                cm.Parameters.AddWithValue("@ColNo", 0);

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
