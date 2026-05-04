namespace WinUI
{
    partial class frmInsertData
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
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmSN", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmMark", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmDes", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmQty", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmPriceAfter", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmAmtShw", 6);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.QtyDesLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UseSystemPrice = new TAUtil.TACheckBoxEditor();
            this.StartDate = new TAUtil.TADateEditor();
            this.EndDate = new TAUtil.TADateEditor();
            this.tagrdDocItmDetail = new TAUtil.TAGridEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.UseAdditionalRemAsDes = new TAUtil.TACheckBoxEditor();
            this.DocID = new TAUtil.TAComboBox();
            this.DocCode = new TAUtil.TAComboBox();
            this.includeNSLink = new TAUtil.TACheckBoxEditor();
            this.btnAppendAll = new Infragistics.Win.Misc.UltraButton();
            this.btnAppend = new Infragistics.Win.Misc.UltraButton();
            this.btnUnSelectAll = new Infragistics.Win.Misc.UltraButton();
            this.btnSelectAll = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.UseSystemPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocItmDetail)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UseAdditionalRemAsDes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.includeNSLink)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(463, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Document Code :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(11, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "From :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QtyDesLabel
            // 
            this.QtyDesLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QtyDesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.QtyDesLabel.Location = new System.Drawing.Point(11, 44);
            this.QtyDesLabel.Name = "QtyDesLabel";
            this.QtyDesLabel.Size = new System.Drawing.Size(33, 25);
            this.QtyDesLabel.TabIndex = 2;
            this.QtyDesLabel.Text = "To :";
            this.QtyDesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(463, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Document ID :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UseSystemPrice
            // 
            appearance15.FontData.ItalicAsString = "True";
            appearance15.FontData.Name = "Calibri";
            appearance15.FontData.SizeInPoints = 10F;
            appearance15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UseSystemPrice.Appearance = appearance15;
            this.UseSystemPrice.cancelUpdate = false;
            this.UseSystemPrice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.UseSystemPrice.Location = new System.Drawing.Point(11, 72);
            this.UseSystemPrice.Name = "UseSystemPrice";
            this.UseSystemPrice.Size = new System.Drawing.Size(128, 20);
            this.UseSystemPrice.TabIndex = 2;
            this.UseSystemPrice.Text = "Use System Price";
            this.UseSystemPrice.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // StartDate
            // 
            appearance4.BorderColor = System.Drawing.Color.LightGray;
            appearance4.TextHAlignAsString = "Right";
            this.StartDate.Appearance = appearance4;
            appearance2.Image = global::WinUI.Properties.Resources.calendar3;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance2;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.StartDate.ButtonsRight.Add(editorButton1);
            this.StartDate.calendarContainer = null;
            this.StartDate.DateValue = null;
            this.StartDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.StartDate.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDate.Format = "";
            this.StartDate.Location = new System.Drawing.Point(125, 16);
            this.StartDate.MaxLength = 20;
            this.StartDate.Name = "StartDate";
            this.StartDate.Size = new System.Drawing.Size(300, 25);
            this.StartDate.TabIndex = 0;
            this.StartDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.StartDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.StartDate_CustomUpdate);
            // 
            // EndDate
            // 
            appearance5.BorderColor = System.Drawing.Color.LightGray;
            appearance5.TextHAlignAsString = "Right";
            this.EndDate.Appearance = appearance5;
            this.EndDate.AutoSize = false;
            appearance1.Image = global::WinUI.Properties.Resources.calendar3;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton2.Appearance = appearance1;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.EndDate.ButtonsRight.Add(editorButton2);
            this.EndDate.calendarContainer = null;
            this.EndDate.DateValue = null;
            this.EndDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.EndDate.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndDate.Format = "";
            this.EndDate.Location = new System.Drawing.Point(125, 41);
            this.EndDate.MaxLength = 20;
            this.EndDate.Name = "EndDate";
            this.EndDate.Size = new System.Drawing.Size(300, 25);
            this.EndDate.TabIndex = 1;
            this.EndDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.EndDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.EndDate_CustomUpdate);
            // 
            // tagrdDocItmDetail
            // 
            this.tagrdDocItmDetail.ActiveConnection = null;
            this.tagrdDocItmDetail.AutoAddNewRow = false;
            this.tagrdDocItmDetail.AutoUseCustomControlsInCells = false;
            this.tagrdDocItmDetail.DefaultValue = null;
            this.tagrdDocItmDetail.DetailObjectKey = 0;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdDocItmDetail.DisplayLayout.Appearance = appearance6;
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
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7});
            this.tagrdDocItmDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdDocItmDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdDocItmDetail.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance7.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDocItmDetail.DisplayLayout.GroupByBox.Appearance = appearance7;
            appearance14.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocItmDetail.DisplayLayout.GroupByBox.BandLabelAppearance = appearance14;
            this.tagrdDocItmDetail.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdDocItmDetail.DisplayLayout.GroupByBox.Hidden = true;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocItmDetail.DisplayLayout.GroupByBox.PromptAppearance = appearance17;
            this.tagrdDocItmDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdDocItmDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdDocItmDetail.DisplayLayout.Override.ActiveCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.SystemColors.Highlight;
            appearance19.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tagrdDocItmDetail.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            this.tagrdDocItmDetail.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdDocItmDetail.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdDocItmDetail.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdDocItmDetail.DisplayLayout.Override.CellAppearance = appearance21;
            this.tagrdDocItmDetail.DisplayLayout.Override.CellPadding = 0;
            this.tagrdDocItmDetail.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDocItmDetail.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            appearance23.TextHAlignAsString = "Left";
            this.tagrdDocItmDetail.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.tagrdDocItmDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdDocItmDetail.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            this.tagrdDocItmDetail.DisplayLayout.Override.RowAppearance = appearance24;
            this.tagrdDocItmDetail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocItmDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDocItmDetail.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdDocItmDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.tagrdDocItmDetail.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDocItmDetail.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDocItmDetail.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDocItmDetail.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.tagrdDocItmDetail.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDocItmDetail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDocItmDetail.HeaderObjectKey = null;
            this.tagrdDocItmDetail.Location = new System.Drawing.Point(12, 176);
            this.tagrdDocItmDetail.Name = "tagrdDocItmDetail";
            this.tagrdDocItmDetail.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDocItmDetail.Size = new System.Drawing.Size(908, 286);
            this.tagrdDocItmDetail.TabIndex = 0;
            this.tagrdDocItmDetail.Text = "taGridEditor1";
            this.tagrdDocItmDetail.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocItmDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.UseAdditionalRemAsDes);
            this.panel1.Controls.Add(this.DocID);
            this.panel1.Controls.Add(this.DocCode);
            this.panel1.Controls.Add(this.includeNSLink);
            this.panel1.Controls.Add(this.btnAppendAll);
            this.panel1.Controls.Add(this.btnAppend);
            this.panel1.Controls.Add(this.btnUnSelectAll);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.EndDate);
            this.panel1.Controls.Add(this.UseSystemPrice);
            this.panel1.Controls.Add(this.QtyDesLabel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.StartDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(908, 158);
            this.panel1.TabIndex = 0;
            // 
            // UseAdditionalRemAsDes
            // 
            appearance3.FontData.ItalicAsString = "True";
            appearance3.FontData.Name = "Calibri";
            appearance3.FontData.SizeInPoints = 10F;
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UseAdditionalRemAsDes.Appearance = appearance3;
            this.UseAdditionalRemAsDes.cancelUpdate = false;
            this.UseAdditionalRemAsDes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.UseAdditionalRemAsDes.Location = new System.Drawing.Point(466, 74);
            this.UseAdditionalRemAsDes.Name = "UseAdditionalRemAsDes";
            this.UseAdditionalRemAsDes.Size = new System.Drawing.Size(154, 35);
            this.UseAdditionalRemAsDes.TabIndex = 38;
            this.UseAdditionalRemAsDes.Text = "Use Additional Remark as Description";
            this.UseAdditionalRemAsDes.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // DocID
            // 
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BorderColor = System.Drawing.Color.LightGray;
            appearance11.FontData.Name = "Calibri";
            appearance11.FontData.SizeInPoints = 11F;
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.DocID.Appearance = appearance11;
            this.DocID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocID.ComboIsDirty = false;
            this.DocID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocID.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocID.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocID.Location = new System.Drawing.Point(587, 43);
            this.DocID.Name = "DocID";
            this.DocID.Size = new System.Drawing.Size(298, 26);
            this.DocID.TabIndex = 37;
            this.DocID.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocID.UserInputText = "";
            this.DocID.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocID_CustomUpdate);
            this.DocID.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // DocCode
            // 
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BorderColor = System.Drawing.Color.LightGray;
            appearance16.FontData.Name = "Calibri";
            appearance16.FontData.SizeInPoints = 11F;
            appearance16.ForeColor = System.Drawing.Color.Black;
            this.DocCode.Appearance = appearance16;
            this.DocCode.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocCode.ComboIsDirty = false;
            this.DocCode.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocCode.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocCode.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocCode.Location = new System.Drawing.Point(587, 17);
            this.DocCode.Name = "DocCode";
            this.DocCode.Size = new System.Drawing.Size(298, 26);
            this.DocCode.TabIndex = 36;
            this.DocCode.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocCode.UserInputText = "";
            this.DocCode.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.DocCode_CustomUpdate);
            this.DocCode.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // includeNSLink
            // 
            appearance26.FontData.ItalicAsString = "True";
            appearance26.FontData.Name = "Calibri";
            appearance26.FontData.SizeInPoints = 10F;
            appearance26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.includeNSLink.Appearance = appearance26;
            this.includeNSLink.cancelUpdate = false;
            this.includeNSLink.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.includeNSLink.Location = new System.Drawing.Point(297, 72);
            this.includeNSLink.Name = "includeNSLink";
            this.includeNSLink.Size = new System.Drawing.Size(128, 20);
            this.includeNSLink.TabIndex = 3;
            this.includeNSLink.Text = "Include NSLink";
            this.includeNSLink.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // btnAppendAll
            // 
            appearance12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance12.Image = global::WinUI.Properties.Resources.Append_16;
            this.btnAppendAll.Appearance = appearance12;
            this.btnAppendAll.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnAppendAll.Location = new System.Drawing.Point(350, 115);
            this.btnAppendAll.Name = "btnAppendAll";
            this.btnAppendAll.Size = new System.Drawing.Size(130, 26);
            this.btnAppendAll.TabIndex = 10;
            this.btnAppendAll.Text = "Append & All records";
            this.btnAppendAll.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnAppendAll.Click += new System.EventHandler(this.btnAppendAll_Click);
            // 
            // btnAppend
            // 
            appearance13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance13.Image = global::WinUI.Properties.Resources.Append_16;
            this.btnAppend.Appearance = appearance13;
            this.btnAppend.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnAppend.Location = new System.Drawing.Point(238, 115);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(106, 26);
            this.btnAppend.TabIndex = 9;
            this.btnAppend.Text = "A&ppend";
            this.btnAppend.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnUnSelectAll
            // 
            appearance10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance10.Image = global::WinUI.Properties.Resources.selectnone;
            this.btnUnSelectAll.Appearance = appearance10;
            this.btnUnSelectAll.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnUnSelectAll.Location = new System.Drawing.Point(126, 115);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(106, 26);
            this.btnUnSelectAll.TabIndex = 8;
            this.btnUnSelectAll.Text = "&Select None";
            this.btnUnSelectAll.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnUnSelectAll.Click += new System.EventHandler(this.btnUnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            appearance9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.Image = global::WinUI.Properties.Resources.selectall;
            this.btnSelectAll.Appearance = appearance9;
            this.btnSelectAll.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnSelectAll.Location = new System.Drawing.Point(14, 115);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(106, 26);
            this.btnSelectAll.TabIndex = 7;
            this.btnSelectAll.Text = "Select &All";
            this.btnSelectAll.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnClose
            // 
            appearance8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance8.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.Appearance = appearance8;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.Location = new System.Drawing.Point(779, 115);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 26);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "&Close";
            this.btnClose.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmInsertData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(932, 474);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tagrdDocItmDetail);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(630, 511);
            this.Name = "frmInsertData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmPopupDocDetInsertData";
            this.Text = "Add By Item";            
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInsertData_FormClosed);
            this.Load += new System.EventHandler(this.frmInsertData_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInsertData_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.UseSystemPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocItmDetail)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UseAdditionalRemAsDes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.includeNSLink)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label QtyDesLabel;
        private System.Windows.Forms.Label label3;
        private TAUtil.TACheckBoxEditor UseSystemPrice;
        private TAUtil.TADateEditor StartDate;
        private TAUtil.TADateEditor EndDate;
        private TAUtil.TAGridEditor tagrdDocItmDetail;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnSelectAll;
        private Infragistics.Win.Misc.UltraButton btnUnSelectAll;
        private Infragistics.Win.Misc.UltraButton btnAppend;
        private Infragistics.Win.Misc.UltraButton btnAppendAll;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private TAUtil.TACheckBoxEditor includeNSLink;
        private TAUtil.TAComboBox DocCode;
        private TAUtil.TAComboBox DocID;
        private TAUtil.TACheckBoxEditor UseAdditionalRemAsDes;

    }
}