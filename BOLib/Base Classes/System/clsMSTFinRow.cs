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
    public class MSTFinRow : Csla.BusinessBase<MSTFinRow>
    {
        #region Business Properties and Methods

        //declare members
        internal int _repKey = 0;
        internal int _repDetKey = 0;
        internal int _rowNo = 0;
        internal int _rowSeq = 0;
        internal int _rowAccTypeKey = 0;
        internal int? _rowAccGrpKey = 0;
        internal string _rowAccF = string.Empty;
        internal string _rowAccT = string.Empty;
        internal string _rowDeptF = string.Empty;
        internal string _rowDeptT = string.Empty;
        internal string _rowBranchF = string.Empty;
        internal string _rowBranchT = string.Empty;
        internal string _rowRangeFilter = string.Empty;
        internal int _rowDisplayType = 0;
        internal string _lineSummaryText = string.Empty;
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

        public int RowNo
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

        public int RowSeq
        {
            get
            {
                return _rowSeq;
            }            
            set
            {
                _rowSeq = value;
                PropertyHasChanged("RowSeq");
            }
        }

        public int RowAccTypeKey
        {
            get
            {
                return _rowAccTypeKey;
            }            
            set
            {
                _rowAccTypeKey = value;
                PropertyHasChanged("RowAccTypeKey");
            }
        }

        public int? RowAccGrpKey
        {
            get
            {
                return _rowAccGrpKey;
            }            
            set
            {
                _rowAccGrpKey = value;
                PropertyHasChanged("RowAccGrpKey");
            }
        }

        public string RowAccF
        {
            get
            {
                return _rowAccF;
            }            
            set
            {
                _rowAccF = value;
                PropertyHasChanged("RowAccF");
            }
        }

        public string RowAccT
        {
            get
            {
                return _rowAccT;
            }            
            set
            {
                _rowAccT = value;
                PropertyHasChanged("RowAccT");
            }
        }

        public string RowDeptF
        {
            get
            {
                return _rowDeptF;
            }            
            set
            {
                _rowDeptF = value;
                PropertyHasChanged("RowDeptF");
            }
        }

        public string RowDeptT
        {
            get
            {
                return _rowDeptT;
            }            
            set
            {
                _rowDeptT = value;
                PropertyHasChanged("RowDeptT");
            }
        }

        public string RowBranchF
        {
            get
            {
                return _rowBranchF;
            }            
            set
            {
                _rowBranchF = value;
                PropertyHasChanged("RowBranchF");
            }
        }

        public string RowBranchT
        {
            get
            {
                return _rowBranchT;
            }            
            set
            {
                _rowBranchT = value;
                PropertyHasChanged("RowBranchT");
            }
        }

        public string RowRangeFilter
        {
            get
            {
                return _rowRangeFilter;
            }            
            set
            {
                _rowRangeFilter = value;
                PropertyHasChanged("RowRangeFilter");
            }
        }

        public int RowDisplayType
        {
            get
            {
                return _rowDisplayType;
            }            
            set
            {
                _rowDisplayType = value;
                PropertyHasChanged("RowDisplayType");
            }
        }

        public string LineSummaryText
        {
            get
            {
                return _lineSummaryText;
            }            
            set
            {
                _lineSummaryText = value;
                PropertyHasChanged("LineSummaryText");
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
            //// MSTFinRow
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "MSTFinRow");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MSTFinRowID", 50));
            ////
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTFinRow()
        { /* require use of factory method */ }

        internal static MSTFinRow New()
        {
            MSTFinRow child = new MSTFinRow();         
            return child;
        }

        internal static MSTFinRow NewChild()
        {
            MSTFinRow child = new MSTFinRow();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTFinRow Get(SafeDataReader dr)
        {
            MSTFinRow child = new MSTFinRow();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTFinRow Get(int repKey,int repDetKey,int rowNo,int rowSeq)
        {
            MSTFinRow child = new MSTFinRow();
            child.Fetch(new Criteria(repKey,repDetKey,rowNo,rowSeq, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
        public int _repKey = 0;
        public int _repDetKey = 0;
        public int _rowNo = 0;
        public int _rowSeq = 0;
        public int _rowAccTypeKey = 0;
        public int? _rowAccGrpKey = 0;
        public string _rowAccF = string.Empty;
        public string _rowAccT = string.Empty;
        public string _rowDeptF = string.Empty;
        public string _rowDeptT = string.Empty;
        public string _rowBranchF = string.Empty;
        public string _rowBranchT = string.Empty;
        public string _rowRangeFilter = string.Empty;
        public int _rowDisplayType = 0;
        public string _lineSummaryText = string.Empty;
        public string _custom1 = string.Empty;
        public string _custom2 = string.Empty;
        public string _custom3 = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int repKey,int repDetKey,int rowNo,int rowSeq)
            {
                _repKey = repKey;
                _repDetKey = repDetKey;
                _rowNo = rowNo;
                _rowSeq = rowSeq;
            }

            internal Criteria(int repKey,int repDetKey,int rowNo,int rowSeq, int? Option)
            {
                _repKey = repKey;
                _repDetKey = repDetKey;
                _rowNo = rowNo;
                _rowSeq = rowSeq;
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
                retValue=this.Fetch(cn, criteria);                           
            }
             
            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue=false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTFinRow_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RepKey", criteria._repKey);
                cm.Parameters.AddWithValue("@RepDetKey", criteria._repDetKey);
                cm.Parameters.AddWithValue("@RowNo", criteria._rowNo);
                cm.Parameters.AddWithValue("@RowSeq", criteria._rowSeq);

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
            _repKey =  dr.GetInt32("RepKey");
            _repDetKey =  dr.GetInt32("RepDetKey");
            _rowNo =  dr.GetInt32("RowNo");
            _rowSeq =  dr.GetInt32("RowSeq");
            _rowAccTypeKey =  dr.GetInt32("RowAccTypeKey");
            _rowAccGrpKey =  dr.GetInt32("RowAccGrpKey");
            _rowAccF =  dr.GetString("RowAccF");
            _rowAccT =  dr.GetString("RowAccT");
            _rowDeptF =  dr.GetString("RowDeptF");
            _rowDeptT =  dr.GetString("RowDeptT");
            _rowBranchF =  dr.GetString("RowBranchF");
            _rowBranchT =  dr.GetString("RowBranchT");
            _rowRangeFilter =  dr.GetString("RowRangeFilter");
            _rowDisplayType =  dr.GetInt32("RowDisplayType");
            _lineSummaryText =  dr.GetString("LineSummaryText");
            _custom1 =  dr.GetString("Custom1");
            _custom2 =  dr.GetString("Custom2");
            _custom3 =  dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(int repKey,int repDetKey,int rowNo,short rowSeq)
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
                    retValue= this.Insert(cn,repKey,repDetKey,rowNo,rowSeq);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope               
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn,int repKey,int repDetKey,int rowNo,short rowSeq)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTFinRow_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewRepKey", repKey);
                cm.Parameters.AddWithValue("@NewRepDetKey", repDetKey);
                cm.Parameters.AddWithValue("@NewRowNo", rowNo);
                cm.Parameters.AddWithValue("@NewRowSeq", rowSeq);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repDetKey == null)
                    cm.Parameters.AddWithValue("@RepDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepDetKey", _repDetKey);

                if (_rowNo == null)
                    cm.Parameters.AddWithValue("@RowNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowNo", _rowNo);

                if (_rowSeq == null)
                    cm.Parameters.AddWithValue("@RowSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowSeq", _rowSeq);

                if (_rowAccTypeKey == null)
                    cm.Parameters.AddWithValue("@RowAccTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccTypeKey", _rowAccTypeKey);

                if (_rowAccGrpKey == null)
                    cm.Parameters.AddWithValue("@RowAccGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccGrpKey", _rowAccGrpKey);

                if (_rowAccF == null)
                    cm.Parameters.AddWithValue("@RowAccF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccF", _rowAccF);

                if (_rowAccT == null)
                    cm.Parameters.AddWithValue("@RowAccT", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccT", _rowAccT);

                if (_rowDeptF == null)
                    cm.Parameters.AddWithValue("@RowDeptF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowDeptF", _rowDeptF);

                if (_rowDeptT == null)
                    cm.Parameters.AddWithValue("@RowDeptT", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowDeptT", _rowDeptT);

                if (_rowBranchF == null)
                    cm.Parameters.AddWithValue("@RowBranchF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowBranchF", _rowBranchF);

                if (_rowBranchT == null)
                    cm.Parameters.AddWithValue("@RowBranchT", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowBranchT", _rowBranchT);

                if (_rowRangeFilter == null)
                    cm.Parameters.AddWithValue("@RowRangeFilter", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowRangeFilter", _rowRangeFilter);

                if (_rowDisplayType == null)
                    cm.Parameters.AddWithValue("@RowDisplayType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowDisplayType", _rowDisplayType);

                if (_lineSummaryText == null)
                    cm.Parameters.AddWithValue("@LineSummaryText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LineSummaryText", _lineSummaryText);

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
                cm.Parameters["@NewRowNo"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewRowSeq"].Direction = ParameterDirection.InputOutput;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();
                
                repKey = GFunc.NEInt(cm.Parameters["@NewRepKey"].Value,0);
                repDetKey = GFunc.NEInt(cm.Parameters["@NewRepDetKey"].Value,0);
                rowNo = GFunc.NEInt(cm.Parameters["@NewRowNo"].Value,0);
                rowSeq = (Int16)cm.Parameters["@NewRowSeq"].Value;

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
            bool retValue=false;
            
                // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue=this.Update(cn);
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
                cm.CommandText = "MSTFinRow_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewRepKey", 0);
                cm.Parameters.AddWithValue("@NewRepDetKey", 0);
                cm.Parameters.AddWithValue("@NewRowNo", 0);
                cm.Parameters.AddWithValue("@NewRowSeq", 0);

                if (_repKey == null)
                    cm.Parameters.AddWithValue("@RepKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepKey", _repKey);

                if (_repDetKey == null)
                    cm.Parameters.AddWithValue("@RepDetKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RepDetKey", _repDetKey);

                if (_rowNo == null)
                    cm.Parameters.AddWithValue("@RowNo", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowNo", _rowNo);

                if (_rowSeq == null)
                    cm.Parameters.AddWithValue("@RowSeq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowSeq", _rowSeq);

                if (_rowAccTypeKey == null)
                    cm.Parameters.AddWithValue("@RowAccTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccTypeKey", _rowAccTypeKey);

                if (_rowAccGrpKey == null)
                    cm.Parameters.AddWithValue("@RowAccGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccGrpKey", _rowAccGrpKey);

                if (_rowAccF == null)
                    cm.Parameters.AddWithValue("@RowAccF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccF", _rowAccF);

                if (_rowAccT == null)
                    cm.Parameters.AddWithValue("@RowAccT", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowAccT", _rowAccT);

                if (_rowDeptF == null)
                    cm.Parameters.AddWithValue("@RowDeptF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowDeptF", _rowDeptF);

                if (_rowDeptT == null)
                    cm.Parameters.AddWithValue("@RowDeptT", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowDeptT", _rowDeptT);

                if (_rowBranchF == null)
                    cm.Parameters.AddWithValue("@RowBranchF", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowBranchF", _rowBranchF);

                if (_rowBranchT == null)
                    cm.Parameters.AddWithValue("@RowBranchT", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowBranchT", _rowBranchT);

                if (_rowRangeFilter == null)
                    cm.Parameters.AddWithValue("@RowRangeFilter", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowRangeFilter", _rowRangeFilter);

                if (_rowDisplayType == null)
                    cm.Parameters.AddWithValue("@RowDisplayType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowDisplayType", _rowDisplayType);

                if (_lineSummaryText == null)
                    cm.Parameters.AddWithValue("@LineSummaryText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LineSummaryText", _lineSummaryText);

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
                cm.Parameters["@NewRowNo"].Direction = ParameterDirection.InputOutput;
                cm.Parameters["@NewRowSeq"].Direction = ParameterDirection.InputOutput;

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
                   retValue= this.Delete(cn, criteria);
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
                cm.CommandText = "MSTFinRow_Delete";

                cm.Parameters.AddWithValue("@RepKey", 0);
                cm.Parameters.AddWithValue("@RepDetKey", 0);
                cm.Parameters.AddWithValue("@RowNo", 0);
                cm.Parameters.AddWithValue("@RowSeq", 0);

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

        internal bool Validation(Criteria criteria,bool isNew)
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
                    retValue= this.Validation(cn, criteria, isNew);
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
                cm.CommandText = "MSTFinRow_Validation";

               
                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@RepKey", 0);
                cm.Parameters.AddWithValue("@RepDetKey", 0);
                cm.Parameters.AddWithValue("@RowNo", 0);
                cm.Parameters.AddWithValue("@RowSeq", 0);

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
