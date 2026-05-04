

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
    public class SYSListSetting : Csla.BusinessBase<SYSListSetting>
    {
        #region Business Properties and Methods

        //declare members
        internal string _listID = string.Empty;
        internal string _listType = string.Empty;
        internal int? _userKey = null;
        internal string _listSQL = string.Empty;
        internal string _colName = string.Empty;
        internal string _colHeader1 = string.Empty;
        internal string _colHeader2 = string.Empty;
        internal string _colHeader3 = string.Empty;
        internal string _colHeader4 = string.Empty;
        internal string _colHeader5 = string.Empty;
        internal string _colHeader6 = string.Empty;
        internal string _colHeader7 = string.Empty;
        internal string _colHeader8 = string.Empty;
        internal string _colHeader9 = string.Empty;
        internal string _colHeader10 = string.Empty;
        internal string _colWidth = string.Empty;
        internal string _colFormat = string.Empty;
        internal string _colDataFormat = string.Empty;
        internal string _valueColName = string.Empty;
        internal string _displayColName = string.Empty;
        internal string _programCode = string.Empty;
        internal int? _rowHeight = null;

        public string ListID
        {
            get
            {
                return _listID;
            }
            set
            {
                _listID = value;
                PropertyHasChanged("ListID");
            }
        }

        public string ListType
        {
            get
            {
                return _listType;
            }
            set
            {
                _listType = value;
                PropertyHasChanged("ListType");
            }
        }

        public int? UserKey
        {
            get
            {
                return _userKey;
            }
            set
            {
                _userKey = value;
                PropertyHasChanged("UserKey");
            }
        }

        public string ListSQL
        {
            get
            {
                return _listSQL;
            }
            set
            {
                _listSQL = value;
                PropertyHasChanged("ListSQL");
            }
        }

        public string ColName
        {
            get
            {
                return _colName;
            }
            set
            {
                _colName = value;
                PropertyHasChanged("ColName");
            }
        }

        public string ColHeader1
        {
            get
            {
                return _colHeader1;
            }
            set
            {
                _colHeader1 = value;
                PropertyHasChanged("ColHeader1");
            }
        }

        public string ColHeader2
        {
            get
            {
                return _colHeader2;
            }
            set
            {
                _colHeader2 = value;
                PropertyHasChanged("ColHeader2");
            }
        }

        public string ColHeader3
        {
            get
            {
                return _colHeader3;
            }
            set
            {
                _colHeader3 = value;
                PropertyHasChanged("ColHeader3");
            }
        }

        public string ColHeader4
        {
            get
            {
                return _colHeader4;
            }
            set
            {
                _colHeader4 = value;
                PropertyHasChanged("ColHeader4");
            }
        }

        public string ColHeader5
        {
            get
            {
                return _colHeader5;
            }
            set
            {
                _colHeader5 = value;
                PropertyHasChanged("ColHeader5");
            }
        }

        public string ColHeader6
        {
            get
            {
                return _colHeader6;
            }
            set
            {
                _colHeader6 = value;
                PropertyHasChanged("ColHeader6");
            }
        }

        public string ColHeader7
        {
            get
            {
                return _colHeader7;
            }
            set
            {
                _colHeader7 = value;
                PropertyHasChanged("ColHeader7");
            }
        }

        public string ColHeader8
        {
            get
            {
                return _colHeader8;
            }
            set
            {
                _colHeader8 = value;
                PropertyHasChanged("ColHeader8");
            }
        }

        public string ColHeader9
        {
            get
            {
                return _colHeader9;
            }
            set
            {
                _colHeader9 = value;
                PropertyHasChanged("ColHeader9");
            }
        }

        public string ColHeader10
        {
            get
            {
                return _colHeader10;
            }
            set
            {
                _colHeader10 = value;
                PropertyHasChanged("ColHeader10");
            }
        }

        public string ColWidth
        {
            get
            {
                return _colWidth;
            }
            set
            {
                _colWidth = value;
                PropertyHasChanged("ColWidth");
            }
        }

        public string ColFormat
        {
            get
            {
                return _colFormat;
            }
            set
            {
                _colFormat = value;
                PropertyHasChanged("ColFormat");
            }
        }

        public string ColDataFormat
        {
            get
            {
                return _colDataFormat;
            }
            set
            {
                _colDataFormat = value;
                PropertyHasChanged("ColDataFormat");
            }
        }

        public string ValueColName
        {
            get
            {
                return _valueColName;
            }
            set
            {
                _valueColName = value;
                PropertyHasChanged("ValueColName");
            }
        }

        public string DisplayColName
        {
            get
            {
                return _displayColName;
            }
            set
            {
                _displayColName = value;
                PropertyHasChanged("DisplayColName");
            }
        }

        public string ProgramCode
        {
            get
            {
                return _programCode;
            }
            set
            {
                _programCode = value;
                PropertyHasChanged("ProgramCode");
            }
        }

        public int? RowHeight
        {
            get
            {
                return _rowHeight;
            }
            set
            {
                _rowHeight = value;
                PropertyHasChanged("RowHeight");
            }
        }

        protected override object GetIdValue()
        {
            return _listID.ToString() + _listType.ToString() + _userKey.ToString();
        }

        #endregion //Business Properties and Methods


        #region Factory Methods

        public SYSListSetting()
        { /* require use of factory method */ }

        public static SYSListSetting New()
        {
            
            SYSListSetting child = new SYSListSetting();
            
            return child;
        }

        public static SYSListSetting NewChild()
        {
            
            SYSListSetting child = new SYSListSetting();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        public static SYSListSetting Get(SafeDataReader dr)
        {
            
            SYSListSetting child = new SYSListSetting();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static SYSListSetting Get(string listID)
        {

            SYSListSetting child = new SYSListSetting();
            child.Fetch(new Criteria(listID, 3));
            return child;
        }

        public static SYSListSetting GetFormList(string listID)
        {
            SYSListSetting child = new SYSListSetting();
            if (child.Fetch(new Criteria(listID, 3)))
                return child;
            else
            {
                //MsgBox.Show(MsgID.Common.SysErr+"% MsgListID do not exist.");
                throw new TAException(MsgID.Common.SysErr + "% MsgListID do not exist.");
            }
        }
        public static SYSListSetting GetFormList(SqlConnection cn, string listID)
        {
            SYSListSetting child = new SYSListSetting();
            if (child.Fetch(cn,new Criteria(listID, 3)))
                return child;
            else
            {
                //MsgBox.Show(MsgID.Common.SysErr+"% MsgListID do not exist.");
                throw new TAException(MsgID.Common.SysErr + "% MsgListID do not exist.");
            }
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        public class Criteria
        {
            internal string _listID = string.Empty;
            public int? _option = null;
            internal string _listType = string.Empty;

            public Criteria()
            {
            }

            public Criteria(string ListID)
            {
                _listID = ListID;
            }

            public Criteria(string ListID, int? Option)
            {
                _listID = ListID;
                _option = Option;
            }



            public Criteria(string ListID, string ListType)
            {
                _listID = ListID;
                _listType = ListType;
            }

            public Criteria(string ListID, string ListType, int? Option)
            {
                _listID = ListID;
                _listType = ListType;
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

        internal bool Fetch(SqlConnection cn, Criteria criteria)
        {
            bool retValue = false;
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSListSetting_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@ListID", criteria._listID);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);

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



                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;

            }// Already close and dispose sql connection.
            return retValue;
        }

        internal bool Fetch(SafeDataReader dr)
        {
            _listID = dr.GetString("ListID");
            _listType = dr.GetString("ListType");
            _userKey = dr.GetInt32("UserKey");
            _listSQL = dr.GetString("ListSQL");
            _colName = dr.GetString("ColName");
            _colHeader1 = dr.GetString("ColHeader1");
            _colHeader2 = dr.GetString("ColHeader2");
            _colHeader3 = dr.GetString("ColHeader3");
            _colHeader4 = dr.GetString("ColHeader4");
            _colHeader5 = dr.GetString("ColHeader5");
            _colHeader6 = dr.GetString("ColHeader6");
            _colHeader7 = dr.GetString("ColHeader7");
            _colHeader8 = dr.GetString("ColHeader8");
            _colHeader9 = dr.GetString("ColHeader9");
            _colHeader10 = dr.GetString("ColHeader10");
            _colWidth = dr.GetString("ColWidth");
            _colFormat = dr.GetString("ColFormat");
            _colDataFormat = dr.GetString("ColDataFormat");
            _valueColName = dr.GetString("ValueColName");
            _displayColName = dr.GetString("DisplayColName");
            _programCode = dr.GetString("ProgramCode");
            _rowHeight = dr.GetInt32("RowHeight");

            ValidationRules.CheckRules();
            return true;
        }
        #endregion //Data Access - Fetch

        #region Data Access - Update

        public bool Update()
        {
            bool retValue = false;
            // Create Transaction Scope
            using (TransactionScope scope = new TransactionScope())
            {
                // Create SqlConnection
                using (SqlConnection cn = new SqlConnection(AppInfor.currentDBConnectionStr))
                {
                    // Open Connection
                    cn.Open();

                    // Call update method.
                    retValue = this.Update(cn);
                }// End of SqlConnection

                // No errors - commit transaction
                  if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
            }// End of TransactionScope

            if (retValue)
                return retValue;
            else
            {
                //MsgBox.Show(MsgID.Common.SysErr+"% MsgListID do not exist.");
                throw new Exception(MsgID.Common.SysErr + "% MsgListID do not exist.");
            }
        }

        public bool Update(SqlConnection cn)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSListSetting_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                //cm.Parameters.AddWithValue("@NewListID", string.Empty);

                if (_listID == null)
                    cm.Parameters.AddWithValue("@ListID", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ListID", _listID);

                if (_listType == null)
                    cm.Parameters.AddWithValue("@ListType", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ListType", _listType);

                if (AppInfor.currentUserKey == 0)
                    cm.Parameters.AddWithValue("@UserKey", 0);
                else
                    cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);

                if (_listSQL == null)
                    cm.Parameters.AddWithValue("@ListSQL", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ListSQL", _listSQL);

                if (_colName == null || _colName == string.Empty)
                    cm.Parameters.AddWithValue("@ColName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColName", _colName);

                if (_colHeader1 == null)
                    cm.Parameters.AddWithValue("@ColHeader1", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader1", _colHeader1);

                if (_colHeader2 == null || _colHeader2 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader2", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader2", _colHeader2);

                if (_colHeader3 == null || _colHeader3 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader3", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader3", _colHeader3);

                if (_colHeader4 == null || _colHeader4 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader4", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader4", _colHeader4);

                if (_colHeader5 == null || _colHeader5 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader5", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader5", _colHeader5);

                if (_colHeader6 == null || _colHeader6 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader6", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader6", _colHeader6);

                if (_colHeader7 == null || _colHeader7 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader7", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader7", _colHeader7);

                if (_colHeader8 == null || _colHeader8 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader8", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader8", _colHeader8);

                if (_colHeader9 == null || _colHeader9 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader9", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader9", _colHeader9);

                if (_colHeader10 == null || _colHeader10 == string.Empty)
                    cm.Parameters.AddWithValue("@ColHeader10", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader10", _colHeader10);

                if (_colWidth == null)
                    cm.Parameters.AddWithValue("@ColWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColWidth", _colWidth);

                if (_colFormat == null)
                    cm.Parameters.AddWithValue("@ColFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColFormat", _colFormat);

                if (_valueColName == null)
                    cm.Parameters.AddWithValue("@ValueColName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ValueColName", _valueColName);

                if (_displayColName == null)
                    cm.Parameters.AddWithValue("@DisplayColName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DisplayColName", _displayColName);

                if (_programCode == null || _programCode == string.Empty)
                    cm.Parameters.AddWithValue("@ProgramCode", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ProgramCode", _programCode);

                if (_rowHeight == null)
                    cm.Parameters.AddWithValue("@RowHeight", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@RowHeight", _rowHeight);

                //cm.Parameters["@NewListID"].Direction = ParameterDirection.Output;

                cm.ExecuteNonQuery();

               

                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;

            }// Already close and dispose sql connection.
            
        }
        #endregion //Data Access - Update

    }
}


