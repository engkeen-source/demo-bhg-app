namespace WinUI
{
    partial class frmDocRelationship
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
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("BindingList`1", -1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tagrdDocList = new TAUtil.TAGridEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblDocID = new Infragistics.Win.Misc.UltraLabel();
            this.RunSummary = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbExport});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(795, 73);
            this.tspBar.TabIndex = 6;
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tsbClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbClose.Image = global::WinUI.Properties.Resources.close;
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbClose.RightToLeftAutoMirrorImage = true;
            this.tsbClose.Size = new System.Drawing.Size(65, 55);
            this.tsbClose.Text = "&Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tagrdDocList
            // 
            this.tagrdDocList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdDocList.AutoAddNewRow = false;
            this.tagrdDocList.AutoUseCustomControlsInCells = false;
            this.tagrdDocList.DefaultValue = null;
            this.tagrdDocList.DetailObjectKey = 0;
            appearance61.AlphaLevel = ((short)(255));
            appearance61.BackColor = System.Drawing.Color.AliceBlue;
            appearance61.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance61.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance61.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdDocList.DisplayLayout.Appearance = appearance61;
            appearance1.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance2;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdDocList.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdDocList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance64.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance64.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance64.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDocList.DisplayLayout.GroupByBox.Appearance = appearance64;
            appearance65.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance65;
            this.tagrdDocList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance66.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance66.BackColor2 = System.Drawing.SystemColors.Control;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance66.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocList.DisplayLayout.GroupByBox.PromptAppearance = appearance66;
            this.tagrdDocList.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdDocList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance67.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdDocList.DisplayLayout.Override.ActiveCellAppearance = appearance67;
            appearance68.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdDocList.DisplayLayout.Override.ActiveRowAppearance = appearance68;
            this.tagrdDocList.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdDocList.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.tagrdDocList.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance69.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdDocList.DisplayLayout.Override.CardAreaAppearance = appearance69;
            appearance70.BorderColor = System.Drawing.Color.Silver;
            appearance70.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdDocList.DisplayLayout.Override.CellAppearance = appearance70;
            this.tagrdDocList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdDocList.DisplayLayout.Override.CellPadding = 0;
            this.tagrdDocList.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdDocList.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance71.AlphaLevel = ((short)(255));
            appearance71.BackColor = System.Drawing.Color.AliceBlue;
            appearance71.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance71.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance71.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance71.ForeColor = System.Drawing.Color.Black;
            appearance71.TextHAlignAsString = "Left";
            this.tagrdDocList.DisplayLayout.Override.HeaderAppearance = appearance71;
            this.tagrdDocList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdDocList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance72.BackColor = System.Drawing.Color.White;
            appearance72.BackColor2 = System.Drawing.Color.White;
            appearance72.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance72.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance72.ForeColor = System.Drawing.Color.Black;
            appearance72.TextVAlignAsString = "Middle";
            this.tagrdDocList.DisplayLayout.Override.RowAppearance = appearance72;
            appearance73.AlphaLevel = ((short)(255));
            appearance73.BackColor = System.Drawing.Color.AliceBlue;
            appearance73.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance73.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance73.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdDocList.DisplayLayout.Override.RowSelectorAppearance = appearance73;
            appearance74.BackColor = System.Drawing.Color.AliceBlue;
            appearance74.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance74.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance74.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdDocList.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance74;
            this.tagrdDocList.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdDocList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocList.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdDocList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance75.BackColor = System.Drawing.Color.Gold;
            appearance75.BackColor2 = System.Drawing.Color.White;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdDocList.DisplayLayout.Override.SelectedRowAppearance = appearance75;
            this.tagrdDocList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDocList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDocList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDocList.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdDocList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDocList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDocList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDocList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdDocList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDocList.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDocList.HeaderObjectKey = null;
            this.tagrdDocList.Location = new System.Drawing.Point(0, 107);
            this.tagrdDocList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdDocList.Name = "tagrdDocList";
            this.tagrdDocList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDocList.Size = new System.Drawing.Size(795, 455);
            this.tagrdDocList.TabIndex = 7;
            this.tagrdDocList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDocList.DoubleClickCell += new Infragistics.Win.UltraWinGrid.DoubleClickCellEventHandler(this.tagrdDocList_DoubleClickCell);
            // 
            // ultraLabel2
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance4;
            this.ultraLabel2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel2.Location = new System.Drawing.Point(12, 77);
            this.ultraLabel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(118, 22);
            this.ultraLabel2.TabIndex = 122;
            this.ultraLabel2.Text = "Document Number";
            this.ultraLabel2.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblDocID
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.TextVAlignAsString = "Middle";
            this.lblDocID.Appearance = appearance3;
            this.lblDocID.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblDocID.Location = new System.Drawing.Point(136, 77);
            this.lblDocID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblDocID.Name = "lblDocID";
            this.lblDocID.Size = new System.Drawing.Size(333, 22);
            this.lblDocID.TabIndex = 123;
            this.lblDocID.Text = "Document Number";
            this.lblDocID.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // RunSummary
            // 
            appearance7.TextHAlignAsString = "Center";
            this.RunSummary.Appearance = appearance7;
            this.RunSummary.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Popup;
            this.RunSummary.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            appearance8.TextHAlignAsString = "Center";
            this.RunSummary.CheckedAppearance = appearance8;
            this.RunSummary.Location = new System.Drawing.Point(489, 76);
            this.RunSummary.Name = "RunSummary";
            this.RunSummary.Size = new System.Drawing.Size(109, 29);
            this.RunSummary.Style = Infragistics.Win.EditCheckStyle.Button;
            this.RunSummary.TabIndex = 124;
            this.RunSummary.Text = "Show Detail";
            this.RunSummary.CheckedChanged += new System.EventHandler(this.RunSummary_CheckedChanged);
            // 
            // tsbExport
            // 
            this.tsbExport.AutoSize = false;
            this.tsbExport.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbExport.Image = global::WinUI.Properties.Resources.export;
            this.tsbExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(65, 55);
            this.tsbExport.Text = "Export";
            this.tsbExport.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // frmDocRelationship
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 581);
            this.Controls.Add(this.RunSummary);
            this.Controls.Add(this.lblDocID);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.tagrdDocList);
            this.Controls.Add(this.tspBar);
            this.Name = "frmDocRelationship";
            this.ShowInTaskbar = false;
            this.Text = "Document Relationship";
            this.Load += new System.EventHandler(this.frmDocRelationship_Load);
            this.Shown += new System.EventHandler(this.frmDocRelationship_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocRelationship_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private TAUtil.TAGridEditor tagrdDocList;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel lblDocID;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor RunSummary;
        private System.Windows.Forms.ToolStripButton tsbExport;
    }
}