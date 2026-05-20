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
    public class SYSMsgList : Csla.BusinessBase<SYSMsgList>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _dataGrp = 0;
        internal int? _msgValue = 10;
        internal int? _msgParentValue = 0;
        internal string _msgMode = string.Empty;
        internal string _dataDes = string.Empty;
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
        internal bool? _buildIn = false;
        internal bool? _hidden = false;
        internal int? _CustomInt1 = 0;
        internal int? _CustomInt2 = 0;

        public int? DataGrp
        {
            get
            {
                return _dataGrp;
            }
        }

        public int? MsgValue
        {
            get
            {
                return _msgValue;
            }
        }

        public int? MsgParentValue
        {
            get
            {
                return _msgParentValue;
            }
        }

        public string MsgMode
        {
            get
            {
                return _msgMode;
            }
        }

        public string DataDes
        {
            get
            {
                return _dataDes;
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

        public bool? BuildIn
        {
            get
            {
                return _buildIn;
            }
        }

        public bool? Hidden
        {
            get
            {
                return _hidden;
            }
        }

        public int? CustomInt1
        {
            get
            {
                return _CustomInt1;
            }
        }

        public int? CustomInt2
        {
            get
            {
                return _CustomInt2;
            }
        }
        protected override object GetIdValue()
        {
            return _dataGrp.ToString() + _msgValue.ToString();
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
            // DataGrp
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "DataGrp");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DataGrp", 50));
            //
            // MsgMode
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "MsgMode");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MsgMode", 50));
            //
            // DataDes
            //
            ValidationRules.AddRule(CommonRules.StringRequired, "DataDes");
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DataDes", 255));
            //
            // LangText1
            //
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

        internal SYSMsgList()
        { /* require use of factory method */ }

        internal static SYSMsgList New()
        {
            
            SYSMsgList child = new SYSMsgList();
            
            return child;
        }

        internal static SYSMsgList NewChild()
        {
            
            SYSMsgList child = new SYSMsgList();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
           
            return child;
        }

        internal static SYSMsgList Get(SafeDataReader dr)
        {
            
            SYSMsgList child = new SYSMsgList();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSMsgList Get(int? dataGrp, int? msgValue)
        {
            
            SYSMsgList child = new SYSMsgList();
            child.Fetch(new Criteria(dataGrp, msgValue, 2));
            return child;
        }
        internal static SYSMsgList Get(SqlConnection cn, int? dataGrp, int? msgValue)
        {

            SYSMsgList child = new SYSMsgList();
            child.Fetch(cn,new Criteria(dataGrp, msgValue, 2));
            return child;
        }
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            internal int? _dataGrp = 0;
            public int? _msgValue = null;
            public int? _option = null;

            internal Criteria()
            {
            }

            internal Criteria(int? DataGrp, int? MsgValue)
            {
                _dataGrp = DataGrp;
                _msgValue = MsgValue;
            }

            internal Criteria(int? DataGrp, int? MsgValue, int? Option)
            {
                _dataGrp = DataGrp;
                _msgValue = MsgValue;
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
                cm.CommandText = "SYSMsgList_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@DataGrp", criteria._dataGrp);
                cm.Parameters.AddWithValue("@MsgValue", criteria._msgValue);

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
            _dataGrp = dr.GetInt32("DataGrp");
            _msgValue = dr.GetInt32("MsgValue");
            _msgParentValue = dr.GetInt32("MsgParentValue");
            _msgMode = dr.GetString("MsgMode");
            _dataDes = dr.GetString("DataDes");
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
            _buildIn = dr.GetBoolean("BuildIn");
            _hidden = dr.GetBoolean("Hidden");
            _CustomInt1 = dr.GetInt32("CustomInt1");
            _CustomInt2 = dr.GetInt32("CustomInt2");
           
            return true;
        }
        #endregion //Data Access - Fetch
    
    }
}
