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
    public class MSTTimesheet : Csla.BusinessBase<MSTTimesheet>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _supervisorKey = null;
        internal int? _emKey = null;
        internal DateTime? _period = null;
        internal int? _overHeadKey = null;
        internal int? _itmKey = null;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        //internal string _custom1 = string.Empty;
        //internal string _custom2 = string.Empty;
        //internal string _custom3 = string.Empty;

        public int? SupervisorKey
        {
            get
            {
                CanReadProperty("SupervisorKey", true);
                return _supervisorKey;
            }
            set
            {
                _supervisorKey = value;
                PropertyHasChanged("SuperVisorKey");
            }
        }

        public int? EmKey
        {
            get
            {
                CanReadProperty("EmKey", true);
                return _emKey;
            }
        }

        public DateTime? Period
        {
            get
            {
                CanReadProperty("Period", true);
                return _period;
            }
            set
            {
                _period = value;
            }
        }

        public int? OverHeadKey
        {
            get
            {
                CanReadProperty("OverHeadKey", true);
                return _overHeadKey;
            }
        }

        public int? ItmKey
        {
            get
            {
                CanReadProperty("ItmKey", true);
                return _itmKey;
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                CanReadProperty("CreateDate", true);
                return _createDate;
            }
            set
            {
                _createDate = value;
            }
        }

        public int? CreateUserKey
        {
            get
            {
                CanReadProperty("CreateUserKey", true);
                return _createUserKey;
            }
            set
            {
                _createUserKey = value;
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                CanReadProperty("LastModifiedDate", true);
                return _lastModifiedDate;
            }
            set
            {
                _lastModifiedDate = value;
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                CanReadProperty("LastModifiedUserKey", true);
                return _lastModifiedUserKey;
            }
            set
            {
                _lastModifiedUserKey = value;
            }
        }

        //public string Custom1
        //{
        //    get
        //    {
        //        CanReadProperty("Custom1", true);
        //        return _custom1;
        //    }
        //    set
        //    {
        //        CanWriteProperty("Custom1", true);
        //        if (value == null) value = string.Empty;

        //            _custom1 = value;
        //            PropertyHasChanged("Custom1");

        //    }
        //}

        //public string Custom2
        //{
        //    get
        //    {
        //        CanReadProperty("Custom2", true);
        //        return _custom2;
        //    }
        //    set
        //    {
        //        CanWriteProperty("Custom2", true);
        //        if (value == null) value = string.Empty;

        //            _custom2 = value;
        //            PropertyHasChanged("Custom2");

        //    }
        //}

        //public string Custom3
        //{
        //    get
        //    {
        //        CanReadProperty("Custom3", true);
        //        return _custom3;
        //    }
        //    set
        //    {
        //        CanWriteProperty("Custom3", true);
        //        if (value == null) value = string.Empty;

        //            _custom3 = value;
        //            PropertyHasChanged("Custom3");

        //    }
        //}

        protected override object GetIdValue()
        {
            return _emKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            //
            // BranchID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "BranchID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BranchID", 50));
            ////
            //// BranchNm
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "BranchNm");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BranchNm", 255));
            ////
            //// Custom1
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
            ////
            //// Custom2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
            ////
            //// Custom3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
            ////
            //// Custom4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom4", 255));
            ////
            //// Custom5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom5", 255));
        }

        protected override void AddBusinessRules()
        {
            //AddCommonRules();
            //AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        public MSTTimesheet()
        { /* require use of factory method */ }

        public static MSTTimesheet New()
        {
            MSTTimesheet child = new MSTTimesheet();
            return child;
        }

        public static MSTTimesheet NewChild()
        {
            MSTTimesheet child = new MSTTimesheet();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        public static MSTTimesheet Get(SafeDataReader dr)
        {

            MSTTimesheet child = new MSTTimesheet();
            child.MarkAsChild();

            return child;
        }

        public static MSTTimesheet Get(int? emKey)
        {

            MSTTimesheet child = new MSTTimesheet();
            child.Fetch(new Criteria(emKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _emKey = null;
            public int? _period = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? EMKey, int? Option)
            {
                _emKey = EMKey;
                _option = Option;
            }
            internal Criteria(int? EMKey, int? Period, int? Option)
            {
                _emKey = EMKey;
                _period = Period;
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
                cm.CommandText = "MSTSalesRep_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EMKey", criteria._emKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    else
                        this.Clear();

                }	// Already close and dispose data reader.

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
                else
                {
                    retValue = false;
                }

            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _emKey = dr.GetInt32("EMKey");
            _itmKey = dr.GetInt32("JobLabourItmKey");
            _overHeadKey = dr.GetInt32("JobCostGrp");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            ValidationRules.CheckRules();

            return true;

        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? branchKey)
        {
            bool retValue = false;

            branchKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out branchKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? branchKey)
        {
            bool retValue = false;

            branchKey = 0;
            //try
            //{
            //    // Using existing sql connection.
            //    using (SqlCommand cm = cn.CreateCommand())
            //    {
            //        cm.CommandType = CommandType.StoredProcedure;
            //        cm.CommandText = "MSTTimesheet_AddUpdate";

            //        cm.Parameters.AddWithValue("@Option", 0);
            //        

            //        cm.Parameters.AddWithValue("@NewBranchKey", branchKey);

            //        if (_branchKey == null)
            //            cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@BranchKey", _branchKey);

            //        if (_branchID == null)
            //            cm.Parameters.AddWithValue("@BranchID", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@BranchID", _branchID);

            //        if (_branchNm == null)
            //            cm.Parameters.AddWithValue("@BranchNm", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@BranchNm", _branchNm);

            //        if (_inactive == null)
            //            cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Inactive", _inactive);

            //        if (_createDate == null)
            //            cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

            //        if (AppInfor.currentUserKey == null)
            //            cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

            //        if (_lastModifiedDate == null)
            //            cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

            //        if (_lastModifiedUserKey == null)
            //            cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

            //        if (_custom1 == null)
            //            cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom1", _custom1);

            //        if (_custom2 == null)
            //            cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom2", _custom2);

            //        if (_custom3 == null)
            //            cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom3", _custom3);

            //        if (_custom4 == null)
            //            cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom4", _custom4);

            //        if (_custom5 == null)
            //            cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom5", _custom5);

            //        
            //        cm.Parameters["@NewBranchKey"].Direction = ParameterDirection.Output;

            //        cm.Parameters.AddWithValue("@RetValue", 0);
            //        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            //        cm.ExecuteNonQuery();

            //        msgID = cm.Parameters["@MsgID"].Value.ToString();
            //        branchKey = (int)cm.Parameters["@NewBranchKey"].Value;

            //        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //            retValue = true;           
            //    }// Already close and dispose sql connection.
            //}
            //catch (Exception ex)
            //{
            //    retValue = false;
            //}
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            bool retValue = false;

            //try
            //{
            //    // Using existing sql connection.
            //    using (SqlCommand cm = cn.CreateCommand())
            //    {
            //        cm.CommandType = CommandType.StoredProcedure;
            //        cm.CommandText = "MSTTimesheet_AddUpdate";

            //        cm.Parameters.AddWithValue("@Option", 1);
            //        
            //        cm.Parameters.AddWithValue("@NewBranchKey", 0);

            //        if (_branchKey == null)
            //            cm.Parameters.AddWithValue("@BranchKey", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@BranchKey", _branchKey);

            //        if (_branchID == null)
            //            cm.Parameters.AddWithValue("@BranchID", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@BranchID", _branchID);

            //        if (_branchNm == null)
            //            cm.Parameters.AddWithValue("@BranchNm", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@BranchNm", _branchNm);

            //        if (_inactive == null)
            //            cm.Parameters.AddWithValue("@Inactive", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Inactive", _inactive);

            //        if (_createDate == null)
            //            cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

            //        if (AppInfor.currentUserKey == null)
            //            cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

            //        if (_lastModifiedDate == null)
            //            cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

            //        if (_lastModifiedUserKey == null)
            //            cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@LastModifiedUserKey", _lastModifiedUserKey);

            //        if (_custom1 == null)
            //            cm.Parameters.AddWithValue("@Custom1", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom1", _custom1);

            //        if (_custom2 == null)
            //            cm.Parameters.AddWithValue("@Custom2", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom2", _custom2);

            //        if (_custom3 == null)
            //            cm.Parameters.AddWithValue("@Custom3", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom3", _custom3);

            //        if (_custom4 == null)
            //            cm.Parameters.AddWithValue("@Custom4", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom4", _custom4);

            //        if (_custom5 == null)
            //            cm.Parameters.AddWithValue("@Custom5", DBNull.Value);
            //        else
            //            cm.Parameters.AddWithValue("@Custom5", _custom5);

            //        
            //        cm.Parameters["@NewBranchKey"].Direction = ParameterDirection.Output;

            //        cm.Parameters.AddWithValue("@RetValue", 0);
            //        cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            //        cm.ExecuteNonQuery();


            //        if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
            //            retValue = true;           
            //    }// Already close and dispose sql connection.
            //}
            //catch (Exception ex)
            //{
            //    retValue = false;
            //}
            return retValue;
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            string msgID = "";
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTTimesheet_Delete";


                cm.Parameters.AddWithValue("@EMKey", criteria._emKey);
                cm.Parameters.AddWithValue("@Period", criteria._period);



                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }// Already close and dispose sql connection.
            return retValue;
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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            string msgID = "";
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTTimesheet_Validation";

                cm.Parameters.AddWithValue("@isNew", isNew);

                cm.Parameters.AddWithValue("@EMKey", criteria._emKey);



                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion //Data Access - Validation

        private void Clear()
        {
            _supervisorKey = null;
            _emKey = null;
            _period = null;
            _overHeadKey = null;
            _itmKey = null;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;

        }


    }
}

