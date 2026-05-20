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
    public class SYSAttachment : Csla.BusinessBase<SYSAttachment>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _docDC = null;
        internal int? _docDK = null;
        internal int? _docDItm = 0;
        internal int? _docDetailType = 0;
        internal int? _seq = 1;
        internal string _attachPath = string.Empty;
        internal string _attachFileType = string.Empty;
        internal string _attachDes = string.Empty;
        internal int? _attachSize = 0;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? DocDC
        {
            get
            {
                return _docDC;
            }
            set
            {
                _docDC = value;
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
            }
        }

        public int? DocDetailType
        {
            get
            {
                return _docDetailType;
            }
            set
            {
                _docDetailType = value;
            }
        }

        public int? Seq
        {
            get
            {
                return _seq;
            }
            set
            {
                _seq = value;
            }
        }

        public string AttachPath
        {
            get
            {
                return _attachPath;
            }
            set
            {
                _attachPath = value;
            }
        }

        public string AttachFileType
        {
            get
            {
                return _attachFileType;
            }
            set
            {
                _attachFileType = value;
            }
        }

        public string AttachDes
        {
            get
            {
                return _attachDes;
            }
            set
            {
                _attachDes = value;
                PropertyHasChanged("AttachDes");
            }
        }

        public int? AttachSize
        {
            get
            {
                return _attachSize;
            }
            set
            {
                _attachSize = value;
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

        protected override object GetIdValue()
        {
            return _docDC.ToString() + _docDK.ToString() + _docDItm.ToString() + _docDetailType.ToString() + _seq.ToString();
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
            //// AttachPath
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "AttachPath");
            ////
            //// AttachFileType
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AttachFileType", 255));
            ////
            //// AttachDes
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AttachDes", 255));
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

        internal SYSAttachment()
        { /* require use of factory method */ }

        internal static SYSAttachment New()
        {
            
            SYSAttachment child = new SYSAttachment();
            
            return child;
        }

        public static SYSAttachment NewChild()
        {
            
            SYSAttachment child = new SYSAttachment();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSAttachment Get(SafeDataReader dr)
        {
            
            SYSAttachment child = new SYSAttachment();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSAttachment Get(int? docDC, int? docDK, int? docDItm, int? docDetailType, int? seq)
        {
            
            SYSAttachment child = new SYSAttachment();
            child.Fetch(new Criteria(docDC, docDK, docDItm, docDetailType, seq, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        public class Criteria
        {
            public int? _docDC = null;
            public int? _docDK = null;
            public int? _docDItm = null;
            public int? _docDetailType = null;
            public int? _seq = null;
            public int? _option = null;

            public Criteria()
            {
            }
            //Added By Thida
            public Criteria(int? DocDC, int? DocDK, int? Option)
            {
                _docDC = DocDC;
                _docDK = DocDK;
                _option = Option;
            }

            public Criteria(int? DocDC, int? DocDK, int? DocDItm, int? DocDetailType, int? Option)
            {
                _docDC = DocDC;
                _docDK = DocDK;
                _docDItm = DocDItm;
                _docDetailType = DocDetailType;
                _option = Option;
            }

            public Criteria(int? DocDC, int? DocDK, int? DocDItm, int? DocDetailType, int? Seq, int? Option)
            {
                _docDC = DocDC;
                _docDK = DocDK;
                _docDItm = DocDItm;
                _docDetailType = DocDetailType;
                _seq = Seq;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        public bool Fetch(Criteria criteria)
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

        public bool Fetch(SqlConnection cn, Criteria criteria)
        {            
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSAttachment_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@DocDItm", criteria._docDItm);
                cm.Parameters.AddWithValue("@DocDetailType", criteria._docDetailType);
                cm.Parameters.AddWithValue("@Seq", criteria._seq);

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
                  
                }	// Already close and dispose data reader.
                

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    

            }// Already close and dispose sql connection.
        }

        public bool Fetch(SafeDataReader dr)
        {   
            
            _docDC = dr.GetInt32("DocDC");
            _docDK = dr.GetInt32("DocDK");
            _docDItm = dr.GetInt32("DocDItm");
            _docDetailType = dr.GetInt32("DocDetailType");
            _seq = dr.GetInt32("Seq");
            _attachPath = dr.GetString("AttachPath");
            _attachFileType = dr.GetString("AttachFileType");
            _attachDes = dr.GetString("AttachDes");
            _attachSize = dr.GetInt32("AttachSize");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
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
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            
            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSAttachment_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);
                

            
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

                if (_docDetailType == null)
                    cm.Parameters.AddWithValue("@DocDetailType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDetailType", _docDetailType);

                if (_seq == null)
                    cm.Parameters.AddWithValue("@Seq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Seq", _seq);

                if (_attachPath == null)
                    cm.Parameters.AddWithValue("@AttachPath", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachPath", _attachPath);

                if (_attachFileType == null)
                    cm.Parameters.AddWithValue("@AttachFileType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachFileType", _attachFileType);

                if (_attachDes == null)
                    cm.Parameters.AddWithValue("@AttachDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachDes", _attachDes);

                if (_attachSize == null)
                    cm.Parameters.AddWithValue("@AttachSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachSize", _attachSize);

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

                

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                

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
                cm.CommandText = "SYSAttachment_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                                

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

                if (_docDetailType == null)
                    cm.Parameters.AddWithValue("@DocDetailType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DocDetailType", _docDetailType);

                if (_seq == null)
                    cm.Parameters.AddWithValue("@Seq", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Seq", _seq);

                if (_attachPath == null)
                    cm.Parameters.AddWithValue("@AttachPath", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachPath", _attachPath);

                if (_attachFileType == null)
                    cm.Parameters.AddWithValue("@AttachFileType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachFileType", _attachFileType);

                if (_attachDes == null)
                    cm.Parameters.AddWithValue("@AttachDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachDes", _attachDes);

                if (_attachSize == null)
                    cm.Parameters.AddWithValue("@AttachSize", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@AttachSize", _attachSize);

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
                cm.CommandText = "SYSAttachment_Delete";
                //Added By Thida
                cm.Parameters.AddWithValue("@Option", criteria._option);
                                    
                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@DocDItm", criteria._docDItm);
                cm.Parameters.AddWithValue("@DocDetailType", criteria._docDetailType);
                cm.Parameters.AddWithValue("@Seq", criteria._seq);

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
            bool retVal = false;
            
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call validation method.
                    retVal=this.Validation(cn, criteria, isNew);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope
            return retVal;           
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {            
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSAttachment_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@DocDC", criteria._docDC);
                cm.Parameters.AddWithValue("@DocDK", criteria._docDK);
                cm.Parameters.AddWithValue("@DocDItm", criteria._docDItm);
                cm.Parameters.AddWithValue("@DocDetailType", criteria._docDetailType);
                cm.Parameters.AddWithValue("@Seq", criteria._seq);

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
