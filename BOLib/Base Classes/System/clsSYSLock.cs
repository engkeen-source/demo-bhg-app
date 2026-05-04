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
    public class SYSLock : Csla.BusinessBase<SYSLock>
    {
        #region Business Properties and Methods

        //declare members
        internal GEnum.SystemCode _codeKey ;
        internal int? _dataKey = 0;
        internal int? _inprogressKey = 0;
        internal int? _userKey = null;
        internal int? _guid = null;
        internal DateTime? _timeStamp = null;

        public GEnum.SystemCode CodeKey
        {
            get
            {
                return _codeKey;
            }
        }

        public int? DataKey
        {
            get
            {
                return _dataKey;
            }
        }

        public int? InprogressKey
        {
            get
            {
                return _inprogressKey;
            }
        }

        public int? UserKey
        {
            get
            {
                return _userKey;
            }
        }

        public int? GUID
        {
            get
            {
                return _guid;
            }
        }

        public DateTime? TimeStamp
        {
            get
            {
                return _timeStamp;
            }
        }

        protected override object GetIdValue()
        {
            return _codeKey.ToString() + _dataKey.ToString() + _inprogressKey.ToString();
        }

        #endregion //Business Properties and Methods

      
        #region Factory Methods

        internal SYSLock()
        { /* require use of factory method */ }

        internal static SYSLock New()
        {
            
            SYSLock child = new SYSLock();
            
            return child;
        }

        internal static SYSLock NewChild()
        {
            
            SYSLock child = new SYSLock();
            child.ValidationRules.CheckRules();
            child.MarkAsChild();
            
            return child;
        }

        internal static SYSLock Get(SafeDataReader dr)
        {
            
            SYSLock child = new SYSLock();
            child.MarkAsChild();
            child.Fetch(dr);
            return child;
        }

        internal static SYSLock Get(GEnum.SystemCode codeKey, int? dataKey, int? inprogressKey)
        {
            
            SYSLock child = new SYSLock();
            child.Fetch(new Criteria(codeKey, dataKey, inprogressKey, 1));
            return child;
        }

        #endregion //Factory Methods

        #region Criteria

        [Serializable()]
        internal class Criteria
        {
            public GEnum.SystemCode _codeKey ;
            public int? _dataKey = null;
            public int? _inprogressKey = null;
            public int? _option = null;
            public int? _userKey = null;
            public int? _GUID = null;

            internal Criteria()
            {
            }

            /// <summary>
            /// option 0- By All,
            /// option 1- By Code Key,
            /// option 2- By Code Key and Data Key, 
            /// option 3- By CodeKey=@CodeKey And DataKey=@DataKey And InprogressKey=@InprogressKey
            /// option 4- By GUID
            /// option 5- By CodeKey And GUID
            /// option 6- By CodeKey And GUID And Data Key
            /// option 7- By CodeKey And DataKey And InprogressKey And GUID
            /// option 8- By UserKey
            /// option 9- By CodeKey And UserKey
            /// option 10- By CodeKey And DataKey And UserKey
            /// option 11- By CodeKey And DataKey And InprogressKey And UserKey
            /// option 12- By CodeKey And GUID AND DataKey<>0
            /// </summary>
            /// <param name="CodeKey"></param>
            /// <param name="UserKey"></param>
            /// <param name="DataKey"></param>
            /// <param name="Option"> </param>
            internal Criteria(GEnum.SystemCode CodeKey, int? UserKey, int? DataKey, int? Option)
            {
                _userKey = UserKey;
                _codeKey = CodeKey;
                _dataKey = DataKey;
                _option = Option;
            }

            /// <summary>
            /// option 0- By All,
            /// option 1- By Code Key,
            /// option 2- By Code Key and Data Key, 
            /// option 3- By CodeKey=@CodeKey And DataKey=@DataKey And InprogressKey=@InprogressKey
            /// option 4- By GUID
            /// option 5- By CodeKey And GUID
            /// option 6- By CodeKey And GUID And Data Key
            /// option 7- By CodeKey And DataKey And InprogressKey And GUID
            /// option 8- By UserKey
            /// option 9- By CodeKey And UserKey
            /// option 10- By CodeKey And DataKey And UserKey
            /// option 11- By CodeKey And DataKey And InprogressKey And UserKey
            /// option 12- By CodeKey And GUID AND DataKey<>0
            /// </summary>
            /// <param name="CodeKey"></param>
            /// <param name="UserKey"></param>
            /// <param name="DataKey"></param>
            /// <param name="Option"> </param>
            internal Criteria(int? GUID, int? UserKey, GEnum.SystemCode CodeKey, int? DataKey, int? InprogressKey, int? Option)
            {
                _GUID = GUID;
                _userKey = UserKey;
                _codeKey = CodeKey;
                _dataKey = DataKey;
                _inprogressKey = InprogressKey;
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
                cm.CommandText = "SYSLock_Get";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                
                if (criteria._dataKey == null)
                    cm.Parameters.AddWithValue("@DataKey",DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataKey", criteria._dataKey);
               
                if (criteria._inprogressKey == null)
                    cm.Parameters.AddWithValue("@InprogressKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@InprogressKey", criteria._inprogressKey);

                if (criteria._GUID != null)
                    cm.Parameters.AddWithValue("@GUID", criteria._GUID);
                else
                    cm.Parameters.AddWithValue("@GUID", DBNull.Value);
                
                if(criteria._userKey!=null)
                    cm.Parameters.AddWithValue("@UserKey", AppInfor.currentUserKey);
                else
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);

                
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

                } // Already close and dispose data reader.

                
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    return true;
                else
                    return false;    

            }// Already close and dispose sql connection.
        }

        internal bool Fetch(SafeDataReader dr)
        {
            
            _codeKey =(GEnum.SystemCode) dr.GetInt32("CodeKey");
            _dataKey = dr.GetInt32("DataKey");
            _inprogressKey = dr.GetInt32("InprogressKey");
            _userKey = dr.GetInt32("UserKey");
            _guid = dr.GetInt32("GUID");
            _timeStamp = dr.GetDateTime("TimeStamp");
           // ValidationRules.CheckRules();
            return true;
        }           
   
        #endregion //Data Access - Fetch

        #region Data Access - Insert

        internal bool Insert()
        {
            bool retValue = false;
            // Create Transaction Scope           
            // Create SqlConnection
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open Connection
                cn.Open();

                // Call insert method.
                retValue = this.Insert(cn);
            }// End of SqlConnection               

            return retValue;
        }

        internal bool Insert(SqlConnection cn)
        {
            try //Added on purpose on 18-Aug-2011, SysLockUtility->AddPosting Lock needed to return false when primarykey error is raised
            {
            // Using existing sql connection.
                using (SqlCommand cm = cn.CreateCommand())
                {
                    // Get current user key
                    _userKey = AppInfor.currentUserKey;

                    cm.CommandType = CommandType.StoredProcedure;
                    cm.CommandText = "SYSLock_AddUpdate";

                    cm.Parameters.AddWithValue("@Option", 0);


                    if (_codeKey == null)
                        cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                    if (_dataKey == null)
                        cm.Parameters.AddWithValue("@DataKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@DataKey", _dataKey);

                    if (_inprogressKey == null)
                        cm.Parameters.AddWithValue("@InprogressKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@InprogressKey", _inprogressKey);

                    if (_userKey == null)
                        cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@UserKey", _userKey);

                    if (_guid == null)
                        cm.Parameters.AddWithValue("@Guid", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@Guid", _guid);

                    if (_timeStamp == null)
                        cm.Parameters.AddWithValue("@TimeStamp", DBNull.Value);
                    else
                        cm.Parameters.AddWithValue("@TimeStamp", _timeStamp.Value);

                    cm.Parameters.AddWithValue("@RetValue", 0);
                    cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();



                    if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                        return true;
                    else
                        return false;               
                }// Already close and dispose sql connection.
            }
            catch
            {
                
                return false;
            }           
        }

        #endregion //Data Access - Insert

        #region Data Access - Check Add Lock

        internal bool CheckAddLock()
        {
            bool retValue = false;            
           
            // Create SqlConnection
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open Connection
                cn.Open();

                // Call update method.
                retValue = this.CheckAddLock(cn);
            }// End of SqlConnection
             
            return retValue;
        }

        internal bool CheckAddLock(SqlConnection cn)
        {
           
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                // Get current user key
                _userKey = AppInfor.currentUserKey;

                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLock_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);                   

                if (_codeKey == null)
                    cm.Parameters.AddWithValue("@CodeKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@CodeKey", _codeKey);

                if (_dataKey == null)
                    cm.Parameters.AddWithValue("@DataKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@DataKey", _dataKey);

                if (_inprogressKey == null)
                    cm.Parameters.AddWithValue("@InprogressKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@InprogressKey", _inprogressKey);                    
                
                if (_userKey == null)
                    cm.Parameters.AddWithValue("@UserKey", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@UserKey", _userKey);

                if (_guid == null)
                    cm.Parameters.AddWithValue("@Guid", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@Guid", _guid);

                if (_timeStamp == null)
                    cm.Parameters.AddWithValue("@TimeStamp", DBNull.Value);
                else
                    cm.Parameters.AddWithValue("@TimeStamp", _timeStamp.Value);

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
          
            // Create SqlConnection
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open Connection
                cn.Open();

                // Call delete method.
                retValue = this.Delete(cn, criteria);
            }// End of SqlConnection
              
            return retValue;
        }

        internal bool Delete(SqlConnection cn, Criteria criteria)
        {            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLock_Delete";
                
                // Added by Richard
                cm.Parameters.AddWithValue("@Option", criteria._option);

                cm.Parameters.AddWithValue("@GUID", criteria._GUID);
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@DataKey", criteria._dataKey);
                cm.Parameters.AddWithValue("@InprogressKey", criteria._inprogressKey);
                cm.Parameters.AddWithValue("@UserKey", criteria._userKey);
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

        internal bool Validation(Criteria criteria,  bool isNew)
        {
            bool retValue = false;
       
            // Create SqlConnection
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open Connection
                cn.Open();

                // Call validation method.
                retValue = this.Validation(cn, criteria, isNew);
            }// End of SqlConnection

            
            return retValue;
        }

        internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
        {
            
            // Using existing sql connection.
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "SYSLock_Validation";

                cm.Parameters.AddWithValue("@Option", criteria._option);
                
                cm.Parameters.AddWithValue("@CodeKey", criteria._codeKey);
                cm.Parameters.AddWithValue("@DataKey", criteria._dataKey);
                cm.Parameters.AddWithValue("@InprogressKey", criteria._inprogressKey);

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
