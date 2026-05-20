using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Resources;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using BOLib;
using Infragistics.Win.UltraWinEditors;
using TAUtil;
using System.IO;
namespace WinUI
{
    public partial class frmLogin : Form
    {
        #region +++ Local Variable +++
        string msgID = string.Empty;
        LoginFactory objLoginFactory;
        bool isMouseEnter = false;

        #endregion

        #region +++ Constructor & Destructor +++
        public frmLogin()
        {
            InitializeComponent();
        }
        #endregion

        #region +++ Windows Form Control Event +++

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                objLoginFactory = new BOLib.LoginFactory();
                lblVersion.Text = AppInfor.ProgramVersion;


                CompanyNm.DataSource = objLoginFactory.GetCompanyList();
                CompanyNm.DisplayMember = "CompanyNm";
                CompanyNm.ValueMember = "DataBaseID";
                CompanyNm.DisplayLayout.Bands[0].Columns["CompanyNm"].Width = 200;
                CompanyNm.DisplayLayout.Bands[0].HeaderVisible = false;
                for (int i = 0; i < CompanyNm.DisplayLayout.Bands[0].Columns.Count; i++)
                    if (GFunc.CompareString(CompanyNm.DisplayLayout.Bands[0].Columns[i].Key , "CompanyNm")==false)
                        CompanyNm.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                
                if (objLoginFactory.DatabaseID != null)
                    CompanyNm.SetValueTrigger(objLoginFactory.DatabaseID,false);

                if (!CompanyNm.IsItemInList(CompanyNm.Text) && ((DataTable)CompanyNm.DataSource).Rows.Count == 1)
                {
                    CompanyNm.SetValueTrigger(CompanyNm.Rows[0].Cells["DataBaseID"].Value.ToString(), false);
                }
                if (objLoginFactory.UserID != null && objLoginFactory.RememberMe)
                {
                    uchkRememberMe.Checked = true;
                    UserID.SetValueTrigger(objLoginFactory.UserID,false);
                }
            }
            catch (TAException tex)
            {
                Error(tex, true); //Custom Msg
            }
            catch (Exception ex)
            {
                Error(ex, true); //System Msg
            }
        }    
        private void frmLogin_Shown(object sender, EventArgs e)
        {
            Password.Focus();           
            Password.SelectAll();
        }
        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            { 
                if(this.ActiveControl.Parent.GetType() == typeof(TAUtil.TATextBoxEditor))
                {
                    if ((this.ActiveControl.Parent).Name == Password.Name)
                    {
                        if (e.KeyCode == Keys.Enter)
                            if (UserID.Text.Length > 0 && Password.Text.Length > 0)
                                btnOK_Click(null, null);
                    }
                }
                
                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Error(ex, true);

            }

        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            string msgValue = string.Empty;
            string msgID = "PasswordInValid";


            this.Cursor = Cursors.WaitCursor;
            //CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc=null;

            try
            {
                objLoginFactory.UserID = (UserID.Text.Equals("Enter user ID")) ? string.Empty : UserID.Text;
                objLoginFactory.Password = (Password.Text.Equals("Enter password")) ? string.Empty : Password.Text;
                objLoginFactory.DatabaseID = Convert.ToString(CompanyNm.Value);
                objLoginFactory.RememberMe = uchkRememberMe.Checked;

                //if (AppInfor.ProgramVersion == "1.2.03" && !File.Exists(@"c:\techace\TempUpdate.txt"))
                //{
                //    try
                //    {
                //        File.Copy(@"\\sgbhvm01\BOSSUPDATER\TECHACEUpdater.exe", @"c:\techace\TECHACEUpdater.exe", true);
                //        File.Copy(@"\\sgbhvm01\BOSSUPDATER\TECHACEUpdater.exe.config", @"c:\techace\TECHACEUpdater.exe.config", true);
                //        File.Copy(@"\\sgbhvm01\BOSSUPDATER\TempUpdate.txt", @"c:\techace\TempUpdate.txt");
                //    }
                //    catch (Exception ex)
                //    {
                //        MsgBox.Show("You do not have Administrative right to copy the new updater. Please contact IT Support.");
                //        Application.Exit();
                //    }
                //}
                
                if (objLoginFactory.Login())
                {
                    // Change Password (start)    // added by KKAung on 03 Aug 2022
                    #region "Change Password for expiry"

                    if (GFunc.NEInt(SysOptionUtility.GetSysOpInt("PasswordExpiredDays"), 0) != -1)       // password never expired
                    {
                        SECUser objSECUser = SECUser.Get(objLoginFactory.UserID);
                        if (objSECUser.Custom1 != "X")              // selected users' password never expired
                        {
                            int pwdExpiryDays = SysOptionUtility.GetSysOpInt("PasswordExpiredDays");
                            DataSet dsPwdExpiry = objLoginFactory.GetPasswordExpiry();

                            //Reset password
                            if (dsPwdExpiry.Tables.Count == 1)
                            {
                                if (GFunc.NEInt(dsPwdExpiry.Tables[0].Rows.Count, 0) == 1 && GFunc.NEStr(dsPwdExpiry.Tables[0].Rows[0]["Custom1"].ToString(), "") == "RESET")
                                {
                                    if (MsgBox.Show("Your password is reset. Please enter your new password.", GEnum.MsgBoxButton.OK) != GEnum.MsgBoxButton.OK)
                                    {
                                        objSECUser.CustomUpdate((int)GEnum.SECUserCustomUpdateOption.SaveUserLogoff);
                                        return;
                                    }
                                    frmSECChangePassword fSECChangePassword = new frmSECChangePassword(true);
                                    fSECChangePassword.WindowState = FormWindowState.Maximized;
                                    DialogResult result = fSECChangePassword.ShowDialog(this);
                                    return;
                                }
                            }

                            // reminder password expiry
                            if (dsPwdExpiry.Tables.Count == 2)
                            {
                                if (GFunc.NEInt(dsPwdExpiry.Tables[0].Rows.Count, 0) == 1 && GFunc.NEInt(dsPwdExpiry.Tables[1].Rows.Count, 0) == 0)
                                {

                                    DateTime passwordDate = GFunc.NEDateTime(dsPwdExpiry.Tables[0].Rows[0]["PasswordDate"].ToString(), "");
                                    int noOfDays = pwdExpiryDays - (DateTime.Today.Subtract(passwordDate.Date).Duration().Days);
                                    int diff = pwdExpiryDays - (DateTime.Today.Date - passwordDate.Date).Days;
                                    GEnum.MsgBoxButton btnSelect;
                                    btnSelect = MsgBox.Show("Your password will expire in " + noOfDays + " day(s).\nDo you want to change it now?", GEnum.MsgBoxIcon.Question, GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.Yes, GEnum.MsgBoxButton.No);

                                    if (btnSelect == GEnum.MsgBoxButton.Yes)
                                    {
                                        frmSECChangePassword fSECChangePassword = new frmSECChangePassword(true);
                                        fSECChangePassword.WindowState = FormWindowState.Maximized;
                                        DialogResult result = fSECChangePassword.ShowDialog();
                                        return;
                                    }

                                    this.DialogResult = DialogResult.OK;

                                    frmMain.gfrmMain.ustsbarMain.Panels["Login"].Text = AppInfor.CurrentUserName.ToString().ToUpper();

                                    if (GFunc.IsNE(objLoginFactory.LastLoginTimeStamp) || GFunc.IsNE(objLoginFactory.LastLoginIdentifier))
                                        msgValue = SysMessageUtility.Get(MsgID.Login.UserFirstLogin);
                                    else
                                    {
                                        //build message Id with parameters
                                        msgID = MsgID.Login.UserLastLoginInfo + "%" + ((DateTime)objLoginFactory.LastLoginTimeStamp).ToString("d MMMM yyyy - h:mm tt")
                                                                            + "%" + objLoginFactory.LastLoginIdentifier.ToString();
                                        msgValue = SysMessageUtility.Get(msgID);
                                    }

                                    frmMain.gfrmMain.ustsbarMain.Panels["Login"].ToolTipText = msgValue;
                                    frmMain.gfrmMain.ustsbarMain.Panels["Status"].Text = msgValue;
                                    DocHDRUtil.CreateDataTransferLogicData();
                                    this.Close();
                                }
                            }

                            // Password expiry
                            if (dsPwdExpiry.Tables.Count == 2)
                            {
                                if (GFunc.NEInt(dsPwdExpiry.Tables[0].Rows.Count, 0) == 1 && GFunc.NEInt(dsPwdExpiry.Tables[1].Rows.Count, 0) == 1)
                                {
                                    if (MsgBox.Show("Your password is expired. Please change the password.", GEnum.MsgBoxButton.OK) != GEnum.MsgBoxButton.OK)
                                    {
                                        objSECUser.CustomUpdate((int)GEnum.SECUserCustomUpdateOption.SaveUserLogoff);
                                        return;
                                    }
                                    frmSECChangePassword fSECChangePassword = new frmSECChangePassword(true);
                                    fSECChangePassword.WindowState = FormWindowState.Maximized;
                                    DialogResult result = fSECChangePassword.ShowDialog();
                                    return;
                                }
                            }
                        }
                    }

                    #endregion
                    // (end)

                    this.DialogResult = DialogResult.OK;

                    frmMain.gfrmMain.ustsbarMain.Panels["Login"].Text = AppInfor.CurrentUserName.ToString().ToUpper();

                    if (GFunc.IsNE(objLoginFactory.LastLoginTimeStamp) || GFunc.IsNE(objLoginFactory.LastLoginIdentifier))
                        msgValue = SysMessageUtility.Get(MsgID.Login.UserFirstLogin);
                    else
                    {
                        //build message Id with parameters
                        msgID = MsgID.Login.UserLastLoginInfo + "%" + ((DateTime)objLoginFactory.LastLoginTimeStamp).ToString("d MMMM yyyy - h:mm tt")
                                                            + "%" + objLoginFactory.LastLoginIdentifier.ToString();
                        msgValue = SysMessageUtility.Get(msgID);
                    }

                    frmMain.gfrmMain.ustsbarMain.Panels["Login"].ToolTipText = msgValue;
                    frmMain.gfrmMain.ustsbarMain.Panels["Status"].Text = msgValue;
                    DocHDRUtil.CreateDataTransferLogicData();

                    //////Try to initialize RPT engine, to be faster when they view Rpt for the first time
                    //rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    //rptDoc.Load(Application.StartupPath + @"\Reports\" + "TestRpt.rpt");
                    //rptDoc.Close();                 

                    //The final solution is to use email hyper link. 
                    //The temporary soulution is to use the dummy Empty exe (which will exist under Application.StartupPath + @"\Reports) before changing RPT files
                    //Temporary Code: to delete old HyperLinkClient.exe file
                    //if (!System.IO.File.Exists(Path.GetDirectoryName(Application.StartupPath) + @"\HyperlinkClient.exe"))
                    //{
                    //    System.IO.File.Delete(Path.GetDirectoryName(Application.StartupPath) + @"\HyperlinkClient.exe"); 
                    //}
                    //if (!System.IO.File.Exists(Application.StartupPath + @"\HyperlinkClient.exe"))
                    //{
                    //    System.IO.File.Delete(Application.StartupPath + @"\HyperlinkClient.exe");
                    //}                   

                    this.Close();
               
                }                
            }
            catch (TAException tex)
            {
                DialogResult = DialogResult.None;

                if (tex.MsgID.Equals("DatabaseConnectionStrInValid"))
                    MsgBox.Show("Unable to obtain database connection string for company \'" + CompanyNm.Text + "\'.");

                else if (tex.MsgID.Equals("DatabaseRegCodeInvalid"))
                    MsgBox.Show("Database Registration Code is Invalid");

                else
                {
                    string sErrMsg = string.Empty;
                    MsgBox.Show(BOLib.SysMessageUtility.Get(msgID)+ "\n");
                    MsgBox.Show(tex.Message);          
                }
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show(ex.Message + " : " + ex.StackTrace);

                if (ex.Message.Equals("DatabaseConnectionStrInValid"))
                    MsgBox.Show("Unable to obtain database connection string for company \'" + CompanyNm.Text + "\'.");

                else if (ex.Message.Equals("UserIDOrPasswordInValid"))
                {
                    MsgBox.Show("Invalid User ID");
                }

                else
                {
                    string sErrMsg = string.Empty;
                    sErrMsg = BOLib.SysMessageUtility.Get(msgID);
                    //MsgBox.Show(sErrMsg, "Login Fail");
                    MsgBox.Show(ex.Message +" : "+ex.StackTrace);                   
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                //if (rptDoc != null)
                //{
                //    rptDoc.Dispose();
                //}
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region Set Error Methods
        private Exception Error(Exception ex, bool ShowMessage)
        {
            Exception l_tmpex = ex;
            try
            {
                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, false, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyException(ex, false, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
                }

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return l_tmpex;
        }
        private TAException Error(TAException ex, bool ShowMessage)
        {
            TAException l_tmpex = ex;
            try
            {
                if (this.ActiveControl.GetType() != typeof(TAUtil.TAGridEditor))
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { this.Name, this.ActiveControl });
                }
                else
                {
                    l_tmpex = SysAuditLogUtility.ModifyTAException(ex, false, new object[] { this.Name, this.ActiveControl.Name, GFunc.GridColumnKey_Get(this.ActiveControl) });
                }

                if (ShowMessage)
                {
                    SysAuditLogUtility.AddErrorLog(l_tmpex);
                }
            }
            catch (Exception nex)
            {
                MsgBox.Show(nex.Message);
            }
            return l_tmpex;
        }
        #endregion
        private void Combo_NotInList(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                GlobalUI.ItemNotInList(sender as TAUtil.TAComboBox, e, false, null);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//CodeCompleted

        /* Forgot PW - added by YLP, modified by YST */
        private void lbl_forgot_pw_Click(object sender, EventArgs e)
        {
            string userId = (UserID.Text.Equals("Enter user ID")) ? string.Empty : UserID.Text;
            if (userId == string.Empty)
            {
                MessageBox.Show("User ID is required !", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (CompanyNm.Value is null)
            if (GFunc.IsNE(Convert.ToString(CompanyNm.Value)))
            {
                MessageBox.Show("Company Name is required !", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            try
            {
                objLoginFactory.UserID = (UserID.Text.Equals("Enter user ID")) ? string.Empty : UserID.Text;

                objLoginFactory.DatabaseID = Convert.ToString(CompanyNm.Value);
                string returnError = objLoginFactory.ForgotPassword(userId);

                if (string.IsNullOrEmpty(returnError))
                {
                    MessageBox.Show("A one-time Password has been sent to your email." + "\nPlease check your inbox."
                                + "\nIf you haven't received the email, kindly inform the E-services team."
                                ,"", MessageBoxButtons.OK, MessageBoxIcon.Information);                  

                } else
                {
                    MessageBox.Show(returnError + "\nFailed to reset your password.","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        /* Toggle PW - added by YST at 2023/11/11 */
        private void pw_eye_MouseUp(object sender, MouseEventArgs e)
        {
            Password.PasswordChar = '*';
            pw_eye.Image = Properties.Resources.eye_off;
        }

        private void pw_eye_MouseDown(object sender, MouseEventArgs e)
        {
            Password.PasswordChar = '\0';
            pw_eye.Image = Properties.Resources.eye_open;
        }
    }
}