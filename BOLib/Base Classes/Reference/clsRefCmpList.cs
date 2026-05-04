

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections;
using System.Transactions;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class REFCmpList : Csla.BusinessBase<REFCmpList>
    {
        #region Business Properties and Methods

        //declare members
        private string _dataBaseID = string.Empty;
        private string _dataBaseNm = string.Empty;
        private string _companyNm = string.Empty;
        private string _connectionDSN = string.Empty;
        private string _authenticateMode = string.Empty;
        private string _connectionUserID = string.Empty;
        private string _connectionPassword = string.Empty;
        private string _lastUserID = string.Empty;

        public string DataBaseID
        {
            get
            {
                CanReadProperty("DataBaseID", true);
                return _dataBaseID;
            }
            set
            {
                CanWriteProperty("DataBaseID", true);
                if (value == null) value = string.Empty;

                _dataBaseID = value;
                PropertyHasChanged("DataBaseID");

            }
        }

        public string DataBaseNm
        {
            get
            {
                CanReadProperty("DataBaseNm", true);
                return _dataBaseNm;
            }
            set
            {
                CanWriteProperty("DataBaseNm", true);
                if (value == null) value = string.Empty;

                _dataBaseNm = value;
                PropertyHasChanged("DataBaseNm");

            }
        }

        public string CompanyNm
        {
            get
            {
                CanReadProperty("CompanyNm", true);
                return _companyNm;
            }
            set
            {
                CanWriteProperty("CompanyNm", true);
                if (value == null) value = string.Empty;

                _companyNm = value;
                PropertyHasChanged("CompanyNm");

            }
        }

        public string ConnectionDSN
        {
            get
            {
                CanReadProperty("ConnectionDSN", true);
                return _connectionDSN;
            }
            set
            {
                CanWriteProperty("ConnectionDSN", true);
                if (value == null) value = string.Empty;

                _connectionDSN = value;
                PropertyHasChanged("ConnectionDSN");

            }
        }

        public string AuthenticateMode
        {
            get
            {
                CanReadProperty("AuthenticateMode", true);
                return _authenticateMode;
            }
        }
        public string ConnectionUserID
        {
            get
            {
                CanReadProperty("ConnectionUserID", true);
                return _connectionUserID;
            }
        }

        public string ConnectionPassword
        {
            get
            {
                CanReadProperty("ConnectionPassword", true);
                return _connectionPassword;
            }
        }

        public string LastUserID
        {
            get
            {
                CanReadProperty("LastUserID", true);
                return _lastUserID;
            }
            set
            {
                CanWriteProperty("LastUserID", true);
                if (value == null) value = string.Empty;

                _lastUserID = value;
                PropertyHasChanged("LastUserID");

            }
        }

        protected override object GetIdValue()
        {
            return _dataBaseID.ToString();
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
            // DataBaseID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "DataBaseID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DataBaseID", 50));
            //
            // DataBaseNm
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "DataBaseNm");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DataBaseNm", 255));
            //
            // CompanyNm
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "CompanyNm");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CompanyNm", 255));
            //
            // ConnectionDSN
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "ConnectionDSN");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConnectionDSN", 50));
            //
            // LastUserID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "LastUserID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LastUserID", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal REFCmpList()
        { /* require use of factory method */ }

        internal static REFCmpList New()
        {
            REFCmpList child = new REFCmpList();
            return child;
        }

        internal static REFCmpList NewChild()
        {
            REFCmpList child = new REFCmpList();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFCmpList Get(SafeDataReader dr)
        {
            REFCmpList child = new REFCmpList();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static REFCmpList Get(string dataBaseID)
        {           
            REFCmpList child = new REFCmpList();
            
            child.Fetch(new Criteria(dataBaseID, 1));
           
            return child;
        }

        public static DataTable Get()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    // Using existing sql connection.
                    using (SqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "REFCmpList_Get";

                        cm.Parameters.AddWithValue("@Option", 0);
                        cm.Parameters.AddWithValue("@DataBaseID", DBNull.Value);

                        SqlDataAdapter adap = new SqlDataAdapter(cm);
                        DataTable dtRefList = new DataTable();
                       
                        adap.Fill(dtRefList);

                       
                        return dtRefList;
                    }

                }

            }
            catch (TAException tex)
            {
                throw tex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _dataBaseID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(string DataBaseID)
            {
                _dataBaseID = DataBaseID;
            }

            internal Criteria(int? option)
            {
                _option = option;
            }

            internal Criteria(string DataBaseID, int? Option)
            {
                _dataBaseID = DataBaseID;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;

            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
            {
                // Open sql connection. 
                cn.Open();
                // Call insert method.
                retValue = this.Fetch(cn, criteria);

            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCmpList_Get";
                cm.Connection = cn;

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@DataBaseID", criteria._dataBaseID);

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    while (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }

                }	// Already close and dispose data reader.                
            }// Already close and dispose sql connection.

            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _dataBaseID = dr.GetString("DataBaseID");
            _dataBaseNm = dr.GetString("DataBaseNm");
            _companyNm = dr.GetString("CompanyNm");
            _connectionDSN = dr.GetString("ConnectionDSN");
            _authenticateMode = dr.GetString("AuthenticateMode");
            _connectionUserID = dr.GetString("ConnectionUserID");
            _connectionPassword = dr.GetString("ConnectionPassword");
            _lastUserID = dr.GetString("LastUserID");
           // ValidationRules.CheckRules();

            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out string dataBaseID)
        {
            bool retValue = false;
            dataBaseID = string.Empty;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out dataBaseID);


                }// Already close and dispose sql connection.

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                    throw new Exception("Transaction has aborted.");
                scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out string dataBaseID)
        {
            dataBaseID = string.Empty;

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCmpList_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);


                cm.Parameters.AddWithValue("@NewDataBaseID", dataBaseID);

                if (_dataBaseID == null)
                    cm.Parameters.AddWithValue("@DataBaseID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataBaseID", _dataBaseID);

                if (_dataBaseNm == null)
                    cm.Parameters.AddWithValue("@DataBaseNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataBaseNm", _dataBaseNm);

                if (_companyNm == null)
                    cm.Parameters.AddWithValue("@CompanyNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CompanyNm", _companyNm);

                if (_connectionDSN == null)
                    cm.Parameters.AddWithValue("@ConnectionDSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConnectionDSN", _connectionDSN);

                if (_lastUserID == null)
                    cm.Parameters.AddWithValue("@LastUserID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastUserID", _lastUserID);


                cm.Parameters["@NewDataBaseID"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();


                dataBaseID = cm.Parameters["@NewDataBaseID"].Value.ToString();
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
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    // Call update method.
                    retValue = this.Update(cn);

                }// Already close and dispose sql connection.

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)
                    throw new Exception("Transaction has aborted.");
                scope.Complete();
            }// End of TransactionScope

            return retValue;

        }

        internal bool Update(SqlConnection cn)
        {

            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFCmpList_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewDataBaseID", string.Empty);

                if (_dataBaseID == null)
                    cm.Parameters.AddWithValue("@DataBaseID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataBaseID", _dataBaseID);

                if (_dataBaseNm == null)
                    cm.Parameters.AddWithValue("@DataBaseNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataBaseNm", _dataBaseNm);

                if (_companyNm == null)
                    cm.Parameters.AddWithValue("@CompanyNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CompanyNm", _companyNm);

                if (_connectionDSN == null)
                    cm.Parameters.AddWithValue("@ConnectionDSN", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ConnectionDSN", _connectionDSN);

                if (_lastUserID == null)
                    cm.Parameters.AddWithValue("@LastUserID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LastUserID", _lastUserID);


                cm.Parameters["@NewDataBaseID"].Direction = ParameterDirection.Output;

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
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    // Call delete method.
                    retValue = this.Delete(cn, criteria);

                }// Already close and dispose sql connection.
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
                cm.CommandText = "REFCmpList_Delete";

                cm.Parameters.AddWithValue("@DataBaseID", criteria._dataBaseID);

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
                // Create new sql connection for this method. 
                using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                {
                    // Open sql connection. 
                    cn.Open();
                    // Call validation method.
                    retValue = this.Validation(cn, criteria, isNew);

                }// Already close and dispose sql connection.
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
                cm.CommandText = "REFCmpList_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@DataBaseID", criteria._dataBaseID);



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


