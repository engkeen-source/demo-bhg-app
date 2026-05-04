

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using TAUtil;
using System.Text.RegularExpressions;

namespace BOLib
{
    [Serializable()]
    public class SECChangePasswordFactory : CommandBase
    {
        #region Member variables and constants

        private SECUser _SECUser = null;
        private GEnum.InstanceMode _instanceMode = GEnum.InstanceMode.Normal;
        private bool _isDirty = false;
        private bool _isValid = false;
        private bool _isNew = false;
        private bool _isReadOnly = false;
        private int _guID = 0;

        private bool _isChangedUserEmail = false;
        private bool _isChangedPassword = false;
        private bool _isChangedNewPassword = false;
        private bool _isChangedConfirmPassword = false;

        private string _userEmail = string.Empty;
        private string _password = string.Empty;
        private string _newPassword = string.Empty;
        private string _confirmPassword = string.Empty;

        public const GEnum.SystemCode constCodeKey = GEnum.SystemCode.Security_ChangePassword;
        public const string constPermID = GVar.PermissionID.Security_Password;

        // Custom Event Declaration 
        public GVar.DirtyEvent dirtyEvent = null;
        public GVar.ErrorEvent errorEvent = null;

        #endregion // Member variables and constant

        #region Factory Properties

        public SECUser ObjSECUser
        {
            get
            {
                return this._SECUser;
            }
            set
            {
                this._SECUser = value;
            }
        }
        internal GEnum.InstanceMode InstanceMode
        {
            get
            {
                return this._instanceMode;
            }
        }
        public bool IsDirty
        {
            get
            {
                return this._isDirty;
            }
        }
        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
        }
        public bool IsNew
        {
            get
            {
                return this._isNew;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
        }
        public int GUID
        {
            get
            {
                return this._guID;
            }
        }

        public string UserEmail
        {
            get
            {
                return this._userEmail;
            }
        }
        public string Password
        {
            get
            {
                return this._password;
            }
        }
        public string NewPassword
        {
            get
            {
                return this._newPassword;
            }
        }
        public string ConfirmPassword
        {
            get
            {
                return this._confirmPassword;
            }
        }
        #endregion

        //Constructors
        public SECChangePasswordFactory(GEnum.InstanceMode instanceMode)
        {
            try
            {
                _instanceMode = instanceMode;
                this.Initialisation();
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }

        }//Completed
        public bool Initialisation()
        {
            try
            {
                // Check Permission
                //if (!SECPermUtility.Any(constPermID, out this._isReadOnly, true))
                //    return false;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        // Get Instance GUID
                        if ((this._guID = SysOptionUtility.GetNewLockingGUID(cn)) == 0)
                        {
                            this._guID = -1;
                            return false;
                        }

                        // Locking
                        if (SysLockUtility.IsProcessLock(cn, false, GEnum.SysLockOption.ByCodKey, constCodeKey, this._guID))
                        {
                            this._guID = -1;
                            return false;
                        }

                        // Add Inprogress Lock
                        if (!SysLockUtility.AddInprogressLock(cn, true, this._guID, constCodeKey))
                        {
                            this._guID = -1;
                            return false;
                        }

                        // Commit Process                           
                        this._isNew = false;
                        this._isReadOnly = false;
                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }
                }
            return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        //Method
        public bool New()
        {
            #region Declaration
            bool restoreFlag = false;
            SECUser copySECUser = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();
                #endregion

                #region Check Security Permission
                //if (SECPermUtility.Any(constPermID, out this._isReadOnly, true) == false)
                //    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        // Remove all locks by GUID except inprogress Locking
                        if (!SysLockUtility.RemoveLockGUIDKeepIP(cn, true, GUID, constCodeKey))
                            return false;

                        // Call New for Header                            
                        this._SECUser = SECUser.New();


                        // Call New for Header                           
                        if (!this._SECUser.Fetch(cn, new SECUser.Criteria(AppInfor.currentUserKey, 1)))
                            throw new TAException(MsgID.Common.NewFail);

                        this._isDirty = false;
                        this._isReadOnly = true;
                        this._isNew = true;

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }
                }
                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._SECUser = copySECUser;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                #endregion
            }
        }//Completed
        public bool Save()
        {
            //For Master and Reference the RecordKey is obtain after saving. 
            //But we do not need to update this Record to the Record Detail as
            //the saving process will delete all details in the server and append the detail from local to server again
            //Note: this will only work if the detail in the server is never updated by other user.
            //So for example : Item Location, the above will not work
            #region Declaration
            bool restoreFlag = false;
            SECUser copySECUser = null;
            #endregion

            try
            {
                #region Make backup of objects for restore purpose
                if (!GFunc.IsNE(this._SECUser))
                    copySECUser = this._SECUser.Clone();
                #endregion

                #region Check Permission
                //if (!SECPermUtility.Perform(constPermID, true))
                //    return false;
                #endregion

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
                    {
                        cn.Open();

                        //Turn on restore flag to restore objects if any error occurs
                        restoreFlag = true;

                        if (!Validation(cn))
                            return false;

                        // Record Locking
                        if (!SysLockUtility.AddLock(cn, true, GUID, constCodeKey, AppInfor.currentUserKey))
                            return false;

                        this._confirmPassword = this._SECUser._userKey.ToString() + this._confirmPassword;
                       

                        this._SECUser._password = TAUtil.Encoder.Encode(this._confirmPassword);    
                       
                        if (!this._SECUser.CustomUpdate(cn, (int)GEnum.SECUserCustomUpdateOption.ChangePassword))
                            throw new TAException("Unable to save changes");

                        this._isDirty = false;
                        this._isNew = false;

                        if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();

                    }
                }


                //if (!SysOptionUtility.GetStr("CompanyName").ToUpper().StartsWith("ZZZ")) /* commented by YST at 2023/11/05 not to check sysOption CompanyName for testing DB */   //added by KKAung on 07 Jun 2023    // prevent updating password for all subsidiaries if CompanyName starts with "ZZZ"
                //#region "commented by Jane 06-Aug-2025. move this code to storeprocedure named SECPassword_UpdateSubsi . due to datetime format issue from user's pc"
                {
                    try
                    {
                        //After all validations passed and saved the password, update all subsidaries if user exists
                        using (SqlConnection cn = new SqlConnection(Database.BOSSSystemMasterConnection))
                        {
                            cn.Open();
                            DataTable dt = GFunc.ExecuteQuery(cn, "exec SECUser_GetAllDBUserKeys '" + this._SECUser._userID + "','" + AppInfor.CurrentDBID + "'"); /* modified by YST -- added AppInfor.CurrentDBID not to update PW to all of live-running subsidiaries if current login is tested DB */
                            if (dt.Rows.Count > 0)
                            {
                                string sql = "";
                                foreach (DataRow dr in dt.Rows)
                                {
                                    string DBNm = dr["DBName"].ToString();
                                    int userKey = GFunc.NEInt(dr["UserKey"], 0);
                                    string password = TAUtil.Encoder.Encode(userKey + this._newPassword);

                                    //sql += "INSERT INTO " + DBNm + ".dbo.SEC_PasswordHistory( UserKey, UserID, UserName, Password, PasswordDate, CreateDate, CreateUserKey, LastModifiedDate, LastModifiedUserKey, Custom1, Custom2, Custom3)" +
                                    //        " VALUES(" + userKey + ",'" + dr["UserID"].ToString() + "','" + dr["UserName"].ToString() + "','" + password + "',GetDate(),'"
                                    //        + GFunc.NEDateTime(dr["CreateDate"], DateTime.Today) + "'," + userKey + ",NULL,NULL,NULL,NULL,'" + dr["OldPassword"].ToString() + "')\n\r";
                                    //sql += "UPDATE " + DBNm + ".dbo.SEC_User set Password='" + password + "' where UserID='" + this._SECUser._userID + "'\n\r";

                                    // updated by KKAung on 07 Jun 2023 
                                    sql += "UPDATE " + DBNm + ".dbo.SEC_User set Password='" + password + "', UserEmail= '" + this._SECUser._userEmail + "' where UserID='" + this._SECUser._userID + "'" + System.Environment.NewLine;

                                    sql += "INSERT INTO " + DBNm + ".dbo.SEC_PasswordHistory( UserKey, UserID, UserName,[Password], PasswordDate, CreateDate, CreateUserKey, LastModifiedDate, LastModifiedUserKey, Custom1, Custom2, Custom3)" +
                                            " VALUES(" + userKey + ",'" + dr["UserID"].ToString() + "','" + dr["UserName"].ToString() + "','" + password + "',GetDate(),'"
                                            + GFunc.NEDateTime(dr["CreateDate"], DateTime.Today) + "'," + userKey + ",NULL,NULL,NULL,NULL,'" + dr["OldPassword"].ToString() + "')" + System.Environment.NewLine;
                                }

                                if (sql != "")
                                {
                                    GFunc.ExecuteNonQuery(cn, sql);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // added by KKAung on 07 Jun 2023 
                        if (!this.ObjSECUser.AddPasswordHistory(0, ObjSECUser._userID, GFunc.NEInt(ObjSECUser._userKey, 0)))
                            MsgBox.Show("Password changed successfully for the current subsidiary.<br/><font color='Red'>Failed to update other subsidiaries.Please contact the authorized person.</font>");
                    }
                }
                //#endregion

                // Audit Log                                            
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.Edit, constCodeKey, this._SECUser._userKey, this._SECUser._userID, GFunc.GetCodeKeyDescription((int)constCodeKey), new object[] { this._SECUser });

                restoreFlag = false;
                return true;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            finally
            {
                #region resetore data to Obj and dtTables
                if (restoreFlag == true)
                {
                    this._SECUser = copySECUser;
                }
                #endregion

                #region Dispose Backup Objects
                copySECUser = null;
                #endregion
            }
        }//Completed
        public bool Dispose()
        {
            try
            {
                if (!SysLockUtility.RemoveLock(true, GEnum.SysLockOption.ByGUID, constCodeKey, GUID, 0, 0))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        //Validation Method
        public bool Validation(SqlConnection cn)
        {
            bool isValidation = false;
            string msgID = BOLib.MsgID.Common.ValidationFail;
            string msgValue = string.Empty;
            this._isValid = false;
            try
            {
                PropertyChangedEventArgs e = null; new PropertyChangedEventArgs(string.Empty);

                if (this.InstanceMode == GEnum.InstanceMode.Normal)
                {
                    // Check Old Password Validation 
                    isValidation = BaseUtility.Validation(out msgID, this._password, "Password", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                    e = new PropertyChangedEventArgs("Password");

                    if (isValidation)
                    {
                        if (TAUtil.Decoder.Decode(this.ObjSECUser.Password) != (this._SECUser._userKey.ToString() + this._password))
                        {
                            isValidation = false;
                            msgID = MsgID.ChangePassword.PasswordIsNotEqual;
                            e = new PropertyChangedEventArgs("Password");
                        }
                    }

                    // Check New Password Validation
                    if (isValidation)
                    {
                        isValidation = BaseUtility.Validation(out msgID, this._newPassword, "NewPassword", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                        e = new PropertyChangedEventArgs("NewPassword");
                    }

                    // Check System Option Min & Max Length for New Password 
                    if (isValidation)
                    {
                        SYSOption objMinSYSOption = SYSOption.New();
                        bool minProcessOK = objMinSYSOption.Fetch(cn, new SYSOption.Criteria(GVar.SystemOption.OpID.PasswordMinLength, 1));

                        SYSOption objMaxSYSOption = SYSOption.New();
                        bool maxProcessOK = objMaxSYSOption.Fetch(cn, new SYSOption.Criteria(GVar.SystemOption.OpID.PasswordMaxLength, 1));

                        if ((minProcessOK) && (maxProcessOK))
                        {
                            if (Convert.ToInt32(objMinSYSOption.OpValue) > this._newPassword.Trim().Length || Convert.ToInt32(objMaxSYSOption.OpValue) < this._newPassword.Trim().Length)
                            {
                                isValidation = false;
                                msgID = MsgID.ChangePassword.PasswordMinMax + "%" + objMinSYSOption.OpValue.ToString() + "%" + objMaxSYSOption.OpValue.ToString() + ".\n";
                                e = new PropertyChangedEventArgs("NewPassword");
                            }
                        }

                        //added by KKAung on 8 Aug 2022 (start) /* modified by YST to show all requirements of new PW at one time */
                        if (!Regex.Match(this._newPassword, @"(?=.*[a-z])(?=.*[A-Z])").Success)
                        {
                            isValidation = false;
                            msgID += MsgID.ChangePassword.PasswordNotContainUpperLowerCharacters + "\n";
                            e = new PropertyChangedEventArgs("NewPassword");
                        }

                        if (!Regex.Match(this._newPassword, @"(?=.*[0-9])").Success)
                        {
                            isValidation = false;
                            msgID += MsgID.ChangePassword.PasswordNotContainDigit + "\n";
                            e = new PropertyChangedEventArgs("NewPassword");
                        }

                        if (!Regex.Match(this._newPassword, @"(?=.*[!@#$%^&*()_+|~=\\`{}\[\]:"";'<>?,./-])").Success)
                        {
                            isValidation = false;
                            msgID += MsgID.ChangePassword.PasswordNotContainSpecialCharacters + "\n";
                            e = new PropertyChangedEventArgs("NewPassword");
                        }

                        string password = TAUtil.Encoder.Encode(this._SECUser._userKey.ToString() + this._newPassword);
                        DataTable dtPwdList = null;
                        using (SqlCommand cm = cn.CreateCommand())
                        {
                            cm.CommandType = CommandType.StoredProcedure;
                            cm.CommandText = "SECPasswordHistory_Get";

                            cm.Parameters.AddWithValue("@Option", 1);
                            cm.Parameters.AddWithValue("@UserID", ObjSECUser.UserID);
                            cm.Parameters.AddWithValue("@UserKey", ObjSECUser.UserKey);
                            cm.Parameters.AddWithValue("@RetValue", 0);
                            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

                            SqlDataAdapter adap = new SqlDataAdapter(cm);
                            dtPwdList = new DataTable();
                            adap.Fill(dtPwdList);
                        }

                        DataRow[] dr = dtPwdList.Select("Password='" + password + "'");
                        if (dr.Count() > 0)
                        {
                            isValidation = false;
                            msgID += MsgID.ChangePassword.PasswordIsEqualLast3;
                            e = new PropertyChangedEventArgs("NewPassword");

                        }
                        // (end)
                    }                    


                    // Check Confirm Password Validation
                    if (isValidation)
                    {
                        isValidation = BaseUtility.Validation(out msgID, this._confirmPassword, "ConfirmPassword", GEnum.DataType.String, GEnum.Require.No, 50, null, null, null, null);
                        e = new PropertyChangedEventArgs("ConfirmPassword");
                    }                    

                    // Check New and Confirm Password Matching
                    if (isValidation)
                    {
                        if (this._newPassword != this._confirmPassword)
                        {
                            isValidation = false;
                            msgID = MsgID.ChangePassword.PasswordAreNotMatch;
                            e = new PropertyChangedEventArgs("ConfirmPassword");
                        }
                    }

                    // Check Email /* added by YST */
                    if (isValidation)
                    {
                        if (!GFunc.ValidateEmail(this._userEmail))
                        {
                            isValidation = false;
                            msgID = "Invalid Email!";
                            e = new PropertyChangedEventArgs("UserEmail");
                        }
                    }
                    
                    if (!isValidation)
                    {
                        this.errorEvent.Invoke(SysMessageUtility.Get(cn, msgID), e);
                        //throw new TAException(msgID);
                    }
                    else
                    {
                        msgID = string.Empty;
                        this._isValid = true;
                        isValidation = true;
                    }
                }
                return isValidation;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }//Completed

        //Unbound Controls Managed Code
        public void OnChangedUserEmail()
        {
            this._isChangedUserEmail = true;
            this._isDirty = true;
        }//Completed
        public void SetUserEmail(string value)
        {
            this._userEmail = value;
        }//Completed
        public void OnChangedPassword()
        {
            this._isChangedPassword = true;
            this._isDirty = true;
        }//Completed
        public void SetPassword(string value)
        {
            this._password = value;
        }//Completed
        public void OnChangedNewPassword()
        {
            this._isChangedNewPassword = true;
            this._isDirty = true;
        }//Completed
        public void SetNewPassword(string value)
        {
            this._newPassword = value;
        }//Completed
        public void OnChangedConfirmPassword()
        {
            this._isChangedConfirmPassword = true;
            this._isDirty = true;
        }//Completed
        public void SetConfirmPassword(string value)
        {
            this._confirmPassword = value;
        }//Completed

        //Error Exceptions
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] {  });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }//Completed
        private TAException Error(TAException ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }//Completed
    }
}
