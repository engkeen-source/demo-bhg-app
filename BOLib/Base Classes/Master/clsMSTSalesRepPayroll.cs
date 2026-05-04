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
    public class MSTSalesRepPayRoll : Csla.BusinessBase<MSTSalesRepPayRoll>, System.ComponentModel.IDataErrorInfo
    {
        #region Business Properties and Methods

        //declare members
        internal int? _eMKey = null;
        internal int? _transKey = null;
        internal int? _transType = 0;
        internal DateTime? _transDate = null;
        internal string _transDes = string.Empty;
        internal decimal? _transAmt = null;
        internal int? _transDeptKey = 0;
        internal int? _transGrpKey = 0;
        internal string _error = string.Empty;

        public int? EMKey
        {
            get
            {
                return _eMKey;
            }
            set
            {
                _eMKey = value;
                PropertyHasChanged("EMKey");
            }
        }

        public int? TransKey
        {
            get
            {
                return _transKey;
            }
            set
            {
                _transKey = value;
                PropertyHasChanged("TransKey");
            }
        }

        public int? TransType
        {
            get
            {
                return _transType;
            }
            set
            {
                _transType = value;
                PropertyHasChanged("TransType");
            }
        }

        public DateTime? TransDate
        {
            get
            {
                return _transDate;
            }
            set
            {
                _transDate = value;
                PropertyHasChanged("TransDate");
            }
        }

        public string TransDes
        {
            get
            {
                return _transDes;
            }
            set
            {
                _transDes = value;
                PropertyHasChanged("TransDes");
            }
        }

        public decimal? TransAmt
        {
            get
            {
                return _transAmt;
            }
            set
            {
                _transAmt = value;
                PropertyHasChanged("TransAmt");
            }
        }

        public int? TransDeptKey
        {
            get
            {
                return _transDeptKey;
            }
            set
            {
                _transDeptKey = value;
                PropertyHasChanged("TransDeptKey");
            }
        }

        public int? TransGrpKey
        {
            get
            {
                return _transGrpKey;
            }
            set
            {
                _transGrpKey = value;
                PropertyHasChanged("TransGrpKey");
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {

                _error = value;

            }
        }

        protected override object GetIdValue()
        {
            return _eMKey.ToString() + _transKey.ToString();
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            /*
           //
           // TransDate
           //
           ValidationRules.AddRule(CommonRules.StringRequired, "TransDateString");
           //
           // TransDes
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TransDes", 255));
            */
        }

        protected override void AddBusinessRules()
        {
            /*
           AddCommonRules();
           AddCustomRules();
            */
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTSalesRepPayRoll()
        { /* require use of factory method */ }

        internal static MSTSalesRepPayRoll New()
        {

            MSTSalesRepPayRoll child = new MSTSalesRepPayRoll();

            return child;
        }

        internal static MSTSalesRepPayRoll NewChild()
        {

            MSTSalesRepPayRoll child = new MSTSalesRepPayRoll();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static MSTSalesRepPayRoll Get(SafeDataReader dr)
        {

            MSTSalesRepPayRoll child = new MSTSalesRepPayRoll();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTSalesRepPayRoll Get(int? eMKey, int? transKey)
        {

            MSTSalesRepPayRoll child = new MSTSalesRepPayRoll();
            child.Fetch(new Criteria(eMKey, transKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _eMKey = null;
            public int? _transKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? EMKey, int? TransKey)
            {
                _eMKey = EMKey;
                _transKey = TransKey;
            }

            internal Criteria(int? EMKey, int? TransKey, int? Option)
            {
                _eMKey = EMKey;
                _option = Option;

                if (TransKey == null)
                    _transKey = 0;
                else
                    _transKey = TransKey;
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
                cm.CommandText = "MSTSalesRepPayRoll_Get";
                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@EMKey", criteria._eMKey);
                cm.Parameters.AddWithValue("@TransKey", criteria._transKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
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
            _eMKey = dr.GetInt32("EMKey");
            _transKey = dr.GetInt32("TransKey");
            _transType = dr.GetInt32("TransType");
            //TransDate                
            if (GFunc.IsNE(dr.GetValue("TransDate")))
                _transDate = null;
            else
                _transDate = dr.GetDateTime("TransDate");
            _transDes = dr.GetString("TransDes");
            _transAmt = dr.GetDecimal("TransAmt");
            _transDeptKey = dr.GetInt32("TransDeptKey");
            _transGrpKey = dr.GetInt32("TransGrpKey");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
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
                    retValue = this.Insert(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            int transKey = 0;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepPayRoll_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@NewTransKey", transKey);

                if (_eMKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _eMKey);

                //if (_transKey == null)
                //    cm.Parameters.AddWithValue("@TransKey", DBNull.Value);
                //else
                cm.Parameters.AddWithValue("@TransKey", transKey);

                if (_transType == null)
                    cm.Parameters.AddWithValue("@TransType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransType", _transType);

                if (_transDate == null)
                    cm.Parameters.AddWithValue("@TransDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransDate", _transDate.Value);

                if (_transDes == null)
                    cm.Parameters.AddWithValue("@TransDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransDes", _transDes);

                if (_transAmt == null)
                    cm.Parameters.AddWithValue("@TransAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransAmt", _transAmt);

                if (_transDeptKey == null)
                    cm.Parameters.AddWithValue("@TransDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransDeptKey", _transDeptKey);

                if (_transGrpKey == null)
                    cm.Parameters.AddWithValue("@TransGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransGrpKey", _transGrpKey);

                cm.Parameters["@NewTransKey"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                transKey = (int)cm.Parameters["@NewTransKey"].Value;

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }// Already close and dispose sql connection.

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
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepPayRoll_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@NewTransKey", 0);

                if (_eMKey == null)
                    cm.Parameters.AddWithValue("@EMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@EMKey", _eMKey);

                if (_transKey == null)
                    cm.Parameters.AddWithValue("@TransKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransKey", _transKey);

                if (_transType == null)
                    cm.Parameters.AddWithValue("@TransType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransType", _transType);

                if (_transDate == null)
                    cm.Parameters.AddWithValue("@TransDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransDate", _transDate.Value);

                if (_transDes == null)
                    cm.Parameters.AddWithValue("@TransDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransDes", _transDes);

                if (_transAmt == null)
                    cm.Parameters.AddWithValue("@TransAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransAmt", _transAmt);

                if (_transDeptKey == null)
                    cm.Parameters.AddWithValue("@TransDeptKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransDeptKey", _transDeptKey);

                if (_transGrpKey == null)
                    cm.Parameters.AddWithValue("@TransGrpKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TransGrpKey", _transGrpKey);

                cm.Parameters["@NewTransKey"].Direction = ParameterDirection.Output;

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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTSalesRepPayRoll_Delete";
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@EMKey", criteria._eMKey);
                cm.Parameters.AddWithValue("@TransKey", criteria._transKey);
                cm.Parameters.AddWithValue("@Option", criteria._option);

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

        }

        #endregion //Data Access - Delete

    }
}

