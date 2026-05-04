namespace BOLib
{
    partial class MsgBoxGrid
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgBoxGrid));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("BindingList`1", -1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.utxtMsgBody = new Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor();
            this.pic = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.imglstMsg = new System.Windows.Forms.ImageList(this.components);
            this.utcMsg = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.tagrdInfo = new TAUtil.TAGridEditor();
            ((System.ComponentModel.ISupportInitialize)(this.utcMsg)).BeginInit();
            this.utcMsg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(533, 7);
            // 
            // utxtMsgBody
            // 
            this.utxtMsgBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance4.FontData.SizeInPoints = 12F;
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.utxtMsgBody.Appearance = appearance4;
            this.utxtMsgBody.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.utxtMsgBody.Location = new System.Drawing.Point(58, 12);
            this.utxtMsgBody.Name = "utxtMsgBody";
            this.utxtMsgBody.ReadOnly = true;
            this.utxtMsgBody.Size = new System.Drawing.Size(821, 78);
            this.utxtMsgBody.TabIndex = 13;
            this.utxtMsgBody.TabStop = false;
            this.utxtMsgBody.Value = "aaaa <br/> bbbb";
            // 
            // pic
            // 
            this.pic.BorderShadowColor = System.Drawing.Color.Empty;
            this.pic.Image = ((object)(resources.GetObject("pic.Image")));
            this.pic.Location = new System.Drawing.Point(12, 12);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(31, 38);
            this.pic.TabIndex = 12;
            // 
            // imglstMsg
            // 
            this.imglstMsg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstMsg.ImageStream")));
            this.imglstMsg.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstMsg.Images.SetKeyName(0, "INFO.ICO");
            this.imglstMsg.Images.SetKeyName(1, "question.ico");
            this.imglstMsg.Images.SetKeyName(2, "WarningHS.png");
            this.imglstMsg.Images.SetKeyName(3, "error.ico");
            this.imglstMsg.Images.SetKeyName(4, "help.ico");
            this.imglstMsg.Images.SetKeyName(5, "Alert.png");
            this.imglstMsg.Images.SetKeyName(6, "ErrorInfo.png");
            this.imglstMsg.Images.SetKeyName(7, "Serious.png");
            // 
            // utcMsg
            // 
            this.utcMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.AliceBlue;
            appearance3.BackColor2 = System.Drawing.Color.AliceBlue;
            appearance3.BorderColor = System.Drawing.Color.AliceBlue;
            this.utcMsg.Appearance = appearance3;
            this.utcMsg.Controls.Add(this.ultraTabSharedControlsPage1);
            this.utcMsg.Controls.Add(this.ultraTabPageControl2);
            this.utcMsg.Location = new System.Drawing.Point(180, 384);
            this.utcMsg.Name = "utcMsg";
            this.utcMsg.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.utcMsg.Size = new System.Drawing.Size(537, 33);
            this.utcMsg.TabIndex = 13;
            ultraTab4.TabPage = this.ultraTabPageControl2;
            ultraTab4.Text = "Detail Info";
            this.utcMsg.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab4});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(533, 7);
            // 
            // pnlButton
            // 
            this.pnlButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButton.Location = new System.Drawing.Point(0, 384);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(937, 50);
            this.pnlButton.TabIndex = 14;
            // 
            // tagrdInfo
            // 
            this.tagrdInfo.ActiveConnection = null;
            this.tagrdInfo.AllowDrop = true;
            this.tagrdInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagrdInfo.AutoAddNewRow = false;
            this.tagrdInfo.AutoUseCustomControlsInCells = false;
            this.tagrdInfo.DefaultValue = null;
            this.tagrdInfo.DetailObjectKey = 0;
            appearance31.AlphaLevel = ((short)(255));
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance31.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance31.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance31.FontData.BoldAsString = "False";
            appearance31.FontData.Name = "Tahoma";
            appearance31.FontData.SizeInPoints = 10.5F;
            this.tagrdInfo.DisplayLayout.Appearance = appearance31;
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            appearance1.BackColor = System.Drawing.Color.LightYellow;
            ultraGridBand1.Override.DataErrorRowAppearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Green;
            ultraGridBand1.Override.DataErrorRowSelectorAppearance = appearance2;
            ultraGridBand1.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            ultraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            ultraGridBand1.Override.RowSelectorWidth = 30;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdInfo.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdInfo.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance34.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance34.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.BorderColor = System.Drawing.SystemColors.Window;
            this.tagrdInfo.DisplayLayout.GroupByBox.Appearance = appearance34;
            appearance35.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdInfo.DisplayLayout.GroupByBox.BandLabelAppearance = appearance35;
            this.tagrdInfo.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance36.BackColor2 = System.Drawing.SystemColors.Control;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance36.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tagrdInfo.DisplayLayout.GroupByBox.PromptAppearance = appearance36;
            this.tagrdInfo.DisplayLayout.MaxColScrollRegions = 1;
            this.tagrdInfo.DisplayLayout.MaxRowScrollRegions = 1;
            appearance37.ForeColor = System.Drawing.Color.Black;
            this.tagrdInfo.DisplayLayout.Override.ActiveCellAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.BlanchedAlmond;
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            appearance38.ForeColor = System.Drawing.Color.Black;
            this.tagrdInfo.DisplayLayout.Override.ActiveRowAppearance = appearance38;
            this.tagrdInfo.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.tagrdInfo.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.tagrdInfo.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdInfo.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007ScrollbarButton;
            appearance39.BackColor = System.Drawing.SystemColors.Window;
            this.tagrdInfo.DisplayLayout.Override.CardAreaAppearance = appearance39;
            appearance40.BorderColor = System.Drawing.Color.Silver;
            appearance40.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.tagrdInfo.DisplayLayout.Override.CellAppearance = appearance40;
            this.tagrdInfo.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.tagrdInfo.DisplayLayout.Override.CellPadding = 0;
            this.tagrdInfo.DisplayLayout.Override.DefaultRowHeight = 25;
            this.tagrdInfo.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            appearance41.AlphaLevel = ((short)(255));
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance41.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance41.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance41.FontData.BoldAsString = "True";
            appearance41.FontData.Name = "Tahoma";
            appearance41.ForeColor = System.Drawing.Color.Black;
            appearance41.TextHAlignAsString = "Left";
            this.tagrdInfo.DisplayLayout.Override.HeaderAppearance = appearance41;
            this.tagrdInfo.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.tagrdInfo.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance42.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            appearance42.ForeColor = System.Drawing.Color.Black;
            appearance42.TextVAlignAsString = "Middle";
            this.tagrdInfo.DisplayLayout.Override.RowAppearance = appearance42;
            appearance43.AlphaLevel = ((short)(255));
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance43.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance43.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdInfo.DisplayLayout.Override.RowSelectorAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            appearance44.BorderColor = System.Drawing.Color.LightSteelBlue;
            appearance44.BorderColor2 = System.Drawing.Color.LightSteelBlue;
            this.tagrdInfo.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance44;
            this.tagrdInfo.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
            this.tagrdInfo.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdInfo.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance45.BackColor = System.Drawing.Color.Gold;
            appearance45.BackColor2 = System.Drawing.Color.White;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            this.tagrdInfo.DisplayLayout.Override.SelectedRowAppearance = appearance45;
            this.tagrdInfo.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdInfo.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsOnly;
            this.tagrdInfo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdInfo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdInfo.DisplayLayout.UseFixedHeaders = true;
            this.tagrdInfo.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.tagrdInfo.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdInfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdInfo.HeaderObjectKey = null;
            this.tagrdInfo.Location = new System.Drawing.Point(12, 97);
            this.tagrdInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tagrdInfo.Name = "tagrdInfo";
            this.tagrdInfo.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdInfo.Size = new System.Drawing.Size(913, 278);
            this.tagrdInfo.TabIndex = 16;
            this.tagrdInfo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdInfo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdInfo.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.tagrdInfo_InitializeLayout);
            // 
            // MsgBoxGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.ClientSize = new System.Drawing.Size(937, 434);
            this.ControlBox = false;
            this.Controls.Add(this.tagrdInfo);
            this.Controls.Add(this.utxtMsgBody);
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.pic);
            this.Controls.Add(this.utcMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MsgBoxGrid";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message Title";
            this.Shown += new System.EventHandler(this.MsgBox_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MsgBoxGrid_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.utcMsg)).EndInit();
            this.utcMsg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imglstMsg;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox pic;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl utcMsg;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor utxtMsgBody;
        private System.Windows.Forms.Panel pnlButton;
        private TAUtil.TAGridEditor tagrdInfo;


        //private System.Windows.Forms.Button ubtn1;
        //private System.Windows.Forms.Button ubtn2;
        //private System.Windows.Forms.Button ubtn3;
        //private System.Windows.Forms.Button ubtn4;
        //private System.Windows.Forms.RichTextBox utxtMsgBody;



    }
}