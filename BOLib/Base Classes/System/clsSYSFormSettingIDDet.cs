

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
    public class SYSFormSettingIDDet : DataTable
    {
        #region Business Properties and Methods

        //declare members           
        
       

       

        #endregion //Business Properties and Methods


        #region Factory Methods

        public SYSFormSettingIDDet()
        { /* require use of factory method */ }

        public static SYSFormSettingIDDet New()
        {

            SYSFormSettingIDDet child = new SYSFormSettingIDDet();            
            return child;
        }

       

        public static SYSFormSettingIDDet Get()
        {

            SYSFormSettingIDDet child = new SYSFormSettingIDDet();            
            return child;
        }

        public static SYSFormSettingIDDet Get(string listID)
        {

            SYSFormSettingIDDet child = new SYSFormSettingIDDet();
            child.Fetch(new Criteria(listID, 2));
            return child;
        }

        public static SYSFormSettingIDDet GetFormList(SqlConnection cn, string listID)
        {

            SYSFormSettingIDDet child = new SYSFormSettingIDDet();
            child.Fetch(cn, new Criteria(listID, 2));
            return child;
        }

       
        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        public class Criteria
        {
            internal string _listID = string.Empty;
            public int? _option = null;
            internal int? _userKey = null;

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
           

            public Criteria(string ListID, int? userKey, int? Option)
            {
                _listID = ListID;
                _userKey = userKey;
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
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSFormSettingIDDet_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@ListID", criteria._listID);
                cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
                sqlAdp.Fill(this);
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;


            }// Already close and dispose sql connection.
           
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
            bool retValue = false;
           
            if (this.Rows.Count == 0)
            {
                return true;
            }
            foreach (DataRow dr in this.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    retValue = true;
                    continue;
                }
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSFormSettingIDDet_AddUpdate";
                    cm.Parameters.AddWithValue("@Option", 0);
                    cm.Parameters.AddWithValue("@ListID", dr["ListID"]);
                    cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);                  
                    cm.Parameters.AddWithValue("@ColFldNm", dr["ColFldNm"]);
                    cm.Parameters.AddWithValue("@ColSeq", dr["ColSeq"]);
                    cm.Parameters.AddWithValue("@ColHeader", dr["ColHeader"]);
                    cm.Parameters.AddWithValue("@ColWidth", dr["ColWidth"]);
                    cm.Parameters.AddWithValue("@ColPosition", dr["ColPosition"]);
                    cm.Parameters.AddWithValue("@ColTabStop", dr["ColTabStop"]);
                    cm.Parameters.AddWithValue("@ColDataFormat", dr["ColDataFormat"]);
                    cm.Parameters.AddWithValue("@ColAlignment", dr["ColAlignment"]);
                    cm.Parameters.AddWithValue("@ColHide", dr["ColHide"]);
                    cm.Parameters.AddWithValue("@ColShortCutMenu", dr["ColShortCutMenu"]);
                    cm.Parameters.AddWithValue("@ColFormNmPopUp", dr["ColFormNmPopUp"]);
                    cm.Parameters.AddWithValue("@ColListID", dr["ColListID"]);
                    cm.Parameters.AddWithValue("@ColControlType", dr["ColControlType"]);
                    cm.Parameters.AddWithValue("@ColControlMemberValue", dr["ColControlMemberValue"]);
                    cm.Parameters.AddWithValue("@ColControlMemberDisplay", dr["ColControlMemberDisplay"]);
                    cm.Parameters.AddWithValue("@ColControlLimitToList", dr["ColControlLimitToList"]);
                    cm.Parameters.AddWithValue("@ProgramCode", dr["ProgramCode"]);
                    
                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                    // Execute command.
                    cm.ExecuteNonQuery();
                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    {
                        retValue = true;
                    }
                }// Already close and dispose sql command.
            }

            return retValue;
            
        }
        #endregion //Data Access - Update

    }
}


