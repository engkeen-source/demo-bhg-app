

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
    public class SYSFormSettingID : Csla.BusinessBase<SYSFormSettingID>
    {
        #region Business Properties and Methods

        //declare members       
        internal string _listType = string.Empty;
        internal string _listID = string.Empty;
        internal int? _userKey = null;
        internal string _listSQL = string.Empty;
        internal string _spName = string.Empty;
        internal string _colFldName = string.Empty;
        internal string _colHeader = string.Empty;       
        internal string _colWidth = string.Empty;
        internal string _colPosition = string.Empty;
        internal string _colDataFormat = string.Empty;
        internal string _colAlignment = string.Empty;
        internal string _colHide = string.Empty;
        internal string _comboMemberValue = string.Empty;
        internal string _comboMemberDisplay = string.Empty;
        internal string _comboLimitToList = string.Empty;
        internal double _rowHeightCM = 0;

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

        public string ColFldName
        {
            get
            {
                return _colFldName;
            }
            set
            {
                _colFldName = value;
                PropertyHasChanged("_colFldName");
            }
        }

        public string ColHeader
        {
            get
            {
                return _colHeader;
            }
            set
            {
                _colHeader = value;
                PropertyHasChanged("_colHeader");
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

        public string ColPosition
        {
            get
            {
                return _colPosition;
            }
            set
            {
                _colPosition = value;
                PropertyHasChanged("_colPosition");
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
                return _comboMemberValue;
            }
            set
            {
                _comboMemberValue = value;
                PropertyHasChanged("_comboMemberValue");
            }
        }

        public string DisplayColName
        {
            get
            {
                return _comboMemberDisplay;
            }
            set
            {
                _comboMemberDisplay = value;
                PropertyHasChanged("_comboMemberDisplay");
            }
        }

        public double RowHeightCM
        {
            get
            {
                return _rowHeightCM;
            }
            set
            {
                _rowHeightCM = value;
                PropertyHasChanged("_rowHeightCM");
            }
        }

        protected override object GetIdValue()
        {
            return _listID.ToString() + _listType.ToString() + _userKey.ToString();
        }

        #endregion //Business Properties and Methods


        #region Factory Methods

        public SYSFormSettingID()
        { /* require use of factory method */ }

        public static SYSFormSettingID New()
        {
            
            SYSFormSettingID child = new SYSFormSettingID();
            
            return child;
        }

        public static SYSFormSettingID NewChild()
        {
            
            SYSFormSettingID child = new SYSFormSettingID();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        public static SYSFormSettingID Get(SafeDataReader dr)
        {
            
            SYSFormSettingID child = new SYSFormSettingID();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        public static SYSFormSettingID Get(string listID)
        {

            SYSFormSettingID child = new SYSFormSettingID();
            child.Fetch(new Criteria(listID, 3));
            return child;
        }

        public static SYSFormSettingID GetFormList(string listID)
        {
            SYSFormSettingID child = new SYSFormSettingID();
            if (child.Fetch(new Criteria(listID, 3)))
                return child;
            else
            {
                //MsgBox.Show(MsgID.Common.SysErr+"% MsgListID do not exist.");
                throw new TAException(MsgID.Common.SysErr + "% MsgListID do not exist.");
            }
        }
        public static SYSFormSettingID GetFormList(SqlConnection cn, string listID)
        {
            SYSFormSettingID child = new SYSFormSettingID();
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
                cm.CommandText = "SYSFormSettingID_Get";

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
            _colFldName = dr.GetString("ColFldNm");
            _colHeader = dr.GetString("ColHeader");          
            _colWidth = dr.GetString("ColWidth");
            _colPosition = dr.GetString("ColPosition");
            _colDataFormat = dr.GetString("ColDataFormat");
            _comboMemberDisplay = dr.GetString("ComboMemberDisplay");
            _comboMemberValue = dr.GetString("ComboMemberValue");
            _rowHeightCM = Convert.ToDouble(dr.GetDecimal("RowHeightCM"));

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
                cm.CommandText = "SYSFormSettingID_AddUpdate";

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

                if (_colFldName == null || _colFldName == string.Empty)
                    cm.Parameters.AddWithValue("@ColFldNm", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColFldNm", _colFldName);

                if (_colHeader == null)
                    cm.Parameters.AddWithValue("@ColHeader", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColHeader", _colHeader);               

                if (_colWidth == null)
                    cm.Parameters.AddWithValue("@ColWidth", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColWidth", _colWidth);

                if (_colPosition== null)
                    cm.Parameters.AddWithValue("@ColPosition", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColPosition", _colPosition);

                if (_colDataFormat == null)
                    cm.Parameters.AddWithValue("@ColDataFormat", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ColDataFormat", _colDataFormat);

                if (_comboMemberValue == null)
                    cm.Parameters.AddWithValue("@ValueColName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@ValueColName", _comboMemberValue);

                if (_comboMemberDisplay == null)
                    cm.Parameters.AddWithValue("@DisplayColName", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DisplayColName", _comboMemberDisplay);

                cm.Parameters.AddWithValue("@RowHeightCM", _rowHeightCM);

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


