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
    public class MSTFinDesigner : Csla.BusinessBase<MSTFinDesigner>
    {
        #region Business Properties and Methods

        //declare members
        internal int _repKey = 0;
        internal int _repDetKey = 0;
        internal short _colNo = 0;
        internal string _designerText = string.Empty;
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

        public string DesignerText
        {
            get
            {
                return _designerText;
            }
            set
            {
                _designerText = value;
                PropertyHasChanged("DesignerText");
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
            //// MSTFinDesigner
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "MSTFinDesigner");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MSTFinDesignerID", 50));
            ////
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal MSTFinDesigner()
        { /* require use of factory method */ }

        internal static MSTFinDesigner New()
        {
            MSTFinDesigner child = new MSTFinDesigner();
            return child;
        }

        internal static MSTFinDesigner NewChild()
        {
            MSTFinDesigner child = new MSTFinDesigner();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static MSTFinDesigner Get(SafeDataReader dr)
        {
            MSTFinDesigner child = new MSTFinDesigner();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTFinDesigner Get(int repKey, int repDetKey, short colNo)
        {
            MSTFinDesigner child = new MSTFinDesigner();
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
            public string _designerText = string.Empty;
            public string _custom1 = string.Empty;
            public string _custom2 = string.Empty;
            public string _custom3 = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int repKey, int? Option)
            {
                _repKey = repKey;
                _repDetKey = 0;
                _colNo = 0;
                _option = Option;
            }

            internal Criteria(int repKey, int repDetKey, int? Option)
            {
                _repKey = repKey;
                _repDetKey = repDetKey;
                _colNo = 0;
                _option = Option;
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
                cm.CommandText = "MSTFinDesigner_Get";

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
            _designerText = dr.GetString("DesignerText");
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

        internal bool Insert(SqlConnection cn, int repKey, int repDetKey, short colNo)
        {
            bool retValue = false;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTFinDesigner_AddUpdate";

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

                if (_designerText == null)
                    cm.Parameters.AddWithValue("@DesignerText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DesignerText", _designerText);

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

                repKey = GFunc.NEInt(cm.Parameters["@NewRepKey"].Value, 0);
                repDetKey = GFunc.NEInt(cm.Parameters["@NewRepDetKey"].Value, 0);
                colNo = (Int16)cm.Parameters["@NewColNo"].Value;

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
                cm.CommandText = "MSTFinDesigner_AddUpdate";

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

                if (_designerText == null)
                    cm.Parameters.AddWithValue("@DesignerText", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DesignerText", _designerText);

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
                cm.CommandText = "MSTFinDesigner_Delete";

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
                cm.CommandText = "MSTFinDesigner_Validation";


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
