namespace WinUI
{
    partial class frmMSTItmKittAssBal
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance228 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMSTItmKittAssBal));
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
            this.chkComponent = new TAUtil.TACheckBoxEditor();
            this.chkProgress = new TAUtil.TACheckBoxEditor();
            this.chkPreKitt = new TAUtil.TACheckBoxEditor();
            this.chkAll = new TAUtil.TACheckBoxEditor();
            this.lblTotalRows = new Infragistics.Win.Misc.UltraLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDocList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordSearch)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkComponent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkProgress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPreKitt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll)).BeginInit();
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
            appearance2.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance3;
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
            this.ultraLabel26.Text = "KITTING ASSEMBLY TRACKING";
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
            this.panel1.Controls.Add(this.chkComponent);
            this.panel1.Controls.Add(this.chkProgress);
            this.panel1.Controls.Add(this.chkPreKitt);
            this.panel1.Controls.Add(this.chkAll);
            this.panel1.Controls.Add(this.lblTotalRows);
            this.panel1.Location = new System.Drawing.Point(12, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1368, 49);
            this.panel1.TabIndex = 462;
            // 
            // chkComponent
            // 
            appearance228.BackColor = System.Drawing.Color.Transparent;
            appearance228.FontData.ItalicAsString = "True";
            appearance228.FontData.Name = "Calibri";
            appearance228.FontData.SizeInPoints = 10F;
            appearance228.ForeColor = System.Drawing.Color.MidnightBlue;
            this.chkComponent.Appearance = appearance228;
            this.chkComponent.BackColor = System.Drawing.Color.Transparent;
            this.chkComponent.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkComponent.cancelUpdate = false;
            this.chkComponent.Location = new System.Drawing.Point(973, 16);
            this.chkComponent.Name = "chkComponent";
            this.chkComponent.Size = new System.Drawing.Size(189, 21);
            this.chkComponent.TabIndex = 472;
            this.chkComponent.TabStop = false;
            this.chkComponent.Text = "Kitted Components (0,000)";
            this.chkComponent.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.toolTip.SetToolTip(this.chkComponent, "Click to review the component items which are already kitted for the finished ass" +
        "embly.");
            this.chkComponent.Visible = false;
            this.chkComponent.CheckedChanged += new System.EventHandler(this.chkComponent_CheckedChanged);
            // 
            // chkProgress
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.FontData.BoldAsString = "True";
            appearance8.FontData.ItalicAsString = "True";
            appearance8.FontData.Name = "Calibri";
            appearance8.FontData.SizeInPoints = 10F;
            appearance8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.chkProgress.Appearance = appearance8;
            this.chkProgress.BackColor = System.Drawing.Color.Transparent;
            this.chkProgress.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkProgress.cancelUpdate = false;
            this.chkProgress.Location = new System.Drawing.Point(789, 16);
            this.chkProgress.Name = "chkProgress";
            this.chkProgress.Size = new System.Drawing.Size(176, 21);
            this.chkProgress.TabIndex = 471;
            this.chkProgress.TabStop = false;
            this.chkProgress.Tag = "red";
            this.chkProgress.Text = "In Progress (000)";
            this.chkProgress.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.toolTip.SetToolTip(this.chkProgress, "Click to review the items which are requied for delivery.");
            this.chkProgress.CheckedValueChanged += new System.EventHandler(this.chkAssembly_CheckedChanged);
            // 
            // chkPreKitt
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.FontData.BoldAsString = "True";
            appearance12.FontData.ItalicAsString = "True";
            appearance12.FontData.Name = "Calibri";
            appearance12.FontData.SizeInPoints = 10F;
            appearance12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(175)))), ((int)(((byte)(0)))));
            this.chkPreKitt.Appearance = appearance12;
            this.chkPreKitt.BackColor = System.Drawing.Color.Transparent;
            this.chkPreKitt.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkPreKitt.cancelUpdate = false;
            this.chkPreKitt.Location = new System.Drawing.Point(590, 16);
            this.chkPreKitt.Name = "chkPreKitt";
            this.chkPreKitt.Size = new System.Drawing.Size(189, 21);
            this.chkPreKitt.TabIndex = 470;
            this.chkPreKitt.TabStop = false;
            this.chkPreKitt.Tag = "green";
            this.chkPreKitt.Text = "Pre-Kitted Assembly (0,000)";
            this.chkPreKitt.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.toolTip.SetToolTip(this.chkPreKitt, "Click to review the items which are ready to pick or ready for sales.");
            this.chkPreKitt.CheckedValueChanged += new System.EventHandler(this.chkAssembly_CheckedChanged);
            // 
            // chkAll
            // 
            appearance14.BackColor = System.Drawing.Color.Transparent;
            appearance14.FontData.BoldAsString = "False";
            appearance14.FontData.ItalicAsString = "True";
            appearance14.FontData.Name = "Calibri";
            appearance14.FontData.SizeInPoints = 10F;
            appearance14.ForeColor = System.Drawing.Color.Black;
            this.chkAll.Appearance = appearance14;
            this.chkAll.BackColor = System.Drawing.Color.Transparent;
            this.chkAll.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkAll.cancelUpdate = false;
            this.chkAll.Location = new System.Drawing.Point(400, 16);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(189, 21);
            this.chkAll.TabIndex = 469;
            this.chkAll.TabStop = false;
            this.chkAll.Text = "All Active Assembly (0,000)";
            this.chkAll.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.toolTip.SetToolTip(this.chkAll, "Click to review all available assembly type items.");
            this.chkAll.CheckedValueChanged += new System.EventHandler(this.chkAssembly_CheckedChanged);
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
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 86);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 46);
            this.pictureBox1.TabIndex = 463;
            this.pictureBox1.TabStop = false;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // frmMSTItmKittAssBal
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
            this.Name = "frmMSTItmKittAssBal";
            this.Text = "Kitting Assembly Tracking";
            this.Load += new System.EventHandler(this.frmList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmList_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDocList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordSearch)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkComponent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkProgress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPreKitt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll)).EndInit();
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
        private Infragistics.Win.Misc.UltraLabel lblTotalRows;
        private System.Windows.Forms.ToolStripButton tsbClearFilter;
        private System.Windows.Forms.ToolTip toolTip;
        private TAUtil.TACheckBoxEditor chkComponent;
        private TAUtil.TACheckBoxEditor chkProgress;
        private TAUtil.TACheckBoxEditor chkPreKitt;
        private TAUtil.TACheckBoxEditor chkAll;
    }
}