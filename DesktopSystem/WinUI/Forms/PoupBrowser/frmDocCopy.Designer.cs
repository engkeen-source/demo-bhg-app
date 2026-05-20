namespace WinUI
{
    partial class frmDocCopy
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.QtyDesLabel = new System.Windows.Forms.Label();
            this.btnAppend = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.DocCode = new TAUtil.TAComboBox();
            this.StartDate = new TAUtil.TADateEditor();
            this.EndDate = new TAUtil.TADateEditor();
            this.tagrdDocList = new TAUtil.TAGridEditor();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.CopyFrom = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRequery = new Infragistics.Win.Misc.UltraButton();
            this.label7 = new System.Windows.Forms.Label();
            this.NSLink = new TAUtil.TACheckBoxEditor();
            this.CopyAs = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.Import = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDownload = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.ExcelPath = new TAUtil.TATextBoxEditor();
            this.defaultItem = new TAUtil.TAComboBox();
            this.ExcelSheets = new TAUtil.TAComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DocCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.CopyFrom.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NSLink)).BeginInit();
            this.CopyAs.SuspendLayout();
            this.Import.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelSheets)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Document Code :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Range :";
            // 
            // QtyDesLabel
            // 
            this.QtyDesLabel.AutoSize = true;
            this.QtyDesLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.QtyDesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.QtyDesLabel.Location = new System.Drawing.Point(275, 73);
            this.QtyDesLabel.Name = "QtyDesLabel";
            this.QtyDesLabel.Size = new System.Drawing.Size(28, 17);
            this.QtyDesLabel.TabIndex = 5;
            this.QtyDesLabel.Text = "To :";
            // 
            // btnAppend
            // 
            this.btnAppend.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnAppend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAppend.Image = global::WinUI.Properties.Resources.ok_16;
            this.btnAppend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppend.Location = new System.Drawing.Point(365, 446);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(100, 25);
            this.btnAppend.TabIndex = 1;
            this.btnAppend.Text = "&OK";
            this.btnAppend.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(471, 446);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DocCode
            // 
            appearance7.BorderColor = System.Drawing.Color.LightGray;
            this.DocCode.Appearance = appearance7;
            this.DocCode.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.DocCode.AutoSize = false;
            this.DocCode.ComboIsDirty = false;
            this.DocCode.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.DocCode.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.DocCode.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DocCode.Font = new System.Drawing.Font("Calibri", 11F);
            this.DocCode.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.DocCode.Location = new System.Drawing.Point(136, 43);
            this.DocCode.Name = "DocCode";
            this.DocCode.Size = new System.Drawing.Size(304, 25);
            this.DocCode.TabIndex = 0;
            this.DocCode.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.DocCode.UserInputText = "OK";
            this.DocCode.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.Criteria_CustomUpdate);
            this.DocCode.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_ItemNotInList);
            // 
            // StartDate
            // 
            appearance5.BorderColor = System.Drawing.Color.LightGray;
            appearance5.TextHAlignAsString = "Right";
            this.StartDate.Appearance = appearance5;
            this.StartDate.AutoSize = false;
            appearance2.Image = global::WinUI.Properties.Resources.calendar3;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance2;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.StartDate.ButtonsRight.Add(editorButton1);
            this.StartDate.calendarContainer = null;
            this.StartDate.DateValue = null;
            this.StartDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.StartDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.StartDate.Format = "";
            this.StartDate.Location = new System.Drawing.Point(136, 68);
            this.StartDate.MaxLength = 20;
            this.StartDate.Name = "StartDate";
            this.StartDate.Size = new System.Drawing.Size(136, 25);
            this.StartDate.TabIndex = 1;
            this.StartDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.StartDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.Criteria_CustomUpdate);
            // 
            // EndDate
            // 
            appearance6.BorderColor = System.Drawing.Color.LightGray;
            appearance6.TextHAlignAsString = "Right";
            this.EndDate.Appearance = appearance6;
            this.EndDate.AutoSize = false;
            appearance1.Image = global::WinUI.Properties.Resources.calendar3;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton2.Appearance = appearance1;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.EndDate.ButtonsRight.Add(editorButton2);
            this.EndDate.calendarContainer = null;
            this.EndDate.DateValue = null;
            this.EndDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.EndDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.EndDate.Format = "";
            this.EndDate.Location = new System.Drawing.Point(304, 68);
            this.EndDate.MaxLength = 20;
            this.EndDate.Name = "EndDate";
            this.EndDate.Size = new System.Drawing.Size(136, 25);
            this.EndDate.TabIndex = 2;
            this.EndDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.EndDate.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.Criteria_CustomUpdate);
            // 
            // tagrdDocList
            // 
            this.tagrdDocList.ActiveConnection = null;
            this.tagrdDocList.AutoAddNewRow = false;
            this.tagrdDocList.AutoUseCustomControlsInCells = false;
            this.tagrdDocList.DefaultValue = null;
            this.tagrdDocList.DetailObjectKey = 0;
            appearance15.BackColor = System.Drawing.SystemColors.Window;
            appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdDocList.DisplayLayout.Appearance = appearance15;
            this.tagrdDocList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdDocList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDocList.DisplayLayout.GroupByBox.Appearance = appearance16;
            appearance26.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance26;
            this.tagrdDocList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdDocList.DisplayLayout.GroupByBox.Hidden = true;
            appearance32.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance32.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDocList.DisplayLayout.GroupByBox.PromptAppearance = appearance32;
            this.tagrdDocList.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdDocList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance33.BackColor = System.Drawing.SystemColors.Window;
            appearance33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdDocList.DisplayLayout.Override.ActiveCellAppearance = appearance33;
            appearance34.BackColor = System.Drawing.SystemColors.Highlight;
            appearance34.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tagrdDocList.DisplayLayout.Override.ActiveRowAppearance = appearance34;
            this.tagrdDocList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDocList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdDocList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdDocList.DisplayLayout.Override.CardAreaAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.Silver;
            appearance36.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdDocList.DisplayLayout.Override.CellAppearance = appearance36;
            this.tagrdDocList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdDocList.DisplayLayout.Override.CellPadding = 0;
            this.tagrdDocList.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance37.BackColor = System.Drawing.SystemColors.Control;
            appearance37.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance37.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance37.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDocList.DisplayLayout.Override.GroupByRowAppearance = appearance37;
            appearance38.TextHAlignAsString = "Left";
            this.tagrdDocList.DisplayLayout.Override.HeaderAppearance = appearance38;
            this.tagrdDocList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdDocList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance39.BackColor = System.Drawing.SystemColors.Window;
            appearance39.BorderColor = System.Drawing.Color.Silver;
            this.tagrdDocList.DisplayLayout.Override.RowAppearance = appearance39;
            this.tagrdDocList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdDocList.DisplayLayout.Override.TemplateAddRowAppearance = appearance40;
            this.tagrdDocList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDocList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDocList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDocList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdDocList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDocList.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDocList.HeaderObjectKey = null;
            this.tagrdDocList.Location = new System.Drawing.Point(9, 147);
            this.tagrdDocList.Name = "tagrdDocList";
            this.tagrdDocList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDocList.Size = new System.Drawing.Size(586, 261);
            this.tagrdDocList.TabIndex = 1;
            this.tagrdDocList.Text = "taGridEditor1";
            this.tagrdDocList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDocList.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.tagrdDocList_DoubleClickRow);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.CopyFrom);
            this.tabControl1.Controls.Add(this.CopyAs);
            this.tabControl1.Controls.Add(this.Import);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(609, 441);
            this.tabControl1.TabIndex = 0;
            // 
            // CopyFrom
            // 
            this.CopyFrom.Controls.Add(this.panel1);
            this.CopyFrom.Controls.Add(this.tagrdDocList);
            this.CopyFrom.Location = new System.Drawing.Point(4, 23);
            this.CopyFrom.Name = "CopyFrom";
            this.CopyFrom.Padding = new System.Windows.Forms.Padding(3);
            this.CopyFrom.Size = new System.Drawing.Size(601, 414);
            this.CopyFrom.TabIndex = 1;
            this.CopyFrom.Text = "Copy From";
            this.CopyFrom.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.btnRequery);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.NSLink);
            this.panel1.Controls.Add(this.QtyDesLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.EndDate);
            this.panel1.Controls.Add(this.DocCode);
            this.panel1.Controls.Add(this.StartDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 123);
            this.panel1.TabIndex = 0;
            // 
            // btnRequery
            // 
            appearance8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance8.Image = global::WinUI.Properties.Resources.refresh;
            this.btnRequery.Appearance = appearance8;
            this.btnRequery.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnRequery.Location = new System.Drawing.Point(469, 43);
            this.btnRequery.Name = "btnRequery";
            this.btnRequery.Size = new System.Drawing.Size(91, 25);
            this.btnRequery.TabIndex = 4;
            this.btnRequery.Text = "&Refresh";
            this.btnRequery.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnRequery.Click += new System.EventHandler(this.btnRequery_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(145, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(283, 19);
            this.label7.TabIndex = 0;
            this.label7.Text = "Create a new copy from another documents";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NSLink
            // 
            appearance4.FontData.ItalicAsString = "True";
            appearance4.FontData.Name = "Calibri";
            appearance4.FontData.SizeInPoints = 10F;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.TextHAlignAsString = "Left";
            this.NSLink.Appearance = appearance4;
            this.NSLink.cancelUpdate = false;
            this.NSLink.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NSLink.Location = new System.Drawing.Point(221, 99);
            this.NSLink.Name = "NSLink";
            this.NSLink.Size = new System.Drawing.Size(219, 17);
            this.NSLink.TabIndex = 3;
            this.NSLink.Text = "Include Non Stock Costing Link";
            this.NSLink.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // CopyAs
            // 
            this.CopyAs.Controls.Add(this.label6);
            this.CopyAs.Location = new System.Drawing.Point(4, 23);
            this.CopyAs.Name = "CopyAs";
            this.CopyAs.Padding = new System.Windows.Forms.Padding(3);
            this.CopyAs.Size = new System.Drawing.Size(601, 414);
            this.CopyAs.TabIndex = 0;
            this.CopyAs.Text = "Copy MySelf";
            this.CopyAs.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(3, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(592, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "Create a new copy from the current document?";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Import
            // 
            this.Import.Controls.Add(this.panel2);
            this.Import.Location = new System.Drawing.Point(4, 23);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(601, 414);
            this.Import.TabIndex = 2;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel2.Controls.Add(this.btnDownload);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.ExcelPath);
            this.panel2.Controls.Add(this.defaultItem);
            this.panel2.Controls.Add(this.ExcelSheets);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(15, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 128);
            this.panel2.TabIndex = 0;
            // 
            // btnDownload
            // 
            this.btnDownload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDownload.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.btnDownload.Image = global::WinUI.Properties.Resources.download_01_16_png;
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.Location = new System.Drawing.Point(369, 62);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(181, 41);
            this.btnDownload.TabIndex = 476;
            this.btnDownload.Text = "Download Excel Template";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label8.Location = new System.Drawing.Point(11, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(543, 21);
            this.label8.TabIndex = 0;
            this.label8.Text = "This action will import items from  Excel file into the current document";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ExcelPath
            // 
            appearance9.BorderColor = System.Drawing.Color.LightGray;
            this.ExcelPath.Appearance = appearance9;
            this.ExcelPath.AutoSize = false;
            appearance3.Image = global::WinUI.Properties.Resources.open3;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Left;
            editorButton3.Appearance = appearance3;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007RibbonButton;
            editorButton3.Text = "...";
            this.ExcelPath.ButtonsRight.Add(editorButton3);
            this.ExcelPath.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ExcelPath.Font = new System.Drawing.Font("Calibri", 10F);
            this.ExcelPath.Format = "";
            this.ExcelPath.IsDirty = false;
            this.ExcelPath.IsEmailTextBox = false;
            this.ExcelPath.Location = new System.Drawing.Point(126, 33);
            this.ExcelPath.Multiline = true;
            this.ExcelPath.Name = "ExcelPath";
            this.ExcelPath.Size = new System.Drawing.Size(424, 25);
            this.ExcelPath.TabIndex = 0;
            this.ExcelPath.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ExcelPath.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.ExcelPath_EditorButtonClick);
            // 
            // defaultItem
            // 
            appearance11.BorderColor = System.Drawing.Color.LightGray;
            this.defaultItem.Appearance = appearance11;
            this.defaultItem.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.defaultItem.AutoSize = false;
            this.defaultItem.ComboIsDirty = false;
            this.defaultItem.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.defaultItem.Font = new System.Drawing.Font("Calibri", 10F);
            this.defaultItem.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.defaultItem.Location = new System.Drawing.Point(126, 83);
            this.defaultItem.Name = "defaultItem";
            this.defaultItem.Size = new System.Drawing.Size(233, 25);
            this.defaultItem.TabIndex = 2;
            this.defaultItem.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.defaultItem.UserInputText = "";
            this.defaultItem.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_ItemNotInList);
            // 
            // ExcelSheets
            // 
            appearance10.BorderColor = System.Drawing.Color.LightGray;
            this.ExcelSheets.Appearance = appearance10;
            this.ExcelSheets.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.ExcelSheets.AutoSize = false;
            this.ExcelSheets.ComboIsDirty = false;
            this.ExcelSheets.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ExcelSheets.Font = new System.Drawing.Font("Calibri", 10F);
            this.ExcelSheets.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.ExcelSheets.Location = new System.Drawing.Point(126, 58);
            this.ExcelSheets.Name = "ExcelSheets";
            this.ExcelSheets.Size = new System.Drawing.Size(233, 25);
            this.ExcelSheets.TabIndex = 1;
            this.ExcelSheets.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ExcelSheets.UserInputText = "";
            this.ExcelSheets.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_ItemNotInList);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(18, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "Default Item :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(18, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Worksheet :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(18, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Excel File Path* :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmDocCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(614, 477);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAppend);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(630, 511);
            this.Name = "frmDocCopy";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add By Document";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDocCopy_OnClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDocCopy_FormClosed);
            this.Load += new System.EventHandler(this.frmDocCopy_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocCopy_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.DocCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.CopyFrom.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NSLink)).EndInit();
            this.CopyAs.ResumeLayout(false);
            this.Import.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelSheets)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label QtyDesLabel;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.Button btnClose;
        private TAUtil.TAComboBox DocCode;
        private TAUtil.TADateEditor StartDate;
        private TAUtil.TADateEditor EndDate;
        private TAUtil.TAGridEditor tagrdDocList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage CopyAs;
        private System.Windows.Forms.TabPage CopyFrom;
        private System.Windows.Forms.TabPage Import;
        private TAUtil.TATextBoxEditor ExcelPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private TAUtil.TAComboBox ExcelSheets;
        private TAUtil.TAComboBox defaultItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private TAUtil.TACheckBoxEditor NSLink;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnRequery;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDownload;
    }
}