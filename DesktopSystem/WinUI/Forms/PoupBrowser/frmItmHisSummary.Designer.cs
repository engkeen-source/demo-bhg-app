namespace WinUI.Forms.PoupBrowser
{
    partial class frmItmHisSummary
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.tagrdItmHisSummary = new TAUtil.TAGridEditor();
            this.ItmID = new TAUtil.TATextBoxEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItmHisSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItmID)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagrdItmHisSummary
            // 
            this.tagrdItmHisSummary.AutoAddNewRow = false;
            this.tagrdItmHisSummary.AutoUseCustomControlsInCells = false;
            this.tagrdItmHisSummary.DefaultValue = null;
            this.tagrdItmHisSummary.DetailObjectKey = 0;
            this.tagrdItmHisSummary.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItmHisSummary.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItmHisSummary.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdItmHisSummary.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            this.tagrdItmHisSummary.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItmHisSummary.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdItmHisSummary.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdItmHisSummary.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdItmHisSummary.DisplayLayout.UseFixedHeaders = true;
            this.tagrdItmHisSummary.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdItmHisSummary.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdItmHisSummary.HeaderObjectKey = null;
            this.tagrdItmHisSummary.Location = new System.Drawing.Point(12, 42);
            this.tagrdItmHisSummary.Name = "tagrdItmHisSummary";
            this.tagrdItmHisSummary.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdItmHisSummary.Size = new System.Drawing.Size(504, 243);
            this.tagrdItmHisSummary.TabIndex = 1;
            this.tagrdItmHisSummary.Text = "taGridEditor1";
            this.tagrdItmHisSummary.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdItmHisSummary.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ItmID
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ItmID.Appearance = appearance1;
            this.ItmID.AutoSize = false;
            this.ItmID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(76)))), ((int)(((byte)(44)))));
            this.ItmID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ItmID.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.ItmID.Format = "";
            this.ItmID.IsDirty = false;
            this.ItmID.IsEmailTextBox = false;
            this.ItmID.Location = new System.Drawing.Point(14, 4);
            this.ItmID.Name = "ItmID";
            this.ItmID.ReadOnly = true;
            this.ItmID.Size = new System.Drawing.Size(300, 25);
            this.ItmID.TabIndex = 2;
            this.ItmID.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.ItmID);
            this.panel1.Location = new System.Drawing.Point(12, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(504, 35);
            this.panel1.TabIndex = 3;
            // 
            // frmItmHisSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(527, 287);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tagrdItmHisSummary);
            this.KeyPreview = true;
            this.Name = "frmItmHisSummary";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmItmHisSummary";
            this.Text = "Item History Summary";
            this.Load += new System.EventHandler(this.frmItmHisSummary_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDocListDet_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdItmHisSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItmID)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TAUtil.TAGridEditor tagrdItmHisSummary;
        private TAUtil.TATextBoxEditor ItmID;
        private System.Windows.Forms.Panel panel1;

    }
}