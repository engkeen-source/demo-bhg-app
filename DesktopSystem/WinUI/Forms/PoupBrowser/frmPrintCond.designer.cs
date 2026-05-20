namespace WinUI
{
    partial class frmPrintCond
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
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            this.mnuGrdPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuEditInNewTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWhereEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWhereNotEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tagrdList = new TAUtil.TAGridEditor();
            this.tslUnprintedMsg = new System.Windows.Forms.ToolStripLabel();
            this.mnuGrdPopup.SuspendLayout();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdList)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuGrdPopup
            // 
            this.mnuGrdPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditInNewTab,
            this.toolStripSeparator3,
            this.mnuFind,
            this.mnuWhereEqual,
            this.mnuWhereNotEqual,
            this.mnuRemoveFilter,
            this.mnuZoom});
            this.mnuGrdPopup.Name = "mnuGrdPopup";
            this.mnuGrdPopup.Size = new System.Drawing.Size(217, 142);
            // 
            // mnuEditInNewTab
            // 
            this.mnuEditInNewTab.Name = "mnuEditInNewTab";
            this.mnuEditInNewTab.Size = new System.Drawing.Size(216, 22);
            this.mnuEditInNewTab.Text = "Edit in &New Tab";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(213, 6);
            // 
            // mnuFind
            // 
            this.mnuFind.Name = "mnuFind";
            this.mnuFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mnuFind.Size = new System.Drawing.Size(216, 22);
            this.mnuFind.Text = "&Find";
            // 
            // mnuWhereEqual
            // 
            this.mnuWhereEqual.Name = "mnuWhereEqual";
            this.mnuWhereEqual.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.mnuWhereEqual.Size = new System.Drawing.Size(216, 22);
            this.mnuWhereEqual.Text = "Where (Equal)";
            // 
            // mnuWhereNotEqual
            // 
            this.mnuWhereNotEqual.Name = "mnuWhereNotEqual";
            this.mnuWhereNotEqual.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mnuWhereNotEqual.Size = new System.Drawing.Size(216, 22);
            this.mnuWhereNotEqual.Text = "Where (Not Equal)";
            // 
            // mnuRemoveFilter
            // 
            this.mnuRemoveFilter.Name = "mnuRemoveFilter";
            this.mnuRemoveFilter.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnuRemoveFilter.Size = new System.Drawing.Size(216, 22);
            // 
            // mnuZoom
            // 
            this.mnuZoom.Name = "mnuZoom";
            this.mnuZoom.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.Z)));
            this.mnuZoom.Size = new System.Drawing.Size(216, 22);
            this.mnuZoom.Text = "Zoom";
            // 
            // tsbCancel
            // 
            this.tsbCancel.AutoSize = false;
            this.tsbCancel.BackColor = System.Drawing.Color.Transparent;
            this.tsbCancel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.tsbCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsbCancel.Image = global::WinUI.Properties.Resources.close;
            this.tsbCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbCancel.RightToLeftAutoMirrorImage = true;
            this.tsbCancel.Size = new System.Drawing.Size(70, 55);
            this.tsbCancel.Text = "&Close";
            this.tsbCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCancel,
            this.tslUnprintedMsg});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(664, 70);
            this.tspBar.TabIndex = 0;
            // 
            // tagrdList
            // 
            this.tagrdList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdList.AutoAddNewRow = false;
            this.tagrdList.AutoUseCustomControlsInCells = true;
            this.tagrdList.DefaultValue = null;
            this.tagrdList.DetailObjectKey = 0;
            appearance15.AlphaLevel = ((short)(255));
            appearance15.BackColor = System.Drawing.Color.AliceBlue;
            appearance15.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance15.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance15.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdList.DisplayLayout.Appearance = appearance15;
            this.tagrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdList.DisplayLayout.GroupByBox.Appearance = appearance16;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdList.DisplayLayout.GroupByBox.BandLabelAppearance = appearance17;
            this.tagrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance18.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance18.BackColor2 = System.Drawing.SystemColors.Control;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdList.DisplayLayout.GroupByBox.PromptAppearance = appearance18;
            this.tagrdList.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdList.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdList.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdList.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            this.tagrdList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdList.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdList.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdList.DisplayLayout.Override.CellAppearance = appearance22;
            this.tagrdList.DisplayLayout.Override.CellPadding = 0;
            this.tagrdList.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdList.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance23.AlphaLevel = ((short)(255));
            appearance23.BackColor = System.Drawing.Color.AliceBlue;
            appearance23.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance23.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance23.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance23.ForeColor = System.Drawing.Color.Black;
            appearance23.TextHAlignAsString = "Left";
            this.tagrdList.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.tagrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance24.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance24.ForeColor = System.Drawing.Color.Black;
            appearance24.TextVAlignAsString = "Middle";
            this.tagrdList.DisplayLayout.Override.RowAppearance = appearance24;
            appearance25.AlphaLevel = ((short)(255));
            appearance25.BackColor = System.Drawing.Color.AliceBlue;
            appearance25.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance25.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance25.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdList.DisplayLayout.Override.RowSelectorAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.AliceBlue;
            appearance26.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance26.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance26.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdList.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance26;
            this.tagrdList.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdList.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance27.BackColor = System.Drawing.Color.Gold;
            appearance27.BackColor2 = System.Drawing.Color.White;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdList.DisplayLayout.Override.SelectedRowAppearance = appearance27;
            this.tagrdList.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.ForeColor = System.Drawing.Color.Black;
            this.tagrdList.DisplayLayout.Override.TemplateAddRowCellAppearance = appearance28;
            this.tagrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdList.DisplayLayout.UseFixedHeaders = true;
            this.tagrdList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdList.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdList.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdList.HeaderObjectKey = null;
            this.tagrdList.Location = new System.Drawing.Point(0, 67);
            this.tagrdList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdList.Name = "tagrdList";
            this.tagrdList.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdList.Size = new System.Drawing.Size(664, 436);
            this.tagrdList.TabIndex = 32;
            this.tagrdList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // tslUnprintedMsg
            // 
            this.tslUnprintedMsg.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslUnprintedMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tslUnprintedMsg.Name = "tslUnprintedMsg";
            this.tslUnprintedMsg.Size = new System.Drawing.Size(298, 67);
            this.tslUnprintedMsg.Text = "The following documents will not be printed.";
            // 
            // frmPrintCond
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(664, 516);
            this.Controls.Add(this.tagrdList);
            this.Controls.Add(this.tspBar);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPrintCond";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmPrintCond";
            this.Text = "Fail Print Condition";
            this.Load += new System.EventHandler(this.frmPrintCond_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPrintCond_KeyDown);
            this.mnuGrdPopup.ResumeLayout(false);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuGrdPopup;
        private System.Windows.Forms.ToolStripMenuItem mnuEditInNewTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuFind;
        private System.Windows.Forms.ToolStripMenuItem mnuWhereEqual;
        private System.Windows.Forms.ToolStripMenuItem mnuWhereNotEqual;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveFilter;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom;
        private System.Windows.Forms.ToolStripButton tsbCancel;
        private System.Windows.Forms.ToolStrip tspBar;
        private TAUtil.TAGridEditor tagrdList;
        private System.Windows.Forms.ToolStripLabel tslUnprintedMsg;      
    }
}