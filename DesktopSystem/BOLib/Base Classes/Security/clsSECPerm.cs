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
    public class SECPerm : Csla.BusinessBase<SECPerm>
    {
        #region Business Properties and Methods

        //declare members
        internal string _permID = string.Empty;
        internal int? _permGrpKey = null;
        internal int? _permType = null;
        internal int? _permSeq = 1;
        internal string _langText1 = string.Empty;
        internal string _langText2 = string.Empty;
        internal string _langText3 = string.Empty;
        internal string _langText4 = string.Empty;
        internal string _langText5 = string.Empty;
        internal string _langText6 = string.Empty;
        internal string _langText7 = string.Empty;
        internal string _langText8 = string.Empty;
        internal string _langText9 = string.Empty;
        internal string _langText10 = string.Empty;
        internal string _custom1 = string.Empty;
        internal string _custom2 = string.Empty;
        internal string _custom3 = string.Empty;

        public string PermID
        {
            get
            {
                CanReadProperty("PermID", true);
                return _permID;
            }
        }

        public int? PermGrpKey
        {
            get
            {
                CanReadProperty("PermGrpKey", true);
                return _permGrpKey;
            }
        }

        public int? PermType
        {
            get
            {
                CanReadProperty("PermType", true);
                return _permType;
            }
        }

        public int? PermSeq
        {
            get
            {
                CanReadProperty("PermSeq", true);
                return _permSeq;
            }
        }

        public string LangText1
        {
            get
            {
                CanReadProperty("LangText1", true);
                return _langText1;
            }
        }

        public string LangText2
        {
            get
            {
                CanReadProperty("LangText2", true);
                return _langText2;
            }
        }

        public string LangText3
        {
            get
            {
                CanReadProperty("LangText3", true);
                return _langText3;
            }
        }

        public string LangText4
        {
            get
            {
                CanReadProperty("LangText4", true);
                return _langText4;
            }
        }

        public string LangText5
        {
            get
            {
                CanReadProperty("LangText5", true);
                return _langText5;
            }
        }

        public string LangText6
        {
            get
            {
                CanReadProperty("LangText6", true);
                return _langText6;
            }
        }

        public string LangText7
        {
            get
            {
                CanReadProperty("LangText7", true);
                return _langText7;
            }
        }

        public string LangText8
        {
            get
            {
                CanReadProperty("LangText8", true);
                return _langText8;
            }
        }

        public string LangText9
        {
            get
            {
                CanReadProperty("LangText9", true);
                return _langText9;
            }
        }

        public string LangText10
        {
            get
            {
                CanReadProperty("LangText10", true);
                return _langText10;
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
            return _permID.ToString();
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
            //// PermID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "PermID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PermID", 50));
            ////
            //// LangText1
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "LangText1");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText1", 255));
            ////
            //// LangText2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText2", 255));
            ////
            //// LangText3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText3", 255));
            ////
            //// LangText4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText4", 255));
            ////
            //// LangText5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText5", 255));
            ////
            //// LangText6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText6", 255));
            ////
            //// LangText7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText7", 255));
            ////
            //// LangText8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText8", 255));
            ////
            //// LangText9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText9", 255));
            ////
            //// LangText10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText10", 255));
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

        public SECPerm()
        { /* require use of factory method */ }

        internal static SECPerm New()
        {

            SECPerm child = new SECPerm();

            return child;
        }

        internal static SECPerm NewChild()
        {

            SECPerm child = new SECPerm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();

            return child;
        }

        internal static SECPerm Get(SafeDataReader dr)
        {

            SECPerm child = new SECPerm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SECPerm Get(string permID)
        {

            SECPerm child = new SECPerm();
            child.Fetch(new Criteria(permID, 1));
            return child;
        }
        internal static SECPerm Get(SqlConnection cn, string permID)
        {

            SECPerm child = new SECPerm();
            child.Fetch(cn, new Criteria(permID, 1));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _permID = string.Empty;
            public int? _option = null;
            internal int _permGrpKey = 0;

            internal Criteria()
            {
            }

            internal Criteria(string PermID)
            {
                _permID = PermID;
            }

            internal Criteria(string PermID, int? Option)
            {
                _permID = PermID;
                _permGrpKey = 0;
                _option = Option;
            }
            //Added By Thida
            internal Criteria(int PermGrpKey, int? Option)
            {
                _permGrpKey = PermGrpKey;
                _permID = "";
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch

        internal bool Fetch(Criteria criteria)
        {
            bool retValue = false;
            string msgID = MsgID.Common.GetFail;

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

            string msgID = MsgID.Common.GetFail;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SECPerm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@PermID", criteria._permID);
                cm.Parameters.AddWithValue("@PermGrpKey", criteria._permGrpKey);

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


            _permID = dr.GetString("PermID");
            _permGrpKey = dr.GetInt32("PermGrpKey");
            _permType = dr.GetInt32("PermType");
            _permSeq = dr.GetInt32("PermSeq");
            _langText1 = dr.GetString("LangText1");
            _langText2 = dr.GetString("LangText2");
            _langText3 = dr.GetString("LangText3");
            _langText4 = dr.GetString("LangText4");
            _langText5 = dr.GetString("LangText5");
            _langText6 = dr.GetString("LangText6");
            _langText7 = dr.GetString("LangText7");
            _langText8 = dr.GetString("LangText8");
            _langText9 = dr.GetString("LangText9");
            _langText10 = dr.GetString("LangText10");
            _custom1 = dr.GetString("Custom1");
            _custom2 = dr.GetString("Custom2");
            _custom3 = dr.GetString("Custom3");
            ValidationRules.CheckRules();
            return true;

        }
        #endregion //Data Access - Fetch

    }
}
