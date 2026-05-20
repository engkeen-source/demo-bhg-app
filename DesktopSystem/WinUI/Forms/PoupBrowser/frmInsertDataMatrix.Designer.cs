namespace WinUI
{
    partial class frmInsertDataMatrix
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
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.label7 = new System.Windows.Forms.Label();
            this.ItmKey = new TAUtil.TAComboBox();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tagrdItmSelected = new TAUtil.TAGridEditor();
            this.tagrdMasterItmList = new TAUtil.TAGridEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAppend = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ItmKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItmSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdMasterItmList)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(9, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(210, 25);
            this.label7.TabIndex = 0;
            this.label7.Text = "Master List                        Go To:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ItmKey
            // 
            appearance2.BorderColor = System.Drawing.Color.LightGray;
            this.ItmKey.Appearance = appearance2;
            this.ItmKey.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.ItmKey.AutoSize = false;
            this.ItmKey.ComboIsDirty = false;
            this.ItmKey.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ItmKey.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDown;
            this.ItmKey.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItmKey.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.ItmKey.Location = new System.Drawing.Point(225, 19);
            this.ItmKey.Name = "ItmKey";
            this.ItmKey.Size = new System.Drawing.Size(300, 25);
            this.ItmKey.TabIndex = 1;
            this.ItmKey.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.ItmKey.UserInputText = "";
            this.ItmKey.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInListAdd);
            this.ItmKey.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.ItmKey_EditorButtonClick);
            this.ItmKey.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.ItmKey_CustomUpdate);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(196, 74);
            // 
            // tagrdItmSelected
            // 
            this.tagrdItmSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdItmSelected.AutoAddNewRow = false;
            this.tagrdItmSelected.AutoUseCustomControlsInCells = false;
            this.tagrdItmSelected.DefaultValue = null;
            this.tagrdItmSelected.DetailObjectKey = 0;
            appearance44.BackColor = System.Drawing.SystemColors.Window;
            appearance44.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdItmSelected.DisplayLayout.Appearance = appearance44;
            this.tagrdItmSelected.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdItmSelected.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance45.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdItmSelected.DisplayLayout.GroupByBox.Appearance = appearance45;
            appearance46.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItmSelected.DisplayLayout.GroupByBox.BandLabelAppearance = appearance46;
            this.tagrdItmSelected.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdItmSelected.DisplayLayout.GroupByBox.Hidden = true;
            appearance47.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance47.BackColor2 = System.Drawing.SystemColors.Control;
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance47.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItmSelected.DisplayLayout.GroupByBox.PromptAppearance = appearance47;
            this.tagrdItmSelected.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdItmSelected.DisplayLayout.MaxRowScrollRegions = 1;
            appearance48.BackColor = System.Drawing.SystemColors.Window;
            appearance48.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdItmSelected.DisplayLayout.Override.ActiveCellAppearance = appearance48;
            this.tagrdItmSelected.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdItmSelected.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance50.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdItmSelected.DisplayLayout.Override.CardAreaAppearance = appearance50;
            appearance51.BorderColor = System.Drawing.Color.Silver;
            appearance51.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdItmSelected.DisplayLayout.Override.CellAppearance = appearance51;
            this.tagrdItmSelected.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdItmSelected.DisplayLayout.Override.CellPadding = 0;
            this.tagrdItmSelected.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance52.BackColor = System.Drawing.SystemColors.Control;
            appearance52.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance52.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance52.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdItmSelected.DisplayLayout.Override.GroupByRowAppearance = appearance52;
            appearance53.TextHAlignAsString = "Left";
            this.tagrdItmSelected.DisplayLayout.Override.HeaderAppearance = appearance53;
            this.tagrdItmSelected.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdItmSelected.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance54.BackColor = System.Drawing.SystemColors.Window;
            appearance54.BorderColor = System.Drawing.Color.Silver;
            this.tagrdItmSelected.DisplayLayout.Override.RowAppearance = appearance54;
            this.tagrdItmSelected.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItmSelected.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance55.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdItmSelected.DisplayLayout.Override.TemplateAddRowAppearance = appearance55;
            this.tagrdItmSelected.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdItmSelected.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdItmSelected.DisplayLayout.UseFixedHeaders = true;
            this.tagrdItmSelected.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.tagrdItmSelected.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdItmSelected.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdItmSelected.HeaderObjectKey = null;
            this.tagrdItmSelected.Location = new System.Drawing.Point(12, 265);
            this.tagrdItmSelected.Name = "tagrdItmSelected";
            this.tagrdItmSelected.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdItmSelected.Size = new System.Drawing.Size(546, 206);
            this.tagrdItmSelected.TabIndex = 2;
            this.tagrdItmSelected.Text = "taGridEditor1";
            this.tagrdItmSelected.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItmSelected.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // tagrdMasterItmList
            // 
            this.tagrdMasterItmList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdMasterItmList.AutoAddNewRow = false;
            this.tagrdMasterItmList.AutoUseCustomControlsInCells = false;
            this.tagrdMasterItmList.DefaultValue = null;
            this.tagrdMasterItmList.DetailObjectKey = 0;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdMasterItmList.DisplayLayout.Appearance = appearance5;
            this.tagrdMasterItmList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdMasterItmList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdMasterItmList.DisplayLayout.GroupByBox.Appearance = appearance6;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdMasterItmList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.tagrdMasterItmList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdMasterItmList.DisplayLayout.GroupByBox.Hidden = true;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdMasterItmList.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.tagrdMasterItmList.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdMasterItmList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdMasterItmList.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance33.BackColor = System.Drawing.SystemColors.Highlight;
            appearance33.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tagrdMasterItmList.DisplayLayout.Override.ActiveRowAppearance = appearance33;
            this.tagrdMasterItmList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdMasterItmList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance34.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdMasterItmList.DisplayLayout.Override.CardAreaAppearance = appearance34;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            appearance35.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdMasterItmList.DisplayLayout.Override.CellAppearance = appearance35;
            this.tagrdMasterItmList.DisplayLayout.Override.CellPadding = 0;
            this.tagrdMasterItmList.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance36.BackColor = System.Drawing.SystemColors.Control;
            appearance36.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance36.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance36.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdMasterItmList.DisplayLayout.Override.GroupByRowAppearance = appearance36;
            appearance37.TextHAlignAsString = "Left";
            this.tagrdMasterItmList.DisplayLayout.Override.HeaderAppearance = appearance37;
            this.tagrdMasterItmList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdMasterItmList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance38.BackColor = System.Drawing.SystemColors.Window;
            appearance38.BorderColor = System.Drawing.Color.Silver;
            this.tagrdMasterItmList.DisplayLayout.Override.RowAppearance = appearance38;
            this.tagrdMasterItmList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdMasterItmList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdMasterItmList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance39.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdMasterItmList.DisplayLayout.Override.TemplateAddRowAppearance = appearance39;
            this.tagrdMasterItmList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdMasterItmList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdMasterItmList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdMasterItmList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.tagrdMasterItmList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdMasterItmList.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdMasterItmList.HeaderObjectKey = null;
            this.tagrdMasterItmList.Location = new System.Drawing.Point(12, 78);
            this.tagrdMasterItmList.Name = "tagrdMasterItmList";
            this.tagrdMasterItmList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdMasterItmList.Size = new System.Drawing.Size(546, 181);
            this.tagrdMasterItmList.TabIndex = 1;
            this.tagrdMasterItmList.Text = "taGridEditor1";
            this.tagrdMasterItmList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdMasterItmList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdMasterItmList.CustomCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.tagrdMasterItmList_CustomCellUpdate);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.ItmKey);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(546, 60);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel2.Controls.Add(this.btnAppend);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(12, 477);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(546, 45);
            this.panel2.TabIndex = 3;
            // 
            // btnAppend
            // 
            appearance11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance11.Image = global::WinUI.Properties.Resources.OK;
            this.btnAppend.Appearance = appearance11;
            this.btnAppend.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAppend.Location = new System.Drawing.Point(347, 7);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(103, 30);
            this.btnAppend.TabIndex = 0;
            this.btnAppend.Text = "A&ppend";
            this.btnAppend.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnCancel
            // 
            appearance8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance8.Image = global::WinUI.Properties.Resources.Cancel_16;
            this.btnCancel.Appearance = appearance8;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(456, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmInsertDataMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(570, 531);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tagrdMasterItmList);
            this.Controls.Add(this.tagrdItmSelected);
            this.Name = "frmInsertDataMatrix";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Insert Data Matrix";
            this.Load += new System.EventHandler(this.frmInsertDataMatrix_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ItmKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItmSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdMasterItmList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private TAUtil.TAComboBox ItmKey;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private TAUtil.TAGridEditor tagrdItmSelected;
        private TAUtil.TAGridEditor tagrdMasterItmList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnAppend;
    }
}