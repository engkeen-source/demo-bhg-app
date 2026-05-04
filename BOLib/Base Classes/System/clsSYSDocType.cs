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
    public class SYSDocType : Csla.BusinessBase<SYSDocType>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _codeKey = null;
        internal int? _docType = null;
        internal short? _docSign = 1;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public int? CodeKey
        {
            get
            {
                return _codeKey;
            }
        }

        public int? DocType
        {
            get
            {
                return _docType;
            }
        }

        public short? DocSign
        {
            get
            {
                return _docSign;
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
            return _codeKey.ToString() + _docType.ToString();
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

        internal SYSDocType()
        { /* require use of factory method */ }

        internal static SYSDocType New()
        {
            
            SYSDocType child = new SYSDocType();
            
            return child;
        }

        internal static SYSDocType NewChild()
        {
            
            SYSDocType child = new SYSDocType();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSDocType Get(SafeDataReader dr)
        {
            
            SYSDocType child = new SYSDocType();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSDocType Get(int? codeKey, int? docType)
        {
            
            SYSDocType child = new SYSDocType();
            child.Fetch(new Criteria(codeKey, docType, 1));
            return child;
        }

        internal static SYSDocType Get(SqlConnection cn, int? codeKey, int? docType)
        {

            SYSDocType child = new SYSDocType();
            child.Fetch(cn,new Criteria(codeKey, docType, 1));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _codeKey = null;
            public int? _docType = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? CodeKey, int? DocType)
            {
                _codeKey = CodeKey;
                _docType = DocType;
            }

            internal Criteria(int? CodeKey, int? DocType, int? Option)
            {
                _codeKey = CodeKey;
                _docType = DocType;
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
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSDocType_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@MsgID", "");
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@DocType", criteria._docType);

                
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

        internal bool Fetch(SafeDataReader dr)
        {
            
            _codeKey = dr.GetInt32("CodeKey");
            _docType = dr.GetInt32("DocType");
            _docSign = dr.GetInt16("DocSign");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch
    }
}

