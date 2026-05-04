namespace WinUI
{
    partial class frmDocList
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
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
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbItemList = new System.Windows.Forms.ToolStripButton();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pDateTo = new TAUtil.TADateEditor();
            this.pDateFrom = new TAUtil.TADateEditor();
            this.pDocType = new TAUtil.TAComboBox();
            this.tagrdDocList = new TAUtil.TAGridEditor();
            this.bdsDocList = new System.Windows.Forms.BindingSource(this.components);
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDateTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDateFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDocType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDocList)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbDelete,
            this.tsbEdit,
            this.tsbExport,
            this.tsbItemList,
            this.tsbPrint,
            this.tsbRefresh});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(792, 73);
            this.tspBar.TabIndex = 5;
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
            // tsbDelete
            // 
            this.tsbDelete.AutoSize = false;
            this.tsbDelete.BackColor = System.Drawing.Color.Transparent;
            this.tsbDelete.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbDelete.Image = global::WinUI.Properties.Resources.deleteaaa;
            this.tsbDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbDelete.RightToLeftAutoMirrorImage = true;
            this.tsbDelete.Size = new System.Drawing.Size(65, 55);
            this.tsbDelete.Text = "&Delete";
            this.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.AutoSize = false;
            this.tsbEdit.BackColor = System.Drawing.Color.Transparent;
            this.tsbEdit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbEdit.Image = global::WinUI.Properties.Resources.edit;
            this.tsbEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbEdit.RightToLeftAutoMirrorImage = true;
            this.tsbEdit.Size = new System.Drawing.Size(65, 55);
            this.tsbEdit.Text = "&Edit";
            this.tsbEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
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
            // tsbItemList
            // 
            this.tsbItemList.AutoSize = false;
            this.tsbItemList.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbItemList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbItemList.Image = global::WinUI.Properties.Resources.list;
            this.tsbItemList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbItemList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbItemList.Name = "tsbItemList";
            this.tsbItemList.Size = new System.Drawing.Size(65, 55);
            this.tsbItemList.Text = "Item List";
            this.tsbItemList.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbItemList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbItemList.Click += new System.EventHandler(this.tsbItemList_Click);
            // 
            // tsbPrint
            // 
            this.tsbPrint.AutoSize = false;
            this.tsbPrint.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbPrint.Image = global::WinUI.Properties.Resources.print;
            this.tsbPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(65, 55);
            this.tsbPrint.Text = "&Print";
            this.tsbPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
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
            // ultraLabel2
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance3;
            this.ultraLabel2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel2.Location = new System.Drawing.Point(12, 88);
            this.ultraLabel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(118, 22);
            this.ultraLabel2.TabIndex = 121;
            this.ultraLabel2.Text = "Document Type";
            this.ultraLabel2.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel1
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance5;
            this.ultraLabel1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel1.Location = new System.Drawing.Point(331, 88);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(39, 22);
            this.ultraLabel1.TabIndex = 123;
            this.ultraLabel1.Text = "From";
            this.ultraLabel1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel3
            // 
            appearance78.BackColor = System.Drawing.Color.Transparent;
            appearance78.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance78.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance78;
            this.ultraLabel3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel3.Location = new System.Drawing.Point(525, 88);
            this.ultraLabel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(34, 22);
            this.ultraLabel3.TabIndex = 125;
            this.ultraLabel3.Text = "To";
            this.ultraLabel3.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // pDateTo
            // 
            appearance9.TextHAlignAsString = "Right";
            this.pDateTo.Appearance = appearance9;
            appearance6.Image = global::WinUI.Properties.Resources.calendar3;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance6;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.pDateTo.ButtonsRight.Add(editorButton1);
            this.pDateTo.calendarContainer = null;
            this.pDateTo.DateValue = null;
            this.pDateTo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.pDateTo.Font = new System.Drawing.Font("Calibri", 11F);
            this.pDateTo.Format = "";
            this.pDateTo.Location = new System.Drawing.Point(559, 88);
            this.pDateTo.MaxLength = 20;
            this.pDateTo.Name = "pDateTo";
            this.pDateTo.Size = new System.Drawing.Size(128, 26);
            this.pDateTo.TabIndex = 2;
            this.pDateTo.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.pDateTo.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.pDate_CustomUpdate);
            this.pDateTo.CustomDataError += new TAUtil.TADataErrorEventHandler(this.pDate_OnDataError);
            // 
            // pDateFrom
            // 
            appearance10.TextHAlignAsString = "Right";
            this.pDateFrom.Appearance = appearance10;
            appearance4.Image = global::WinUI.Properties.Resources.calendar3;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton2.Appearance = appearance4;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.pDateFrom.ButtonsRight.Add(editorButton2);
            this.pDateFrom.calendarContainer = null;
            this.pDateFrom.DateValue = null;
            this.pDateFrom.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.pDateFrom.Font = new System.Drawing.Font("Calibri", 11F);
            this.pDateFrom.Format = "";
            this.pDateFrom.Location = new System.Drawing.Point(376, 88);
            this.pDateFrom.MaxLength = 20;
            this.pDateFrom.Name = "pDateFrom";
            this.pDateFrom.Size = new System.Drawing.Size(128, 26);
            this.pDateFrom.TabIndex = 1;
            this.pDateFrom.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.pDateFrom.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.pDate_CustomUpdate);
            this.pDateFrom.CustomDataError += new TAUtil.TADataErrorEventHandler(this.pDate_OnDataError);
            // 
            // pDocType
            // 
            appearance30.BackColor = System.Drawing.Color.White;
            this.pDocType.Appearance = appearance30;
            this.pDocType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.pDocType.ComboIsDirty = false;
            this.pDocType.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.pDocType.Font = new System.Drawing.Font("Calibri", 11F);
            this.pDocType.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.pDocType.Location = new System.Drawing.Point(136, 88);
            this.pDocType.Name = "pDocType";
            this.pDocType.Size = new System.Drawing.Size(180, 26);
            this.pDocType.TabIndex = 0;
            this.pDocType.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.pDocType.UserInputText = "";
            this.pDocType.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.pDocType_CustomUpdate);
            // 
            // tagrdDocList
            // 
            this.tagrdDocList.ActiveConnection = null;
            this.tagrdDocList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdDocList.AutoAddNewRow = false;
            this.tagrdDocList.AutoUseCustomControlsInCells = false;
            this.tagrdDocList.DataSource = this.bdsDocList;
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
            this.tagrdDocList.Location = new System.Drawing.Point(12, 131);
            this.tagrdDocList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdDocList.Name = "tagrdDocList";
            this.tagrdDocList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDocList.Size = new System.Drawing.Size(768, 372);
            this.tagrdDocList.TabIndex = 4;
            this.tagrdDocList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDocList.AfterRowActivate += new System.EventHandler(this.tagrdDocList_AfterRowActivate);
            this.tagrdDocList.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.tagrdDocList_ClickCellButton);
            this.tagrdDocList.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.tagrdDocList_DoubleClickRow);
            // 
            // frmDocList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(792, 516);
            this.Controls.Add(this.tagrdDocList);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.pDateTo);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.pDateFrom);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.pDocType);
            this.Controls.Add(this.tspBar);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDocList";
            this.Text = "Document List";
            this.Activated += new System.EventHandler(this.frm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_FormClosing);
            this.Load += new System.EventHandler(this.frm_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocList_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDateTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDateFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDocType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDocList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private TAUtil.TAComboBox pDocType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private TAUtil.TADateEditor pDateFrom;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private TAUtil.TADateEditor pDateTo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolStripButton tsbItemList;
        private TAUtil.TAGridEditor tagrdDocList;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.BindingSource bdsDocList;
        private System.Windows.Forms.ToolStripButton tsbRefresh;

    }
}