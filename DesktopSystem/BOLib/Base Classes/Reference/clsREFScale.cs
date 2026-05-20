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
    public class REFScale : Csla.BusinessBase<REFScale>
    {
        #region Business Properties and Methods

        //declare members
        internal int? _scaleKey = 0;
        internal string _scaleID = string.Empty;
        internal string _scaleDes = string.Empty;
        internal string _size1 = string.Empty;
        internal string _size2 = string.Empty;
        internal string _size3 = string.Empty;
        internal string _size4 = string.Empty;
        internal string _size5 = string.Empty;
        internal string _size6 = string.Empty;
        internal string _size7 = string.Empty;
        internal string _size8 = string.Empty;
        internal string _size9 = string.Empty;
        internal string _size10 = string.Empty;
        internal string _size11 = string.Empty;
        internal string _size12 = string.Empty;
        internal string _size13 = string.Empty;
        internal string _size14 = string.Empty;
        internal string _size15 = string.Empty;
        internal string _size16 = string.Empty;
        internal string _size17 = string.Empty;
        internal string _size18 = string.Empty;
        internal string _size19 = string.Empty;
        internal string _size20 = string.Empty;
        internal string _size21 = string.Empty;
        internal string _size22 = string.Empty;
        internal string _size23 = string.Empty;
        internal string _size24 = string.Empty;
        internal string _size25 = string.Empty;
        internal string _size26 = string.Empty;
        internal string _size27 = string.Empty;
        internal string _size28 = string.Empty;
        internal string _size29 = string.Empty;
        internal string _size30 = string.Empty;
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

        public string ScaleID
        {
            get
            {

                return _scaleID;
            }
            set
            {

                if (value == null) value = string.Empty;

                _scaleID = value;
                PropertyHasChanged("ScaleID");


            }
        }

        public string ScaleDes
        {
            get
            {

                return _scaleDes;
            }
            set
            {

                if (value == null) value = string.Empty;

                _scaleDes = value;
                PropertyHasChanged("ScaleDes");


            }
        }

        public string Size1
        {
            get
            {

                return _size1;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size1 = value;
                PropertyHasChanged("Size1");


            }
        }

        public string Size2
        {
            get
            {
                return _size2;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size2 = value;
                PropertyHasChanged("Size2");


            }
        }

        public string Size3
        {
            get
            {

                return _size3;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size3 = value;
                PropertyHasChanged("Size3");


            }
        }

        public string Size4
        {
            get
            {
                return _size4;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size4 = value;
                PropertyHasChanged("Size4");


            }
        }

        public string Size5
        {
            get
            {

                return _size5;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size5 = value;
                PropertyHasChanged("Size5");


            }
        }

        public string Size6
        {
            get
            {

                return _size6;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size6 = value;
                PropertyHasChanged("Size6");


            }
        }

        public string Size7
        {
            get
            {
                return _size7;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size7 = value;
                PropertyHasChanged("Size7");


            }
        }

        public string Size8
        {
            get
            {
                return _size8;
            }
            set
            {
                if (value == null) value = string.Empty;

                _size8 = value;
                PropertyHasChanged("Size8");


            }
        }

        public string Size9
        {
            get
            {
                return _size9;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size9 = value;
                PropertyHasChanged("Size9");


            }
        }

        public string Size10
        {
            get
            {
                return _size10;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size10 = value;
                PropertyHasChanged("Size10");


            }
        }

        public string Size11
        {
            get
            {
                return _size11;
            }
            set
            {
                if (value == null) value = string.Empty;

                _size11 = value;
                PropertyHasChanged("Size11");


            }
        }

        public string Size12
        {
            get
            {

                return _size12;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size12 = value;
                PropertyHasChanged("Size12");


            }
        }

        public string Size13
        {
            get
            {

                return _size13;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size13 = value;
                PropertyHasChanged("Size13");

            }
        }

        public string Size14
        {
            get
            {
                return _size14;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size14 = value;
                PropertyHasChanged("Size14");


            }
        }

        public string Size15
        {
            get
            {

                return _size15;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size15 = value;
                PropertyHasChanged("Size15");


            }
        }

        public string Size16
        {
            get
            {

                return _size16;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size16 = value;
                PropertyHasChanged("Size16");


            }
        }

        public string Size17
        {
            get
            {

                return _size17;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size17 = value;
                PropertyHasChanged("Size17");


            }
        }

        public string Size18
        {
            get
            {

                return _size18;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size18 = value;
                PropertyHasChanged("Size18");

            }
        }

        public string Size19
        {
            get
            {

                return _size19;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size19 = value;
                PropertyHasChanged("Size19");


            }
        }

        public string Size20
        {
            get
            {

                return _size20;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size20 = value;
                PropertyHasChanged("Size20");


            }
        }

        public string Size21
        {
            get
            {

                return _size21;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size21 = value;
                PropertyHasChanged("Size21");


            }
        }

        public string Size22
        {
            get
            {
                return _size22;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size22 = value;
                PropertyHasChanged("Size22");


            }
        }

        public string Size23
        {
            get
            {

                return _size23;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size23 = value;
                PropertyHasChanged("Size23");


            }
        }

        public string Size24
        {
            get
            {

                return _size24;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size24 = value;
                PropertyHasChanged("Size24");


            }
        }

        public string Size25
        {
            get
            {
                return _size25;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size25 = value;
                PropertyHasChanged("Size25");


            }
        }

        public string Size26
        {
            get
            {
                return _size26;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size26 = value;
                PropertyHasChanged("Size26");


            }
        }

        public string Size27
        {
            get
            {

                return _size27;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size27 = value;
                PropertyHasChanged("Size27");

            }
        }

        public string Size28
        {
            get
            {
                return _size28;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size28 = value;
                PropertyHasChanged("Size28");


            }
        }

        public string Size29
        {
            get
            {

                return _size29;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size29 = value;
                PropertyHasChanged("Size29");


            }
        }

        public string Size30
        {
            get
            {
                return _size30;
            }
            set
            {

                if (value == null) value = string.Empty;

                _size30 = value;
                PropertyHasChanged("Size30");


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
            return _scaleKey.ToString();
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
            //// ScaleID
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "ScaleID");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ScaleID", 50));
            ////
            //// ScaleDes
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "ScaleDes");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ScaleDes", 255));
            ////
            //// Size1
            ////
            //ValidationRules.AddRule(CommonRules.StringRequired, "Size1");
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size1", 50));
            ////
            //// Size2
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size2", 50));
            ////
            //// Size3
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size3", 50));
            ////
            //// Size4
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size4", 50));
            ////
            //// Size5
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size5", 50));
            ////
            //// Size6
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size6", 50));
            ////
            //// Size7
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size7", 50));
            ////
            //// Size8
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size8", 50));
            ////
            //// Size9
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size9", 50));
            ////
            //// Size10
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size10", 50));
            ////
            //// Size11
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size11", 50));
            ////
            //// Size12
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size12", 50));
            ////
            //// Size13
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size13", 50));
            ////
            //// Size14
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size14", 50));
            ////
            //// Size15
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size15", 50));
            ////
            //// Size16
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size16", 50));
            ////
            //// Size17
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size17", 50));
            ////
            //// Size18
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size18", 50));
            ////
            //// Size19
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size19", 50));
            ////
            //// Size20
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size20", 50));
            ////
            //// Size21
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size21", 50));
            ////
            //// Size22
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size22", 50));
            ////
            //// Size23
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size23", 50));
            ////
            //// Size24
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size24", 50));
            ////
            //// Size25
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size25", 50));
            ////
            //// Size26
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size26", 50));
            ////
            //// Size27
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size27", 50));
            ////
            //// Size28
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size28", 50));
            ////
            //// Size29
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size29", 50));
            ////
            //// Size30
            ////
            //ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Size30", 50));
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

        internal REFScale()
        { /* require use of factory method */ }

        internal static REFScale New()
        {
            REFScale child = new REFScale();
            return child;
        }

        internal static REFScale NewChild()
        {
            REFScale child = new REFScale();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            return child;
        }

        internal static REFScale Get(SafeDataReader dr)
        {
            REFScale child = new REFScale();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static REFScale Get(int? scaleKey)
        {
            REFScale child = new REFScale();
            child.Fetch(new Criteria(scaleKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public int? _scaleKey = null;
            public int? _option = null;
            public string _scaleID = string.Empty;

            internal Criteria()
            {
            }

            internal Criteria(int? ScaleKey)
            {
                _scaleKey = ScaleKey;
            }
            internal Criteria(int? ScaleKey, string ScaleID)
            {
                _scaleKey = ScaleKey;
                _scaleID = ScaleID;
            }
            internal Criteria(int? ScaleKey, int? Option)
            {
                _scaleKey = ScaleKey;
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
                cm.CommandText = "REFScale_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                cm.Parameters.AddWithValue("@ScaleKey", criteria._scaleKey);


                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                // Using data reader as record set.
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    // If data reader can read, continue...
                    if (dr.Read())
                    {
                        retValue = this.Fetch(dr);
                    }
                    else
                        this.Clear();
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
            _scaleID = dr.GetString("ScaleID");
            _scaleDes = dr.GetString("ScaleDes");
            _size1 = dr.GetString("Size1");
            _size2 = dr.GetString("Size2");
            _size3 = dr.GetString("Size3");
            _size4 = dr.GetString("Size4");
            _size5 = dr.GetString("Size5");
            _size6 = dr.GetString("Size6");
            _size7 = dr.GetString("Size7");
            _size8 = dr.GetString("Size8");
            _size9 = dr.GetString("Size9");
            _size10 = dr.GetString("Size10");
            _size11 = dr.GetString("Size11");
            _size12 = dr.GetString("Size12");
            _size13 = dr.GetString("Size13");
            _size14 = dr.GetString("Size14");
            _size15 = dr.GetString("Size15");
            _size16 = dr.GetString("Size16");
            _size17 = dr.GetString("Size17");
            _size18 = dr.GetString("Size18");
            _size19 = dr.GetString("Size19");
            _size20 = dr.GetString("Size20");
            _size21 = dr.GetString("Size21");
            _size22 = dr.GetString("Size22");
            _size23 = dr.GetString("Size23");
            _size24 = dr.GetString("Size24");
            _size25 = dr.GetString("Size25");
            _size26 = dr.GetString("Size26");
            _size27 = dr.GetString("Size27");
            _size28 = dr.GetString("Size28");
            _size29 = dr.GetString("Size29");
            _size30 = dr.GetString("Size30");
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

        internal bool Insert(out int? scaleKey)
        {
            bool retValue = false;
            scaleKey = null;

            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                {
                    // Open Connection
                    cn.Open();

                    // Call insert method.
                    retValue = this.Insert(cn, out scaleKey);
                }// End of SqlConnection

                // No errors - commit transaction
                if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active) throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            return retValue;
        }

        internal bool Insert(SqlConnection cn, out int? scaleKey)
        {
            scaleKey = 0;
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _createUserKey = AppInfor.currentUserKey;
                _lastModifiedUserKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "REFScale_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 0);

                cm.Parameters.AddWithValue("@NewScaleKey", scaleKey);

                if (_scaleKey == null)
                    cm.Parameters.AddWithValue("@ScaleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleKey", _scaleKey);

                if (_scaleID == null)
                    cm.Parameters.AddWithValue("@ScaleID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleID", _scaleID);

                if (_scaleDes == null)
                    cm.Parameters.AddWithValue("@ScaleDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleDes", _scaleDes);

                if (_size1 == null)
                    cm.Parameters.AddWithValue("@Size1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size1", _size1);

                if (_size2 == null)
                    cm.Parameters.AddWithValue("@Size2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size2", _size2);

                if (_size3 == null)
                    cm.Parameters.AddWithValue("@Size3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size3", _size3);

                if (_size4 == null)
                    cm.Parameters.AddWithValue("@Size4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size4", _size4);

                if (_size5 == null)
                    cm.Parameters.AddWithValue("@Size5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size5", _size5);

                if (_size6 == null)
                    cm.Parameters.AddWithValue("@Size6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size6", _size6);

                if (_size7 == null)
                    cm.Parameters.AddWithValue("@Size7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size7", _size7);

                if (_size8 == null)
                    cm.Parameters.AddWithValue("@Size8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size8", _size8);

                if (_size9 == null)
                    cm.Parameters.AddWithValue("@Size9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size9", _size9);

                if (_size10 == null)
                    cm.Parameters.AddWithValue("@Size10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size10", _size10);

                if (_size11 == null)
                    cm.Parameters.AddWithValue("@Size11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size11", _size11);

                if (_size12 == null)
                    cm.Parameters.AddWithValue("@Size12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size12", _size12);

                if (_size13 == null)
                    cm.Parameters.AddWithValue("@Size13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size13", _size13);

                if (_size14 == null)
                    cm.Parameters.AddWithValue("@Size14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size14", _size14);

                if (_size15 == null)
                    cm.Parameters.AddWithValue("@Size15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size15", _size15);

                if (_size16 == null)
                    cm.Parameters.AddWithValue("@Size16", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size16", _size16);

                if (_size17 == null)
                    cm.Parameters.AddWithValue("@Size17", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size17", _size17);

                if (_size18 == null)
                    cm.Parameters.AddWithValue("@Size18", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size18", _size18);

                if (_size19 == null)
                    cm.Parameters.AddWithValue("@Size19", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size19", _size19);

                if (_size20 == null)
                    cm.Parameters.AddWithValue("@Size20", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size20", _size20);

                if (_size21 == null)
                    cm.Parameters.AddWithValue("@Size21", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size21", _size21);

                if (_size22 == null)
                    cm.Parameters.AddWithValue("@Size22", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size22", _size22);

                if (_size23 == null)
                    cm.Parameters.AddWithValue("@Size23", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size23", _size23);

                if (_size24 == null)
                    cm.Parameters.AddWithValue("@Size24", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size24", _size24);

                if (_size25 == null)
                    cm.Parameters.AddWithValue("@Size25", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size25", _size25);

                if (_size26 == null)
                    cm.Parameters.AddWithValue("@Size26", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size26", _size26);

                if (_size27 == null)
                    cm.Parameters.AddWithValue("@Size27", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size27", _size27);

                if (_size28 == null)
                    cm.Parameters.AddWithValue("@Size28", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size28", _size28);

                if (_size29 == null)
                    cm.Parameters.AddWithValue("@Size29", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size29", _size29);

                if (_size30 == null)
                    cm.Parameters.AddWithValue("@Size30", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size30", _size30);

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

                // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

                scaleKey = (int)cm.Parameters["@NewScaleKey"].Value;

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
                cm.CommandText = "REFScale_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);
                cm.Parameters.AddWithValue("@NewScaleKey", 0);

                if (_scaleKey == null)
                    cm.Parameters.AddWithValue("@ScaleKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleKey", _scaleKey);

                if (_scaleID == null)
                    cm.Parameters.AddWithValue("@ScaleID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleID", _scaleID);

                if (_scaleDes == null)
                    cm.Parameters.AddWithValue("@ScaleDes", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ScaleDes", _scaleDes);

                if (_size1 == null)
                    cm.Parameters.AddWithValue("@Size1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size1", _size1);

                if (_size2 == null)
                    cm.Parameters.AddWithValue("@Size2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size2", _size2);

                if (_size3 == null)
                    cm.Parameters.AddWithValue("@Size3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size3", _size3);

                if (_size4 == null)
                    cm.Parameters.AddWithValue("@Size4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size4", _size4);

                if (_size5 == null)
                    cm.Parameters.AddWithValue("@Size5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size5", _size5);

                if (_size6 == null)
                    cm.Parameters.AddWithValue("@Size6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size6", _size6);

                if (_size7 == null)
                    cm.Parameters.AddWithValue("@Size7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size7", _size7);

                if (_size8 == null)
                    cm.Parameters.AddWithValue("@Size8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size8", _size8);

                if (_size9 == null)
                    cm.Parameters.AddWithValue("@Size9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size9", _size9);

                if (_size10 == null)
                    cm.Parameters.AddWithValue("@Size10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size10", _size10);

                if (_size11 == null)
                    cm.Parameters.AddWithValue("@Size11", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size11", _size11);

                if (_size12 == null)
                    cm.Parameters.AddWithValue("@Size12", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size12", _size12);

                if (_size13 == null)
                    cm.Parameters.AddWithValue("@Size13", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size13", _size13);

                if (_size14 == null)
                    cm.Parameters.AddWithValue("@Size14", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size14", _size14);

                if (_size15 == null)
                    cm.Parameters.AddWithValue("@Size15", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size15", _size15);

                if (_size16 == null)
                    cm.Parameters.AddWithValue("@Size16", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size16", _size16);

                if (_size17 == null)
                    cm.Parameters.AddWithValue("@Size17", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size17", _size17);

                if (_size18 == null)
                    cm.Parameters.AddWithValue("@Size18", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size18", _size18);

                if (_size19 == null)
                    cm.Parameters.AddWithValue("@Size19", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size19", _size19);

                if (_size20 == null)
                    cm.Parameters.AddWithValue("@Size20", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size20", _size20);

                if (_size21 == null)
                    cm.Parameters.AddWithValue("@Size21", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size21", _size21);

                if (_size22 == null)
                    cm.Parameters.AddWithValue("@Size22", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size22", _size22);

                if (_size23 == null)
                    cm.Parameters.AddWithValue("@Size23", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size23", _size23);

                if (_size24 == null)
                    cm.Parameters.AddWithValue("@Size24", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size24", _size24);

                if (_size25 == null)
                    cm.Parameters.AddWithValue("@Size25", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size25", _size25);

                if (_size26 == null)
                    cm.Parameters.AddWithValue("@Size26", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size26", _size26);

                if (_size27 == null)
                    cm.Parameters.AddWithValue("@Size27", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size27", _size27);

                if (_size28 == null)
                    cm.Parameters.AddWithValue("@Size28", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size28", _size28);

                if (_size29 == null)
                    cm.Parameters.AddWithValue("@Size29", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size29", _size29);

                if (_size30 == null)
                    cm.Parameters.AddWithValue("@Size30", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Size30", _size30);

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
                cm.CommandText = "REFScale_Delete";

                cm.Parameters.AddWithValue("@ScaleKey", criteria._scaleKey);

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
                cm.CommandText = "REFScale_Validation";

                cm.Parameters.AddWithValue("@IsNew", isNew);
                cm.Parameters.AddWithValue("@ScaleKey", criteria._scaleKey);
                cm.Parameters.AddWithValue("@ScaleID", criteria._scaleID);

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

        private void Clear()
        {
            _scaleKey = 0;
            _scaleID = string.Empty;
            _scaleDes = string.Empty;
            _size1 = string.Empty;
            _size2 = string.Empty;
            _size3 = string.Empty;
            _size4 = string.Empty;
            _size5 = string.Empty;
            _size6 = string.Empty;
            _size7 = string.Empty;
            _size8 = string.Empty;
            _size9 = string.Empty;
            _size10 = string.Empty;
            _size11 = string.Empty;
            _size12 = string.Empty;
            _size13 = string.Empty;
            _size14 = string.Empty;
            _size15 = string.Empty;
            _size16 = string.Empty;
            _size17 = string.Empty;
            _size18 = string.Empty;
            _size19 = string.Empty;
            _size20 = string.Empty;
            _size21 = string.Empty;
            _size22 = string.Empty;
            _size23 = string.Empty;
            _size24 = string.Empty;
            _size25 = string.Empty;
            _size26 = string.Empty;
            _size27 = string.Empty;
            _size28 = string.Empty;
            _size29 = string.Empty;
            _size30 = string.Empty;
            _createDate = null;
            _createUserKey = null;
            _lastModifiedDate = null;
            _lastModifiedUserKey = null;
            _custom1 = string.Empty;
            _custom2 = string.Empty;
            _custom3 = string.Empty;

        }
    }
}
