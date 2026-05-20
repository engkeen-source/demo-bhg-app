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
    public class SYSMsgApp : Csla.BusinessBase<SYSMsgApp>
    {
        #region Business Properties and Methods

        //declare members
        private string _msgID = string.Empty;
        private string _msgGrp = string.Empty;
        private string _msgText1 = string.Empty;
        private string _msgText2 = string.Empty;
        private string _msgText3 = string.Empty;
        private string _msgText4 = string.Empty;
        private string _msgText5 = string.Empty;
        private string _msgText6 = string.Empty;
        private string _msgText7 = string.Empty;
        private string _msgText8 = string.Empty;
        private string _msgText9 = string.Empty;
        private string _msgText10 = string.Empty;
        private string _msgTitle1 = string.Empty;
        private string _msgTitle2 = string.Empty;
        private string _msgTitle3 = string.Empty;
        private string _msgTitle4 = string.Empty;
        private string _msgTitle5 = string.Empty;
        private string _msgTitle6 = string.Empty;
        private string _msgTitle7 = string.Empty;
        private string _msgTitle8 = string.Empty;
        private string _msgTitle9 = string.Empty;
        private string _msgTitle10 = string.Empty;
        private string _msgOption1 = string.Empty;
        private string _msgOption2 = string.Empty;
        private string _msgOption3 = string.Empty;
        private string _msgOption4 = string.Empty;
        private string _msgOption5 = string.Empty;
        private string _msgOption6 = string.Empty;
        private string _msgOption7 = string.Empty;
        private string _msgOption8 = string.Empty;
        private string _msgOption9 = string.Empty;
        private string _msgOption10 = string.Empty;

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

        public string MsgText1
        {
            get
            {
                return _msgText1;
            }
        }

        public string MsgText2
        {
            get
            {
                return _msgText2;
            }
        }

        public string MsgText3
        {
            get
            {
                return _msgText3;
            }
        }

        public string MsgText4
        {
            get
            {
                return _msgText4;
            }
        }

        public string MsgText5
        {
            get
            {
                return _msgText5;
            }
        }

        public string MsgText6
        {
            get
            {
                return _msgText6;
            }
        }

        public string MsgText7
        {
            get
            {
                return _msgText7;
            }
        }

        public string MsgText8
        {
            get
            {
                return _msgText8;
            }
        }

        public string MsgText9
        {
            get
            {
                return _msgText9;
            }
        }

        public string MsgText10
        {
            get
            {
                return _msgText10;
            }
        }

        public string MsgTitle1
        {
            get
            {
                return _msgTitle1;
            }
        }

        public string MsgTitle2
        {
            get
            {
                return _msgTitle2;
            }
        }

        public string MsgTitle3
        {
            get
            {
                return _msgTitle3;
            }
        }

        public string MsgTitle4
        {
            get
            {
                return _msgTitle4;
            }
        }

        public string MsgTitle5
        {
            get
            {
                return _msgTitle5;
            }
        }

        public string MsgTitle6
        {
            get
            {
                return _msgTitle6;
            }
        }

        public string MsgTitle7
        {
            get
            {
                return _msgTitle7;
            }
        }

        public string MsgTitle8
        {
            get
            {
                return _msgTitle8;
            }
        }

        public string MsgTitle9
        {
            get
            {
                return _msgTitle9;
            }
        }

        public string MsgTitle10
        {
            get
            {
                return _msgTitle10;
            }
        }

        public string MsgOption1
        {
            get
            {
                return _msgOption1;
            }
        }

        public string MsgOption2
        {
            get
            {
                return _msgOption2;
            }
        }

        public string MsgOption3
        {
            get
            {
                return _msgOption3;
            }
        }

        public string MsgOption4
        {
            get
            {
                return _msgOption4;
            }
        }

        public string MsgOption5
        {
            get
            {
                return _msgOption5;
            }
        }

        public string MsgOption6
        {
            get
            {
                return _msgOption6;
            }
        }

        public string MsgOption7
        {
            get
            {
                return _msgOption7;
            }
        }

        public string MsgOption8
        {
            get
            {
                return _msgOption8;
            }
        }

        public string MsgOption9
        {
            get
            {
                return _msgOption9;
            }
        }

        public string MsgOption10
        {
            get
            {
                return _msgOption10;
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
            // MsgText1
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "MsgText1");
            //
            // MsgTitle1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle1", 255));
            //
            // MsgTitle2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle2", 255));
            //
            // MsgTitle3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle3", 255));
            //
            // MsgTitle4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle4", 255));
            //
            // MsgTitle5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle5", 255));
            //
            // MsgTitle6
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle6", 255));
            //
            // MsgTitle7
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle7", 255));
            //
            // MsgTitle8
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle8", 255));
            //
            // MsgTitle9
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle9", 255));
            //
            // MsgTitle10
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgTitle10", 255));
            //
            // MsgOption1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption1", 255));
            //
            // MsgOption2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption2", 255));
            //
            // MsgOption3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption3", 255));
            //
            // MsgOption4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption4", 255));
            //
            // MsgOption5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption5", 255));
            //
            // MsgOption6
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption6", 255));
            //
            // MsgOption7
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption7", 255));
            //
            // MsgOption8
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption8", 255));
            //
            // MsgOption9
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption9", 255));
            //
            // MsgOption10
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgOption10", 255));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Factory Methods

        internal SYSMsgApp()
        { /* require use of factory method */ }

        internal static SYSMsgApp New()
        {
            
            SYSMsgApp child = new SYSMsgApp();
            
            return child;
        }

        internal static SYSMsgApp NewChild()
        {
            
            SYSMsgApp child = new SYSMsgApp();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSMsgApp Get(SafeDataReader dr)
        {
            SYSMsgApp child = new SYSMsgApp();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSMsgApp Get(string msgID)
        {
            SYSMsgApp child = new SYSMsgApp();
            child.Fetch(new Criteria(msgID, 1));
            return child;
        }
        internal static SYSMsgApp Get(SqlConnection cn, string msgID)
        {
            SYSMsgApp child = new SYSMsgApp();
            child.Fetch(cn, new Criteria(msgID, 1));
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
                cm.CommandText = "SYSMsgApp_Get";                    

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
            _msgText1 = dr.GetString("MsgText1");
            _msgText2 = dr.GetString("MsgText2");
            _msgText3 = dr.GetString("MsgText3");
            _msgText4 = dr.GetString("MsgText4");
            _msgText5 = dr.GetString("MsgText5");
            _msgText6 = dr.GetString("MsgText6");
            _msgText7 = dr.GetString("MsgText7");
            _msgText8 = dr.GetString("MsgText8");
            _msgText9 = dr.GetString("MsgText9");
            _msgText10 = dr.GetString("MsgText10");
            _msgTitle1 = dr.GetString("MsgTitle1");
            _msgTitle2 = dr.GetString("MsgTitle2");
            _msgTitle3 = dr.GetString("MsgTitle3");
            _msgTitle4 = dr.GetString("MsgTitle4");
            _msgTitle5 = dr.GetString("MsgTitle5");
            _msgTitle6 = dr.GetString("MsgTitle6");
            _msgTitle7 = dr.GetString("MsgTitle7");
            _msgTitle8 = dr.GetString("MsgTitle8");
            _msgTitle9 = dr.GetString("MsgTitle9");
            _msgTitle10 = dr.GetString("MsgTitle10");
            _msgOption1 = dr.GetString("MsgOption1");
            _msgOption2 = dr.GetString("MsgOption2");
            _msgOption3 = dr.GetString("MsgOption3");
            _msgOption4 = dr.GetString("MsgOption4");
            _msgOption5 = dr.GetString("MsgOption5");
            _msgOption6 = dr.GetString("MsgOption6");
            _msgOption7 = dr.GetString("MsgOption7");
            _msgOption8 = dr.GetString("MsgOption8");
            _msgOption9 = dr.GetString("MsgOption9");
            _msgOption10 = dr.GetString("MsgOption10");
            ValidationRules.CheckRules();
            return true;
        
        }
        #endregion //Data Access - Fetch
    }
}