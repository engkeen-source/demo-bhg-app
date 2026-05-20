


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
    public class MSTJobDetOpenBal : Csla.BusinessBase<MSTJobDetOpenBal>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _jobBalKey = null;
        internal int? _jobKey = null;
        internal int? _jobPhaseKey = null;
        internal int? _jobTaskKey = null;
        internal int? _jobCostTypeKey = null;
        internal int? _jobBalLineType = null;
        internal int? _jobBalItmKey = null;
        internal int? _jobBalItmKeySelect = null;
        internal string _jobBalItmDes = string.Empty;
        internal decimal? _jobBalQty = null;
        internal int? _jobBalUOMKey = null;
        internal decimal? _jobBalConRate = null;
        internal decimal? _jobBalPrice = null;
        internal decimal? _jobBalExpAmt = null;
        internal decimal? _jobBalRevAmt = null;
        internal decimal? _jobBalPaidAmt = null;
        internal string _docID = string.Empty;
        internal DateTime? _docDate = null;
        internal string _docDes = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? JobBalKey
        {
            get
            {
                CanReadProperty("JobBalKey", true);
                return _jobBalKey;
            }
            set
            {
                CanWriteProperty("JobBalKey", true);

                _jobBalKey = value;
                PropertyHasChanged("JobBalKey");

            }
        }

        public int? JobKey
        {
            get
            {
                CanReadProperty("JobKey", true);
                return _jobKey;
            }
            set
            {
                CanWriteProperty("JobKey", true);

                _jobKey = value;
                PropertyHasChanged("JobKey");

            }
        }

        public int? JobPhaseKey
        {
            get
            {
                CanReadProperty("JobPhaseKey", true);
                return _jobPhaseKey;
            }
            set
            {
                CanWriteProperty("JobPhaseKey", true);

                _jobPhaseKey = value;
                PropertyHasChanged("JobPhaseKey");

            }
        }

        public int? JobTaskKey
        {
            get
            {
                CanReadProperty("JobTaskKey", true);
                return _jobTaskKey;
            }
            set
            {
                CanWriteProperty("JobTaskKey", true);

                _jobTaskKey = value;
                PropertyHasChanged("JobTaskKey");

            }
        }

        public int? JobCostTypeKey
        {
            get
            {
                CanReadProperty("JobCostTypeKey", true);
                return _jobCostTypeKey;
            }
            set
            {
                CanWriteProperty("JobCostTypeKey", true);

                _jobCostTypeKey = value;
                PropertyHasChanged("JobCostTypeKey");

            }
        }

        public int? JobBalLineType
        {
            get
            {
                CanReadProperty("JobBalLineType", true);
                return _jobBalLineType;
            }
            set
            {
                CanWriteProperty("JobBalLineType", true);

                _jobBalLineType = value;
                PropertyHasChanged("JobBalLineType");

            }
        }

        public int? JobBalItmKey
        {
            get
            {
                CanReadProperty("JobBalItmKey", true);
                return _jobBalItmKey;
            }
            set
            {
                CanWriteProperty("JobBalItmKey", true);

                _jobBalItmKey = value;
                PropertyHasChanged("JobBalItmKey");

            }
        }

        public int? JobBalItmKeySelect
        {
            get
            {
                CanReadProperty("JobBalItmKeySelect", true);
                return _jobBalItmKeySelect;
            }
            set
            {
                CanWriteProperty("JobBalItmKeySelect", true);

                _jobBalItmKeySelect = value;
                PropertyHasChanged("JobBalItmKeySelect");
            }
        }

        public string JobBalItmDes
        {
            get
            {
                CanReadProperty("JobBalItmDes", true);
                return _jobBalItmDes;
            }
            set
            {
                CanWriteProperty("JobBalItmDes", true);
                if (value == null) value = string.Empty;

                _jobBalItmDes = value;
                PropertyHasChanged("JobBalItmDes");

            }
        }

        public decimal? JobBalQty
        {
            get
            {
                CanReadProperty("JobBalQty", true);
                return _jobBalQty;
            }
            set
            {
                CanWriteProperty("JobBalQty", true);

                _jobBalQty = value;
                PropertyHasChanged("JobBalQty");

            }
        }

        public int? JobBalUOMKey
        {
            get
            {
                CanReadProperty("JobBalUOMKey", true);
                return _jobBalUOMKey;
            }
            set
            {
                CanWriteProperty("JobBalUOMKey", true);

                _jobBalUOMKey = value;
                PropertyHasChanged("JobBalUOMKey");

            }
        }

        public decimal? JobBalConRate
        {
            get
            {
                CanReadProperty("JobBalConRate", true);
                return _jobBalConRate;
            }
            set
            {
                CanWriteProperty("JobBalConRate", true);

                _jobBalConRate = value;
                PropertyHasChanged("JobBalConRate");

            }
        }

        public decimal? JobBalPrice
        {
            get
            {
                CanReadProperty("JobBalPrice", true);
                return _jobBalPrice;
            }
            set
            {
                CanWriteProperty("JobBalPrice", true);

                _jobBalPrice = value;
                PropertyHasChanged("JobBalPrice");

            }
        }

        public decimal? JobBalExpAmt
        {
            get
            {
                CanReadProperty("JobBalExpAmt", true);
                return _jobBalExpAmt;
            }
            set
            {
                CanWriteProperty("JobBalExpAmt", true);

                _jobBalExpAmt = value;
                PropertyHasChanged("JobBalExpAmt");

            }
        }

        public decimal? JobBalRevAmt
        {
            get
            {
                CanReadProperty("JobBalRevAmt", true);
                return _jobBalRevAmt;
            }
            set
            {
                CanWriteProperty("JobBalRevAmt", true);

                _jobBalRevAmt = value;
                PropertyHasChanged("JobBalRevAmt");

            }
        }

        public decimal? JobBalPaidAmt
        {
            get
            {
                CanReadProperty("JobBalPaidAmt", true);
                return _jobBalPaidAmt;
            }
            set
            {
                CanWriteProperty("JobBalPaidAmt", true);

                _jobBalPaidAmt = value;
                PropertyHasChanged("JobBalPaidAmt");

            }
        }

        public string DocID
        {
            get
            {
                CanReadProperty("DocID", true);
                return _docID;
            }
            set
            {
                CanWriteProperty("DocID", true);
                if (value == null) value = string.Empty;

                _docID = value;
                PropertyHasChanged("DocID");

            }
        }

        public DateTime? DocDate
        {
            get
            {
                CanReadProperty("DocDate", true);
                return _docDate;
            }
            set
            {
                CanWriteProperty("DocDate", true);

                _docDate = value;
                PropertyHasChanged("DocDate");

            }
        }

        public string DocDes
        {
            get
            {
                CanReadProperty("DocDes", true);
                return _docDes;
            }
            set
            {
                CanWriteProperty("DocDes", true);
                if (value == null) value = string.Empty;

                _docDes = value;
                PropertyHasChanged("DocDes");

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
                CanWriteProperty("CreateDate", true);

                _createDate = value;
                PropertyHasChanged("CreateDate");

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
                CanWriteProperty("CreateUserKey", true);

                _createUserKey = value;
                PropertyHasChanged("CreateUserKey");

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
                CanWriteProperty("LastModifiedDate", true);

                _lastModifiedDate = value;
                PropertyHasChanged("LastModifiedDate");
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
                CanWriteProperty("LastModifiedUserKey", true);

                _lastModifiedUserKey = value;
                PropertyHasChanged("LastModifiedUserKey");

            }
        }

        public string Custom1
        {
            get
            {
                CanReadProperty("Custom1", true);
                return _custom1;
            }
            set
            {
                CanWriteProperty("Custom1", true);
                if (value == null) value = string.Empty;

                _custom1 = value;
                PropertyHasChanged("Custom1");

            }
        }

        public string Custom2
        {
            get
            {
                CanReadProperty("Custom2", true);
                return _custom2;
            }
            set
            {
                CanWriteProperty("Custom2", true);
                if (value == null) value = string.Empty;

                _custom2 = value;
                PropertyHasChanged("Custom2");

            }
        }

        public string Custom3
        {
            get
            {
                CanReadProperty("Custom3", true);
                return _custom3;
            }
            set
            {
                CanWriteProperty("Custom3", true);
                if (value == null) value = string.Empty;

                _custom3 = value;
                PropertyHasChanged("Custom3");

            }
        }

        protected override object GetIdValue()
        {
            return _jobBalKey.ToString();
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
            // DocID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "DocID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocID", 50));
            //
            // DocDate
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "DocDateString");
            //
            // DocDes
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DocDes", 255));
            //
            // Custom1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom1", 255));
            //
            // Custom2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom2", 255));
            //
            // Custom3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Custom3", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTJobDetOpenBal()
        { /* require use of factory method */ }

        internal static MSTJobDetOpenBal New()
        {

            MSTJobDetOpenBal child = new MSTJobDetOpenBal();

            return child;
        }

        internal static MSTJobDetOpenBal NewChild()
        {

            MSTJobDetOpenBal child = new MSTJobDetOpenBal();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static MSTJobDetOpenBal Get(SafeDataReader dr)
        {

            MSTJobDetOpenBal child = new MSTJobDetOpenBal();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTJobDetOpenBal Get(int? jobBalKey)
        {

            MSTJobDetOpenBal child = new MSTJobDetOpenBal();
            child.Fetch(new Criteria(jobBalKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _jobBalKey = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? JobBalKey)
            {
                _jobBalKey = JobBalKey;
            }

            internal Criteria(int? JobBalKey, int? Option)
            {
                _jobBalKey = JobBalKey;
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
                cm.CommandText = "MSTJobDetOpenBal_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@JobBalKey", criteria._jobBalKey);



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


                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        retValue = true;
                    else
                        retValue = false;
                }	// Already close and dispose data reader.

            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {

            _jobBalKey = dr.GetInt32("JobBalKey");
            _jobKey = dr.GetInt32("JobKey");
            _jobPhaseKey = dr.GetInt32("JobPhaseKey");
            _jobTaskKey = dr.GetInt32("JobTaskKey");
            _jobCostTypeKey = dr.GetInt32("JobCostTypeKey");
            _jobBalLineType = dr.GetInt32("JobBalLineType");
            _jobBalItmKey = dr.GetInt32("JobBalItmKey");
            _jobBalItmKeySelect = dr.GetInt32("JobBalItmKeySelect");
            _jobBalItmDes = dr.GetString("JobBalItmDes");
            _jobBalQty = dr.GetDecimal("JobBalQty");
            _jobBalUOMKey = dr.GetInt32("JobBalUOMKey");
            _jobBalConRate = dr.GetDecimal("JobBalConRate");
            _jobBalPrice = dr.GetDecimal("JobBalPrice");
            _jobBalExpAmt = dr.GetDecimal("JobBalExpAmt");
            _jobBalRevAmt = dr.GetDecimal("JobBalRevAmt");
            _jobBalPaidAmt = dr.GetDecimal("JobBalPaidAmt");
            _docID = dr.GetString("DocID");
            _docDate = dr.GetDateTime("DocDate");
            _docDes = dr.GetString("DocDes");
            _createDate = dr.GetDateTime("CreateDate");
            _createUserKey = dr.GetInt32("CreateUserKey");
            _lastModifiedDate = dr.GetDateTime("LastModifiedDate");
            _lastModifiedUserKey = dr.GetInt32("LastModifiedUserKey");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? jobBalKey)
        {
            bool retValue = false;

            jobBalKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out jobBalKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? jobBalKey)
        {

            jobBalKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJobDetOpenBal_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);


                cm.Parameters.AddWithValue("@NewJobBalKey", jobBalKey);

                if (_jobBalKey == null)
                    cm.Parameters.AddWithValue("@JobBalKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalKey", _jobBalKey);

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobPhaseKey == null)
                    cm.Parameters.AddWithValue("@JobPhaseKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPhaseKey", _jobPhaseKey);

                if (_jobTaskKey == null)
                    cm.Parameters.AddWithValue("@JobTaskKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobTaskKey", _jobTaskKey);

                if (_jobCostTypeKey == null)
                    cm.Parameters.AddWithValue("@JobCostTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeKey", _jobCostTypeKey);

                if (_jobBalLineType == null)
                    cm.Parameters.AddWithValue("@JobBalLineType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalLineType", _jobBalLineType);

                if (_jobBalItmKey == null)
                    cm.Parameters.AddWithValue("@JobBalItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalItmKey", _jobBalItmKey);

                if (_jobBalItmKeySelect == null)
                    cm.Parameters.AddWithValue("@JobBalItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalItmKeySelect", _jobBalItmKeySelect);

                if (_jobBalItmDes == null)
                    cm.Parameters.AddWithValue("@JobBalItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalItmDes", _jobBalItmDes);

                if (_jobBalQty == null)
                    cm.Parameters.AddWithValue("@JobBalQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalQty", _jobBalQty);

                if (_jobBalUOMKey == null)
                    cm.Parameters.AddWithValue("@JobBalUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalUOMKey", _jobBalUOMKey);

                if (_jobBalConRate == null)
                    cm.Parameters.AddWithValue("@JobBalConRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalConRate", _jobBalConRate);

                if (_jobBalPrice == null)
                    cm.Parameters.AddWithValue("@JobBalPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalPrice", _jobBalPrice);

                if (_jobBalExpAmt == null)
                    cm.Parameters.AddWithValue("@JobBalExpAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalExpAmt", _jobBalExpAmt);

                if (_jobBalRevAmt == null)
                    cm.Parameters.AddWithValue("@JobBalRevAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalRevAmt", _jobBalRevAmt);

                if (_jobBalPaidAmt == null)
                    cm.Parameters.AddWithValue("@JobBalPaidAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalPaidAmt", _jobBalPaidAmt);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDate == null)
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDate", _docDate.Value);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (AppInfor.currentUserKey == null)
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


                //cm.Parameters["@NewJobBalKey"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                jobBalKey = (int)cm.Parameters["@NewJobBalKey"].Value;



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
                cm.CommandText = "MSTJobDetOpenBal_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@NewJobBalKey", 0);

                if (_jobBalKey == null)
                    cm.Parameters.AddWithValue("@JobBalKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalKey", _jobBalKey);

                if (_jobKey == null)
                    cm.Parameters.AddWithValue("@JobKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobKey", _jobKey);

                if (_jobPhaseKey == null)
                    cm.Parameters.AddWithValue("@JobPhaseKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobPhaseKey", _jobPhaseKey);

                if (_jobTaskKey == null)
                    cm.Parameters.AddWithValue("@JobTaskKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobTaskKey", _jobTaskKey);

                if (_jobCostTypeKey == null)
                    cm.Parameters.AddWithValue("@JobCostTypeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobCostTypeKey", _jobCostTypeKey);

                if (_jobBalLineType == null)
                    cm.Parameters.AddWithValue("@JobBalLineType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalLineType", _jobBalLineType);

                if (_jobBalItmKey == null)
                    cm.Parameters.AddWithValue("@JobBalItmKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalItmKey", _jobBalItmKey);

                if (_jobBalItmKeySelect == null)
                    cm.Parameters.AddWithValue("@JobBalItmKeySelect", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalItmKeySelect", _jobBalItmKeySelect);

                if (_jobBalItmDes == null)
                    cm.Parameters.AddWithValue("@JobBalItmDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalItmDes", _jobBalItmDes);

                if (_jobBalQty == null)
                    cm.Parameters.AddWithValue("@JobBalQty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalQty", _jobBalQty);

                if (_jobBalUOMKey == null)
                    cm.Parameters.AddWithValue("@JobBalUOMKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalUOMKey", _jobBalUOMKey);

                if (_jobBalConRate == null)
                    cm.Parameters.AddWithValue("@JobBalConRate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalConRate", _jobBalConRate);

                if (_jobBalPrice == null)
                    cm.Parameters.AddWithValue("@JobBalPrice", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalPrice", _jobBalPrice);

                if (_jobBalExpAmt == null)
                    cm.Parameters.AddWithValue("@JobBalExpAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalExpAmt", _jobBalExpAmt);

                if (_jobBalRevAmt == null)
                    cm.Parameters.AddWithValue("@JobBalRevAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalRevAmt", _jobBalRevAmt);

                if (_jobBalPaidAmt == null)
                    cm.Parameters.AddWithValue("@JobBalPaidAmt", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@JobBalPaidAmt", _jobBalPaidAmt);

                if (_docID == null)
                    cm.Parameters.AddWithValue("@DocID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocID", _docID);

                if (_docDate == null)
                    cm.Parameters.AddWithValue("@DocDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDate", _docDate.Value);

                if (_docDes == null)
                    cm.Parameters.AddWithValue("@DocDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDes", _docDes);

                if (_createDate == null)
                    cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateDate", _createDate.Value);

                if (_createUserKey == null)
                    cm.Parameters.AddWithValue("@CreateUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CreateUserKey", _createUserKey);

                if (_lastModifiedDate == null)
                    cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedDate", _lastModifiedDate.Value);

                if (AppInfor.currentUserKey == null)
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

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


                cm.Parameters["@NewJobBalKey"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTJobDetOpenBal_Delete";


                cm.Parameters.AddWithValue("@JobBalKey", criteria._jobBalKey);


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
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTJobDetOpenBal_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@JobBalKey", criteria._jobBalKey);



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