namespace WinUI
{
    partial class frmDocSearch
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("BindingList`1", -1);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbSelect = new System.Windows.Forms.ToolStripButton();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.SearchType = new TAUtil.TAComboBox();
            this.SearchText = new TAUtil.TATextBoxEditor();
            this.ToDate = new TAUtil.TADateEditor();
            this.FromDate = new TAUtil.TADateEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblSearchType = new Infragistics.Win.Misc.UltraLabel();
            this.lblSearchText = new Infragistics.Win.Misc.UltraLabel();
            this.tagrdPopups = new TAUtil.TAGridEditor();
            this.tspBar.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SearchType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdPopups)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbSelect});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(839, 70);
            this.tspBar.TabIndex = 0;
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
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
            // tsbSelect
            // 
            this.tsbSelect.AutoSize = false;
            this.tsbSelect.BackColor = System.Drawing.Color.Transparent;
            this.tsbSelect.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbSelect.Image = global::WinUI.Properties.Resources.select;
            this.tsbSelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbSelect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelect.Name = "tsbSelect";
            this.tsbSelect.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbSelect.RightToLeftAutoMirrorImage = true;
            this.tsbSelect.Size = new System.Drawing.Size(60, 55);
            this.tsbSelect.Text = "&Select";
            this.tsbSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSelect.Click += new System.EventHandler(this.tsbSelect_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panelSearch.Controls.Add(this.SearchType);
            this.panelSearch.Controls.Add(this.SearchText);
            this.panelSearch.Controls.Add(this.ToDate);
            this.panelSearch.Controls.Add(this.FromDate);
            this.panelSearch.Controls.Add(this.ultraLabel4);
            this.panelSearch.Controls.Add(this.ultraLabel3);
            this.panelSearch.Controls.Add(this.lblSearchType);
            this.panelSearch.Controls.Add(this.lblSearchText);
            this.panelSearch.Location = new System.Drawing.Point(12, 78);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(815, 85);
            this.panelSearch.TabIndex = 1;
            // 
            // SearchType
            // 
            appearance1.BorderColor = System.Drawing.Color.LightGray;
            this.SearchType.Appearance = appearance1;
            this.SearchType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.SearchType.AutoSize = false;
            this.SearchType.ComboIsDirty = false;
            this.SearchType.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.SearchType.Font = new System.Drawing.Font("Calibri", 11F);
            this.SearchType.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.SearchType.LimitToList = true;
            this.SearchType.Location = new System.Drawing.Point(497, 12);
            this.SearchType.Name = "SearchType";
            this.SearchType.Size = new System.Drawing.Size(300, 25);
            this.SearchType.TabIndex = 2;
            this.SearchType.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.SearchType.UserInputText = "";
            this.SearchType.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            this.SearchType.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.SearchType_CustomUpdate);
            // 
            // SearchText
            // 
            appearance47.BackColor = System.Drawing.Color.White;
            appearance47.BorderColor = System.Drawing.Color.LightGray;
            this.SearchText.Appearance = appearance47;
            this.SearchText.AutoSize = false;
            this.SearchText.BackColor = System.Drawing.Color.White;
            this.SearchText.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.SearchText.Font = new System.Drawing.Font("Calibri", 11F);
            this.SearchText.Format = "";
            this.SearchText.IsDirty = false;
            this.SearchText.IsEmailTextBox = false;
            this.SearchText.Location = new System.Drawing.Point(497, 37);
            this.SearchText.Multiline = true;
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(300, 25);
            this.SearchText.TabIndex = 3;
            this.SearchText.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.SearchText.TextChanged += new System.EventHandler(this.SearchText_TextChanged);
            // 
            // ToDate
            // 
            appearance148.BackColor = System.Drawing.Color.White;
            appearance148.BorderColor = System.Drawing.Color.LightGray;
            appearance148.TextHAlignAsString = "Right";
            this.ToDate.Appearance = appearance148;
            this.ToDate.AutoSize = false;
            this.ToDate.BackColor = System.Drawing.Color.White;
            appearance149.Image = global::WinUI.Properties.Resources.calendar3;
            appearance149.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance149;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.ToDate.ButtonsRight.Add(editorButton1);
            this.ToDate.calendarContainer = null;
            this.ToDate.DateValue = null;
            this.ToDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ToDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.ToDate.Format = "";
            this.ToDate.Location = new System.Drawing.Point(100, 37);
            this.ToDate.MaxLength = 20;
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new System.Drawing.Size(300, 25);
            this.ToDate.TabIndex = 1;
            this.ToDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ToDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.ToDate_CustomUpdate);
            // 
            // FromDate
            // 
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BorderColor = System.Drawing.Color.LightGray;
            appearance17.TextHAlignAsString = "Right";
            this.FromDate.Appearance = appearance17;
            this.FromDate.AutoSize = false;
            this.FromDate.BackColor = System.Drawing.Color.White;
            appearance18.Image = global::WinUI.Properties.Resources.calendar3;
            appearance18.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton2.Appearance = appearance18;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.FromDate.ButtonsRight.Add(editorButton2);
            this.FromDate.calendarContainer = null;
            this.FromDate.DateValue = null;
            this.FromDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.FromDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.FromDate.Format = "";
            this.FromDate.Location = new System.Drawing.Point(100, 12);
            this.FromDate.MaxLength = 20;
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(300, 25);
            this.FromDate.TabIndex = 0;
            this.FromDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.FromDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.FromDate_CustomUpdate);
            // 
            // ultraLabel4
            // 
            appearance5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance5;
            this.ultraLabel4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel4.Location = new System.Drawing.Point(13, 36);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(69, 25);
            this.ultraLabel4.TabIndex = 2;
            this.ultraLabel4.Text = "Date To";
            this.ultraLabel4.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel3
            // 
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance3;
            this.ultraLabel3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel3.Location = new System.Drawing.Point(13, 12);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(71, 25);
            this.ultraLabel3.TabIndex = 0;
            this.ultraLabel3.Text = "Date From";
            this.ultraLabel3.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblSearchType
            // 
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance2.TextVAlignAsString = "Middle";
            this.lblSearchType.Appearance = appearance2;
            this.lblSearchType.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblSearchType.Location = new System.Drawing.Point(410, 12);
            this.lblSearchType.Name = "lblSearchType";
            this.lblSearchType.Size = new System.Drawing.Size(86, 25);
            this.lblSearchType.TabIndex = 4;
            this.lblSearchType.Text = "Search Type";
            this.lblSearchType.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblSearchText
            // 
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.TextVAlignAsString = "Middle";
            this.lblSearchText.Appearance = appearance4;
            this.lblSearchText.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblSearchText.Location = new System.Drawing.Point(410, 37);
            this.lblSearchText.Name = "lblSearchText";
            this.lblSearchText.Size = new System.Drawing.Size(81, 25);
            this.lblSearchText.TabIndex = 6;
            this.lblSearchText.Text = "Search Text";
            this.lblSearchText.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // tagrdPopups
            // 
            this.tagrdPopups.AllowDrop = true;
            this.tagrdPopups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdPopups.AutoAddNewRow = false;
            this.tagrdPopups.AutoUseCustomControlsInCells = false;
            this.tagrdPopups.DefaultValue = null;
            this.tagrdPopups.DetailObjectKey = 0;
            appearance7.AlphaLevel = ((short)(255));
            appearance7.BackColor = System.Drawing.Color.AliceBlue;
            appearance7.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance7.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance7.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdPopups.DisplayLayout.Appearance = appearance7;
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            appearance8.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance8;
            appearance26.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance26;
            ultraGridBand1.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            ultraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            ultraGridBand1.Override.RowSelectorWidth = 30;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdPopups.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdPopups.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdPopups.DisplayLayout.GroupByBox.Appearance = appearance27;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdPopups.DisplayLayout.GroupByBox.BandLabelAppearance = appearance28;
            this.tagrdPopups.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance29.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance29.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdPopups.DisplayLayout.GroupByBox.PromptAppearance = appearance29;
            this.tagrdPopups.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdPopups.DisplayLayout.MaxRowScrollRegions = 1;
            appearance30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdPopups.DisplayLayout.Override.ActiveCellAppearance = appearance30;
            appearance33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdPopups.DisplayLayout.Override.ActiveRowAppearance = appearance33;
            this.tagrdPopups.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdPopups.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.tagrdPopups.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdPopups.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdPopups.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance34.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdPopups.DisplayLayout.Override.CardAreaAppearance = appearance34;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            appearance35.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdPopups.DisplayLayout.Override.CellAppearance = appearance35;
            this.tagrdPopups.DisplayLayout.Override.CellPadding = 0;
            this.tagrdPopups.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdPopups.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance36.AlphaLevel = ((short)(255));
            appearance36.BackColor = System.Drawing.Color.AliceBlue;
            appearance36.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance36.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance36.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance36.ForeColor = System.Drawing.Color.Black;
            appearance36.TextHAlignAsString = "Left";
            this.tagrdPopups.DisplayLayout.Override.HeaderAppearance = appearance36;
            this.tagrdPopups.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdPopups.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.White;
            appearance37.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance37.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance37.ForeColor = System.Drawing.Color.Black;
            appearance37.TextVAlignAsString = "Middle";
            this.tagrdPopups.DisplayLayout.Override.RowAppearance = appearance37;
            appearance38.AlphaLevel = ((short)(255));
            appearance38.BackColor = System.Drawing.Color.AliceBlue;
            appearance38.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance38.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance38.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdPopups.DisplayLayout.Override.RowSelectorAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.AliceBlue;
            appearance39.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance39.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance39.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdPopups.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance39;
            this.tagrdPopups.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdPopups.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdPopups.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdPopups.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance40.BackColor = System.Drawing.Color.Gold;
            appearance40.BackColor2 = System.Drawing.Color.White;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdPopups.DisplayLayout.Override.SelectedRowAppearance = appearance40;
            this.tagrdPopups.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdPopups.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.tagrdPopups.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdPopups.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdPopups.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdPopups.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdPopups.DisplayLayout.UseFixedHeaders = true;
            this.tagrdPopups.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdPopups.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdPopups.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdPopups.HeaderObjectKey = null;
            this.tagrdPopups.Location = new System.Drawing.Point(12, 170);
            this.tagrdPopups.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdPopups.Name = "tagrdPopups";
            this.tagrdPopups.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdPopups.Size = new System.Drawing.Size(815, 334);
            this.tagrdPopups.TabIndex = 2;
            this.tagrdPopups.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdPopups.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdPopups.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdPopups_DoubleClickRow);
            // 
            // frmDocSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(839, 516);
            this.Controls.Add(this.tagrdPopups);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.tspBar);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDocSearch";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmDocSearch";
            this.Text = "Document Search";
            this.Load += new System.EventHandler(this.frmDocSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocSearch_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SearchType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdPopups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbSelect;
        private System.Windows.Forms.Panel panelSearch;
        private TAUtil.TAGridEditor tagrdPopups;
        private Infragistics.Win.Misc.UltraLabel lblSearchType;
        private Infragistics.Win.Misc.UltraLabel lblSearchText;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private TAUtil.TADateEditor ToDate;
        private TAUtil.TADateEditor FromDate;
        private TAUtil.TATextBoxEditor SearchText;
        private TAUtil.TAComboBox SearchType;

    }
}