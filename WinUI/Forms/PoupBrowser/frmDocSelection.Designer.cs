namespace WinUI
{
    partial class frmDocSelection
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
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocNums");
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            this.tagrdDocNums = new TAUtil.TAGridEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ToDate = new TAUtil.TADateEditor();
            this.FromDate = new TAUtil.TADateEditor();
            this.IncludeBatch = new TAUtil.TACheckBoxEditor();
            this.DocCode = new TAUtil.TAComboBox();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.btnRequery = new Infragistics.Win.Misc.UltraButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocNums)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IncludeBatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocCode)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagrdDocNums
            // 
            this.tagrdDocNums.AllowDrop = true;
            this.tagrdDocNums.AutoAddNewRow = true;
            this.tagrdDocNums.AutoUseCustomControlsInCells = true;
            this.tagrdDocNums.DefaultValue = null;
            this.tagrdDocNums.DetailObjectKey = 0;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdDocNums.DisplayLayout.Appearance = appearance27;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1});
            this.tagrdDocNums.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdDocNums.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdDocNums.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance28.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance28.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDocNums.DisplayLayout.GroupByBox.Appearance = appearance28;
            appearance29.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocNums.DisplayLayout.GroupByBox.BandLabelAppearance = appearance29;
            this.tagrdDocNums.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance30.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocNums.DisplayLayout.GroupByBox.PromptAppearance = appearance30;
            this.tagrdDocNums.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdDocNums.DisplayLayout.MaxRowScrollRegions = 1;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            appearance31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdDocNums.DisplayLayout.Override.ActiveCellAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.Gold;
            appearance32.BackColor2 = System.Drawing.Color.White;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.tagrdDocNums.DisplayLayout.Override.ActiveRowAppearance = appearance32;
            this.tagrdDocNums.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.tagrdDocNums.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdDocNums.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdDocNums.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance33.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdDocNums.DisplayLayout.Override.CardAreaAppearance = appearance33;
            appearance34.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance34.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance34.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdDocNums.DisplayLayout.Override.CellAppearance = appearance34;
            this.tagrdDocNums.DisplayLayout.Override.CellPadding = 0;
            appearance35.BackColor = System.Drawing.Color.LightYellow;
            this.tagrdDocNums.DisplayLayout.Override.DataErrorRowAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.ForestGreen;
            this.tagrdDocNums.DisplayLayout.Override.DataErrorRowSelectorAppearance = appearance36;
            this.tagrdDocNums.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance37.BackColor = System.Drawing.SystemColors.Control;
            appearance37.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance37.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance37.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDocNums.DisplayLayout.Override.GroupByRowAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.AliceBlue;
            appearance38.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance38.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance38.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance38.TextHAlignAsString = "Left";
            this.tagrdDocNums.DisplayLayout.Override.HeaderAppearance = appearance38;
            this.tagrdDocNums.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdDocNums.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance39.BackColor = System.Drawing.SystemColors.Window;
            appearance39.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance39.ForeColor = System.Drawing.Color.Black;
            this.tagrdDocNums.DisplayLayout.Override.RowAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.AliceBlue;
            appearance40.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance40.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance40.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdDocNums.DisplayLayout.Override.RowSelectorAppearance = appearance40;
            appearance41.BackColor = System.Drawing.Color.AliceBlue;
            appearance41.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance41.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance41.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdDocNums.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.Gold;
            this.tagrdDocNums.DisplayLayout.Override.SelectedRowAppearance = appearance42;
            this.tagrdDocNums.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDocNums.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.tagrdDocNums.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance43.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdDocNums.DisplayLayout.Override.TemplateAddRowAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.ForeColor = System.Drawing.Color.Black;
            this.tagrdDocNums.DisplayLayout.Override.TemplateAddRowCellAppearance = appearance44;
            this.tagrdDocNums.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.tagrdDocNums.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDocNums.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDocNums.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDocNums.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDocNums.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDocNums.HeaderObjectKey = null;
            this.tagrdDocNums.Location = new System.Drawing.Point(116, 100);
            this.tagrdDocNums.Name = "tagrdDocNums";
            this.tagrdDocNums.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDocNums.Size = new System.Drawing.Size(536, 148);
            this.tagrdDocNums.TabIndex = 5;
            this.tagrdDocNums.Text = "taGridEditor1";
            this.tagrdDocNums.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocNums.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(26, 259);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 1);
            this.panel1.TabIndex = 9;
            // 
            // ToDate
            // 
            appearance19.BorderColor = System.Drawing.Color.LightGray;
            appearance19.TextHAlignAsString = "Right";
            this.ToDate.Appearance = appearance19;
            this.ToDate.AutoSize = false;
            appearance22.Image = global::WinUI.Properties.Resources.calendar3;
            appearance22.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance22;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.ToDate.ButtonsRight.Add(editorButton1);
            this.ToDate.calendarContainer = null;
            this.ToDate.DateValue = null;
            this.ToDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ToDate.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToDate.Format = "";
            this.ToDate.Location = new System.Drawing.Point(447, 37);
            this.ToDate.MaxLength = 20;
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new System.Drawing.Size(300, 25);
            this.ToDate.TabIndex = 2;
            this.ToDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // FromDate
            // 
            appearance23.BorderColor = System.Drawing.Color.LightGray;
            appearance23.TextHAlignAsString = "Right";
            this.FromDate.Appearance = appearance23;
            this.FromDate.AutoSize = false;
            appearance24.Image = global::WinUI.Properties.Resources.calendar3;
            appearance24.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton2.Appearance = appearance24;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.FromDate.ButtonsRight.Add(editorButton2);
            this.FromDate.calendarContainer = null;
            this.FromDate.DateValue = null;
            this.FromDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.FromDate.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FromDate.Format = "";
            this.FromDate.Location = new System.Drawing.Point(116, 37);
            this.FromDate.MaxLength = 20;
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(300, 25);
            this.FromDate.TabIndex = 1;
            this.FromDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // IncludeBatch
            // 
            this.IncludeBatch.BackColor = System.Drawing.Color.Transparent;
            this.IncludeBatch.BackColorInternal = System.Drawing.Color.Transparent;
            this.IncludeBatch.cancelUpdate = false;
            this.IncludeBatch.Location = new System.Drawing.Point(118, 65);
            this.IncludeBatch.Name = "IncludeBatch";
            this.IncludeBatch.Size = new System.Drawing.Size(20, 20);
            this.IncludeBatch.TabIndex = 3;
            // 
            // DocCode
            // 
            appearance20.BorderColor = System.Drawing.Color.LightGray;
            this.DocCode.Appearance = appearance20;
            this.DocCode.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocCode.AutoSize = false;
            this.DocCode.ComboIsDirty = false;
            this.DocCode.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocCode.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocCode.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocCode.Location = new System.Drawing.Point(116, 12);
            this.DocCode.Name = "DocCode";
            this.DocCode.Size = new System.Drawing.Size(300, 25);
            this.DocCode.TabIndex = 0;
            this.DocCode.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocCode.UserInputText = "";
            this.DocCode.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // btnCancel
            // 
            appearance26.FontData.BoldAsString = "False";
            appearance26.FontData.ItalicAsString = "True";
            appearance26.FontData.Name = "Calibri";
            appearance26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance26.Image = global::WinUI.Properties.Resources.Cancel_16;
            this.btnCancel.Appearance = appearance26;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnCancel.Location = new System.Drawing.Point(666, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 26);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            appearance25.FontData.BoldAsString = "False";
            appearance25.FontData.ItalicAsString = "True";
            appearance25.FontData.Name = "Calibri";
            appearance25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance25.Image = global::WinUI.Properties.Resources.ok_16;
            this.btnOK.Appearance = appearance25;
            this.btnOK.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnOK.Location = new System.Drawing.Point(666, 182);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(81, 26);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&OK";
            this.btnOK.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnRequery
            // 
            appearance21.FontData.BoldAsString = "False";
            appearance21.FontData.ItalicAsString = "True";
            appearance21.FontData.Name = "Calibri";
            appearance21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance21.Image = global::WinUI.Properties.Resources.refresh;
            this.btnRequery.Appearance = appearance21;
            this.btnRequery.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnRequery.Location = new System.Drawing.Point(666, 64);
            this.btnRequery.Name = "btnRequery";
            this.btnRequery.Size = new System.Drawing.Size(81, 32);
            this.btnRequery.TabIndex = 4;
            this.btnRequery.Text = "&Refresh";
            this.btnRequery.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnRequery.Click += new System.EventHandler(this.btnRequery_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(422, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(28, 263);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(511, 48);
            this.label5.TabIndex = 8;
            this.label5.Text = "Note:\r\nThe Data in the Packing List Detail will be replace with the New sets of D" +
                "ata selected.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(23, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Include Batch";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(23, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date Range";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(23, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Doc Code";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel2.Controls.Add(this.tagrdDocNums);
            this.panel2.Controls.Add(this.DocCode);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.ToDate);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.FromDate);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.IncludeBatch);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnRequery);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(763, 332);
            this.panel2.TabIndex = 0;
            // 
            // frmDocSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(787, 356);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmDocSelection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " CM Document Selection";
            this.Load += new System.EventHandler(this.frmDocSelection_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocSelection_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocNums)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IncludeBatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocCode)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private TAUtil.TADateEditor FromDate;
        private TAUtil.TAComboBox DocCode;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnOK;
        private Infragistics.Win.Misc.UltraButton btnRequery;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private TAUtil.TAGridEditor tagrdDocNums;
        internal TAUtil.TACheckBoxEditor IncludeBatch;
        private TAUtil.TADateEditor ToDate;
        private System.Windows.Forms.Panel panel2;

    }
}