namespace WinUI
{
    partial class frmGeneratePO
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
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("BindingList`1", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocVendorKey", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocCurrKey", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocVendorID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocVendorNm", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DocID", 4);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ultraLabel16 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.bntOK = new Infragistics.Win.Misc.UltraButton();
            this.tagrdGeneratePO = new TAUtil.TAGridEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdGeneratePO)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel16
            // 
            appearance17.BackColor = System.Drawing.Color.Transparent;
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabel16.Appearance = appearance17;
            this.ultraLabel16.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)
                            | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel16.Location = new System.Drawing.Point(14, 11);
            this.ultraLabel16.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel16.Name = "ultraLabel16";
            this.ultraLabel16.Size = new System.Drawing.Size(346, 22);
            this.ultraLabel16.TabIndex = 0;
            this.ultraLabel16.Text = "Following Vendor PO will be generated";
            this.ultraLabel16.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // btnClose
            // 
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance1.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.Appearance = appearance1;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(328, 309);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // bntOK
            // 
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.Image = global::WinUI.Properties.Resources.ok_16;
            this.bntOK.Appearance = appearance3;
            this.bntOK.Font = new System.Drawing.Font("Calibri", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntOK.Location = new System.Drawing.Point(250, 309);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(72, 30);
            this.bntOK.TabIndex = 2;
            this.bntOK.Text = "Ok";
            this.bntOK.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
            // 
            // tagrdGeneratePO
            // 
            this.tagrdGeneratePO.AllowDrop = true;
            this.tagrdGeneratePO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdGeneratePO.AutoAddNewRow = false;
            this.tagrdGeneratePO.AutoUseCustomControlsInCells = true;
            this.tagrdGeneratePO.DefaultValue = null;
            this.tagrdGeneratePO.DetailObjectKey = 0;
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdGeneratePO.DisplayLayout.Appearance = appearance4;
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
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            ultraGridBand1.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            ultraGridBand1.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            ultraGridBand1.Override.RowSelectorWidth = 30;
            this.tagrdGeneratePO.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdGeneratePO.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdGeneratePO.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdGeneratePO.DisplayLayout.GroupByBox.Appearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdGeneratePO.DisplayLayout.GroupByBox.BandLabelAppearance = appearance6;
            this.tagrdGeneratePO.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance7.BackColor2 = System.Drawing.SystemColors.Control;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdGeneratePO.DisplayLayout.GroupByBox.PromptAppearance = appearance7;
            this.tagrdGeneratePO.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdGeneratePO.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdGeneratePO.DisplayLayout.Override.ActiveCellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.Gold;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.tagrdGeneratePO.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            this.tagrdGeneratePO.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdGeneratePO.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdGeneratePO.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdGeneratePO.DisplayLayout.Override.CardAreaAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance11.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdGeneratePO.DisplayLayout.Override.CellAppearance = appearance11;
            this.tagrdGeneratePO.DisplayLayout.Override.CellPadding = 0;
            appearance12.BackColor = System.Drawing.Color.LightYellow;
            this.tagrdGeneratePO.DisplayLayout.Override.DataErrorRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.ForestGreen;
            this.tagrdGeneratePO.DisplayLayout.Override.DataErrorRowSelectorAppearance = appearance13;
            this.tagrdGeneratePO.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance14.BackColor = System.Drawing.SystemColors.Control;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdGeneratePO.DisplayLayout.Override.GroupByRowAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.AliceBlue;
            appearance15.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance15.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance15.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance15.TextHAlignAsString = "Left";
            this.tagrdGeneratePO.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.tagrdGeneratePO.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdGeneratePO.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance16.ForeColor = System.Drawing.Color.Black;
            this.tagrdGeneratePO.DisplayLayout.Override.RowAppearance = appearance16;
            appearance18.BackColor = System.Drawing.Color.AliceBlue;
            appearance18.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance18.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance18.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdGeneratePO.DisplayLayout.Override.RowSelectorAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.AliceBlue;
            appearance19.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance19.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance19.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdGeneratePO.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance19;
            this.tagrdGeneratePO.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdGeneratePO.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdGeneratePO.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance20.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdGeneratePO.DisplayLayout.Override.TemplateAddRowAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.ForeColor = System.Drawing.Color.Black;
            this.tagrdGeneratePO.DisplayLayout.Override.TemplateAddRowCellAppearance = appearance21;
            this.tagrdGeneratePO.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.tagrdGeneratePO.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdGeneratePO.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdGeneratePO.DisplayLayout.UseFixedHeaders = true;
            this.tagrdGeneratePO.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdGeneratePO.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdGeneratePO.HeaderObjectKey = null;
            this.tagrdGeneratePO.Location = new System.Drawing.Point(14, 40);
            this.tagrdGeneratePO.Name = "tagrdGeneratePO";
            this.tagrdGeneratePO.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdGeneratePO.Size = new System.Drawing.Size(629, 263);
            this.tagrdGeneratePO.TabIndex = 1;
            this.tagrdGeneratePO.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdGeneratePO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.ultraLabel16);
            this.panel1.Controls.Add(this.tagrdGeneratePO);
            this.panel1.Controls.Add(this.bntOK);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 351);
            this.panel1.TabIndex = 0;
            // 
            // frmGeneratePO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(676, 376);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmGeneratePO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmGeneratePO";
            this.Text = "Generate PO";
            this.Load += new System.EventHandler(this.frmGeneratePO_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGeneratePO_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdGeneratePO)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel16;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton bntOK;
        private TAUtil.TAGridEditor tagrdGeneratePO;
        private System.Windows.Forms.Panel panel1;
    }
}