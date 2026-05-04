namespace WinUI
{
    partial class frmVehicleDataImport
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
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance440 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance349 = new Infragistics.Win.Appearance();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.tslReadOnly = new System.Windows.Forms.ToolStripLabel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.ultraLabel81 = new Infragistics.Win.Misc.UltraLabel();
            this.txtText = new TAUtil.TATextBoxEditor();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tspBar.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            this.panel1.Location = new System.Drawing.Point(65, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(479, 5);
            this.panel1.TabIndex = 59;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WinUI.Properties.Resources.Import;
            this.pictureBox1.Location = new System.Drawing.Point(13, 81);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 58;
            this.pictureBox1.TabStop = false;
            // 
            // ultraLabel10
            // 
            appearance18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel10.Appearance = appearance18;
            this.ultraLabel10.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel10.Location = new System.Drawing.Point(65, 93);
            this.ultraLabel10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(422, 22);
            this.ultraLabel10.TabIndex = 57;
            this.ultraLabel10.Text = "Vehicle Data Import";
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbImport,
            this.tsbClear,
            this.tslReadOnly});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(557, 70);
            this.tspBar.TabIndex = 56;
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tsbClose.Image = global::WinUI.Properties.Resources.close;
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbClose.RightToLeftAutoMirrorImage = true;
            this.tsbClose.Size = new System.Drawing.Size(60, 55);
            this.tsbClose.Text = "&Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tsbImport
            // 
            this.tsbImport.AutoSize = false;
            this.tsbImport.BackColor = System.Drawing.Color.Transparent;
            this.tsbImport.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tsbImport.Image = global::WinUI.Properties.Resources.DataImport1;
            this.tsbImport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbImport.RightToLeftAutoMirrorImage = true;
            this.tsbImport.Size = new System.Drawing.Size(60, 55);
            this.tsbImport.Text = "Import";
            this.tsbImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbImport.Click += new System.EventHandler(this.tsbImport_Click);
            // 
            // tsbClear
            // 
            this.tsbClear.AutoSize = false;
            this.tsbClear.BackColor = System.Drawing.Color.Transparent;
            this.tsbClear.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tsbClear.Image = global::WinUI.Properties.Resources.clear123;
            this.tsbClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbClear.RightToLeftAutoMirrorImage = true;
            this.tsbClear.Size = new System.Drawing.Size(60, 55);
            this.tsbClear.Text = "Cle&ar";
            this.tsbClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // tslReadOnly
            // 
            this.tslReadOnly.AutoSize = false;
            this.tslReadOnly.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslReadOnly.ForeColor = System.Drawing.Color.Blue;
            this.tslReadOnly.Name = "tslReadOnly";
            this.tslReadOnly.Size = new System.Drawing.Size(150, 67);
            // 
            // panel7
            // 
            this.panel7.AutoScroll = true;
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel7.Controls.Add(this.ultraLabel81);
            this.panel7.Controls.Add(this.txtText);
            this.panel7.Location = new System.Drawing.Point(13, 135);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(532, 339);
            this.panel7.TabIndex = 60;
            // 
            // ultraLabel81
            // 
            appearance440.BackColor = System.Drawing.Color.Transparent;
            appearance440.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance440.TextVAlignAsString = "Middle";
            this.ultraLabel81.Appearance = appearance440;
            this.ultraLabel81.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel81.Location = new System.Drawing.Point(7, 41);
            this.ultraLabel81.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel81.Name = "ultraLabel81";
            this.ultraLabel81.Size = new System.Drawing.Size(81, 34);
            this.ultraLabel81.TabIndex = 391;
            this.ultraLabel81.Text = "Import Data";
            this.ultraLabel81.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // txtText
            // 
            appearance349.BackColor = System.Drawing.Color.White;
            appearance349.BorderColor = System.Drawing.Color.LightGray;
            appearance349.FontData.Name = "Calibri";
            appearance349.FontData.SizeInPoints = 11F;
            appearance349.ForeColor = System.Drawing.Color.Black;
            this.txtText.Appearance = appearance349;
            this.txtText.AutoSize = false;
            this.txtText.BackColor = System.Drawing.Color.White;
            this.txtText.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtText.Font = new System.Drawing.Font("Calibri", 11F);
            this.txtText.Format = "";
            this.txtText.IsDirty = false;
            this.txtText.IsEmailTextBox = false;
            this.txtText.Location = new System.Drawing.Point(90, 16);
            this.txtText.MaxLength = 255;
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(424, 305);
            this.txtText.TabIndex = 1;
            this.txtText.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // frmVehicleDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(557, 477);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ultraLabel10);
            this.Controls.Add(this.tspBar);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVehicleDataImport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmVehicleDataImport";
            this.Text = "Vehicle Data Import";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbImport;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripLabel tslReadOnly;
        private System.Windows.Forms.Panel panel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel81;
        private TAUtil.TATextBoxEditor txtText;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}