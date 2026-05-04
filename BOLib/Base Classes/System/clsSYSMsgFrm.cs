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
    public class SYSMsgFrm : Csla.BusinessBase<SYSMsgFrm>
    {
        #region Business Properties and Methods

        //declare members
        internal string _msgID = string.Empty;
        internal string _msgGrp = string.Empty;
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

        public string MsgID
        {
            get
            {
                return _msgID;
            }
        }

        public string MsgGrp
        {
            get
            {
                return _msgGrp;
            }
        }

        public string LangText1
        {
            get
            {
                return _langText1;
            }
        }

        public string LangText2
        {
            get
            {
                return _langText2;
            }
        }

        public string LangText3
        {
            get
            {
                return _langText3;
            }
        }

        public string LangText4
        {
            get
            {
                return _langText4;
            }
        }

        public string LangText5
        {
            get
            {
                return _langText5;
            }
        }

        public string LangText6
        {
            get
            {
                return _langText6;
            }
        }

        public string LangText7
        {
            get
            {
                return _langText7;
            }
        }

        public string LangText8
        {
            get
            {
                return _langText8;
            }
        }

        public string LangText9
        {
            get
            {
                return _langText9;
            }
        }

        public string LangText10
        {
            get
            {
                return _langText10;
            }
        }

        protected override object GetIdValue()
        {
            return _msgID.ToString();
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
            // MsgID
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "MsgID");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgID", 50));
            //
            // MsgGrp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgGrp", 50));
            //
            // LangText1
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "LangText1");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText1", 255));
            //
            // LangText2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText2", 255));
            //
            // LangText3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText3", 255));
            //
            // LangText4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText4", 255));
            //
            // LangText5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText5", 255));
            //
            // LangText6
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText6", 255));
            //
            // LangText7
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText7", 255));
            //
            // LangText8
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText8", 255));
            //
            // LangText9
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText9", 255));
            //
            // LangText10
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LangText10", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal SYSMsgFrm()
        { /* require use of factory method */ }

        internal static SYSMsgFrm New(out string msgID)
        {
            msgID = BOLib.MsgID.Common.NewFail;
            SYSMsgFrm child = new SYSMsgFrm();
            
            return child;
        }

        internal static SYSMsgFrm NewChild(out string msgID)
        {
            msgID = BOLib.MsgID.Common.NewFail;
            SYSMsgFrm child = new SYSMsgFrm();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSMsgFrm Get(SafeDataReader dr)
        {
            
            SYSMsgFrm child = new SYSMsgFrm();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSMsgFrm Get(string msgID)
        {
            
            SYSMsgFrm child = new SYSMsgFrm();
            //Changed By Thida
            child.Fetch(new Criteria(msgID, 1), msgID);
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal string _msgID = string.Empty;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(string MsgID)
            {
                _msgID = MsgID;
            }

            internal Criteria(string MsgID, int? Option)
            {
                _msgID = MsgID;
                _option = Option;
            }
        }

        #endregion //Criteria

        #region Data Access - Fetch
        //Change By Thida
        internal bool Fetch(Criteria criteria, string msgID)
        {
            bool retValue = false;
 
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Fetch(cn, criteria, msgID);
            }
              
            return retValue;
        }

        //Changed By Thida
        internal bool Fetch(SqlConnection cn, Criteria criteria, string msgID)
        {
 
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSMsgFrm_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@MsgID", criteria._msgID);

                
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
            
            _msgID = dr.GetString("MsgID");
            _msgGrp = dr.GetString("MsgGrp");
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
            ValidationRules.CheckRules();
            return true;
        

        }
        #endregion //Data Access - Fetch
    }
}
