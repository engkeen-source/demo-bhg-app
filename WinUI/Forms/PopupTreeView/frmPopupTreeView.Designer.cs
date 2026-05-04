namespace WinUI
{
    partial class frmPopupTreeView
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
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.tspBar = new System.Windows.Forms.ToolStrip();
            this.tsbSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.tatreeTranGroup = new Infragistics.Win.UltraWinTree.UltraTree();
            this.TranGroupID = new TAUtil.TATextBoxEditor();
            this.tspBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tatreeTranGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TranGroupID)).BeginInit();
            this.SuspendLayout();
            // 
            // tspBar
            // 
            this.tspBar.AutoSize = false;
            this.tspBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSelect,
            this.toolStripSeparator1,
            this.tsbCancel});
            this.tspBar.Location = new System.Drawing.Point(0, 0);
            this.tspBar.Name = "tspBar";
            this.tspBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tspBar.Size = new System.Drawing.Size(384, 103);
            this.tspBar.TabIndex = 1;
            // 
            // tsbSelect
            // 
            this.tsbSelect.AutoSize = false;
            this.tsbSelect.BackColor = System.Drawing.Color.Transparent;
            this.tsbSelect.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbSelect.Image = global::WinUI.Properties.Resources.allow_all_32;
            this.tsbSelect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelect.Name = "tsbSelect";
            this.tsbSelect.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbSelect.RightToLeftAutoMirrorImage = true;
            this.tsbSelect.Size = new System.Drawing.Size(70, 55);
            this.tsbSelect.Text = "&Select";
            this.tsbSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSelect.Click += new System.EventHandler(this.tsbSelect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 103);
            // 
            // tsbCancel
            // 
            this.tsbCancel.AutoSize = false;
            this.tsbCancel.BackColor = System.Drawing.Color.Transparent;
            this.tsbCancel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbCancel.Image = global::WinUI.Properties.Resources.close_a_32;
            this.tsbCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbCancel.RightToLeftAutoMirrorImage = true;
            this.tsbCancel.Size = new System.Drawing.Size(70, 55);
            this.tsbCancel.Text = "&Cancel";
            this.tsbCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // ultraLabel17
            // 
            appearance106.BackColor = System.Drawing.Color.Transparent;
            appearance106.TextVAlignAsString = "Middle";
            this.ultraLabel17.Appearance = appearance106;
            this.ultraLabel17.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ultraLabel17.Location = new System.Drawing.Point(0, 120);
            this.ultraLabel17.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.Size = new System.Drawing.Size(126, 22);
            this.ultraLabel17.TabIndex = 156;
            this.ultraLabel17.Text = "Transaction Group";
            // 
            // tatreeTranGroup
            // 
            appearance7.TextTrimming = Infragistics.Win.TextTrimming.None;
            this.tatreeTranGroup.ColumnSettings.ActiveCellAppearance = appearance7;
            this.tatreeTranGroup.ColumnSettings.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Raised;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            ultraTreeColumnSet1.ActiveCellAppearance = appearance1;
            ultraTreeColumnSet1.LabelStyle = Infragistics.Win.UltraWinTree.NodeLayoutLabelStyle.Separate;
            this.tatreeTranGroup.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
            this.tatreeTranGroup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tatreeTranGroup.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tatreeTranGroup.Location = new System.Drawing.Point(16, 148);
            this.tatreeTranGroup.Name = "tatreeTranGroup";
            this.tatreeTranGroup.NodeConnectorColor = System.Drawing.SystemColors.ControlDark;
            this.tatreeTranGroup.NodeConnectorStyle = Infragistics.Win.UltraWinTree.NodeConnectorStyle.None;
            appearance2.BackColor = System.Drawing.Color.Gold;
            appearance2.BackColor2 = System.Drawing.Color.White;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            appearance2.TextTrimming = Infragistics.Win.TextTrimming.None;
            _override1.ActiveNodeAppearance = appearance2;
            _override1.BorderStyleNode = Infragistics.Win.UIElementBorderStyle.Rounded4;
            _override1.CellClickAction = Infragistics.Win.UltraWinTree.CellClickAction.SelectNodeOnly;
            _override1.DrawImageBackground = Infragistics.Win.DefaultableBoolean.True;
            _override1.HotTracking = Infragistics.Win.DefaultableBoolean.True;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            _override1.HotTrackingNodeAppearance = appearance3;
            _override1.ItemHeight = 30;
            appearance4.BorderColor3DBase = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            _override1.LabelEditAppearance = appearance4;
            appearance5.FontData.Name = "Trebuchet MS";
            appearance5.FontData.SizeInPoints = 8.5F;
            appearance5.TextTrimming = Infragistics.Win.TextTrimming.None;
            _override1.NodeAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance6.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            _override1.SelectedNodeAppearance = appearance6;
            _override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Single;
            _override1.TipStyleNode = Infragistics.Win.UltraWinTree.TipStyleNode.Show;
            this.tatreeTranGroup.Override = _override1;
            this.tatreeTranGroup.RightImagesSize = new System.Drawing.Size(30, 30);
            this.tatreeTranGroup.SettingsKey = "frmMSTAccTranGrp.tatreeTranGroup";
            this.tatreeTranGroup.Size = new System.Drawing.Size(356, 370);
            this.tatreeTranGroup.TabIndex = 50;
            this.tatreeTranGroup.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.tatreeTranGroup.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.Standard;
            this.tatreeTranGroup.Click += new System.EventHandler(this.tatreeTranGroup_Click);
            this.tatreeTranGroup.DoubleClick += new System.EventHandler(this.tatreeTranGroup_DoubleClick);
            // 
            // TranGroupID
            // 
            this.TranGroupID.Font = new System.Drawing.Font("Tahoma", 10F);
            this.TranGroupID.Format = "";
            this.TranGroupID.IsDirty = false;
            this.TranGroupID.IsEmailTextBox = false;
            this.TranGroupID.Location = new System.Drawing.Point(120, 120);
            this.TranGroupID.Multiline = true;
            this.TranGroupID.Name = "TranGroupID";
            this.TranGroupID.Size = new System.Drawing.Size(252, 22);
            this.TranGroupID.TabIndex = 0;
            this.TranGroupID.Tag = "TranGroupID";
            this.TranGroupID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TranGroupID_KeyPress);
            this.TranGroupID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TranGroupID_KeyDown);
            // 
            // frmPopupTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(384, 534);
            this.Controls.Add(this.TranGroupID);
            this.Controls.Add(this.ultraLabel17);
            this.Controls.Add(this.tspBar);
            this.Controls.Add(this.tatreeTranGroup);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPopupTreeView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmPopupTreeView";
            this.Text = "Transaction Group";
            this.Load += new System.EventHandler(this.frmPopupTreeView_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPopupTreeView_FormClosing);
            this.tspBar.ResumeLayout(false);
            this.tspBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tatreeTranGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TranGroupID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tspBar;
        private System.Windows.Forms.ToolStripButton tsbSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel17;
        private TAUtil.TATextBoxEditor TranGroupID;
        private Infragistics.Win.UltraWinTree.UltraTree tatreeTranGroup;
        private System.Windows.Forms.ToolStripButton tsbCancel;
    }
}