namespace BOLib
{
    partial class MsgBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgBox));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.imglstMsg = new System.Windows.Forms.ImageList(this.components);
            this.utxtMsgBody = new Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor();
            this.pic = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // imglstMsg
            // 
            this.imglstMsg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstMsg.ImageStream")));
            this.imglstMsg.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstMsg.Images.SetKeyName(0, "INFO.ICO");
            this.imglstMsg.Images.SetKeyName(1, "question.ico");
            this.imglstMsg.Images.SetKeyName(2, "WarningHS.png");
            this.imglstMsg.Images.SetKeyName(3, "error.ico");
            this.imglstMsg.Images.SetKeyName(4, "help.ico");
            this.imglstMsg.Images.SetKeyName(5, "Alert.png");
            this.imglstMsg.Images.SetKeyName(6, "ErrorInfo.png");
            this.imglstMsg.Images.SetKeyName(7, "Serious.png");
            // 
            // utxtMsgBody
            // 
            this.utxtMsgBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.AliceBlue;
            appearance2.BorderColor = System.Drawing.Color.AliceBlue;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.utxtMsgBody.Appearance = appearance2;
            this.utxtMsgBody.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.utxtMsgBody.Location = new System.Drawing.Point(50, 10);
            this.utxtMsgBody.Name = "utxtMsgBody";
            this.utxtMsgBody.ReadOnly = true;
            this.utxtMsgBody.Size = new System.Drawing.Size(438, 182);
            this.utxtMsgBody.TabIndex = 9;
            this.utxtMsgBody.TabStop = false;
            this.utxtMsgBody.Value = "aaaa <br/> bbbb";
            this.utxtMsgBody.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.utxtMsgBody_KeyPress);
            // 
            // pic
            // 
            this.pic.BorderShadowColor = System.Drawing.Color.Empty;
            this.pic.Image = ((object)(resources.GetObject("pic.Image")));
            this.pic.Location = new System.Drawing.Point(7, 12);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(38, 38);
            this.pic.TabIndex = 10;
            // 
            // pnlButton
            // 
            this.pnlButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButton.Location = new System.Drawing.Point(1, 197);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(498, 50);
            this.pnlButton.TabIndex = 11;
            // 
            // MsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(501, 250);
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.pic);
            this.Controls.Add(this.utxtMsgBody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MsgBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message Title";
            this.Shown += new System.EventHandler(this.MsgBox_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imglstMsg;
        private Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor utxtMsgBody;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox pic;
        private System.Windows.Forms.Panel pnlButton;

        //private System.Windows.Forms.Button ubtn1;
        //private System.Windows.Forms.Button ubtn2;
        //private System.Windows.Forms.Button ubtn3;
        //private System.Windows.Forms.Button ubtn4;
        //private System.Windows.Forms.RichTextBox utxtMsgBody;



    }
}