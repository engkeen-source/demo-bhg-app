namespace WinUI
{
    partial class frmDocVendSendListSelect
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.label3 = new System.Windows.Forms.Label();
            this.Vendor = new TAUtil.TAComboBox();
            this.aRPYBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tagrdVendors = new TAUtil.TAGridEditor();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.Vendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aRPYBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdVendors)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            appearance1.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.Appearance = appearance1;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.Location = new System.Drawing.Point(645, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 26);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Close";
            this.btnClose.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(21, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Vendor";
            // 
            // Vendor
            // 
            appearance24.BorderColor = System.Drawing.Color.LightGray;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.Vendor.Appearance = appearance24;
            this.Vendor.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.Vendor.AutoSize = false;
            this.Vendor.ComboIsDirty = false;
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.Vendor.DisplayLayout.Appearance = appearance2;
            this.Vendor.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.Vendor.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            this.Vendor.DisplayLayout.GroupByBox.Appearance = appearance15;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Vendor.DisplayLayout.GroupByBox.BandLabelAppearance = appearance16;
            this.Vendor.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Vendor.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.Vendor.DisplayLayout.MaxColScrollRegions = 1;
            this.Vendor.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Vendor.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.Vendor.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.Vendor.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.Vendor.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.Vendor.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.Vendor.DisplayLayout.Override.CellAppearance = appearance8;
            this.Vendor.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.Vendor.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.Vendor.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.Vendor.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.Vendor.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.Vendor.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.Vendor.DisplayLayout.Override.RowAppearance = appearance11;
            this.Vendor.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Vendor.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.Vendor.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.Vendor.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.Vendor.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.Vendor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.Vendor.Font = new System.Drawing.Font("Calibri", 11F);
            this.Vendor.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.Vendor.Location = new System.Drawing.Point(75, 12);
            this.Vendor.Name = "Vendor";
            this.Vendor.Size = new System.Drawing.Size(211, 25);
            this.Vendor.TabIndex = 1;
            this.Vendor.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.Vendor.UserInputText = "\\";
            this.Vendor.CustomUpdate += new System.ComponentModel.CancelEventHandler(this.Vendor_CustomUpdate);
            // 
            // aRPYBindingSource
            // 
            this.aRPYBindingSource.DataSource = typeof(BOLib.ARPY);
            // 
            // tagrdVendors
            // 
            this.tagrdVendors.AutoAddNewRow = false;
            this.tagrdVendors.AutoUseCustomControlsInCells = false;
            this.tagrdVendors.DefaultValue = null;
            this.tagrdVendors.DetailObjectKey = 0;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.tagrdVendors.DisplayLayout.Appearance = appearance41;
            this.tagrdVendors.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.tagrdVendors.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance42.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance42.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdVendors.DisplayLayout.GroupByBox.Appearance = appearance42;
            appearance43.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdVendors.DisplayLayout.GroupByBox.BandLabelAppearance = appearance43;
            this.tagrdVendors.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance44.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance44.BackColor2 = System.Drawing.SystemColors.Control;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance44.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdVendors.DisplayLayout.GroupByBox.PromptAppearance = appearance44;
            this.tagrdVendors.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdVendors.DisplayLayout.MaxRowScrollRegions = 1;
            appearance45.BackColor = System.Drawing.SystemColors.Window;
            appearance45.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdVendors.DisplayLayout.Override.ActiveCellAppearance = appearance45;
            appearance46.BackColor = System.Drawing.SystemColors.Highlight;
            appearance46.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.tagrdVendors.DisplayLayout.Override.ActiveRowAppearance = appearance46;
            this.tagrdVendors.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdVendors.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdVendors.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdVendors.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdVendors.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdVendors.DisplayLayout.Override.CardAreaAppearance = appearance47;
            appearance48.BorderColor = System.Drawing.Color.Silver;
            appearance48.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdVendors.DisplayLayout.Override.CellAppearance = appearance48;
            this.tagrdVendors.DisplayLayout.Override.CellPadding = 0;
            this.tagrdVendors.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance49.BackColor = System.Drawing.SystemColors.Control;
            appearance49.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance49.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance49.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdVendors.DisplayLayout.Override.GroupByRowAppearance = appearance49;
            appearance50.TextHAlignAsString = "Left";
            this.tagrdVendors.DisplayLayout.Override.HeaderAppearance = appearance50;
            this.tagrdVendors.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance51.BackColor = System.Drawing.SystemColors.Window;
            appearance51.BorderColor = System.Drawing.Color.Silver;
            this.tagrdVendors.DisplayLayout.Override.RowAppearance = appearance51;
            this.tagrdVendors.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdVendors.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdVendors.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.tagrdVendors.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.tagrdVendors.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdVendors.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            appearance52.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdVendors.DisplayLayout.Override.TemplateAddRowAppearance = appearance52;
            this.tagrdVendors.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdVendors.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdVendors.DisplayLayout.UseFixedHeaders = true;
            this.tagrdVendors.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdVendors.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdVendors.HeaderObjectKey = null;
            this.tagrdVendors.Location = new System.Drawing.Point(8, 55);
            this.tagrdVendors.Name = "tagrdVendors";
            this.tagrdVendors.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdVendors.Size = new System.Drawing.Size(743, 399);
            this.tagrdVendors.TabIndex = 2;
            this.tagrdVendors.Text = "taGridEditor2";
            this.tagrdVendors.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdVendors.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnOk
            // 
            appearance13.Image = global::WinUI.Properties.Resources.ok_16;
            this.btnOk.Appearance = appearance13;
            this.btnOk.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnOk.Location = new System.Drawing.Point(533, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(106, 26);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&Ok";
            this.btnOk.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmDocVendSendListSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(763, 466);
            this.Controls.Add(this.tagrdVendors);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Vendor);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClose);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmDocVendSendListSelect";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmDocVendSendListSelect";
            this.Text = "Edit Vendor Send List";
            this.Load += new System.EventHandler(this.frmDocVendSendListSelect_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDocVendSendListSelect_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocVendSendListSelect_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Vendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aRPYBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdVendors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnClose;
        private System.Windows.Forms.BindingSource aRPYBindingSource;
        private System.Windows.Forms.Label label3;
        private TAUtil.TAComboBox Vendor;
        private TAUtil.TAGridEditor tagrdVendors;
        private Infragistics.Win.Misc.UltraButton btnOk;
    }
}