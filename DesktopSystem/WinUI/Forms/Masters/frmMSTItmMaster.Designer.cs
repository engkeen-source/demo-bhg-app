namespace WinUI
{
    partial class frmMSTItmMaster
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
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.bdsItem = new System.Windows.Forms.BindingSource(this.components);
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tslReadOnly = new System.Windows.Forms.ToolStripLabel();
            this.ultralblChooseItmType = new Infragistics.Win.Misc.UltraLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MasterItmType = new TAUtil.TAComboBox();
            this.tagrdItems = new TAUtil.TAGridEditor();
            ((System.ComponentModel.ISupportInitialize)(this.bdsItem)).BeginInit();
            this.tspBar.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MasterItmType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItems)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.tsbCancel,
            this.tslReadOnly});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(642, 57);
            this.tspBar.TabIndex = 31;
            // 
            // tsbSave
            // 
            this.tsbSave.AutoSize = false;
            this.tsbSave.BackColor = System.Drawing.Color.Transparent;
            this.tsbSave.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbSave.Image = global::WinUI.Properties.Resources.save;
            this.tsbSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.RightToLeftAutoMirrorImage = true;
            this.tsbSave.Size = new System.Drawing.Size(60, 55);
            this.tsbSave.Text = "&Save";
            this.tsbSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.AutoSize = false;
            this.tsbCancel.BackColor = System.Drawing.Color.Transparent;
            this.tsbCancel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
            this.tsbCancel.Image = global::WinUI.Properties.Resources.close;
            this.tsbCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbCancel.RightToLeftAutoMirrorImage = true;
            this.tsbCancel.Size = new System.Drawing.Size(60, 55);
            this.tsbCancel.Text = "&Cancel";
            this.tsbCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tslReadOnly
            // 
            this.tslReadOnly.AutoSize = false;
            this.tslReadOnly.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslReadOnly.ForeColor = System.Drawing.Color.Blue;
            this.tslReadOnly.Name = "tslReadOnly";
            this.tslReadOnly.Size = new System.Drawing.Size(150, 67);
            // 
            // ultralblChooseItmType
            // 
            appearance14.TextVAlignAsString = "Middle";
            this.ultralblChooseItmType.Appearance = appearance14;
            this.ultralblChooseItmType.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ultralblChooseItmType.Location = new System.Drawing.Point(15, 13);
            this.ultralblChooseItmType.Name = "ultralblChooseItmType";
            this.ultralblChooseItmType.Size = new System.Drawing.Size(116, 23);
            this.ultralblChooseItmType.TabIndex = 33;
            this.ultralblChooseItmType.Text = "Mster Item Type ";
            this.ultralblChooseItmType.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.MasterItmType);
            this.panel1.Controls.Add(this.ultralblChooseItmType);
            this.panel1.Location = new System.Drawing.Point(3, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(639, 48);
            this.panel1.TabIndex = 35;
            // 
            // MasterItmType
            // 
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BorderColor = System.Drawing.Color.LightGray;
            appearance27.ForeColor = System.Drawing.Color.Black;
            this.MasterItmType.Appearance = appearance27;
            this.MasterItmType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.MasterItmType.ComboIsDirty = false;
            appearance15.BackColor = System.Drawing.SystemColors.Window;
            appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.MasterItmType.DisplayLayout.Appearance = appearance15;
            this.MasterItmType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.MasterItmType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.MasterItmType.DisplayLayout.GroupByBox.Appearance = appearance16;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.MasterItmType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance17;
            this.MasterItmType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance18.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance18.BackColor2 = System.Drawing.SystemColors.Control;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this.MasterItmType.DisplayLayout.GroupByBox.PromptAppearance = appearance18;
            this.MasterItmType.DisplayLayout.MaxColScrollRegions = 1;
            this.MasterItmType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MasterItmType.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.SystemColors.Highlight;
            appearance20.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.MasterItmType.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            this.MasterItmType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.MasterItmType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            this.MasterItmType.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.MasterItmType.DisplayLayout.Override.CellAppearance = appearance22;
            this.MasterItmType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.MasterItmType.DisplayLayout.Override.CellPadding = 0;
            appearance23.BackColor = System.Drawing.SystemColors.Control;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this.MasterItmType.DisplayLayout.Override.GroupByRowAppearance = appearance23;
            appearance24.TextHAlignAsString = "Left";
            this.MasterItmType.DisplayLayout.Override.HeaderAppearance = appearance24;
            this.MasterItmType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.MasterItmType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.Color.Silver;
            this.MasterItmType.DisplayLayout.Override.RowAppearance = appearance25;
            this.MasterItmType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLight;
            this.MasterItmType.DisplayLayout.Override.TemplateAddRowAppearance = appearance26;
            this.MasterItmType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.MasterItmType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.MasterItmType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.MasterItmType.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.MasterItmType.Enabled = false;
            this.MasterItmType.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MasterItmType.ItemMatchingMode = Infragistics.Win.ValueListItemMatchingMode.DoNotConvertDataValueToString;
            this.MasterItmType.Location = new System.Drawing.Point(137, 13);
            this.MasterItmType.Name = "MasterItmType";
            this.MasterItmType.Size = new System.Drawing.Size(300, 26);
            this.MasterItmType.TabIndex = 34;
            this.MasterItmType.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.MasterItmType.UserInputText = "";
            this.MasterItmType.ItemNotInList += new Infragistics.Win.UltraWinGrid.ItemNotInListEventHandler(this.Combo_NotInList);
            // 
            // tagrdItems
            // 
            this.tagrdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdItems.AutoAddNewRow = false;
            this.tagrdItems.AutoUseCustomControlsInCells = false;
            this.tagrdItems.DefaultValue = null;
            this.tagrdItems.DetailObjectKey = 0;
            appearance1.AlphaLevel = ((short)(255));
            appearance1.BackColor = System.Drawing.Color.AliceBlue;
            appearance1.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance1.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance1.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdItems.DisplayLayout.Appearance = appearance1;
            this.tagrdItems.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdItems.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItems.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.tagrdItems.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdItems.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.tagrdItems.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdItems.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdItems.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tagrdItems.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.tagrdItems.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdItems.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdItems.DisplayLayout.Override.CellAppearance = appearance8;
            this.tagrdItems.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdItems.DisplayLayout.Override.CellPadding = 0;
            this.tagrdItems.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdItems.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance9.AlphaLevel = ((short)(255));
            appearance9.BackColor = System.Drawing.Color.AliceBlue;
            appearance9.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance9.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance9.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            this.tagrdItems.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.tagrdItems.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance10.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance10.ForeColor = System.Drawing.Color.Black;
            appearance10.TextVAlignAsString = "Middle";
            this.tagrdItems.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.AliceBlue;
            appearance11.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance11.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance11.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdItems.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.AliceBlue;
            appearance12.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance12.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance12.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdItems.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance12;
            this.tagrdItems.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.tagrdItems.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItems.DisplayLayout.Override.RowSelectorWidth = 30;
            this.tagrdItems.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance13.BackColor = System.Drawing.Color.Gold;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdItems.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this.tagrdItems.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItems.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItems.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdItems.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdItems.DisplayLayout.UseFixedHeaders = true;
            this.tagrdItems.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdItems.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdItems.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdItems.HeaderObjectKey = null;
            this.tagrdItems.Location = new System.Drawing.Point(0, 110);
            this.tagrdItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdItems.Name = "tagrdItems";
            this.tagrdItems.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdItems.Size = new System.Drawing.Size(642, 362);
            this.tagrdItems.TabIndex = 32;
            this.tagrdItems.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItems.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItems.CustomDataError += new TAUtil.TADataErrorEventHandler(this.tagrdItems_CustomDataError);
            // 
            // frmMSTItmMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(642, 476);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tagrdItems);
            this.Controls.Add(this.tspBar);
            this.MaximizeBox = false;
            this.Name = "frmMSTItmMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "frmMSTItmMaster";
            this.Text = "Create New Item per Cells";
            this.Load += new System.EventHandler(this.frmMSTItmMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMSTItmMaster_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bdsItem)).EndInit();
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MasterItmType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bdsItem;
        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripLabel tslReadOnly;
        private System.Windows.Forms.ToolStripButton tsbCancel;
        private TAUtil.TAGridEditor tagrdItems;
        private Infragistics.Win.Misc.UltraLabel ultralblChooseItmType;
        private TAUtil.TAComboBox MasterItmType;
        private System.Windows.Forms.Panel panel1;
    }
}