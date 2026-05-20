

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
    public class MSTItmSerialLog : Csla.BusinessBase<MSTItmSerialLog>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _serialKey = null;
        internal int? _docDC = null;
        internal int? _docDK = null;
        internal int? _docDItm = null;
        internal short? _logType = null;
        internal decimal? _qty = null;
        internal string _warranty = string.Empty;

        public int? SerialKey
        {
            get
            {
                return _serialKey;
            }
            set
            {
                _serialKey = value;
                PropertyHasChanged("SerialKey");
            }
        }

        public int? DocDC
        {
            get
            {
                return _docDC;
            }
            set
            {
                _docDC = value;
                PropertyHasChanged("DocDC");
            }
        }

        public int? DocDK
        {
            get
            {
                return _docDK;
            }
            set
            {
                _docDK = value;
                PropertyHasChanged("DocDK");
            }
        }

        public int? DocDItm
        {
            get
            {
                return _docDItm;
            }
            set
            {
                _docDItm = value;
                PropertyHasChanged("DocDItm");
            }
        }

        public short? LogType
        {
            get
            {
                return _logType;
            }
            set
            {
                _logType = value;
                PropertyHasChanged("LogType");
            }
        }

        public decimal? Qty
        {
            get
            {
                return _qty;
            }
            set
            {
                _qty = value;
                PropertyHasChanged("Qty");
            }
        }

        public string Warranty
        {
            get
            {
                return _warranty;
            }
            set
            {
                _warranty = value;
                PropertyHasChanged("Warranty");
            }
        }

        protected override object GetIdValue()
        {
            return _serialKey.ToString() + _docDC.ToString() + _docDK.ToString() + _docDItm.ToString();
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
           // Warranty
           //
           ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Warranty", 255));
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

        internal MSTItmSerialLog()
        { /* require use of factory method */ }

        internal static MSTItmSerialLog New()
        {         
            MSTItmSerialLog child = new MSTItmSerialLog();     
            return child;
        }

        internal static MSTItmSerialLog NewChild()
        {          
            MSTItmSerialLog child = new MSTItmSerialLog();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();          
            return child;
        }

        internal static MSTItmSerialLog Get(SafeDataReader dr)
        {          
            MSTItmSerialLog child = new MSTItmSerialLog();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static MSTItmSerialLog Get(int? serialKey, int? docDC, int? docDK, int? docDItm)
        {
            
            MSTItmSerialLog child = new MSTItmSerialLog();
            child.Fetch(new Criteria(serialKey, docDC, docDK, docDItm, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _serialKey = null;
            public int? _docDC = null;
            public int? _docDK = null;
            public int? _docDItm = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? SerialKey, int? DocDC, int? DocDK, int? DocDItm)
            {
                _serialKey = SerialKey;
                _docDC = DocDC;
                _docDK = DocDK;
                _docDItm = DocDItm;
            }

            internal Criteria(int? SerialKey, int? DocDC, int? DocDK, int? DocDItm, int? Option)
            {
                _serialKey = SerialKey;
                _docDC = DocDC;
                _docDK = DocDK;
                _docDItm = DocDItm;
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
                cm.CommandText = "MSTItmSerialLog_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);              
                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);
                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@DocDItm", criteria._docDItm);                

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
            _serialKey = dr.GetInt32("SerialKey");
            _docDC = dr.GetInt32("DocDC");
            _docDK = dr.GetInt32("DocDK");
            _docDItm = dr.GetInt32("DocDItm");
            _logType = dr.GetInt16("LogType");
            _qty = dr.GetDecimal("Qty");
            _warranty = dr.GetString("Warranty");
            ValidationRules.CheckRules();

            return true;
            
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert(out int? serialKey, out int? docDC, out int? docDK, out int? docDItm)
        {
            bool retValue = false;           
            serialKey = null;
            docDC = null;
            docDK = null;
            docDItm = null;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out serialKey, out docDC, out docDK, out docDItm);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn,out int? serialKey, out int? docDC, out int? docDK, out int? docDItm)
        {
            serialKey = 0;
            docDC = 0;
            docDK = 0;
            docDItm = 0;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmSerialLog_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewSerialKey", serialKey);
                cm.Parameters.AddWithValue("@NewDocDC", docDC);
                cm.Parameters.AddWithValue("@NewDocDK", docDK);
                cm.Parameters.AddWithValue("@NewDocDItm", docDItm);

                if (_serialKey == null)
                    cm.Parameters.AddWithValue("@SerialKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialKey", _serialKey);

                if (_docDC == null)
                    cm.Parameters.AddWithValue("@DocDC", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDC", _docDC);

                if (_docDK == null)
                    cm.Parameters.AddWithValue("@DocDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDK", _docDK);

                if (_docDItm == null)
                    cm.Parameters.AddWithValue("@DocDItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDItm", _docDItm);

                if (_logType == null)
                    cm.Parameters.AddWithValue("@LogType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogType", _logType);

                if (_qty == null)
                    cm.Parameters.AddWithValue("@Qty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Qty", _qty);

                if (_warranty == null)
                    cm.Parameters.AddWithValue("@Warranty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Warranty", _warranty);

                cm.Parameters["@NewSerialKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewDocDC"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewDocDK"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewDocDItm"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

             
                serialKey = (int)cm.Parameters["@NewSerialKey"].Value;
                docDC = (int)cm.Parameters["@NewDocDC"].Value;
                docDK = (int)cm.Parameters["@NewDocDK"].Value;
                docDItm = (int)cm.Parameters["@NewDocDItm"].Value;
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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Update(SqlConnection cn)
        {
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmSerialLog_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@NewSerialKey", 0);
                cm.Parameters.AddWithValue("@NewDocDC", 0);
                cm.Parameters.AddWithValue("@NewDocDK", 0);
                cm.Parameters.AddWithValue("@NewDocDItm", 0);

                if (_serialKey == null)
                    cm.Parameters.AddWithValue("@SerialKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@SerialKey", _serialKey);

                if (_docDC == null)
                    cm.Parameters.AddWithValue("@DocDC", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDC", _docDC);

                if (_docDK == null)
                    cm.Parameters.AddWithValue("@DocDK", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDK", _docDK);

                if (_docDItm == null)
                    cm.Parameters.AddWithValue("@DocDItm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDItm", _docDItm);

                if (_logType == null)
                    cm.Parameters.AddWithValue("@LogType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@LogType", _logType);

                if (_qty == null)
                    cm.Parameters.AddWithValue("@Qty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Qty", _qty);

                if (_warranty == null)
                    cm.Parameters.AddWithValue("@Warranty", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Warranty", _warranty);

                cm.Parameters["@NewSerialKey"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewDocDC"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewDocDK"].Direction = ParameterDirection.Output;
                cm.Parameters["@NewDocDItm"].Direction = ParameterDirection.Output;

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
                cm.CommandText = "MSTItmSerialLog_Delete";

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);
                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@DocDItm", criteria._docDItm);

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

        internal bool Validation(Criteria criteria, bool? isNew)
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

        internal bool Validation(SqlConnection cn, Criteria criteria, bool? isNew)
        {            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmSerialLog_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.Parameters.AddWithValue("@SerialKey", criteria._serialKey);
                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@DocDItm", criteria._docDItm);

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


