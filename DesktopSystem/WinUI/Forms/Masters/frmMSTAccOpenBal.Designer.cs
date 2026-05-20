namespace WinUI
{
    partial class frmMSTAccOpenBal
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
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tslReadOnly = new System.Windows.Forms.ToolStripLabel();
            this.tagrdAccOpeningBalanceList = new TAUtil.TAGridEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.DiffAmount = new TAUtil.TANumericEditor();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdAccOpeningBalanceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffAmount)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbSave,
            this.tslReadOnly});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(1045, 70);
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
            this.tsbClose.Size = new System.Drawing.Size(60, 55);
            this.tsbClose.Text = "&Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.AutoSize = false;
            this.tsbSave.BackColor = System.Drawing.Color.Transparent;
            this.tsbSave.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbSave.Image = global::WinUI.Properties.Resources.save;
            this.tsbSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbSave.RightToLeftAutoMirrorImage = true;
            this.tsbSave.Size = new System.Drawing.Size(60, 55);
            this.tsbSave.Text = "&Save";
            this.tsbSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tslReadOnly
            // 
            this.tslReadOnly.AutoSize = false;
            this.tslReadOnly.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslReadOnly.ForeColor = System.Drawing.Color.Blue;
            this.tslReadOnly.Name = "tslReadOnly";
            this.tslReadOnly.Size = new System.Drawing.Size(150, 67);
            // 
            // tagrdAccOpeningBalanceList
            // 
            this.tagrdAccOpeningBalanceList.AutoAddNewRow = false;
            this.tagrdAccOpeningBalanceList.AutoUseCustomControlsInCells = false;
            this.tagrdAccOpeningBalanceList.DefaultValue = null;
            this.tagrdAccOpeningBalanceList.DetailObjectKey = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Appearance = appearance10;
            this.tagrdAccOpeningBalanceList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdAccOpeningBalanceList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdAccOpeningBalanceList.DisplayLayout.GroupByBox.Appearance = appearance11;
            appearance12.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdAccOpeningBalanceList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance12;
            this.tagrdAccOpeningBalanceList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdAccOpeningBalanceList.DisplayLayout.GroupByBox.Hidden = true;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance13.BackColor2 = System.Drawing.SystemColors.Control;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdAccOpeningBalanceList.DisplayLayout.GroupByBox.PromptAppearance = appearance13;
            this.tagrdAccOpeningBalanceList.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdAccOpeningBalanceList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.ActiveCellAppearance = appearance14;
            appearance2.BackColor = System.Drawing.Color.Gold;
            appearance2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.CellAppearance = appearance20;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.CellPadding = 0;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.RowAppearance = appearance23;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdAccOpeningBalanceList.DisplayLayout.Override.TemplateAddRowAppearance = appearance27;
            this.tagrdAccOpeningBalanceList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdAccOpeningBalanceList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdAccOpeningBalanceList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdAccOpeningBalanceList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.tagrdAccOpeningBalanceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagrdAccOpeningBalanceList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdAccOpeningBalanceList.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tagrdAccOpeningBalanceList.HeaderObjectKey = null;
            this.tagrdAccOpeningBalanceList.Location = new System.Drawing.Point(0, 0);
            this.tagrdAccOpeningBalanceList.Name = "tagrdAccOpeningBalanceList";
            this.tagrdAccOpeningBalanceList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdAccOpeningBalanceList.Size = new System.Drawing.Size(1019, 591);
            this.tagrdAccOpeningBalanceList.TabIndex = 0;
            this.tagrdAccOpeningBalanceList.Text = "taGridEditor1";
            this.tagrdAccOpeningBalanceList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdAccOpeningBalanceList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdAccOpeningBalanceList.CustomCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.tagrdAccOpeningBalanceList_CustomCellUpdate);
            this.tagrdAccOpeningBalanceList.CustomDataError += new TAUtil.TADataErrorEventHandler(this.tagrdAccOpeningBalanceList_CustomDataError);
            this.tagrdAccOpeningBalanceList.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.tagrdAccOpeningBalanceList_InitializeRow);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(603, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Different Amount";
            // 
            // DiffAmount
            // 
            this.DiffAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance1.BorderColor = System.Drawing.Color.LightGray;
            appearance1.TextHAlignAsString = "Right";
            this.DiffAmount.Appearance = appearance1;
            this.DiffAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DiffAmount.Font = new System.Drawing.Font("Calibri", 10F);
            this.DiffAmount.ForceExitByRestoreValue = false;
            this.DiffAmount.Format = "";
            this.DiffAmount.Location = new System.Drawing.Point(716, 21);
            this.DiffAmount.Name = "DiffAmount";
            this.DiffAmount.NumberType = TAUtil.NumericType.Decimal;
            this.DiffAmount.Size = new System.Drawing.Size(300, 25);
            this.DiffAmount.TabIndex = 1;
            this.DiffAmount.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DiffAmount.ZeroIfEmpty = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(12, 129);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tagrdAccOpeningBalanceList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.splitContainer1.Panel2.Controls.Add(this.DiffAmount);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1021, 722);
            this.splitContainer1.SplitterDistance = 593;
            this.splitContainer1.TabIndex = 36;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            this.panel2.Location = new System.Drawing.Point(64, 105);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(968, 5);
            this.panel2.TabIndex = 48;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WinUI.Properties.Resources.chartofaccount_openbal;
            this.pictureBox1.Location = new System.Drawing.Point(12, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            // 
            // ultraLabel11
            // 
            appearance18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel11.Appearance = appearance18;
            this.ultraLabel11.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel11.Location = new System.Drawing.Point(64, 85);
            this.ultraLabel11.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(422, 22);
            this.ultraLabel11.TabIndex = 46;
            this.ultraLabel11.Text = "ACCOUNT OPENING BALANCE";
            // 
            // frmMSTAccOpenBal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1045, 863);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ultraLabel11);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tspBar);
            this.Name = "frmMSTAccOpenBal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account Opening Balance ";
            this.Load += new System.EventHandler(this.frmMSTAccOpenBal_Load);
            this.Shown += new System.EventHandler(this.frmMSTAccOpenBal_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMSTAccOpenBal_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMSTAccOpenBal_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdAccOpeningBalanceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffAmount)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private TAUtil.TAGridEditor tagrdAccOpeningBalanceList;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.Label label1;
        private TAUtil.TANumericEditor DiffAmount;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripLabel tslReadOnly;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
    }
}