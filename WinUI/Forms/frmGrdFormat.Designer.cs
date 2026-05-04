namespace WinUI
{
    partial class frmGrdFormat
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
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("BindingList`1", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Show", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Caption", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Key", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Format", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Width", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("GrdColumnKey", 5);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance675 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance660 = new Infragistics.Win.Appearance();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbSelectAll = new System.Windows.Forms.ToolStripButton();
            this.tsbUnselectAll = new System.Windows.Forms.ToolStripButton();
            this.tsbSelectHighlighted = new System.Windows.Forms.ToolStripButton();
            this.tsbUnselectHighlighted = new System.Windows.Forms.ToolStripButton();
            this.tsbDisplayShownColumn = new System.Windows.Forms.ToolStripButton();
            this.tsbDisplayAll = new System.Windows.Forms.ToolStripButton();
            this.bdsFormatGrdData = new System.Windows.Forms.BindingSource(this.components);
            this.tagrdFormatGrid = new TAUtil.TAGridEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.GridRowHeightCM = new TAUtil.TANumericEditor();
            this.ultraLabel76 = new Infragistics.Win.Misc.UltraLabel();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsFormatGrdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdFormatGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridRowHeightCM)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbSave,
            this.tsbSelectAll,
            this.tsbUnselectAll,
            this.tsbSelectHighlighted,
            this.tsbUnselectHighlighted,
            this.tsbDisplayShownColumn,
            this.tsbDisplayAll});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(709, 66);
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
            this.tsbSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbSelectAll
            // 
            this.tsbSelectAll.AutoSize = false;
            this.tsbSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.tsbSelectAll.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbSelectAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbSelectAll.Image = global::WinUI.Properties.Resources.select_plane_ok_128;
            this.tsbSelectAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelectAll.Name = "tsbSelectAll";
            this.tsbSelectAll.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbSelectAll.RightToLeftAutoMirrorImage = true;
            this.tsbSelectAll.Size = new System.Drawing.Size(70, 55);
            this.tsbSelectAll.Text = "Select All";
            this.tsbSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSelectAll.Click += new System.EventHandler(this.tsbSelectAll_Click);
            // 
            // tsbUnselectAll
            // 
            this.tsbUnselectAll.AutoSize = false;
            this.tsbUnselectAll.BackColor = System.Drawing.Color.Transparent;
            this.tsbUnselectAll.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbUnselectAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbUnselectAll.Image = global::WinUI.Properties.Resources.select_plane_close_128;
            this.tsbUnselectAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbUnselectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUnselectAll.Name = "tsbUnselectAll";
            this.tsbUnselectAll.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbUnselectAll.RightToLeftAutoMirrorImage = true;
            this.tsbUnselectAll.Size = new System.Drawing.Size(70, 55);
            this.tsbUnselectAll.Text = "Select None";
            this.tsbUnselectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbUnselectAll.Click += new System.EventHandler(this.tsbUnselectAll_Click);
            // 
            // tsbSelectHighlighted
            // 
            this.tsbSelectHighlighted.AutoSize = false;
            this.tsbSelectHighlighted.BackColor = System.Drawing.Color.Transparent;
            this.tsbSelectHighlighted.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbSelectHighlighted.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbSelectHighlighted.Image = global::WinUI.Properties.Resources.select_plane_next_128;
            this.tsbSelectHighlighted.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSelectHighlighted.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelectHighlighted.Name = "tsbSelectHighlighted";
            this.tsbSelectHighlighted.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbSelectHighlighted.RightToLeftAutoMirrorImage = true;
            this.tsbSelectHighlighted.Size = new System.Drawing.Size(120, 55);
            this.tsbSelectHighlighted.Text = "Select HighLighted";
            this.tsbSelectHighlighted.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSelectHighlighted.Click += new System.EventHandler(this.tsbSelectHighlighted_Click);
            // 
            // tsbUnselectHighlighted
            // 
            this.tsbUnselectHighlighted.AutoSize = false;
            this.tsbUnselectHighlighted.BackColor = System.Drawing.Color.Transparent;
            this.tsbUnselectHighlighted.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbUnselectHighlighted.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbUnselectHighlighted.Image = global::WinUI.Properties.Resources.select_plane_delete_128;
            this.tsbUnselectHighlighted.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbUnselectHighlighted.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUnselectHighlighted.Name = "tsbUnselectHighlighted";
            this.tsbUnselectHighlighted.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbUnselectHighlighted.RightToLeftAutoMirrorImage = true;
            this.tsbUnselectHighlighted.Size = new System.Drawing.Size(140, 55);
            this.tsbUnselectHighlighted.Text = "select None HighLighted";
            this.tsbUnselectHighlighted.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbUnselectHighlighted.Click += new System.EventHandler(this.tsbUnselectHighlighted_Click);
            // 
            // tsbDisplayShownColumn
            // 
            this.tsbDisplayShownColumn.AutoSize = false;
            this.tsbDisplayShownColumn.BackColor = System.Drawing.Color.Transparent;
            this.tsbDisplayShownColumn.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbDisplayShownColumn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbDisplayShownColumn.Image = global::WinUI.Properties.Resources.show__wireframe_128;
            this.tsbDisplayShownColumn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDisplayShownColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDisplayShownColumn.Name = "tsbDisplayShownColumn";
            this.tsbDisplayShownColumn.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbDisplayShownColumn.RightToLeftAutoMirrorImage = true;
            this.tsbDisplayShownColumn.Size = new System.Drawing.Size(140, 55);
            this.tsbDisplayShownColumn.Text = "Display Shown Column";
            this.tsbDisplayShownColumn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDisplayShownColumn.Click += new System.EventHandler(this.tsbDisplayShownColumn_Click);
            // 
            // tsbDisplayAll
            // 
            this.tsbDisplayAll.AutoSize = false;
            this.tsbDisplayAll.BackColor = System.Drawing.Color.Transparent;
            this.tsbDisplayAll.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbDisplayAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbDisplayAll.Image = global::WinUI.Properties.Resources.application_list_32;
            this.tsbDisplayAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDisplayAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDisplayAll.Name = "tsbDisplayAll";
            this.tsbDisplayAll.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbDisplayAll.RightToLeftAutoMirrorImage = true;
            this.tsbDisplayAll.Size = new System.Drawing.Size(70, 55);
            this.tsbDisplayAll.Text = "Display All";
            this.tsbDisplayAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDisplayAll.Click += new System.EventHandler(this.tsbDisplayAll_Click);
            // 
            // bdsFormatGrdData
            // 
            this.bdsFormatGrdData.AllowNew = false;
            // 
            // tagrdFormatGrid
            // 
            this.tagrdFormatGrid.ActiveConnection = null;
            this.tagrdFormatGrid.AllowDrop = true;
            this.tagrdFormatGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdFormatGrid.AutoAddNewRow = false;
            this.tagrdFormatGrid.AutoUseCustomControlsInCells = false;
            this.tagrdFormatGrid.DataSource = this.bdsFormatGrdData;
            this.tagrdFormatGrid.DefaultValue = null;
            this.tagrdFormatGrid.DetailObjectKey = 0;
            appearance31.AlphaLevel = ((short)(255));
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance31.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance31.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance31.FontData.BoldAsString = "False";
            appearance31.FontData.Name = "Tahoma";
            appearance31.FontData.SizeInPoints = 10.5F;
            this.tagrdFormatGrid.DisplayLayout.Appearance = appearance31;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.DataType = typeof(bool);
            ultraGridColumn1.DefaultCellValue = false;
            ultraGridColumn1.Header.Caption = "Show Column";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn2.Header.Caption = "ColHeader Caption";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn4.DataType = typeof(int);
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn5.Format = "#,##0.00";
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            appearance1.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance2;
            ultraGridBand1.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            ultraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            ultraGridBand1.Override.RowSelectorWidth = 30;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdFormatGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdFormatGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance34.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance34.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdFormatGrid.DisplayLayout.GroupByBox.Appearance = appearance34;
            appearance35.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdFormatGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance35;
            this.tagrdFormatGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance36.BackColor2 = System.Drawing.SystemColors.Control;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance36.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdFormatGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance36;
            this.tagrdFormatGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdFormatGrid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance37.ForeColor = System.Drawing.Color.Black;
            this.tagrdFormatGrid.DisplayLayout.Override.ActiveCellAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.BlanchedAlmond;
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            appearance38.ForeColor = System.Drawing.Color.Black;
            this.tagrdFormatGrid.DisplayLayout.Override.ActiveRowAppearance = appearance38;
            this.tagrdFormatGrid.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdFormatGrid.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.tagrdFormatGrid.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance39.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdFormatGrid.DisplayLayout.Override.CardAreaAppearance = appearance39;
            appearance40.BorderColor = System.Drawing.Color.Silver;
            appearance40.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdFormatGrid.DisplayLayout.Override.CellAppearance = appearance40;
            this.tagrdFormatGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdFormatGrid.DisplayLayout.Override.CellPadding = 0;
            this.tagrdFormatGrid.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdFormatGrid.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance41.AlphaLevel = ((short)(255));
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance41.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance41.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance41.FontData.BoldAsString = "True";
            appearance41.FontData.Name = "Tahoma";
            appearance41.ForeColor = System.Drawing.Color.Black;
            appearance41.TextHAlignAsString = "Left";
            this.tagrdFormatGrid.DisplayLayout.Override.HeaderAppearance = appearance41;
            this.tagrdFormatGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.tagrdFormatGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance42.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance42.ForeColor = System.Drawing.Color.Black;
            appearance42.TextVAlignAsString = "Middle";
            this.tagrdFormatGrid.DisplayLayout.Override.RowAppearance = appearance42;
            appearance43.AlphaLevel = ((short)(255));
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance43.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance43.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdFormatGrid.DisplayLayout.Override.RowSelectorAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance44.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance44.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdFormatGrid.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance44;
            this.tagrdFormatGrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdFormatGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdFormatGrid.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdFormatGrid.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance45.BackColor = System.Drawing.Color.Gold;
            appearance45.BackColor2 = System.Drawing.Color.White;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdFormatGrid.DisplayLayout.Override.SelectedRowAppearance = appearance45;
            this.tagrdFormatGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdFormatGrid.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdFormatGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdFormatGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdFormatGrid.DisplayLayout.UseFixedHeaders = true;
            this.tagrdFormatGrid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdFormatGrid.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdFormatGrid.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdFormatGrid.HeaderObjectKey = null;
            this.tagrdFormatGrid.Location = new System.Drawing.Point(0, 70);
            this.tagrdFormatGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdFormatGrid.Name = "tagrdFormatGrid";
            this.tagrdFormatGrid.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdFormatGrid.Size = new System.Drawing.Size(709, 428);
            this.tagrdFormatGrid.TabIndex = 15;
            this.tagrdFormatGrid.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdFormatGrid.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdFormatGrid.CustomDataError += new TAUtil.TADataErrorEventHandler(this.OnDataError);
            this.tagrdFormatGrid.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.tagrdFormatGrid_AfterCellUpdate);
            this.tagrdFormatGrid.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.tagrdFormatGrid_CellChange);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(683, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 22);
            this.label1.TabIndex = 16;
            this.label1.Text = "*";
            this.label1.DoubleClick += new System.EventHandler(this.tsbTest_Click);
            // 
            // GridRowHeightCM
            // 
            this.GridRowHeightCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance675.BorderColor = System.Drawing.Color.Transparent;
            appearance675.FontData.Name = "Calibri";
            appearance675.FontData.SizeInPoints = 11F;
            appearance675.ForeColor = System.Drawing.Color.Black;
            appearance675.TextHAlignAsString = "Right";
            this.GridRowHeightCM.Appearance = appearance675;
            this.GridRowHeightCM.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.GridRowHeightCM.Font = new System.Drawing.Font("Calibri", 11F);
            this.GridRowHeightCM.ForceExitByRestoreValue = false;
            this.GridRowHeightCM.Format = "";
            this.GridRowHeightCM.Location = new System.Drawing.Point(132, 498);
            this.GridRowHeightCM.MaxLength = 19;
            this.GridRowHeightCM.Name = "GridRowHeightCM";
            this.GridRowHeightCM.NumberType = TAUtil.NumericType.Double;
            this.GridRowHeightCM.Size = new System.Drawing.Size(88, 26);
            this.GridRowHeightCM.TabIndex = 339;
            this.GridRowHeightCM.Text = "0.00";
            this.GridRowHeightCM.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.GridRowHeightCM.ZeroIfEmpty = false;
            // 
            // ultraLabel76
            // 
            this.ultraLabel76.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance660.BackColor = System.Drawing.Color.Transparent;
            appearance660.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance660.TextHAlignAsString = "Left";
            appearance660.TextVAlignAsString = "Middle";
            this.ultraLabel76.Appearance = appearance660;
            this.ultraLabel76.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ultraLabel76.Location = new System.Drawing.Point(0, 498);
            this.ultraLabel76.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel76.Name = "ultraLabel76";
            this.ultraLabel76.Size = new System.Drawing.Size(126, 28);
            this.ultraLabel76.TabIndex = 340;
            this.ultraLabel76.Text = "Grid Row Height";
            this.ultraLabel76.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // frmGrdFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.ClientSize = new System.Drawing.Size(709, 525);
            this.Controls.Add(this.GridRowHeightCM);
            this.Controls.Add(this.ultraLabel76);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tagrdFormatGrid);
            this.Controls.Add(this.tspBar);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmGrdFormat";
            this.Text = "Format Grid";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGrdFormat_FormClosing);
            this.Load += new System.EventHandler(this.frmGrdFormat_Load);
            this.Shown += new System.EventHandler(this.frmGrdFormat_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGrdFormat_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsFormatGrdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdFormatGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridRowHeightCM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.BindingSource bdsFormatGrdData;
        private TAUtil.TAGridEditor tagrdFormatGrid;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbUnselectAll;
        private System.Windows.Forms.ToolStripButton tsbUnselectHighlighted;
        private System.Windows.Forms.ToolStripButton tsbDisplayShownColumn;
        private System.Windows.Forms.ToolStripButton tsbSelectAll;
        private System.Windows.Forms.ToolStripButton tsbSelectHighlighted;
        private System.Windows.Forms.ToolStripButton tsbDisplayAll;
        private System.Windows.Forms.Label label1;
        private TAUtil.TANumericEditor GridRowHeightCM;
        private Infragistics.Win.Misc.UltraLabel ultraLabel76;

    }
}