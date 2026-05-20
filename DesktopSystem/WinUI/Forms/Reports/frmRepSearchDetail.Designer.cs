namespace WinUI
{
    partial class frmRepSearchDetail
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
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            this.tagrdTrans = new TAUtil.TAGridEditor();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.label16 = new System.Windows.Forms.Label();
            this.DateAvailable = new TAUtil.TADateEditor();
            this.pnlDateFilter = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateAvailable)).BeginInit();
            this.pnlDateFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagrdTrans
            // 
            this.tagrdTrans.ActiveConnection = null;
            this.tagrdTrans.AllowDrop = true;
            this.tagrdTrans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdTrans.AutoAddNewRow = false;
            this.tagrdTrans.AutoUseCustomControlsInCells = true;
            this.tagrdTrans.DefaultValue = null;
            this.tagrdTrans.DetailObjectKey = 0;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdTrans.DisplayLayout.Appearance = appearance19;
            this.tagrdTrans.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdTrans.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance20.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdTrans.DisplayLayout.GroupByBox.Appearance = appearance20;
            appearance21.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdTrans.DisplayLayout.GroupByBox.BandLabelAppearance = appearance21;
            this.tagrdTrans.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance23.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance23.BackColor2 = System.Drawing.SystemColors.Control;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdTrans.DisplayLayout.GroupByBox.PromptAppearance = appearance23;
            this.tagrdTrans.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdTrans.DisplayLayout.MaxRowScrollRegions = 1;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdTrans.DisplayLayout.Override.ActiveCellAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.Gold;
            appearance25.BackColor2 = System.Drawing.Color.White;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.tagrdTrans.DisplayLayout.Override.ActiveRowAppearance = appearance25;
            this.tagrdTrans.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdTrans.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdTrans.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance26.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdTrans.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance17.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance17.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance17.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdTrans.DisplayLayout.Override.CellAppearance = appearance17;
            this.tagrdTrans.DisplayLayout.Override.CellPadding = 0;
            appearance28.BackColor = System.Drawing.Color.LightYellow;
            this.tagrdTrans.DisplayLayout.Override.DataErrorRowAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.ForestGreen;
            this.tagrdTrans.DisplayLayout.Override.DataErrorRowSelectorAppearance = appearance29;
            this.tagrdTrans.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance30.BackColor = System.Drawing.SystemColors.Control;
            appearance30.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance30.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance30.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdTrans.DisplayLayout.Override.GroupByRowAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.AliceBlue;
            appearance31.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance31.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance31.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance31.TextHAlignAsString = "Left";
            this.tagrdTrans.DisplayLayout.Override.HeaderAppearance = appearance31;
            this.tagrdTrans.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdTrans.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.tagrdTrans.DisplayLayout.Override.RowAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.AliceBlue;
            appearance33.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance33.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance33.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdTrans.DisplayLayout.Override.RowSelectorAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.AliceBlue;
            appearance34.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance34.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance34.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdTrans.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance34;
            this.tagrdTrans.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdTrans.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdTrans.DisplayLayout.Override.RowSelectorWidth = 30;
            appearance35.BackColor = System.Drawing.Color.Gold;
            this.tagrdTrans.DisplayLayout.Override.SelectedRowAppearance = appearance35;
            this.tagrdTrans.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdTrans.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdTrans.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdTrans.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.ForeColor = System.Drawing.Color.Black;
            this.tagrdTrans.DisplayLayout.Override.TemplateAddRowCellAppearance = appearance37;
            this.tagrdTrans.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.tagrdTrans.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdTrans.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdTrans.DisplayLayout.UseFixedHeaders = true;
            this.tagrdTrans.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdTrans.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tagrdTrans.HeaderObjectKey = null;
            this.tagrdTrans.Location = new System.Drawing.Point(8, 46);
            this.tagrdTrans.Name = "tagrdTrans";
            this.tagrdTrans.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdTrans.Size = new System.Drawing.Size(602, 344);
            this.tagrdTrans.TabIndex = 0;
            this.tagrdTrans.Text = "taGridEditor1";
            this.tagrdTrans.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdTrans.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdTrans.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.tagrdItms_InitializeLayout);
            this.tagrdTrans.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.tagrdTrans_DoubleClickRow);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.label16.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label16.Location = new System.Drawing.Point(8, 8);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(125, 19);
            this.label16.TabIndex = 31;
            this.label16.Text = "Promised Date <=";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DateAvailable
            // 
            appearance45.TextHAlignAsString = "Right";
            this.DateAvailable.Appearance = appearance45;
            appearance46.Image = global::WinUI.Properties.Resources.calendar3;
            appearance46.ImageHAlign = Infragistics.Win.HAlign.Right;
            editorButton1.Appearance = appearance46;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.DateAvailable.ButtonsRight.Add(editorButton1);
            this.DateAvailable.calendarContainer = null;
            this.DateAvailable.DateValue = null;
            this.DateAvailable.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.DateAvailable.Font = new System.Drawing.Font("Calibri", 11F);
            this.DateAvailable.Format = "";
            this.DateAvailable.Location = new System.Drawing.Point(138, 4);
            this.DateAvailable.MaxLength = 20;
            this.DateAvailable.Name = "DateAvailable";
            this.DateAvailable.Size = new System.Drawing.Size(111, 27);
            this.DateAvailable.TabIndex = 41;
            // 
            // pnlDateFilter
            // 
            this.pnlDateFilter.Controls.Add(this.label16);
            this.pnlDateFilter.Controls.Add(this.DateAvailable);
            this.pnlDateFilter.Location = new System.Drawing.Point(342, 6);
            this.pnlDateFilter.Name = "pnlDateFilter";
            this.pnlDateFilter.Size = new System.Drawing.Size(264, 35);
            this.pnlDateFilter.TabIndex = 42;
            // 
            // frmRepSearchDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.ClientSize = new System.Drawing.Size(618, 396);
            this.Controls.Add(this.pnlDateFilter);
            this.Controls.Add(this.tagrdTrans);
            this.MaximizeBox = false;
            this.Name = "frmRepSearchDetail";
            this.ShowInTaskbar = false;
            this.Text = "Detail";
            this.Load += new System.EventHandler(this.frmRepSearchDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateAvailable)).EndInit();
            this.pnlDateFilter.ResumeLayout(false);
            this.pnlDateFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private TAUtil.TAGridEditor tagrdTrans;
        private System.Windows.Forms.Label label16;
        private TAUtil.TADateEditor DateAvailable;
        private System.Windows.Forms.Panel pnlDateFilter;
    }
}