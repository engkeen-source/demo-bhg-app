namespace WinUI
{
    partial class frmKeyCustomersImport
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
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.cmdBudgetYear = new TAUtil.TAComboBox();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.ExcelPath = new TAUtil.TATextBoxEditor();
            this.ExcelSheets = new TAUtil.TAComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tagrdDocList = new TAUtil.TAGridEditor();
            this.lblmessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdBudgetYear)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelSheets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            this.panel1.Location = new System.Drawing.Point(51, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(528, 5);
            this.panel1.TabIndex = 45;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WinUI.Properties.Resources.Reference;
            this.pictureBox1.Location = new System.Drawing.Point(-1, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 44;
            this.pictureBox1.TabStop = false;
            // 
            // ultraLabel10
            // 
            appearance18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel10.Appearance = appearance18;
            this.ultraLabel10.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel10.Location = new System.Drawing.Point(51, 13);
            this.ultraLabel10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(422, 22);
            this.ultraLabel10.TabIndex = 43;
            this.ultraLabel10.Text = "Key Customer Import";
            // 
            // cmdBudgetYear
            // 
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BorderColor = System.Drawing.Color.LightGray;
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.cmdBudgetYear.Appearance = appearance30;
            this.cmdBudgetYear.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cmdBudgetYear.ComboIsDirty = false;
            this.cmdBudgetYear.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cmdBudgetYear.Font = new System.Drawing.Font("Calibri", 11F);
            this.cmdBudgetYear.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.cmdBudgetYear.LimitToList = true;
            this.cmdBudgetYear.Location = new System.Drawing.Point(127, 24);
            this.cmdBudgetYear.Name = "cmdBudgetYear";
            this.cmdBudgetYear.Size = new System.Drawing.Size(239, 26);
            this.cmdBudgetYear.TabIndex = 46;
            this.cmdBudgetYear.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.cmdBudgetYear.UserInputText = "";
            // 
            // ultraLabel4
            // 
            appearance48.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance48.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance48;
            this.ultraLabel4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel4.Location = new System.Drawing.Point(22, 28);
            this.ultraLabel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(95, 22);
            this.ultraLabel4.TabIndex = 47;
            this.ultraLabel4.Text = "Budget Year";
            this.ultraLabel4.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(111, 547);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 25);
            this.btnClose.TabIndex = 49;
            this.btnClose.Text = "&Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnAppend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAppend.Image = global::WinUI.Properties.Resources.ok_16;
            this.btnAppend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppend.Location = new System.Drawing.Point(5, 547);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(100, 25);
            this.btnAppend.TabIndex = 48;
            this.btnAppend.Text = "&OK";
            this.btnAppend.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.ExcelPath);
            this.panel2.Controls.Add(this.cmdBudgetYear);
            this.panel2.Controls.Add(this.ultraLabel4);
            this.panel2.Controls.Add(this.ExcelSheets);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(5, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 125);
            this.panel2.TabIndex = 50;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label8.Location = new System.Drawing.Point(-3, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(571, 17);
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
            editorButton1.Appearance = appearance3;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007RibbonButton;
            editorButton1.Text = "...";
            this.ExcelPath.ButtonsRight.Add(editorButton1);
            this.ExcelPath.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ExcelPath.Font = new System.Drawing.Font("Calibri", 10F);
            this.ExcelPath.Format = "";
            this.ExcelPath.IsDirty = false;
            this.ExcelPath.IsEmailTextBox = false;
            this.ExcelPath.Location = new System.Drawing.Point(127, 53);
            this.ExcelPath.Multiline = true;
            this.ExcelPath.Name = "ExcelPath";
            this.ExcelPath.Size = new System.Drawing.Size(424, 26);
            this.ExcelPath.TabIndex = 0;
            this.ExcelPath.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ExcelPath.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.ExcelPath_EditorButtonClick);
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
            this.ExcelSheets.Location = new System.Drawing.Point(127, 84);
            this.ExcelSheets.Name = "ExcelSheets";
            this.ExcelSheets.Size = new System.Drawing.Size(239, 25);
            this.ExcelSheets.TabIndex = 1;
            this.ExcelSheets.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ExcelSheets.UserInputText = "";
            this.ExcelSheets.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.ExcelSheets_RowSelected);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(19, 85);
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
            this.label4.Location = new System.Drawing.Point(19, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Excel File Path* :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.tagrdDocList.Location = new System.Drawing.Point(5, 205);
            this.tagrdDocList.Name = "tagrdDocList";
            this.tagrdDocList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDocList.Size = new System.Drawing.Size(568, 336);
            this.tagrdDocList.TabIndex = 51;
            this.tagrdDocList.Text = "taGridEditor1";
            this.tagrdDocList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblmessage
            // 
            this.lblmessage.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblmessage.Location = new System.Drawing.Point(2, 185);
            this.lblmessage.Name = "lblmessage";
            this.lblmessage.Size = new System.Drawing.Size(571, 17);
            this.lblmessage.TabIndex = 52;
            this.lblmessage.Text = "Rows in the following grid can not be imported into Key Customer because these ke" +
    "y customers are not in Customer Master.";
            this.lblmessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblmessage.Visible = false;
            // 
            // frmKeyCustomersImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(579, 575);
            this.Controls.Add(this.lblmessage);
            this.Controls.Add(this.tagrdDocList);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAppend);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ultraLabel10);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "frmKeyCustomersImport";
            this.Text = "frmKeyCustomersImport";
            this.Load += new System.EventHandler(this.frmKeyCustomersImport_Load);
            this.Shown += new System.EventHandler(this.frmKeyCustomersImport_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdBudgetYear)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelSheets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        private TAUtil.TAComboBox cmdBudgetYear;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private TAUtil.TATextBoxEditor ExcelPath;
        private TAUtil.TAComboBox ExcelSheets;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private TAUtil.TAGridEditor tagrdDocList;
        private System.Windows.Forms.Label lblmessage;
    }
}