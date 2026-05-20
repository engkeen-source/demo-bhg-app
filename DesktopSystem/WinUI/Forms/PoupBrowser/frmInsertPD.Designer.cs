namespace WinUI
{
    partial class frmInsertPD
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmSN", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmKey", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmDes", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmQty", 3);
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            this.btnAppend = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.DocDate = new TAUtil.TADateEditor();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DocKey = new TAUtil.TAComboBox();
            this.tagrdInsertPD = new TAUtil.TAGridEditor();
            ((System.ComponentModel.ISupportInitialize)(this.DocDate)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DocKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdInsertPD)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAppend
            // 
            this.btnAppend.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnAppend.Image = global::WinUI.Properties.Resources.Append_16;
            this.btnAppend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppend.Location = new System.Drawing.Point(526, 38);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(90, 25);
            this.btnAppend.TabIndex = 5;
            this.btnAppend.Text = "Append";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(622, 38);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DocDate
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BorderColor = System.Drawing.Color.LightGray;
            appearance4.FontData.Name = "Calibri";
            appearance4.FontData.SizeInPoints = 11F;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.TextHAlignAsString = "Right";
            this.DocDate.Appearance = appearance4;
            this.DocDate.BackColor = System.Drawing.Color.White;
            appearance1.Image = global::WinUI.Properties.Resources.calendar3;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.DocDate.ButtonsRight.Add(editorButton1);
            this.DocDate.calendarContainer = null;
            this.DocDate.DateValue = null;
            this.DocDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.DocDate.Format = "";
            this.DocDate.Location = new System.Drawing.Point(101, 11);
            this.DocDate.MaxLength = 20;
            this.DocDate.Name = "DocDate";
            this.DocDate.Size = new System.Drawing.Size(300, 26);
            this.DocDate.TabIndex = 1;
            this.DocDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocDate_CustomUpdate);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Date";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(12, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Doc Num:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnRefresh.Image = global::WinUI.Properties.Resources.refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(430, 38);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 25);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.DocKey);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnAppend);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DocDate);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(12, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(726, 77);
            this.panel1.TabIndex = 15;
            // 
            // DocKey
            // 
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BorderColor = System.Drawing.Color.LightGray;
            appearance31.FontData.Name = "Calibri";
            appearance31.FontData.SizeInPoints = 11F;
            appearance31.ForeColor = System.Drawing.Color.Black;
            this.DocKey.Appearance = appearance31;
            this.DocKey.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocKey.ComboIsDirty = false;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.DocKey.DisplayLayout.Appearance = appearance11;
            this.DocKey.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.DocKey.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance20.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.DocKey.DisplayLayout.GroupByBox.Appearance = appearance20;
            appearance21.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DocKey.DisplayLayout.GroupByBox.BandLabelAppearance = appearance21;
            this.DocKey.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance22.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DocKey.DisplayLayout.GroupByBox.PromptAppearance = appearance22;
            this.DocKey.DisplayLayout.MaxColScrollRegions = 1;
            this.DocKey.DisplayLayout.MaxRowScrollRegions = 1;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DocKey.DisplayLayout.Override.ActiveCellAppearance = appearance23;
            appearance24.BackColor = System.Drawing.SystemColors.Highlight;
            appearance24.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.DocKey.DisplayLayout.Override.ActiveRowAppearance = appearance24;
            this.DocKey.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.DocKey.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            this.DocKey.DisplayLayout.Override.CardAreaAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Silver;
            appearance26.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.DocKey.DisplayLayout.Override.CellAppearance = appearance26;
            this.DocKey.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.DocKey.DisplayLayout.Override.CellPadding = 0;
            appearance27.BackColor = System.Drawing.SystemColors.Control;
            appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance27.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance27.BorderColor = System.Drawing.SystemColors.Window;
            this.DocKey.DisplayLayout.Override.GroupByRowAppearance = appearance27;
            appearance28.TextHAlignAsString = "Left";
            this.DocKey.DisplayLayout.Override.HeaderAppearance = appearance28;
            this.DocKey.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.DocKey.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            this.DocKey.DisplayLayout.Override.RowAppearance = appearance32;
            this.DocKey.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance30.BackColor = System.Drawing.SystemColors.ControlLight;
            this.DocKey.DisplayLayout.Override.TemplateAddRowAppearance = appearance30;
            this.DocKey.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.DocKey.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.DocKey.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.DocKey.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocKey.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocKey.Location = new System.Drawing.Point(101, 37);
            this.DocKey.Name = "DocKey";
            this.DocKey.Size = new System.Drawing.Size(300, 26);
            this.DocKey.TabIndex = 15;
            this.DocKey.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocKey.UserInputText = "";
            this.DocKey.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocKey_CustomUpdate);
            this.DocKey.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // tagrdInsertPD
            // 
            this.tagrdInsertPD.ActiveConnection = null;
            this.tagrdInsertPD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdInsertPD.AutoAddNewRow = false;
            this.tagrdInsertPD.AutoUseCustomControlsInCells = false;
            this.tagrdInsertPD.DefaultValue = null;
            this.tagrdInsertPD.DetailObjectKey = 0;
            appearance46.AlphaLevel = ((short)(255));
            appearance46.BackColor = System.Drawing.Color.AliceBlue;
            appearance46.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance46.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance46.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdInsertPD.DisplayLayout.Appearance = appearance46;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            this.tagrdInsertPD.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdInsertPD.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance47.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance47.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance47.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdInsertPD.DisplayLayout.GroupByBox.Appearance = appearance47;
            appearance48.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdInsertPD.DisplayLayout.GroupByBox.BandLabelAppearance = appearance48;
            this.tagrdInsertPD.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance49.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance49.BackColor2 = System.Drawing.SystemColors.Control;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance49.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdInsertPD.DisplayLayout.GroupByBox.PromptAppearance = appearance49;
            this.tagrdInsertPD.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdInsertPD.DisplayLayout.MaxRowScrollRegions = 1;
            appearance50.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdInsertPD.DisplayLayout.Override.ActiveCellAppearance = appearance50;
            appearance51.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdInsertPD.DisplayLayout.Override.ActiveRowAppearance = appearance51;
            this.tagrdInsertPD.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdInsertPD.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdInsertPD.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance52.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdInsertPD.DisplayLayout.Override.CardAreaAppearance = appearance52;
            appearance53.BorderColor = System.Drawing.Color.Silver;
            appearance53.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdInsertPD.DisplayLayout.Override.CellAppearance = appearance53;
            this.tagrdInsertPD.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdInsertPD.DisplayLayout.Override.CellPadding = 0;
            this.tagrdInsertPD.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdInsertPD.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance54.AlphaLevel = ((short)(255));
            appearance54.BackColor = System.Drawing.Color.AliceBlue;
            appearance54.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance54.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance54.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance54.ForeColor = System.Drawing.Color.Black;
            appearance54.TextHAlignAsString = "Left";
            this.tagrdInsertPD.DisplayLayout.Override.HeaderAppearance = appearance54;
            this.tagrdInsertPD.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.tagrdInsertPD.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance55.BackColor = System.Drawing.Color.White;
            appearance55.BackColor2 = System.Drawing.Color.White;
            appearance55.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance55.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance55.ForeColor = System.Drawing.Color.Black;
            appearance55.TextVAlignAsString = "Middle";
            this.tagrdInsertPD.DisplayLayout.Override.RowAppearance = appearance55;
            appearance56.AlphaLevel = ((short)(255));
            appearance56.BackColor = System.Drawing.Color.AliceBlue;
            appearance56.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance56.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance56.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdInsertPD.DisplayLayout.Override.RowSelectorAppearance = appearance56;
            appearance57.BackColor = System.Drawing.Color.AliceBlue;
            appearance57.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance57.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance57.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdInsertPD.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance57;
            this.tagrdInsertPD.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdInsertPD.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdInsertPD.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdInsertPD.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance58.BackColor = System.Drawing.Color.Gold;
            appearance58.BackColor2 = System.Drawing.Color.White;
            appearance58.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdInsertPD.DisplayLayout.Override.SelectedRowAppearance = appearance58;
            this.tagrdInsertPD.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.tagrdInsertPD.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            appearance59.BackColor = System.Drawing.Color.White;
            appearance59.ForeColor = System.Drawing.Color.Black;
            this.tagrdInsertPD.DisplayLayout.Override.TemplateAddRowCellAppearance = appearance59;
            this.tagrdInsertPD.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdInsertPD.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdInsertPD.DisplayLayout.UseFixedHeaders = true;
            this.tagrdInsertPD.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdInsertPD.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdInsertPD.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdInsertPD.HeaderObjectKey = null;
            this.tagrdInsertPD.Location = new System.Drawing.Point(12, 88);
            this.tagrdInsertPD.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdInsertPD.Name = "tagrdInsertPD";
            this.tagrdInsertPD.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdInsertPD.Size = new System.Drawing.Size(726, 336);
            this.tagrdInsertPD.TabIndex = 16;
            this.tagrdInsertPD.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdInsertPD.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // frmInsertPD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(748, 437);
            this.Controls.Add(this.tagrdInsertPD);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmInsertPD";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmInsertPD";
            this.Text = "Document Selection";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInsertPD_FormClosed);
            this.Load += new System.EventHandler(this.frmInsertPD_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInsertPD_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.DocDate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DocKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdInsertPD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.Button btnClose;
        private TAUtil.TADateEditor DocDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel1;
        private TAUtil.TAGridEditor tagrdInsertPD;
        private TAUtil.TAComboBox DocKey;
      
    }
}