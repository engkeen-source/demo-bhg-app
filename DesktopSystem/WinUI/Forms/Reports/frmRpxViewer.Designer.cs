namespace WinUI
{
    partial class frmRpxViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRpxViewer));
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslReadOnly = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tagrdFinExport = new TAUtil.TAGridEditor();
            this.pdfExport1 = new DataDynamics.ActiveReports.Export.Pdf.PdfExport();
            this.xlsExport1 = new DataDynamics.ActiveReports.Export.Xls.XlsExport();
            this.htmlExport1 = new DataDynamics.ActiveReports.Export.Html.HtmlExport();
            this.rtfExport1 = new DataDynamics.ActiveReports.Export.Rtf.RtfExport();
            this.textExport1 = new DataDynamics.ActiveReports.Export.Text.TextExport();
            this.arvMain = new DataDynamics.ActiveReports.Viewer.Viewer();
            this.tspBar.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdFinExport)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AllowMerge = false;
            this.tspBar.AutoSize = false;
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.toolStripSeparator1,
            this.tslReadOnly});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(949, 70);
            this.tspBar.TabIndex = 0;
            this.tspBar.Visible = false;
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbClose.Image = global::WinUI.Properties.Resources.close_a_32;
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbClose.RightToLeftAutoMirrorImage = true;
            this.tsbClose.Size = new System.Drawing.Size(70, 55);
            this.tsbClose.Text = "&Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 70);
            // 
            // tslReadOnly
            // 
            this.tslReadOnly.AutoSize = false;
            this.tslReadOnly.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslReadOnly.ForeColor = System.Drawing.Color.Blue;
            this.tslReadOnly.Name = "tslReadOnly";
            this.tslReadOnly.Size = new System.Drawing.Size(150, 67);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.SteelBlue;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.splitContainer1.Panel1Collapsed = true;
            this.splitContainer1.Panel1MinSize = 20;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.splitContainer1.Panel2.Controls.Add(this.arvMain);
            this.splitContainer1.Panel2.Controls.Add(this.tagrdFinExport);
            this.splitContainer1.Size = new System.Drawing.Size(949, 714);
            this.splitContainer1.SplitterDistance = 41;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // tagrdFinExport
            // 
            this.tagrdFinExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdFinExport.AutoAddNewRow = false;
            this.tagrdFinExport.AutoUseCustomControlsInCells = false;
            this.tagrdFinExport.DefaultValue = null;
            this.tagrdFinExport.DetailObjectKey = 0;
            appearance61.AlphaLevel = ((short)(255));
            appearance61.BackColor = System.Drawing.Color.AliceBlue;
            appearance61.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance61.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance61.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdFinExport.DisplayLayout.Appearance = appearance61;
            appearance1.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance2;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdFinExport.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdFinExport.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance64.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance64.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance64.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdFinExport.DisplayLayout.GroupByBox.Appearance = appearance64;
            appearance65.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdFinExport.DisplayLayout.GroupByBox.BandLabelAppearance = appearance65;
            this.tagrdFinExport.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance66.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance66.BackColor2 = System.Drawing.SystemColors.Control;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance66.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdFinExport.DisplayLayout.GroupByBox.PromptAppearance = appearance66;
            this.tagrdFinExport.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdFinExport.DisplayLayout.MaxRowScrollRegions = 1;
            appearance67.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdFinExport.DisplayLayout.Override.ActiveCellAppearance = appearance67;
            appearance68.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdFinExport.DisplayLayout.Override.ActiveRowAppearance = appearance68;
            this.tagrdFinExport.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdFinExport.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.tagrdFinExport.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance69.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdFinExport.DisplayLayout.Override.CardAreaAppearance = appearance69;
            appearance70.BorderColor = System.Drawing.Color.Silver;
            appearance70.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdFinExport.DisplayLayout.Override.CellAppearance = appearance70;
            this.tagrdFinExport.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdFinExport.DisplayLayout.Override.CellPadding = 0;
            this.tagrdFinExport.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdFinExport.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance71.AlphaLevel = ((short)(255));
            appearance71.BackColor = System.Drawing.Color.AliceBlue;
            appearance71.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance71.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance71.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance71.ForeColor = System.Drawing.Color.Black;
            appearance71.TextHAlignAsString = "Left";
            this.tagrdFinExport.DisplayLayout.Override.HeaderAppearance = appearance71;
            this.tagrdFinExport.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdFinExport.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance72.BackColor = System.Drawing.Color.White;
            appearance72.BackColor2 = System.Drawing.Color.White;
            appearance72.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance72.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance72.ForeColor = System.Drawing.Color.Black;
            appearance72.TextVAlignAsString = "Middle";
            this.tagrdFinExport.DisplayLayout.Override.RowAppearance = appearance72;
            appearance73.AlphaLevel = ((short)(255));
            appearance73.BackColor = System.Drawing.Color.AliceBlue;
            appearance73.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance73.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance73.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdFinExport.DisplayLayout.Override.RowSelectorAppearance = appearance73;
            appearance74.BackColor = System.Drawing.Color.AliceBlue;
            appearance74.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance74.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance74.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdFinExport.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance74;
            this.tagrdFinExport.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdFinExport.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdFinExport.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdFinExport.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance75.BackColor = System.Drawing.Color.Gold;
            appearance75.BackColor2 = System.Drawing.Color.White;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdFinExport.DisplayLayout.Override.SelectedRowAppearance = appearance75;
            this.tagrdFinExport.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdFinExport.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdFinExport.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdFinExport.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdFinExport.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdFinExport.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdFinExport.DisplayLayout.UseFixedHeaders = true;
            this.tagrdFinExport.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdFinExport.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdFinExport.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdFinExport.HeaderObjectKey = null;
            this.tagrdFinExport.Location = new System.Drawing.Point(141, 94);
            this.tagrdFinExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdFinExport.Name = "tagrdFinExport";
            this.tagrdFinExport.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdFinExport.Size = new System.Drawing.Size(340, 93);
            this.tagrdFinExport.TabIndex = 5;
            this.tagrdFinExport.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdFinExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdFinExport.Visible = false;
            // 
            // xlsExport1
            // 
            this.xlsExport1.Tweak = 0;
            // 
            // rtfExport1
            // 
            this.rtfExport1.EnableShapes = false;
            // 
            // textExport1
            // 
            this.textExport1.Encoding = ((System.Text.Encoding)(resources.GetObject("textExport1.Encoding")));
            // 
            // arvMain
            // 
            this.arvMain.BackColor = System.Drawing.SystemColors.Control;
            this.arvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arvMain.Document = new DataDynamics.ActiveReports.Document.Document("ARNet Document");
            this.arvMain.Location = new System.Drawing.Point(0, 0);
            this.arvMain.Name = "arvMain";
            this.arvMain.ReportViewer.CurrentPage = 0;
            this.arvMain.ReportViewer.MultiplePageCols = 3;
            this.arvMain.ReportViewer.MultiplePageRows = 2;
            this.arvMain.ReportViewer.ViewType = DataDynamics.ActiveReports.Viewer.ViewType.Normal;
            this.arvMain.Size = new System.Drawing.Size(949, 714);
            this.arvMain.TabIndex = 6;
            this.arvMain.TableOfContents.Text = "Table Of Contents";
            this.arvMain.TableOfContents.Width = 200;
            this.arvMain.TabTitleLength = 35;
            this.arvMain.Toolbar.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.arvMain.DoubleClick += new System.EventHandler(this.arvMain_DoubleClick);
            this.arvMain.ToolClick += new DataDynamics.ActiveReports.Toolbar.ToolClickEventHandler(this.arvMain_ToolClick);
            this.arvMain.HyperLink += new DataDynamics.ActiveReports.Viewer.HyperLinkEventHandler(this.arvMain_HyperLink);
            // 
            // frmRpxViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 714);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tspBar);
            this.Name = "frmRpxViewer";
            this.Text = "Report File List";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmRpxViewer_Load);
            this.Activated += new System.EventHandler(this.frmRpxViewer_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRpxViewer_FormClosing);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdFinExport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tslReadOnly;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DataDynamics.ActiveReports.Export.Pdf.PdfExport pdfExport1;
        private DataDynamics.ActiveReports.Export.Xls.XlsExport xlsExport1;
        private DataDynamics.ActiveReports.Export.Html.HtmlExport htmlExport1;
        private DataDynamics.ActiveReports.Export.Rtf.RtfExport rtfExport1;
        private DataDynamics.ActiveReports.Export.Text.TextExport textExport1;
        private TAUtil.TAGridEditor tagrdFinExport;
        public DataDynamics.ActiveReports.Viewer.Viewer arvMain;        
    }
}