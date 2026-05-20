using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using BOLib;
using TAUtil;
using Infragistics.Win.Misc;

namespace WinUI
{
    public partial class frmSECChangePassword : Form
    {
        #region Local Variables
        private BOLib.SECChangePasswordFactory objSECChangePasswordFactory = null;
        string ContextMenuSetting = string.Empty;
        private string msgID = string.Empty;
        private bool formClose = false;
        private bool passwordExpired = false;

        #endregion

        #region Initialize
        public frmSECChangePassword()
        {
            InitializeComponent();
        }

        //added by KKAung on 18 Sep 2022
        public frmSECChangePassword(bool passwordexpired)
        {
            InitializeComponent();
            this.passwordExpired = passwordexpired;
           
        }

        #endregion

        //Form Events
        private void frmSECChangePassword_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Call Initialization                
                this.objSECChangePasswordFactory = new BOLib.SECChangePasswordFactory(BOLib.GEnum.InstanceMode.Normal);
                if (objSECChangePasswordFactory.GUID <= 0) formClose = true;
                this.objSECChangePasswordFactory.New();               

                this.objSECChangePasswordFactory.errorEvent += new GVar.ErrorEvent(this.OnHeaderError);
                if (!GFunc.IsNE(this.objSECChangePasswordFactory.ObjSECUser))
                    this.Refresh_UserInfo();
                this.Password.Focus();
                GlobalUI.FormGrids_Set(this, (int)GEnum.SystemCode.Security_ChangePassword, out ContextMenuSetting);
                ContextMenuSetting += GlobalUI.ContextMenuSetting_GetNew((int)GEnum.SystemCode.Security_ChangePassword);
            }
            catch (TAException tex)
            {
                // Check Process 
                if (tex.MsgID == MsgID.Common.NoMultiInstanceAllowed)
                {
                    this.formClose = true;
                    frmMain.gfrmMain.IsExistingForm(this);
                }
                else
                {
                    this.formClose = true;
                    Error(tex, true);
                }
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void frmSECChangePassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (e.CloseReason == System.Windows.Forms.CloseReason.MdiFormClosing)
                {
                    e.Cancel = true;
                    
                    return;
                }

                if (!this.IsSaveChanges())
                {
                    if (passwordExpired && this.objSECChangePasswordFactory.ObjSECUser != null) 
                        this.objSECChangePasswordFactory.ObjSECUser.CustomUpdate((int)GEnum.SECUserCustomUpdateOption.SaveUserLogoff); 
                    this.objSECChangePasswordFactory.Dispose();
                }
                else
                {
                    //When the form is closed by main form, to prohibit closing
                   
                    frmMain.gfrmMain.Tag = GVar.CancelMainFormClosing;
                    e.Cancel = true;
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//Completed
        private void frmSECChangePassword_Shown(object sender, EventArgs e)
        {
            // Check Form Close State is True ...
            if (formClose)
            {
                this.Close();
            }
            else
            {                   
                this.splitContainer1.Panel2.Focus();
                this.UserID.Focus();                
            }
        }//Completed
        private void frmSECChangePassword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Set Focus Next Control
                GlobalUI.SelectNextControl(this, e);
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed

        //Menu Strip Event
        private void tsbClose_Click(object sender, EventArgs e)
        {
            //if (!IsSaveChanges())
            {
                //if (passwordExpired)
                //    objSECChangePasswordFactory.ObjSECUser.CustomUpdate((int)GEnum.SECUserCustomUpdateOption.SaveUserLogoff);
                this.formClose = true;
                this.Close();
            }
           
        }//Completed
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if(this.UserEmail.Text == String.Empty || this.UserEmail.Text == "eservices@benghui.com")
                //{
                //    DialogResult result = MessageBox.Show("Please update your email. \nDo you want to continue with default email?", "Email", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                //    if(result == DialogResult.Yes)
                //    {
                //        this.UserEmail.Text = "eservices@benghui.com";
                //        return;
                //    }
                //}
                if (this.Save_Process())
                {                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();  
                }
            }
            catch (TAException tex)
            {
                Error(tex, true);
            }
            catch (Exception ex)
            {
                Error(ex, true);
            }
        }//Completed

        //ValueChange Events
        private void UserEmail_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.objSECChangePasswordFactory.OnChangedUserEmail();
        }
        private void Password_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.objSECChangePasswordFactory.OnChangedPassword();
        }//Completed
        private void NewPassword_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.objSECChangePasswordFactory.OnChangedNewPassword();
        }//Completed
        private void ConfirmPassword_CustomUpdate(object sender, CancelEventArgs e)
        {
            this.objSECChangePasswordFactory.OnChangedConfirmPassword();
        }//Completed

        //Event Methods
        private void Refresh_UserInfo()
        {
            try
            {
                // Object binding for User Information  

                this.bdsSECUser.DataSource = this.objSECChangePasswordFactory.ObjSECUser;
                this.bdsSECUser.ResetBindings(false);
                this.Password.Focus();

            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
        }//Completed
        private bool Save_Process()
        {
            this.Cursor = Cursors.WaitCursor;
            bool isSave = false;

            try
            {
                this.errorProvider1.Clear();
                this.Validate();

                // Check Factory Object is not null
                if (!BOLib.GFunc.IsNE(this.objSECChangePasswordFactory))
                {
                    this.objSECChangePasswordFactory.SetUserEmail(this.UserEmail.Text);
                    this.objSECChangePasswordFactory.SetPassword(this.Password.Text);
                    this.objSECChangePasswordFactory.SetNewPassword(this.NewPassword.Text);
                    this.objSECChangePasswordFactory.SetConfirmPassword(this.ConfirmPassword.Text);

                    isSave = this.objSECChangePasswordFactory.Save();
                    ShowPasswordError();

                }
                if (isSave)
                {
                    this.Refresh_UserInfo();
                    this.errorProvider1.Clear();
                    
                    // added by KKAung on 18 Sep 2022 (start)

                    if (passwordExpired)
                    {
                        this.objSECChangePasswordFactory.ObjSECUser.CustomUpdate((int)GEnum.SECUserCustomUpdateOption.SaveUserLogoff);
                        MsgBox.Show("Password changed successfully. Please re-login with new password.",
                                            GEnum.MsgBoxIcon.Information,
                                            GEnum.MsgBoxButton.OK);
                    }
                    else
                    {
                        MsgBox.Show("Password changed successfully.",GEnum.MsgBoxIcon.Information,GEnum.MsgBoxButton.OK);
                    }
                    this.formClose = true;                    
                    
                    // end
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return isSave;
        }                //Completed
        private void OnDirty(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName.ToLower())
            {
                case "password":
                    this.errorProvider1.SetError(this.Password, string.Empty);
                    break;
                case "newpassword":
                    this.errorProvider1.SetError(this.NewPassword, string.Empty);
                    break;
                case "confirmpassword":
                    this.errorProvider1.SetError(this.ConfirmPassword, string.Empty);
                    break;                
            }
        }//Completed
        private void OnHeaderError(string errorMsg, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName.ToLower())
            {
                case "useremail":
                    this.errorProvider1.SetError(this.UserEmail, errorMsg);
                    this.UserEmail.Focus();
                    break;
                case "password":
                    this.errorProvider1.SetError(this.Password, errorMsg);
                    this.Password.Focus();
                    break;
                case "newpassword":
                    this.errorProvider1.SetError(this.NewPassword, errorMsg);
                    this.NewPassword.Focus();
                    break;                
                case "confirmpassword":
                    this.errorProvider1.SetError(this.ConfirmPassword, errorMsg);
                    this.ConfirmPassword.Focus();
                    break;                
            }
        }        //Completed
        private bool IsSaveChanges()
        {
            this.Cursor = Cursors.WaitCursor;
            bool isSave = false;

            try
            {
                // Check Form Validation
                this.Validate();

                // Check Factory Object is Dirty ...
                if (this.objSECChangePasswordFactory.IsDirty)
                {
                    // Ask Confirmation To Save
                    GEnum.MsgBoxButton btnSelect;
                    btnSelect = MsgBox.Show(MsgID.Common.SaveChanges,
                                          GEnum.MsgBoxIcon.Question,
                                          GEnum.MsgBoxButton.Save_Changes,
                                          GEnum.MsgBoxButton.Discard_Changes,
                                          GEnum.MsgBoxButton.I_Dont_Know);

                    // Yes, I want to save
                    if (btnSelect == GEnum.MsgBoxButton.Save_Changes)
                        isSave = !this.Save_Process();
                    // No, I don't want to Save
                    else if (btnSelect == GEnum.MsgBoxButton.Discard_Changes)           // added by KKAung on 18 Sep 2022 
                    { 
                        if (passwordExpired) this.objSECChangePasswordFactory.ObjSECUser.CustomUpdate((int)GEnum.SECUserCustomUpdateOption.SaveUserLogoff); 
                    }
                    else // No, I don't know
                        isSave = true;
                }
            }
            catch (TAException tex)
            {
                throw Error(tex, false);
            }
            catch (Exception ex)
            {
                throw Error(ex, false);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return isSave;
        }//Completed

        //Set Error Methods
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
        }//Completed
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
        }//Completed        

        #region TogglePassword
        /* added by YLP at 2023/10/25, modified by YST at 2023/11/01 */
        private void pw_eye_Click(object sender, EventArgs e)
        {
            TogglePassword((PictureBox)sender, Password, true);           
        }
        private void new_pw_eye_Click(object sender, EventArgs e)
        {
            TogglePassword((PictureBox)sender, NewPassword, true);
        }
        private void confirm_pw_eye_Click(object sender, EventArgs e)
        {
            TogglePassword((PictureBox)sender, ConfirmPassword, true);       
        }                

        private void Password_Leave(object sender, EventArgs e)
        {
            TogglePassword(pw_eye,(TATextBoxEditor)sender, false);
        }
        private void NewPassword_Leave(object sender, EventArgs e)
        {
            TogglePassword(new_pw_eye, (TATextBoxEditor)sender, false);
        }
        private void ConfirmPassword_Leave(object sender, EventArgs e)
        {
            TogglePassword(confirm_pw_eye, (TATextBoxEditor)sender, false);
        }

        private void TogglePassword(PictureBox pic, TATextBoxEditor txt, bool isEyeClick)
        {
            if (isEyeClick)
            {
                if (txt.Name != Password.Name) Password.PasswordChar = '*';
                if (txt.Name != NewPassword.Name) NewPassword.PasswordChar = '*';
                if (txt.Name != ConfirmPassword.Name) ConfirmPassword.PasswordChar = '*';

                if (pic.Name != pw_eye.Name)  pw_eye.Image = Properties.Resources.eye_off;
                if (pic.Name != new_pw_eye.Name) new_pw_eye.Image = Properties.Resources.eye_off;
                if (pic.Name != confirm_pw_eye.Name) confirm_pw_eye.Image = Properties.Resources.eye_off;

                pic.Image = txt.PasswordChar == '*' ? Properties.Resources.eye_open : Properties.Resources.eye_off;
                txt.PasswordChar = txt.PasswordChar == '*' ? '\0' : '*';
                txt.Focus();
                txt.SelectionStart = txt.TextLength;
            }
            else
            {
                txt.PasswordChar = '*';
                pic.Image = Properties.Resources.eye_off;
            }
        }
        private void ShowPasswordError()
        {
            string NewPasswordError = errorProvider1.GetError(NewPassword);            
            if (!string.IsNullOrEmpty(NewPassword.Text) || !string.IsNullOrEmpty(NewPasswordError))
            {
                Format_lblErrPW(lblErrPWLengeth, NewPasswordError.ToLower().Contains("length"));
                Format_lblErrPW(lblErrPWDigit, NewPasswordError.ToLower().Contains("digit"));
                Format_lblErrPW(lblErrPWCase, NewPasswordError.ToLower().Contains("case"));
                Format_lblErrPW(lblErrPWSChar, NewPasswordError.ToLower().Contains("special"));
                Format_lblErrPW(lblErrPWHistory, NewPasswordError.ToLower().Contains("last"));
            }
        }
        private void Format_lblErrPW(UltraLabel lbl,bool hasError)
        {
            if (hasError)
            {
                lbl.Text = lbl.Text.Replace("■", "❎");
                lbl.Text = lbl.Text.Replace("✅", "❎");
                lbl.Appearance.ForeColor = Color.Red;
            }
            else
            {
                lbl.Text = lbl.Text.Replace("■", "✅");
                lbl.Text = lbl.Text.Replace("❎", "✅");
                lbl.Appearance.ForeColor = Color.Green;
            }            
        }

        #endregion        
    }
}