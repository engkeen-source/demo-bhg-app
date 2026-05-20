
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Collections;
using System.Windows.Forms;
using TAUtil;

namespace BOLib
{
    [Serializable()]
    public class LoginFactory : CommandBase
    {
        #region Member variables and constants

        private string _workStationIdentifier;
        private string _databaseID;
        private string _userID;
        private string _password;

        private string _lastDatabaseID;
        private bool _rememberMe;

        private string _lastLoginIdentifier;
        private DateTime? _lastLoginTimeStamp;
        private string _ConnectionString; //This value will be set only when the login factory is called from Agent Service
        public bool CallFromAgent = false;


        #endregion // Member variables and constant

        #region Factory Properties

        public string WorkStationIdentifier
        {
            get
            {
                return this._workStationIdentifier;
            }
        }

        public bool RememberMe
        {
            get
            {
                return this._rememberMe;
            }
            set
            {
                this._rememberMe = value;
            }
        }

        public string DatabaseID
        {
            get
            {
                return this._databaseID;
            }

            set
            {
                this._databaseID = value;
            }
        }

        public string UserID
        {
            get
            {
                return this._userID;
            }

            set
            {
                this._userID = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }

            set
            {
                this._password = value;
            }
        }

        public string LastLoginIdentifier
        {
            get
            {
                return this._lastLoginIdentifier;
            }
        }

        public DateTime? LastLoginTimeStamp
        {
            get
            {
                return this._lastLoginTimeStamp;
            }
        }

        public string ConnectionString //This value will be set only when the login factory is called from Agent Service
        {
            set
            {
                _ConnectionString = value;
            }
            get
            {
                return _ConnectionString;
            }
        }

        #endregion // Constructors

        #region Constructors


        /// <summary>
        /// Default constructor for this Factory.
        /// </summary>
        public LoginFactory()
        {
            try
            {
                Initialisation();
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }
        //For EStoreImport
        public LoginFactory(string ConnectionStr)
        {
            try
            {               
                this._ConnectionString = ConnectionStr; 
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        #endregion // Constructors

        #region Initialization Method

        public bool Initialisation()
        {
            bool isInitialisation = false;
            string msgID = MsgID.Common.InitialisationFail;

            try
            {
                AppInfor.GetVersion();
                if (GetAppResourcesData())
                {
                    _workStationIdentifier = AppInfor.GetMachineIP();
                    msgID = string.Empty;
                    isInitialisation = true;
                }
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            return isInitialisation;
        }

        #endregion //Initialisation Method

        #region Private Functions


        private bool GetAppResourcesData()
        {
            try
            {
                string fileName = Application.StartupPath+ @"\resUser.resx";
              
                using (ResXResourceReader resxReader = new ResXResourceReader(fileName))
                {
                    string lastUser = "";
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        if (((string)entry.Key).Equals("RememberMe"))
                            RememberMe = GFunc.NEBool(entry.Value.ToString(), false);
                        else if (((string)entry.Key).Equals("LastUserID"))
                            lastUser = entry.Value.ToString();
                        else if (((string)entry.Key).Equals("LastDatabaseID"))
                            _databaseID = entry.Value.ToString();
                    }

                    if (RememberMe)
                        _userID = lastUser;
                }
                //try
                //{
                //    if (Properties.Settings.Default.RememberMe)
                //    {
                //        _userID = Properties.Settings.Default.LastUserID;
                //    }
                //    _databaseID = Properties.Settings.Default.LastDatabaseID;
                //    _rememberMe = Properties.Settings.Default.RememberMe;
                //}
                //catch
                //{

                //}    

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
        }
        #endregion

        #region +++public option+++

        public bool SaveDatabaseConnectionList()
        {
            string msgID = MsgID.Common.SaveFail;
            try
            {
                string fileName = Application.StartupPath + @"\resUser.resx";              

                using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    ResXResourceWriter RwX = new ResXResourceWriter(stream);

                    RwX.AddResource("RememberMe", _rememberMe);
                    RwX.AddResource("LastDatabaseID", _databaseID);
                    if (_rememberMe)
                        RwX.AddResource("LastUserID", _userID);
                    RwX.Close();
                    stream.Close();
                }
                //try
                //{
                //    if (_rememberMe)
                //    {
                //        Properties.Settings.Default.LastUserID = _userID;
                //    }
                //    Properties.Settings.Default.RememberMe = _rememberMe;
                //    Properties.Settings.Default.LastDatabaseID = _databaseID;
                //    Properties.Settings.Default.Save();
                //}
                //catch
                //{                    

                //}
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                msgID = "ConfigurationFileSaveFail";
                throw Error(ex);
            }
            msgID = string.Empty;
            return true;
        }

        public DataTable GetCompanyList()
        {
            string msgID = MsgID.Common.GetFail;
            try
            {
                DataTable dt = null;
                dt = REFCmpList.Get();
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        msgID = string.Empty;
                        return dt;
                    }

                return null;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public bool Login()
        {
            string msgID = MsgID.Login.LoginFail;

            string innerMsgID = string.Empty;            
            //Declaration

            //**************************************         

            int loginRetry = 0;
            int loginRetryTimeOut = 0;
            
            try
            {
                if (!CallFromAgent) //_ConnectionString is already set when this factory is called from Agent
                {
                    //Validation Check

                    //**************************************
                    if (GFunc.IsNE(_databaseID))
                    {                        
                        MessageBox.Show(MsgID.Login.DataBaseIDIsEmpty);
                        return false;
                    }

                    _ConnectionString = GetCurrentConnectionStr();
                }

               
                SECUserFactory objSecUserFactory = null;
                //Check Database Connection

                //**************************************               
                //If cannot make connection to database 
                msgID = MsgID.Login.DatabaseConnectionStrInValid;
                try
                {
                    using (SqlConnection cn = new SqlConnection(_ConnectionString))
                    {
                        cn.Open();
                        cn.Close();
                    }

                    AppInfor.Initialisation();
                    AppInfor.currentDBConnectionStr = _ConnectionString;

                    msgID = string.Empty;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }

                //Check User ID
                if (GFunc.IsNE(_userID))
                {
                    MsgBox.Show("UserID" + MsgID.Validation.IsRequire);
                    return false;
                }

                //Check Database RegistrationCode

                //**************************************

                if (!GFunc.IsDatabaseRegCodeIsValid()) return false;



                //Check Database version
                //**************************************               

                string[] dbVersionFromDatabase = SysOptionUtility.GetStr("DatabaseVersion").Split('.');
                string[] dbVersionFromApp = AppInfor.DatabaseVersion.Split('.');

                if(dbVersionFromApp.Length==4 && dbVersionFromDatabase.Length==4)
                {
                    for(int i=0;i<4;i++)
                    {
                        if (!dbVersionFromDatabase[i].Equals(dbVersionFromApp[i]))
                        {
                            MsgBox.Show("Auto update is not working on your computer!\n Please contact technical support to get the latest version.");
                            Application.Exit();
                        }
                    }
                }
                else 
                {
                    MsgBox.Show("Auto update is not working on your computer!\n Please contact technical support to get the latest version.");
                    Application.Exit();
                }


                //Check User IsValid and Load User record

                //**************************************

                objSecUserFactory = new SECUserFactory(GEnum.InstanceMode.InternalCall);
                //Added Samuel
                if (!objSecUserFactory.UserIDCheck(_userID))
                    MsgBox.Show(innerMsgID);
                //End
                if (!objSecUserFactory.GetUserInforForLogin(_userID))
                    MsgBox.Show(innerMsgID);
                

                //Check User is already Login               
                if (!GFunc.IsNE(objSecUserFactory.ObjSECUser._loginIdentifierCurrent) || !GFunc.IsNE(objSecUserFactory.ObjSECUser._loginTimeStampCurrent))
                {
                    if (objSecUserFactory.ObjSECUser._userID != "ALVIN")
                    {
                        //if (MsgBox.Show(MsgID.Login.UserIsAlredayLogin + "\n Would you like to clear the lock for user " + _userID + "?", GEnum.MsgBoxIcon.Serious, new GEnum.MsgBoxButton[] { GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No }) == GEnum.MsgBoxButton.Yes)
                        if (MsgBox.Show("Login user - " + _userID + " has not logged out properly from the BOSS System. <br/> Unsaved data from the existing login window might be discarded.<br/> <font color='Red'> Would you like to exit and continue with the new login window?</font>", GEnum.MsgBoxIcon.Serious, new GEnum.MsgBoxButton[] { GEnum.MsgBoxButton.Continue, GEnum.MsgBoxButton.No }) == GEnum.MsgBoxButton.Continue)
                        {
                            string pswd = string.Empty;
                            //if (GlobalBO.PasswordInputBox("Clear active user", "Enter system password to clear the current locked user:", ref pswd) == DialogResult.OK)
                            {
                                pswd = "techace"; /* commented and modified by YST on 2023/07/26 suggested by Elieen that puplic password should not be used */
                                //int tryCount = 0;
                                while (pswd != "techace")
                                {
                                    MsgBox.Show("Wrong Password!");
                                    if (GlobalBO.PasswordInputBox("Clear active user", "Enter system password to clear the current locked user:", ref pswd) == DialogResult.Cancel)
                                        break;
                                }
                                if (pswd == "techace")
                                {
                                    if (objSecUserFactory.ClearLoginAndLocks() == false)
                                    {
                                        MsgBox.Show("Cannot clear lock for the user.");
                                        return false;
                                    }
                                }
                                else // User cancel
                                {
                                    return false;
                                }
                            }
                            //else
                            //    return false;
                        }
                        else
                            return false;
                    }
                    else
                        objSecUserFactory.ClearLoginAndLocks();                   
                }

                // Check User IsDisabled

                //**************************************

                if ((bool)objSecUserFactory.ObjSECUser.AccDisabled)
                {
                    MsgBox.Show(MsgID.Login.UserIsDisabled);
                    return false;
                }

                loginRetryTimeOut = SysOptionUtility.GetInt("LoginRetryTimeOut");
                loginRetry = SysOptionUtility.GetInt("LoginRetry");


                // Check User IsLockOut and Clear Lockout if TimeOut has expired

                //**************************************

                if ((bool)objSecUserFactory.ObjSECUser.AccLockOut)
                {
                    TimeSpan tsMaxLoginRetry = new TimeSpan(0, loginRetryTimeOut, 0);
                    DateTime now = Convert.ToDateTime(GFunc.ExecuteQuery("Select GetDate()").Rows[0][0]);
                    if (now > objSecUserFactory.ObjSECUser.AccLockOutTimeStamp.Value.Add(tsMaxLoginRetry))
                    {
                        objSecUserFactory.ResetUserLoginRetry(_userID);
                    }
                    else
                    {
                        MsgBox.Show(MsgID.Login.UserIsLockout);
                        return false;
                    }
                }


                //Check PasswordIsValid

                //**************************************
               if (GFunc.IsPasswordValid(objSecUserFactory.ObjSECUser.UserKey.ToString() + _password, objSecUserFactory.ObjSECUser.Password))
                {
                    AppInfor.securityKey = Guid.NewGuid();

                    if (!objSecUserFactory.SaveUserLogin((int)objSecUserFactory.ObjSECUser.UserKey))
                        return false;

                }
                else
                {
                    //Process was not ok already
                    string vTmpNextMsg = string.Empty;
                    bool innerProcessOk;
                    
                    if (objSecUserFactory.ObjSECUser.LoginRetries >= loginRetry)
                    {
                        msgID = MsgID.Login.UserIsLockout;
                        innerProcessOk = objSecUserFactory.SetUserLockout((int)objSecUserFactory.ObjSECUser.UserKey);
                        GFunc.AlertCheck(GEnum.AlertApplyTo.Wrongpasswordlockouts, objSecUserFactory.ObjSECUser);
                        MsgBox.Show(MsgID.Login.UserIsLockout);
                    }
                    else
                    {
                        innerProcessOk = objSecUserFactory.IncrementUserLoginRetry((int)objSecUserFactory.ObjSECUser.UserKey);

                        string WarningMsg = ""; /* added by YST at 2023/11/10 , LoginRetries => 0 to 4 , loginRetry = 5 */                       
                        if (objSecUserFactory.ObjSECUser.LoginRetries + 1 == loginRetry - 1)
                        {
                            WarningMsg = "<br/><br/><font color='red'>You have attempted to log in with an invalid password " + (objSecUserFactory.ObjSECUser.LoginRetries + 1).ToString() + " times already."
                                       + "<br/>If " + loginRetry + "th attempt is unsuccessful, your next login will be locked.</font>";
                            MsgBox.Show("Invalid Password" + WarningMsg, GEnum.MsgBoxIcon.Warning, GEnum.MsgBoxButton.OK);
                        }
                        else if (objSecUserFactory.ObjSECUser.LoginRetries + 1 == loginRetry)
                        {
                            WarningMsg = "<br/><br/><font color='red'>Sorry, your " + loginRetry + "th attempt was also unsuccessful, your login has been locked temporarily.</font>";
                            MsgBox.Show("Invalid Password" + WarningMsg, GEnum.MsgBoxIcon.Serious, GEnum.MsgBoxButton.OK);
                        }
                        else
                        {                            
                            MsgBox.Show("Invalid Password" , GEnum.MsgBoxIcon.Information, GEnum.MsgBoxButton.OK);
                        }                        
                    }
                    return false;
                }

                //Set Values in ApplicationInforFactory

                // **************************************               

                AppInfor.currentUserKey = (int)objSecUserFactory.ObjSECUser.UserKey;

                AppInfor.currentUserID = objSecUserFactory.ObjSECUser.UserID;
                AppInfor.currentUserName = objSecUserFactory.ObjSECUser.UserName;
                AppInfor.conAccessLevel = (int)objSecUserFactory.ObjSECUser._conAccessLevel;
                AppInfor.conAccessGroup = (int)objSecUserFactory.ObjSECUser._conAccessGrp;
                AppInfor.itemAccessLevel = (int)objSecUserFactory.ObjSECUser._itmAccessLevel;
                AppInfor.itemAccessGroup = (int)objSecUserFactory.ObjSECUser._itmAccessGrp;
                AppInfor.jobAccessLevel = (int)objSecUserFactory.ObjSECUser._jobAccessLevel;
                AppInfor.jobAccessGroup = (int)objSecUserFactory.ObjSECUser._jobAccessGrp;
                AppInfor.currentDBID = DatabaseID;
                AppInfor.branchKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._branchKey) ? 0 : objSecUserFactory.ObjSECUser._branchKey.Value;
                AppInfor.deptKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._deptKey) ? 0 : objSecUserFactory.ObjSECUser._deptKey.Value;
                AppInfor.tranGrpKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._tranGrpKey) ? 0 : objSecUserFactory.ObjSECUser._tranGrpKey.Value;
                AppInfor.emKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._emKey) ? 0 : objSecUserFactory.ObjSECUser._emKey.Value;

                SECPermUtility.LoadAllUserPermission(out msgID);

                LoadGlobalOptions();

                this._lastLoginIdentifier = objSecUserFactory.ObjSECUser._loginIdentifierLast;
                this._lastLoginTimeStamp = objSecUserFactory.ObjSECUser._loginTimeStampLast;

                //objSecUserFactory.ClearLoginAndLocks();

                if (!CallFromAgent)
                {
                    //Save Database Connection list to Configuration File

                    //**************************************
                    if (!SaveDatabaseConnectionList())
                        return false;
                }

                //Add to Audit Log

                //**************************************
                //if (ProcessOK)
                string pw = this.Password;
                this.Password = "XXXXXXXXXXXX";
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.NA, GEnum.SystemCode.Security_User, AppInfor.currentUserKey, AppInfor.currentUserID, new object[] { this });
                this.Password = pw;
                //else
                //{
                //    string vmsgID = string.Empty;
                //    SysAuditLogUtility.AddErrorLog(out vmsgID, GEnum.AuditLogMode.NA, GEnum.SystemCode.Security_User, msgID, this);
                //    throw new TAException(vmsgID);
                //}

                //Return Status to Caller

                //**************************************

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
        }

        public bool Login(int userKey) //
        {
            try
            {
                return Login(userKey, false);
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
            
        }
        public bool Login(int userKey, bool ForceLogin) //Duplicate Login is allowed when calling from agent
        {
            string msgID = MsgID.Login.LoginFail;

            string innerMsgID = string.Empty;
            //Declaration

            //**************************************         

            int loginRetry = 0;
            int loginRetryTimeOut = 0;
            SECUserFactory objSecUserFactory = null;
            string ConnStr;

            if (this._ConnectionString == string.Empty)
            {
                //Validation Check

                //**************************************
                if (GFunc.IsNE(_databaseID))
                {
                    MsgBox.Show(MsgID.Login.DataBaseIDIsEmpty);
                    return false;
                }

                ConnStr = GetCurrentConnectionStr();
            }
            else
                ConnStr = this._ConnectionString;

            try
            {

                //Check Database Connection

                //**************************************               
                //If cannot make connection to database 
                msgID = MsgID.Login.DatabaseConnectionStrInValid;
                try
                {
                    using (SqlConnection cn = new SqlConnection(ConnStr))
                    {
                        cn.Open();
                        cn.Close();
                    }


                    AppInfor.Initialisation();
                    AppInfor.currentDBConnectionStr = ConnStr;

                    msgID = string.Empty;
                }
                catch (Exception e)
                {
                    MsgBox.Show(e.Message);
                    return false;
                }

                //Check Database RegistrationCode
                //**************************************

                GFunc.IsDatabaseRegCodeIsValid();

                //Check Database version    
                //**************************************

                string optValue = SysOptionUtility.GetStr("DatabaseVersion");
                if (!AppInfor.DatabaseVersion.Equals(optValue))
                {
                    MsgBox.Show(MsgID.Login.DatabaseVersionInValid);
                    return false;
                }


                //Check User IsValid and Load User record
                //**************************************

                objSecUserFactory = new SECUserFactory(GEnum.InstanceMode.InternalCall);

                if (!objSecUserFactory.GetUserInforForLogin(userKey))
                    MsgBox.Show(innerMsgID);


                if (!ForceLogin)//Added by May on 16-Feb-2011, To skip already Login checking when calling from Agent
                {
                    //Check User is already Login       
                    if (!GFunc.IsNE(objSecUserFactory.ObjSECUser._loginIdentifierCurrent) || !GFunc.IsNE(objSecUserFactory.ObjSECUser._loginTimeStampCurrent))
                    {
                        MsgBox.Show(MsgID.Login.UserIsAlredayLogin, GEnum.MsgBoxIcon.Serious);
                        return false;
                    }
                }

                // Check User IsDisabled

                //**************************************

                if ((bool)objSecUserFactory.ObjSECUser.AccDisabled)
                {
                    MsgBox.Show(MsgID.Login.UserIsDisabled);
                    return false;
                }

                loginRetryTimeOut = SysOptionUtility.GetInt("LoginRetryTimeOut");
                loginRetry = SysOptionUtility.GetInt("LoginRetry");


                // Check User IsLockOut and Clear Lockout if TimeOut has expired

                //**************************************

                if ((bool)objSecUserFactory.ObjSECUser.AccLockOut)
                {
                    TimeSpan tsMaxLoginRetry = new TimeSpan(0, loginRetryTimeOut, 0);
                    DateTime now = Convert.ToDateTime(GFunc.ExecuteQuery("Select GetDate()").Rows[0][0]);
                    if (now > objSecUserFactory.ObjSECUser.AccLockOutTimeStamp.Value.Add(tsMaxLoginRetry))
                    {
                        objSecUserFactory.ResetUserLoginRetry(_userID);

                    }
                    else
                    {
                        MsgBox.Show(MsgID.Login.UserIsLockout);
                        return false;
                    }
                }

                //May changed for Fin Report popup on 25 Nov 2014
                if ((GFunc.IsNE(objSecUserFactory.ObjSECUser._loginIdentifierCurrent) || GFunc.IsNE(objSecUserFactory.ObjSECUser._loginTimeStampCurrent)))//May changed for Fin Report popup on 25 Nov 2014 (added function)
                {
                    AppInfor.securityKey = Guid.NewGuid();
                    if (!objSecUserFactory.SaveUserLogin((int)objSecUserFactory.ObjSECUser.UserKey))
                        return false;
                }
                else if (ForceLogin)
                    AppInfor.securityKey = objSecUserFactory.ObjSECUser.SecurityKey.Value;

                //Set Values in ApplicationInforFactory

                //**************************************               
                AppInfor.currentUserKey = (int)objSecUserFactory.ObjSECUser.UserKey;
                AppInfor.currentUserID = objSecUserFactory.ObjSECUser.UserID;
                AppInfor.currentUserName = objSecUserFactory.ObjSECUser.UserName;
                AppInfor.currentDBID = DatabaseID;
                AppInfor.conAccessLevel = (int)objSecUserFactory.ObjSECUser._conAccessLevel;
                AppInfor.conAccessGroup = (int)objSecUserFactory.ObjSECUser._conAccessGrp;
                AppInfor.itemAccessLevel = (int)objSecUserFactory.ObjSECUser._itmAccessLevel;
                AppInfor.itemAccessGroup = (int)objSecUserFactory.ObjSECUser._itmAccessGrp;
                AppInfor.jobAccessLevel = (int)objSecUserFactory.ObjSECUser._jobAccessLevel;
                AppInfor.jobAccessGroup = (int)objSecUserFactory.ObjSECUser._jobAccessGrp;

                AppInfor.branchKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._branchKey) ? 0 : objSecUserFactory.ObjSECUser._branchKey.Value;
                AppInfor.deptKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._deptKey) ? 0 : objSecUserFactory.ObjSECUser._deptKey.Value;
                AppInfor.tranGrpKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._tranGrpKey) ? 0 : objSecUserFactory.ObjSECUser._tranGrpKey.Value;
                AppInfor.emKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._emKey) ? 0 : objSecUserFactory.ObjSECUser._emKey.Value;

                SECPermUtility.LoadAllUserPermission(out msgID);

                LoadGlobalOptions();

                this._lastLoginIdentifier = objSecUserFactory.ObjSECUser._loginIdentifierLast;
                this._lastLoginTimeStamp = objSecUserFactory.ObjSECUser._loginTimeStampLast;

                if (!ForceLogin)//May changed for Fin Report popup on 25 Nov 2014 (added if Statement)
                {
                    objSecUserFactory.ClearLoginAndLocks();
                }


                //Save Database Connection list to Configuration File

                //**************************************


                if (!SaveDatabaseConnectionList())
                    return false;


                //Add to Audit Log

                //**************************************
                //if (ProcessOK)
                SysAuditLogUtility.AddAuditLog(GEnum.AuditLogMode.NA, GEnum.SystemCode.Security_User, AppInfor.currentUserKey, AppInfor.currentUserID, new object[] { this });
                //else
                //{
                //    string vmsgID = string.Empty;
                //    SysAuditLogUtility.AddErrorLog(out vmsgID, GEnum.AuditLogMode.NA, GEnum.SystemCode.Security_User, msgID, this);
                //    throw new TAException(vmsgID);
                //}

                //Return Status to Caller

                //**************************************

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
        }

        public bool Login(string LoginUserID,string AuthString) //Login from EStore Import. Cannot use MsgBox as it is calling from web service
        {
            string msgID = MsgID.Login.LoginFail;
            string innerMsgID = string.Empty;          
            SECUserFactory objSecUserFactory = null;
            string ConnStr = this._ConnectionString;

            if (AuthString != "") //will use an authentication string in future
                throw new Exception("Authentication failed");         

            try
            {

                //Check Database Connection

                //**************************************               
                //If cannot make connection to database 
                msgID = MsgID.Login.DatabaseConnectionStrInValid;
                try
                {
                    using (SqlConnection cn = new SqlConnection(ConnStr))
                    {
                        cn.Open();
                        cn.Close();
                    }
                    
                    AppInfor.currentDBConnectionStr = ConnStr;

                    msgID = string.Empty;
                }
                catch (Exception e)
                {
                    throw (e);
                }

                //Check Database RegistrationCode
                //**************************************

                GFunc.IsDatabaseRegCodeIsValid();

                //Check Database version    
                //**************************************               
                string dbVersionFromDatabase = SysOptionUtility.GetStr("DatabaseVersion");

                if (dbVersionFromDatabase.Split('.')[0] != AppInfor.DatabaseVersion.Split('.')[0] || dbVersionFromDatabase.Split('.')[1] != AppInfor.DatabaseVersion.Split('.')[1])
                {
                    MsgBox.Show("You have connected to wrong database version!\n Please select the correct database or contact technical support");
                    Application.Exit();
                }

                objSecUserFactory = new SECUserFactory(GEnum.InstanceMode.InternalCall);

                if (!objSecUserFactory.GetUserInforForLogin(LoginUserID))
                    throw new Exception("User not exists!");               

                // Check User IsDisabled

                //**************************************

                if ((bool)objSecUserFactory.ObjSECUser.AccDisabled)
                {
                    throw new Exception(MsgID.Login.UserIsDisabled);                   
                }

                AppInfor.securityKey = Guid.NewGuid();
                if (!objSecUserFactory.SaveUserLogin((int)objSecUserFactory.ObjSECUser.UserKey))
                    return false;

                //Set Values in ApplicationInforFactory

                //**************************************               
                AppInfor.currentUserKey = (int)objSecUserFactory.ObjSECUser.UserKey;
                AppInfor.currentUserID = objSecUserFactory.ObjSECUser.UserID;
                AppInfor.currentUserName = objSecUserFactory.ObjSECUser.UserName;
                AppInfor.currentDBID = DatabaseID;
                AppInfor.conAccessLevel = (int)objSecUserFactory.ObjSECUser._conAccessLevel;
                AppInfor.conAccessGroup = (int)objSecUserFactory.ObjSECUser._conAccessGrp;
                AppInfor.itemAccessLevel = (int)objSecUserFactory.ObjSECUser._itmAccessLevel;
                AppInfor.itemAccessGroup = (int)objSecUserFactory.ObjSECUser._itmAccessGrp;
                AppInfor.jobAccessLevel = (int)objSecUserFactory.ObjSECUser._jobAccessLevel;
                AppInfor.jobAccessGroup = (int)objSecUserFactory.ObjSECUser._jobAccessGrp;

                AppInfor.branchKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._branchKey) ? 0 : objSecUserFactory.ObjSECUser._branchKey.Value;
                AppInfor.deptKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._deptKey) ? 0 : objSecUserFactory.ObjSECUser._deptKey.Value;
                AppInfor.tranGrpKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._tranGrpKey) ? 0 : objSecUserFactory.ObjSECUser._tranGrpKey.Value;
                AppInfor.emKey = GFunc.IsNEZ(objSecUserFactory.ObjSECUser._emKey) ? 0 : objSecUserFactory.ObjSECUser._emKey.Value;

                SECPermUtility.LoadAllUserPermission(out msgID);

                LoadGlobalOptions();

                this._lastLoginIdentifier = objSecUserFactory.ObjSECUser._loginIdentifierLast;
                this._lastLoginTimeStamp = objSecUserFactory.ObjSECUser._loginTimeStampLast;
          
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
        }

        private void LoadGlobalOptions()
        {
            try
            {

                string msgID = string.Empty;
                SYSOptions objPopupOptions = new SYSOptions();
                if (objPopupOptions.Fetch(new SYSOptions.Criteria(string.Empty, AppInfor.currentUserKey, 4)))
                {
                    foreach (SYSOption objPopupOption in objPopupOptions)
                    {
                        int opValue = 0;
                        Int32.TryParse(objPopupOption.OpValue, out opValue);

                        if (objPopupOption.OpID.Equals("SearchCustomerNumChar"))
                        {
                            SysOptionUtility.SearchCustomerNumChar = opValue;
                        }
                        //added by thettm on 18 jul 2018(start)
                        else if (objPopupOption.OpID.Equals("DatabaseBranchCode"))
                        {
                            if (GFunc.IsNE(objPopupOption.OpValue))
                            {
                                SysOptionUtility.DatabaseBranchCode = string.Empty;
                            }
                            else
                            {                       
                                SysOptionUtility.DatabaseBranchCode = objPopupOption.OpValue.ToString();
                            }
                        }
                        else if (objPopupOption.OpID.Equals("DosCreation"))
                        {
                            if (GFunc.IsNE(objPopupOption.OpValue))
                            {
                                SysOptionUtility.DOManualCreation = false;
                            }
                            else
                            {
                                SysOptionUtility.DOManualCreation = Convert.ToBoolean(GFunc.NEInt(objPopupOption.OpValue, 0));
                            }
                        }
                        else if (objPopupOption.OpID.Equals("PDsCreation"))
                        {
                            if (GFunc.IsNE(objPopupOption.OpValue))
                            {
                                SysOptionUtility.PDManualCreation = false;
                            }
                            else
                            {
                                SysOptionUtility.PDManualCreation = Convert.ToBoolean(GFunc.NEInt(objPopupOption.OpValue, 0));
                            }
                        }
                        else if (objPopupOption.OpID.Equals("ProcessingItem"))
                        {
                            if (GFunc.IsNE(objPopupOption.OpValue))
                            {
                                SysOptionUtility.ProcessingItem = 0;
                            }
                            else
                            {
                                SysOptionUtility.ProcessingItem = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                            }
                        }
                        else if (objPopupOption.OpID.Equals("ProcessingFee"))
                        {
                            if (GFunc.IsNE(objPopupOption.OpValue))
                            {
                                SysOptionUtility.ProcessingFee = 0;
                            }
                            else
                            {
                                SysOptionUtility.ProcessingFee = Convert.ToDecimal(GFunc.NEDec(objPopupOption.OpValue, 0));
                            }
                        }
                        //added by thettm on 18 jul 2018(end)
                        else if (objPopupOption.OpID.Equals("SearchVendorNumChar"))
                        {
                            SysOptionUtility.SearchVendorNumChar = opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchAccountNumChar"))
                        {
                            SysOptionUtility.SearchAccountNumChar = opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchItemNumChar"))
                        {
                            SysOptionUtility.SearchItemNumChar = opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchJobNumChar"))
                        {
                            SysOptionUtility.SearchJobNumChar = opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchCustomerMatch"))
                        {
                            SysOptionUtility.SearchCustomerMatch = (GEnum.SearchMatchOption)opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchVendorMatch"))
                        {
                            SysOptionUtility.SearchVendorMatch = (GEnum.SearchMatchOption)opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchAccountMatch"))
                        {
                            SysOptionUtility.SearchAccountMatch = (GEnum.SearchMatchOption)opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchJobMatch"))
                        {
                            SysOptionUtility.SearchJobMatch = (GEnum.SearchMatchOption)opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchItemMatch"))
                        {
                            SysOptionUtility.SearchItemMatch = (GEnum.SearchMatchOption)opValue;
                        }
                        else if (objPopupOption.OpID.Equals("SearchItemMatchID"))
                        {
                            SysOptionUtility.SearchItemMatchID = (GEnum.SearchMatchOption)opValue;
                        }
                        else if (objPopupOption.OpID.Equals("UseDept"))
                        {
                            SysOptionUtility.UseDept = Convert.ToBoolean(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("UseProject"))
                        {
                            SysOptionUtility.UseProject = Convert.ToBoolean(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("UseBranch"))
                        {
                            SysOptionUtility.UseBranch = Convert.ToBoolean(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("UseWMS"))
                        {
                            SysOptionUtility.UseWMS = Convert.ToBoolean(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("CountryCurrency"))
                        {
                            SysOptionUtility.CountryCurrency = Convert.ToInt32(opValue);
                        }
                        else if (objPopupOption.OpID.Equals("TaxCalculationOnTotal"))
                        {
                            SysOptionUtility.TaxCalculationOnTotal = GFunc.NEBool(opValue, true);
                        }
                        else if (objPopupOption.OpID.Equals("EnterKeyBehaviour"))
                        {
                            TAUtil.ControlGVar.CtrlEnterNewLine = Convert.ToBoolean(opValue);
                        }
                        else if (objPopupOption.OpID.Equals("BaseCurrency"))
                        {
                            SysOptionUtility.BaseCurrency = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("FiscalStartPeriod"))
                        {
                            SysOptionUtility.FiscalStartPeriod = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("InventoryValuationMethod"))
                        {
                            SysOptionUtility.InventoryValuationMethod = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("LanguageUsed"))
                        {
                            SysOptionUtility.LanguageUsed = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("LastARAPRevaluationPeriod"))
                        {
                            SysOptionUtility.LastARAPRevaluationPeriod = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("LastYearEndedDate"))
                        {
                            if (GFunc.IsNE(objPopupOption.OpValue))
                            {
                                SysOptionUtility.LastYearEndedDate = null;
                            }
                            else
                            {
                                DateTime retValue = new DateTime();
                                DateTime.TryParse(objPopupOption.OpValue, out retValue);
                                SysOptionUtility.LastYearEndedDate = retValue;
                            }
                        }
                        else if (objPopupOption.OpID.Equals("LandedCostOption"))
                        {
                            SysOptionUtility.LandedCostOption = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("StockCountStatus"))
                        {
                            SysOptionUtility.StockCountStatus = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("StockCountItemTotal"))
                        {
                            SysOptionUtility.StockCountItemTotal = Convert.ToDecimal(GFunc.NEDec(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("StockCountItemCounted"))
                        {
                            SysOptionUtility.StockCountItemCounted = Convert.ToDecimal(GFunc.NEDec(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("StockCountItemRemaining"))
                        {
                            SysOptionUtility.StockCountItemRemaining = Convert.ToDecimal(GFunc.NEDec(objPopupOption.OpValue, 0));
                        }
                        else if (objPopupOption.OpID.Equals("ShipMarkInitialNumber"))
                        {
                            SysOptionUtility.ShipMarkInitialNumber = Convert.ToInt32(GFunc.NEInt(objPopupOption.OpValue, 0));
                        }
                        else if (GFunc.CompareString(objPopupOption.OpID,"HasDMASLink"))
                        {
                            SysOptionUtility.HasDMASLink = GFunc.NEBool(objPopupOption.OpValue, 0);
                        }
                       
                        else if (objPopupOption.OpID.Equals("UOBBulkFilePath")) /* added by YST on 2022/10/01 */
                        {
                            SysOptionUtility.UOBBulkFilePath = objPopupOption.OpValue.ToString();
                        }
                        else if (objPopupOption.OpID.Equals("UOBBulkFilePath_OMSMaster")) /* added by YST on 2022/10/01 */
                        {
                            SysOptionUtility.UOBBulkFilePath_OMSMaster = objPopupOption.OpValue.ToString();
                        }
                        else if (objPopupOption.OpID.Equals("ExportFilePath")) /* added by YST on 2023/05/16 */
                        {
                            SysOptionUtility.ExportFilePath = objPopupOption.OpValue.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
        public static bool Logoff()
        {
            try
            {
                string ConnStr = AppInfor.currentDBConnectionStr;
                if (ConnStr == string.Empty)
                {
                    LoginFactory obj = new LoginFactory();
                    ConnStr= obj.GetCurrentConnectionStr();                    
                }
                SqlConnection cn = new SqlConnection(ConnStr);

                string msgID = MsgID.Login.LogoffFail;
                bool ProcessOK = true;
                SECUser objSecUser = null;

                //Get instance
                objSecUser = SECUser.New();

                //Set Current user info
                objSecUser._userKey = AppInfor.currentUserKey;
                objSecUser._securityKey = AppInfor.securityKey;
                
                //Update to database
                cn.Open();
                if (!objSecUser.CustomUpdate(cn, 4))
                    throw new TAException(msgID);


                AppInfor.currentDBConnectionStr = string.Empty;
                AppInfor.currentIdentifier = string.Empty;
                AppInfor.currentUserID = string.Empty;
                AppInfor.currentUserKey = 0;
                AppInfor.currentUserName = string.Empty;
                AppInfor.securityKey = Guid.Empty;

                cn.Close();               

                //**************************************
                return ProcessOK;
            }
            catch (Exception ex)
            {
                throw Errors(ex);
            }
        }

        private string GetCurrentConnectionStr()
        {
            try
            {
                string connStr = string.Empty;
                string msgID = string.Empty;
                REFCmpList objRefCmp = REFCmpList.Get(_databaseID);
                if (objRefCmp != null)
                {
                    if (objRefCmp.AuthenticateMode.Equals("Window"))
                    {
                        connStr += "Data Source=" + objRefCmp.ConnectionDSN.Trim();
                        connStr += ";Initial Catalog=" + objRefCmp.DataBaseNm.Trim();
                        connStr += ";Integrated Security=True";
                    }
                    else if (objRefCmp.AuthenticateMode.Equals("SQLServer"))
                    {
                        connStr += "Data Source=" + objRefCmp.ConnectionDSN.Trim();
                        connStr += ";Initial Catalog=" + objRefCmp.DataBaseNm.Trim();
                        connStr += ";user id=" + objRefCmp.ConnectionUserID;
                        connStr += ";password=" + objRefCmp.ConnectionPassword;
                    }                   
                }
               
                return connStr;
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        // added by KKAung on 05 Aug 2022 (start)
        public DataSet GetPasswordExpiry()
        {
            string msgID = MsgID.Common.GetFail;
            try
            {
                DataSet dt = null;

                dt = SECUser.GetPasswordHistory(new SECUser.Criteria(_userID, 2));

                //AppInfor.currentUserKey;
                
                if (!GFunc.IsNE(dt))
                    if (dt.Tables.Count > 0)
                    {
                        msgID = string.Empty;
                    }

                return dt;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }

        public DataSet GetPasswordHistory() 
        {
            string msgID = MsgID.Common.GetFail;
            try
            {
                DataSet dt = null;

                dt = SECUser.GetPasswordHistory(new SECUser.Criteria(_userID,1));
                //AppInfor.currentUserKey;

                if (dt != null)
                    if (dt.Tables.Count > 0)
                    {
                        msgID = string.Empty;
                        return dt;
                    }

                return dt;
            }
            catch (TAException tex)
            {
                throw Error(tex);
            }
            catch (Exception ex)
            {
                throw Error(ex);
            }
        }
       
        // (end)

        #endregion

        //Error Exceptions
        private Exception Error(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
        private static Exception Errors(Exception ex)
        {
            try
            {
                ex = SysAuditLogUtility.ModifyException(ex, false, new object[] { });
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return ex;
        }
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
        }


        // Forgot Password
        public string ForgotPassword(string userId)
        {
            int userKey = 0;
            string result = "";
            try
            {
                _ConnectionString = GetCurrentConnectionStr();

                if(_ConnectionString.Equals(""))
                {                   
                    return "Invalid Company Name!";
                }
                using (SqlConnection cn = new SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cm = cn.CreateCommand())
                    {
                        cn.Open();
                        cm.CommandType = System.Data.CommandType.Text;
                        cm.CommandTimeout = 100;
                        cm.CommandText = "Select UserKey from Sec_User(nolock) where userID='" + userId + "'";
                        string user = cm.ExecuteScalar() != null ? cm.ExecuteScalar().ToString() : "";

                        if(user.Equals(""))
                        {
                            //MessageBox.Show("User with ID \'"+userId+"\' not found! Please check userID again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return "Invalid User ID!";
                        }
                        Int32.TryParse(user, out userKey);

                        if (userKey > 0)
                        {
                            SECUser objSecUser = SECUser.New();

                            // generate random password                        
                            string resetPwd = GFunc.GenerateRandomCode(6);

                            objSecUser._userID = userId;
                            objSecUser._userKey = userKey;
                            objSecUser._password = resetPwd;

                            // update with random password
                            if (objSecUser.UpdatePassword(_ConnectionString))
                                result = "";
                            else
                                result = "Error:UpdatePassword";
                        } 
                        else
                        {
                            result = "Invalid User ID!";
                        }
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                throw Errors(ex);
            }

        }

    }
}
