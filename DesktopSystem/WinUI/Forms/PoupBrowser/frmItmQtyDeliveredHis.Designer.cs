namespace WinUI
{
    partial class frmItmQtyDeliveredHis
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
            this.btnClose = new System.Windows.Forms.Button();
            this.tagrdDocList = new TAUtil.TAGridEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.btnClose.Image = global::WinUI.Properties.Resources.Close_16;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(521, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tagrdDocList
            // 
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
            this.tagrdDocList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.tagrdDocList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdDocList.DisplayLayout.Override.CardAreaAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.Silver;
            appearance36.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdDocList.DisplayLayout.Override.CellAppearance = appearance36;
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
            this.tagrdDocList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDocList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDocList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdDocList.DisplayLayout.Override.TemplateAddRowAppearance = appearance40;
            this.tagrdDocList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDocList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDocList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDocList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.tagrdDocList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDocList.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDocList.HeaderObjectKey = null;
            this.tagrdDocList.Location = new System.Drawing.Point(2, 2);
            this.tagrdDocList.Name = "tagrdDocList";
            this.tagrdDocList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDocList.Size = new System.Drawing.Size(609, 442);
            this.tagrdDocList.TabIndex = 43;
            this.tagrdDocList.Text = "taGridEditor1";
            this.tagrdDocList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDocList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(2, 445);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(609, 33);
            this.panel1.TabIndex = 44;
            // 
            // frmItmQtyDeliveredHis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(614, 479);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tagrdDocList);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(630, 511);
            this.Name = "frmItmQtyDeliveredHis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add By Document";
            this.Load += new System.EventHandler(this.frmItmQtyDeliveredHis_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItmQtyDeliveredHis_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDocList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private TAUtil.TAGridEditor tagrdDocList;
        private System.Windows.Forms.Panel panel1;

    }
}