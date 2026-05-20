namespace WinUI
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.Password = new TAUtil.TATextBoxEditor();
            this.uchkRememberMe = new TAUtil.TACheckBoxEditor();
            this.CompanyNm = new TAUtil.TAComboBox();
            this.UserID = new TAUtil.TATextBoxEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbExit1 = new System.Windows.Forms.ToolStripButton();
            this.tsbLogin1 = new System.Windows.Forms.ToolStripButton();
            this.lblVersion = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.tsbLogin = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pw_eye = new System.Windows.Forms.PictureBox();
            this.lbl_forgot_pw = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Password)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uchkRememberMe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyNm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserID)).BeginInit();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pw_eye)).BeginInit();
            this.SuspendLayout();
            // 
            // Password
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.Image = global::WinUI.Properties.Resources.PrimaryKeyHS;
            this.Password.Appearance = appearance1;
            this.Password.Font = new System.Drawing.Font("Calibri", 11F);
            this.Password.Format = "";
            this.Password.IsDirty = false;
            this.Password.IsEmailTextBox = false;
            this.Password.Location = new System.Drawing.Point(15, 114);
            this.Password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Password.Name = "Password";
            this.Password.NullText = "Enter password";
            appearance6.ForeColor = System.Drawing.Color.Silver;
            this.Password.NullTextAppearance = appearance6;
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(230, 26);
            this.Password.TabIndex = 1;
            this.Password.Text = "Enter password";
            this.Password.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // uchkRememberMe
            // 
            this.uchkRememberMe.BackColor = System.Drawing.Color.Transparent;
            this.uchkRememberMe.BackColorInternal = System.Drawing.Color.Transparent;
            this.uchkRememberMe.cancelUpdate = false;
            this.uchkRememberMe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uchkRememberMe.Location = new System.Drawing.Point(131, 191);
            this.uchkRememberMe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uchkRememberMe.Name = "uchkRememberMe";
            this.uchkRememberMe.Size = new System.Drawing.Size(19, 25);
            this.uchkRememberMe.TabIndex = 4;
            this.uchkRememberMe.Text = "taCheckBoxEditor1";
            this.uchkRememberMe.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // CompanyNm
            // 
            appearance8.ForeColor = System.Drawing.Color.Black;
            appearance8.Image = global::WinUI.Properties.Resources.HomeHS;
            this.CompanyNm.Appearance = appearance8;
            this.CompanyNm.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.CompanyNm.ComboIsDirty = false;
            this.CompanyNm.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.CompanyNm.Font = new System.Drawing.Font("Calibri", 11F);
            this.CompanyNm.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.CompanyNm.LimitToList = true;
            this.CompanyNm.Location = new System.Drawing.Point(15, 149);
            this.CompanyNm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CompanyNm.Name = "CompanyNm";
            this.CompanyNm.NullText = "Select a company";
            this.CompanyNm.Size = new System.Drawing.Size(230, 26);
            this.CompanyNm.TabIndex = 2;
            this.CompanyNm.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.CompanyNm.UserInputText = "";
            this.CompanyNm.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // UserID
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.Image = global::WinUI.Properties.Resources.User;
            this.UserID.Appearance = appearance7;
            this.UserID.BackColor = System.Drawing.Color.White;
            this.UserID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.UserID.Font = new System.Drawing.Font("Calibri", 11F);
            this.UserID.Format = "";
            this.UserID.IsDirty = false;
            this.UserID.IsEmailTextBox = false;
            this.UserID.Location = new System.Drawing.Point(15, 77);
            this.UserID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UserID.Name = "UserID";
            this.UserID.NullText = "Enter user ID";
            appearance5.ForeColor = System.Drawing.Color.Silver;
            this.UserID.NullTextAppearance = appearance5;
            this.UserID.Size = new System.Drawing.Size(230, 26);
            this.UserID.TabIndex = 0;
            this.UserID.Text = "Enter user ID";
            this.UserID.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel2
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ultraLabel2.Appearance = appearance3;
            this.ultraLabel2.Font = new System.Drawing.Font("Calibri", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ultraLabel2.Location = new System.Drawing.Point(15, 196);
            this.ultraLabel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(110, 27);
            this.ultraLabel2.TabIndex = 3;
            this.ultraLabel2.Text = "Remember Me";
            this.ultraLabel2.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 60);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(17, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 17);
            this.label5.TabIndex = 48;
            this.label5.Text = "Welcome to System!";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(17, 259);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 62);
            this.label1.TabIndex = 51;
            this.label1.Text = "Choose associated company in order to connect the system database.";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(17, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 62);
            this.label4.TabIndex = 52;
            this.label4.Text = "Use a valid User ID and Password to gain access to the system.";
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tspBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbExit1,
            this.tsbLogin1,
            this.lblVersion,
            this.toolStripLabel1});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(490, 74);
            this.tspBar.TabIndex = 1;
            // 
            // tsbExit1
            // 
            this.tsbExit1.AutoSize = false;
            this.tsbExit1.BackColor = System.Drawing.Color.Transparent;
            this.tsbExit1.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Italic);
            this.tsbExit1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbExit1.Image = global::WinUI.Properties.Resources.door_back_32;
            this.tsbExit1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbExit1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit1.Name = "tsbExit1";
            this.tsbExit1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbExit1.RightToLeftAutoMirrorImage = true;
            this.tsbExit1.Size = new System.Drawing.Size(70, 55);
            this.tsbExit1.Text = "E&xit";
            this.tsbExit1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbExit1.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tsbLogin1
            // 
            this.tsbLogin1.AutoSize = false;
            this.tsbLogin1.BackColor = System.Drawing.Color.Transparent;
            this.tsbLogin1.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Italic);
            this.tsbLogin1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbLogin1.Image = global::WinUI.Properties.Resources.user_next_32;
            this.tsbLogin1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbLogin1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLogin1.Name = "tsbLogin1";
            this.tsbLogin1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbLogin1.RightToLeftAutoMirrorImage = true;
            this.tsbLogin1.Size = new System.Drawing.Size(70, 55);
            this.tsbLogin1.Text = "&Login";
            this.tsbLogin1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbLogin1.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.lblVersion.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Italic);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.ReadOnly = true;
            this.lblVersion.Size = new System.Drawing.Size(80, 74);
            this.lblVersion.Text = "1.0.1.2";
            this.lblVersion.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Italic);
            this.toolStripLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(60, 71);
            this.toolStripLabel1.Text = "Version :";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WinUI.Properties.Resources.Lkeys;
            this.pictureBox1.InitialImage = global::WinUI.Properties.Resources.Lkeys;
            this.pictureBox1.Location = new System.Drawing.Point(21, 89);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 58);
            this.pictureBox1.TabIndex = 50;
            this.pictureBox1.TabStop = false;
            // 
            // tsbExit
            // 
            this.tsbExit.AutoSize = false;
            this.tsbExit.BackColor = System.Drawing.Color.Transparent;
            this.tsbExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            this.tsbExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbExit.RightToLeftAutoMirrorImage = true;
            this.tsbExit.Size = new System.Drawing.Size(70, 55);
            this.tsbExit.Text = "E&xit";
            this.tsbExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbExit.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tsbLogin
            // 
            this.tsbLogin.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tsbLogin.AutoSize = false;
            this.tsbLogin.BackColor = System.Drawing.Color.Transparent;
            this.tsbLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbLogin.Image = ((System.Drawing.Image)(resources.GetObject("tsbLogin.Image")));
            this.tsbLogin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLogin.Name = "tsbLogin";
            this.tsbLogin.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbLogin.RightToLeftAutoMirrorImage = true;
            this.tsbLogin.Size = new System.Drawing.Size(70, 55);
            this.tsbLogin.Text = "&Login";
            this.tsbLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.pw_eye);
            this.panel1.Controls.Add(this.lbl_forgot_pw);
            this.panel1.Controls.Add(this.ultraLabel1);
            this.panel1.Controls.Add(this.UserID);
            this.panel1.Controls.Add(this.Password);
            this.panel1.Controls.Add(this.ultraLabel2);
            this.panel1.Controls.Add(this.CompanyNm);
            this.panel1.Controls.Add(this.uchkRememberMe);
            this.panel1.Location = new System.Drawing.Point(201, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 264);
            this.panel1.TabIndex = 0;
            // 
            // pw_eye
            // 
            this.pw_eye.BackColor = System.Drawing.Color.White;
            this.pw_eye.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pw_eye.Image = ((System.Drawing.Image)(resources.GetObject("pw_eye.Image")));
            this.pw_eye.Location = new System.Drawing.Point(223, 118);
            this.pw_eye.Name = "pw_eye";
            this.pw_eye.Size = new System.Drawing.Size(16, 18);
            this.pw_eye.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pw_eye.TabIndex = 73;
            this.pw_eye.TabStop = false;
            this.pw_eye.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pw_eye_MouseDown);
            this.pw_eye.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pw_eye_MouseUp);
            // 
            // lbl_forgot_pw
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.Blue;
            this.lbl_forgot_pw.Appearance = appearance4;
            this.lbl_forgot_pw.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_forgot_pw.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic);
            this.lbl_forgot_pw.Location = new System.Drawing.Point(15, 224);
            this.lbl_forgot_pw.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbl_forgot_pw.Name = "lbl_forgot_pw";
            this.lbl_forgot_pw.Size = new System.Drawing.Size(99, 18);
            this.lbl_forgot_pw.TabIndex = 7;
            this.lbl_forgot_pw.Text = "Forgot Password?";
            this.lbl_forgot_pw.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.lbl_forgot_pw.Click += new System.EventHandler(this.lbl_forgot_pw_Click);
            // 
            // ultraLabel1
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabel1.Font = new System.Drawing.Font("Calibri", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ultraLabel1.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(273, 30);
            this.ultraLabel1.TabIndex = 4;
            this.ultraLabel1.Text = "Log In";
            this.ultraLabel1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // frmLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(490, 370);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tspBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmLogin";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Password)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uchkRememberMe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyNm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserID)).EndInit();
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pw_eye)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbLogin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbExit1;
        private System.Windows.Forms.ToolStripButton tsbLogin1;
        private TAUtil.TATextBoxEditor UserID;
        private TAUtil.TAComboBox CompanyNm;
        private TAUtil.TACheckBoxEditor uchkRememberMe;
        private TAUtil.TATextBoxEditor Password;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.ToolStripTextBox lblVersion;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private Infragistics.Win.Misc.UltraLabel lbl_forgot_pw;
        private System.Windows.Forms.PictureBox pw_eye;
    }
}