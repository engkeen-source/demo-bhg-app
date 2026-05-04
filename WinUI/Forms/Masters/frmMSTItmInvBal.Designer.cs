namespace WinUI
{
    partial class frmMSTItmInvBal
    {

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
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("BindingList`1", -1);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance562 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMSTItmInvBal));
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbClearFilter = new System.Windows.Forms.ToolStripButton();
            this.tslConType = new System.Windows.Forms.ToolStripLabel();
            this.bdsDocList = new System.Windows.Forms.BindingSource(this.components);
            this.tagrdList = new TAUtil.TAGridEditor();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.ultraLabel26 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClearFilter = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.txtKeywordSearch = new TAUtil.TATextBoxEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotalRows = new Infragistics.Win.Misc.UltraLabel();
            this.lblAll = new Infragistics.Win.Misc.UltraLabel();
            this.lblOrange = new Infragistics.Win.Misc.UltraLabel();
            this.lblBlue = new Infragistics.Win.Misc.UltraLabel();
            this.lblRed = new Infragistics.Win.Misc.UltraLabel();
            this.lblGreen = new Infragistics.Win.Misc.UltraLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDocList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordSearch)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbExport,
            this.tsbRefresh,
            this.tsbClearFilter,
            this.tslConType});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(1392, 72);
            this.tspBar.TabIndex = 0;
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
            this.tsbClose.Size = new System.Drawing.Size(60, 55);
            this.tsbClose.Text = "&Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tsbExport
            // 
            this.tsbExport.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tsbExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbExport.Image = global::WinUI.Properties.Resources.export;
            this.tsbExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(49, 69);
            this.tsbExport.Text = "Export";
            this.tsbExport.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.AutoSize = false;
            this.tsbRefresh.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbRefresh.Image = global::WinUI.Properties.Resources.refresh32;
            this.tsbRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(65, 55);
            this.tsbRefresh.Text = "&Refresh";
            this.tsbRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbClearFilter
            // 
            this.tsbClearFilter.AutoSize = false;
            this.tsbClearFilter.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tsbClearFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbClearFilter.Image = global::WinUI.Properties.Resources.filter_refresh_322;
            this.tsbClearFilter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClearFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearFilter.Name = "tsbClearFilter";
            this.tsbClearFilter.Size = new System.Drawing.Size(130, 55);
            this.tsbClearFilter.Text = "Clear Filter && Refresh";
            this.tsbClearFilter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbClearFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClearFilter.Click += new System.EventHandler(this.tsbClearFilter_Click);
            // 
            // tslConType
            // 
            this.tslConType.AutoSize = false;
            this.tslConType.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslConType.ForeColor = System.Drawing.Color.Blue;
            this.tslConType.Name = "tslConType";
            this.tslConType.Size = new System.Drawing.Size(150, 67);
            // 
            // tagrdList
            // 
            this.tagrdList.ActiveConnection = null;
            this.tagrdList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdList.AutoAddNewRow = false;
            this.tagrdList.AutoUseCustomControlsInCells = false;
            this.tagrdList.DataSource = this.bdsDocList;
            this.tagrdList.DefaultValue = null;
            this.tagrdList.DetailObjectKey = 0;
            appearance61.AlphaLevel = ((short)(255));
            appearance61.BackColor = System.Drawing.Color.AliceBlue;
            appearance61.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance61.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance61.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdList.DisplayLayout.Appearance = appearance61;
            appearance3.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance4;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdList.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance64.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance64.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance64.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdList.DisplayLayout.GroupByBox.Appearance = appearance64;
            appearance65.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance65;
            this.tagrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance66.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance66.BackColor2 = System.Drawing.SystemColors.Control;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance66.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdList.DisplayLayout.GroupByBox.PromptAppearance = appearance66;
            this.tagrdList.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance67.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdList.DisplayLayout.Override.ActiveCellAppearance = appearance67;
            appearance68.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdList.DisplayLayout.Override.ActiveRowAppearance = appearance68;
            this.tagrdList.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdList.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.tagrdList.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance69.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdList.DisplayLayout.Override.CardAreaAppearance = appearance69;
            appearance70.BorderColor = System.Drawing.Color.Silver;
            appearance70.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdList.DisplayLayout.Override.CellAppearance = appearance70;
            this.tagrdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdList.DisplayLayout.Override.CellPadding = 0;
            this.tagrdList.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdList.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance71.AlphaLevel = ((short)(255));
            appearance71.BackColor = System.Drawing.Color.AliceBlue;
            appearance71.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance71.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance71.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance71.ForeColor = System.Drawing.Color.Black;
            appearance71.TextHAlignAsString = "Left";
            this.tagrdList.DisplayLayout.Override.HeaderAppearance = appearance71;
            this.tagrdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance72.BackColor = System.Drawing.Color.White;
            appearance72.BackColor2 = System.Drawing.Color.White;
            appearance72.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance72.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance72.ForeColor = System.Drawing.Color.Black;
            appearance72.TextVAlignAsString = "Middle";
            this.tagrdList.DisplayLayout.Override.RowAppearance = appearance72;
            appearance73.AlphaLevel = ((short)(255));
            appearance73.BackColor = System.Drawing.Color.AliceBlue;
            appearance73.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance73.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance73.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdList.DisplayLayout.Override.RowSelectorAppearance = appearance73;
            appearance74.BackColor = System.Drawing.Color.AliceBlue;
            appearance74.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance74.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance74.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdList.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance74;
            this.tagrdList.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdList.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance75.BackColor = System.Drawing.Color.Gold;
            appearance75.BackColor2 = System.Drawing.Color.White;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdList.DisplayLayout.Override.SelectedRowAppearance = appearance75;
            this.tagrdList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdList.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdList.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdList.HeaderObjectKey = null;
            this.tagrdList.Location = new System.Drawing.Point(12, 194);
            this.tagrdList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdList.Name = "tagrdList";
            this.tagrdList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdList.Size = new System.Drawing.Size(1368, 440);
            this.tagrdList.TabIndex = 131;
            this.tagrdList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdList.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.tagrdList_InitializeRow);
            this.tagrdList.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.tagrdList_DoubleClickRow);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            this.panel2.Location = new System.Drawing.Point(68, 110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1312, 10);
            this.panel2.TabIndex = 458;
            // 
            // ultraLabel26
            // 
            appearance562.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            appearance562.TextVAlignAsString = "Middle";
            this.ultraLabel26.Appearance = appearance562;
            this.ultraLabel26.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel26.Location = new System.Drawing.Point(68, 87);
            this.ultraLabel26.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel26.Name = "ultraLabel26";
            this.ultraLabel26.Size = new System.Drawing.Size(315, 22);
            this.ultraLabel26.TabIndex = 457;
            this.ultraLabel26.Text = "INVENTORY CROSS-CHECKING";
            // 
            // btnClearFilter
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClearFilter.Appearance = appearance1;
            this.btnClearFilter.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClearFilter.Location = new System.Drawing.Point(356, 151);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(28, 26);
            this.btnClearFilter.TabIndex = 461;
            this.btnClearFilter.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // ultraLabel6
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance5.ForeColor = System.Drawing.Color.Blue;
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance5;
            this.ultraLabel6.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel6.Location = new System.Drawing.Point(15, 151);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(87, 23);
            this.ultraLabel6.TabIndex = 460;
            this.ultraLabel6.Text = "Quick Search :";
            this.ultraLabel6.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // txtKeywordSearch
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.FontData.Name = "Calibri";
            appearance6.FontData.SizeInPoints = 11F;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.txtKeywordSearch.Appearance = appearance6;
            this.txtKeywordSearch.AutoSize = false;
            this.txtKeywordSearch.BackColor = System.Drawing.Color.White;
            this.txtKeywordSearch.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtKeywordSearch.Font = new System.Drawing.Font("Calibri", 11F);
            this.txtKeywordSearch.Format = "";
            this.txtKeywordSearch.IsDirty = false;
            this.txtKeywordSearch.IsEmailTextBox = false;
            this.txtKeywordSearch.Location = new System.Drawing.Point(103, 151);
            this.txtKeywordSearch.Multiline = true;
            this.txtKeywordSearch.Name = "txtKeywordSearch";
            this.txtKeywordSearch.NullText = "Type here to search for the keyword(s).";
            appearance7.FontData.ItalicAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.Silver;
            this.txtKeywordSearch.NullTextAppearance = appearance7;
            this.txtKeywordSearch.Size = new System.Drawing.Size(252, 25);
            this.txtKeywordSearch.TabIndex = 459;
            this.txtKeywordSearch.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.txtKeywordSearch.TextChanged += new System.EventHandler(this.txtKeywordSearch_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.lblTotalRows);
            this.panel1.Controls.Add(this.lblAll);
            this.panel1.Controls.Add(this.lblOrange);
            this.panel1.Controls.Add(this.lblBlue);
            this.panel1.Controls.Add(this.lblRed);
            this.panel1.Controls.Add(this.lblGreen);
            this.panel1.Location = new System.Drawing.Point(12, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1368, 49);
            this.panel1.TabIndex = 462;
            // 
            // lblTotalRows
            // 
            this.lblTotalRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance13.ForeColor = System.Drawing.Color.Blue;
            appearance13.TextHAlignAsString = "Right";
            appearance13.TextVAlignAsString = "Middle";
            this.lblTotalRows.Appearance = appearance13;
            this.lblTotalRows.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblTotalRows.Location = new System.Drawing.Point(1267, 16);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(95, 23);
            this.lblTotalRows.TabIndex = 464;
            this.lblTotalRows.Text = "Total: 00,000";
            this.lblTotalRows.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblAll
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.Image = global::WinUI.Properties.Resources.green_border_161;
            appearance2.TextVAlignAsString = "Middle";
            this.lblAll.Appearance = appearance2;
            this.lblAll.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblAll.Location = new System.Drawing.Point(391, 16);
            this.lblAll.Name = "lblAll";
            this.lblAll.Size = new System.Drawing.Size(161, 23);
            this.lblAll.TabIndex = 468;
            this.lblAll.Tag = "stock";
            this.lblAll.Text = "All Active Stock (00,000)";
            this.lblAll.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.lblAll.Click += new System.EventHandler(this.lblColorIndicator_Click);
            // 
            // lblOrange
            // 
            appearance10.ForeColor = System.Drawing.Color.Black;
            appearance10.Image = global::WinUI.Properties.Resources.orange_16;
            appearance10.TextVAlignAsString = "Middle";
            this.lblOrange.Appearance = appearance10;
            this.lblOrange.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblOrange.Location = new System.Drawing.Point(862, 16);
            this.lblOrange.Name = "lblOrange";
            this.lblOrange.Size = new System.Drawing.Size(121, 23);
            this.lblOrange.TabIndex = 466;
            this.lblOrange.Tag = "orange";
            this.lblOrange.Text = "In Progess (000)";
            this.lblOrange.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.lblOrange.Click += new System.EventHandler(this.lblColorIndicator_Click);
            // 
            // lblBlue
            // 
            appearance11.ForeColor = System.Drawing.Color.Black;
            appearance11.Image = global::WinUI.Properties.Resources.light_blue_16;
            appearance11.TextVAlignAsString = "Middle";
            this.lblBlue.Appearance = appearance11;
            this.lblBlue.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblBlue.Location = new System.Drawing.Point(991, 16);
            this.lblBlue.Name = "lblBlue";
            this.lblBlue.Size = new System.Drawing.Size(169, 23);
            this.lblBlue.TabIndex = 467;
            this.lblBlue.Tag = "blue";
            this.lblBlue.Text = "Kitted Components (000)";
            this.lblBlue.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.lblBlue.Click += new System.EventHandler(this.lblColorIndicator_Click);
            // 
            // lblRed
            // 
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.Image = global::WinUI.Properties.Resources.red_16;
            appearance12.TextVAlignAsString = "Middle";
            this.lblRed.Appearance = appearance12;
            this.lblRed.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblRed.Location = new System.Drawing.Point(711, 16);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(140, 23);
            this.lblRed.TabIndex = 464;
            this.lblRed.Tag = "red";
            this.lblRed.Text = "Discrepancy (000)";
            this.lblRed.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.lblRed.Click += new System.EventHandler(this.lblColorIndicator_Click);
            // 
            // lblGreen
            // 
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.Image = global::WinUI.Properties.Resources.green_16;
            appearance9.TextVAlignAsString = "Middle";
            this.lblGreen.Appearance = appearance9;
            this.lblGreen.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblGreen.Location = new System.Drawing.Point(566, 16);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(138, 23);
            this.lblGreen.TabIndex = 465;
            this.lblGreen.Tag = "green";
            this.lblGreen.Text = "Matched (00,000)";
            this.lblGreen.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.lblGreen.Click += new System.EventHandler(this.lblColorIndicator_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 86);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 46);
            this.pictureBox1.TabIndex = 463;
            this.pictureBox1.TabStop = false;
            // 
            // frmMSTItmInvBal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1392, 647);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.ultraLabel6);
            this.Controls.Add(this.txtKeywordSearch);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ultraLabel26);
            this.Controls.Add(this.tagrdList);
            this.Controls.Add(this.tspBar);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMSTItmInvBal";
            this.Text = "Inventory Cross-Checking";
            this.Load += new System.EventHandler(this.frmList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmList_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDocList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordSearch)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripLabel tslConType;
        private System.Windows.Forms.ToolStripButton tsbExport;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.BindingSource bdsDocList;
        private TAUtil.TAGridEditor tagrdList;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel26;
        private Infragistics.Win.Misc.UltraButton btnClearFilter;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private TAUtil.TATextBoxEditor txtKeywordSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraLabel lblAll;
        private Infragistics.Win.Misc.UltraLabel lblBlue;
        private Infragistics.Win.Misc.UltraLabel lblOrange;
        private Infragistics.Win.Misc.UltraLabel lblGreen;
        private Infragistics.Win.Misc.UltraLabel lblRed;
        private Infragistics.Win.Misc.UltraLabel lblTotalRows;
        private System.Windows.Forms.ToolStripButton tsbClearFilter;
    }
}