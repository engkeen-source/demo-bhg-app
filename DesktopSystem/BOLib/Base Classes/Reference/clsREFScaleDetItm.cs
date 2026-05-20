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
    public class REFScaleDetItm : Csla.BusinessBase<REFScaleDetItm>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _scaleKey = 0;
        internal short? _sizeNum = 1;
        internal string _sizeID = string.Empty;
        internal DateTime? _createDate = null;
        internal int? _createUserKey = null;
        internal DateTime? _lastModifiedDate = null;
        internal int? _lastModifiedUserKey = null;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? ScaleKey
        {
            get
            {
                CanReadProperty("ScaleKey", true);
                return _scaleKey;
            }
        }

        public short? SizeNum
        {
            get
            {
                CanReadProperty("SizeNum", true);
                return _sizeNum;
            }
        }

        public string SizeID
        {
            get
            {
                CanReadProperty("SizeID", true);
                return _sizeID;
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                CanReadProperty("CreateDate", true);
                return _createDate;
            }
        }

        public int? CreateUserKey
        {
            get
            {
                CanReadProperty("CreateUserKey", true);
                return _createUserKey;
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                CanReadProperty("LastModifiedDate", true);
                return _lastModifiedDate;
            }
        }

        public int? LastModifiedUserKey
        {
            get
            {
                CanReadProperty("LastModifiedUserKey", true);
                return _lastModifiedUserKey;
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
                if (value == null) value = string.Empty;

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

                if (value == null) value = string.Empty;

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

                if (value == null) value = string.Empty;

                _custom3 = value;
                PropertyHasChanged("Custom3");


            }
        }

        protected override object GetIdValue()
        {
            return _scaleKey.ToString() + _sizeNum.ToString();
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
            //// SizeID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "SizeID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SizeID", 50));
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
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal REFScaleDetItm()
        { /* require use of factory method */ }

        internal static REFScaleDetItm New()
        {
            REFScaleDetItm child = new REFScaleDetItm();
            return child;
        }

        internal static REFScaleDetItm NewChild()
        {
            REFScaleDetItm child = new REFScaleDetItm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFScaleDetItm Get(SafeDataReader dr)
        {
            REFScaleDetItm child = new REFScaleDetItm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFScaleDetItm Get(int? scaleKey, short? sizeNum)
        {
            REFScaleDetItm child = new REFScaleDetItm();
            child.Fetch(new Criteria(scaleKey, sizeNum, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _scaleKey = null;
            public short? _sizeNum = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? ScaleKey, short? SizeNum)
            {
                _scaleKey = ScaleKey;
                _sizeNum = SizeNum;
            }

            internal Criteria(int? ScaleKey, short? SizeNum, int? Option)
            {
                _scaleKey = ScaleKey;
                _sizeNum = SizeNum;
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
                cm.CommandText = "REFScaleDetItm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ScaleKey", criteria._scaleKey);
                cm.Parameters.AddWithValue("@SizeNum", criteria._sizeNum);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
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

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _scaleKey = dr.GetInt32("ScaleKey");
            _sizeNum = dr.GetInt16("SizeNum");
            _sizeID = dr.GetString("SizeID");
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

        internal bool Insert(out int? scaleKey, out short? sizeNum)
        {
            bool retValue = false;
            scaleKey = null;
            sizeNum = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out scaleKey, out sizeNum);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? scaleKey, out short? sizeNum)
        {
            scaleKey = 0;
            sizeNum = 0;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFScaleDetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewScaleKey", scaleKey);
                cm.Parameters.AddWithValue("@NewSizeNum", sizeNum);

                if (_scaleKey == null)
                    cm.Parameters.AddWithValue("@ScaleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleKey", _scaleKey);

                if (_sizeNum == null)
                    cm.Parameters.AddWithValue("@SizeNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SizeNum", _sizeNum);

                if (_sizeID == null)
                    cm.Parameters.AddWithValue("@SizeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SizeID", _sizeID);

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

                cm.Parameters["@NewScaleKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewSizeNum"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                scaleKey = (int)cm.Parameters["@NewScaleKey"].Value;
                sizeNum = (short)cm.Parameters["@NewSizeNum"].Value;

                // Check Return Value -- Changed By Richard
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
                // Get current user key
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFScaleDetItm_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewScaleKey", 0);
                cm.Parameters.AddWithValue("@NewSizeNum", 0);

                if (_scaleKey == null)
                    cm.Parameters.AddWithValue("@ScaleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleKey", _scaleKey);

                if (_sizeNum == null)
                    cm.Parameters.AddWithValue("@SizeNum", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SizeNum", _sizeNum);

                if (_sizeID == null)
                    cm.Parameters.AddWithValue("@SizeID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SizeID", _sizeID);

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

                cm.Parameters["@NewScaleKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewSizeNum"].Direction = ParameterDirection.Output;

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "REFScaleDetItm_Delete";

                cm.Parameters.AddWithValue("@ScaleKey", criteria._scaleKey);
                cm.Parameters.AddWithValue("@SizeNum", criteria._sizeNum);



                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
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
                cm.CommandText = "REFScaleDetItm_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ScaleKey", criteria._scaleKey);
                cm.Parameters.AddWithValue("@SizeNum", criteria._sizeNum);

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                // Check Return Value -- Changed By Richard
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;
            }
        }
        #endregion //Data Access - Validation
    }
}
