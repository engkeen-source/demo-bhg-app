namespace WinUI
{
    partial class frmItmPrevPrice
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.btnRequery = new System.Windows.Forms.Button();
            this.ItmID = new TAUtil.TATextBoxEditor();
            this.tagrdItm = new TAUtil.TAGridEditor();
            this.ToDate = new TAUtil.TADateEditor();
            this.FromDate = new TAUtil.TADateEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblItmDes = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ItmID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDate)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance2.TextVAlignAsString = "Middle";
            this.label2.Appearance = appearance2;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label2.Location = new System.Drawing.Point(22, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 22);
            this.label2.TabIndex = 127;
            this.label2.Text = "Date From/ To";
            this.label2.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // btnRequery
            // 
            this.btnRequery.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnRequery.Image = global::WinUI.Properties.Resources.refresh;
            this.btnRequery.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRequery.Location = new System.Drawing.Point(340, 10);
            this.btnRequery.Name = "btnRequery";
            this.btnRequery.Size = new System.Drawing.Size(82, 26);
            this.btnRequery.TabIndex = 2;
            this.btnRequery.Text = "Refresh";
            this.btnRequery.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRequery.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRequery.UseVisualStyleBackColor = true;
            this.btnRequery.Click += new System.EventHandler(this.btnRequery_Click);
            // 
            // ItmID
            // 
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BorderColor = System.Drawing.Color.LightGray;
            appearance25.FontData.Name = "Calibri";
            appearance25.FontData.SizeInPoints = 11F;
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.ItmID.Appearance = appearance25;
            this.ItmID.AutoSize = false;
            this.ItmID.BackColor = System.Drawing.Color.White;
            appearance34.Image = global::WinUI.Properties.Resources.open3;
            appearance34.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance34;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            this.ItmID.ButtonsRight.Add(editorButton1);
            this.ItmID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ItmID.Font = new System.Drawing.Font("Calibri", 11F);
            this.ItmID.Format = "";
            this.ItmID.IsDirty = false;
            this.ItmID.IsEmailTextBox = false;
            this.ItmID.Location = new System.Drawing.Point(120, 37);
            this.ItmID.Multiline = true;
            this.ItmID.Name = "ItmID";
            this.ItmID.Size = new System.Drawing.Size(105, 25);
            this.ItmID.TabIndex = 3;
            this.ItmID.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ItmID.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.ItmID_CustomUpdate);
            this.ItmID.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.ItmID_EditorButtonClick);
            // 
            // tagrdItm
            // 
            this.tagrdItm.ActiveConnection = null;
            this.tagrdItm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdItm.AutoAddNewRow = false;
            this.tagrdItm.AutoUseCustomControlsInCells = false;
            this.tagrdItm.DefaultValue = null;
            this.tagrdItm.DetailObjectKey = 0;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdItm.DisplayLayout.Appearance = appearance11;
            this.tagrdItm.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdItm.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance8.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdItm.DisplayLayout.GroupByBox.Appearance = appearance8;
            appearance9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItm.DisplayLayout.GroupByBox.BandLabelAppearance = appearance9;
            this.tagrdItm.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdItm.DisplayLayout.GroupByBox.Hidden = true;
            appearance10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance10.BackColor2 = System.Drawing.SystemColors.Control;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItm.DisplayLayout.GroupByBox.PromptAppearance = appearance10;
            this.tagrdItm.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdItm.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdItm.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance14.BackColor = System.Drawing.SystemColors.Highlight;
            appearance14.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tagrdItm.DisplayLayout.Override.ActiveRowAppearance = appearance14;
            this.tagrdItm.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdItm.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdItm.DisplayLayout.Override.CardAreaAppearance = appearance13;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            appearance12.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdItm.DisplayLayout.Override.CellAppearance = appearance12;
            this.tagrdItm.DisplayLayout.Override.CellPadding = 0;
            this.tagrdItm.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance16.BackColor = System.Drawing.SystemColors.Control;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdItm.DisplayLayout.Override.GroupByRowAppearance = appearance16;
            appearance18.TextHAlignAsString = "Left";
            this.tagrdItm.DisplayLayout.Override.HeaderAppearance = appearance18;
            this.tagrdItm.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdItm.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.BorderColor = System.Drawing.Color.Silver;
            this.tagrdItm.DisplayLayout.Override.RowAppearance = appearance17;
            this.tagrdItm.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItm.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItm.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdItm.DisplayLayout.Override.TemplateAddRowAppearance = appearance15;
            this.tagrdItm.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdItm.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdItm.DisplayLayout.UseFixedHeaders = true;
            this.tagrdItm.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.tagrdItm.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdItm.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdItm.HeaderObjectKey = null;
            this.tagrdItm.Location = new System.Drawing.Point(2, 84);
            this.tagrdItm.Name = "tagrdItm";
            this.tagrdItm.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdItm.Size = new System.Drawing.Size(782, 321);
            this.tagrdItm.TabIndex = 1;
            this.tagrdItm.Text = "taGridEditor1";
            this.tagrdItm.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItm.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ToDate
            // 
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BorderColor = System.Drawing.Color.LightGray;
            appearance20.FontData.Name = "Calibri";
            appearance20.FontData.SizeInPoints = 11F;
            appearance20.ForeColor = System.Drawing.Color.Black;
            appearance20.TextHAlignAsString = "Right";
            this.ToDate.Appearance = appearance20;
            this.ToDate.AutoSize = false;
            this.ToDate.BackColor = System.Drawing.Color.White;
            appearance5.Image = global::WinUI.Properties.Resources.calendar3;
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton2.Appearance = appearance5;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.ToDate.ButtonsRight.Add(editorButton2);
            this.ToDate.calendarContainer = null;
            this.ToDate.DateValue = null;
            this.ToDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ToDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.ToDate.Format = "";
            this.ToDate.Location = new System.Drawing.Point(230, 10);
            this.ToDate.MaxLength = 20;
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new System.Drawing.Size(104, 25);
            this.ToDate.TabIndex = 1;
            this.ToDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ToDate.CustomDataError += new TAUtil.TADataErrorEventHandler(this.CustomDataError);
            // 
            // FromDate
            // 
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BorderColor = System.Drawing.Color.LightGray;
            appearance21.FontData.Name = "Calibri";
            appearance21.FontData.SizeInPoints = 11F;
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Right";
            this.FromDate.Appearance = appearance21;
            this.FromDate.AutoSize = false;
            this.FromDate.BackColor = System.Drawing.Color.White;
            appearance4.Image = global::WinUI.Properties.Resources.calendar3;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton3.Appearance = appearance4;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.FromDate.ButtonsRight.Add(editorButton3);
            this.FromDate.calendarContainer = null;
            this.FromDate.DateValue = null;
            this.FromDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.FromDate.Font = new System.Drawing.Font("Calibri", 11F);
            this.FromDate.Format = "";
            this.FromDate.Location = new System.Drawing.Point(120, 10);
            this.FromDate.MaxLength = 20;
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(104, 25);
            this.FromDate.TabIndex = 0;
            this.FromDate.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.FromDate.CustomDataError += new TAUtil.TADataErrorEventHandler(this.CustomDataError);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.lblItmDes);
            this.panel1.Controls.Add(this.ultraLabel1);
            this.panel1.Controls.Add(this.btnRequery);
            this.panel1.Controls.Add(this.FromDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ToDate);
            this.panel1.Controls.Add(this.ItmID);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 77);
            this.panel1.TabIndex = 0;
            // 
            // lblItmDes
            // 
            appearance28.BackColor = System.Drawing.Color.Transparent;
            appearance28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance28.TextVAlignAsString = "Middle";
            this.lblItmDes.Appearance = appearance28;
            this.lblItmDes.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lblItmDes.Location = new System.Drawing.Point(237, 40);
            this.lblItmDes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblItmDes.Name = "lblItmDes";
            this.lblItmDes.Size = new System.Drawing.Size(530, 22);
            this.lblItmDes.TabIndex = 135;
            this.lblItmDes.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // ultraLabel1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ultraLabel1.Location = new System.Drawing.Point(22, 37);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(93, 22);
            this.ultraLabel1.TabIndex = 134;
            this.ultraLabel1.Text = "Item ID/ Des";
            this.ultraLabel1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // frmItmPrevPrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(789, 415);
            this.Controls.Add(this.tagrdItm);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frmItmPrevPrice";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmItmPrevPrice";
            this.Text = "Search For Previous Items Price";
            this.Load += new System.EventHandler(this.frmItemHistory_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItmPrevPrice_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ItmID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FromDate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TAUtil.TADateEditor ToDate;
        private TAUtil.TADateEditor FromDate;
        private Infragistics.Win.Misc.UltraLabel label2;
        private TAUtil.TAGridEditor tagrdItm;
        private TAUtil.TATextBoxEditor ItmID;
        private System.Windows.Forms.Button btnRequery;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblItmDes;

    }
}