namespace WinUI
{
    partial class frmMSTAccOpenBalSelect
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
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenCompany = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenDepartment = new System.Windows.Forms.ToolStripButton();
            this.tagrdDepartmentList = new TAUtil.TAGridEditor();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDepartmentList)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbOpenCompany,
            this.tsbOpenDepartment});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(484, 70);
            this.tspBar.TabIndex = 0;
            // 
            // tsbClose
            // 
            this.tsbClose.AutoSize = false;
            this.tsbClose.BackColor = System.Drawing.Color.Transparent;
            this.tsbClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbClose.Image = global::WinUI.Properties.Resources.close;
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbClose.RightToLeftAutoMirrorImage = true;
            this.tsbClose.Size = new System.Drawing.Size(60, 55);
            this.tsbClose.Text = "&Close";
            this.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tsbOpenCompany
            // 
            this.tsbOpenCompany.AutoSize = false;
            this.tsbOpenCompany.BackColor = System.Drawing.Color.Transparent;
            this.tsbOpenCompany.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbOpenCompany.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbOpenCompany.Image = global::WinUI.Properties.Resources.open_company;
            this.tsbOpenCompany.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbOpenCompany.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenCompany.Name = "tsbOpenCompany";
            this.tsbOpenCompany.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbOpenCompany.RightToLeftAutoMirrorImage = true;
            this.tsbOpenCompany.Size = new System.Drawing.Size(95, 55);
            this.tsbOpenCompany.Text = "&Open Company";
            this.tsbOpenCompany.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbOpenCompany.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbOpenCompany.Click += new System.EventHandler(this.tsbOpenCompany_Click);
            // 
            // tsbOpenDepartment
            // 
            this.tsbOpenDepartment.AutoSize = false;
            this.tsbOpenDepartment.BackColor = System.Drawing.Color.Transparent;
            this.tsbOpenDepartment.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbOpenDepartment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbOpenDepartment.Image = global::WinUI.Properties.Resources.open_department;
            this.tsbOpenDepartment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbOpenDepartment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenDepartment.Name = "tsbOpenDepartment";
            this.tsbOpenDepartment.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbOpenDepartment.RightToLeftAutoMirrorImage = true;
            this.tsbOpenDepartment.Size = new System.Drawing.Size(105, 55);
            this.tsbOpenDepartment.Text = "&Open Department";
            this.tsbOpenDepartment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbOpenDepartment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbOpenDepartment.Click += new System.EventHandler(this.tsbOpenDepartment_Click);
            // 
            // tagrdDepartmentList
            // 
            this.tagrdDepartmentList.AutoAddNewRow = false;
            this.tagrdDepartmentList.AutoUseCustomControlsInCells = false;
            this.tagrdDepartmentList.DefaultValue = null;
            this.tagrdDepartmentList.DetailObjectKey = 0;
            appearance30.AlphaLevel = ((short)(255));
            appearance30.BackColor = System.Drawing.Color.AliceBlue;
            appearance30.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance30.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance30.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdDepartmentList.DisplayLayout.Appearance = appearance30;
            this.tagrdDepartmentList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.tagrdDepartmentList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance31.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance31.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance31.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdDepartmentList.DisplayLayout.GroupByBox.Appearance = appearance31;
            appearance32.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDepartmentList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance32;
            this.tagrdDepartmentList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance33.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance33.BackColor2 = System.Drawing.SystemColors.Control;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdDepartmentList.DisplayLayout.GroupByBox.PromptAppearance = appearance33;
            this.tagrdDepartmentList.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdDepartmentList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdDepartmentList.DisplayLayout.Override.ActiveCellAppearance = appearance1;
            appearance91.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdDepartmentList.DisplayLayout.Override.ActiveRowAppearance = appearance91;
            this.tagrdDepartmentList.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance3.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdDepartmentList.DisplayLayout.Override.CardAreaAppearance = appearance3;
            appearance5.BorderColor = System.Drawing.Color.Silver;
            appearance5.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdDepartmentList.DisplayLayout.Override.CellAppearance = appearance5;
            this.tagrdDepartmentList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.tagrdDepartmentList.DisplayLayout.Override.CellPadding = 0;
            this.tagrdDepartmentList.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdDepartmentList.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance92.AlphaLevel = ((short)(255));
            appearance92.BackColor = System.Drawing.Color.AliceBlue;
            appearance92.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance92.BorderColor = System.Drawing.Color.Gray;
            appearance92.BorderColor2 = System.Drawing.Color.Gray;
            appearance92.ForeColor = System.Drawing.Color.Black;
            appearance92.TextHAlignAsString = "Left";
            this.tagrdDepartmentList.DisplayLayout.Override.HeaderAppearance = appearance92;
            this.tagrdDepartmentList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.tagrdDepartmentList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance7.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.TextVAlignAsString = "Middle";
            this.tagrdDepartmentList.DisplayLayout.Override.RowAppearance = appearance7;
            appearance8.AlphaLevel = ((short)(255));
            appearance8.BackColor = System.Drawing.Color.AliceBlue;
            appearance8.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance8.BorderColor = System.Drawing.Color.Gray;
            appearance8.BorderColor2 = System.Drawing.Color.Gray;
            this.tagrdDepartmentList.DisplayLayout.Override.RowSelectorAppearance = appearance8;
            appearance9.AlphaLevel = ((short)(255));
            appearance9.BackColor = System.Drawing.Color.AliceBlue;
            appearance9.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            appearance9.BorderColor2 = System.Drawing.Color.Gray;
            this.tagrdDepartmentList.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance9;
            this.tagrdDepartmentList.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdDepartmentList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDepartmentList.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdDepartmentList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance10.BackColor = System.Drawing.Color.Gold;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdDepartmentList.DisplayLayout.Override.SelectedRowAppearance = appearance10;
            this.tagrdDepartmentList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDepartmentList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tagrdDepartmentList.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.tagrdDepartmentList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDepartmentList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDepartmentList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDepartmentList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdDepartmentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagrdDepartmentList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDepartmentList.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDepartmentList.HeaderObjectKey = null;
            this.tagrdDepartmentList.Location = new System.Drawing.Point(0, 70);
            this.tagrdDepartmentList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tagrdDepartmentList.Name = "tagrdDepartmentList";
            this.tagrdDepartmentList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDepartmentList.Size = new System.Drawing.Size(484, 355);
            this.tagrdDepartmentList.TabIndex = 1;
            this.tagrdDepartmentList.TabStop = false;
            this.tagrdDepartmentList.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.tagrdDepartmentList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDepartmentList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // frmMSTAccOpenBalSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 425);
            this.ControlBox = false;
            this.Controls.Add(this.tagrdDepartmentList);
            this.Controls.Add(this.tspBar);
            this.Name = "frmMSTAccOpenBalSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opening Balance Selection";
            this.Load += new System.EventHandler(this.frmMSTAccOpenBalSelect_Load);
            this.Shown += new System.EventHandler(this.frmMSTAccOpenBalSelect_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMSTAccOpenBalSelect_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMSTAccOpenBalSelect_KeyDown);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDepartmentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbOpenCompany;
        private System.Windows.Forms.ToolStripButton tsbOpenDepartment;
        private TAUtil.TAGridEditor tagrdDepartmentList;
    }
}