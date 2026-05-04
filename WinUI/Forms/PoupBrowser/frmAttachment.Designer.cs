namespace WinUI
{
    partial class frmAttachment
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
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("BindingList`1", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocDc", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocDk", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocDItm", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocDetailType", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Seq", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AttachPath", 5, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AttachFileType", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AttachDes", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AttachSize", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Custom1", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Custom2", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Custom3", 11);
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            this.tagrdAttachment = new TAUtil.TAGridEditor();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbAttach = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.gbEmail = new System.Windows.Forms.GroupBox();
            this.lblEmailSubj = new System.Windows.Forms.Label();
            this.txtEmailSubj = new TAUtil.TATextBoxEditor();
            this.lblLineEmailSubj = new System.Windows.Forms.Label();
            this.txtFromEmail = new TAUtil.TATextBoxEditor();
            this.chkSentStatus = new TAUtil.TACheckBoxEditor();
            this.txtToEmail = new TAUtil.TATextBoxEditor();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblLineTo = new System.Windows.Forms.Label();
            this.lblLineFrom = new System.Windows.Forms.Label();
            this.txtTo = new TAUtil.TATextBoxEditor();
            this.txtFrom = new TAUtil.TATextBoxEditor();
            this.chkZipFiles = new TAUtil.TACheckBoxEditor();
            this.flpnlAttachment = new System.Windows.Forms.FlowLayoutPanel();
            this.chkCustomerPO = new TAUtil.TACheckBoxEditor();
            this.chkMergePDF = new TAUtil.TACheckBoxEditor();
            this.btnDownload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdAttachment)).BeginInit();
            this.tspBar.SuspendLayout();
            this.gbEmail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailSubj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSentStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZipFiles)).BeginInit();
            this.flpnlAttachment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkCustomerPO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMergePDF)).BeginInit();
            this.SuspendLayout();
            // 
            // tagrdAttachment
            // 
            this.tagrdAttachment.ActiveConnection = null;
            this.tagrdAttachment.AllowDrop = true;
            this.tagrdAttachment.AutoAddNewRow = false;
            this.tagrdAttachment.AutoUseCustomControlsInCells = false;
            this.tagrdAttachment.DefaultValue = null;
            this.tagrdAttachment.DetailObjectKey = 0;
            appearance31.AlphaLevel = ((short)(255));
            appearance31.BackColor = System.Drawing.Color.AliceBlue;
            appearance31.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance31.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance31.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdAttachment.DisplayLayout.Appearance = appearance31;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12});
            appearance32.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance33;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdAttachment.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdAttachment.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance34.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance34.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdAttachment.DisplayLayout.GroupByBox.Appearance = appearance34;
            appearance35.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdAttachment.DisplayLayout.GroupByBox.BandLabelAppearance = appearance35;
            this.tagrdAttachment.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance36.BackColor2 = System.Drawing.SystemColors.Control;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance36.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdAttachment.DisplayLayout.GroupByBox.PromptAppearance = appearance36;
            this.tagrdAttachment.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdAttachment.DisplayLayout.MaxRowScrollRegions = 1;
            appearance37.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdAttachment.DisplayLayout.Override.ActiveCellAppearance = appearance37;
            appearance38.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdAttachment.DisplayLayout.Override.ActiveRowAppearance = appearance38;
            this.tagrdAttachment.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdAttachment.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.tagrdAttachment.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance39.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdAttachment.DisplayLayout.Override.CardAreaAppearance = appearance39;
            appearance40.BorderColor = System.Drawing.Color.Silver;
            appearance40.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdAttachment.DisplayLayout.Override.CellAppearance = appearance40;
            this.tagrdAttachment.DisplayLayout.Override.CellPadding = 0;
            this.tagrdAttachment.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdAttachment.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance41.AlphaLevel = ((short)(255));
            appearance41.BackColor = System.Drawing.Color.AliceBlue;
            appearance41.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance41.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance41.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance41.ForeColor = System.Drawing.Color.Black;
            appearance41.TextHAlignAsString = "Left";
            this.tagrdAttachment.DisplayLayout.Override.HeaderAppearance = appearance41;
            this.tagrdAttachment.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdAttachment.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance42.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance42.ForeColor = System.Drawing.Color.Black;
            appearance42.TextVAlignAsString = "Middle";
            this.tagrdAttachment.DisplayLayout.Override.RowAppearance = appearance42;
            appearance43.AlphaLevel = ((short)(255));
            appearance43.BackColor = System.Drawing.Color.AliceBlue;
            appearance43.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance43.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance43.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdAttachment.DisplayLayout.Override.RowSelectorAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.AliceBlue;
            appearance44.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance44.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance44.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdAttachment.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance44;
            this.tagrdAttachment.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdAttachment.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdAttachment.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdAttachment.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance45.BackColor = System.Drawing.Color.Gold;
            appearance45.BackColor2 = System.Drawing.Color.White;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdAttachment.DisplayLayout.Override.SelectedRowAppearance = appearance45;
            this.tagrdAttachment.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdAttachment.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.tagrdAttachment.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdAttachment.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdAttachment.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdAttachment.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdAttachment.DisplayLayout.UseFixedHeaders = true;
            this.tagrdAttachment.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdAttachment.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdAttachment.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdAttachment.HeaderObjectKey = null;
            this.tagrdAttachment.Location = new System.Drawing.Point(3, 167);
            this.tagrdAttachment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdAttachment.Name = "tagrdAttachment";
            this.tagrdAttachment.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdAttachment.Size = new System.Drawing.Size(812, 223);
            this.tagrdAttachment.TabIndex = 16;
            this.tagrdAttachment.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdAttachment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdAttachment.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.tagrdAttachment_ClickCellButton);
            this.tagrdAttachment.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.tagrdAttachment_DoubleClickRow);
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbDelete,
            this.tsbAttach,
            this.tsbCopy});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(822, 62);
            this.tspBar.TabIndex = 21;
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // tsbDelete
            // 
            this.tsbDelete.AutoSize = false;
            this.tsbDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tsbDelete.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbDelete.Image = global::WinUI.Properties.Resources.deleteaaa;
            this.tsbDelete.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(60, 55);
            this.tsbDelete.Text = "&Delete";
            this.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbAttach
            // 
            this.tsbAttach.AutoSize = false;
            this.tsbAttach.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tsbAttach.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbAttach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbAttach.Image = global::WinUI.Properties.Resources.attachment1;
            this.tsbAttach.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbAttach.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAttach.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAttach.Name = "tsbAttach";
            this.tsbAttach.Size = new System.Drawing.Size(60, 55);
            this.tsbAttach.Text = "&Attach";
            this.tsbAttach.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAttach.Click += new System.EventHandler(this.tsbAttach_Click);
            // 
            // tsbCopy
            // 
            this.tsbCopy.AutoSize = false;
            this.tsbCopy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbCopy.Image = global::WinUI.Properties.Resources.copy123;
            this.tsbCopy.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(60, 55);
            this.tsbCopy.Text = "&Copy to IV";
            this.tsbCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbCopy.Click += new System.EventHandler(this.tsbCopy_Click);
            // 
            // gbEmail
            // 
            this.gbEmail.Controls.Add(this.lblEmailSubj);
            this.gbEmail.Controls.Add(this.txtEmailSubj);
            this.gbEmail.Controls.Add(this.lblLineEmailSubj);
            this.gbEmail.Controls.Add(this.txtFromEmail);
            this.gbEmail.Controls.Add(this.chkSentStatus);
            this.gbEmail.Controls.Add(this.txtToEmail);
            this.gbEmail.Controls.Add(this.btnSend);
            this.gbEmail.Controls.Add(this.lblLineTo);
            this.gbEmail.Controls.Add(this.lblLineFrom);
            this.gbEmail.Controls.Add(this.txtTo);
            this.gbEmail.Controls.Add(this.txtFrom);
            this.gbEmail.Location = new System.Drawing.Point(3, 3);
            this.gbEmail.Name = "gbEmail";
            this.gbEmail.Size = new System.Drawing.Size(812, 129);
            this.gbEmail.TabIndex = 154;
            this.gbEmail.TabStop = false;
            // 
            // lblEmailSubj
            // 
            this.lblEmailSubj.Font = new System.Drawing.Font("Calibri", 10.5F);
            this.lblEmailSubj.ForeColor = System.Drawing.Color.DimGray;
            this.lblEmailSubj.Location = new System.Drawing.Point(126, 88);
            this.lblEmailSubj.Name = "lblEmailSubj";
            this.lblEmailSubj.Size = new System.Drawing.Size(61, 27);
            this.lblEmailSubj.TabIndex = 472;
            this.lblEmailSubj.Text = "Subject";
            this.lblEmailSubj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEmailSubj
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.txtEmailSubj.Appearance = appearance3;
            this.txtEmailSubj.AutoSize = false;
            this.txtEmailSubj.BackColor = System.Drawing.Color.White;
            this.txtEmailSubj.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtEmailSubj.Font = new System.Drawing.Font("Calibri", 10F);
            this.txtEmailSubj.Format = "";
            this.txtEmailSubj.IsDirty = false;
            this.txtEmailSubj.IsEmailTextBox = false;
            this.txtEmailSubj.Location = new System.Drawing.Point(193, 87);
            this.txtEmailSubj.Name = "txtEmailSubj";
            this.txtEmailSubj.Size = new System.Drawing.Size(610, 21);
            this.txtEmailSubj.TabIndex = 471;
            this.txtEmailSubj.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // lblLineEmailSubj
            // 
            this.lblLineEmailSubj.BackColor = System.Drawing.Color.DarkGray;
            this.lblLineEmailSubj.Location = new System.Drawing.Point(194, 111);
            this.lblLineEmailSubj.Name = "lblLineEmailSubj";
            this.lblLineEmailSubj.Size = new System.Drawing.Size(610, 1);
            this.lblLineEmailSubj.TabIndex = 470;
            // 
            // txtFromEmail
            // 
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.FontData.Name = "Calibri";
            appearance10.FontData.SizeInPoints = 10F;
            appearance10.ForeColor = System.Drawing.Color.DimGray;
            this.txtFromEmail.Appearance = appearance10;
            this.txtFromEmail.AutoSize = false;
            this.txtFromEmail.BackColor = System.Drawing.Color.White;
            this.txtFromEmail.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtFromEmail.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtFromEmail.Font = new System.Drawing.Font("Calibri", 10F);
            this.txtFromEmail.Format = "";
            this.txtFromEmail.IsDirty = false;
            this.txtFromEmail.IsEmailTextBox = true;
            this.txtFromEmail.Location = new System.Drawing.Point(193, 23);
            this.txtFromEmail.Name = "txtFromEmail";
            this.txtFromEmail.ReadOnly = true;
            this.txtFromEmail.Size = new System.Drawing.Size(610, 21);
            this.txtFromEmail.TabIndex = 468;
            this.txtFromEmail.Text = "ar@bhglobal.com.sg;";
            this.txtFromEmail.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // chkSentStatus
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.FontData.ItalicAsString = "True";
            appearance11.FontData.Name = "Calibri";
            appearance11.FontData.SizeInPoints = 9.5F;
            this.chkSentStatus.Appearance = appearance11;
            this.chkSentStatus.BackColor = System.Drawing.Color.Transparent;
            this.chkSentStatus.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkSentStatus.cancelUpdate = true;
            this.chkSentStatus.Checked = true;
            this.chkSentStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSentStatus.Enabled = false;
            this.chkSentStatus.Location = new System.Drawing.Point(16, 89);
            this.chkSentStatus.Name = "chkSentStatus";
            this.chkSentStatus.Size = new System.Drawing.Size(105, 27);
            this.chkSentStatus.TabIndex = 469;
            this.chkSentStatus.TabStop = false;
            this.chkSentStatus.Text = "Sent e-Invoices";
            this.chkSentStatus.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // txtToEmail
            // 
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.txtToEmail.Appearance = appearance12;
            this.txtToEmail.AutoSize = false;
            this.txtToEmail.BackColor = System.Drawing.Color.White;
            this.txtToEmail.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtToEmail.Font = new System.Drawing.Font("Calibri", 10F);
            this.txtToEmail.Format = "";
            this.txtToEmail.IsDirty = false;
            this.txtToEmail.IsEmailTextBox = false;
            this.txtToEmail.Location = new System.Drawing.Point(193, 55);
            this.txtToEmail.Name = "txtToEmail";
            this.txtToEmail.Size = new System.Drawing.Size(610, 21);
            this.txtToEmail.TabIndex = 467;
            this.txtToEmail.Text = "yin.suthwin@bhglobal.com.sg;";
            this.txtToEmail.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.White;
            this.btnSend.BackgroundImage = global::WinUI.Properties.Resources.OutLookSend;
            this.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSend.Font = new System.Drawing.Font("Calibri", 11F);
            this.btnSend.Location = new System.Drawing.Point(14, 20);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(81, 62);
            this.btnSend.TabIndex = 466;
            this.btnSend.Text = "Send";
            this.btnSend.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblLineTo
            // 
            this.lblLineTo.BackColor = System.Drawing.Color.DarkGray;
            this.lblLineTo.Location = new System.Drawing.Point(194, 79);
            this.lblLineTo.Name = "lblLineTo";
            this.lblLineTo.Size = new System.Drawing.Size(610, 1);
            this.lblLineTo.TabIndex = 465;
            // 
            // lblLineFrom
            // 
            this.lblLineFrom.BackColor = System.Drawing.Color.DarkGray;
            this.lblLineFrom.Location = new System.Drawing.Point(194, 47);
            this.lblLineFrom.Name = "lblLineFrom";
            this.lblLineFrom.Size = new System.Drawing.Size(610, 1);
            this.lblLineFrom.TabIndex = 464;
            this.lblLineFrom.Text = "label1";
            // 
            // txtTo
            // 
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.LightGray;
            appearance13.FontData.Name = "Calibri";
            appearance13.FontData.SizeInPoints = 11F;
            appearance13.ForeColor = System.Drawing.Color.DimGray;
            appearance13.TextHAlignAsString = "Center";
            this.txtTo.Appearance = appearance13;
            this.txtTo.AutoSize = false;
            this.txtTo.BackColor = System.Drawing.Color.White;
            this.txtTo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtTo.Font = new System.Drawing.Font("Calibri", 11F);
            this.txtTo.Format = "";
            this.txtTo.IsDirty = false;
            this.txtTo.IsEmailTextBox = false;
            this.txtTo.Location = new System.Drawing.Point(124, 53);
            this.txtTo.MaxLength = 50;
            this.txtTo.Name = "txtTo";
            this.txtTo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTo.Size = new System.Drawing.Size(63, 27);
            this.txtTo.TabIndex = 463;
            this.txtTo.Text = "To";
            this.txtTo.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // txtFrom
            // 
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BorderColor = System.Drawing.Color.LightGray;
            appearance14.FontData.Name = "Calibri";
            appearance14.FontData.SizeInPoints = 11F;
            appearance14.ForeColor = System.Drawing.Color.DimGray;
            appearance14.TextHAlignAsString = "Center";
            this.txtFrom.Appearance = appearance14;
            this.txtFrom.AutoSize = false;
            this.txtFrom.BackColor = System.Drawing.Color.White;
            this.txtFrom.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtFrom.Font = new System.Drawing.Font("Calibri", 11F);
            this.txtFrom.Format = "";
            this.txtFrom.IsDirty = false;
            this.txtFrom.IsEmailTextBox = false;
            this.txtFrom.Location = new System.Drawing.Point(124, 21);
            this.txtFrom.MaxLength = 50;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFrom.Size = new System.Drawing.Size(63, 27);
            this.txtFrom.TabIndex = 462;
            this.txtFrom.Text = "From";
            this.txtFrom.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // chkZipFiles
            // 
            appearance15.BackColor = System.Drawing.Color.Transparent;
            appearance15.FontData.ItalicAsString = "True";
            appearance15.FontData.Name = "Calibri";
            appearance15.FontData.SizeInPoints = 9.5F;
            appearance15.FontData.StrikeoutAsString = "True";
            this.chkZipFiles.Appearance = appearance15;
            this.chkZipFiles.BackColor = System.Drawing.Color.Transparent;
            this.chkZipFiles.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkZipFiles.cancelUpdate = true;
            this.chkZipFiles.Enabled = false;
            this.chkZipFiles.Location = new System.Drawing.Point(385, 138);
            this.chkZipFiles.Name = "chkZipFiles";
            this.chkZipFiles.Size = new System.Drawing.Size(175, 22);
            this.chkZipFiles.TabIndex = 475;
            this.chkZipFiles.TabStop = false;
            this.chkZipFiles.Text = "Zip PDF and Non-PDF Files ";
            this.chkZipFiles.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.chkZipFiles.Visible = false;
            // 
            // flpnlAttachment
            // 
            this.flpnlAttachment.Controls.Add(this.gbEmail);
            this.flpnlAttachment.Controls.Add(this.chkCustomerPO);
            this.flpnlAttachment.Controls.Add(this.chkMergePDF);
            this.flpnlAttachment.Controls.Add(this.btnDownload);
            this.flpnlAttachment.Controls.Add(this.chkZipFiles);
            this.flpnlAttachment.Controls.Add(this.tagrdAttachment);
            this.flpnlAttachment.Location = new System.Drawing.Point(2, 62);
            this.flpnlAttachment.Name = "flpnlAttachment";
            this.flpnlAttachment.Size = new System.Drawing.Size(818, 395);
            this.flpnlAttachment.TabIndex = 469;
            // 
            // chkCustomerPO
            // 
            appearance17.BackColor = System.Drawing.Color.Transparent;
            appearance17.FontData.ItalicAsString = "True";
            appearance17.FontData.Name = "Calibri";
            appearance17.FontData.SizeInPoints = 9.5F;
            appearance17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.chkCustomerPO.Appearance = appearance17;
            this.chkCustomerPO.BackColor = System.Drawing.Color.Transparent;
            this.chkCustomerPO.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkCustomerPO.cancelUpdate = false;
            this.chkCustomerPO.Location = new System.Drawing.Point(3, 138);
            this.chkCustomerPO.Name = "chkCustomerPO";
            this.chkCustomerPO.Size = new System.Drawing.Size(147, 22);
            this.chkCustomerPO.TabIndex = 429;
            this.chkCustomerPO.TabStop = false;
            this.chkCustomerPO.Text = "With Customer PO#";
            this.chkCustomerPO.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // chkMergePDF
            // 
            appearance16.BackColor = System.Drawing.Color.Transparent;
            appearance16.FontData.ItalicAsString = "True";
            appearance16.FontData.Name = "Calibri";
            appearance16.FontData.SizeInPoints = 9.5F;
            this.chkMergePDF.Appearance = appearance16;
            this.chkMergePDF.BackColor = System.Drawing.Color.Transparent;
            this.chkMergePDF.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkMergePDF.cancelUpdate = false;
            this.chkMergePDF.Location = new System.Drawing.Point(156, 138);
            this.chkMergePDF.Name = "chkMergePDF";
            this.chkMergePDF.Size = new System.Drawing.Size(121, 22);
            this.chkMergePDF.TabIndex = 473;
            this.chkMergePDF.TabStop = false;
            this.chkMergePDF.Text = "Merge PDF Files";
            this.chkMergePDF.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.White;
            this.btnDownload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDownload.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Calibri", 9.5F, System.Drawing.FontStyle.Italic);
            this.btnDownload.Image = global::WinUI.Properties.Resources.download_01_16_png;
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.Location = new System.Drawing.Point(283, 138);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(96, 22);
            this.btnDownload.TabIndex = 474;
            this.btnDownload.Text = "Download";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // frmAttachment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(822, 457);
            this.Controls.Add(this.flpnlAttachment);
            this.Controls.Add(this.tspBar);
            this.KeyPreview = true;
            this.Name = "frmAttachment";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmPopupAttachment";
            this.Text = "frmPopupAttachment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPopupAttachment_FormClosing);
            this.Load += new System.EventHandler(this.frmPopupAttachment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAttachment_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdAttachment)).EndInit();
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            this.gbEmail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailSubj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSentStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZipFiles)).EndInit();
            this.flpnlAttachment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkCustomerPO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMergePDF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TAUtil.TAGridEditor tagrdAttachment;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbAttach;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.GroupBox gbEmail;
        private TAUtil.TATextBoxEditor txtTo;
        private TAUtil.TATextBoxEditor txtFrom;
        private System.Windows.Forms.Label lblLineTo;
        private System.Windows.Forms.Label lblLineFrom;
        private System.Windows.Forms.Button btnSend;
        private TAUtil.TATextBoxEditor txtToEmail;
        private TAUtil.TATextBoxEditor txtFromEmail;
        private System.Windows.Forms.FlowLayoutPanel flpnlAttachment;
        private TAUtil.TACheckBoxEditor chkSentStatus;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.Label lblEmailSubj;
        private TAUtil.TATextBoxEditor txtEmailSubj;
        private System.Windows.Forms.Label lblLineEmailSubj;
        private TAUtil.TACheckBoxEditor chkZipFiles;
        private TAUtil.TACheckBoxEditor chkCustomerPO;
        private System.Windows.Forms.Button btnDownload;
        private TAUtil.TACheckBoxEditor chkMergePDF;
    }
}