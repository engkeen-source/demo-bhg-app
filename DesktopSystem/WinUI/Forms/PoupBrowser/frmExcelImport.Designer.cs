namespace WinUI
{
    partial class frmExcelImport
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
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ExcelSheets = new TAUtil.TAComboBox();
            this.ExcelPath = new TAUtil.TATextBoxEditor();
            this.chkOverwrite = new TAUtil.TACheckBoxEditor();
            this.tabImport = new System.Windows.Forms.TabControl();
            this.tbStockCount = new System.Windows.Forms.TabPage();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelSheets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOverwrite)).BeginInit();
            this.tabImport.SuspendLayout();
            this.tbStockCount.SuspendLayout();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbImport});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(811, 70);
            this.tspBar.TabIndex = 0;
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbClose.Image = global::WinUI.Properties.Resources.close;
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbClose.RightToLeftAutoMirrorImage = true;
            this.tsbClose.Size = new System.Drawing.Size(70, 55);
            this.tsbClose.Text = "&Cancel";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tsbImport
            // 
            this.tsbImport.AutoSize = false;
            this.tsbImport.BackColor = System.Drawing.Color.Transparent;
            this.tsbImport.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbImport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbImport.Image = global::WinUI.Properties.Resources.copy_add_32;
            this.tsbImport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbImport.RightToLeftAutoMirrorImage = true;
            this.tsbImport.Size = new System.Drawing.Size(70, 55);
            this.tsbImport.Text = "&Import";
            this.tsbImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbImport.Click += new System.EventHandler(this.tsbImport_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(13, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Excel File Path*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(13, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Worksheet";
            // 
            // ExcelSheets
            // 
            appearance21.BorderColor = System.Drawing.Color.LightGray;
            appearance21.ForeColor = System.Drawing.Color.Black;
            this.ExcelSheets.Appearance = appearance21;
            this.ExcelSheets.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.ExcelSheets.AutoSize = false;
            this.ExcelSheets.ComboIsDirty = false;
            this.ExcelSheets.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ExcelSheets.Font = new System.Drawing.Font("Calibri", 11F);
            this.ExcelSheets.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.ExcelSheets.Location = new System.Drawing.Point(124, 134);
            this.ExcelSheets.Name = "ExcelSheets";
            this.ExcelSheets.Size = new System.Drawing.Size(300, 25);
            this.ExcelSheets.TabIndex = 1;
            this.ExcelSheets.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ExcelSheets.UserInputText = "";
            // 
            // ExcelPath
            // 
            appearance23.BorderColor = System.Drawing.Color.LightGray;
            appearance23.ForeColor = System.Drawing.Color.Black;
            this.ExcelPath.Appearance = appearance23;
            appearance3.Image = global::WinUI.Properties.Resources.open3;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance3;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007RibbonButton;
            editorButton1.Text = "";
            this.ExcelPath.ButtonsRight.Add(editorButton1);
            this.ExcelPath.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ExcelPath.Font = new System.Drawing.Font("Calibri", 10F);
            this.ExcelPath.Format = "";
            this.ExcelPath.IsDirty = false;
            this.ExcelPath.IsEmailTextBox = false;
            this.ExcelPath.Location = new System.Drawing.Point(124, 108);
            this.ExcelPath.Multiline = true;
            this.ExcelPath.Name = "ExcelPath";
            this.ExcelPath.Size = new System.Drawing.Size(455, 26);
            this.ExcelPath.TabIndex = 0;
            this.ExcelPath.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ExcelPath.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.ExcelPath_EditorButtonClick);
            // 
            // chkOverwrite
            // 
            this.chkOverwrite.cancelUpdate = false;
            this.chkOverwrite.Location = new System.Drawing.Point(120, 70);
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.Size = new System.Drawing.Size(300, 20);
            this.chkOverwrite.TabIndex = 0;
            this.chkOverwrite.Text = "Overwite counted Quantity";
            // 
            // tabImport
            // 
            this.tabImport.Controls.Add(this.tbStockCount);
            this.tabImport.Location = new System.Drawing.Point(0, 73);
            this.tabImport.Name = "tabImport";
            this.tabImport.SelectedIndex = 0;
            this.tabImport.Size = new System.Drawing.Size(786, 126);
            this.tabImport.TabIndex = 1;
            // 
            // tbStockCount
            // 
            this.tbStockCount.Controls.Add(this.chkOverwrite);
            this.tbStockCount.Location = new System.Drawing.Point(4, 22);
            this.tbStockCount.Name = "tbStockCount";
            this.tbStockCount.Padding = new System.Windows.Forms.Padding(3);
            this.tbStockCount.Size = new System.Drawing.Size(778, 100);
            this.tbStockCount.TabIndex = 0;
            this.tbStockCount.Text = "Stock Count";
            this.tbStockCount.UseVisualStyleBackColor = true;
            // 
            // frmExcelImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 347);
            this.Controls.Add(this.ExcelSheets);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ExcelPath);
            this.Controls.Add(this.tabImport);
            this.Controls.Add(this.tspBar);
            this.KeyPreview = true;
            this.Name = "frmExcelImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Data from Excel";
            this.Load += new System.EventHandler(this.frmExcelImport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmExcelImport_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelSheets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOverwrite)).EndInit();
            this.tabImport.ResumeLayout(false);
            this.tbStockCount.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbImport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private TAUtil.TAComboBox ExcelSheets;
        private TAUtil.TATextBoxEditor ExcelPath;
        private TAUtil.TACheckBoxEditor chkOverwrite;
        private System.Windows.Forms.TabControl tabImport;
        private System.Windows.Forms.TabPage tbStockCount;
    }
}