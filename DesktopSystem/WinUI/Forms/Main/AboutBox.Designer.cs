namespace WinUI.Forms
{
    partial class frmAboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAboutBox));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ComponentName", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Version");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tagrdVersionInfo = new TAUtil.TAGridEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnCopyInfo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.labelProductName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdVersionInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 181F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel.Controls.Add(this.tagrdVersionInfo, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.panel1, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.pictureBox2, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.btnCopyInfo, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.label1, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.okButton, 3, 6);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.label2, 1, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.988604F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.65242F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65.14286F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.142858F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(524, 512);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // tagrdVersionInfo
            // 
            this.tagrdVersionInfo.ActiveConnection = null;
            this.tagrdVersionInfo.AutoAddNewRow = false;
            this.tagrdVersionInfo.AutoUseCustomControlsInCells = false;
            this.tableLayoutPanel.SetColumnSpan(this.tagrdVersionInfo, 3);
            this.tagrdVersionInfo.DefaultValue = ((object)(resources.GetObject("tagrdVersionInfo.DefaultValue")));
            this.tagrdVersionInfo.DetailObjectKey = 0;
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdVersionInfo.DisplayLayout.Appearance = appearance2;
            ultraGridColumn1.Header.Caption = "Component Name";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 192;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 139;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.tagrdVersionInfo.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdVersionInfo.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdVersionInfo.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdVersionInfo.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdVersionInfo.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.tagrdVersionInfo.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdVersionInfo.DisplayLayout.GroupByBox.Hidden = true;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdVersionInfo.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.tagrdVersionInfo.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdVersionInfo.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdVersionInfo.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tagrdVersionInfo.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.tagrdVersionInfo.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdVersionInfo.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdVersionInfo.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdVersionInfo.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdVersionInfo.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdVersionInfo.DisplayLayout.Override.CellAppearance = appearance9;
            this.tagrdVersionInfo.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.tagrdVersionInfo.DisplayLayout.Override.CellPadding = 0;
            this.tagrdVersionInfo.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdVersionInfo.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlignAsString = "Left";
            this.tagrdVersionInfo.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.tagrdVersionInfo.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdVersionInfo.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.tagrdVersionInfo.DisplayLayout.Override.RowAppearance = appearance12;
            this.tagrdVersionInfo.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdVersionInfo.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.tagrdVersionInfo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdVersionInfo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdVersionInfo.DisplayLayout.UseFixedHeaders = true;
            this.tagrdVersionInfo.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.tagrdVersionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagrdVersionInfo.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdVersionInfo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdVersionInfo.HeaderObjectKey = null;
            this.tagrdVersionInfo.Location = new System.Drawing.Point(42, 93);
            this.tagrdVersionInfo.Name = "tagrdVersionInfo";
            this.tagrdVersionInfo.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdVersionInfo.Size = new System.Drawing.Size(429, 223);
            this.tagrdVersionInfo.TabIndex = 25;
            this.tagrdVersionInfo.Text = "taGridEditor1";
            this.tagrdVersionInfo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdVersionInfo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel.SetColumnSpan(this.panel1, 5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 463);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(518, 3);
            this.panel1.TabIndex = 27;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(42, 17);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(167, 70);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 28;
            this.pictureBox2.TabStop = false;
            // 
            // btnCopyInfo
            // 
            this.btnCopyInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopyInfo.Location = new System.Drawing.Point(396, 325);
            this.btnCopyInfo.Name = "btnCopyInfo";
            this.btnCopyInfo.Size = new System.Drawing.Size(75, 23);
            this.btnCopyInfo.TabIndex = 24;
            this.btnCopyInfo.Text = "Cop&y Info";
            this.btnCopyInfo.Click += new System.EventHandler(this.btnCopyInfo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 319);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 32);
            this.label1.TabIndex = 29;
            this.label1.Text = "To copy component name and version information, click Copy Info.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(396, 486);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // labelProductName
            // 
            this.labelProductName.Location = new System.Drawing.Point(6, 0);
            this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(0, 14);
            this.labelProductName.TabIndex = 19;
            this.labelProductName.Text = "Product Name";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.label2, 3);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 351);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(429, 109);
            this.label2.TabIndex = 30;
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmAboutBox
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.ClientSize = new System.Drawing.Size(542, 530);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutBox";
            this.Load += new System.EventHandler(this.AboutBox1_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdVersionInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Button okButton;
        private TAUtil.TAGridEditor tagrdVersionInfo;
        private System.Windows.Forms.Button btnCopyInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
