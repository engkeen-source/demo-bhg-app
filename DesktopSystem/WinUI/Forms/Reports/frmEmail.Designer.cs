namespace WinUI
{
    partial class frmEmail
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
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSend = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel16 = new Infragistics.Win.Misc.UltraLabel();
            this.lblSubject = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtEmail = new TAUtil.TATextBoxEditor();
            this.Subject = new TAUtil.TATextBoxEditor();
            this.message = new TAUtil.TATextBoxEditor();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Subject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.message)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            appearance24.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.Appearance = appearance24;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.Location = new System.Drawing.Point(490, 211);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 30);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSend
            // 
            appearance23.Image = global::WinUI.Properties.Resources.send;
            this.btnSend.Appearance = appearance23;
            this.btnSend.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnSend.Location = new System.Drawing.Point(400, 211);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(84, 30);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // ultraLabel16
            // 
            appearance17.BackColor = System.Drawing.Color.Transparent;
            appearance17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabel16.Appearance = appearance17;
            this.ultraLabel16.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel16.Location = new System.Drawing.Point(17, 25);
            this.ultraLabel16.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel16.Name = "ultraLabel16";
            this.ultraLabel16.Size = new System.Drawing.Size(94, 22);
            this.ultraLabel16.TabIndex = 0;
            this.ultraLabel16.Text = "Email Address";
            this.ultraLabel16.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblSubject
            // 
            appearance18.BackColor = System.Drawing.Color.Transparent;
            appearance18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance18.TextVAlignAsString = "Middle";
            this.lblSubject.Appearance = appearance18;
            this.lblSubject.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblSubject.Location = new System.Drawing.Point(17, 53);
            this.lblSubject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(62, 22);
            this.lblSubject.TabIndex = 4;
            this.lblSubject.Text = "Subject";
            this.lblSubject.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblMessage
            // 
            appearance14.BackColor = System.Drawing.Color.Transparent;
            appearance14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance14.TextVAlignAsString = "Middle";
            this.lblMessage.Appearance = appearance14;
            this.lblMessage.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblMessage.Location = new System.Drawing.Point(17, 85);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(62, 22);
            this.lblMessage.TabIndex = 7;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.ultraLabel16);
            this.panel1.Controls.Add(this.btnSend);
            this.panel1.Controls.Add(this.Subject);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.message);
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Controls.Add(this.lblSubject);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(599, 255);
            this.panel1.TabIndex = 0;
            // 
            // txtEmail
            // 
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BorderColor = System.Drawing.Color.LightGray;
            appearance20.FontData.Name = "Calibri";
            appearance20.FontData.SizeInPoints = 11F;
            appearance20.ForeColor = System.Drawing.Color.Black;
            this.txtEmail.Appearance = appearance20;
            this.txtEmail.AutoSize = false;
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtEmail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Format = "";
            this.txtEmail.IsDirty = false;
            this.txtEmail.IsEmailTextBox = false;
            this.txtEmail.Location = new System.Drawing.Point(117, 25);
            this.txtEmail.Multiline = true;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(457, 25);
            this.txtEmail.TabIndex = 0;
            this.txtEmail.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // Subject
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.Color.LightGray;
            appearance1.FontData.Name = "Calibri";
            appearance1.FontData.SizeInPoints = 11F;
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.Subject.Appearance = appearance1;
            this.Subject.AutoSize = false;
            this.Subject.BackColor = System.Drawing.Color.White;
            this.Subject.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.Subject.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Subject.Format = "";
            this.Subject.IsDirty = false;
            this.Subject.IsEmailTextBox = false;
            this.Subject.Location = new System.Drawing.Point(117, 52);
            this.Subject.Multiline = true;
            this.Subject.Name = "Subject";
            this.Subject.Size = new System.Drawing.Size(457, 25);
            this.Subject.TabIndex = 1;
            this.Subject.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // message
            // 
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BorderColor = System.Drawing.Color.LightGray;
            appearance21.FontData.Name = "Calibri";
            appearance21.FontData.SizeInPoints = 11F;
            appearance21.ForeColor = System.Drawing.Color.Black;
            this.message.Appearance = appearance21;
            this.message.AutoSize = false;
            this.message.BackColor = System.Drawing.Color.White;
            this.message.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.message.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message.Format = "";
            this.message.IsDirty = false;
            this.message.IsEmailTextBox = false;
            this.message.Location = new System.Drawing.Point(117, 79);
            this.message.Multiline = true;
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(457, 119);
            this.message.TabIndex = 2;
            this.message.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // frmEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(623, 279);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmEmail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmEmail";
            this.Text = "Send Selection";
            this.Load += new System.EventHandler(this.frmEmail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmEmail_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Subject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.message)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSend;
        private Infragistics.Win.Misc.UltraLabel ultraLabel16;
        private Infragistics.Win.Misc.UltraLabel lblSubject;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private TAUtil.TATextBoxEditor message;
        private TAUtil.TATextBoxEditor Subject;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panel1;
        private TAUtil.TATextBoxEditor txtEmail;
    }
}