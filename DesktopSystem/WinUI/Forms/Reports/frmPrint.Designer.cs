namespace WinUI
{
    partial class frmPrint
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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.lblName = new System.Windows.Forms.Label();
            this.gbOrientation = new System.Windows.Forms.GroupBox();
            this.tanuToPage = new TAUtil.TANumericEditor();
            this.tanuFromPage = new TAUtil.TANumericEditor();
            this.lblBottom = new System.Windows.Forms.Label();
            this.lblTop = new System.Windows.Forms.Label();
            this.optPageRanges = new System.Windows.Forms.RadioButton();
            this.optAllPages = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CollateCopies = new TAUtil.TACheckBoxEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.NoOfCopies = new System.Windows.Forms.NumericUpDown();
            this.lblPrinterNm = new System.Windows.Forms.Label();
            this.gbOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tanuToPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tanuFromPage)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollateCopies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoOfCopies)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(10, 14);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(54, 17);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Printer :";
            // 
            // gbOrientation
            // 
            this.gbOrientation.Controls.Add(this.tanuToPage);
            this.gbOrientation.Controls.Add(this.tanuFromPage);
            this.gbOrientation.Controls.Add(this.lblBottom);
            this.gbOrientation.Controls.Add(this.lblTop);
            this.gbOrientation.Controls.Add(this.optPageRanges);
            this.gbOrientation.Controls.Add(this.optAllPages);
            this.gbOrientation.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.gbOrientation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gbOrientation.Location = new System.Drawing.Point(13, 57);
            this.gbOrientation.Name = "gbOrientation";
            this.gbOrientation.Size = new System.Drawing.Size(300, 96);
            this.gbOrientation.TabIndex = 2;
            this.gbOrientation.TabStop = false;
            this.gbOrientation.Text = "Print Range";
            // 
            // tanuToPage
            // 
            appearance5.BorderColor = System.Drawing.Color.LightGray;
            appearance5.TextHAlignAsString = "Right";
            this.tanuToPage.Appearance = appearance5;
            this.tanuToPage.AutoSize = false;
            this.tanuToPage.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.tanuToPage.Enabled = false;
            this.tanuToPage.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tanuToPage.ForceExitByRestoreValue = false;
            this.tanuToPage.Format = "0.##";
            this.tanuToPage.Location = new System.Drawing.Point(216, 51);
            this.tanuToPage.Name = "tanuToPage";
            this.tanuToPage.NullText = "1";
            this.tanuToPage.NumberType = TAUtil.NumericType.Decimal;
            this.tanuToPage.Size = new System.Drawing.Size(46, 27);
            this.tanuToPage.TabIndex = 9;
            this.tanuToPage.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.tanuToPage.ZeroIfEmpty = false;
            // 
            // tanuFromPage
            // 
            appearance2.BorderColor = System.Drawing.Color.LightGray;
            appearance2.TextHAlignAsString = "Right";
            this.tanuFromPage.Appearance = appearance2;
            this.tanuFromPage.AutoSize = false;
            this.tanuFromPage.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.tanuFromPage.Enabled = false;
            this.tanuFromPage.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tanuFromPage.ForceExitByRestoreValue = false;
            this.tanuFromPage.Format = "0.##";
            this.tanuFromPage.Location = new System.Drawing.Point(125, 51);
            this.tanuFromPage.Name = "tanuFromPage";
            this.tanuFromPage.NullText = "1";
            this.tanuFromPage.NumberType = TAUtil.NumericType.Decimal;
            this.tanuFromPage.Size = new System.Drawing.Size(46, 27);
            this.tanuFromPage.TabIndex = 7;
            this.tanuFromPage.Text = "1";
            this.tanuFromPage.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.tanuFromPage.ZeroIfEmpty = false;
            // 
            // lblBottom
            // 
            this.lblBottom.AutoSize = true;
            this.lblBottom.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblBottom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblBottom.Location = new System.Drawing.Point(185, 56);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(25, 17);
            this.lblBottom.TabIndex = 10;
            this.lblBottom.Text = "To:";
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTop.Location = new System.Drawing.Point(82, 56);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(41, 17);
            this.lblTop.TabIndex = 8;
            this.lblTop.Text = "From:";
            // 
            // optPageRanges
            // 
            this.optPageRanges.AutoSize = true;
            this.optPageRanges.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.optPageRanges.Location = new System.Drawing.Point(15, 54);
            this.optPageRanges.Name = "optPageRanges";
            this.optPageRanges.Size = new System.Drawing.Size(59, 21);
            this.optPageRanges.TabIndex = 1;
            this.optPageRanges.Text = "Pages";
            this.optPageRanges.UseVisualStyleBackColor = true;
            this.optPageRanges.CheckedChanged += new System.EventHandler(this.optPageRanges_CheckedChanged);
            // 
            // optAllPages
            // 
            this.optAllPages.AutoSize = true;
            this.optAllPages.Checked = true;
            this.optAllPages.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.optAllPages.Location = new System.Drawing.Point(15, 25);
            this.optAllPages.Name = "optAllPages";
            this.optAllPages.Size = new System.Drawing.Size(40, 21);
            this.optAllPages.TabIndex = 0;
            this.optAllPages.TabStop = true;
            this.optAllPages.Text = "All";
            this.optAllPages.UseVisualStyleBackColor = true;
            this.optAllPages.CheckedChanged += new System.EventHandler(this.optAllPages_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnOK.Image = global::WinUI.Properties.Resources.OK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(368, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(81, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.Image = global::WinUI.Properties.Resources.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(368, 43);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.CollateCopies);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.NoOfCopies);
            this.panel1.Controls.Add(this.lblPrinterNm);
            this.panel1.Controls.Add(this.gbOrientation);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(476, 169);
            this.panel1.TabIndex = 0;
            // 
            // CollateCopies
            // 
            appearance9.FontData.ItalicAsString = "True";
            appearance9.FontData.Name = "Calibri";
            appearance9.FontData.SizeInPoints = 10F;
            appearance9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CollateCopies.Appearance = appearance9;
            this.CollateCopies.cancelUpdate = false;
            this.CollateCopies.Checked = true;
            this.CollateCopies.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CollateCopies.Location = new System.Drawing.Point(348, 122);
            this.CollateCopies.Name = "CollateCopies";
            this.CollateCopies.Size = new System.Drawing.Size(115, 20);
            this.CollateCopies.TabIndex = 10;
            this.CollateCopies.Text = "Collate Copies";
            this.CollateCopies.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(345, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Copies:";
            // 
            // NoOfCopies
            // 
            this.NoOfCopies.Location = new System.Drawing.Point(399, 93);
            this.NoOfCopies.Name = "NoOfCopies";
            this.NoOfCopies.Size = new System.Drawing.Size(50, 20);
            this.NoOfCopies.TabIndex = 8;
            this.NoOfCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NoOfCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NoOfCopies.ValueChanged += new System.EventHandler(this.NoOfCopies_ValueChanged);
            // 
            // lblPrinterNm
            // 
            this.lblPrinterNm.AutoSize = true;
            this.lblPrinterNm.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrinterNm.Location = new System.Drawing.Point(83, 14);
            this.lblPrinterNm.Name = "lblPrinterNm";
            this.lblPrinterNm.Size = new System.Drawing.Size(90, 17);
            this.lblPrinterNm.TabIndex = 7;
            this.lblPrinterNm.Text = "System Printer";
            // 
            // frmPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(499, 193);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print";
            this.Load += new System.EventHandler(this.frmPrint_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPrint_KeyDown);
            this.gbOrientation.ResumeLayout(false);
            this.gbOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tanuToPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tanuFromPage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollateCopies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoOfCopies)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.GroupBox gbOrientation;
        private System.Windows.Forms.RadioButton optPageRanges;
        private System.Windows.Forms.RadioButton optAllPages;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private TAUtil.TANumericEditor tanuToPage;
        private TAUtil.TANumericEditor tanuFromPage;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.Label lblPrinterNm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NoOfCopies;
        private TAUtil.TACheckBoxEditor CollateCopies;
    }
}