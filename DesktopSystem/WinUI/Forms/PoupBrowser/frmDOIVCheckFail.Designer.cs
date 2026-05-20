namespace WinUI
{
    partial class frmDOIVCheckFail
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DoDate", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DONumber", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SN", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Qty", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Markup", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Price", 7);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnIgnoreWarningAndContinue = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tagrdDOwithoutCostPrice = new TAUtil.TAGridEditor();
            this.tagrdDObelowMinMarkup = new TAUtil.TAGridEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDOwithoutCostPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDObelowMinMarkup)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.Image = global::WinUI.Properties.Resources.Cancel_16;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(3, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(142, 29);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel Transfer";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnIgnoreWarningAndContinue
            // 
            this.btnIgnoreWarningAndContinue.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnoreWarningAndContinue.ForeColor = System.Drawing.Color.Red;
            this.btnIgnoreWarningAndContinue.Location = new System.Drawing.Point(439, 7);
            this.btnIgnoreWarningAndContinue.Name = "btnIgnoreWarningAndContinue";
            this.btnIgnoreWarningAndContinue.Size = new System.Drawing.Size(258, 29);
            this.btnIgnoreWarningAndContinue.TabIndex = 1;
            this.btnIgnoreWarningAndContinue.Text = "Ignore Warning and Continue DO Transfer";
            this.btnIgnoreWarningAndContinue.UseVisualStyleBackColor = true;
            this.btnIgnoreWarningAndContinue.Click += new System.EventHandler(this.btnIgnoreWarningAndContinue_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(700, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "List of Delivery Order with Selling price but without cost price";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(700, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "List of Delivery Order below minimum markup";
            // 
            // tagrdDOwithoutCostPrice
            // 
            this.tagrdDOwithoutCostPrice.AutoAddNewRow = false;
            this.tagrdDOwithoutCostPrice.AutoUseCustomControlsInCells = false;
            this.tagrdDOwithoutCostPrice.DefaultValue = null;
            this.tagrdDOwithoutCostPrice.DetailObjectKey = 0;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 70;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 110;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 50;
            ultraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 170;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 60;
            ultraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Width = 65;
            ultraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Width = 60;
            ultraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Width = 65;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8});
            this.tagrdDOwithoutCostPrice.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.tagrdDOwithoutCostPrice.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDOwithoutCostPrice.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            this.tagrdDOwithoutCostPrice.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDOwithoutCostPrice.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDOwithoutCostPrice.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDOwithoutCostPrice.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDOwithoutCostPrice.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDOwithoutCostPrice.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDOwithoutCostPrice.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDOwithoutCostPrice.HeaderObjectKey = null;
            this.tagrdDOwithoutCostPrice.Location = new System.Drawing.Point(12, 249);
            this.tagrdDOwithoutCostPrice.Name = "tagrdDOwithoutCostPrice";
            this.tagrdDOwithoutCostPrice.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDOwithoutCostPrice.Size = new System.Drawing.Size(700, 181);
            this.tagrdDOwithoutCostPrice.TabIndex = 3;
            this.tagrdDOwithoutCostPrice.Text = "taGridEditor1";
            this.tagrdDOwithoutCostPrice.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDOwithoutCostPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // tagrdDObelowMinMarkup
            // 
            this.tagrdDObelowMinMarkup.AutoAddNewRow = false;
            this.tagrdDObelowMinMarkup.AutoUseCustomControlsInCells = false;
            this.tagrdDObelowMinMarkup.DefaultValue = null;
            this.tagrdDObelowMinMarkup.DetailObjectKey = 0;
            this.tagrdDObelowMinMarkup.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.tagrdDObelowMinMarkup.DisplayLayout.Override.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            this.tagrdDObelowMinMarkup.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDObelowMinMarkup.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.tagrdDObelowMinMarkup.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.tagrdDObelowMinMarkup.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.tagrdDObelowMinMarkup.DisplayLayout.UseFixedHeaders = true;
            this.tagrdDObelowMinMarkup.EnterKeyDirection = TAUtil.EnterKeyDirectionEnum.NextCellByTab;
            this.tagrdDObelowMinMarkup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagrdDObelowMinMarkup.HeaderObjectKey = null;
            this.tagrdDObelowMinMarkup.Location = new System.Drawing.Point(12, 33);
            this.tagrdDObelowMinMarkup.Name = "tagrdDObelowMinMarkup";
            this.tagrdDObelowMinMarkup.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.tagrdDObelowMinMarkup.Size = new System.Drawing.Size(700, 184);
            this.tagrdDObelowMinMarkup.TabIndex = 1;
            this.tagrdDObelowMinMarkup.Text = "taGridEditor1";
            this.tagrdDObelowMinMarkup.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.tagrdDObelowMinMarkup.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(234)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnIgnoreWarningAndContinue);
            this.panel1.Location = new System.Drawing.Point(12, 436);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 42);
            this.panel1.TabIndex = 4;
            // 
            // frmDOIVCheckFail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(725, 489);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tagrdDOwithoutCostPrice);
            this.Controls.Add(this.tagrdDObelowMinMarkup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDOIVCheckFail";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "frmDOIVCheckFail";
            this.Text = "Delivery Order Transfer Check";
            this.Load += new System.EventHandler(this.frmDOIVCheckFail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDOIVCheckFail_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDOwithoutCostPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagrdDObelowMinMarkup)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnIgnoreWarningAndContinue;
        private TAUtil.TAGridEditor tagrdDObelowMinMarkup;
        private TAUtil.TAGridEditor tagrdDOwithoutCostPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}